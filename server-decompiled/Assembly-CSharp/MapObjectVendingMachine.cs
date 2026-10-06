using System;
using UnityEngine;

// Token: 0x0200063E RID: 1598
public class MapObjectVendingMachine : MapObject
{
	// Token: 0x06003413 RID: 13331 RVA: 0x0014DDFB File Offset: 0x0014BFFB
	public MapObjectVendingMachine(Vector3 _position, Entity _entity)
		: base(EnumMapObjectType.VendingMachine, _position, (long)_entity.entityId, _entity, false)
	{
	}

	// Token: 0x06003414 RID: 13332 RVA: 0x0014DE0F File Offset: 0x0014C00F
	public override string GetMapIcon()
	{
		return "ui_game_symbol_vending";
	}

	// Token: 0x06003415 RID: 13333 RVA: 0x0014DE0F File Offset: 0x0014C00F
	public override string GetCompassIcon()
	{
		return "ui_game_symbol_vending";
	}

	// Token: 0x06003416 RID: 13334 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsOnCompass()
	{
		return true;
	}

	// Token: 0x06003417 RID: 13335 RVA: 0x00010E62 File Offset: 0x0000F062
	public override int GetLayerForMapIcon()
	{
		return 0;
	}

	// Token: 0x06003418 RID: 13336 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsMapIconEnabled()
	{
		return true;
	}

	// Token: 0x06003419 RID: 13337 RVA: 0x000BD7EF File Offset: 0x000BB9EF
	public override Color GetMapIconColor()
	{
		return Color.white;
	}
}
