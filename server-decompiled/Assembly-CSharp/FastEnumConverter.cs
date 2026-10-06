using System;
using Unity.Collections.LowLevel.Unsafe;

// Token: 0x02001420 RID: 5152
public static class FastEnumConverter<TEnum> where TEnum : struct, IConvertible
{
	// Token: 0x0600A17D RID: 41341 RVA: 0x003CABC9 File Offset: 0x003C8DC9
	public static int ToInt(TEnum _enum)
	{
		return (int)((long)UnsafeUtility.EnumToInt<TEnum>(_enum) & FastEnumConverter<TEnum>.underlyingMask);
	}

	// Token: 0x040079A8 RID: 31144
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly int underlyingSize = UnsafeUtility.SizeOf(Enum.GetUnderlyingType(typeof(TEnum)));

	// Token: 0x040079A9 RID: 31145
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly long underlyingMask = ((FastEnumConverter<TEnum>.underlyingSize >= 8) ? (-1L) : ((1L << FastEnumConverter<TEnum>.underlyingSize * 8) - 1L));
}
