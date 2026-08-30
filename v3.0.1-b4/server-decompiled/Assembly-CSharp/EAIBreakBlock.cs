using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000432 RID: 1074
[Preserve]
public class EAIBreakBlock : EAIBase
{
	// Token: 0x060020F1 RID: 8433 RVA: 0x000C6AA2 File Offset: 0x000C4CA2
	public override void Init(EntityAlive _theEntity)
	{
		base.Init(_theEntity);
		this.MutexBits = 8;
		this.executeDelay = 0.15f;
	}

	// Token: 0x060020F2 RID: 8434 RVA: 0x000C6AC0 File Offset: 0x000C4CC0
	public override bool CanExecute()
	{
		if (this.theEntity.bodyDamage.CurrentStun != EnumEntityStunType.None)
		{
			return false;
		}
		EntityMoveHelper moveHelper = this.theEntity.moveHelper;
		if (moveHelper.BlockedTime < 0.35f || !moveHelper.CanBreakBlocks)
		{
			return false;
		}
		if (this.theEntity.Jumping && !moveHelper.IsDestroyArea)
		{
			return false;
		}
		int num = ((this.theEntity.crouchType == 0 && this.theEntity.physicsHeight >= 1f) ? 7 : 5);
		if ((moveHelper.BlockedFlags & num) > 0)
		{
			Vector3i blockPos = moveHelper.HitInfo.hit.blockPos;
			if (this.theEntity.world.GetBlock(blockPos).isair)
			{
				return false;
			}
			float num2 = moveHelper.CalcBlockedDistanceSq();
			float num3 = this.theEntity.m_characterController.GetRadius() + 0.7f;
			if (num2 <= num3 * num3)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060020F3 RID: 8435 RVA: 0x000C6BA0 File Offset: 0x000C4DA0
	public override void Start()
	{
		this.attackDelay = 1;
		Vector3i blockPos = this.theEntity.moveHelper.HitInfo.hit.blockPos;
		Block block = this.theEntity.world.GetBlock(blockPos).Block;
		if (block.HasTag(BlockTags.Door) || block.HasTag(BlockTags.ClosetDoor))
		{
			this.theEntity.IsBreakingDoors = true;
		}
	}

	// Token: 0x060020F4 RID: 8436 RVA: 0x000C6C0A File Offset: 0x000C4E0A
	public override bool Continue()
	{
		return (this.theEntity.onGround || this.theEntity.IsInElevator()) && this.CanExecute();
	}

	// Token: 0x060020F5 RID: 8437 RVA: 0x000C6C2E File Offset: 0x000C4E2E
	public override void Update()
	{
		EntityMoveHelper moveHelper = this.theEntity.moveHelper;
		if (this.attackDelay > 0)
		{
			this.attackDelay--;
		}
		if (this.attackDelay <= 0)
		{
			this.AttackBlock();
		}
	}

	// Token: 0x060020F6 RID: 8438 RVA: 0x000C6C62 File Offset: 0x000C4E62
	public override void Reset()
	{
		this.theEntity.IsBreakingBlocks = false;
		this.theEntity.IsBreakingDoors = false;
	}

	// Token: 0x060020F7 RID: 8439 RVA: 0x000C6C7C File Offset: 0x000C4E7C
	[PublicizedFrom(EAccessModifier.Private)]
	public void AttackBlock()
	{
		this.theEntity.SetLookPosition(Vector3.zero);
		ItemActionAttackData itemActionAttackData = this.theEntity.inventory.holdingItemData.actionData[0] as ItemActionAttackData;
		if (itemActionAttackData == null)
		{
			return;
		}
		this.damageBoostPercent = 0f;
		if (this.theEntity is EntityZombie)
		{
			Bounds bounds = new Bounds(this.theEntity.position, new Vector3(1.7f, 1.5f, 1.7f));
			this.theEntity.world.GetEntitiesInBounds(typeof(EntityZombie), bounds, this.allies);
			for (int i = this.allies.Count - 1; i >= 0; i--)
			{
				if ((EntityZombie)this.allies[i] != this.theEntity)
				{
					this.damageBoostPercent += 0.2f;
				}
			}
			this.allies.Clear();
		}
		if (this.theEntity.Attack(false))
		{
			this.theEntity.IsBreakingBlocks = true;
			float num = 0.25f + base.RandomFloat * 0.8f;
			if (this.theEntity.moveHelper.IsUnreachableAbove)
			{
				num *= 0.5f;
			}
			this.attackDelay = (int)((num + 0.75f) * 20f);
			itemActionAttackData.hitDelegate = new ItemActionAttackData.HitDelegate(this.GetHitInfo);
			this.theEntity.Attack(true);
		}
	}

	// Token: 0x060020F8 RID: 8440 RVA: 0x000C6DF0 File Offset: 0x000C4FF0
	[PublicizedFrom(EAccessModifier.Private)]
	public WorldRayHitInfo GetHitInfo(out float damageScale)
	{
		EntityMoveHelper moveHelper = this.theEntity.moveHelper;
		damageScale = moveHelper.DamageScale + this.damageBoostPercent;
		return moveHelper.HitInfo;
	}

	// Token: 0x0400169A RID: 5786
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cDamageBoostPerAlly = 0.2f;

	// Token: 0x0400169B RID: 5787
	[PublicizedFrom(EAccessModifier.Private)]
	public int attackDelay;

	// Token: 0x0400169C RID: 5788
	[PublicizedFrom(EAccessModifier.Private)]
	public float damageBoostPercent;

	// Token: 0x0400169D RID: 5789
	[PublicizedFrom(EAccessModifier.Private)]
	public List<Entity> allies = new List<Entity>();
}
