using Microsoft.VisualStudio.TestTools.UnitTesting;
using narlang;
using System.IO;
using System.Linq;

namespace narlang_test
{
	[TestClass]
	public class Syntax : CompilerTests
	{
		[DataTestMethod]
		[DataRow("Nonexistant Reference", "ERROR: Nonexistant Reference.nls [line: 9, col:2]	Reference not found: ~object \"I don't exist\"")]
		[DataRow("Simple Document - Missing End Bracket", "ERROR: Simple Document - Missing End Bracket.nls [line: 9, col:0]	Expected \"}\" at end of file.")]
		public void SyntaxException(string file, string expectedError)
		{
			AssertX.Throws<ParseException>(
					() => Compiler.Compile($"{GetInputPath()}\\{file}.nls", GetOutputPath()),
					e => e.Message == expectedError);
		}

		[DataTestMethod]
		/*[DataRow("Simple Document", "Test", "This is a test.\r\n")]
		[DataRow("Reference", "Test", "This is a test.\r\n")]
		[DataRow("Two References", "Test", "This is the first object.\r\nThis is the second object.\r\n")]*/
		[DataRow("Comments", "Test", "This is a test.\r\n")]
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
