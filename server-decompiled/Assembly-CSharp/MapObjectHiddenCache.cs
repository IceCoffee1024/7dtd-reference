using System;
using UnityEngine;

// Token: 0x02000639 RID: 1593
public class MapObjectHiddenCache : MapObject
{
	// Token: 0x060033B7 RID: 13239 RVA: 0x0014E3A8 File Offset: 0x0014C5A8
	public MapObjectHiddenCache(Vector3 _position)
		: base(EnumMapObjectType.HiddenCache, _position, (long)(++MapObjectHiddenCache.newID), null, false)
	{
	}

	// Token: 0x060033B8 RID: 13240 RVA: 0x0014E369 File Offset: 0x0014C569
	public override string GetMapIcon()
	{
		return "ui_game_symbol_fetch_loot";
	}

	// Token: 0x060033B9 RID: 13241 RVA: 0x0014E369 File Offset: 0x0014C569
	public override string GetCompassIcon()
	{
		return "ui_game_symbol_fetch_loot";
	}

	// Token: 0x060033BA RID: 13242 RVA: 0x0014E370 File Offset: 0x0014C570
	public override string GetCompassDownIcon()
	{
		return "ui_game_symbol_fetch_loot_down";
	}

	// Token: 0x060033BB RID: 13243 RVA: 0x0014E377 File Offset: 0x0014C577
	public override string GetCompassUpIcon()
	{
		return "ui_game_symbol_fetch_loot_up";
	}

	// Token: 0x060033BC RID: 13244 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool UseUpDownCompassIcons()
	{
		return false;
	}

	// Token: 0x060033BD RID: 13245 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsOnCompass()
	{
		return true;
	}

	// Token: 0x060033BE RID: 13246 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsCompassIconClamped()
	{
		return true;
	}

	// Token: 0x060033BF RID: 13247 RVA: 0x00010E62 File Offset: 0x0000F062
	public override int GetLayerForMapIcon()
	{
		return 0;
	}

	// Token: 0x060033C0 RID: 13248 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool IsMapIconEnabled()
	{
		return false;
	}

	// Token: 0x060033C1 RID: 13249 RVA: 0x0014E3C3 File Offset: 0x0014C5C3
	public override Color GetMapIconColor()
	{
		if (this.IsSelected)
		{
			return new Color32(byte.MaxValue, 180, 0, byte.MaxValue);
		}
		return Color.white;
	}

	// Token: 0x04002915 RID: 10517
	public bool IsSelected;

	// Token: 0x04002916 RID: 10518
	[PublicizedFrom(EAccessModifier.Private)]
	public static int newID;
}
