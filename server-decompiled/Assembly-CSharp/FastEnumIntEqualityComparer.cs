using System;
using System.Collections.Generic;

// Token: 0x02001431 RID: 5169
public class FastEnumIntEqualityComparer<TEnum> : IEqualityComparer<TEnum> where TEnum : struct, IConvertible
{
	// Token: 0x0600A1D1 RID: 41425 RVA: 0x003CCFAA File Offset: 0x003CB1AA
	[PublicizedFrom(EAccessModifier.Private)]
	public int ToInt(TEnum _enum)
	{
		return EnumInt32ToInt.Convert<TEnum>(_enum);
	}

	// Token: 0x0600A1D2 RID: 41426 RVA: 0x003CCFB2 File Offset: 0x003CB1B2
	public bool Equals(TEnum firstEnum, TEnum secondEnum)
	{
		return this.ToInt(firstEnum) == this.ToInt(secondEnum);
	}

	// Token: 0x0600A1D3 RID: 41427 RVA: 0x003CCFC4 File Offset: 0x003CB1C4
	public int GetHashCode(TEnum firstEnum)
	{
		return this.ToInt(firstEnum);
	}
}
