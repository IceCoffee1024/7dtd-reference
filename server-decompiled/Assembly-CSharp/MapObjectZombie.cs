using System;
using UnityEngine;

// Token: 0x02000647 RID: 1607
public class MapObjectZombie : MapObject
{
	// Token: 0x06003436 RID: 13366 RVA: 0x0014E0FE File Offset: 0x0014C2FE
	public MapObjectZombie(Entity _entity)
		: base(EnumMapObjectType.Entity, Vector3.zero, (long)_entity.entityId, _entity, false)
	{
	}

	// Token: 0x06003437 RID: 13367 RVA: 0x0014E115 File Offset: 0x0014C315
	public MapObjectZombie(MapObjectZombie _other)
		: base(EnumMapObjectType.Entity, _other.position, (long)_other.entity.entityId, _other.entity, false)
	{
	}

	// Token: 0x06003438 RID: 13368 RVA: 0x0014EAD6 File Offset: 0x0014CCD6
	public override void RefreshData()
	{
		if (this.type == EnumMapObjectType.Entity && this.entity != null)
		{
			EntityAlive entityAlive = (EntityAlive)this.entity;
			this.entity.world.GetPrimaryPlayer();
		}
		this.trackingType = MapObjectZombie.TrackingTypes.None;
	}

	// Token: 0x06003439 RID: 13369 RVA: 0x0014EB12 File Offset: 0x0014CD12
	public override bool IsOnCompass()
	{
		return this.trackingType > MapObjectZombie.TrackingTypes.None;
	}

	// Token: 0x0600343A RID: 13370 RVA: 0x0014EB1D File Offset: 0x0014CD1D
	public override string GetMapIcon()
	{
		if (this.trackingType != MapObjectZombie.TrackingTypes.Tracking)
		{
			return this.entity.GetMapIcon();
		}
		return this.entity.GetTrackerIcon();
	}

	// Token: 0x0600343B RID: 13371 RVA: 0x0014EB3F File Offset: 0x0014CD3F
	public override string GetCompassIcon()
	{
		if (this.trackingType != MapObjectZombie.TrackingTypes.Tracking)
		{
			return this.entity.GetCompassIcon();
		}
		return this.entity.GetTrackerIcon();
	}

	// Token: 0x0600343C RID: 13372 RVA: 0x0014EB61 File Offset: 0x0014CD61
	public override bool UseUpDownCompassIcons()
	{
		return this.trackingType == MapObjectZombie.TrackingTypes.Quest;
	}

	// Token: 0x0600343D RID: 13373 RVA: 0x0014EB61 File Offset: 0x0014CD61
	public override bool IsCompassIconClamped()
	{
		return this.trackingType == MapObjectZombie.TrackingTypes.Quest;
	}

	// Token: 0x0600343E RID: 13374 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool NearbyCompassBlink()
	{
		return true;
	}

	// Token: 0x0600343F RID: 13375 RVA: 0x0014EB6C File Offset: 0x0014CD6C
	public override bool IsMapIconEnabled()
	{
		return this.trackingType == MapObjectZombie.TrackingTypes.Tracking;
	}

	// Token: 0x06003440 RID: 13376 RVA: 0x00041024 File Offset: 0x0003F224
	public override float GetMaxCompassIconScale()
	{
		return 1f;
	}

	// Token: 0x06003441 RID: 13377 RVA: 0x0014E211 File Offset: 0x0014C411
	public override float GetMinCompassIconScale()
	{
		return 0.6f;
	}

	// Token: 0x06003442 RID: 13378 RVA: 0x0014EB77 File Offset: 0x0014CD77
	public override Color GetMapIconColor()
	{
		if (this.trackingType != MapObjectZombie.TrackingTypes.Quest)
		{
			return this.entity.GetMapIconColor();
		}
		return Color.red;
	}

	// Token: 0x06003443 RID: 13379 RVA: 0x0014EB93 File Offset: 0x0014CD93
	public override float GetMaxCompassDistance()
	{
		return (float)((this.trackingType == MapObjectZombie.TrackingTypes.Quest) ? 32 : 100);
	}

	// Token: 0x06003444 RID: 13380 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool IsShowName()
	{
		return false;
	}

	// Token: 0x04002928 RID: 10536
	[PublicizedFrom(EAccessModifier.Private)]
	public MapObjectZombie.TrackingTypes trackingType;

	// Token: 0x02000648 RID: 1608
	[PublicizedFrom(EAccessModifier.Private)]
	public enum TrackingTypes
	{
		// Token: 0x0400292A RID: 10538
		None,
		// Token: 0x0400292B RID: 10539
		Tracking,
		// Token: 0x0400292C RID: 10540
		Quest
	}
}
