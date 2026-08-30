using System;
using UnityEngine;

// Token: 0x02000630 RID: 1584
public class MapObjectHiddenCache : MapObject
{
	// Token: 0x06003366 RID: 13158 RVA: 0x0014CE44 File Offset: 0x0014B044
	public MapObjectHiddenCache(Vector3 _position)
		: base(EnumMapObjectType.HiddenCache, _position, (long)(++MapObjectHiddenCache.newID), null, false)
	{
	}

	// Token: 0x06003367 RID: 13159 RVA: 0x0014CE05 File Offset: 0x0014B005
	public override string GetMapIcon()
	{
		return "ui_game_symbol_fetch_loot";
	}

	// Token: 0x06003368 RID: 13160 RVA: 0x0014CE05 File Offset: 0x0014B005
	public override string GetCompassIcon()
	{
		return "ui_game_symbol_fetch_loot";
	}

	// Token: 0x06003369 RID: 13161 RVA: 0x0014CE0C File Offset: 0x0014B00C
	public override string GetCompassDownIcon()
	{
		return "ui_game_symbol_fetch_loot_down";
	}

	// Token: 0x0600336A RID: 13162 RVA: 0x0014CE13 File Offset: 0x0014B013
	public override string GetCompassUpIcon()
	{
		return "ui_game_symbol_fetch_loot_up";
	}

	// Token: 0x0600336B RID: 13163 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool UseUpDownCompassIcons()
	{
		return false;
	}

	// Token: 0x0600336C RID: 13164 RVA: 0x00020021 File Offset: 0x0001E221
	public override bool IsOnCompass()
	{
		return true;
	}

	// Token: 0x0600336D RID: 13165 RVA: 0x00020021 File Offset: 0x0001E221
	public override bool IsCompassIconClamped()
	{
		return true;
	}

	// Token: 0x0600336E RID: 13166 RVA: 0x00010E62 File Offset: 0x0000F062
	public override int GetLayerForMapIcon()
	{
		return 0;
	}

	// Token: 0x0600336F RID: 13167 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool IsMapIconEnabled()
	{
		return false;
	}

	// Token: 0x06003370 RID: 13168 RVA: 0x0014CE5F File Offset: 0x0014B05F
	public override Color GetMapIconColor()
	{
		if (this.IsSelected)
		{
			return new Color32(byte.MaxValue, 180, 0, byte.MaxValue);
		}
		return Color.white;
	}

	// Token: 0x04002901 RID: 10497
	public bool IsSelected;

	// Token: 0x04002902 RID: 10498
	[PublicizedFrom(EAccessModifier.Private)]
	public static int newID;
}
