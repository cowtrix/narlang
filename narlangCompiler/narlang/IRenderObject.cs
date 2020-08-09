using System;
using System.Collections.Generic;

namespace narlang
{
	internal interface INarlangObject
	{
		public Guid Guid { get; }
		FileAddress Address { get; }
		NarlangID ID { get; }
		NarlangNode Parent { get; }
		string Render(IRenderer rendererContext);
	}

	internal interface IRenderer
	{
		Dictionary<NarlangID, NarlangNode> Nodes { get; }
		void RenderValue(string path, string str, INarlangObject source);
		bool TryGetTemplate(string type, out string template);
		void RecordEntry(INarlangObject obj);
		void RecordExit(INarlangObject obj);
		string Extension { get; }
		bool Render(string format, out List<string> outPaths, out string error);
		string MutateValue(string result, INarlangObject source);
		string RenderSymbol(NarlangNode context);
	}
}
