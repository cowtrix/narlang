using Common;
using Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace narlang
{
	public class RawString : NarlangObject
	{
		private string m_value;
		public Encoding Encoding;

		public RawString(NarlangNode parent, string str, NarlangID id, FileAddress address, Encoding encoding = null) : base(id, address)
		{
			Parent = parent;
			m_value = StringExtensions.FormatString(str);
			Encoding = encoding ?? Encoding.UTF8;
			Logger.Debug($"Captured raw value:\n\"{str}\"");
		}

		public override string ToString() => $"Raw: [{m_value.Substring(0, Math.Min(32, m_value.Length))}]";

		protected override string RenderInternal(IRenderer rendererContext)
		{
			// swap out any variables with their values
			rendererContext.RecordEntry(this);
			//var result = NarlangUtility.ReplaceVariables(m_value, rendererContext, Parent);
			var result = m_value;
			result = rendererContext.MutateValue(result, this);
			rendererContext.RecordExit(this);
			return result;
		}
	}
}
