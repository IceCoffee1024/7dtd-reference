using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

// Token: 0x02000C35 RID: 3125
public static class EnumDecoAllowedExtensions
{
	// Token: 0x06005ED2 RID: 24274 RVA: 0x00252509 File Offset: 0x00250709
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static EnumDecoAllowedSlope GetSlope(this EnumDecoAllowed decoAllowed)
	{
		return (EnumDecoAllowedSlope)((decoAllowed & (EnumDecoAllowed.SlopeLo | EnumDecoAllowed.SlopeHi)) / EnumDecoAllowed.SlopeLo);
	}

	// Token: 0x06005ED3 RID: 24275 RVA: 0x00252511 File Offset: 0x00250711
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static EnumDecoAllowed WithSlope(this EnumDecoAllowed decoAllowed, EnumDecoAllowedSlope slope)
	{
		return (EnumDecoAllowed)(((int)decoAllowed & -4) | (int)slope);
	}

	// Token: 0x06005ED4 RID: 24276 RVA: 0x0025251A File Offset: 0x0025071A
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static EnumDecoAllowedSize GetSize(this EnumDecoAllowed decoAllowed)
	{
		return (EnumDecoAllowedSize)((decoAllowed & (EnumDecoAllowed.SizeLo | EnumDecoAllowed.SizeHi)) / EnumDecoAllowed.SizeLo);
	}

	// Token: 0x06005ED5 RID: 24277 RVA: 0x00252523 File Offset: 0x00250723
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static EnumDecoAllowed WithSize(this EnumDecoAllowed decoAllowed, EnumDecoAllowedSize size)
	{
		return (EnumDecoAllowed)(((int)decoAllowed & -13) | (int)(size * (EnumDecoAllowedSize)4));
	}

	// Token: 0x06005ED6 RID: 24278 RVA: 0x0025252E File Offset: 0x0025072E
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool AllowBigDeco(this EnumDecoAllowed decoAllowed)
	{
		return decoAllowed.GetSize() == EnumDecoAllowedSize.Any;
	}

	// Token: 0x06005ED7 RID: 24279 RVA: 0x00252539 File Offset: 0x00250739
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool AllowSmallDeco(this EnumDecoAllowed decoAllowed)
	{
		return decoAllowed.GetSize() < EnumDecoAllowedSize.None;
	}

	// Token: 0x06005ED8 RID: 24280 RVA: 0x00252544 File Offset: 0x00250744
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool GetStreetOnly(this EnumDecoAllowed decoAllowed)
	{
		return (decoAllowed & EnumDecoAllowed.StreetOnly) == EnumDecoAllowed.StreetOnly;
	}

	// Token: 0x06005ED9 RID: 24281 RVA: 0x0025254E File Offset: 0x0025074E
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static EnumDecoAllowed WithStreetOnly(this EnumDecoAllowed decoAllowed, bool streetOnly)
	{
		if (streetOnly)
		{
			return decoAllowed | EnumDecoAllowed.StreetOnly;
		}
		return (EnumDecoAllowed)((int)decoAllowed & -17);
	}

	// Token: 0x06005EDA RID: 24282 RVA: 0x0025255E File Offset: 0x0025075E
	public static bool IsNothing(this EnumDecoAllowed decoAllowed)
	{
		return decoAllowed.GetSlope().IsNothing() || decoAllowed.GetSize().IsNothing();
	}

	// Token: 0x06005EDB RID: 24283 RVA: 0x0025257A File Offset: 0x0025077A
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsNothing(this EnumDecoAllowedSlope decoSlope)
	{
		return decoSlope >= EnumDecoAllowedSlope.Steep;
	}

	// Token: 0x06005EDC RID: 24284 RVA: 0x0025257A File Offset: 0x0025077A
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsNothing(this EnumDecoAllowedSize decoSize)
	{
		return decoSize >= EnumDecoAllowedSize.None;
	}

	// Token: 0x06005EDD RID: 24285 RVA: 0x00252584 File Offset: 0x00250784
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

	// Token: 0x06005EDE RID: 24286 RVA: 0x002525B8 File Offset: 0x002507B8
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

	// Token: 0x040049EA RID: 18922
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly EnumDictionary<EnumDecoAllowed, string> s_toStringCache = new EnumDictionary<EnumDecoAllowed, string>();
}
