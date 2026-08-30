using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200047F RID: 1151
[Preserve]
public abstract class EntityAnimal : EntityAlive
{
	// Token: 0x060023BE RID: 9150 RVA: 0x000D8CF5 File Offset: 0x000D6EF5
	public void SetDistressed(bool _isDistressed, float _minTime, float _maxTime, int _playerId)
	{
		this.isDistressed = _isDistressed;
		this.minStressTime = _minTime;
		this.maxStressTime = _maxTime;
		this.playerId = _playerId;
		this.timer = 0f;
	}

	// Token: 0x060023BF RID: 9151 RVA: 0x000D8D20 File Offset: 0x000D6F20
	public void ClearDistressed()
	{
		this.isDistressed = false;
		EntityPlayerLocal entityPlayerLocal = this.getEntityPlayerLocal();
		if (entityPlayerLocal)
		{
			entityPlayerLocal.Waypoints.TryRemoveLastKnownPositionWaypoint(this.entityId);
		}
	}

	// Token: 0x060023C0 RID: 9152 RVA: 0x000D8D58 File Offset: 0x000D6F58
	public override void OnUpdateLive()
	{
		base.GetEntitySenses().Clear();
		base.OnUpdateLive();
		if (this.isDistressed && base.IsAlive())
		{
			this.timer -= Time.deltaTime;
			if (this.timer <= 0f)
			{
				this.timer += this.rand.RandomRange(this.minStressTime, this.maxStressTime);
				GameManager.Instance.PlaySoundAtPositionServer(this.position, base.GetSoundHurt(), AudioRolloffMode.Linear, 1, this.entityId, 1f);
			}
			EntityPlayerLocal entityPlayerLocal = this.getEntityPlayerLocal();
			if (entityPlayerLocal != null)
			{
				entityPlayerLocal.Waypoints.UpdateEntityAnimalWayPoint(this, true);
			}
		}
	}

	// Token: 0x060023C1 RID: 9153 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool IsDrawMapIcon()
	{
		return false;
	}

	// Token: 0x060023C2 RID: 9154 RVA: 0x000D8E0F File Offset: 0x000D700F
	public override Color GetMapIconColor()
	{
		return new Color(1f, 0.8235294f, 0.34117648f);
	}

	// Token: 0x060023C3 RID: 9155 RVA: 0x000D8E25 File Offset: 0x000D7025
	[PublicizedFrom(EAccessModifier.Protected)]
	public override float getNextStepSoundDistance()
	{
		return 0.8f;
	}

	// Token: 0x060023C4 RID: 9156 RVA: 0x00010E62 File Offset: 0x0000F062
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool isGameMessageOnDeath()
	{
		return false;
	}

	// Token: 0x060023C5 RID: 9157 RVA: 0x000D8E2C File Offset: 0x000D702C
	public override int DamageEntity(DamageSource _damageSource, int _strength, bool _criticalHit, float impulseScale)
	{
		return base.DamageEntity(_damageSource, _strength, _criticalHit, impulseScale);
	}

	// Token: 0x060023C6 RID: 9158 RVA: 0x000D8E3C File Offset: 0x000D703C
	public override void OnEntityDeath()
	{
		if (this.PhysicsTransform)
		{
			this.PhysicsTransform.gameObject.SetActive(false);
		}
		base.OnEntityDeath();
		EntityPlayerLocal entityPlayerLocal = this.getEntityPlayerLocal();
		if (entityPlayerLocal)
		{
			entityPlayerLocal.Waypoints.TryRemoveLastKnownPositionWaypoint(this.entityId);
		}
	}

	// Token: 0x060023C7 RID: 9159 RVA: 0x000D8E90 File Offset: 0x000D7090
	public override void OnEntityUnload()
	{
		base.OnEntityUnload();
		EntityPlayerLocal entityPlayerLocal = this.getEntityPlayerLocal();
		if (entityPlayerLocal)
		{
			entityPlayerLocal.Waypoints.TryRemoveLastKnownPositionWaypoint(this.entityId);
		}
	}

	// Token: 0x060023C8 RID: 9160 RVA: 0x000D8EC4 File Offset: 0x000D70C4
	public override void OnCollectServer(int _playerId)
	{
		GameManager.Instance.World.RemoveEntity(this.entityId, EnumRemoveEntityReason.Captured);
	}

	// Token: 0x060023C9 RID: 9161 RVA: 0x000D8EE0 File Offset: 0x000D70E0
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityPlayerLocal getEntityPlayerLocal()
	{
		EntityPlayerLocal entityPlayerLocal = GameManager.Instance.World.GetPrimaryPlayer();
		if (entityPlayerLocal && entityPlayerLocal.entityId != this.playerId)
		{
			entityPlayerLocal = null;
		}
		return entityPlayerLocal;
	}

	// Token: 0x060023CA RID: 9162 RVA: 0x000D8F16 File Offset: 0x000D7116
	[PublicizedFrom(EAccessModifier.Protected)]
	public EntityAnimal()
	{
	}

	// Token: 0x0400196E RID: 6510
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float minStressTime = 2.5f;

	// Token: 0x0400196F RID: 6511
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float maxStressTime = 1f;

	// Token: 0x04001970 RID: 6512
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool isDistressed;

	// Token: 0x04001971 RID: 6513
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int playerId = -1;

	// Token: 0x04001972 RID: 6514
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float timer;
}
