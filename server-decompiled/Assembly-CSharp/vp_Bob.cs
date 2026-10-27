using System;
using UnityEngine;

// Token: 0x020015A9 RID: 5545
public class vp_Bob : MonoBehaviour
{
	// Token: 0x0600ACF9 RID: 44281 RVA: 0x0040782D File Offset: 0x00405A2D
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Awake()
	{
		this.m_Transform = base.transform;
		this.m_InitialPosition = this.m_Transform.position;
	}

	// Token: 0x0600ACFA RID: 44282 RVA: 0x0040784C File Offset: 0x00405A4C
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnEnable()
	{
		this.m_Transform.position = this.m_InitialPosition;
		if (this.RandomizeBobOffset)
		{
			this.BobOffset = global::UnityEngine.Random.value;
		}
	}

	// Token: 0x0600ACFB RID: 44283 RVA: 0x00407874 File Offset: 0x00405A74
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

	// Token: 0x04007FF6 RID: 32758
	public Vector3 BobAmp = new Vector3(0f, 0.1f, 0f);

	// Token: 0x04007FF7 RID: 32759
	public Vector3 BobRate = new Vector3(0f, 4f, 0f);

	// Token: 0x04007FF8 RID: 32760
	public float BobOffset;

	// Token: 0x04007FF9 RID: 32761
	public float GroundOffset;

	// Token: 0x04007FFA RID: 32762
	public bool RandomizeBobOffset;

	// Token: 0x04007FFB RID: 32763
	public bool LocalMotion;

	// Token: 0x04007FFC RID: 32764
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform m_Transform;

	// Token: 0x04007FFD RID: 32765
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_InitialPosition;

	// Token: 0x04007FFE RID: 32766
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_Offset;
}
