using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020004D4 RID: 1236
[Preserve]
public class EntitySwarm : EntityVulture
{
	// Token: 0x0600283C RID: 10300 RVA: 0x000F94A4 File Offset: 0x000F76A4
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

	// Token: 0x0600283D RID: 10301 RVA: 0x000F94F4 File Offset: 0x000F76F4
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

	// Token: 0x04001E27 RID: 7719
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float dissipateDelay;
}
