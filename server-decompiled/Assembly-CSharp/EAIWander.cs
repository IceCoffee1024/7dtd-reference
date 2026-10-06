using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200045F RID: 1119
[Preserve]
public class EAIWander : EAIBase
{
	// Token: 0x060021F1 RID: 8689 RVA: 0x000C71B8 File Offset: 0x000C53B8
	public override void Init(EntityAlive _theEntity)
	{
		base.Init(_theEntity);
		this.MutexBits = 1;
	}

	// Token: 0x060021F2 RID: 8690 RVA: 0x000CD8D0 File Offset: 0x000CBAD0
	public override void SetData(Dictionary<string, string> data)
	{
		base.SetData(data);
		base.GetData(data, "exePer", ref this.executePercent);
		base.GetData(data, "fade", ref this.fade);
		base.GetData(data, "lookMin", ref this.lookMin);
		base.GetData(data, "lookMax", ref this.lookMax);
	}

	// Token: 0x060021F3 RID: 8691 RVA: 0x000CD92C File Offset: 0x000CBB2C
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

	// Token: 0x060021F4 RID: 8692 RVA: 0x000CDA21 File Offset: 0x000CBC21
	public override void Start()
	{
		this.time = 0f;
		this.theEntity.FindPath(this.position, this.theEntity.GetMoveSpeed(), false, this);
		this.theEntity.renderFadeMax = this.fade;
	}

	// Token: 0x060021F5 RID: 8693 RVA: 0x000CDA60 File Offset: 0x000CBC60
	public override bool Continue()
	{
		return this.theEntity.bodyDamage.CurrentStun == EnumEntityStunType.None && this.theEntity.moveHelper.BlockedTime <= 0.3f && this.time <= 30f && !this.theEntity.navigator.noPathAndNotPlanningOne();
	}

	// Token: 0x060021F6 RID: 8694 RVA: 0x000CDABC File Offset: 0x000CBCBC
	public override void Update()
	{
		this.time += 0.05f;
	}

	// Token: 0x060021F7 RID: 8695 RVA: 0x000CDAD0 File Offset: 0x000CBCD0
	public override void Reset()
	{
		this.manager.lookTime = base.Random.RandomRange(this.lookMin, this.lookMax);
		this.theEntity.moveHelper.Stop();
		this.theEntity.renderFadeMax = 1f;
	}

	// Token: 0x04001774 RID: 6004
	[PublicizedFrom(EAccessModifier.Private)]
	public float fade = 1f;

	// Token: 0x04001775 RID: 6005
	[PublicizedFrom(EAccessModifier.Private)]
	public float lookMin = 0.5f;

	// Token: 0x04001776 RID: 6006
	[PublicizedFrom(EAccessModifier.Private)]
	public float lookMax = 5f;

	// Token: 0x04001777 RID: 6007
	[PublicizedFrom(EAccessModifier.Private)]
	public float executePercent = 0.2f;

	// Token: 0x04001778 RID: 6008
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 position;

	// Token: 0x04001779 RID: 6009
	[PublicizedFrom(EAccessModifier.Private)]
	public float time;
}
