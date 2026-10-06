using System;
using UnityEngine;

// Token: 0x02000631 RID: 1585
public class MapObjectFetchItem : MapObject
{
	// Token: 0x0600339D RID: 13213 RVA: 0x0014D75A File Offset: 0x0014B95A
	public MapObjectFetchItem(Vector3 _position)
		: base(EnumMapObjectType.FetchItem, _position, (long)(++MapObjectFetchItem.newID), null, false)
	{
	}

	// Token: 0x0600339E RID: 13214 RVA: 0x0014D775 File Offset: 0x0014B975
	public override string GetMapIcon()
	{
		return "ui_game_symbol_fetch_loot";
	}

	// Token: 0x0600339F RID: 13215 RVA: 0x0014D775 File Offset: 0x0014B975
	public override string GetCompassIcon()
	{
		return "ui_game_symbol_fetch_loot";
	}

	// Token: 0x060033A0 RID: 13216 RVA: 0x0014D77C File Offset: 0x0014B97C
	public override string GetCompassDownIcon()
	{
		return "ui_game_symbol_fetch_loot_down";
	}

	// Token: 0x060033A1 RID: 13217 RVA: 0x0014D783 File Offset: 0x0014B983
	public override string GetCompassUpIcon()
	{
		return "ui_game_symbol_fetch_loot_up";
	}

	// Token: 0x060033A2 RID: 13218 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool UseUpDownCompassIcons()
	{
		return true;
	}

	// Token: 0x060033A3 RID: 13219 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsOnCompass()
	{
		return true;
	}

	// Token: 0x060033A4 RID: 13220 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsCompassIconClamped()
	{
		return true;
	}

	// Token: 0x060033A5 RID: 13221 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool NearbyCompassBlink()
	{
		return true;
	}

	// Token: 0x060033A6 RID: 13222 RVA: 0x00010E62 File Offset: 0x0000F062
	public override int GetLayerForMapIcon()
	{
		return 0;
	}

	// Token: 0x060033A7 RID: 13223 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool IsMapIconEnabled()
	{
		return false;
	}

	// Token: 0x060033A8 RID: 13224 RVA: 0x0014D78A File Offset: 0x0014B98A
	public override Color GetMapIconColor()
	{
		if (this.IsSelected)
		{
			return new Color32(byte.MaxValue, 180, 0, byte.MaxValue);
		}
		return Color.white;
	}

	// Token: 0x040028F8 RID: 10488
	public bool IsSelected;

	// Token: 0x040028F9 RID: 10489
	[PublicizedFrom(EAccessModifier.Private)]
	public static int newID;
}
