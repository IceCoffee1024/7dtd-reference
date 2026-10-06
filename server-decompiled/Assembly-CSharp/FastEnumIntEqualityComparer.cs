using System;
using System.Collections.Generic;

// Token: 0x0200141F RID: 5151
public class FastEnumIntEqualityComparer<TEnum> : IEqualityComparer<TEnum> where TEnum : struct, IConvertible
{
	// Token: 0x0600A179 RID: 41337 RVA: 0x003CABA6 File Offset: 0x003C8DA6
	[PublicizedFrom(EAccessModifier.Private)]
	public int ToInt(TEnum _enum)
	{
		return EnumInt32ToInt.Convert<TEnum>(_enum);
	}

	// Token: 0x0600A17A RID: 41338 RVA: 0x003CABAE File Offset: 0x003C8DAE
	public bool Equals(TEnum firstEnum, TEnum secondEnum)
	{
		return this.ToInt(firstEnum) == this.ToInt(secondEnum);
	}

	// Token: 0x0600A17B RID: 41339 RVA: 0x003CABC0 File Offset: 0x003C8DC0
	public int GetHashCode(TEnum firstEnum)
	{
		return this.ToInt(firstEnum);
	}
}
