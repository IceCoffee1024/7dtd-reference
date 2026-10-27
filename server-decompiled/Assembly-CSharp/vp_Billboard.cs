using System;
using UnityEngine;

// Token: 0x020015A8 RID: 5544
public class vp_Billboard : MonoBehaviour
{
	// Token: 0x0600ACF6 RID: 44278 RVA: 0x004077B0 File Offset: 0x004059B0
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Start()
	{
		this.m_Transform = base.transform;
		if (this.m_CameraTransform == null)
		{
			this.m_CameraTransform = Camera.main.transform;
		}
	}

	// Token: 0x0600ACF7 RID: 44279 RVA: 0x004077DC File Offset: 0x004059DC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Update()
	{
		if (this.m_CameraTransform != null)
		{
			this.m_Transform.localEulerAngles = this.m_CameraTransform.eulerAngles;
		}
		this.m_Transform.localEulerAngles = this.m_Transform.localEulerAngles;
	}

	// Token: 0x04007FF4 RID: 32756
	public Transform m_CameraTransform;

	// Token: 0x04007FF5 RID: 32757
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform m_Transform;
}
