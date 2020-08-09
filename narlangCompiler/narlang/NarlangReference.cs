using Common;
using System.Collections.Generic;
using System.IO;

namespace narlang
{
	internal struct FileAddress
	{
		internal int LineNumber;
		internal int CharacterIndex;
		internal string SourcePath;

		public override string ToString() => $"{Path.GetFileName(SourcePath)} [r:{LineNumber}, c:{CharacterIndex}]";
	}

	internal class NarlangReference : NarlangObject
	{
		internal NarlangReference(NarlangID id, FileAddress address) : base(id, address)
		{
		}

		protected override string RenderInternal(IRenderer renderer)
		{
			if (!renderer.Nodes.TryGetValue(ID, out var node))
			{
				throw new ParseException(Address, $"Reference not found: {ID}");
			}
			Logger.Debug($"Following reference to {ID}");
			return node.Render(renderer);
		}

		public override string ToString()
		{
			return $"ref[{ID}]";
		}
	}
}
