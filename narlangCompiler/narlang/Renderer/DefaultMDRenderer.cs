using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace narlang
{
	public class DefaultMDRenderer : Renderer
	{
		public DefaultMDRenderer(string outputDir, Dictionary<NarlangID, NarlangNode> nodes) : base(outputDir, nodes)
		{
		}

		public override string Extension => "md";

		public override void RenderValue(string path, string val, INarlangObject source)
		{
			if(string.IsNullOrEmpty(val))
			{
				return;
			}
			if (TryGetTemplate("wrapper\\document", out var template))
			{
				template = NarlangUtility.ReplaceVariables(template, this, source as NarlangNode ?? source.Parent);
				val = template.Replace("$" + Const.RENDER_FUNCTION, val);
			}
			using var fs = new FileStream(path, FileMode.Create);
			using var sw = new StreamWriter(fs);
			sw.WriteLine(val);
		}
	}

}
