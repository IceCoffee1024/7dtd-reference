using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

// Token: 0x02000C23 RID: 3107
public static class EnumDecoAllowedExtensions
{
	// Token: 0x06005EB9 RID: 24249 RVA: 0x0024F20D File Offset: 0x0024D40D
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static EnumDecoAllowedSlope GetSlope(this EnumDecoAllowed decoAllowed)
	{
		return (EnumDecoAllowedSlope)((decoAllowed & (EnumDecoAllowed.SlopeLo | EnumDecoAllowed.SlopeHi)) / EnumDecoAllowed.SlopeLo);
	}

	// Token: 0x06005EBA RID: 24250 RVA: 0x0024F215 File Offset: 0x0024D415
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static EnumDecoAllowed WithSlope(this EnumDecoAllowed decoAllowed, EnumDecoAllowedSlope slope)
	{
		return (EnumDecoAllowed)(((int)decoAllowed & -4) | (int)slope);
	}

	// Token: 0x06005EBB RID: 24251 RVA: 0x0024F21E File Offset: 0x0024D41E
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static EnumDecoAllowedSize GetSize(this EnumDecoAllowed decoAllowed)
	{
		return (EnumDecoAllowedSize)((decoAllowed & (EnumDecoAllowed.SizeLo | EnumDecoAllowed.SizeHi)) / EnumDecoAllowed.SizeLo);
	}

	// Token: 0x06005EBC RID: 24252 RVA: 0x0024F227 File Offset: 0x0024D427
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static EnumDecoAllowed WithSize(this EnumDecoAllowed decoAllowed, EnumDecoAllowedSize size)
	{
		return (EnumDecoAllowed)(((int)decoAllowed & -13) | (int)(size * (EnumDecoAllowedSize)4));
	}

	// Token: 0x06005EBD RID: 24253 RVA: 0x0024F232 File Offset: 0x0024D432
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool AllowBigDeco(this EnumDecoAllowed decoAllowed)
	{
		return decoAllowed.GetSize() == EnumDecoAllowedSize.Any;
	}

	// Token: 0x06005EBE RID: 24254 RVA: 0x0024F23D File Offset: 0x0024D43D
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool AllowSmallDeco(this EnumDecoAllowed decoAllowed)
	{
		return decoAllowed.GetSize() < EnumDecoAllowedSize.None;
	}

	// Token: 0x06005EBF RID: 24255 RVA: 0x0024F248 File Offset: 0x0024D448
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool GetStreetOnly(this EnumDecoAllowed decoAllowed)
	{
		return (decoAllowed & EnumDecoAllowed.StreetOnly) == EnumDecoAllowed.StreetOnly;
	}

	// Token: 0x06005EC0 RID: 24256 RVA: 0x0024F252 File Offset: 0x0024D452
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static EnumDecoAllowed WithStreetOnly(this EnumDecoAllowed decoAllowed, bool streetOnly)
	{
		if (streetOnly)
		{
			return decoAllowed | EnumDecoAllowed.StreetOnly;
		}
		return (EnumDecoAllowed)((int)decoAllowed & -17);
	}

	// Token: 0x06005EC1 RID: 24257 RVA: 0x0024F262 File Offset: 0x0024D462
	public static bool IsNothing(this EnumDecoAllowed decoAllowed)
	{
		return decoAllowed.GetSlope().IsNothing() || decoAllowed.GetSize().IsNothing();
	}

	// Token: 0x06005EC2 RID: 24258 RVA: 0x0024F27E File Offset: 0x0024D47E
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsNothing(this EnumDecoAllowedSlope decoSlope)
	{
		return decoSlope >= EnumDecoAllowedSlope.Steep;
	}

	// Token: 0x06005EC3 RID: 24259 RVA: 0x0024F27E File Offset: 0x0024D47E
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsNothing(this EnumDecoAllowedSize decoSize)
	{
		return decoSize >= EnumDecoAllowedSize.None;
	}

	// Token: 0x06005EC4 RID: 24260 RVA: 0x0024F288 File Offset: 0x0024D488
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

	// Token: 0x06005EC5 RID: 24261 RVA: 0x0024F2BC File Offset: 0x0024D4BC
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

	// Token: 0x040049AB RID: 18859
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly EnumDictionary<EnumDecoAllowed, string> s_toStringCache = new EnumDictionary<EnumDecoAllowed, string>();
}
