using System;

namespace XMLData.Formatters
{
	// Token: 0x0200163E RID: 5694
	public class MassFormatter : ValueFormatter<float>
	{
		// Token: 0x0600B2E8 RID: 45800 RVA: 0x00110C65 File Offset: 0x0010EE65
		public override string FormatValue(float _value)
		{
			return _value.ToCultureInvariantString();
		}
	}
}
