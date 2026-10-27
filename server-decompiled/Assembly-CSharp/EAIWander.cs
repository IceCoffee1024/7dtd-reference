using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000461 RID: 1121
[Preserve]
public class EAIWander : EAIBase
{
	// Token: 0x060021F6 RID: 8694 RVA: 0x000C7564 File Offset: 0x000C5764
	public override void Init(EntityAlive _theEntity)
	{
		base.Init(_theEntity);
		this.MutexBits = 1;
	}

	// Token: 0x060021F7 RID: 8695 RVA: 0x000CDC7C File Offset: 0x000CBE7C
	public override void SetData(Dictionary<string, string> data)
	{
		base.SetData(data);
		base.GetData(data, "exePer", ref this.executePercent);
		base.GetData(data, "fade", ref this.fade);
		base.GetData(data, "lookMin", ref this.lookMin);
		base.GetData(data, "lookMax", ref this.lookMax);
	}

	// Token: 0x060021F8 RID: 8696 RVA: 0x000CDCD8 File Offset: 0x000CBED8
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

	// Token: 0x060021F9 RID: 8697 RVA: 0x000CDDCD File Offset: 0x000CBFCD
	public override void Start()
	{
		this.time = 0f;
		this.theEntity.FindPath(this.position, this.theEntity.GetMoveSpeed(), false, this);
		this.theEntity.renderFadeMax = this.fade;
	}

	// Token: 0x060021FA RID: 8698 RVA: 0x000CDE0C File Offset: 0x000CC00C
	public override bool Continue()
	{
		return this.theEntity.bodyDamage.CurrentStun == EnumEntityStunType.None && this.theEntity.moveHelper.BlockedTime <= 0.3f && this.time <= 30f && !this.theEntity.navigator.noPathAndNotPlanningOne();
	}

	// Token: 0x060021FB RID: 8699 RVA: 0x000CDE68 File Offset: 0x000CC068
	public override void Update()
	{
		this.time += 0.05f;
	}

	// Token: 0x060021FC RID: 8700 RVA: 0x000CDE7C File Offset: 0x000CC07C
	public override void Reset()
	{
		this.manager.lookTime = base.Random.RandomRange(this.lookMin, this.lookMax);
		this.theEntity.moveHelper.Stop();
		this.theEntity.renderFadeMax = 1f;
	}

	// Token: 0x04001784 RID: 6020
	[PublicizedFrom(EAccessModifier.Private)]
	public float fade = 1f;

	// Token: 0x04001785 RID: 6021
	[PublicizedFrom(EAccessModifier.Private)]
	public float lookMin = 0.5f;

	// Token: 0x04001786 RID: 6022
	[PublicizedFrom(EAccessModifier.Private)]
	public float lookMax = 5f;

	// Token: 0x04001787 RID: 6023
	[PublicizedFrom(EAccessModifier.Private)]
	public float executePercent = 0.2f;

	// Token: 0x04001788 RID: 6024
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 position;

	// Token: 0x04001789 RID: 6025
	[PublicizedFrom(EAccessModifier.Private)]
	public float time;
}
