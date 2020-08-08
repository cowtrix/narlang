using Common;
using Common.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace narlang
{
	public class NarlangStreamReader
	{
		public Dictionary<NarlangID, NarlangNode> Nodes { get; } = new Dictionary<NarlangID, NarlangNode>();
		private NarlangNode m_currentNode;
		public NarlangStream RootStream { get; private set; }
		public void ReadStream(string file, string data)
		{
			var nStr = new NarlangStream(this, file, data);
			FileAddress address() => new FileAddress
			{
				LineNumber = nStr.LineNumber,
				CharacterIndex = nStr.CharacterIndex,
				SourcePath = nStr.SourcePath
			};
			RootStream = nStr;
			foreach (var c in nStr)
			{
				// new node declaration
				var newNodeMatch = Regex.Match(nStr.RestOfLine, Const.REGEX_NEW, RegexOptions.Multiline);
				if (newNodeMatch.Success)
				{
					// node creation
					if (!newNodeMatch.Success)
					{
						// Might just be prose
						continue;
					}
					var type = newNodeMatch.Groups[1].Value;
					var name = newNodeMatch.Groups[2].Value;
					var id = new NarlangID { Identifier = name, Type = type };
					// Check for existing key
					if (Nodes.ContainsKey(id))
					{
						new ParseException(nStr, $"Duplicate declaration {id}. Did you mean to make a reference?");
					}
					if (m_currentNode != null)
					{
						var newNode = new NarlangNode(m_currentNode, id, address());
						m_currentNode.Children.Add(newNode);
						m_currentNode = newNode;
					}
					else
					{
						m_currentNode = new NarlangNode(id, address());
					}
					Nodes.Add(id, m_currentNode);
					nStr.Skip(newNodeMatch.Index + newNodeMatch.Length);
					continue;
				}
				else if (m_currentNode != null)
				{
					if (nStr.Peek.StartsWith(Const.BLOCK_START))
					{
						if (m_currentNode.Opened)
						{
							new ParseException(nStr, $"Unexpected character {Const.BLOCK_START}: {nStr.CurrentLine}");
						}
						m_currentNode.Opened = true;
						nStr.Skip(Const.BLOCK_START);
						continue;
					}
					else if (nStr.Peek.StartsWith(Const.BLOCK_END))
					{
						if (!m_currentNode.Opened)
						{
							new ParseException(nStr, $"Unexpected character {Const.BLOCK_END}: {nStr.CurrentLine}");
						}
						m_currentNode.Opened = false;
						m_currentNode = m_currentNode.Parent;
						nStr.Skip(Const.BLOCK_END);
						continue;
					}
					else if (!m_currentNode.Opened && !char.IsWhiteSpace(c))
					{
						throw new ParseException(nStr, $"Invalid object declaration: {nStr.CurrentLine}");
					}
					// we are within an object block. Check if it looks like a field.
					var dataMatch = Regex.Match(nStr.Peek, Const.VARIABLE_DECLARATION_REGEX);
					if (!dataMatch.Success)
					{
						dataMatch = Regex.Match(nStr.Peek, Const.FUNCTION_DECLARATION_REGEX, RegexOptions.Multiline);
					}
					if (dataMatch.Success && dataMatch.Index < nStr.RestOfLine.Length)
					{
						var name = dataMatch.Groups[1].Value;
						if (m_currentNode.Data.ContainsKey(name))
						{
							throw new ParseException(nStr, $"Duplicate object definition in {m_currentNode}: {name}");
						}
						var rawValue = dataMatch.Groups[2].Value;
						// Move the stream to the start of the function body
						var startReadIndex = dataMatch.Groups[2].Index;
						nStr.Skip(startReadIndex);
						try
						{
							var collapse = CollapseFunction(nStr, name, rawValue).ToList();
							m_currentNode.Data.Add(name, new NarlangFunction(m_currentNode, new NarlangID { Type = "~func", Identifier = $"{m_currentNode.ID.Identifier}.{name}" },
								address(), collapse));
						}
						catch(ParseException e)
						{
							throw new ParseException(e.Stream, nStr, e.Message);
						}
						Logger.Debug($"Discovered data {name} for {m_currentNode}");
						nStr.Skip(dataMatch.Length - startReadIndex);
						//nStr.Skip(Const.BLOCK_END);
						continue;
					}
				}
				else if (!char.IsWhiteSpace(c))
				{
					throw new ParseException(nStr, $"Couldn't parse: {nStr.CurrentLine}");
				}
			}
			if(m_currentNode != null)
			{
				throw new ParseException(nStr, $"Expected \"{Const.BLOCK_END}\" at end of file.");
			}
		}

		private IEnumerable<INarlangObject> CollapseFunction(NarlangStream parent, string functionName, string function)
		{
			var sb = new StringBuilder();
			var subStr = new NarlangStream(parent.Reader, parent.SourcePath, function);
			FileAddress address() => new FileAddress
			{
				LineNumber = parent.LineNumber + subStr.LineNumber,
				CharacterIndex = subStr.CharacterIndex,
				SourcePath = subStr.SourcePath
			};
			foreach (var c in subStr)
			{

				if(c == Const.REFERENCE_CHAR)
				{
					var match = Regex.Match(subStr.RestOfLine, Const.REFERENCE_REGEX);
					if (match.Success)
					{
						// Dump any strings out
						if (!string.IsNullOrWhiteSpace(sb.ToString()))
						{
							yield return new RawString(m_currentNode, sb.ToString(), new NarlangID { Identifier = functionName }, address());
							sb.Clear();
						}
						// Get var
						var type = match.Groups[1].Value;
						var name = match.Groups[2].Value;
						var id = new NarlangID { Type = type, Identifier = name };
						yield return new NarlangReference(id, address());
						subStr.Skip(match.Index + match.Length);
						continue;
					}
				}
				sb.Append(c);
			}
			// Dump any strings out
			if (!string.IsNullOrWhiteSpace(sb.ToString()))
			{
				yield return new RawString(m_currentNode, sb.ToString(), new NarlangID { Identifier = functionName }, address());
				sb.Clear();
			}
		}
	}

}
