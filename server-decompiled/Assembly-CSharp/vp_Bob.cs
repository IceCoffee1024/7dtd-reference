using System;
using UnityEngine;

// Token: 0x02001597 RID: 5527
public class vp_Bob : MonoBehaviour
{
	// Token: 0x0600ACA1 RID: 44193 RVA: 0x00405429 File Offset: 0x00403629
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Awake()
	{
		this.m_Transform = base.transform;
		this.m_InitialPosition = this.m_Transform.position;
	}

	// Token: 0x0600ACA2 RID: 44194 RVA: 0x00405448 File Offset: 0x00403648
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnEnable()
	{
		this.m_Transform.position = this.m_InitialPosition;
		if (this.RandomizeBobOffset)
		{
			this.BobOffset = global::UnityEngine.Random.value;
		}
	}

	// Token: 0x0600ACA3 RID: 44195 RVA: 0x00405470 File Offset: 0x00403670
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Update()
	{
		if (this.BobRate.x != 0f && this.BobAmp.x != 0f)
		{
			this.m_Offset.x = vp_MathUtility.Sinus(this.BobRate.x, this.BobAmp.x, this.BobOffset);
		}
		if (this.BobRate.y != 0f && this.BobAmp.y != 0f)
		{
			this.m_Offset.y = vp_MathUtility.Sinus(this.BobRate.y, this.BobAmp.y, this.BobOffset);
		}
		if (this.BobRate.z != 0f && this.BobAmp.z != 0f)
		{
			this.m_Offset.z = vp_MathUtility.Sinus(this.BobRate.z, this.BobAmp.z, this.BobOffset);
		}
		if (!this.LocalMotion)
		{
			this.m_Transform.position = this.m_InitialPosition + this.m_Offset + Vector3.up * this.GroundOffset;
			return;
		}
		this.m_Transform.position = this.m_InitialPosition + Vector3.up * this.GroundOffset;
		this.m_Transform.localPosition += this.m_Transform.TransformDirection(this.m_Offset);
	}

	// Token: 0x04007FB5 RID: 32693
	public Vector3 BobAmp = new Vector3(0f, 0.1f, 0f);

	// Token: 0x04007FB6 RID: 32694
	public Vector3 BobRate = new Vector3(0f, 4f, 0f);

	// Token: 0x04007FB7 RID: 32695
	public float BobOffset;

	// Token: 0x04007FB8 RID: 32696
	public float GroundOffset;

	// Token: 0x04007FB9 RID: 32697
	public bool RandomizeBobOffset;

	// Token: 0x04007FBA RID: 32698
	public bool LocalMotion;

	// Token: 0x04007FBB RID: 32699
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform m_Transform;

	// Token: 0x04007FBC RID: 32700
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_InitialPosition;

	// Token: 0x04007FBD RID: 32701
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_Offset;
}
