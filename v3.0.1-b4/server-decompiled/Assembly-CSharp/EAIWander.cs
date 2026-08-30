using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000459 RID: 1113
[Preserve]
public class EAIWander : EAIBase
{
	// Token: 0x060021E5 RID: 8677 RVA: 0x000C68F8 File Offset: 0x000C4AF8
	public override void Init(EntityAlive _theEntity)
	{
		base.Init(_theEntity);
		this.MutexBits = 1;
	}

	// Token: 0x060021E6 RID: 8678 RVA: 0x000CD080 File Offset: 0x000CB280
	public override void SetData(Dictionary<string, string> data)
	{
		base.SetData(data);
		base.GetData(data, "exePer", ref this.executePercent);
		base.GetData(data, "fade", ref this.fade);
		base.GetData(data, "lookMin", ref this.lookMin);
		base.GetData(data, "lookMax", ref this.lookMax);
	}

	// Token: 0x060021E7 RID: 8679 RVA: 0x000CD0DC File Offset: 0x000CB2DC
	public override bool CanExecute()
	{
		if (this.theEntity.sleepingOrWakingUp)
		{
			return false;
		}
		if (this.manager.lookTime > 0f)
		{
			return false;
		}
		if (this.fade == 1f && this.theEntity.GetTicksNoPlayerAdjacent() >= 120)
		{
			return false;
		}
		if (this.theEntity.bodyDamage.CurrentStun != EnumEntityStunType.None)
		{
			return false;
		}
		bool isAlert = this.theEntity.IsAlert;
		if (!isAlert && this.executePercent * this.executeWaitTime <= base.RandomFloat)
		{
			return false;
		}
		int num = 1;
		int num2 = (int)this.manager.interestDistance;
		if (isAlert)
		{
			num = 2;
			num2 *= 2;
		}
		Vector3 vector = ((base.RandomFloat < 0.6f) ? this.theEntity.GetForwardVector() : base.Random.RandomOnUnitCircleXZ);
		Vector3 vector2 = RandomPositionGenerator.CalcInDir(this.theEntity, num, num2, num2, vector, 90f);
		if (vector2.y == 0f)
		{
			return false;
		}
		this.position = vector2;
		return true;
	}

	// Token: 0x060021E8 RID: 8680 RVA: 0x000CD1D1 File Offset: 0x000CB3D1
	public override void Start()
	{
		this.time = 0f;
		this.theEntity.FindPath(this.position, this.theEntity.GetMoveSpeed(), false, this);
		this.theEntity.renderFadeMax = this.fade;
	}

	// Token: 0x060021E9 RID: 8681 RVA: 0x000CD210 File Offset: 0x000CB410
	public override bool Continue()
	{
		return this.theEntity.bodyDamage.CurrentStun == EnumEntityStunType.None && this.theEntity.moveHelper.BlockedTime <= 0.3f && this.time <= 30f && !this.theEntity.navigator.noPathAndNotPlanningOne();
	}

	// Token: 0x060021EA RID: 8682 RVA: 0x000CD26C File Offset: 0x000CB46C
	public override void Update()
	{
		this.time += 0.05f;
	}

	// Token: 0x060021EB RID: 8683 RVA: 0x000CD280 File Offset: 0x000CB480
	public override void Reset()
	{
		this.manager.lookTime = base.Random.RandomRange(this.lookMin, this.lookMax);
		this.theEntity.moveHelper.Stop();
		this.theEntity.renderFadeMax = 1f;
	}

	// Token: 0x04001767 RID: 5991
	[PublicizedFrom(EAccessModifier.Private)]
	public float fade = 1f;

	// Token: 0x04001768 RID: 5992
	[PublicizedFrom(EAccessModifier.Private)]
	public float lookMin = 0.5f;

	// Token: 0x04001769 RID: 5993
	[PublicizedFrom(EAccessModifier.Private)]
	public float lookMax = 5f;

	// Token: 0x0400176A RID: 5994
	[PublicizedFrom(EAccessModifier.Private)]
	public float executePercent = 0.2f;

	// Token: 0x0400176B RID: 5995
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 position;

	// Token: 0x0400176C RID: 5996
	[PublicizedFrom(EAccessModifier.Private)]
	public float time;
}
