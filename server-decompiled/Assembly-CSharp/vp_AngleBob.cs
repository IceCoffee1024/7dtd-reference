using System;
using UnityEngine;

// Token: 0x02001595 RID: 5525
public class vp_AngleBob : MonoBehaviour
{
	// Token: 0x0600AC9A RID: 44186 RVA: 0x00405153 File Offset: 0x00403353
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Awake()
	{
		this.m_Transform = base.transform;
		this.m_InitialRotation = this.m_Transform.eulerAngles;
	}

	// Token: 0x0600AC9B RID: 44187 RVA: 0x00405172 File Offset: 0x00403372
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnEnable()
	{
		this.m_Transform.eulerAngles = this.m_InitialRotation;
		if (this.RandomizeBobOffset)
		{
			this.YOffset = global::UnityEngine.Random.value;
		}
	}

	// Token: 0x0600AC9C RID: 44188 RVA: 0x00405198 File Offset: 0x00403398
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

	// Token: 0x04007FAA RID: 32682
	public Vector3 BobAmp = new Vector3(0f, 0.1f, 0f);

	// Token: 0x04007FAB RID: 32683
	public Vector3 BobRate = new Vector3(0f, 4f, 0f);

	// Token: 0x04007FAC RID: 32684
	public float YOffset;

	// Token: 0x04007FAD RID: 32685
	public bool RandomizeBobOffset;

	// Token: 0x04007FAE RID: 32686
	public bool LocalMotion;

	// Token: 0x04007FAF RID: 32687
	public bool FadeToTarget;

	// Token: 0x04007FB0 RID: 32688
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform m_Transform;

	// Token: 0x04007FB1 RID: 32689
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_InitialRotation;

	// Token: 0x04007FB2 RID: 32690
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_Offset;
}
