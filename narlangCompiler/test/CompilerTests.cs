using Common;
using narlang;
using System;
using System.IO;

namespace narlang_test
{
	public static class TestUtil
	{
		static TestUtil()
		{
			Logger.Debug($"Data Path: {DataPath}");
			Logger.Debug($"Build Path: {BuildPath}");
		}
		public static string DataPath
		{
			get
			{
				var env = Environment.GetEnvironmentVariable("TEST_DATA_PATH");
				if (string.IsNullOrEmpty(env))
				{
					return Path.GetFullPath("testData");
				}
				return Path.GetFullPath(Path.Combine(env, "narlangCompiler", "test", "testData"));
			}
		}
		public static string BuildPath
		{
			get
			{
				var env = Environment.GetEnvironmentVariable("TEST_OUTPUT_PATH");
				if (string.IsNullOrEmpty(env))
				{
					return Path.GetFullPath(Path.Combine(Path.GetTempPath(), "narlang", "build"));
				}
				return Path.GetFullPath(Path.Combine(env, "narlang", "build"));
			}
		}
	}

	public abstract class CompilerTests
	{
		protected string GetInputPath()
		{
			return Path.Combine(TestUtil.DataPath, GetType().Name.ToLowerInvariant());
		}

		protected string GetOutputPath()
		{
			return TestUtil.BuildPath;
		}
	}
}
