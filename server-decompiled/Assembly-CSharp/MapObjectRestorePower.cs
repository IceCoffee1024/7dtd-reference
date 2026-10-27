using System;
using UnityEngine;

// Token: 0x0200063F RID: 1599
public class MapObjectRestorePower : MapObject
{
	// Token: 0x060033EB RID: 13291 RVA: 0x0014E7FD File Offset: 0x0014C9FD
	public MapObjectRestorePower(Vector3 _position)
		: base(EnumMapObjectType.RestorePower, _position, (long)(++MapObjectRestorePower.newID), null, false)
	{
	}

	// Token: 0x060033EC RID: 13292 RVA: 0x0014E369 File Offset: 0x0014C569
	public override string GetMapIcon()
	{
		return "ui_game_symbol_fetch_loot";
	}

	// Token: 0x060033ED RID: 13293 RVA: 0x0014E369 File Offset: 0x0014C569
	public override string GetCompassIcon()
	{
		return "ui_game_symbol_fetch_loot";
	}

	// Token: 0x060033EE RID: 13294 RVA: 0x0014E370 File Offset: 0x0014C570
	public override string GetCompassDownIcon()
	{
		return "ui_game_symbol_fetch_loot_down";
	}

	// Token: 0x060033EF RID: 13295 RVA: 0x0014E377 File Offset: 0x0014C577
	public override string GetCompassUpIcon()
	{
		return "ui_game_symbol_fetch_loot_up";
	}

	// Token: 0x060033F0 RID: 13296 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool UseUpDownCompassIcons()
	{
		return true;
	}

	// Token: 0x060033F1 RID: 13297 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsOnCompass()
	{
		return true;
	}

	// Token: 0x060033F2 RID: 13298 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsCompassIconClamped()
	{
		return true;
	}

	// Token: 0x060033F3 RID: 13299 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool NearbyCompassBlink()
	{
		return true;
	}

	// Token: 0x060033F4 RID: 13300 RVA: 0x00010E62 File Offset: 0x0000F062
	public override int GetLayerForMapIcon()
	{
		return 0;
	}

	// Token: 0x060033F5 RID: 13301 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool IsMapIconEnabled()
	{
		return false;
	}

	// Token: 0x060033F6 RID: 13302 RVA: 0x0014E818 File Offset: 0x0014CA18
	public override Color GetMapIconColor()
	{
		if (this.IsSelected)
		{
			return new Color32(byte.MaxValue, 180, 0, byte.MaxValue);
		}
		return Color.yellow;
	}

	// Token: 0x0400291E RID: 10526
	public bool IsSelected;

	// Token: 0x0400291F RID: 10527
	[PublicizedFrom(EAccessModifier.Private)]
	public static int newID;
}
