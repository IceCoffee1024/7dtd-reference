using System;
using UnityEngine;

// Token: 0x02000626 RID: 1574
public class MapObjectFetchItem : MapObject
{
	// Token: 0x06003307 RID: 13063 RVA: 0x0014ACCE File Offset: 0x00148ECE
	public MapObjectFetchItem(Vector3 _position)
		: base(EnumMapObjectType.FetchItem, _position, (long)(++MapObjectFetchItem.newID), null, false)
	{
	}

	// Token: 0x06003308 RID: 13064 RVA: 0x0014ACE9 File Offset: 0x00148EE9
	public override string GetMapIcon()
	{
		return "ui_game_symbol_fetch_loot";
	}

	// Token: 0x06003309 RID: 13065 RVA: 0x0014ACE9 File Offset: 0x00148EE9
	public override string GetCompassIcon()
	{
		return "ui_game_symbol_fetch_loot";
	}

	// Token: 0x0600330A RID: 13066 RVA: 0x0014ACF0 File Offset: 0x00148EF0
	public override string GetCompassDownIcon()
	{
		return "ui_game_symbol_fetch_loot_down";
	}

	// Token: 0x0600330B RID: 13067 RVA: 0x0014ACF7 File Offset: 0x00148EF7
	public override string GetCompassUpIcon()
	{
		return "ui_game_symbol_fetch_loot_up";
	}

	// Token: 0x0600330C RID: 13068 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool UseUpDownCompassIcons()
	{
		return true;
	}

	// Token: 0x0600330D RID: 13069 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool IsOnCompass()
	{
		return true;
	}

	// Token: 0x0600330E RID: 13070 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool IsCompassIconClamped()
	{
		return true;
	}

	// Token: 0x0600330F RID: 13071 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool NearbyCompassBlink()
	{
		return true;
	}

	// Token: 0x06003310 RID: 13072 RVA: 0x00010E62 File Offset: 0x0000F062
	public override int GetLayerForMapIcon()
	{
		return 0;
	}

	// Token: 0x06003311 RID: 13073 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool IsMapIconEnabled()
	{
		return false;
	}

	// Token: 0x06003312 RID: 13074 RVA: 0x0014ACFE File Offset: 0x00148EFE
	public override Color GetMapIconColor()
	{
		if (this.IsSelected)
		{
			return new Color32(byte.MaxValue, 180, 0, byte.MaxValue);
		}
		return Color.white;
	}

	// Token: 0x0400288C RID: 10380
	public bool IsSelected;

	// Token: 0x0400288D RID: 10381
	[PublicizedFrom(EAccessModifier.Private)]
	public static int newID;
}
