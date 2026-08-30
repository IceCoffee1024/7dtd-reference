using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200045D RID: 1117
[Preserve]
public class EAIWander : EAIBase
{
	// Token: 0x06002202 RID: 8706 RVA: 0x000C7104 File Offset: 0x000C5304
	public override void Init(EntityAlive _theEntity)
	{
		base.Init(_theEntity);
		this.MutexBits = 1;
	}

	// Token: 0x06002203 RID: 8707 RVA: 0x000CD81C File Offset: 0x000CBA1C
	public override void SetData(Dictionary<string, string> data)
	{
		base.SetData(data);
		base.GetData(data, "exePer", ref this.executePercent);
		base.GetData(data, "fade", ref this.fade);
		base.GetData(data, "lookMin", ref this.lookMin);
		base.GetData(data, "lookMax", ref this.lookMax);
	}

	// Token: 0x06002204 RID: 8708 RVA: 0x000CD878 File Offset: 0x000CBA78
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

	// Token: 0x06002205 RID: 8709 RVA: 0x000CD96D File Offset: 0x000CBB6D
	public override void Start()
	{
		this.time = 0f;
		this.theEntity.FindPath(this.position, this.theEntity.GetMoveSpeed(), false, this);
		this.theEntity.renderFadeMax = this.fade;
	}

	// Token: 0x06002206 RID: 8710 RVA: 0x000CD9AC File Offset: 0x000CBBAC
	public override bool Continue()
	{
		return this.theEntity.bodyDamage.CurrentStun == EnumEntityStunType.None && this.theEntity.moveHelper.BlockedTime <= 0.3f && this.time <= 30f && !this.theEntity.navigator.noPathAndNotPlanningOne();
	}

	// Token: 0x06002207 RID: 8711 RVA: 0x000CDA08 File Offset: 0x000CBC08
	public override void Update()
	{
		this.time += 0.05f;
	}

	// Token: 0x06002208 RID: 8712 RVA: 0x000CDA1C File Offset: 0x000CBC1C
	public override void Reset()
	{
		this.manager.lookTime = base.Random.RandomRange(this.lookMin, this.lookMax);
		this.theEntity.moveHelper.Stop();
		this.theEntity.renderFadeMax = 1f;
	}

	// Token: 0x0400177D RID: 6013
	[PublicizedFrom(EAccessModifier.Private)]
	public float fade = 1f;

	// Token: 0x0400177E RID: 6014
	[PublicizedFrom(EAccessModifier.Private)]
	public float lookMin = 0.5f;

	// Token: 0x0400177F RID: 6015
	[PublicizedFrom(EAccessModifier.Private)]
	public float lookMax = 5f;

	// Token: 0x04001780 RID: 6016
	[PublicizedFrom(EAccessModifier.Private)]
	public float executePercent = 0.2f;

	// Token: 0x04001781 RID: 6017
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 position;

	// Token: 0x04001782 RID: 6018
	[PublicizedFrom(EAccessModifier.Private)]
	public float time;
}
