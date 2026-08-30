using System;
using UnityEngine;

// Token: 0x02000631 RID: 1585
public class MapObjectTreasureChest : MapObject
{
	// Token: 0x06003370 RID: 13168 RVA: 0x0014B2E4 File Offset: 0x001494E4
	public MapObjectTreasureChest(Vector3 _position, int _questCode, int _defaultRadius)
		: base(EnumMapObjectType.TreasureChest, _position, (long)_questCode, null, false)
	{
		this.DefaultRadius = _defaultRadius;
	}

	// Token: 0x06003371 RID: 13169 RVA: 0x0014B300 File Offset: 0x00149500
	public override string GetMapIcon()
	{
		return "ui_game_symbol_treasure";
	}

	// Token: 0x06003372 RID: 13170 RVA: 0x0014B300 File Offset: 0x00149500
	public override string GetCompassIcon()
	{
		return "ui_game_symbol_treasure";
	}

	// Token: 0x06003373 RID: 13171 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool IsOnCompass()
	{
		return true;
	}

	// Token: 0x06003374 RID: 13172 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool IsCompassIconClamped()
	{
		return true;
	}

	// Token: 0x06003375 RID: 13173 RVA: 0x00010E62 File Offset: 0x0000F062
	public override int GetLayerForMapIcon()
	{
		return 0;
	}

	// Token: 0x06003376 RID: 13174 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool IsMapIconEnabled()
	{
		return true;
	}

	// Token: 0x06003377 RID: 13175 RVA: 0x0014B307 File Offset: 0x00149507
	public override Color GetMapIconColor()
	{
		if (this.IsSelected)
		{
			return new Color32(222, 206, 163, byte.MaxValue);
		}
		return Color.white;
	}

	// Token: 0x0400289B RID: 10395
	public bool IsSelected;

	// Token: 0x0400289C RID: 10396
	public int DefaultRadius = 1;
}
