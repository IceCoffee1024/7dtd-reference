using System;
using Unity.Collections.LowLevel.Unsafe;

// Token: 0x02001432 RID: 5170
public static class FastEnumConverter<TEnum> where TEnum : struct, IConvertible
{
	// Token: 0x0600A1D5 RID: 41429 RVA: 0x003CCFCD File Offset: 0x003CB1CD
	public static int ToInt(TEnum _enum)
	{
		return (int)((long)UnsafeUtility.EnumToInt<TEnum>(_enum) & FastEnumConverter<TEnum>.underlyingMask);
	}

	// Token: 0x040079E9 RID: 31209
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly int underlyingSize = UnsafeUtility.SizeOf(Enum.GetUnderlyingType(typeof(TEnum)));

	// Token: 0x040079EA RID: 31210
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly long underlyingMask = ((FastEnumConverter<TEnum>.underlyingSize >= 8) ? (-1L) : ((1L << FastEnumConverter<TEnum>.underlyingSize * 8) - 1L));
}
