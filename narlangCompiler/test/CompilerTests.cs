using narlang;
using System.IO;

namespace narlang_test
{
	public abstract class CompilerTests
	{
		protected virtual string DataFolder => GetType().Name.ToLowerInvariant();
		protected string DataPath => Path.GetFullPath(@".\testData\" + DataFolder);
		protected string BuildPath => "test\\build";
	}
}
