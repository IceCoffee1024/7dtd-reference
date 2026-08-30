using System;

namespace XMLData.Formatters
{
	// Token: 0x0200163F RID: 5695
	public abstract class ValueFormatter<TValue>
	{
		// Token: 0x0600B2EA RID: 45802
		public abstract string FormatValue(TValue _value);

		// Token: 0x0600B2EB RID: 45803 RVA: 0x0000640C File Offset: 0x0000460C
		[PublicizedFrom(EAccessModifier.Protected)]
		public ValueFormatter()
		{
		}
	}
}
