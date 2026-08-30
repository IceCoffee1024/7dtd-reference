using System;
using UnityEngine;

// Token: 0x02001559 RID: 5465
public class vp_Billboard : MonoBehaviour
{
	// Token: 0x0600AAEE RID: 43758 RVA: 0x003FB218 File Offset: 0x003F9418
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Start()
	{
		this.m_Transform = base.transform;
		if (this.m_CameraTransform == null)
		{
			this.m_CameraTransform = Camera.main.transform;
		}
	}

	// Token: 0x0600AAEF RID: 43759 RVA: 0x003FB244 File Offset: 0x003F9444
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Update()
	{
		if (this.m_CameraTransform != null)
		{
			this.m_Transform.localEulerAngles = this.m_CameraTransform.eulerAngles;
		}
		this.m_Transform.localEulerAngles = this.m_Transform.localEulerAngles;
	}

	// Token: 0x04007E57 RID: 32343
	public Transform m_CameraTransform;

	// Token: 0x04007E58 RID: 32344
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform m_Transform;
}
