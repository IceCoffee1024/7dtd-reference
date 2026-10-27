using System;
using System.Collections.Generic;
using GamePath;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200044B RID: 1099
[Preserve]
public class EAILeap : EAIBase
{
	// Token: 0x0600215F RID: 8543 RVA: 0x000C971D File Offset: 0x000C791D
	public override void Init(EntityAlive _theEntity)
	{
		base.Init(_theEntity);
		this.MutexBits = 3;
		this.executeDelay = 1f + base.RandomFloat;
	}

	// Token: 0x06002160 RID: 8544 RVA: 0x000C973F File Offset: 0x000C793F
	public override void SetData(Dictionary<string, string> data)
	{
		base.SetData(data);
		base.GetData(data, "legs", ref this.legCount);
	}

	// Token: 0x06002161 RID: 8545 RVA: 0x000C975C File Offset: 0x000C795C
	public override bool CanExecute()
	{
		if (this.theEntity.IsDancing)
		{
			return false;
		}
		if (!this.theEntity.GetAttackTarget())
		{
			return false;
		}
		if (this.theEntity.Jumping)
		{
			return false;
		}
		if ((this.legCount <= 2) ? this.theEntity.bodyDamage.IsAnyLegMissing : this.theEntity.bodyDamage.IsAnyArmOrLegMissing)
		{
			return false;
		}
		if (this.theEntity.moveHelper.BlockedFlags > 0)
		{
			return false;
		}
		PathEntity path = this.theEntity.navigator.getPath();
		if (path == null)
		{
			return false;
		}
		float jumpMaxDistance = this.theEntity.jumpMaxDistance;
		this.leapV = path.GetEndPos() - this.theEntity.position;
		if (this.leapV.y < -5f || this.leapV.y > 0.5f + jumpMaxDistance * 0.5f)
		{
			return false;
		}
		this.leapDist = Mathf.Sqrt(this.leapV.x * this.leapV.x + this.leapV.z * this.leapV.z);
		if (this.leapDist < 2.8f || this.leapDist > jumpMaxDistance)
		{
			return false;
		}
		Vector3 position = this.theEntity.position;
		position.y += 1.5f;
		RaycastHit raycastHit;
		return !Physics.Raycast(position - Origin.position, this.leapV, out raycastHit, this.leapDist - 0.5f, 1082195968);
	}

	// Token: 0x06002162 RID: 8546 RVA: 0x000C98E8 File Offset: 0x000C7AE8
	public override void Start()
	{
		this.abortTime = 5f;
		this.theEntity.moveHelper.Stop();
		this.leapYaw = Mathf.Atan2(this.leapV.x, this.leapV.z) * 57.29578f;
	}

	// Token: 0x06002163 RID: 8547 RVA: 0x000C9938 File Offset: 0x000C7B38
	public override bool Continue()
	{
		if (this.theEntity.bodyDamage.CurrentStun != EnumEntityStunType.None)
		{
			return false;
		}
		if (this.abortTime <= 0f)
		{
			return false;
		}
		EntityMoveHelper moveHelper = this.theEntity.moveHelper;
		this.theEntity.SeekYaw(this.leapYaw, 0f, 10f);
		if (Utils.FastAbs(Mathf.DeltaAngle(this.theEntity.rotation.y, this.leapYaw)) < 1f)
		{
			moveHelper.StartJump(false, this.leapDist, this.leapV.y);
			return false;
		}
		return true;
	}

	// Token: 0x06002164 RID: 8548 RVA: 0x000C99D2 File Offset: 0x000C7BD2
	public override void Update()
	{
		this.abortTime -= 0.05f;
	}

	// Token: 0x06002165 RID: 8549 RVA: 0x000027FC File Offset: 0x000009FC
	public override void Reset()
	{
	}

	// Token: 0x04001703 RID: 5891
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cCollisionMask = 1082195968;

	// Token: 0x04001704 RID: 5892
	[PublicizedFrom(EAccessModifier.Private)]
	public int legCount = 2;

	// Token: 0x04001705 RID: 5893
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 leapV;

	// Token: 0x04001706 RID: 5894
	[PublicizedFrom(EAccessModifier.Private)]
	public float leapDist;

	// Token: 0x04001707 RID: 5895
	[PublicizedFrom(EAccessModifier.Private)]
	public float leapYaw;

	// Token: 0x04001708 RID: 5896
	[PublicizedFrom(EAccessModifier.Private)]
	public float abortTime;
}
