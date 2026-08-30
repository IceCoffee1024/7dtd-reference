using System;
using UnityEngine;

// Token: 0x0200155A RID: 5466
public class vp_Bob : MonoBehaviour
{
	// Token: 0x0600AAF1 RID: 43761 RVA: 0x003FB295 File Offset: 0x003F9495
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Awake()
	{
		this.m_Transform = base.transform;
		this.m_InitialPosition = this.m_Transform.position;
	}

	// Token: 0x0600AAF2 RID: 43762 RVA: 0x003FB2B4 File Offset: 0x003F94B4
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnEnable()
	{
		this.m_Transform.position = this.m_InitialPosition;
		if (this.RandomizeBobOffset)
		{
			this.BobOffset = global::UnityEngine.Random.value;
		}
	}

	// Token: 0x0600AAF3 RID: 43763 RVA: 0x003FB2DC File Offset: 0x003F94DC
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

	// Token: 0x04007E59 RID: 32345
	public Vector3 BobAmp = new Vector3(0f, 0.1f, 0f);

	// Token: 0x04007E5A RID: 32346
	public Vector3 BobRate = new Vector3(0f, 4f, 0f);

	// Token: 0x04007E5B RID: 32347
	public float BobOffset;

	// Token: 0x04007E5C RID: 32348
	public float GroundOffset;

	// Token: 0x04007E5D RID: 32349
	public bool RandomizeBobOffset;

	// Token: 0x04007E5E RID: 32350
	public bool LocalMotion;

	// Token: 0x04007E5F RID: 32351
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform m_Transform;

	// Token: 0x04007E60 RID: 32352
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_InitialPosition;

	// Token: 0x04007E61 RID: 32353
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_Offset;
}
