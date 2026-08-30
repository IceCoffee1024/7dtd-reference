using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020005CA RID: 1482
[Preserve]
public class ItemActionSpawnEntity : ItemAction
{
	// Token: 0x0600303A RID: 12346 RVA: 0x001378F9 File Offset: 0x00135AF9
	public override ItemActionData CreateModifierData(ItemInventoryData _invData, int _indexInEntityOfAction)
	{
		return new ItemActionSpawnEntity.ItemActionDataSpawnEntity(_invData, _indexInEntityOfAction);
	}

	// Token: 0x0600303B RID: 12347 RVA: 0x00137904 File Offset: 0x00135B04
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

	// Token: 0x0600303C RID: 12348 RVA: 0x000027FC File Offset: 0x000009FC
	public override void StartHolding(ItemActionData _actionData)
	{
	}

	// Token: 0x0600303D RID: 12349 RVA: 0x000027FC File Offset: 0x000009FC
	public override void StopHolding(ItemActionData _actionData)
	{
	}

	// Token: 0x0600303E RID: 12350 RVA: 0x00137980 File Offset: 0x00135B80
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

	// Token: 0x0600303F RID: 12351 RVA: 0x001379DB File Offset: 0x00135BDB
	public override void CancelAction(ItemActionData _actionData)
	{
		((ItemActionSpawnEntity.ItemActionDataSpawnEntity)_actionData).state = ItemActionSpawnEntity.ItemActionDataSpawnEntity.State.None;
	}

	// Token: 0x06003040 RID: 12352 RVA: 0x001379EC File Offset: 0x00135BEC
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

	// Token: 0x06003041 RID: 12353 RVA: 0x00137A61 File Offset: 0x00135C61
	public override bool IsActionRunning(ItemActionData _actionData)
	{
		return ((ItemActionSpawnEntity.ItemActionDataSpawnEntity)_actionData).state > ItemActionSpawnEntity.ItemActionDataSpawnEntity.State.None;
	}

	// Token: 0x06003042 RID: 12354 RVA: 0x00137A74 File Offset: 0x00135C74
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

	// Token: 0x04002567 RID: 9575
	[PublicizedFrom(EAccessModifier.Private)]
	public int animType;

	// Token: 0x04002568 RID: 9576
	[PublicizedFrom(EAccessModifier.Private)]
	public float animWait;

	// Token: 0x04002569 RID: 9577
	[PublicizedFrom(EAccessModifier.Private)]
	public string soundWarn;

	// Token: 0x0400256A RID: 9578
	[PublicizedFrom(EAccessModifier.Private)]
	public string soundAttack;

	// Token: 0x0400256B RID: 9579
	[PublicizedFrom(EAccessModifier.Private)]
	public string entityToSpawn;

	// Token: 0x0400256C RID: 9580
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 entityOffset;

	// Token: 0x020005CB RID: 1483
	[PublicizedFrom(EAccessModifier.Protected)]
	public class ItemActionDataSpawnEntity : ItemActionAttackData
	{
		// Token: 0x06003043 RID: 12355 RVA: 0x00129694 File Offset: 0x00127894
		public ItemActionDataSpawnEntity(ItemInventoryData _invData, int _indexInEntityOfAction)
			: base(_invData, _indexInEntityOfAction)
		{
		}

		// Token: 0x0400256D RID: 9581
		public ItemActionSpawnEntity.ItemActionDataSpawnEntity.State state;

		// Token: 0x0400256E RID: 9582
		public float stateTime;

		// Token: 0x020005CC RID: 1484
		public enum State
		{
			// Token: 0x04002570 RID: 9584
			None,
			// Token: 0x04002571 RID: 9585
			Anim,
			// Token: 0x04002572 RID: 9586
			Spawn,
			// Token: 0x04002573 RID: 9587
			End
		}
	}
}
