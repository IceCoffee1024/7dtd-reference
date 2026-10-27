using System;
using UnityEngine;

// Token: 0x02000642 RID: 1602
public class MapObjectSupplyDrop : MapObject
{
	// Token: 0x0600340B RID: 13323 RVA: 0x0014E948 File Offset: 0x0014CB48
	public MapObjectSupplyDrop(Vector3 _position, long entityID)
		: base(EnumMapObjectType.SupplyDrop, _position, entityID, null, false)
	{
	}

	// Token: 0x0600340C RID: 13324 RVA: 0x0014E956 File Offset: 0x0014CB56
	public override string GetMapIcon()
	{
		return "ui_game_symbol_airdrop";
	}

	// Token: 0x0600340D RID: 13325 RVA: 0x0014E956 File Offset: 0x0014CB56
	public override string GetCompassIcon()
	{
		return "ui_game_symbol_airdrop";
	}

	// Token: 0x0600340E RID: 13326 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsOnCompass()
	{
		return true;
	}

	// Token: 0x0600340F RID: 13327 RVA: 0x00010E62 File Offset: 0x0000F062
	public override int GetLayerForMapIcon()
	{
		return 0;
	}

	// Token: 0x06003410 RID: 13328 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsMapIconEnabled()
	{
		return true;
	}

	// Token: 0x06003411 RID: 13329 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool IsShowName()
	{
		return false;
	}

	// Token: 0x06003412 RID: 13330 RVA: 0x0014E95D File Offset: 0x0014CB5D
	public override float GetMaxCompassDistance()
	{
		return 4096f;
	}

	// Token: 0x06003413 RID: 13331 RVA: 0x0014E888 File Offset: 0x0014CA88
	public override Color GetMapIconColor()
	{
		return new Color32(byte.MaxValue, 180, 0, byte.MaxValue);
	}
}
