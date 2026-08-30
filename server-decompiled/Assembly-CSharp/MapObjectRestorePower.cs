using System;
using UnityEngine;

// Token: 0x02000636 RID: 1590
public class MapObjectRestorePower : MapObject
{
	// Token: 0x0600339A RID: 13210 RVA: 0x0014D299 File Offset: 0x0014B499
	public MapObjectRestorePower(Vector3 _position)
		: base(EnumMapObjectType.RestorePower, _position, (long)(++MapObjectRestorePower.newID), null, false)
	{
	}

	// Token: 0x0600339B RID: 13211 RVA: 0x0014CE05 File Offset: 0x0014B005
	public override string GetMapIcon()
	{
		return "ui_game_symbol_fetch_loot";
	}

	// Token: 0x0600339C RID: 13212 RVA: 0x0014CE05 File Offset: 0x0014B005
	public override string GetCompassIcon()
	{
		return "ui_game_symbol_fetch_loot";
	}

	// Token: 0x0600339D RID: 13213 RVA: 0x0014CE0C File Offset: 0x0014B00C
	public override string GetCompassDownIcon()
	{
		return "ui_game_symbol_fetch_loot_down";
	}

	// Token: 0x0600339E RID: 13214 RVA: 0x0014CE13 File Offset: 0x0014B013
	public override string GetCompassUpIcon()
	{
		return "ui_game_symbol_fetch_loot_up";
	}

	// Token: 0x0600339F RID: 13215 RVA: 0x00020021 File Offset: 0x0001E221
	public override bool UseUpDownCompassIcons()
	{
		return true;
	}

	// Token: 0x060033A0 RID: 13216 RVA: 0x00020021 File Offset: 0x0001E221
	public override bool IsOnCompass()
	{
		return true;
	}

	// Token: 0x060033A1 RID: 13217 RVA: 0x00020021 File Offset: 0x0001E221
	public override bool IsCompassIconClamped()
	{
		return true;
	}

	// Token: 0x060033A2 RID: 13218 RVA: 0x00020021 File Offset: 0x0001E221
	public override bool NearbyCompassBlink()
	{
		return true;
	}

	// Token: 0x060033A3 RID: 13219 RVA: 0x00010E62 File Offset: 0x0000F062
	public override int GetLayerForMapIcon()
	{
		return 0;
	}

	// Token: 0x060033A4 RID: 13220 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool IsMapIconEnabled()
	{
		return false;
	}

	// Token: 0x060033A5 RID: 13221 RVA: 0x0014D2B4 File Offset: 0x0014B4B4
	public override Color GetMapIconColor()
	{
		if (this.IsSelected)
		{
			return new Color32(byte.MaxValue, 180, 0, byte.MaxValue);
		}
		return Color.yellow;
	}

	// Token: 0x0400290A RID: 10506
	public bool IsSelected;

	// Token: 0x0400290B RID: 10507
	[PublicizedFrom(EAccessModifier.Private)]
	public static int newID;
}
