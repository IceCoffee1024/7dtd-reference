using System;
using UnityEngine;

// Token: 0x0200062D RID: 1581
public class MapObjectRestorePower : MapObject
{
	// Token: 0x06003347 RID: 13127 RVA: 0x0014B17D File Offset: 0x0014937D
	public MapObjectRestorePower(Vector3 _position)
		: base(EnumMapObjectType.RestorePower, _position, (long)(++MapObjectRestorePower.newID), null, false)
	{
	}

	// Token: 0x06003348 RID: 13128 RVA: 0x0014ACE9 File Offset: 0x00148EE9
	public override string GetMapIcon()
	{
		return "ui_game_symbol_fetch_loot";
	}

	// Token: 0x06003349 RID: 13129 RVA: 0x0014ACE9 File Offset: 0x00148EE9
	public override string GetCompassIcon()
	{
		return "ui_game_symbol_fetch_loot";
	}

	// Token: 0x0600334A RID: 13130 RVA: 0x0014ACF0 File Offset: 0x00148EF0
	public override string GetCompassDownIcon()
	{
		return "ui_game_symbol_fetch_loot_down";
	}

	// Token: 0x0600334B RID: 13131 RVA: 0x0014ACF7 File Offset: 0x00148EF7
	public override string GetCompassUpIcon()
	{
		return "ui_game_symbol_fetch_loot_up";
	}

	// Token: 0x0600334C RID: 13132 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool UseUpDownCompassIcons()
	{
		return true;
	}

	// Token: 0x0600334D RID: 13133 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool IsOnCompass()
	{
		return true;
	}

	// Token: 0x0600334E RID: 13134 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool IsCompassIconClamped()
	{
		return true;
	}

	// Token: 0x0600334F RID: 13135 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool NearbyCompassBlink()
	{
		return true;
	}

	// Token: 0x06003350 RID: 13136 RVA: 0x00010E62 File Offset: 0x0000F062
	public override int GetLayerForMapIcon()
	{
		return 0;
	}

	// Token: 0x06003351 RID: 13137 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool IsMapIconEnabled()
	{
		return false;
	}

	// Token: 0x06003352 RID: 13138 RVA: 0x0014B198 File Offset: 0x00149398
	public override Color GetMapIconColor()
	{
		if (this.IsSelected)
		{
			return new Color32(byte.MaxValue, 180, 0, byte.MaxValue);
		}
		return Color.yellow;
	}

	// Token: 0x04002897 RID: 10391
	public bool IsSelected;

	// Token: 0x04002898 RID: 10392
	[PublicizedFrom(EAccessModifier.Private)]
	public static int newID;
}
