using System;
using UnityEngine;

// Token: 0x02001558 RID: 5464
public class vp_AngleBob : MonoBehaviour
{
	// Token: 0x0600AAEA RID: 43754 RVA: 0x003FAFBF File Offset: 0x003F91BF
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Awake()
	{
		this.m_Transform = base.transform;
		this.m_InitialRotation = this.m_Transform.eulerAngles;
	}

	// Token: 0x0600AAEB RID: 43755 RVA: 0x003FAFDE File Offset: 0x003F91DE
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnEnable()
	{
		this.m_Transform.eulerAngles = this.m_InitialRotation;
		if (this.RandomizeBobOffset)
		{
			this.YOffset = global::UnityEngine.Random.value;
		}
	}

	// Token: 0x0600AAEC RID: 43756 RVA: 0x003FB004 File Offset: 0x003F9204
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Update()
	{
		if (this.BobRate.x != 0f && this.BobAmp.x != 0f)
		{
			this.m_Offset.x = vp_MathUtility.Sinus(this.BobRate.x, this.BobAmp.x, 0f);
		}
		if (this.BobRate.y != 0f && this.BobAmp.y != 0f)
		{
			this.m_Offset.y = vp_MathUtility.Sinus(this.BobRate.y, this.BobAmp.y, 0f);
		}
		if (this.BobRate.z != 0f && this.BobAmp.z != 0f)
		{
			this.m_Offset.z = vp_MathUtility.Sinus(this.BobRate.z, this.BobAmp.z, 0f);
		}
		if (this.LocalMotion)
		{
			this.m_Transform.eulerAngles = this.m_InitialRotation + Vector3.up * this.YOffset;
			this.m_Transform.localEulerAngles += this.m_Transform.TransformDirection(this.m_Offset);
			return;
		}
		if (this.FadeToTarget)
		{
			this.m_Transform.rotation = Quaternion.Lerp(this.m_Transform.rotation, Quaternion.Euler(this.m_InitialRotation + this.m_Offset + Vector3.up * this.YOffset), Time.deltaTime);
			return;
		}
		this.m_Transform.eulerAngles = this.m_InitialRotation + this.m_Offset + Vector3.up * this.YOffset;
	}

	// Token: 0x04007E4E RID: 32334
	public Vector3 BobAmp = new Vector3(0f, 0.1f, 0f);

	// Token: 0x04007E4F RID: 32335
	public Vector3 BobRate = new Vector3(0f, 4f, 0f);

	// Token: 0x04007E50 RID: 32336
	public float YOffset;

	// Token: 0x04007E51 RID: 32337
	public bool RandomizeBobOffset;

	// Token: 0x04007E52 RID: 32338
	public bool LocalMotion;

	// Token: 0x04007E53 RID: 32339
	public bool FadeToTarget;

	// Token: 0x04007E54 RID: 32340
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform m_Transform;

	// Token: 0x04007E55 RID: 32341
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_InitialRotation;

	// Token: 0x04007E56 RID: 32342
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_Offset;
}
