using System;

namespace XMLData.Formatters
{
	// Token: 0x02001679 RID: 5753
	public abstract class ValueFormatter<TValue>
	{
		// Token: 0x0600B479 RID: 46201
		public abstract string FormatValue(TValue _value);

		// Token: 0x0600B47A RID: 46202 RVA: 0x0000640C File Offset: 0x0000460C
		[PublicizedFrom(EAccessModifier.Protected)]
		public ValueFormatter()
		{
		}
	}
}
