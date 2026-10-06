using System;
using UnityEngine;

// Token: 0x020015CC RID: 5580
public class vp_SecurityCamTurret : vp_SimpleAITurret
{
	// Token: 0x0600AE37 RID: 44599 RVA: 0x0040FC58 File Offset: 0x0040DE58
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

	// Token: 0x0600AE38 RID: 44600 RVA: 0x0040FCE8 File Offset: 0x0040DEE8
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

	// Token: 0x04008184 RID: 33156
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public vp_AngleBob m_AngleBob;

	// Token: 0x04008185 RID: 33157
	public GameObject Swivel;

	// Token: 0x04008186 RID: 33158
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 SwivelRotation = Vector3.zero;

	// Token: 0x04008187 RID: 33159
	public float SwivelAmp = 100f;

	// Token: 0x04008188 RID: 33160
	public float SwivelRate = 0.5f;

	// Token: 0x04008189 RID: 33161
	public float SwivelOffset;

	// Token: 0x0400818A RID: 33162
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public vp_Timer.Handle vp_ResumeSwivelTimer = new vp_Timer.Handle();
}
