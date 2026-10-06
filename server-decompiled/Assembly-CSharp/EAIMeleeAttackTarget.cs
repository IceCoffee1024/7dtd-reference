using System;
using System.Collections.Generic;
using Audio;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200044D RID: 1101
[Preserve]
public class EAIMeleeAttackTarget : EAIBase
{
	// Token: 0x06002183 RID: 8579 RVA: 0x000CACDC File Offset: 0x000C8EDC
	public override void Init(EntityAlive _theEntity)
	{
		base.Init(_theEntity);
		this.MutexBits = 11;
		this.cooldown = 3f;
		this.attackDuration = 20f;
	}

	// Token: 0x06002184 RID: 8580 RVA: 0x000CAD04 File Offset: 0x000C8F04
	public override void SetData(Dictionary<string, string> data)
	{
		base.SetData(data);
		base.GetData(data, "slot", ref this.inventorySlot);
		base.GetData(data, "itemType", ref this.itemActionType);
		base.GetData(data, "startAnimType", ref this.startAnimType);
		base.GetData(data, "releaseDelay", ref this.releaseDelay);
		base.GetData(data, "cooldown", ref this.baseCooldown);
		base.GetData(data, "duration", ref this.attackDuration);
		base.GetData(data, "minRange", ref this.minRange);
		base.GetData(data, "maxRange", ref this.maxRange);
		base.GetData(data, "unreachableRange", ref this.unreachableRange);
		data.TryGetValue("sndStart", out this.soundStartName);
		data.TryGetValue("sndRelease", out this.soundReleaseName);
	}

	// Token: 0x06002185 RID: 8581 RVA: 0x000CADE0 File Offset: 0x000C8FE0
	public override bool CanExecute()
	{
		if (this.theEntity.IsDancing)
		{
			return false;
		}
		if (this.cooldown > 0f)
		{
			this.cooldown -= this.executeWaitTime;
			return false;
		}
		if (!this.theEntity.IsAttackValid())
		{
			return false;
		}
		this.entityTarget = this.theEntity.GetAttackTarget();
		return this.entityTarget && !this.entityTarget.IsDead() && !this.theEntity.bodyDamage.IsAnyLegMissing && (this.startAnimType < 0 || !this.theEntity.bodyDamage.IsAnyArmOrLegMissing) && this.InRange() && this.theEntity.CanSee(this.entityTarget);
	}

	// Token: 0x06002186 RID: 8582 RVA: 0x000CAEA8 File Offset: 0x000C90A8
	public override void Start()
	{
		this.theEntity.emodel.avatarController.hitWeightMax = 0.5f;
		this.painHitsFelt = this.theEntity.painHitsFelt;
		this.elapsedTime = 0f;
		this.state = EAIMeleeAttackTarget.State.Attack;
		this.stateTime = 0f;
		if (this.startAnimType >= 0)
		{
			this.state = EAIMeleeAttackTarget.State.StartAnim;
			this.theEntity.StartAnimAction(this.startAnimType + 3000);
		}
		if (!string.IsNullOrEmpty(this.soundStartName))
		{
			Manager.BroadcastPlay(this.theEntity, this.soundStartName, false, 1f);
		}
		this.theEntity.inventory.SetHoldingItemIdx(this.inventorySlot);
	}

	// Token: 0x06002187 RID: 8583 RVA: 0x000CAF60 File Offset: 0x000C9160
	public override bool Continue()
	{
		return this.entityTarget && !this.entityTarget.IsDead() && this.elapsedTime < this.attackDuration && this.theEntity.bodyDamage.CurrentStun == EnumEntityStunType.None && !this.theEntity.Electrocuted && (this.startAnimType < 0 || !this.theEntity.bodyDamage.IsAnyArmOrLegMissing) && this.InRange() && this.theEntity.painHitsFelt - this.painHitsFelt < 1f;
	}

	// Token: 0x06002188 RID: 8584 RVA: 0x000CAFF8 File Offset: 0x000C91F8
	public override void Update()
	{
		this.elapsedTime += 0.05f;
		if (this.elapsedTime < this.attackDuration * 0.5f)
		{
			Vector3 headPosition = this.entityTarget.getHeadPosition();
			if (this.theEntity.IsInFrontOfMe(headPosition))
			{
				this.theEntity.SetLookPosition(headPosition);
			}
			this.theEntity.SeekYawToPos(this.entityTarget.position, 30f);
		}
		this.stateTime += 0.05f;
		if (this.state == EAIMeleeAttackTarget.State.StartAnim)
		{
			if (this.theEntity.GetAnimActionState() != AvatarController.ActionState.Ready)
			{
				return;
			}
			this.theEntity.ContinueAnimAction(this.startAnimType + 1 + 3000);
			this.state = EAIMeleeAttackTarget.State.ReleaseAnim;
			this.stateTime = 0f;
			if (!string.IsNullOrEmpty(this.soundReleaseName))
			{
				Manager.BroadcastPlay(this.theEntity, this.soundReleaseName, false, 1f);
			}
		}
		if (this.state == EAIMeleeAttackTarget.State.ReleaseAnim)
		{
			if (this.stateTime < this.releaseDelay)
			{
				return;
			}
			this.state = EAIMeleeAttackTarget.State.Attack;
			this.stateTime = 0f;
		}
		this.theEntity.UseHoldingItem(this.itemActionType, false);
		if (!this.theEntity.IsHoldingItemInUse(this.itemActionType))
		{
			this.elapsedTime = float.MaxValue;
		}
	}

