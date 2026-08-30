using System;
using UnityEngine;

// Token: 0x0200063A RID: 1594
public class MapObjectTreasureChest : MapObject
{
	// Token: 0x060033C3 RID: 13251 RVA: 0x0014D400 File Offset: 0x0014B600
	public MapObjectTreasureChest(Vector3 _position, int _questCode, int _defaultRadius)
		: base(EnumMapObjectType.TreasureChest, _position, (long)_questCode, null, false)
	{
		this.DefaultRadius = _defaultRadius;
	}

	// Token: 0x060033C4 RID: 13252 RVA: 0x0014D41C File Offset: 0x0014B61C
	public override string GetMapIcon()
	{
		return "ui_game_symbol_treasure";
	}

	// Token: 0x060033C5 RID: 13253 RVA: 0x0014D41C File Offset: 0x0014B61C
	public override string GetCompassIcon()
	{
		return "ui_game_symbol_treasure";
	}

	// Token: 0x060033C6 RID: 13254 RVA: 0x00020021 File Offset: 0x0001E221
	public override bool IsOnCompass()
	{
		return true;
	}

	// Token: 0x060033C7 RID: 13255 RVA: 0x00020021 File Offset: 0x0001E221
	public override bool IsCompassIconClamped()
	{
		return true;
	}

	// Token: 0x060033C8 RID: 13256 RVA: 0x00010E62 File Offset: 0x0000F062
	public override int GetLayerForMapIcon()
	{
		return 0;
	}

	// Token: 0x060033C9 RID: 13257 RVA: 0x00020021 File Offset: 0x0001E221
	public override bool IsMapIconEnabled()
	{
		return true;
	}

	// Token: 0x060033CA RID: 13258 RVA: 0x0014D423 File Offset: 0x0014B623
	public override Color GetMapIconColor()
	{
		if (this.IsSelected)
		{
			return new Color32(222, 206, 163, byte.MaxValue);
		}
		return Color.white;
	}

	// Token: 0x0400290E RID: 10510
	public bool IsSelected;

	// Token: 0x0400290F RID: 10511
	public int DefaultRadius = 1;
}
