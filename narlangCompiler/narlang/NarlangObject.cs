using System;

namespace narlang
{
	public abstract class NarlangObject : INarlangObject
	{
		public NarlangObject(NarlangID id, FileAddress address)
		{
			Guid = Guid.NewGuid();
			Address = address;
			ID = id;
		}

		public Guid Guid { get; }

		public FileAddress Address { get; }

		public NarlangID ID { get; }

		public NarlangNode Parent { get; protected set; }

		public string Render(IRenderer rendererContext)
		{
			var result = RenderInternal(rendererContext);
			return result;
		}

		protected abstract string RenderInternal(IRenderer rendererContext);
	}
}
