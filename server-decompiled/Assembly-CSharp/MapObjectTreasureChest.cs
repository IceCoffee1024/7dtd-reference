using System;
using UnityEngine;

// Token: 0x0200063C RID: 1596
public class MapObjectTreasureChest : MapObject
{
	// Token: 0x06003406 RID: 13318 RVA: 0x0014DD70 File Offset: 0x0014BF70
	public MapObjectTreasureChest(Vector3 _position, int _questCode, int _defaultRadius)
		: base(EnumMapObjectType.TreasureChest, _position, (long)_questCode, null, false)
	{
		this.DefaultRadius = _defaultRadius;
	}

	// Token: 0x06003407 RID: 13319 RVA: 0x0014DD8C File Offset: 0x0014BF8C
	public override string GetMapIcon()
	{
		return "ui_game_symbol_treasure";
	}

	// Token: 0x06003408 RID: 13320 RVA: 0x0014DD8C File Offset: 0x0014BF8C
	public override string GetCompassIcon()
	{
		return "ui_game_symbol_treasure";
	}

	// Token: 0x06003409 RID: 13321 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsOnCompass()
	{
		return true;
	}

	// Token: 0x0600340A RID: 13322 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsCompassIconClamped()
	{
		return true;
	}

	// Token: 0x0600340B RID: 13323 RVA: 0x00010E62 File Offset: 0x0000F062
	public override int GetLayerForMapIcon()
	{
		return 0;
	}

	// Token: 0x0600340C RID: 13324 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsMapIconEnabled()
	{
		return true;
	}

	// Token: 0x0600340D RID: 13325 RVA: 0x0014DD93 File Offset: 0x0014BF93
	public override Color GetMapIconColor()
	{
		if (this.IsSelected)
		{
			return new Color32(222, 206, 163, byte.MaxValue);
		}
		return Color.white;
	}

	// Token: 0x04002907 RID: 10503
	public bool IsSelected;

	// Token: 0x04002908 RID: 10504
	public int DefaultRadius = 1;
}
