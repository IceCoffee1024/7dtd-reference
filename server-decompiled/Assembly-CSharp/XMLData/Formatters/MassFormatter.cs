using System;

namespace XMLData.Formatters
{
	// Token: 0x02001678 RID: 5752
	public class MassFormatter : ValueFormatter<float>
	{
		// Token: 0x0600B477 RID: 46199 RVA: 0x00112AAD File Offset: 0x00110CAD
		public override string FormatValue(float _value)
		{
			return _value.ToCultureInvariantString();
		}
	}
}
