using System;
using UnityEngine;

// Token: 0x020015A7 RID: 5543
public class vp_AngleBob : MonoBehaviour
{
	// Token: 0x0600ACF2 RID: 44274 RVA: 0x00407557 File Offset: 0x00405757
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Awake()
	{
		this.m_Transform = base.transform;
		this.m_InitialRotation = this.m_Transform.eulerAngles;
	}

	// Token: 0x0600ACF3 RID: 44275 RVA: 0x00407576 File Offset: 0x00405776
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnEnable()
	{
		this.m_Transform.eulerAngles = this.m_InitialRotation;
		if (this.RandomizeBobOffset)
		{
			this.YOffset = global::UnityEngine.Random.value;
		}
	}

	// Token: 0x0600ACF4 RID: 44276 RVA: 0x0040759C File Offset: 0x0040579C
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

	// Token: 0x04007FEB RID: 32747
	public Vector3 BobAmp = new Vector3(0f, 0.1f, 0f);

	// Token: 0x04007FEC RID: 32748
	public Vector3 BobRate = new Vector3(0f, 4f, 0f);

	// Token: 0x04007FED RID: 32749
	public float YOffset;

	// Token: 0x04007FEE RID: 32750
	public bool RandomizeBobOffset;

	// Token: 0x04007FEF RID: 32751
	public bool LocalMotion;

	// Token: 0x04007FF0 RID: 32752
	public bool FadeToTarget;

	// Token: 0x04007FF1 RID: 32753
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform m_Transform;

	// Token: 0x04007FF2 RID: 32754
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_InitialRotation;

	// Token: 0x04007FF3 RID: 32755
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_Offset;
}
