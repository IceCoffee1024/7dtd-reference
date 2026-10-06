using System;
using System.Collections.Generic;
using GamePath;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000449 RID: 1097
[Preserve]
public class EAILeap : EAIBase
{
	// Token: 0x0600215A RID: 8538 RVA: 0x000C9371 File Offset: 0x000C7571
	public override void Init(EntityAlive _theEntity)
	{
		base.Init(_theEntity);
		this.MutexBits = 3;
		this.executeDelay = 1f + base.RandomFloat;
	}

	// Token: 0x0600215B RID: 8539 RVA: 0x000C9393 File Offset: 0x000C7593
	public override void SetData(Dictionary<string, string> data)
	{
		base.SetData(data);
		base.GetData(data, "legs", ref this.legCount);
	}

	// Token: 0x0600215C RID: 8540 RVA: 0x000C93B0 File Offset: 0x000C75B0
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

	// Token: 0x0600215D RID: 8541 RVA: 0x000C953C File Offset: 0x000C773C
	public override void Start()
	{
		this.abortTime = 5f;
		this.theEntity.moveHelper.Stop();
		this.leapYaw = Mathf.Atan2(this.leapV.x, this.leapV.z) * 57.29578f;
	}

	// Token: 0x0600215E RID: 8542 RVA: 0x000C958C File Offset: 0x000C778C
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

	// Token: 0x0600215F RID: 8543 RVA: 0x000C9626 File Offset: 0x000C7826
	public override void Update()
	{
		this.abortTime -= 0.05f;
	}

	// Token: 0x06002160 RID: 8544 RVA: 0x000027FC File Offset: 0x000009FC
	public override void Reset()
	{
	}

	// Token: 0x040016F3 RID: 5875
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cCollisionMask = 1082195968;

	// Token: 0x040016F4 RID: 5876
	[PublicizedFrom(EAccessModifier.Private)]
	public int legCount = 2;

	// Token: 0x040016F5 RID: 5877
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 leapV;

	// Token: 0x040016F6 RID: 5878
	[PublicizedFrom(EAccessModifier.Private)]
	public float leapDist;

	// Token: 0x040016F7 RID: 5879
	[PublicizedFrom(EAccessModifier.Private)]
	public float leapYaw;

	// Token: 0x040016F8 RID: 5880
	[PublicizedFrom(EAccessModifier.Private)]
	public float abortTime;
}
