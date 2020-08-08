using Common;
using Common.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace narlang
{
	public static class Compiler
	{
		public static bool Debug { get; private set; } = true;

		static void Main(string[] args)
		{
			try
			{
				var commands = new CommandArguments(args);
				var output = commands.MustGetValue<string>("output");
				var input = commands.MustGetValue<string>("input");
				var format = commands.TryGetValue("format", "html");
				Debug = commands.TryGetValue("debug", true);

				Compile(input, output, format);
			}
			catch (Exception e)
			{
				Logger.Exception(e);
			}
		}

		public static void Compile(string input, string output, string format = "md")
		{
			var sln = new NarlangStreamReader();
			// Generate any directories needed in output
			output = Path.GetFullPath(Path.GetDirectoryName(output));
			Directory.Delete(output, true);
			Directory.CreateDirectory(output);
			foreach (var path in DiscoverFiles(input))
			{
				sln.ReadStream(path, File.ReadAllText(path));
			}
			var renderer = Renderer.GetForFormat(format, output, sln.Nodes);
			if (!renderer.Render(format, out var outputPaths, out var error))
			{
				throw new Exception(error);
			}
			Logger.Debug($"Compiled successfully:\n{string.Join("\n", outputPaths)}");
		}

		static IEnumerable<string> DiscoverFiles(string dir)
		{
			dir = Path.GetFullPath(dir);
			if (File.Exists(dir))
			{
				yield return dir;
				yield break;
			}
			if (!Directory.Exists(dir))
			{
				throw new DirectoryNotFoundException(dir);
			}
			foreach (var f in Directory.GetFiles(dir, "*.nls", SearchOption.AllDirectories))
			{
				Logger.Info($"Discovered file at {f}");
				yield return f;
			}
		}
	}
}
