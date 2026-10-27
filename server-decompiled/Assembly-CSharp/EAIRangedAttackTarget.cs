using System;
using System.Collections.Generic;
using Audio;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000452 RID: 1106
[Preserve]
public class EAIRangedAttackTarget : EAIBase
{
	// Token: 0x06002199 RID: 8601 RVA: 0x000CB88C File Offset: 0x000C9A8C
	public override void Init(EntityAlive _theEntity)
	{
		base.Init(_theEntity);
		this.MutexBits = 11;
		this.cooldown = 3f;
		this.attackDuration = 20f;
	}

	// Token: 0x0600219A RID: 8602 RVA: 0x000CB8B4 File Offset: 0x000C9AB4
	public override void SetData(Dictionary<string, string> data)
	{
		base.SetData(data);
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

	// Token: 0x0600219B RID: 8603 RVA: 0x000CB97C File Offset: 0x000C9B7C
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

	// Token: 0x0600219C RID: 8604 RVA: 0x000CBA44 File Offset: 0x000C9C44
	public override void Start()
	{
		this.theEntity.emodel.avatarController.hitWeightMax = 0.5f;
		this.painHitsFelt = this.theEntity.painHitsFelt;
		this.elapsedTime = 0f;
		this.state = EAIRangedAttackTarget.State.Attack;
		this.stateTime = 0f;
		if (this.startAnimType >= 0)
		{
			this.state = EAIRangedAttackTarget.State.StartAnim;
			this.theEntity.StartAnimAction(this.startAnimType + 3000);
		}
		if (!string.IsNullOrEmpty(this.soundStartName))
		{
			Manager.BroadcastPlay(this.theEntity, this.soundStartName, false, 1f);
		}
	}

	// Token: 0x0600219D RID: 8605 RVA: 0x000CBAE4 File Offset: 0x000C9CE4
	public override bool Continue()
	{
		return this.entityTarget && !this.entityTarget.IsDead() && this.elapsedTime < this.attackDuration && this.theEntity.bodyDamage.CurrentStun == EnumEntityStunType.None && !this.theEntity.Electrocuted && (this.startAnimType < 0 || !this.theEntity.bodyDamage.IsAnyArmOrLegMissing) && this.theEntity.painHitsFelt - this.painHitsFelt < 1f;
	}

	// Token: 0x0600219E RID: 8606 RVA: 0x000CBB74 File Offset: 0x000C9D74
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
		if (this.state == EAIRangedAttackTarget.State.StartAnim)
		{
			if (this.theEntity.GetAnimActionState() != AvatarController.ActionState.Ready)
			{
				return;
			}
			this.theEntity.ContinueAnimAction(this.startAnimType + 1 + 3000);
			this.state = EAIRangedAttackTarget.State.ReleaseAnim;
			this.stateTime = 0f;
			if (!string.IsNullOrEmpty(this.soundReleaseName))
			{
				Manager.BroadcastPlay(this.theEntity, this.soundReleaseName, false, 1f);
			}
		}
		if (this.state == EAIRangedAttackTarget.State.ReleaseAnim)
		{
			if (this.stateTime < this.releaseDelay)
			{
				return;
			}
			this.state = EAIRangedAttackTarget.State.Attack;
			this.stateTime = 0f;
		}
		this.theEntity.UseHoldingItem(this.itemActionType, false);
		if (!this.theEntity.IsHoldingItemInUse(this.itemActionType))
		{
			this.elapsedTime = float.MaxValue;
		}
	}

	// Token: 0x0600219F RID: 8607 RVA: 0x000CBCBC File Offset: 0x000C9EBC
	public override void Reset()
	{
		this.theEntity.ShowHoldingItem(false);
		this.theEntity.UseHoldingItem(this.itemActionType, true);
		this.theEntity.StartAnimAction(9999);
		this.theEntity.SetLookPosition(Vector3.zero);
		this.theEntity.emodel.avatarController.hitWeightMax = 1f;
		this.entityTarget = null;
		this.cooldown = this.baseCooldown + this.baseCooldown * 0.5f * base.RandomFloat;
	}

	// Token: 0x060021A0 RID: 8608 RVA: 0x000CBD4C File Offset: 0x000C9F4C
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

	// Token: 0x060021A1 RID: 8609 RVA: 0x000CBDD0 File Offset: 0x000C9FD0
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

	// Token: 0x0400173A RID: 5946
	[PublicizedFrom(EAccessModifier.Private)]
	public int itemActionType;

	// Token: 0x0400173B RID: 5947
	[PublicizedFrom(EAccessModifier.Private)]
	public int startAnimType = -1;

	// Token: 0x0400173C RID: 5948
	[PublicizedFrom(EAccessModifier.Private)]
	public float releaseDelay = 0.5f;

	// Token: 0x0400173D RID: 5949
	[PublicizedFrom(EAccessModifier.Private)]
	public float baseCooldown;

	// Token: 0x0400173E RID: 5950
	[PublicizedFrom(EAccessModifier.Private)]
	public float cooldown;

	// Token: 0x0400173F RID: 5951
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityAlive entityTarget;

	// Token: 0x04001740 RID: 5952
	[PublicizedFrom(EAccessModifier.Private)]
	public float attackDuration;

	// Token: 0x04001741 RID: 5953
	[PublicizedFrom(EAccessModifier.Private)]
	public float elapsedTime;

	// Token: 0x04001742 RID: 5954
	[PublicizedFrom(EAccessModifier.Private)]
	public float minRange = 4f;

	// Token: 0x04001743 RID: 5955
	[PublicizedFrom(EAccessModifier.Private)]
	public float maxRange = 25f;

	// Token: 0x04001744 RID: 5956
	[PublicizedFrom(EAccessModifier.Private)]
	public float unreachableRange;

	// Token: 0x04001745 RID: 5957
	[PublicizedFrom(EAccessModifier.Private)]
	public string soundStartName;

	// Token: 0x04001746 RID: 5958
	[PublicizedFrom(EAccessModifier.Private)]
	public string soundReleaseName;

	// Token: 0x04001747 RID: 5959
	[PublicizedFrom(EAccessModifier.Private)]
	public EAIRangedAttackTarget.State state;

	// Token: 0x04001748 RID: 5960
	[PublicizedFrom(EAccessModifier.Private)]
	public float stateTime;

	// Token: 0x04001749 RID: 5961
	[PublicizedFrom(EAccessModifier.Private)]
	public float painHitsFelt;

	// Token: 0x02000453 RID: 1107
	[PublicizedFrom(EAccessModifier.Private)]
	public enum State
	{
		// Token: 0x0400174B RID: 5963
		StartAnim,
		// Token: 0x0400174C RID: 5964
		ReleaseAnim,
		// Token: 0x0400174D RID: 5965
		Attack,
		// Token: 0x0400174E RID: 5966
		Release
	}
}