	// Token: 0x06002189 RID: 8585 RVA: 0x000CB140 File Offset: 0x000C9340
	public override void Reset()
	{
		this.theEntity.UseHoldingItem(this.itemActionType, true);
		this.theEntity.StartAnimAction(9999);
		this.theEntity.SetLookPosition(Vector3.zero);
		this.theEntity.emodel.avatarController.hitWeightMax = 1f;
		this.entityTarget = null;
		this.cooldown = this.baseCooldown + this.baseCooldown * 0.5f * base.RandomFloat;
	}

	// Token: 0x0600218A RID: 8586 RVA: 0x000CB1C4 File Offset: 0x000C93C4
	[PublicizedFrom(EAccessModifier.Private)]
	public bool InRange()
	{
		float distanceSq = this.entityTarget.GetDistanceSq(this.theEntity);
		if (this.unreachableRange > 0f)
		{
			EntityMoveHelper moveHelper = this.theEntity.moveHelper;
			if (moveHelper.IsUnreachableAbove || moveHelper.IsUnreachableSide)
			{
				return distanceSq <= this.unreachableRange * this.unreachableRange;
			}
		}
		return distanceSq >= this.minRange * this.minRange && distanceSq <= this.maxRange * this.maxRange;
	}

	// Token: 0x0600218B RID: 8587 RVA: 0x000CB248 File Offset: 0x000C9448
	public override string ToString()
	{
		bool flag = this.entityTarget && this.InRange();
		return string.Format("{0} {1}, inRange{2}, Time {3}", new object[]
		{
			base.ToString(),
			this.entityTarget ? this.entityTarget.EntityName : "",
			flag,
			this.elapsedTime.ToCultureInvariantString("0.00")
		});
	}

	// Token: 0x0400170E RID: 5902
	[PublicizedFrom(EAccessModifier.Private)]
	public int inventorySlot;

	// Token: 0x0400170F RID: 5903
	[PublicizedFrom(EAccessModifier.Private)]
	public int itemActionType;

	// Token: 0x04001710 RID: 5904
	[PublicizedFrom(EAccessModifier.Private)]
	public int startAnimType = -1;

	// Token: 0x04001711 RID: 5905
	[PublicizedFrom(EAccessModifier.Private)]
	public float releaseDelay = 0.5f;

	// Token: 0x04001712 RID: 5906
	[PublicizedFrom(EAccessModifier.Private)]
	public float baseCooldown;

	// Token: 0x04001713 RID: 5907
	[PublicizedFrom(EAccessModifier.Private)]
	public float cooldown;

	// Token: 0x04001714 RID: 5908
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityAlive entityTarget;

	// Token: 0x04001715 RID: 5909
	[PublicizedFrom(EAccessModifier.Private)]
	public float attackDuration;

	// Token: 0x04001716 RID: 5910
	[PublicizedFrom(EAccessModifier.Private)]
	public float elapsedTime;

	// Token: 0x04001717 RID: 5911
	[PublicizedFrom(EAccessModifier.Private)]
	public float minRange = 4f;

	// Token: 0x04001718 RID: 5912
	[PublicizedFrom(EAccessModifier.Private)]
	public float maxRange = 25f;

	// Token: 0x04001719 RID: 5913
	[PublicizedFrom(EAccessModifier.Private)]
	public float unreachableRange;

	// Token: 0x0400171A RID: 5914
	[PublicizedFrom(EAccessModifier.Private)]
	public string soundStartName;

	// Token: 0x0400171B RID: 5915
	[PublicizedFrom(EAccessModifier.Private)]
	public string soundReleaseName;

	// Token: 0x0400171C RID: 5916
	[PublicizedFrom(EAccessModifier.Private)]
	public EAIMeleeAttackTarget.State state;

	// Token: 0x0400171D RID: 5917
	[PublicizedFrom(EAccessModifier.Private)]
	public float stateTime;

	// Token: 0x0400171E RID: 5918
	[PublicizedFrom(EAccessModifier.Private)]
	public float painHitsFelt;

	// Token: 0x0200044E RID: 1102
	[PublicizedFrom(EAccessModifier.Private)]
	public enum State
	{
		// Token: 0x04001720 RID: 5920
		StartAnim,
		// Token: 0x04001721 RID: 5921
		ReleaseAnim,
		// Token: 0x04001722 RID: 5922
		Attack,
		// Token: 0x04001723 RID: 5923
		Release
	}
}
