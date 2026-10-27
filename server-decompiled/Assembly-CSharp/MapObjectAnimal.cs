using System;
using UnityEngine;

// Token: 0x02000636 RID: 1590
public class MapObjectAnimal : MapObject
{
	// Token: 0x06003394 RID: 13204 RVA: 0x0014E0FE File Offset: 0x0014C2FE
	public MapObjectAnimal(Entity _entity)
		: base(EnumMapObjectType.Entity, Vector3.zero, (long)_entity.entityId, _entity, false)
	{
	}

	// Token: 0x06003395 RID: 13205 RVA: 0x0014E115 File Offset: 0x0014C315
	public MapObjectAnimal(MapObjectAnimal _other)
		: base(EnumMapObjectType.Entity, _other.position, (long)_other.entity.entityId, _other.entity, false)
	{
	}

	// Token: 0x06003396 RID: 13206 RVA: 0x0014E138 File Offset: 0x0014C338
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

	// Token: 0x06003397 RID: 13207 RVA: 0x0014E1C7 File Offset: 0x0014C3C7
	public override bool IsOnCompass()
	{
		return this.isTracked;
	}

	// Token: 0x06003398 RID: 13208 RVA: 0x0014E1CF File Offset: 0x0014C3CF
	public override string GetMapIcon()
	{
		if (!this.isTracked)
		{
			return this.entity.GetMapIcon();
		}
		return this.entity.GetTrackerIcon();
	}

	// Token: 0x06003399 RID: 13209 RVA: 0x0014E1F0 File Offset: 0x0014C3F0
	public override string GetCompassIcon()
	{
		if (!this.isTracked)
		{
			return this.entity.GetCompassIcon();
		}
		return this.entity.GetTrackerIcon();
	}

	// Token: 0x0600339A RID: 13210 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool NearbyCompassBlink()
	{
		return true;
	}

	// Token: 0x0600339B RID: 13211 RVA: 0x0014E1C7 File Offset: 0x0014C3C7
	public override bool IsMapIconEnabled()
	{
		return this.isTracked;
	}

	// Token: 0x0600339C RID: 13212 RVA: 0x00041024 File Offset: 0x0003F224
	public override float GetMaxCompassIconScale()
	{
		return 1f;
	}

	// Token: 0x0600339D RID: 13213 RVA: 0x0014E211 File Offset: 0x0014C411
	public override float GetMinCompassIconScale()
	{
		return 0.6f;
	}

	// Token: 0x0600339E RID: 13214 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool UseUpDownCompassIcons()
	{
		return false;
	}

	// Token: 0x0600339F RID: 13215 RVA: 0x0014E218 File Offset: 0x0014C418
	public override float GetMaxCompassDistance()
	{
		return 500f;
	}

	// Token: 0x060033A0 RID: 13216 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool IsShowName()
	{
		return false;
	}

	// Token: 0x0400290F RID: 10511
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isTracked;
}
