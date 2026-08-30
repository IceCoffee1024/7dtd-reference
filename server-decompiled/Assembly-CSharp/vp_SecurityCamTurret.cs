using System;
using UnityEngine;

// Token: 0x020015AA RID: 5546
public class vp_SecurityCamTurret : vp_SimpleAITurret
{
	// Token: 0x0600AD84 RID: 44420 RVA: 0x0040AE04 File Offset: 0x00409004
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

	// Token: 0x0600AD85 RID: 44421 RVA: 0x0040AE94 File Offset: 0x00409094
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

	// Token: 0x04008101 RID: 33025
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public vp_AngleBob m_AngleBob;

	// Token: 0x04008102 RID: 33026
	public GameObject Swivel;

	// Token: 0x04008103 RID: 33027
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 SwivelRotation = Vector3.zero;

	// Token: 0x04008104 RID: 33028
	public float SwivelAmp = 100f;

	// Token: 0x04008105 RID: 33029
	public float SwivelRate = 0.5f;

	// Token: 0x04008106 RID: 33030
	public float SwivelOffset;

	// Token: 0x04008107 RID: 33031
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public vp_Timer.Handle vp_ResumeSwivelTimer = new vp_Timer.Handle();
}
