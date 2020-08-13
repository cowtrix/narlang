using System;

namespace narlang
{
	internal static class Const
	{
		internal const string KEYWORD_DOCUMENT = "document";
		internal const string KEYWORD_MENTIONS = "~mentions";
		internal const string RENDER_FUNCTION = "render";
		internal const string NAME_VARIABLE = "name";
		internal const string GUID_VARIABLE = "guid";
		internal const string MULTILINE_COMMENT_START = @"/*";
		internal const string MULTILINE_COMMENT_END = @"*/";
		internal const string SINGLE_LINE_COMMENT = @"^\s*(\/\/)(.*)";
		internal const string REGEX_NEW = "^\\s*new\\s*(\\w*)\\s*\\\"(.*?)\\\"";
		internal const string KEYWORD_NEW = "new";
		internal const string BLOCK_START = "{";
		internal const string BLOCK_END = "}";
		internal static string NEWLINE = Environment.NewLine;
		internal const string FUNCTION_DECLARATION_REGEX = @"^\s*(\w+)\s*\{((?:[\n\r\s]|.)*?)\}\s*";
		internal const string VARIABLE_DECLARATION_REGEX = "^\\s*(\\w{3,}):\\s*\\\"(.*)\\\"\\s*";
		internal const string REFERENCE_REGEX = "~(\\w{3,})\\s*\"(.*)\"";
		internal const char REFERENCE_CHAR = '~';
		internal const string VARIABLE_REGEX = "\\$(\\w{3,})";

		internal const string TEMPLATE_DIR = @".\templates\";
		internal const string SYMBOLS_DIR = "symbols";
		internal const string TYPE_VARIABLE = "type";
	}
}
