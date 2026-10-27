using System;
using UnityEngine;

// Token: 0x02000644 RID: 1604
public class MapObjectVehicle : MapObject
{
	// Token: 0x0600341C RID: 13340 RVA: 0x0014E0FE File Offset: 0x0014C2FE
	public MapObjectVehicle(Entity _entity)
		: base(EnumMapObjectType.Entity, Vector3.zero, (long)_entity.entityId, _entity, false)
	{
	}

	// Token: 0x0600341D RID: 13341 RVA: 0x0014E115 File Offset: 0x0014C315
	public MapObjectVehicle(MapObjectVehicle _other)
		: base(EnumMapObjectType.Entity, _other.position, (long)_other.entity.entityId, _other.entity, false)
	{
	}

	// Token: 0x0600341E RID: 13342 RVA: 0x0014E9B5 File Offset: 0x0014CBB5
	public override bool IsOnCompass()
	{
		return !(this.entity as EntityVehicle).HasDriver;
	}

	// Token: 0x0600341F RID: 13343 RVA: 0x0014E9CA File Offset: 0x0014CBCA
	public override string GetCompassIcon()
	{
		if (this.type == EnumMapObjectType.Entity && this.entity != null)
		{
			return this.entity.GetMapIcon();
		}
		return null;
	}

	// Token: 0x06003420 RID: 13344 RVA: 0x0004E452 File Offset: 0x0004C652
	public override Vector3 GetRotation()
	{
		return Vector3.zero;
	}
}
