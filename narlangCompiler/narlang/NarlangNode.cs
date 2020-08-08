using Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace narlang
{
	public class NarlangNode : NarlangObject
	{
		public bool Opened { get; set; }
		public NarlangNode Parent { get; set; }
		public List<NarlangNode> Children { get; } = new List<NarlangNode>();
		public List<NarlangMention> Mentions { get; } = new List<NarlangMention>();
		public Dictionary<string, INarlangObject> Data { get; } = new Dictionary<string, INarlangObject>();
		protected NarlangFunction RenderFunction => Data.SingleOrDefault(kvp => kvp.Key == Const.RENDER_FUNCTION).Value as NarlangFunction;

		public NarlangNode(NarlangID id, FileAddress address) : base(id, address)
		{
			if(Parent == null)
				Logger.Debug($"New Node: {id}");
		}

		public NarlangNode(NarlangNode parentNode, NarlangID id, FileAddress address) : this(id, address)
		{
			Parent = parentNode;
			Logger.Debug($"New Node: {id} {Parent}");
		}

		protected override string RenderInternal(IRenderer renderer)
		{
			if(RenderFunction == null)
			{
				return renderer.RenderSymbol(this);
			}
			var result = RenderFunction.Render(renderer);
			if (renderer.TryGetTemplate(ID.Type, out var template))
			{
				template = NarlangUtility.ReplaceVariables(template, renderer, this);
				return template;
			}
			return result;
		}

		public override string ToString()
		{
			return $"[{ID}]";
		}
	}
}
