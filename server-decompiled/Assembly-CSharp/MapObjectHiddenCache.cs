using System;
using UnityEngine;

// Token: 0x02000627 RID: 1575
public class MapObjectHiddenCache : MapObject
{
	// Token: 0x06003313 RID: 13075 RVA: 0x0014AD28 File Offset: 0x00148F28
	public MapObjectHiddenCache(Vector3 _position)
		: base(EnumMapObjectType.HiddenCache, _position, (long)(++MapObjectHiddenCache.newID), null, false)
	{
	}

	// Token: 0x06003314 RID: 13076 RVA: 0x0014ACE9 File Offset: 0x00148EE9
	public override string GetMapIcon()
	{
		return "ui_game_symbol_fetch_loot";
	}

	// Token: 0x06003315 RID: 13077 RVA: 0x0014ACE9 File Offset: 0x00148EE9
	public override string GetCompassIcon()
	{
		return "ui_game_symbol_fetch_loot";
	}

	// Token: 0x06003316 RID: 13078 RVA: 0x0014ACF0 File Offset: 0x00148EF0
	public override string GetCompassDownIcon()
	{
		return "ui_game_symbol_fetch_loot_down";
	}

	// Token: 0x06003317 RID: 13079 RVA: 0x0014ACF7 File Offset: 0x00148EF7
	public override string GetCompassUpIcon()
	{
		return "ui_game_symbol_fetch_loot_up";
	}

	// Token: 0x06003318 RID: 13080 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool UseUpDownCompassIcons()
	{
		return false;
	}

	// Token: 0x06003319 RID: 13081 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool IsOnCompass()
	{
		return true;
	}

	// Token: 0x0600331A RID: 13082 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool IsCompassIconClamped()
	{
		return true;
	}

	// Token: 0x0600331B RID: 13083 RVA: 0x00010E62 File Offset: 0x0000F062
	public override int GetLayerForMapIcon()
	{
		return 0;
	}

	// Token: 0x0600331C RID: 13084 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool IsMapIconEnabled()
	{
		return false;
	}

	// Token: 0x0600331D RID: 13085 RVA: 0x0014AD43 File Offset: 0x00148F43
	public override Color GetMapIconColor()
	{
		if (this.IsSelected)
		{
			return new Color32(byte.MaxValue, 180, 0, byte.MaxValue);
		}
		return Color.white;
	}

	// Token: 0x0400288E RID: 10382
	public bool IsSelected;

	// Token: 0x0400288F RID: 10383
	[PublicizedFrom(EAccessModifier.Private)]
	public static int newID;
}
