﻿using narlang;
using System;
using System.IO;

namespace narlang_test
{
	public static class TestUtil
	{
		public static string DataPath
		{
			get
			{
				var env = Environment.GetEnvironmentVariable("TEST_DATA_PATH");
				if (string.IsNullOrEmpty(env))
				{
					return "./testData/";
				}
				return Path.Combine(env, "narlangCompiler", "test", "testData");
			}
		}
		public static string BuildPath => Path.GetTempPath() + "narlangtest\\build";
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
