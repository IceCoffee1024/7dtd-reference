using System;
using UnityEngine;

// Token: 0x02000633 RID: 1587
public class MapObjectVendingMachine : MapObject
{
	// Token: 0x0600337D RID: 13181 RVA: 0x0014B36F File Offset: 0x0014956F
	public MapObjectVendingMachine(Vector3 _position, Entity _entity)
		: base(EnumMapObjectType.VendingMachine, _position, (long)_entity.entityId, _entity, false)
	{
	}

	// Token: 0x0600337E RID: 13182 RVA: 0x0014B383 File Offset: 0x00149583
	public override string GetMapIcon()
	{
		return "ui_game_symbol_vending";
	}

	// Token: 0x0600337F RID: 13183 RVA: 0x0014B383 File Offset: 0x00149583
	public override string GetCompassIcon()
	{
		return "ui_game_symbol_vending";
	}

	// Token: 0x06003380 RID: 13184 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool IsOnCompass()
	{
		return true;
	}

	// Token: 0x06003381 RID: 13185 RVA: 0x00010E62 File Offset: 0x0000F062
	public override int GetLayerForMapIcon()
	{
		return 0;
	}

	// Token: 0x06003382 RID: 13186 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool IsMapIconEnabled()
	{
		return true;
	}

	// Token: 0x06003383 RID: 13187 RVA: 0x000BCED3 File Offset: 0x000BB0D3
	public override Color GetMapIconColor()
	{
		return Color.white;
	}
}
