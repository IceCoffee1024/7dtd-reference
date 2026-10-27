using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

// Token: 0x02000C40 RID: 3136
public static class EnumDecoAllowedExtensions
{
	// Token: 0x06005EF7 RID: 24311 RVA: 0x00253A31 File Offset: 0x00251C31
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static EnumDecoAllowedSlope GetSlope(this EnumDecoAllowed decoAllowed)
	{
		return (EnumDecoAllowedSlope)((decoAllowed & (EnumDecoAllowed.SlopeLo | EnumDecoAllowed.SlopeHi)) / EnumDecoAllowed.SlopeLo);
	}

	// Token: 0x06005EF8 RID: 24312 RVA: 0x00253A39 File Offset: 0x00251C39
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static EnumDecoAllowed WithSlope(this EnumDecoAllowed decoAllowed, EnumDecoAllowedSlope slope)
	{
		return (EnumDecoAllowed)(((int)decoAllowed & -4) | (int)slope);
	}

	// Token: 0x06005EF9 RID: 24313 RVA: 0x00253A42 File Offset: 0x00251C42
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static EnumDecoAllowedSize GetSize(this EnumDecoAllowed decoAllowed)
	{
		return (EnumDecoAllowedSize)((decoAllowed & (EnumDecoAllowed.SizeLo | EnumDecoAllowed.SizeHi)) / EnumDecoAllowed.SizeLo);
	}

	// Token: 0x06005EFA RID: 24314 RVA: 0x00253A4B File Offset: 0x00251C4B
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static EnumDecoAllowed WithSize(this EnumDecoAllowed decoAllowed, EnumDecoAllowedSize size)
	{
		return (EnumDecoAllowed)(((int)decoAllowed & -13) | (int)(size * (EnumDecoAllowedSize)4));
	}

	// Token: 0x06005EFB RID: 24315 RVA: 0x00253A56 File Offset: 0x00251C56
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool AllowBigDeco(this EnumDecoAllowed decoAllowed)
	{
		return decoAllowed.GetSize() == EnumDecoAllowedSize.Any;
	}

	// Token: 0x06005EFC RID: 24316 RVA: 0x00253A61 File Offset: 0x00251C61
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool AllowSmallDeco(this EnumDecoAllowed decoAllowed)
	{
		return decoAllowed.GetSize() < EnumDecoAllowedSize.None;
	}

	// Token: 0x06005EFD RID: 24317 RVA: 0x00253A6C File Offset: 0x00251C6C
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool GetStreetOnly(this EnumDecoAllowed decoAllowed)
	{
		return (decoAllowed & EnumDecoAllowed.StreetOnly) == EnumDecoAllowed.StreetOnly;
	}

	// Token: 0x06005EFE RID: 24318 RVA: 0x00253A76 File Offset: 0x00251C76
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static EnumDecoAllowed WithStreetOnly(this EnumDecoAllowed decoAllowed, bool streetOnly)
	{
		if (streetOnly)
		{
			return decoAllowed | EnumDecoAllowed.StreetOnly;
		}
		return (EnumDecoAllowed)((int)decoAllowed & -17);
	}

	// Token: 0x06005EFF RID: 24319 RVA: 0x00253A86 File Offset: 0x00251C86
	public static bool IsNothing(this EnumDecoAllowed decoAllowed)
	{
		return decoAllowed.GetSlope().IsNothing() || decoAllowed.GetSize().IsNothing();
	}

	// Token: 0x06005F00 RID: 24320 RVA: 0x00253AA2 File Offset: 0x00251CA2
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsNothing(this EnumDecoAllowedSlope decoSlope)
	{
		return decoSlope >= EnumDecoAllowedSlope.Steep;
	}

	// Token: 0x06005F01 RID: 24321 RVA: 0x00253AA2 File Offset: 0x00251CA2
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsNothing(this EnumDecoAllowedSize decoSize)
	{
		return decoSize >= EnumDecoAllowedSize.None;
	}

	// Token: 0x06005F02 RID: 24322 RVA: 0x00253AAC File Offset: 0x00251CAC
	public static string ToStringFriendlyCached(this EnumDecoAllowed decoAllowed)
	{
		string text;
		if (EnumDecoAllowedExtensions.s_toStringCache.TryGetValue(decoAllowed, out text))
		{
			return text;
		}
		string text2 = EnumDecoAllowedExtensions.ToStringInternal(decoAllowed);
		EnumDecoAllowedExtensions.s_toStringCache[decoAllowed] = text2;
		return text2;
	}

	// Token: 0x06005F03 RID: 24323 RVA: 0x00253AE0 File Offset: 0x00251CE0
	[PublicizedFrom(EAccessModifier.Private)]
	public static string ToStringInternal(EnumDecoAllowed decoAllowed)
	{
		if (decoAllowed == EnumDecoAllowed.Everything)
		{
			return "Everything";
		}
		if (decoAllowed == EnumDecoAllowed.Nothing)
		{
			return "Nothing";
		}
		List<string> list = new List<string>();
		EnumDecoAllowedSlope slope = decoAllowed.GetSlope();
		if (slope > EnumDecoAllowedSlope.Flat)
		{
			list.Add(slope.ToStringCached<EnumDecoAllowedSlope>());
		}
		EnumDecoAllowedSize size = decoAllowed.GetSize();
		if (size > EnumDecoAllowedSize.Any)
		{
			list.Add(size.ToStringCached<EnumDecoAllowedSize>());
		}
		if (decoAllowed.GetStreetOnly())
		{
			list.Add("StreetOnly");
		}
		if (list.Count > 0)
		{
			return string.Join(",", list);
		}
		return "Unknown";
	}

	// Token: 0x04004A0F RID: 18959
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly EnumDictionary<EnumDecoAllowed, string> s_toStringCache = new EnumDictionary<EnumDecoAllowed, string>();
}
