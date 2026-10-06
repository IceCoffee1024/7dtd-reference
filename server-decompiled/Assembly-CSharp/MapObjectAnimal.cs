using System;
using UnityEngine;

// Token: 0x0200062F RID: 1583
public class MapObjectAnimal : MapObject
{
	// Token: 0x06003386 RID: 13190 RVA: 0x0014D50A File Offset: 0x0014B70A
	public MapObjectAnimal(Entity _entity)
		: base(EnumMapObjectType.Entity, Vector3.zero, (long)_entity.entityId, _entity, false)
	{
	}

	// Token: 0x06003387 RID: 13191 RVA: 0x0014D521 File Offset: 0x0014B721
	public MapObjectAnimal(MapObjectAnimal _other)
		: base(EnumMapObjectType.Entity, _other.position, (long)_other.entity.entityId, _other.entity, false)
	{
	}

	// Token: 0x06003388 RID: 13192 RVA: 0x0014D544 File Offset: 0x0014B744
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

	// Token: 0x06003389 RID: 13193 RVA: 0x0014D5D3 File Offset: 0x0014B7D3
	public override bool IsOnCompass()
	{
		return this.isTracked;
	}

	// Token: 0x0600338A RID: 13194 RVA: 0x0014D5DB File Offset: 0x0014B7DB
	public override string GetMapIcon()
	{
		if (!this.isTracked)
		{
			return this.entity.GetMapIcon();
		}
		return this.entity.GetTrackerIcon();
	}

	// Token: 0x0600338B RID: 13195 RVA: 0x0014D5FC File Offset: 0x0014B7FC
	public override string GetCompassIcon()
	{
		if (!this.isTracked)
		{
			return this.entity.GetCompassIcon();
		}
		return this.entity.GetTrackerIcon();
	}

	// Token: 0x0600338C RID: 13196 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool NearbyCompassBlink()
	{
		return true;
	}

	// Token: 0x0600338D RID: 13197 RVA: 0x0014D5D3 File Offset: 0x0014B7D3
	public override bool IsMapIconEnabled()
	{
		return this.isTracked;
	}

	// Token: 0x0600338E RID: 13198 RVA: 0x00041064 File Offset: 0x0003F264
	public override float GetMaxCompassIconScale()
	{
		return 1f;
	}

	// Token: 0x0600338F RID: 13199 RVA: 0x0014D61D File Offset: 0x0014B81D
	public override float GetMinCompassIconScale()
	{
		return 0.6f;
	}

	// Token: 0x06003390 RID: 13200 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool UseUpDownCompassIcons()
	{
		return false;
	}

	// Token: 0x06003391 RID: 13201 RVA: 0x0014D624 File Offset: 0x0014B824
	public override float GetMaxCompassDistance()
	{
		return 500f;
	}

	// Token: 0x06003392 RID: 13202 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool IsShowName()
	{
		return false;
	}

	// Token: 0x040028F4 RID: 10484
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isTracked;
}
