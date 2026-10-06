using System;
using UnityEngine;

// Token: 0x02000632 RID: 1586
public class MapObjectHiddenCache : MapObject
{
	// Token: 0x060033A9 RID: 13225 RVA: 0x0014D7B4 File Offset: 0x0014B9B4
	public MapObjectHiddenCache(Vector3 _position)
		: base(EnumMapObjectType.HiddenCache, _position, (long)(++MapObjectHiddenCache.newID), null, false)
	{
	}

	// Token: 0x060033AA RID: 13226 RVA: 0x0014D775 File Offset: 0x0014B975
	public override string GetMapIcon()
	{
		return "ui_game_symbol_fetch_loot";
	}

	// Token: 0x060033AB RID: 13227 RVA: 0x0014D775 File Offset: 0x0014B975
	public override string GetCompassIcon()
	{
		return "ui_game_symbol_fetch_loot";
	}

	// Token: 0x060033AC RID: 13228 RVA: 0x0014D77C File Offset: 0x0014B97C
	public override string GetCompassDownIcon()
	{
		return "ui_game_symbol_fetch_loot_down";
	}

	// Token: 0x060033AD RID: 13229 RVA: 0x0014D783 File Offset: 0x0014B983
	public override string GetCompassUpIcon()
	{
		return "ui_game_symbol_fetch_loot_up";
	}

	// Token: 0x060033AE RID: 13230 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool UseUpDownCompassIcons()
	{
		return false;
	}

	// Token: 0x060033AF RID: 13231 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsOnCompass()
	{
		return true;
	}

	// Token: 0x060033B0 RID: 13232 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsCompassIconClamped()
	{
		return true;
	}

	// Token: 0x060033B1 RID: 13233 RVA: 0x00010E62 File Offset: 0x0000F062
	public override int GetLayerForMapIcon()
	{
		return 0;
	}

	// Token: 0x060033B2 RID: 13234 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool IsMapIconEnabled()
	{
		return false;
	}

	// Token: 0x060033B3 RID: 13235 RVA: 0x0014D7CF File Offset: 0x0014B9CF
	public override Color GetMapIconColor()
	{
		if (this.IsSelected)
		{
			return new Color32(byte.MaxValue, 180, 0, byte.MaxValue);
		}
		return Color.white;
	}

	// Token: 0x040028FA RID: 10490
	public bool IsSelected;

	// Token: 0x040028FB RID: 10491
	[PublicizedFrom(EAccessModifier.Private)]
	public static int newID;
}
