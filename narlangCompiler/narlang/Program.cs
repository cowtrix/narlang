using Common.Commands;
using System;

namespace narlang
{
	public static class Program
	{
		static void Main(string[] args)
		{
			CommandManager.Execute(args);
		}
	}
}
