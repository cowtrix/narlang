using System;

namespace narlang
{
	internal struct NarlangID
	{
		internal string Type { get; set; }
		internal string Identifier { get; set; }

		public override bool Equals(object obj)
		{
			return obj is NarlangID iD &&
				   Type == iD.Type &&
				   Identifier == iD.Identifier;
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(Type, Identifier);
		}

		public override string ToString()
		{
			return $"~{Type} \"{Identifier}\"";
		}
	}
}
