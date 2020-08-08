using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace narlang_test
{
	public static class AssertX
	{
		public static void Throws<T>(Action action, Func<Exception, bool> validator) where T:Exception
		{
			try
			{
				action?.Invoke();
				Assert.Fail("No exception was thrown");
			}
			catch(T e)
			{
				Assert.IsTrue(validator(e), $"Unexpected exception: {e}");
			}
		}
	}
}
