using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000485 RID: 1157
[Preserve]
public abstract class EntityAnimal : EntityAlive
{
	// Token: 0x060023D7 RID: 9175 RVA: 0x000D99A1 File Offset: 0x000D7BA1
	public void SetDistressed(bool _isDistressed, float _minTime, float _maxTime, int _playerId)
	{
		this.isDistressed = _isDistressed;
		this.minStressTime = _minTime;
		this.maxStressTime = _maxTime;
		this.playerId = _playerId;
		this.timer = 2.5f;
	}

	// Token: 0x060023D8 RID: 9176 RVA: 0x000D99CC File Offset: 0x000D7BCC
	public override void ClearDistressed()
	{
		this.isDistressed = false;
		EntityPlayerLocal entityPlayerLocal = this.getEntityPlayerLocal();
		if (entityPlayerLocal)
		{
			entityPlayerLocal.Waypoints.TryRemoveLastKnownPositionWaypoint(this.entityId);
		}
	}

	// Token: 0x060023D9 RID: 9177 RVA: 0x000D9A04 File Offset: 0x000D7C04
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

	// Token: 0x060023DA RID: 9178 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool IsDrawMapIcon()
	{
		return false;
	}

	// Token: 0x060023DB RID: 9179 RVA: 0x000D9ABB File Offset: 0x000D7CBB
	public override Color GetMapIconColor()
	{
		return new Color(1f, 0.8235294f, 0.34117648f);
	}

	// Token: 0x060023DC RID: 9180 RVA: 0x000D9AD1 File Offset: 0x000D7CD1
	[PublicizedFrom(EAccessModifier.Protected)]
	public override float getNextStepSoundDistance()
	{
		return 0.8f;
	}

	// Token: 0x060023DD RID: 9181 RVA: 0x00010E62 File Offset: 0x0000F062
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool isGameMessageOnDeath()
	{
		return false;
	}

	// Token: 0x060023DE RID: 9182 RVA: 0x000D9AD8 File Offset: 0x000D7CD8
	public override int DamageEntity(DamageSource _damageSource, int _strength, bool _criticalHit, float impulseScale)
	{
		return base.DamageEntity(_damageSource, _strength, _criticalHit, impulseScale);
	}

	// Token: 0x060023DF RID: 9183 RVA: 0x000D9AE8 File Offset: 0x000D7CE8
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

	// Token: 0x060023E0 RID: 9184 RVA: 0x000D9B3C File Offset: 0x000D7D3C
	public override void OnEntityUnload()
	{
		base.OnEntityUnload();
		EntityPlayerLocal entityPlayerLocal = this.getEntityPlayerLocal();
		if (entityPlayerLocal)
		{
			entityPlayerLocal.Waypoints.TryRemoveLastKnownPositionWaypoint(this.entityId);
		}
	}

	// Token: 0x060023E1 RID: 9185 RVA: 0x000D9B70 File Offset: 0x000D7D70
	public override void OnCollectServer(int _playerId)
	{
		GameManager.Instance.World.RemoveEntity(this.entityId, EnumRemoveEntityReason.Captured);
	}

	// Token: 0x060023E2 RID: 9186 RVA: 0x000D9B8C File Offset: 0x000D7D8C
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

	// Token: 0x060023E3 RID: 9187 RVA: 0x000D9BC2 File Offset: 0x000D7DC2
	[PublicizedFrom(EAccessModifier.Protected)]
	public EntityAnimal()
	{
	}

	// Token: 0x0400197D RID: 6525
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float minStressTime = 2.5f;

	// Token: 0x0400197E RID: 6526
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float maxStressTime = 1f;

	// Token: 0x0400197F RID: 6527
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool isDistressed;

	// Token: 0x04001980 RID: 6528
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int playerId = -1;

	// Token: 0x04001981 RID: 6529
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float timer;
}
