using System;
using System.Collections.Generic;

// Token: 0x020013E4 RID: 5092
public class FastEnumIntEqualityComparer<TEnum> : IEqualityComparer<TEnum> where TEnum : struct, IConvertible
{
	// Token: 0x06009FD1 RID: 40913 RVA: 0x003C0CC6 File Offset: 0x003BEEC6
	[PublicizedFrom(EAccessModifier.Private)]
	public int ToInt(TEnum _enum)
	{
		return EnumInt32ToInt.Convert<TEnum>(_enum);
	}

	// Token: 0x06009FD2 RID: 40914 RVA: 0x003C0CCE File Offset: 0x003BEECE
	public bool Equals(TEnum firstEnum, TEnum secondEnum)
	{
		return this.ToInt(firstEnum) == this.ToInt(secondEnum);
	}

	// Token: 0x06009FD3 RID: 40915 RVA: 0x003C0CE0 File Offset: 0x003BEEE0
	public int GetHashCode(TEnum firstEnum)
	{
		return this.ToInt(firstEnum);
	}
}
