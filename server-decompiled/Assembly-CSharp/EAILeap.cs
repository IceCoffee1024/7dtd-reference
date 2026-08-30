using System;
using System.Collections.Generic;
using GamePath;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000443 RID: 1091
[Preserve]
public class EAILeap : EAIBase
{
	// Token: 0x0600214E RID: 8526 RVA: 0x000C8AB1 File Offset: 0x000C6CB1
	public override void Init(EntityAlive _theEntity)
	{
		base.Init(_theEntity);
		this.MutexBits = 3;
		this.executeDelay = 1f + base.RandomFloat;
	}

	// Token: 0x0600214F RID: 8527 RVA: 0x000C8AD3 File Offset: 0x000C6CD3
	public override void SetData(Dictionary<string, string> data)
	{
		base.SetData(data);
		base.GetData(data, "legs", ref this.legCount);
	}

	// Token: 0x06002150 RID: 8528 RVA: 0x000C8AF0 File Offset: 0x000C6CF0
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

	// Token: 0x06002151 RID: 8529 RVA: 0x000C8C7C File Offset: 0x000C6E7C
	public override void Start()
	{
		this.abortTime = 5f;
		this.theEntity.moveHelper.Stop();
		this.leapYaw = Mathf.Atan2(this.leapV.x, this.leapV.z) * 57.29578f;
	}

	// Token: 0x06002152 RID: 8530 RVA: 0x000C8CCC File Offset: 0x000C6ECC
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

	// Token: 0x06002153 RID: 8531 RVA: 0x000C8D66 File Offset: 0x000C6F66
	public override void Update()
	{
		this.abortTime -= 0.05f;
	}

	// Token: 0x06002154 RID: 8532 RVA: 0x000027FC File Offset: 0x000009FC
	public override void Reset()
	{
	}

	// Token: 0x040016E7 RID: 5863
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cCollisionMask = 1082195968;

	// Token: 0x040016E8 RID: 5864
	[PublicizedFrom(EAccessModifier.Private)]
	public int legCount = 2;

	// Token: 0x040016E9 RID: 5865
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 leapV;

	// Token: 0x040016EA RID: 5866
	[PublicizedFrom(EAccessModifier.Private)]
	public float leapDist;

	// Token: 0x040016EB RID: 5867
	[PublicizedFrom(EAccessModifier.Private)]
	public float leapYaw;

	// Token: 0x040016EC RID: 5868
	[PublicizedFrom(EAccessModifier.Private)]
	public float abortTime;
}
