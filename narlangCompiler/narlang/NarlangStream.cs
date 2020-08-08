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
		private enum eState
		{
			read,
			blockcomment,
		}
		private eState m_state;
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
			for (Index = 0; Index < m_raw.Length;)
			{
				Invalidate();
				var indexStore = Index;
				if (Peek.StartsWith(Const.NEWLINE))
				{
					LineNumber++;
					ClearLine();
					Skip(Const.NEWLINE);
					yield return '\n';
					continue;
				}
				if (ConsumeComments())
				{
					continue;
				}
				char c = m_raw[Index];
				yield return c;
				// If we haven't skipped, move now
				if(Index == indexStore)
				{
					Skip(c);
				}
			}
		}

		void Invalidate()
		{
			Peek = m_raw.Substring(Index);
			var nextLineBreakIndex = Peek.IndexOf(Const.NEWLINE);
			RestOfLine = Peek;
			if (nextLineBreakIndex >= 0)
			{
				RestOfLine = Peek.Substring(0, nextLineBreakIndex);
			}
		}

		bool ConsumeComments()
		{
			// Single line comment - ignore
			if (Regex.IsMatch(RestOfLine, Const.SINGLE_LINE_COMMENT))
			{
				Skip(RestOfLine);
				return true;
			}
			// Multi line comment - ignore
			if (m_state == eState.blockcomment)
			{
				if (Peek.StartsWith(Const.MULTILINE_COMMENT_END))
				{
					m_state = eState.read;
					Skip(Const.MULTILINE_COMMENT_END);
					return true;
				}
				Skip(m_raw[Index]);
				return true;
			}
			if (Peek.StartsWith(Const.MULTILINE_COMMENT_START))
			{
				m_state = eState.blockcomment;
				Skip(Const.MULTILINE_COMMENT_START);
				return true;
			}
			else if (Peek.StartsWith(Const.MULTILINE_COMMENT_END))
			{
				throw new ParseException(this, $"Unexpected multiline comment end: {CurrentLine}");
			}
			return false;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public void Skip(int length)
		{
			if (m_raw.Length < Index + length)
			{
				throw new ParseException(SourcePath, LineNumber, CharacterIndex, "Bad skip value");
			}
			var str = m_raw.Substring(Index, length);
			Skip(str);
		}

		public void Skip(string str)
		{
			m_lineHistory.Append(str);
			Index += str.Length;
			if(str.Contains('\n'))
			{
				ClearLine();
				LineNumber += str.Count(c => c == '\n');
			}
			Invalidate();
		}

		private void Skip(char c)
		{
			m_lineHistory.Append(c);
			Index++;
			Invalidate();
		}

		public void ClearLine()
		{
			m_lineHistory.Clear();
		}
	}
}
