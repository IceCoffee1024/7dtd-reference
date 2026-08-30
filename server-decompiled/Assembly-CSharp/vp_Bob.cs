using System;
using UnityEngine;

// Token: 0x02001575 RID: 5493
public class vp_Bob : MonoBehaviour
{
	// Token: 0x0600ABEE RID: 44014 RVA: 0x004005D5 File Offset: 0x003FE7D5
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Awake()
	{
		this.m_Transform = base.transform;
		this.m_InitialPosition = this.m_Transform.position;
	}

	// Token: 0x0600ABEF RID: 44015 RVA: 0x004005F4 File Offset: 0x003FE7F4
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnEnable()
	{
		this.m_Transform.position = this.m_InitialPosition;
		if (this.RandomizeBobOffset)
		{
			this.BobOffset = global::UnityEngine.Random.value;
		}
	}

	// Token: 0x0600ABF0 RID: 44016 RVA: 0x0040061C File Offset: 0x003FE81C
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

	// Token: 0x04007F32 RID: 32562
	public Vector3 BobAmp = new Vector3(0f, 0.1f, 0f);

	// Token: 0x04007F33 RID: 32563
	public Vector3 BobRate = new Vector3(0f, 4f, 0f);

	// Token: 0x04007F34 RID: 32564
	public float BobOffset;

	// Token: 0x04007F35 RID: 32565
	public float GroundOffset;

	// Token: 0x04007F36 RID: 32566
	public bool RandomizeBobOffset;

	// Token: 0x04007F37 RID: 32567
	public bool LocalMotion;

	// Token: 0x04007F38 RID: 32568
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform m_Transform;

	// Token: 0x04007F39 RID: 32569
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_InitialPosition;

	// Token: 0x04007F3A RID: 32570
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_Offset;
}
