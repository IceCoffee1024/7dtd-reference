using System;

namespace XMLData.Formatters
{
	// Token: 0x0200168B RID: 5771
	public abstract class ValueFormatter<TValue>
	{
		// Token: 0x0600B4D1 RID: 46289
		public abstract string FormatValue(TValue _value);

		// Token: 0x0600B4D2 RID: 46290 RVA: 0x0000640C File Offset: 0x0000460C
		[PublicizedFrom(EAccessModifier.Protected)]
		public ValueFormatter()
		{
		}
	}
}
