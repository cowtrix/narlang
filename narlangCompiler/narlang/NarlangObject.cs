using System;

namespace narlang
{
	internal abstract class NarlangObject : INarlangObject
	{
		internal NarlangObject(NarlangID id, FileAddress address)
		{
			Guid = Guid.NewGuid();
			Address = address;
			ID = id;
		}

		public Guid Guid { get; }

		public FileAddress Address { get; }

		public NarlangID ID { get; }

		public NarlangNode Parent { get; set; }

		public string Render(IRenderer rendererContext)
		{
			var result = RenderInternal(rendererContext);
			return result;
		}

		protected abstract string RenderInternal(IRenderer rendererContext);
	}
}
