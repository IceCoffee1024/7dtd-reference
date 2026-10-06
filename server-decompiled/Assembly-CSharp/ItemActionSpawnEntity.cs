using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020005D0 RID: 1488
[Preserve]
public class ItemActionSpawnEntity : ItemAction
{
	// Token: 0x060030AF RID: 12463 RVA: 0x00139441 File Offset: 0x00137641
	public override ItemActionData CreateModifierData(ItemInventoryData _invData, int _indexInEntityOfAction)
	{
		return new ItemActionSpawnEntity.ItemActionDataSpawnEntity(_invData, _indexInEntityOfAction);
	}

	// Token: 0x060030B0 RID: 12464 RVA: 0x0013944C File Offset: 0x0013764C
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

	// Token: 0x060030B1 RID: 12465 RVA: 0x000027FC File Offset: 0x000009FC
	public override void StartHolding(ItemActionData _actionData)
	{
	}

	// Token: 0x060030B2 RID: 12466 RVA: 0x000027FC File Offset: 0x000009FC
	public override void StopHolding(ItemActionData _actionData)
	{
	}

	// Token: 0x060030B3 RID: 12467 RVA: 0x001394C8 File Offset: 0x001376C8
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

	// Token: 0x060030B4 RID: 12468 RVA: 0x00139523 File Offset: 0x00137723
	public override void CancelAction(ItemActionData _actionData)
	{
		((ItemActionSpawnEntity.ItemActionDataSpawnEntity)_actionData).state = ItemActionSpawnEntity.ItemActionDataSpawnEntity.State.None;
	}

	// Token: 0x060030B5 RID: 12469 RVA: 0x00139534 File Offset: 0x00137734
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

	// Token: 0x060030B6 RID: 12470 RVA: 0x001395A9 File Offset: 0x001377A9
	public override bool IsActionRunning(ItemActionData _actionData)
	{
		return ((ItemActionSpawnEntity.ItemActionDataSpawnEntity)_actionData).state > ItemActionSpawnEntity.ItemActionDataSpawnEntity.State.None;
	}

	// Token: 0x060030B7 RID: 12471 RVA: 0x001395BC File Offset: 0x001377BC
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

	// Token: 0x04002595 RID: 9621
	[PublicizedFrom(EAccessModifier.Private)]
	public int animType;

	// Token: 0x04002596 RID: 9622
	[PublicizedFrom(EAccessModifier.Private)]
	public float animWait;

	// Token: 0x04002597 RID: 9623
	[PublicizedFrom(EAccessModifier.Private)]
	public string soundWarn;

	// Token: 0x04002598 RID: 9624
	[PublicizedFrom(EAccessModifier.Private)]
	public string soundAttack;

	// Token: 0x04002599 RID: 9625
	[PublicizedFrom(EAccessModifier.Private)]
	public string entityToSpawn;

	// Token: 0x0400259A RID: 9626
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 entityOffset;

	// Token: 0x020005D1 RID: 1489
	[PublicizedFrom(EAccessModifier.Protected)]
	public class ItemActionDataSpawnEntity : ItemActionAttackData
	{
		// Token: 0x060030B8 RID: 12472 RVA: 0x0012B224 File Offset: 0x00129424
		public ItemActionDataSpawnEntity(ItemInventoryData _invData, int _indexInEntityOfAction)
			: base(_invData, _indexInEntityOfAction)
		{
		}

		// Token: 0x0400259B RID: 9627
		public ItemActionSpawnEntity.ItemActionDataSpawnEntity.State state;

		// Token: 0x0400259C RID: 9628
		public float stateTime;

		// Token: 0x020005D2 RID: 1490
		public enum State
		{
			// Token: 0x0400259E RID: 9630
			None,
			// Token: 0x0400259F RID: 9631
			Anim,
			// Token: 0x040025A0 RID: 9632
			Spawn,
			// Token: 0x040025A1 RID: 9633
			End
		}
	}
}
