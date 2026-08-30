using System;
using UnityEngine;

// Token: 0x0200063C RID: 1596
public class MapObjectVendingMachine : MapObject
{
	// Token: 0x060033D0 RID: 13264 RVA: 0x0014D48B File Offset: 0x0014B68B
	public MapObjectVendingMachine(Vector3 _position, Entity _entity)
		: base(EnumMapObjectType.VendingMachine, _position, (long)_entity.entityId, _entity, false)
	{
	}

	// Token: 0x060033D1 RID: 13265 RVA: 0x0014D49F File Offset: 0x0014B69F
	public override string GetMapIcon()
	{
		return "ui_game_symbol_vending";
	}

	// Token: 0x060033D2 RID: 13266 RVA: 0x0014D49F File Offset: 0x0014B69F
	public override string GetCompassIcon()
	{
		return "ui_game_symbol_vending";
	}

	// Token: 0x060033D3 RID: 13267 RVA: 0x00020021 File Offset: 0x0001E221
	public override bool IsOnCompass()
	{
		return true;
	}

	// Token: 0x060033D4 RID: 13268 RVA: 0x00010E62 File Offset: 0x0000F062
	public override int GetLayerForMapIcon()
	{
		return 0;
	}

	// Token: 0x060033D5 RID: 13269 RVA: 0x00020021 File Offset: 0x0001E221
	public override bool IsMapIconEnabled()
	{
		return true;
	}

	// Token: 0x060033D6 RID: 13270 RVA: 0x000BD617 File Offset: 0x000BB817
	public override Color GetMapIconColor()
	{
		return Color.white;
	}
}
