using System;
using UnityEngine;

// Token: 0x02000643 RID: 1603
public class MapObjectTreasureChest : MapObject
{
	// Token: 0x06003414 RID: 13332 RVA: 0x0014E964 File Offset: 0x0014CB64
	public MapObjectTreasureChest(Vector3 _position, int _questCode, int _defaultRadius)
		: base(EnumMapObjectType.TreasureChest, _position, (long)_questCode, null, false)
	{
		this.DefaultRadius = _defaultRadius;
	}

	// Token: 0x06003415 RID: 13333 RVA: 0x0014E980 File Offset: 0x0014CB80
	public override string GetMapIcon()
	{
		return "ui_game_symbol_treasure";
	}

	// Token: 0x06003416 RID: 13334 RVA: 0x0014E980 File Offset: 0x0014CB80
	public override string GetCompassIcon()
	{
		return "ui_game_symbol_treasure";
	}

	// Token: 0x06003417 RID: 13335 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsOnCompass()
	{
		return true;
	}

	// Token: 0x06003418 RID: 13336 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsCompassIconClamped()
	{
		return true;
	}

	// Token: 0x06003419 RID: 13337 RVA: 0x00010E62 File Offset: 0x0000F062
	public override int GetLayerForMapIcon()
	{
		return 0;
	}

	// Token: 0x0600341A RID: 13338 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsMapIconEnabled()
	{
		return true;
	}

	// Token: 0x0600341B RID: 13339 RVA: 0x0014E987 File Offset: 0x0014CB87
	public override Color GetMapIconColor()
	{
		if (this.IsSelected)
		{
			return new Color32(222, 206, 163, byte.MaxValue);
		}
		return Color.white;
	}

	// Token: 0x04002922 RID: 10530
	public bool IsSelected;

	// Token: 0x04002923 RID: 10531
	public int DefaultRadius = 1;
}
