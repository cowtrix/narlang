using Common.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace narlang
{
	public abstract class Renderer : IRenderer
	{
		public Dictionary<NarlangID, NarlangNode> Nodes { get; }
		protected HashSet<NarlangID> m_cycleDetector = new HashSet<NarlangID>();
		protected string OutputDir { get; }

		public Renderer(string outputDir, Dictionary<NarlangID, NarlangNode> nodes)
		{
			OutputDir = outputDir;
			Nodes = nodes;
		}

		public bool Render(string format, out List<string> outPaths, out string error)
		{
			outPaths = new List<string>();
			foreach (var node in Nodes.Values)
			{
				var isEntryPoint = node.ID.Type == Const.KEYWORD_DOCUMENT;
				if (!Compiler.Debug && !isEntryPoint)
				{
					continue;
				}
				// Get the stories
				var outDir = OutputDir;
				if(!isEntryPoint)
				{
					outDir = Path.Combine(outDir, Const.SYMBOLS_DIR, node.ID.Type);
					Directory.CreateDirectory(outDir);
				}
				var outputPath = Path.GetFullPath(Path.Combine(outDir, $"{node.ID.Identifier}.{format}"));
				RenderValue(outputPath, node.Render(this), node);
				outPaths.Add(outputPath);
			}
			error = null;
			return true;
		}

		public bool TryGetTemplate(string type, out string template)
		{
			var templatePath = Path.GetFullPath(Path.Combine(Const.TEMPLATE_DIR, Extension, $"{type}.{Extension}"));
			if (File.Exists(templatePath))
			{
				template = MutateTemplate(File.ReadAllText(templatePath));
				return true;
			}
			template = null;
			return false;
		}

		public static IRenderer GetForFormat(string format, string outputPath, object nodes)
		{
			var types = ReflectionExtensions.GetConcreteClasses<IRenderer>();
			foreach (var t in types)
			{
				var r = Activator.CreateInstance(t, new[] { outputPath, nodes }) as IRenderer;
				if (r.Extension == format)
				{
					return r;
				}
			}
			throw new Exception($"Format not recognised: {format}");
		}

		public abstract string Extension { get; }

		public void RecordEntry(INarlangObject obj)
		{
			if (m_cycleDetector.Contains(obj.ID))
			{
				throw new ParseException(obj.Address, "Reference cycle detected");
			}
			m_cycleDetector.Add(obj.ID);
		}

		public void RecordExit(INarlangObject obj)
		{
			if (!m_cycleDetector.Contains(obj.ID))
			{
				throw new Exception("Bad cycle tracking");
			}
			m_cycleDetector.Remove(obj.ID);
		}

		public abstract void RenderValue(string path, string value, INarlangObject source);

		protected virtual string MutateTemplate(string template) => template;

		public virtual string MutateValue(string value, INarlangObject source) => value;

		public virtual string RenderSymbol(NarlangNode context)
		{
			return JsonConvert.SerializeObject(context.Data.ToDictionary(d => d.Key, d => d.Value.Render(this)), Formatting.Indented);
		}
	}
}
