using System;
using UnityEngine;

// Token: 0x02000635 RID: 1589
public class MapObjectZombie : MapObject
{
	// Token: 0x06003392 RID: 13202 RVA: 0x0014AA7E File Offset: 0x00148C7E
	public MapObjectZombie(Entity _entity)
		: base(EnumMapObjectType.Entity, Vector3.zero, (long)_entity.entityId, _entity, false)
	{
	}

	// Token: 0x06003393 RID: 13203 RVA: 0x0014AA95 File Offset: 0x00148C95
	public MapObjectZombie(MapObjectZombie _other)
		: base(EnumMapObjectType.Entity, _other.position, (long)_other.entity.entityId, _other.entity, false)
	{
	}

	// Token: 0x06003394 RID: 13204 RVA: 0x0014B456 File Offset: 0x00149656
	public override void RefreshData()
	{
		if (this.type == EnumMapObjectType.Entity && this.entity != null)
		{
			EntityAlive entityAlive = (EntityAlive)this.entity;
			this.entity.world.GetPrimaryPlayer();
		}
		this.trackingType = MapObjectZombie.TrackingTypes.None;
	}

	// Token: 0x06003395 RID: 13205 RVA: 0x0014B492 File Offset: 0x00149692
	public override bool IsOnCompass()
	{
		return this.trackingType > MapObjectZombie.TrackingTypes.None;
	}

	// Token: 0x06003396 RID: 13206 RVA: 0x0014B49D File Offset: 0x0014969D
	public override string GetMapIcon()
	{
		if (this.trackingType != MapObjectZombie.TrackingTypes.Tracking)
		{
			return this.entity.GetMapIcon();
		}
		return this.entity.GetTrackerIcon();
	}

	// Token: 0x06003397 RID: 13207 RVA: 0x0014B4BF File Offset: 0x001496BF
	public override string GetCompassIcon()
	{
		if (this.trackingType != MapObjectZombie.TrackingTypes.Tracking)
		{
			return this.entity.GetCompassIcon();
		}
		return this.entity.GetTrackerIcon();
	}

	// Token: 0x06003398 RID: 13208 RVA: 0x0014B4E1 File Offset: 0x001496E1
	public override bool UseUpDownCompassIcons()
	{
		return this.trackingType == MapObjectZombie.TrackingTypes.Quest;
	}

	// Token: 0x06003399 RID: 13209 RVA: 0x0014B4E1 File Offset: 0x001496E1
	public override bool IsCompassIconClamped()
	{
		return this.trackingType == MapObjectZombie.TrackingTypes.Quest;
	}

	// Token: 0x0600339A RID: 13210 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool NearbyCompassBlink()
	{
		return true;
	}

	// Token: 0x0600339B RID: 13211 RVA: 0x0014B4EC File Offset: 0x001496EC
	public override bool IsMapIconEnabled()
	{
		return this.trackingType == MapObjectZombie.TrackingTypes.Tracking;
	}

	// Token: 0x0600339C RID: 13212 RVA: 0x00040D24 File Offset: 0x0003EF24
	public override float GetMaxCompassIconScale()
	{
		return 1f;
	}

	// Token: 0x0600339D RID: 13213 RVA: 0x0014AB91 File Offset: 0x00148D91
	public override float GetMinCompassIconScale()
	{
		return 0.6f;
	}

	// Token: 0x0600339E RID: 13214 RVA: 0x0014B4F7 File Offset: 0x001496F7
	public override Color GetMapIconColor()
	{
		if (this.trackingType != MapObjectZombie.TrackingTypes.Quest)
		{
			return this.entity.GetMapIconColor();
		}
		return Color.red;
	}

	// Token: 0x0600339F RID: 13215 RVA: 0x0014B513 File Offset: 0x00149713
	public override float GetMaxCompassDistance()
	{
		return (float)((this.trackingType == MapObjectZombie.TrackingTypes.Quest) ? 32 : 100);
	}

	// Token: 0x060033A0 RID: 13216 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool IsShowName()
	{
		return false;
	}

	// Token: 0x040028A1 RID: 10401
	[PublicizedFrom(EAccessModifier.Private)]
	public MapObjectZombie.TrackingTypes trackingType;

	// Token: 0x02000636 RID: 1590
	[PublicizedFrom(EAccessModifier.Private)]
	public enum TrackingTypes
	{
		// Token: 0x040028A3 RID: 10403
		None,
		// Token: 0x040028A4 RID: 10404
		Tracking,
		// Token: 0x040028A5 RID: 10405
		Quest
	}
}
