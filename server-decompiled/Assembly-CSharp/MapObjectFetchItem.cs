using System;
using UnityEngine;

// Token: 0x02000638 RID: 1592
public class MapObjectFetchItem : MapObject
{
	// Token: 0x060033AB RID: 13227 RVA: 0x0014E34E File Offset: 0x0014C54E
	public MapObjectFetchItem(Vector3 _position)
		: base(EnumMapObjectType.FetchItem, _position, (long)(++MapObjectFetchItem.newID), null, false)
	{
	}

	// Token: 0x060033AC RID: 13228 RVA: 0x0014E369 File Offset: 0x0014C569
	public override string GetMapIcon()
	{
		return "ui_game_symbol_fetch_loot";
	}

	// Token: 0x060033AD RID: 13229 RVA: 0x0014E369 File Offset: 0x0014C569
	public override string GetCompassIcon()
	{
		return "ui_game_symbol_fetch_loot";
	}

	// Token: 0x060033AE RID: 13230 RVA: 0x0014E370 File Offset: 0x0014C570
	public override string GetCompassDownIcon()
	{
		return "ui_game_symbol_fetch_loot_down";
	}

	// Token: 0x060033AF RID: 13231 RVA: 0x0014E377 File Offset: 0x0014C577
	public override string GetCompassUpIcon()
	{
		return "ui_game_symbol_fetch_loot_up";
	}

	// Token: 0x060033B0 RID: 13232 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool UseUpDownCompassIcons()
	{
		return true;
	}

	// Token: 0x060033B1 RID: 13233 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsOnCompass()
	{
		return true;
	}

	// Token: 0x060033B2 RID: 13234 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsCompassIconClamped()
	{
		return true;
	}

	// Token: 0x060033B3 RID: 13235 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool NearbyCompassBlink()
	{
		return true;
	}

	// Token: 0x060033B4 RID: 13236 RVA: 0x00010E62 File Offset: 0x0000F062
	public override int GetLayerForMapIcon()
	{
		return 0;
	}

	// Token: 0x060033B5 RID: 13237 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool IsMapIconEnabled()
	{
		return false;
	}

	// Token: 0x060033B6 RID: 13238 RVA: 0x0014E37E File Offset: 0x0014C57E
	public override Color GetMapIconColor()
	{
		if (this.IsSelected)
		{
			return new Color32(byte.MaxValue, 180, 0, byte.MaxValue);
		}
		return Color.white;
	}

	// Token: 0x04002913 RID: 10515
	public bool IsSelected;

	// Token: 0x04002914 RID: 10516
	[PublicizedFrom(EAccessModifier.Private)]
	public static int newID;
}
