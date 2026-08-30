using System;
using UnityEngine;

// Token: 0x0200063E RID: 1598
public class MapObjectZombie : MapObject
{
	// Token: 0x060033E5 RID: 13285 RVA: 0x0014CB9A File Offset: 0x0014AD9A
	public MapObjectZombie(Entity _entity)
		: base(EnumMapObjectType.Entity, Vector3.zero, (long)_entity.entityId, _entity, false)
	{
	}

	// Token: 0x060033E6 RID: 13286 RVA: 0x0014CBB1 File Offset: 0x0014ADB1
	public MapObjectZombie(MapObjectZombie _other)
		: base(EnumMapObjectType.Entity, _other.position, (long)_other.entity.entityId, _other.entity, false)
	{
	}

	// Token: 0x060033E7 RID: 13287 RVA: 0x0014D572 File Offset: 0x0014B772
	public override void RefreshData()
	{
		if (this.type == EnumMapObjectType.Entity && this.entity != null)
		{
			EntityAlive entityAlive = (EntityAlive)this.entity;
			this.entity.world.GetPrimaryPlayer();
		}
		this.trackingType = MapObjectZombie.TrackingTypes.None;
	}

	// Token: 0x060033E8 RID: 13288 RVA: 0x0014D5AE File Offset: 0x0014B7AE
	public override bool IsOnCompass()
	{
		return this.trackingType > MapObjectZombie.TrackingTypes.None;
	}

	// Token: 0x060033E9 RID: 13289 RVA: 0x0014D5B9 File Offset: 0x0014B7B9
	public override string GetMapIcon()
	{
		if (this.trackingType != MapObjectZombie.TrackingTypes.Tracking)
		{
			return this.entity.GetMapIcon();
		}
		return this.entity.GetTrackerIcon();
	}

	// Token: 0x060033EA RID: 13290 RVA: 0x0014D5DB File Offset: 0x0014B7DB
	public override string GetCompassIcon()
	{
		if (this.trackingType != MapObjectZombie.TrackingTypes.Tracking)
		{
			return this.entity.GetCompassIcon();
		}
		return this.entity.GetTrackerIcon();
	}

	// Token: 0x060033EB RID: 13291 RVA: 0x0014D5FD File Offset: 0x0014B7FD
	public override bool UseUpDownCompassIcons()
	{
		return this.trackingType == MapObjectZombie.TrackingTypes.Quest;
	}

	// Token: 0x060033EC RID: 13292 RVA: 0x0014D5FD File Offset: 0x0014B7FD
	public override bool IsCompassIconClamped()
	{
		return this.trackingType == MapObjectZombie.TrackingTypes.Quest;
	}

	// Token: 0x060033ED RID: 13293 RVA: 0x00020021 File Offset: 0x0001E221
	public override bool NearbyCompassBlink()
	{
		return true;
	}

	// Token: 0x060033EE RID: 13294 RVA: 0x0014D608 File Offset: 0x0014B808
	public override bool IsMapIconEnabled()
	{
		return this.trackingType == MapObjectZombie.TrackingTypes.Tracking;
	}

	// Token: 0x060033EF RID: 13295 RVA: 0x00040F84 File Offset: 0x0003F184
	public override float GetMaxCompassIconScale()
	{
		return 1f;
	}

	// Token: 0x060033F0 RID: 13296 RVA: 0x0014CCAD File Offset: 0x0014AEAD
	public override float GetMinCompassIconScale()
	{
		return 0.6f;
	}

	// Token: 0x060033F1 RID: 13297 RVA: 0x0014D613 File Offset: 0x0014B813
	public override Color GetMapIconColor()
	{
		if (this.trackingType != MapObjectZombie.TrackingTypes.Quest)
		{
			return this.entity.GetMapIconColor();
		}
		return Color.red;
	}

	// Token: 0x060033F2 RID: 13298 RVA: 0x0014D62F File Offset: 0x0014B82F
	public override float GetMaxCompassDistance()
	{
		return (float)((this.trackingType == MapObjectZombie.TrackingTypes.Quest) ? 32 : 100);
	}

	// Token: 0x060033F3 RID: 13299 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool IsShowName()
	{
		return false;
	}

	// Token: 0x04002914 RID: 10516
	[PublicizedFrom(EAccessModifier.Private)]
	public MapObjectZombie.TrackingTypes trackingType;

	// Token: 0x0200063F RID: 1599
	[PublicizedFrom(EAccessModifier.Private)]
	public enum TrackingTypes
	{
		// Token: 0x04002916 RID: 10518
		None,
		// Token: 0x04002917 RID: 10519
		Tracking,
		// Token: 0x04002918 RID: 10520
		Quest
	}
}
