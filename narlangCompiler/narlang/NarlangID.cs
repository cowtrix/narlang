using System;

namespace narlang
{
	public struct NarlangID
	{
		public string Type { get; set; }
		public string Identifier { get; set; }

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
