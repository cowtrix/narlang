using Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace narlang
{
	public class NarlangStream : IEnumerable<char>
	{
		public int Index { get; private set; }
		public int LineNumber { get; private set; } = 1;
		public string SourcePath { get; }
		public string Peek { get; private set; }
		public string RestOfLine { get; private set; }
		public string CurrentLine => m_lineHistory.ToString() + RestOfLine;
		public int CharacterIndex => m_lineHistory.Length;
		public NarlangStreamReader Reader { get; }

		private string m_raw;
		private StringBuilder m_lineHistory = new StringBuilder();

		public NarlangStream(NarlangStreamReader reader, string path, string data)
		{
			Reader = reader;
			SourcePath = path;
			m_raw = data;
		}

		public IEnumerator<char> GetEnumerator()
		{
			for (Index = 0; Index < m_raw.Length; Index++)
			{
				char c = m_raw[Index];
				{
					Peek = m_raw.Substring(Index);
					var nextLineBreakIndex = Peek.IndexOf(Const.NEWLINE);
					RestOfLine = Peek;
					if (nextLineBreakIndex >= 0)
					{
						RestOfLine = Peek.Substring(0, nextLineBreakIndex);
					}
					if (Peek.StartsWith(Const.NEWLINE))
					{
						LineNumber++;
						m_lineHistory.Clear();
						Skip(Const.NEWLINE);
						yield return '\n';
						continue;
					}
				}
				yield return c;
				m_lineHistory.Append(c);
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public void Skip(Match match)
		{
			Index += match.Length - 1 + match.Index;
			LineNumber += match.Value.Count(c => c == '\n');
		}

		public void Skip(int length)
		{
			if(m_raw.Length < Index + length)
			{
				throw new ParseException(SourcePath, LineNumber, CharacterIndex, "Bad skip value");
			}
			var str = m_raw.Substring(Index, length);
			Skip(str);
		}

		public void Skip(string str)
		{
			Index += str.Length - 1;
			LineNumber += str.Count(c => c == '\n');
		}

		public void ClearLine()
		{
			m_lineHistory.Clear();
		}
	}
}
