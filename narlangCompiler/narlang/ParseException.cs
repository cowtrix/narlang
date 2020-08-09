using System;
using System.IO;

namespace narlang
{
	public class ParseException : Exception
	{
		internal NarlangStream Stream { get;  }
		internal ParseException(NarlangStream stream, string error)
			: this(stream.SourcePath, stream.LineNumber, stream.CharacterIndex, error)
		{
			Stream = stream;
		}

		internal ParseException(NarlangStream stream, NarlangStream parentStream, string error)
			: this(stream.SourcePath, parentStream.LineNumber + stream.LineNumber, parentStream.CharacterIndex + stream.CharacterIndex, error)
		{
			Stream = stream;
		}

		internal ParseException(string source, int line, int character, string error)
			: base($"ERROR: {Path.GetFileName(source)} [line: {line}, col:{character}]\t{error}")
		{
		}

		internal ParseException(FileAddress fileAddress, string error) : 
			this(fileAddress.SourcePath, fileAddress.LineNumber, fileAddress.CharacterIndex, error)
		{
		}
	}
}
