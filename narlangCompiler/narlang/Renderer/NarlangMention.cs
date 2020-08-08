namespace narlang
{
	public class NarlangMention : NarlangObject
	{
		public string Snippet { get; }
		public NarlangMention(string snippet, NarlangID id, FileAddress address) : base(id, address)
		{
			Snippet = snippet;
		}

		protected override string RenderInternal(IRenderer rendererContext)
		{
			return $"{Address}:\n{Snippet}";
		}
	}

}
