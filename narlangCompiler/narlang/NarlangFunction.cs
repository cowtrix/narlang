using Common;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace narlang
{
	public class NarlangFunction : NarlangObject
	{
		public List<INarlangObject> Data = new List<INarlangObject>();

		public NarlangFunction(NarlangNode parent, NarlangID id, FileAddress address, IEnumerable<INarlangObject> data) : base(id, address)
		{
			Data = data.ToList();
			Parent = parent;
		}

		protected override string RenderInternal(IRenderer rendererContext)
		{
			var sb = new StringBuilder();
			rendererContext.RecordEntry(this);
			foreach (var d in Data)
			{
				sb.AppendLine(d.Render(rendererContext));
			}
			rendererContext.RecordExit(this);
			return sb.ToString().Trim();
		}

		public override string ToString() => $"func[{ID.Identifier}]";
	}
}
