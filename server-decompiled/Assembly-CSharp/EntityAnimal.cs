using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000483 RID: 1155
[Preserve]
public abstract class EntityAnimal : EntityAlive
{
	// Token: 0x060023E4 RID: 9188 RVA: 0x000D9885 File Offset: 0x000D7A85
	public void SetDistressed(bool _isDistressed, float _minTime, float _maxTime, int _playerId)
	{
		this.isDistressed = _isDistressed;
		this.minStressTime = _minTime;
		this.maxStressTime = _maxTime;
		this.playerId = _playerId;
		this.timer = 2.5f;
	}

	// Token: 0x060023E5 RID: 9189 RVA: 0x000D98B0 File Offset: 0x000D7AB0
	public override void ClearDistressed()
	{
		this.isDistressed = false;
		EntityPlayerLocal entityPlayerLocal = this.getEntityPlayerLocal();
		if (entityPlayerLocal)
		{
			entityPlayerLocal.Waypoints.TryRemoveLastKnownPositionWaypoint(this.entityId);
		}
	}

	// Token: 0x060023E6 RID: 9190 RVA: 0x000D98E8 File Offset: 0x000D7AE8
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

	// Token: 0x060023E7 RID: 9191 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool IsDrawMapIcon()
	{
		return false;
	}

	// Token: 0x060023E8 RID: 9192 RVA: 0x000D999F File Offset: 0x000D7B9F
	public override Color GetMapIconColor()
	{
		return new Color(1f, 0.8235294f, 0.34117648f);
	}

	// Token: 0x060023E9 RID: 9193 RVA: 0x000D99B5 File Offset: 0x000D7BB5
	[PublicizedFrom(EAccessModifier.Protected)]
	public override float getNextStepSoundDistance()
	{
		return 0.8f;
	}

	// Token: 0x060023EA RID: 9194 RVA: 0x00010E62 File Offset: 0x0000F062
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool isGameMessageOnDeath()
	{
		return false;
	}

	// Token: 0x060023EB RID: 9195 RVA: 0x000D99BC File Offset: 0x000D7BBC
	public override int DamageEntity(DamageSource _damageSource, int _strength, bool _criticalHit, float impulseScale)
	{
		return base.DamageEntity(_damageSource, _strength, _criticalHit, impulseScale);
	}

	// Token: 0x060023EC RID: 9196 RVA: 0x000D99CC File Offset: 0x000D7BCC
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

	// Token: 0x060023ED RID: 9197 RVA: 0x000D9A20 File Offset: 0x000D7C20
	public override void OnEntityUnload()
	{
		base.OnEntityUnload();
		EntityPlayerLocal entityPlayerLocal = this.getEntityPlayerLocal();
		if (entityPlayerLocal)
		{
			entityPlayerLocal.Waypoints.TryRemoveLastKnownPositionWaypoint(this.entityId);
		}
	}

	// Token: 0x060023EE RID: 9198 RVA: 0x000D9A54 File Offset: 0x000D7C54
	public override void OnCollectServer(int _playerId)
	{
		GameManager.Instance.World.RemoveEntity(this.entityId, EnumRemoveEntityReason.Captured);
	}

	// Token: 0x060023EF RID: 9199 RVA: 0x000D9A70 File Offset: 0x000D7C70
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

	// Token: 0x060023F0 RID: 9200 RVA: 0x000D9AA6 File Offset: 0x000D7CA6
	[PublicizedFrom(EAccessModifier.Protected)]
	public EntityAnimal()
	{
	}

	// Token: 0x04001986 RID: 6534
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float minStressTime = 2.5f;

	// Token: 0x04001987 RID: 6535
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float maxStressTime = 1f;

	// Token: 0x04001988 RID: 6536
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool isDistressed;

	// Token: 0x04001989 RID: 6537
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int playerId = -1;

	// Token: 0x0400198A RID: 6538
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float timer;
}
