using System;
using UnityEngine;

// Token: 0x0200062F RID: 1583
public class MapObjectFetchItem : MapObject
{
	// Token: 0x0600335A RID: 13146 RVA: 0x0014CDEA File Offset: 0x0014AFEA
	public MapObjectFetchItem(Vector3 _position)
		: base(EnumMapObjectType.FetchItem, _position, (long)(++MapObjectFetchItem.newID), null, false)
	{
	}

	// Token: 0x0600335B RID: 13147 RVA: 0x0014CE05 File Offset: 0x0014B005
	public override string GetMapIcon()
	{
		return "ui_game_symbol_fetch_loot";
	}

	// Token: 0x0600335C RID: 13148 RVA: 0x0014CE05 File Offset: 0x0014B005
	public override string GetCompassIcon()
	{
		return "ui_game_symbol_fetch_loot";
	}

	// Token: 0x0600335D RID: 13149 RVA: 0x0014CE0C File Offset: 0x0014B00C
	public override string GetCompassDownIcon()
	{
		return "ui_game_symbol_fetch_loot_down";
	}

	// Token: 0x0600335E RID: 13150 RVA: 0x0014CE13 File Offset: 0x0014B013
	public override string GetCompassUpIcon()
	{
		return "ui_game_symbol_fetch_loot_up";
	}

	// Token: 0x0600335F RID: 13151 RVA: 0x00020021 File Offset: 0x0001E221
	public override bool UseUpDownCompassIcons()
	{
		return true;
	}

	// Token: 0x06003360 RID: 13152 RVA: 0x00020021 File Offset: 0x0001E221
	public override bool IsOnCompass()
	{
		return true;
	}

	// Token: 0x06003361 RID: 13153 RVA: 0x00020021 File Offset: 0x0001E221
	public override bool IsCompassIconClamped()
	{
		return true;
	}

	// Token: 0x06003362 RID: 13154 RVA: 0x00020021 File Offset: 0x0001E221
	public override bool NearbyCompassBlink()
	{
		return true;
	}

	// Token: 0x06003363 RID: 13155 RVA: 0x00010E62 File Offset: 0x0000F062
	public override int GetLayerForMapIcon()
	{
		return 0;
	}

	// Token: 0x06003364 RID: 13156 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool IsMapIconEnabled()
	{
		return false;
	}

	// Token: 0x06003365 RID: 13157 RVA: 0x0014CE1A File Offset: 0x0014B01A
	public override Color GetMapIconColor()
	{
		if (this.IsSelected)
		{
			return new Color32(byte.MaxValue, 180, 0, byte.MaxValue);
		}
		return Color.white;
	}

	// Token: 0x040028FF RID: 10495
	public bool IsSelected;

	// Token: 0x04002900 RID: 10496
	[PublicizedFrom(EAccessModifier.Private)]
	public static int newID;
}
