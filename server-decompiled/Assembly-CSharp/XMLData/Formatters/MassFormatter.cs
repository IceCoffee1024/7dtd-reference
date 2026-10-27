using System;

namespace XMLData.Formatters
{
	// Token: 0x0200168A RID: 5770
	public class MassFormatter : ValueFormatter<float>
	{
		// Token: 0x0600B4CF RID: 46287 RVA: 0x00113039 File Offset: 0x00111239
		public override string FormatValue(float _value)
		{
			return _value.ToCultureInvariantString();
		}
	}
}
