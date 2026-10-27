using System;
using UnityEngine;

// Token: 0x020015DE RID: 5598
public class vp_SecurityCamTurret : vp_SimpleAITurret
{
	// Token: 0x0600AE8F RID: 44687 RVA: 0x0041205C File Offset: 0x0041025C
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

	// Token: 0x0600AE90 RID: 44688 RVA: 0x004120EC File Offset: 0x004102EC
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

	// Token: 0x040081C5 RID: 33221
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public vp_AngleBob m_AngleBob;

	// Token: 0x040081C6 RID: 33222
	public GameObject Swivel;

	// Token: 0x040081C7 RID: 33223
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 SwivelRotation = Vector3.zero;

	// Token: 0x040081C8 RID: 33224
	public float SwivelAmp = 100f;

	// Token: 0x040081C9 RID: 33225
	public float SwivelRate = 0.5f;

	// Token: 0x040081CA RID: 33226
	public float SwivelOffset;

	// Token: 0x040081CB RID: 33227
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public vp_Timer.Handle vp_ResumeSwivelTimer = new vp_Timer.Handle();
}
