using System;
using UnityEngine;

// Token: 0x0200158F RID: 5519
public class vp_SecurityCamTurret : vp_SimpleAITurret
{
	// Token: 0x0600AC87 RID: 44167 RVA: 0x00405AC4 File Offset: 0x00403CC4
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Start()
	{
		base.Start();
		this.m_Transform = base.transform;
		this.m_AngleBob = base.gameObject.AddComponent<vp_AngleBob>();
		this.m_AngleBob.BobAmp.y = this.SwivelAmp;
		this.m_AngleBob.BobRate.y = this.SwivelRate;
		this.m_AngleBob.YOffset = this.SwivelOffset;
		this.m_AngleBob.FadeToTarget = true;
		this.SwivelRotation = this.Swivel.transform.eulerAngles;
	}

	// Token: 0x0600AC88 RID: 44168 RVA: 0x00405B54 File Offset: 0x00403D54
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Update()
	{
		base.Update();
		if (this.m_Target != null && this.m_AngleBob.enabled)
		{
			this.m_AngleBob.enabled = false;
			this.vp_ResumeSwivelTimer.Cancel();
		}
		if (this.m_Target == null && !this.m_AngleBob.enabled && !this.vp_ResumeSwivelTimer.Active)
		{
			vp_Timer.In(this.WakeInterval * 2f, delegate
			{
				this.m_AngleBob.enabled = true;
			}, this.vp_ResumeSwivelTimer);
		}
		this.SwivelRotation.y = this.m_Transform.eulerAngles.y;
		this.Swivel.transform.eulerAngles = this.SwivelRotation;
	}

	// Token: 0x04008028 RID: 32808
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public vp_AngleBob m_AngleBob;

	// Token: 0x04008029 RID: 32809
	public GameObject Swivel;

	// Token: 0x0400802A RID: 32810
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 SwivelRotation = Vector3.zero;

	// Token: 0x0400802B RID: 32811
	public float SwivelAmp = 100f;

	// Token: 0x0400802C RID: 32812
	public float SwivelRate = 0.5f;

	// Token: 0x0400802D RID: 32813
	public float SwivelOffset;

	// Token: 0x0400802E RID: 32814
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public vp_Timer.Handle vp_ResumeSwivelTimer = new vp_Timer.Handle();
}
