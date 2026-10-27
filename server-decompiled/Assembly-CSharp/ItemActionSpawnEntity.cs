using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020005D7 RID: 1495
[Preserve]
public class ItemActionSpawnEntity : ItemAction
{
	// Token: 0x060030BD RID: 12477 RVA: 0x0013A035 File Offset: 0x00138235
	public override ItemActionData CreateModifierData(ItemInventoryData _invData, int _indexInEntityOfAction)
	{
		return new ItemActionSpawnEntity.ItemActionDataSpawnEntity(_invData, _indexInEntityOfAction);
	}

	// Token: 0x060030BE RID: 12478 RVA: 0x0013A040 File Offset: 0x00138240
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

	// Token: 0x060030BF RID: 12479 RVA: 0x000027FC File Offset: 0x000009FC
	public override void StartHolding(ItemActionData _actionData)
	{
	}

	// Token: 0x060030C0 RID: 12480 RVA: 0x000027FC File Offset: 0x000009FC
	public override void StopHolding(ItemActionData _actionData)
	{
	}

	// Token: 0x060030C1 RID: 12481 RVA: 0x0013A0BC File Offset: 0x001382BC
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

	// Token: 0x060030C2 RID: 12482 RVA: 0x0013A117 File Offset: 0x00138317
	public override void CancelAction(ItemActionData _actionData)
	{
		((ItemActionSpawnEntity.ItemActionDataSpawnEntity)_actionData).state = ItemActionSpawnEntity.ItemActionDataSpawnEntity.State.None;
	}

	// Token: 0x060030C3 RID: 12483 RVA: 0x0013A128 File Offset: 0x00138328
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

	// Token: 0x060030C4 RID: 12484 RVA: 0x0013A19D File Offset: 0x0013839D
	public override bool IsActionRunning(ItemActionData _actionData)
	{
		return ((ItemActionSpawnEntity.ItemActionDataSpawnEntity)_actionData).state > ItemActionSpawnEntity.ItemActionDataSpawnEntity.State.None;
	}

	// Token: 0x060030C5 RID: 12485 RVA: 0x0013A1B0 File Offset: 0x001383B0
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

	// Token: 0x040025B0 RID: 9648
	[PublicizedFrom(EAccessModifier.Private)]
	public int animType;

	// Token: 0x040025B1 RID: 9649
	[PublicizedFrom(EAccessModifier.Private)]
	public float animWait;

	// Token: 0x040025B2 RID: 9650
	[PublicizedFrom(EAccessModifier.Private)]
	public string soundWarn;

	// Token: 0x040025B3 RID: 9651
	[PublicizedFrom(EAccessModifier.Private)]
	public string soundAttack;

	// Token: 0x040025B4 RID: 9652
	[PublicizedFrom(EAccessModifier.Private)]
	public string entityToSpawn;

	// Token: 0x040025B5 RID: 9653
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 entityOffset;

	// Token: 0x020005D8 RID: 1496
	[PublicizedFrom(EAccessModifier.Protected)]
	public class ItemActionDataSpawnEntity : ItemActionAttackData
	{
		// Token: 0x060030C6 RID: 12486 RVA: 0x0012BE18 File Offset: 0x0012A018
		public ItemActionDataSpawnEntity(ItemInventoryData _invData, int _indexInEntityOfAction)
			: base(_invData, _indexInEntityOfAction)
		{
		}

		// Token: 0x040025B6 RID: 9654
		public ItemActionSpawnEntity.ItemActionDataSpawnEntity.State state;

		// Token: 0x040025B7 RID: 9655
		public float stateTime;

		// Token: 0x020005D9 RID: 1497
		public enum State
		{
			// Token: 0x040025B9 RID: 9657
			None,
			// Token: 0x040025BA RID: 9658
			Anim,
			// Token: 0x040025BB RID: 9659
			Spawn,
			// Token: 0x040025BC RID: 9660
			End
		}
	}
}
