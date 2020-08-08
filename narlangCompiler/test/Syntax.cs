using Microsoft.VisualStudio.TestTools.UnitTesting;
using narlang;
using System.IO;
using System.Linq;

namespace narlang_test
{
	[TestClass]
	public class Syntax : CompilerTests
	{
		/*[DataTestMethod]
		[DataRow("Nonexistant Variable", "ERROR: Nonexistant Variable.nls [line: 11, col:1]	Unable to resolve variable nonexistantVariable")]
		//[DataRow("Circular Reference", "ERROR: Circular Reference.nls [line: 1, col:0]	Reference cycle detected")]
		public void SyntaxBad(string file, string expectedError)
		{
			AssertX.Throws<ParseException>(
					() => Compiler.Compile($"{DataPath}\\{file}.nls", BuildPath),
					e => e.Message == expectedError);
		}*/

		[DataTestMethod]
		[DataRow("Simple Story", "Test", "This is a test.\r\n")]
		//[DataRow("Circular Reference", "ERROR: Circular Reference.nls [line: 1, col:0]	Reference cycle detected")]
		public void SyntaxOutput(string file, string outFileName, string outFileContent)
		{
			Compiler.Compile($"{GetInputPath()}\\{file}.nls", GetOutputPath());
			var files = Directory.GetFiles(GetOutputPath());
			Assert.IsTrue(files.Length == 1, "Unexpected file count in build: " + files.Length);
			Assert.AreEqual(Path.GetFileNameWithoutExtension(files.Single()), outFileName);
			Assert.AreEqual(outFileContent, File.ReadAllText(files.Single()));
		}
	}
}
