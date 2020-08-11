using Common.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace narlang
{
	internal class HtmlRenderer : DefaultMDRenderer
	{
		public HtmlRenderer(string outputPath, Dictionary<NarlangID, NarlangNode> nodes) : base(outputPath, nodes)
		{
		}

		public override string Extension => "html";

		public override void RenderValue(string path, string value, INarlangObject source)
		{			
			//value = ProcessNewlines(value);
			base.RenderValue(path, value, source);
		}

		private string GenerateReference(NarlangNode node)
		{
			var style = "";
			if(node.Data.TryGetValue("color", out var colorval))
			{
				var val = colorval.Render(this);
				if(!string.IsNullOrEmpty(val))
				{
					style = $" style=\"color:{val}\"";
				}
			}
			var url = $"./{Const.SYMBOLS_DIR}/{node.ID.Type}/{node.ID.Identifier}.{Extension}";
			return $"<button class=\"ref ref_{node.ID.Type} btn-ref\" type=\"button\" data-toggle=\"modal\" data-target=\"#refModal\" data-title=\"{node.ID.Identifier} - {node.ID.Type}\" data-iframe=\"{url}\" {style}>{node.ID.Identifier}</button>";
		}

		public override string MutateValue(string value, INarlangObject source)
		{
			value = ProcessNewlines(value);
			if (Compiler.Debug)
			{
				// Insert object references
				foreach (var node in Nodes.Values.Where(n => n.GetType() == typeof(NarlangNode) && value.Contains(n.ID.Identifier)))
				{
					var reference = GenerateReference(node);
					var nextIndex = 0;
					var lineCounter = 0;
					while (true)
					{
						nextIndex = value.IndexOf(node.ID.Identifier, nextIndex);
						if(nextIndex < 0)
						{
							break;
						}
						lineCounter += value.Substring(0, nextIndex).Count(c => c == '\n');
						const int margin = 64;
						var snippet = value.Substring(Math.Max(0, nextIndex - margin), Math.Min(margin * 2 + node.ID.Identifier.Length, value.Length - nextIndex));
						snippet = snippet.Replace("</p>", " ").Replace("<p>", "");
						snippet = System.Web.HttpUtility.HtmlEncode(snippet);
						snippet = snippet.Replace(node.ID.Identifier, $"<span style=\"color:red\">{node.ID.Identifier}</span>");
						node.Mentions.Add(new NarlangMention(snippet, node.ID, new FileAddress
						{
							SourcePath = source.Address.SourcePath,
							LineNumber = source.Address.LineNumber + lineCounter,
							CharacterIndex = 0,
						}));
						value = value.Substring(0, nextIndex) + reference + value.Substring(nextIndex + node.ID.Identifier.Length);
						nextIndex += reference.Length;
					}
				}
				
			}
			var val = Markdig.Markdown.ToHtml(value).Trim();
			if(val.StartsWith("<p>"))
			{
				val = val.Substring(3);
			}
			if(val.EndsWith("</p>"))
			{
				val = val.Substring(0, val.Length - 4);
			}
			return val;
		}

		static string ProcessNewlines(string val)
		{
			const string newlinePattern = @"\n{2,}|[\r\n]{2,}";
			if(string.IsNullOrEmpty(val))
			{
				return val;
			}
			val = val.Replace("\t", "");
			if (string.IsNullOrEmpty(val))
			{
				return null;
			}
			val = StringExtensions.ReplaceAll(val, newlinePattern, "</p><p>");
			val = val.Replace("<p></p>", "");
			return val;
		}

		protected override string MutateTemplate(string template)
		{
			template = template.Trim();
			template = template.Replace("\t", "").Replace("\n", "").Replace("\r", "");
			return template;
		}

		public override string RenderSymbol(NarlangNode context)
		{
			var sb = new StringBuilder();
			foreach(var d in context.Data)
			{
				sb.AppendLine(GetSymbolField(d.Key, d.Value));
			}
			foreach(var m in context.Mentions)
			{
				if (TryGetTemplate($"wrapper\\mention", out var template))
				{
					template = template.Replace("$snippet", m.Snippet);
					sb.AppendLine(template);
				}
			}
			if(TryGetTemplate($"wrapper\\symbol_wrapper", out var wrapperTemplate))
			{
				return wrapperTemplate.Replace("$" + Const.NAME_VARIABLE, context.ID.Identifier)
					.Replace("$" + Const.RENDER_FUNCTION, sb.ToString())
					.Replace("$" + Const.TYPE_VARIABLE, context.ID.Type);
			}
			return sb.ToString();
		}

		private string GetSymbolField(string name, INarlangObject obj)
		{
			var strData = obj.Render(this);
			if (TryGetTemplate($"wrapper\\symbol\\{obj.ID.Type}", out var template) || TryGetTemplate($"wrapper\\symbol", out template))
			{
				strData = template.Replace("$" + Const.NAME_VARIABLE, name).Replace("$" + Const.RENDER_FUNCTION, strData);
			}
			else
			{
				throw new Exception($"Can't dump symbols, no generic template found at {GetTemplatePath("wrapper\\symbol\\")}{obj.ID.Type}");
			}
			return strData;
		}
	}

}
