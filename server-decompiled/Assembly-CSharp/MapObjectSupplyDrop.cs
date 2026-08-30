using System;
using UnityEngine;

// Token: 0x02000639 RID: 1593
public class MapObjectSupplyDrop : MapObject
{
	// Token: 0x060033BA RID: 13242 RVA: 0x0014D3E4 File Offset: 0x0014B5E4
	public MapObjectSupplyDrop(Vector3 _position, long entityID)
		: base(EnumMapObjectType.SupplyDrop, _position, entityID, null, false)
	{
	}

	// Token: 0x060033BB RID: 13243 RVA: 0x0014D3F2 File Offset: 0x0014B5F2
	public override string GetMapIcon()
	{
		return "ui_game_symbol_airdrop";
	}

	// Token: 0x060033BC RID: 13244 RVA: 0x0014D3F2 File Offset: 0x0014B5F2
	public override string GetCompassIcon()
	{
		return "ui_game_symbol_airdrop";
	}

	// Token: 0x060033BD RID: 13245 RVA: 0x00020021 File Offset: 0x0001E221
	public override bool IsOnCompass()
	{
		return true;
	}

	// Token: 0x060033BE RID: 13246 RVA: 0x00010E62 File Offset: 0x0000F062
	public override int GetLayerForMapIcon()
	{
		return 0;
	}

	// Token: 0x060033BF RID: 13247 RVA: 0x00020021 File Offset: 0x0001E221
	public override bool IsMapIconEnabled()
	{
		return true;
	}

	// Token: 0x060033C0 RID: 13248 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool IsShowName()
	{
		return false;
	}

	// Token: 0x060033C1 RID: 13249 RVA: 0x0014D3F9 File Offset: 0x0014B5F9
	public override float GetMaxCompassDistance()
	{
		return 4096f;
	}

	// Token: 0x060033C2 RID: 13250 RVA: 0x0014D324 File Offset: 0x0014B524
	public override Color GetMapIconColor()
	{
		return new Color32(byte.MaxValue, 180, 0, byte.MaxValue);
	}
}
