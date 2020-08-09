namespace narlang
{
	internal class NarlangMention : NarlangObject
	{
		internal string Snippet { get; }
		internal NarlangMention(string snippet, NarlangID id, FileAddress address) : base(id, address)
		{
			Snippet = snippet;
		}

		protected override string RenderInternal(IRenderer rendererContext)
		{
			return $"{Address}:\n{Snippet}";
		}
	}

}
