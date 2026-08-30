using System;
using Unity.Collections.LowLevel.Unsafe;

// Token: 0x02001400 RID: 5120
public static class FastEnumConverter<TEnum> where TEnum : struct, IConvertible
{
	// Token: 0x0600A0D2 RID: 41170 RVA: 0x003C602D File Offset: 0x003C422D
	public static int ToInt(TEnum _enum)
	{
		return (int)((long)UnsafeUtility.EnumToInt<TEnum>(_enum) & FastEnumConverter<TEnum>.underlyingMask);
	}

	// Token: 0x0400792D RID: 31021
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly int underlyingSize = UnsafeUtility.SizeOf(Enum.GetUnderlyingType(typeof(TEnum)));

	// Token: 0x0400792E RID: 31022
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly long underlyingMask = ((FastEnumConverter<TEnum>.underlyingSize >= 8) ? (-1L) : ((1L << FastEnumConverter<TEnum>.underlyingSize * 8) - 1L));
}
