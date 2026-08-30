using System;
using UnityEngine;

// Token: 0x02000630 RID: 1584
public class MapObjectSupplyDrop : MapObject
{
	// Token: 0x06003367 RID: 13159 RVA: 0x0014B2C8 File Offset: 0x001494C8
	public MapObjectSupplyDrop(Vector3 _position, long entityID)
		: base(EnumMapObjectType.SupplyDrop, _position, entityID, null, false)
	{
	}

	// Token: 0x06003368 RID: 13160 RVA: 0x0014B2D6 File Offset: 0x001494D6
	public override string GetMapIcon()
	{
		return "ui_game_symbol_airdrop";
	}

	// Token: 0x06003369 RID: 13161 RVA: 0x0014B2D6 File Offset: 0x001494D6
	public override string GetCompassIcon()
	{
		return "ui_game_symbol_airdrop";
	}

	// Token: 0x0600336A RID: 13162 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool IsOnCompass()
	{
		return true;
	}

	// Token: 0x0600336B RID: 13163 RVA: 0x00010E62 File Offset: 0x0000F062
	public override int GetLayerForMapIcon()
	{
		return 0;
	}

	// Token: 0x0600336C RID: 13164 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool IsMapIconEnabled()
	{
		return true;
	}

	// Token: 0x0600336D RID: 13165 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool IsShowName()
	{
		return false;
	}

	// Token: 0x0600336E RID: 13166 RVA: 0x0014B2DD File Offset: 0x001494DD
	public override float GetMaxCompassDistance()
	{
		return 4096f;
	}

	// Token: 0x0600336F RID: 13167 RVA: 0x0014B208 File Offset: 0x00149408
	public override Color GetMapIconColor()
	{
		return new Color32(byte.MaxValue, 180, 0, byte.MaxValue);
	}
}
