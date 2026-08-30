using System;
using Unity.Collections.LowLevel.Unsafe;

// Token: 0x020013E5 RID: 5093
public static class FastEnumConverter<TEnum> where TEnum : struct, IConvertible
{
	// Token: 0x06009FD5 RID: 40917 RVA: 0x003C0CE9 File Offset: 0x003BEEE9
	public static int ToInt(TEnum _enum)
	{
		return (int)((long)UnsafeUtility.EnumToInt<TEnum>(_enum) & FastEnumConverter<TEnum>.underlyingMask);
	}

	// Token: 0x04007854 RID: 30804
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly int underlyingSize = UnsafeUtility.SizeOf(Enum.GetUnderlyingType(typeof(TEnum)));

	// Token: 0x04007855 RID: 30805
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly long underlyingMask = ((FastEnumConverter<TEnum>.underlyingSize >= 8) ? (-1L) : ((1L << FastEnumConverter<TEnum>.underlyingSize * 8) - 1L));
}
