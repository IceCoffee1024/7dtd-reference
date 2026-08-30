using System;
using UnityEngine;

// Token: 0x02000624 RID: 1572
public class MapObjectAnimal : MapObject
{
	// Token: 0x060032F0 RID: 13040 RVA: 0x0014AA7E File Offset: 0x00148C7E
	public MapObjectAnimal(Entity _entity)
		: base(EnumMapObjectType.Entity, Vector3.zero, (long)_entity.entityId, _entity, false)
	{
	}

	// Token: 0x060032F1 RID: 13041 RVA: 0x0014AA95 File Offset: 0x00148C95
	public MapObjectAnimal(MapObjectAnimal _other)
		: base(EnumMapObjectType.Entity, _other.position, (long)_other.entity.entityId, _other.entity, false)
	{
	}

	// Token: 0x060032F2 RID: 13042 RVA: 0x0014AAB8 File Offset: 0x00148CB8
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

	// Token: 0x060032F3 RID: 13043 RVA: 0x0014AB47 File Offset: 0x00148D47
	public override bool IsOnCompass()
	{
		return this.isTracked;
	}

	// Token: 0x060032F4 RID: 13044 RVA: 0x0014AB4F File Offset: 0x00148D4F
	public override string GetMapIcon()
	{
		if (!this.isTracked)
		{
			return this.entity.GetMapIcon();
		}
		return this.entity.GetTrackerIcon();
	}

	// Token: 0x060032F5 RID: 13045 RVA: 0x0014AB70 File Offset: 0x00148D70
	public override string GetCompassIcon()
	{
		if (!this.isTracked)
		{
			return this.entity.GetCompassIcon();
		}
		return this.entity.GetTrackerIcon();
	}

	// Token: 0x060032F6 RID: 13046 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool NearbyCompassBlink()
	{
		return true;
	}

	// Token: 0x060032F7 RID: 13047 RVA: 0x0014AB47 File Offset: 0x00148D47
	public override bool IsMapIconEnabled()
	{
		return this.isTracked;
	}

	// Token: 0x060032F8 RID: 13048 RVA: 0x00040D24 File Offset: 0x0003EF24
	public override float GetMaxCompassIconScale()
	{
		return 1f;
	}

	// Token: 0x060032F9 RID: 13049 RVA: 0x0014AB91 File Offset: 0x00148D91
	public override float GetMinCompassIconScale()
	{
		return 0.6f;
	}

	// Token: 0x060032FA RID: 13050 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool UseUpDownCompassIcons()
	{
		return false;
	}

	// Token: 0x060032FB RID: 13051 RVA: 0x0014AB98 File Offset: 0x00148D98
	public override float GetMaxCompassDistance()
	{
		return 500f;
	}

	// Token: 0x060032FC RID: 13052 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool IsShowName()
	{
		return false;
	}

	// Token: 0x04002888 RID: 10376
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isTracked;
}
