using System;

namespace XMLData.Formatters
{
	// Token: 0x0200165A RID: 5722
	public abstract class ValueFormatter<TValue>
	{
		// Token: 0x0600B3E7 RID: 46055
		public abstract string FormatValue(TValue _value);

		// Token: 0x0600B3E8 RID: 46056 RVA: 0x0000640C File Offset: 0x0000460C
		[PublicizedFrom(EAccessModifier.Protected)]
		public ValueFormatter()
		{
		}
	}
}
