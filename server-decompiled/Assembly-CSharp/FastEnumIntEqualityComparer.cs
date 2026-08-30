using System;
using System.Collections.Generic;

// Token: 0x020013FF RID: 5119
public class FastEnumIntEqualityComparer<TEnum> : IEqualityComparer<TEnum> where TEnum : struct, IConvertible
{
	// Token: 0x0600A0CE RID: 41166 RVA: 0x003C600A File Offset: 0x003C420A
	[PublicizedFrom(EAccessModifier.Private)]
	public int ToInt(TEnum _enum)
	{
		return EnumInt32ToInt.Convert<TEnum>(_enum);
	}

	// Token: 0x0600A0CF RID: 41167 RVA: 0x003C6012 File Offset: 0x003C4212
	public bool Equals(TEnum firstEnum, TEnum secondEnum)
	{
		return this.ToInt(firstEnum) == this.ToInt(secondEnum);
	}

	// Token: 0x0600A0D0 RID: 41168 RVA: 0x003C6024 File Offset: 0x003C4224
	public int GetHashCode(TEnum firstEnum)
	{
		return this.ToInt(firstEnum);
	}
}
