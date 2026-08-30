using System;
using System.Collections.Generic;
using Audio;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000447 RID: 1095
[Preserve]
public class EAIMeleeAttackTarget : EAIBase
{
	// Token: 0x06002177 RID: 8567 RVA: 0x000CA41C File Offset: 0x000C861C
	public override void Init(EntityAlive _theEntity)
	{
		base.Init(_theEntity);
		this.MutexBits = 11;
		this.cooldown = 3f;
		this.attackDuration = 20f;
	}

	// Token: 0x06002178 RID: 8568 RVA: 0x000CA444 File Offset: 0x000C8644
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

	// Token: 0x06002179 RID: 8569 RVA: 0x000CA520 File Offset: 0x000C8720
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

	// Token: 0x0600217A RID: 8570 RVA: 0x000CA5E8 File Offset: 0x000C87E8
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

	// Token: 0x0600217B RID: 8571 RVA: 0x000CA6A0 File Offset: 0x000C88A0
	public override bool Continue()
	{
		return this.entityTarget && !this.entityTarget.IsDead() && this.elapsedTime < this.attackDuration && this.theEntity.bodyDamage.CurrentStun == EnumEntityStunType.None && !this.theEntity.Electrocuted && (this.startAnimType < 0 || !this.theEntity.bodyDamage.IsAnyArmOrLegMissing) && this.InRange() && this.theEntity.painHitsFelt - this.painHitsFelt < 1f;
	}

	// Token: 0x0600217C RID: 8572 RVA: 0x000CA738 File Offset: 0x000C8938
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

	// Token: 0x0600217D RID: 8573 RVA: 0x000CA880 File Offset: 0x000C8A80
	public override void Reset()
	{
		this.theEntity.UseHoldingItem(this.itemActionType, true);
		this.theEntity.StartAnimAction(9999);
		this.theEntity.SetLookPosition(Vector3.zero);
		this.theEntity.emodel.avatarController.hitWeightMax = 1f;
		this.entityTarget = null;
		this.cooldown = this.baseCooldown + this.baseCooldown * 0.5f * base.RandomFloat;
	}

	// Token: 0x0600217E RID: 8574 RVA: 0x000CA904 File Offset: 0x000C8B04
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

	// Token: 0x0600217F RID: 8575 RVA: 0x000CA988 File Offset: 0x000C8B88
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

	// Token: 0x04001702 RID: 5890
	[PublicizedFrom(EAccessModifier.Private)]
	public int inventorySlot;

	// Token: 0x04001703 RID: 5891
	[PublicizedFrom(EAccessModifier.Private)]
	public int itemActionType;

	// Token: 0x04001704 RID: 5892
	[PublicizedFrom(EAccessModifier.Private)]
	public int startAnimType = -1;

	// Token: 0x04001705 RID: 5893
	[PublicizedFrom(EAccessModifier.Private)]
	public float releaseDelay = 0.5f;

	// Token: 0x04001706 RID: 5894
	[PublicizedFrom(EAccessModifier.Private)]
	public float baseCooldown;

	// Token: 0x04001707 RID: 5895
	[PublicizedFrom(EAccessModifier.Private)]
	public float cooldown;

	// Token: 0x04001708 RID: 5896
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityAlive entityTarget;

	// Token: 0x04001709 RID: 5897
	[PublicizedFrom(EAccessModifier.Private)]
	public float attackDuration;

	// Token: 0x0400170A RID: 5898
	[PublicizedFrom(EAccessModifier.Private)]
	public float elapsedTime;

	// Token: 0x0400170B RID: 5899
	[PublicizedFrom(EAccessModifier.Private)]
	public float minRange = 4f;

	// Token: 0x0400170C RID: 5900
	[PublicizedFrom(EAccessModifier.Private)]
	public float maxRange = 25f;

	// Token: 0x0400170D RID: 5901
	[PublicizedFrom(EAccessModifier.Private)]
	public float unreachableRange;

	// Token: 0x0400170E RID: 5902
	[PublicizedFrom(EAccessModifier.Private)]
	public string soundStartName;

	// Token: 0x0400170F RID: 5903
	[PublicizedFrom(EAccessModifier.Private)]
	public string soundReleaseName;

	// Token: 0x04001710 RID: 5904
	[PublicizedFrom(EAccessModifier.Private)]
	public EAIMeleeAttackTarget.State state;

	// Token: 0x04001711 RID: 5905
	[PublicizedFrom(EAccessModifier.Private)]
	public float stateTime;

	// Token: 0x04001712 RID: 5906
	[PublicizedFrom(EAccessModifier.Private)]
	public float painHitsFelt;

	// Token: 0x02000448 RID: 1096
	[PublicizedFrom(EAccessModifier.Private)]
	public enum State
	{
		// Token: 0x04001714 RID: 5908
		StartAnim,
		// Token: 0x04001715 RID: 5909
		ReleaseAnim,
		// Token: 0x04001716 RID: 5910
		Attack,
		// Token: 0x04001717 RID: 5911
		Release
	}
}
