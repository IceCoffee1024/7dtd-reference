using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020004E1 RID: 1249
[Preserve]
public class EntitySwarm : EntityVulture
{
	// Token: 0x0600286C RID: 10348 RVA: 0x000FAFD8 File Offset: 0x000F91D8
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

	// Token: 0x0600286D RID: 10349 RVA: 0x000FB028 File Offset: 0x000F9228
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

	// Token: 0x04001E5D RID: 7773
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float dissipateDelay;
}
