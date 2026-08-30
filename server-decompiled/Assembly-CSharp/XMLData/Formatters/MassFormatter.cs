using System;

namespace XMLData.Formatters
{
	// Token: 0x02001659 RID: 5721
	public class MassFormatter : ValueFormatter<float>
	{
		// Token: 0x0600B3E5 RID: 46053 RVA: 0x00111CDD File Offset: 0x0010FEDD
		public override string FormatValue(float _value)
		{
			return _value.ToCultureInvariantString();
		}
	}
}
