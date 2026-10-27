using System;
using UnityEngine;

// Token: 0x02000645 RID: 1605
public class MapObjectVendingMachine : MapObject
{
	// Token: 0x06003421 RID: 13345 RVA: 0x0014E9EF File Offset: 0x0014CBEF
	public MapObjectVendingMachine(Vector3 _position, Entity _entity)
		: base(EnumMapObjectType.VendingMachine, _position, (long)_entity.entityId, _entity, false)
	{
	}

	// Token: 0x06003422 RID: 13346 RVA: 0x0014EA03 File Offset: 0x0014CC03
	public override string GetMapIcon()
	{
		return "ui_game_symbol_vending";
	}

	// Token: 0x06003423 RID: 13347 RVA: 0x0014EA03 File Offset: 0x0014CC03
	public override string GetCompassIcon()
	{
		return "ui_game_symbol_vending";
	}

	// Token: 0x06003424 RID: 13348 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsOnCompass()
	{
		return true;
	}

	// Token: 0x06003425 RID: 13349 RVA: 0x00010E62 File Offset: 0x0000F062
	public override int GetLayerForMapIcon()
	{
		return 0;
	}

	// Token: 0x06003426 RID: 13350 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsMapIconEnabled()
	{
		return true;
	}

	// Token: 0x06003427 RID: 13351 RVA: 0x000BDB9B File Offset: 0x000BBD9B
	public override Color GetMapIconColor()
	{
		return Color.white;
	}
}
