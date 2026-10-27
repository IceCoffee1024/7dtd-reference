using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000487 RID: 1159
[Preserve]
public abstract class EntityAnimal : EntityAlive
{
	// Token: 0x060023DD RID: 9181 RVA: 0x000D9D25 File Offset: 0x000D7F25
	public void SetDistressed(bool _isDistressed, float _minTime, float _maxTime, int _playerId)
	{
		this.isDistressed = _isDistressed;
		this.minStressTime = _minTime;
		this.maxStressTime = _maxTime;
		this.playerId = _playerId;
		this.timer = 2.5f;
	}

	// Token: 0x060023DE RID: 9182 RVA: 0x000D9D50 File Offset: 0x000D7F50
	public override void ClearDistressed()
	{
		this.isDistressed = false;
		EntityPlayerLocal entityPlayerLocal = this.getEntityPlayerLocal();
		if (entityPlayerLocal)
		{
			entityPlayerLocal.Waypoints.TryRemoveLastKnownPositionWaypoint(this.entityId);
		}
	}

	// Token: 0x060023DF RID: 9183 RVA: 0x000D9D88 File Offset: 0x000D7F88
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
				GameManager.Instance.PlaySoundAtPositionServer(this.position, base.GetSoundDistressed(), AudioRolloffMode.Linear, 1, this.entityId, 1f);
			}
			EntityPlayerLocal entityPlayerLocal = this.getEntityPlayerLocal();
			if (entityPlayerLocal != null)
			{
				entityPlayerLocal.Waypoints.UpdateEntityAnimalWayPoint(this, true);
			}
		}
	}

	// Token: 0x060023E0 RID: 9184 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool IsDrawMapIcon()
	{
		return false;
	}

	// Token: 0x060023E1 RID: 9185 RVA: 0x000D9E3F File Offset: 0x000D803F
	public override Color GetMapIconColor()
	{
		return new Color(1f, 0.8235294f, 0.34117648f);
	}

	// Token: 0x060023E2 RID: 9186 RVA: 0x000D9E55 File Offset: 0x000D8055
	[PublicizedFrom(EAccessModifier.Protected)]
	public override float getNextStepSoundDistance()
	{
		return 0.8f;
	}

	// Token: 0x060023E3 RID: 9187 RVA: 0x00010E62 File Offset: 0x0000F062
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool isGameMessageOnDeath()
	{
		return false;
	}

	// Token: 0x060023E4 RID: 9188 RVA: 0x000D9E5C File Offset: 0x000D805C
	public override int DamageEntity(DamageSource _damageSource, int _strength, bool _criticalHit, float impulseScale)
	{
		return base.DamageEntity(_damageSource, _strength, _criticalHit, impulseScale);
	}

	// Token: 0x060023E5 RID: 9189 RVA: 0x000D9E6C File Offset: 0x000D806C
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

	// Token: 0x060023E6 RID: 9190 RVA: 0x000D9EC0 File Offset: 0x000D80C0
	public override void OnEntityUnload()
	{
		base.OnEntityUnload();
		EntityPlayerLocal entityPlayerLocal = this.getEntityPlayerLocal();
		if (entityPlayerLocal)
		{
			entityPlayerLocal.Waypoints.TryRemoveLastKnownPositionWaypoint(this.entityId);
		}
	}

	// Token: 0x060023E7 RID: 9191 RVA: 0x000D9EF4 File Offset: 0x000D80F4
	public override void OnCollectServer(int _playerId)
	{
		GameManager.Instance.World.RemoveEntity(this.entityId, EnumRemoveEntityReason.Captured);
	}

	// Token: 0x060023E8 RID: 9192 RVA: 0x000D9F10 File Offset: 0x000D8110
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

	// Token: 0x060023E9 RID: 9193 RVA: 0x000D9F46 File Offset: 0x000D8146
	[PublicizedFrom(EAccessModifier.Protected)]
	public EntityAnimal()
	{
	}

	// Token: 0x0400198D RID: 6541
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float minStressTime = 2.5f;

	// Token: 0x0400198E RID: 6542
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float maxStressTime = 1f;

	// Token: 0x0400198F RID: 6543
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool isDistressed;

	// Token: 0x04001990 RID: 6544
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int playerId = -1;

	// Token: 0x04001991 RID: 6545
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float timer;
}
