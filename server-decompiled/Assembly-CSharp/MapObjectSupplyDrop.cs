using System;
using UnityEngine;

// Token: 0x0200063B RID: 1595
public class MapObjectSupplyDrop : MapObject
{
	// Token: 0x060033FD RID: 13309 RVA: 0x0014DD54 File Offset: 0x0014BF54
	public MapObjectSupplyDrop(Vector3 _position, long entityID)
		: base(EnumMapObjectType.SupplyDrop, _position, entityID, null, false)
	{
	}

	// Token: 0x060033FE RID: 13310 RVA: 0x0014DD62 File Offset: 0x0014BF62
	public override string GetMapIcon()
	{
		return "ui_game_symbol_airdrop";
	}

	// Token: 0x060033FF RID: 13311 RVA: 0x0014DD62 File Offset: 0x0014BF62
	public override string GetCompassIcon()
	{
		return "ui_game_symbol_airdrop";
	}

	// Token: 0x06003400 RID: 13312 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsOnCompass()
	{
		return true;
	}

	// Token: 0x06003401 RID: 13313 RVA: 0x00010E62 File Offset: 0x0000F062
	public override int GetLayerForMapIcon()
	{
		return 0;
	}

	// Token: 0x06003402 RID: 13314 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsMapIconEnabled()
	{
		return true;
	}

	// Token: 0x06003403 RID: 13315 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool IsShowName()
	{
		return false;
	}

	// Token: 0x06003404 RID: 13316 RVA: 0x0014DD69 File Offset: 0x0014BF69
	public override float GetMaxCompassDistance()
	{
		return 4096f;
	}

	// Token: 0x06003405 RID: 13317 RVA: 0x0014DC94 File Offset: 0x0014BE94
	public override Color GetMapIconColor()
	{
		return new Color32(byte.MaxValue, 180, 0, byte.MaxValue);
	}
}
