using System;
using UnityEngine;

// Token: 0x02000640 RID: 1600
public class MapObjectZombie : MapObject
{
	// Token: 0x06003428 RID: 13352 RVA: 0x0014D50A File Offset: 0x0014B70A
	public MapObjectZombie(Entity _entity)
		: base(EnumMapObjectType.Entity, Vector3.zero, (long)_entity.entityId, _entity, false)
	{
	}

	// Token: 0x06003429 RID: 13353 RVA: 0x0014D521 File Offset: 0x0014B721
	public MapObjectZombie(MapObjectZombie _other)
		: base(EnumMapObjectType.Entity, _other.position, (long)_other.entity.entityId, _other.entity, false)
	{
	}

	// Token: 0x0600342A RID: 13354 RVA: 0x0014DEE2 File Offset: 0x0014C0E2
	public override void RefreshData()
	{
		if (this.type == EnumMapObjectType.Entity && this.entity != null)
		{
			EntityAlive entityAlive = (EntityAlive)this.entity;
			this.entity.world.GetPrimaryPlayer();
		}
		this.trackingType = MapObjectZombie.TrackingTypes.None;
	}

	// Token: 0x0600342B RID: 13355 RVA: 0x0014DF1E File Offset: 0x0014C11E
	public override bool IsOnCompass()
	{
		return this.trackingType > MapObjectZombie.TrackingTypes.None;
	}

	// Token: 0x0600342C RID: 13356 RVA: 0x0014DF29 File Offset: 0x0014C129
	public override string GetMapIcon()
	{
		if (this.trackingType != MapObjectZombie.TrackingTypes.Tracking)
		{
			return this.entity.GetMapIcon();
		}
		return this.entity.GetTrackerIcon();
	}

	// Token: 0x0600342D RID: 13357 RVA: 0x0014DF4B File Offset: 0x0014C14B
	public override string GetCompassIcon()
	{
		if (this.trackingType != MapObjectZombie.TrackingTypes.Tracking)
		{
			return this.entity.GetCompassIcon();
		}
		return this.entity.GetTrackerIcon();
	}

	// Token: 0x0600342E RID: 13358 RVA: 0x0014DF6D File Offset: 0x0014C16D
	public override bool UseUpDownCompassIcons()
	{
		return this.trackingType == MapObjectZombie.TrackingTypes.Quest;
	}

	// Token: 0x0600342F RID: 13359 RVA: 0x0014DF6D File Offset: 0x0014C16D
	public override bool IsCompassIconClamped()
	{
		return this.trackingType == MapObjectZombie.TrackingTypes.Quest;
	}

	// Token: 0x06003430 RID: 13360 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool NearbyCompassBlink()
	{
		return true;
	}

	// Token: 0x06003431 RID: 13361 RVA: 0x0014DF78 File Offset: 0x0014C178
	public override bool IsMapIconEnabled()
	{
		return this.trackingType == MapObjectZombie.TrackingTypes.Tracking;
	}

	// Token: 0x06003432 RID: 13362 RVA: 0x00041064 File Offset: 0x0003F264
	public override float GetMaxCompassIconScale()
	{
		return 1f;
	}

	// Token: 0x06003433 RID: 13363 RVA: 0x0014D61D File Offset: 0x0014B81D
	public override float GetMinCompassIconScale()
	{
		return 0.6f;
	}

	// Token: 0x06003434 RID: 13364 RVA: 0x0014DF83 File Offset: 0x0014C183
	public override Color GetMapIconColor()
	{
		if (this.trackingType != MapObjectZombie.TrackingTypes.Quest)
		{
			return this.entity.GetMapIconColor();
		}
		return Color.red;
	}

	// Token: 0x06003435 RID: 13365 RVA: 0x0014DF9F File Offset: 0x0014C19F
	public override float GetMaxCompassDistance()
	{
		return (float)((this.trackingType == MapObjectZombie.TrackingTypes.Quest) ? 32 : 100);
	}

	// Token: 0x06003436 RID: 13366 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool IsShowName()
	{
		return false;
	}

	// Token: 0x0400290D RID: 10509
	[PublicizedFrom(EAccessModifier.Private)]
	public MapObjectZombie.TrackingTypes trackingType;

	// Token: 0x02000641 RID: 1601
	[PublicizedFrom(EAccessModifier.Private)]
	public enum TrackingTypes
	{
		// Token: 0x0400290F RID: 10511
		None,
		// Token: 0x04002910 RID: 10512
		Tracking,
		// Token: 0x04002911 RID: 10513
		Quest
	}
}
