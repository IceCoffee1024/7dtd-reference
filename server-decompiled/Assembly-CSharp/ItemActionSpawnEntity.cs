using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020005CF RID: 1487
[Preserve]
public class ItemActionSpawnEntity : ItemAction
{
	// Token: 0x06003068 RID: 12392 RVA: 0x00138969 File Offset: 0x00136B69
	public override ItemActionData CreateModifierData(ItemInventoryData _invData, int _indexInEntityOfAction)
	{
		return new ItemActionSpawnEntity.ItemActionDataSpawnEntity(_invData, _indexInEntityOfAction);
	}

	// Token: 0x06003069 RID: 12393 RVA: 0x00138974 File Offset: 0x00136B74
	public override void ReadFrom(DynamicProperties _props)
	{
		base.ReadFrom(_props);
		this.animType = _props.GetInt("AnimType");
		this.animWait = _props.GetFloat("AnimWait");
		this.soundWarn = _props.GetString("SoundWarn");
		this.soundAttack = _props.GetString("SoundAttack");
		this.entityToSpawn = _props.GetString("Entity");
		_props.ParseVec("EntityOffset", ref this.entityOffset);
	}

	// Token: 0x0600306A RID: 12394 RVA: 0x000027FC File Offset: 0x000009FC
	public override void StartHolding(ItemActionData _actionData)
	{
	}

	// Token: 0x0600306B RID: 12395 RVA: 0x000027FC File Offset: 0x000009FC
	public override void StopHolding(ItemActionData _actionData)
	{
	}

	// Token: 0x0600306C RID: 12396 RVA: 0x001389F0 File Offset: 0x00136BF0
	public override void OnHoldingUpdate(ItemActionData _actionData)
	{
		ItemActionSpawnEntity.ItemActionDataSpawnEntity itemActionDataSpawnEntity = (ItemActionSpawnEntity.ItemActionDataSpawnEntity)_actionData;
		itemActionDataSpawnEntity.stateTime += 0.05f;
		ItemActionSpawnEntity.ItemActionDataSpawnEntity.State state = itemActionDataSpawnEntity.state;
		if (state != ItemActionSpawnEntity.ItemActionDataSpawnEntity.State.Anim)
		{
			if (state != ItemActionSpawnEntity.ItemActionDataSpawnEntity.State.Spawn)
			{
				return;
			}
			this.Spawn(itemActionDataSpawnEntity);
			itemActionDataSpawnEntity.state = ItemActionSpawnEntity.ItemActionDataSpawnEntity.State.End;
			return;
		}
		else
		{
			if (itemActionDataSpawnEntity.stateTime < this.animWait)
			{
				return;
			}
			itemActionDataSpawnEntity.state = ItemActionSpawnEntity.ItemActionDataSpawnEntity.State.Spawn;
			return;
		}
	}

	// Token: 0x0600306D RID: 12397 RVA: 0x00138A4B File Offset: 0x00136C4B
	public override void CancelAction(ItemActionData _actionData)
	{
		((ItemActionSpawnEntity.ItemActionDataSpawnEntity)_actionData).state = ItemActionSpawnEntity.ItemActionDataSpawnEntity.State.None;
	}

	// Token: 0x0600306E RID: 12398 RVA: 0x00138A5C File Offset: 0x00136C5C
	public override void ExecuteAction(ItemActionData _actionData, bool _bReleased)
	{
		EntityAlive holdingEntity = _actionData.invData.holdingEntity;
		if (!holdingEntity)
		{
			return;
		}
		ItemActionSpawnEntity.ItemActionDataSpawnEntity itemActionDataSpawnEntity = (ItemActionSpawnEntity.ItemActionDataSpawnEntity)_actionData;
		if (!_bReleased)
		{
			if (itemActionDataSpawnEntity.state == ItemActionSpawnEntity.ItemActionDataSpawnEntity.State.None)
			{
				itemActionDataSpawnEntity.state = ItemActionSpawnEntity.ItemActionDataSpawnEntity.State.Anim;
				itemActionDataSpawnEntity.stateTime = 0f;
				holdingEntity.StartAnimAction(this.animType + 3000);
				holdingEntity.PlayOneShot(this.soundWarn, false, false, false, null, 1f);
			}
			return;
		}
		itemActionDataSpawnEntity.state = ItemActionSpawnEntity.ItemActionDataSpawnEntity.State.None;
	}

	// Token: 0x0600306F RID: 12399 RVA: 0x00138AD1 File Offset: 0x00136CD1
	public override bool IsActionRunning(ItemActionData _actionData)
	{
		return ((ItemActionSpawnEntity.ItemActionDataSpawnEntity)_actionData).state > ItemActionSpawnEntity.ItemActionDataSpawnEntity.State.None;
	}

	// Token: 0x06003070 RID: 12400 RVA: 0x00138AE4 File Offset: 0x00136CE4
	[PublicizedFrom(EAccessModifier.Private)]
	public void Spawn(ItemActionData _actionData)
	{
		EntityAlive holdingEntity = _actionData.invData.holdingEntity;
		if (!holdingEntity)
		{
			return;
		}
		if (holdingEntity.IsAttackValid())
		{
			Vector3 vector = holdingEntity.getHeadPosition();
			vector += holdingEntity.qrotation * this.entityOffset;
			Entity entity = EntityFactory.CreateEntity(EntityClass.GetId(this.entityToSpawn), vector, new Vector3(0f, holdingEntity.rotation.y, 0f));
			entity.SetSpawnerSource(EnumSpawnerSource.StaticSpawner);
			GameManager.Instance.World.SpawnEntityInWorld(entity);
			EntityAlive entityAlive = entity as EntityAlive;
			if (entityAlive != null)
			{
				entityAlive.SetAttackTarget(holdingEntity.GetAttackTarget(), 600);
			}
			holdingEntity.PlayOneShot(this.soundAttack, false, false, false, null, 1f);
		}
	}

	// Token: 0x0400259B RID: 9627
	[PublicizedFrom(EAccessModifier.Private)]
	public int animType;

	// Token: 0x0400259C RID: 9628
	[PublicizedFrom(EAccessModifier.Private)]
	public float animWait;

	// Token: 0x0400259D RID: 9629
	[PublicizedFrom(EAccessModifier.Private)]
	public string soundWarn;

	// Token: 0x0400259E RID: 9630
	[PublicizedFrom(EAccessModifier.Private)]
	public string soundAttack;

	// Token: 0x0400259F RID: 9631
	[PublicizedFrom(EAccessModifier.Private)]
	public string entityToSpawn;

	// Token: 0x040025A0 RID: 9632
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 entityOffset;

	// Token: 0x020005D0 RID: 1488
	[PublicizedFrom(EAccessModifier.Protected)]
	public class ItemActionDataSpawnEntity : ItemActionAttackData
	{
		// Token: 0x06003071 RID: 12401 RVA: 0x0012A704 File Offset: 0x00128904
		public ItemActionDataSpawnEntity(ItemInventoryData _invData, int _indexInEntityOfAction)
			: base(_invData, _indexInEntityOfAction)
		{
		}

		// Token: 0x040025A1 RID: 9633
		public ItemActionSpawnEntity.ItemActionDataSpawnEntity.State state;

		// Token: 0x040025A2 RID: 9634
		public float stateTime;

		// Token: 0x020005D1 RID: 1489
		public enum State
		{
			// Token: 0x040025A4 RID: 9636
			None,
			// Token: 0x040025A5 RID: 9637
			Anim,
			// Token: 0x040025A6 RID: 9638
			Spawn,
			// Token: 0x040025A7 RID: 9639
			End
		}
	}
}
