using System;
using UnityEngine;

// Token: 0x02001574 RID: 5492
public class vp_Billboard : MonoBehaviour
{
	// Token: 0x0600ABEB RID: 44011 RVA: 0x00400558 File Offset: 0x003FE758
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Start()
	{
		this.m_Transform = base.transform;
		if (this.m_CameraTransform == null)
		{
			this.m_CameraTransform = Camera.main.transform;
		}
	}

	// Token: 0x0600ABEC RID: 44012 RVA: 0x00400584 File Offset: 0x003FE784
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Update()
	{
		if (this.m_CameraTransform != null)
		{
			this.m_Transform.localEulerAngles = this.m_CameraTransform.eulerAngles;
		}
		this.m_Transform.localEulerAngles = this.m_Transform.localEulerAngles;
	}

	// Token: 0x04007F30 RID: 32560
	public Transform m_CameraTransform;

	// Token: 0x04007F31 RID: 32561
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform m_Transform;
}
