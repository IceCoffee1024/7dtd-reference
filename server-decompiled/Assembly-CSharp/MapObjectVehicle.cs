using System;
using UnityEngine;

// Token: 0x0200063D RID: 1597
public class MapObjectVehicle : MapObject
{
	// Token: 0x0600340E RID: 13326 RVA: 0x0014D50A File Offset: 0x0014B70A
	public MapObjectVehicle(Entity _entity)
		: base(EnumMapObjectType.Entity, Vector3.zero, (long)_entity.entityId, _entity, false)
	{
	}

	// Token: 0x0600340F RID: 13327 RVA: 0x0014D521 File Offset: 0x0014B721
	public MapObjectVehicle(MapObjectVehicle _other)
		: base(EnumMapObjectType.Entity, _other.position, (long)_other.entity.entityId, _other.entity, false)
	{
	}

	// Token: 0x06003410 RID: 13328 RVA: 0x0014DDC1 File Offset: 0x0014BFC1
	public override bool IsOnCompass()
	{
		return !(this.entity as EntityVehicle).HasDriver;
	}

	// Token: 0x06003411 RID: 13329 RVA: 0x0014DDD6 File Offset: 0x0014BFD6
	public override string GetCompassIcon()
	{
		if (this.type == EnumMapObjectType.Entity && this.entity != null)
		{
			return this.entity.GetMapIcon();
		}
		return null;
	}

	// Token: 0x06003412 RID: 13330 RVA: 0x0004E492 File Offset: 0x0004C692
	public override Vector3 GetRotation()
	{
		return Vector3.zero;
	}
}
