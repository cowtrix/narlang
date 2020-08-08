namespace narlang
{
	public static class Const
	{
		public const string KEYWORD_MENTIONS = "~mentions";
		public const string RENDER_FUNCTION = "render";
		public const string NAME_VARIABLE = "name";
		public const string GUID_VARIABLE = "guid";
		public const string MULTILINE_COMMENT_START = @"/*";
		public const string MULTILINE_COMMENT_END = @"*/";
		public const string SINGLE_LINE_COMMENT = @"^\s*(\/\/)(.*)";
		public const string REGEX_NEW = "^\\s*new\\s*(\\w*)\\s*\\\"(.*?)\\\"";
		public const string KEYWORD_NEW = "new";
		public const string BLOCK_START = "{";
		public const string BLOCK_END = "}";
		public const string NEWLINE = "\r\n";
		public const string FUNCTION_DECLARATION_REGEX = @"^\s*(\w+)\s*\{((?:[\n\r\s]|.)*?)\}\s*";
		public const string VARIABLE_DECLARATION_REGEX = "^\\s*(\\w{3,}):\\s*\\\"(.*)\\\"\\s*";
		public const string REFERENCE_REGEX = "~(\\w{3,})\\s*\"(.*)\"";
		public const char REFERENCE_CHAR = '~';
		public const string VARIABLE_REGEX = "\\$(\\w{3,})";

		public const string TEMPLATE_DIR = @".\templates\";
		public const string SYMBOLS_DIR = "symbols";
		public const string TYPE_VARIABLE = "type";
	}
}
