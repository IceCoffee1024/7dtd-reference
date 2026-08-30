using System;
using UnityEngine;

// Token: 0x0200062D RID: 1581
public class MapObjectAnimal : MapObject
{
	// Token: 0x06003343 RID: 13123 RVA: 0x0014CB9A File Offset: 0x0014AD9A
	public MapObjectAnimal(Entity _entity)
		: base(EnumMapObjectType.Entity, Vector3.zero, (long)_entity.entityId, _entity, false)
	{
	}

	// Token: 0x06003344 RID: 13124 RVA: 0x0014CBB1 File Offset: 0x0014ADB1
	public MapObjectAnimal(MapObjectAnimal _other)
		: base(EnumMapObjectType.Entity, _other.position, (long)_other.entity.entityId, _other.entity, false)
	{
	}

	// Token: 0x06003345 RID: 13125 RVA: 0x0014CBD4 File Offset: 0x0014ADD4
	public override void RefreshData()
	{
		if (this.type == EnumMapObjectType.Entity && this.entity != null)
		{
			if (!((EntityAlive)this.entity).IsAlive())
			{
				this.isTracked = false;
				return;
			}
			EntityPlayerLocal primaryPlayer = this.entity.world.GetPrimaryPlayer();
			if (primaryPlayer != null && EffectManager.GetValue(PassiveEffects.Tracking, null, 0f, primaryPlayer, null, this.entity.EntityTags, true, true, true, true, true, 1, true, false) > 0f)
			{
				this.isTracked = true;
				return;
			}
		}
		this.isTracked = false;
	}

	// Token: 0x06003346 RID: 13126 RVA: 0x0014CC63 File Offset: 0x0014AE63
	public override bool IsOnCompass()
	{
		return this.isTracked;
	}

	// Token: 0x06003347 RID: 13127 RVA: 0x0014CC6B File Offset: 0x0014AE6B
	public override string GetMapIcon()
	{
		if (!this.isTracked)
		{
			return this.entity.GetMapIcon();
		}
		return this.entity.GetTrackerIcon();
	}

	// Token: 0x06003348 RID: 13128 RVA: 0x0014CC8C File Offset: 0x0014AE8C
	public override string GetCompassIcon()
	{
		if (!this.isTracked)
		{
			return this.entity.GetCompassIcon();
		}
		return this.entity.GetTrackerIcon();
	}

	// Token: 0x06003349 RID: 13129 RVA: 0x00020021 File Offset: 0x0001E221
	public override bool NearbyCompassBlink()
	{
		return true;
	}

	// Token: 0x0600334A RID: 13130 RVA: 0x0014CC63 File Offset: 0x0014AE63
	public override bool IsMapIconEnabled()
	{
		return this.isTracked;
	}

	// Token: 0x0600334B RID: 13131 RVA: 0x00040F84 File Offset: 0x0003F184
	public override float GetMaxCompassIconScale()
	{
		return 1f;
	}

	// Token: 0x0600334C RID: 13132 RVA: 0x0014CCAD File Offset: 0x0014AEAD
	public override float GetMinCompassIconScale()
	{
		return 0.6f;
	}

	// Token: 0x0600334D RID: 13133 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool UseUpDownCompassIcons()
	{
		return false;
	}

	// Token: 0x0600334E RID: 13134 RVA: 0x0014CCB4 File Offset: 0x0014AEB4
	public override float GetMaxCompassDistance()
	{
		return 500f;
	}

	// Token: 0x0600334F RID: 13135 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool IsShowName()
	{
		return false;
	}

	// Token: 0x040028FB RID: 10491
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isTracked;
}
