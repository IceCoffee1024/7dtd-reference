using System;
using UnityEngine;

// Token: 0x02000632 RID: 1586
public class MapObjectVehicle : MapObject
{
	// Token: 0x06003378 RID: 13176 RVA: 0x0014AA7E File Offset: 0x00148C7E
	public MapObjectVehicle(Entity _entity)
		: base(EnumMapObjectType.Entity, Vector3.zero, (long)_entity.entityId, _entity, false)
	{
	}

	// Token: 0x06003379 RID: 13177 RVA: 0x0014AA95 File Offset: 0x00148C95
	public MapObjectVehicle(MapObjectVehicle _other)
		: base(EnumMapObjectType.Entity, _other.position, (long)_other.entity.entityId, _other.entity, false)
	{
	}

	// Token: 0x0600337A RID: 13178 RVA: 0x0014B335 File Offset: 0x00149535
	public override bool IsOnCompass()
	{
		return !(this.entity as EntityVehicle).HasDriver;
	}

	// Token: 0x0600337B RID: 13179 RVA: 0x0014B34A File Offset: 0x0014954A
	public override string GetCompassIcon()
	{
		if (this.type == EnumMapObjectType.Entity && this.entity != null)
		{
			return this.entity.GetMapIcon();
		}
		return null;
	}

	// Token: 0x0600337C RID: 13180 RVA: 0x0004E076 File Offset: 0x0004C276
	public override Vector3 GetRotation()
	{
		return Vector3.zero;
	}
}
