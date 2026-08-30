using System;
using UnityEngine;

// Token: 0x0200063B RID: 1595
public class MapObjectVehicle : MapObject
{
	// Token: 0x060033CB RID: 13259 RVA: 0x0014CB9A File Offset: 0x0014AD9A
	public MapObjectVehicle(Entity _entity)
		: base(EnumMapObjectType.Entity, Vector3.zero, (long)_entity.entityId, _entity, false)
	{
	}

	// Token: 0x060033CC RID: 13260 RVA: 0x0014CBB1 File Offset: 0x0014ADB1
	public MapObjectVehicle(MapObjectVehicle _other)
		: base(EnumMapObjectType.Entity, _other.position, (long)_other.entity.entityId, _other.entity, false)
	{
	}

	// Token: 0x060033CD RID: 13261 RVA: 0x0014D451 File Offset: 0x0014B651
	public override bool IsOnCompass()
	{
		return !(this.entity as EntityVehicle).HasDriver;
	}

	// Token: 0x060033CE RID: 13262 RVA: 0x0014D466 File Offset: 0x0014B666
	public override string GetCompassIcon()
	{
		if (this.type == EnumMapObjectType.Entity && this.entity != null)
		{
			return this.entity.GetMapIcon();
		}
		return null;
	}

	// Token: 0x060033CF RID: 13263 RVA: 0x0004E48E File Offset: 0x0004C68E
	public override Vector3 GetRotation()
	{
		return Vector3.zero;
	}
}
