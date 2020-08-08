using Microsoft.VisualStudio.TestTools.UnitTesting;
using narlang;

namespace narlang_test
{
	[TestClass]
	public class Syntax : CompilerTests
	{
		[DataTestMethod]
		[DataRow("Nonexistant Variable", "ERROR: Nonexistant Variable.nls [line: 11, col:1]	Unable to resolve variable nonexistantVariable")]
		//[DataRow("Circular Reference", "ERROR: Circular Reference.nls [line: 1, col:0]	Reference cycle detected")]
		public void SyntaxBad(string file, string expectedError)
		{
			AssertX.Throws<ParseException>(
					() => Compiler.Compile($"{DataPath}\\{file}.nls", BuildPath),
					e => e.Message == expectedError);
		}
	}
}
