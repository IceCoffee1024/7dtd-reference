using System;
using UnityEngine;

// Token: 0x02001596 RID: 5526
public class vp_Billboard : MonoBehaviour
{
	// Token: 0x0600AC9E RID: 44190 RVA: 0x004053AC File Offset: 0x004035AC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Start()
	{
		this.m_Transform = base.transform;
		if (this.m_CameraTransform == null)
		{
			this.m_CameraTransform = Camera.main.transform;
		}
	}

	// Token: 0x0600AC9F RID: 44191 RVA: 0x004053D8 File Offset: 0x004035D8
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Update()
	{
		if (this.m_CameraTransform != null)
		{
			this.m_Transform.localEulerAngles = this.m_CameraTransform.eulerAngles;
		}
		this.m_Transform.localEulerAngles = this.m_Transform.localEulerAngles;
	}

	// Token: 0x04007FB3 RID: 32691
	public Transform m_CameraTransform;

	// Token: 0x04007FB4 RID: 32692
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform m_Transform;
}
