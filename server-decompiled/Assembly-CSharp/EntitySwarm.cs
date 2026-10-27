using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020004E3 RID: 1251
[Preserve]
public class EntitySwarm : EntityVulture
{
	// Token: 0x06002873 RID: 10355 RVA: 0x000FB510 File Offset: 0x000F9710
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Init()
	{
		base.Init();
		this.targetAttackHealthPercent = 1f;
		this.ignoreTargetAttached = true;
		this.wanderHeightRange.x = 1f;
		this.wanderHeightRange.y = 8f;
		this.dissipateDelay = 24f;
	}

	// Token: 0x06002874 RID: 10356 RVA: 0x000FB560 File Offset: 0x000F9760
	public override void OnUpdateLive()
	{
		base.OnUpdateLive();
		if (!this.IsDead())
		{
			this.dissipateDelay -= 0.05f;
			if (this.dissipateDelay <= 2f && this.state != EntityVulture.State.Home)
			{
				base.StartHome(this.position + new Vector3(0f, 50f, 0f));
			}
			if (this.dissipateDelay <= 0f)
			{
				this.Kill(DamageResponse.New(true));
			}
		}
	}

	// Token: 0x04001E6D RID: 7789
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float dissipateDelay;
}
