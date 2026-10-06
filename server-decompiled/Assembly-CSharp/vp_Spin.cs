using System;
using UnityEngine;

// Token: 0x0200159D RID: 5533
public class vp_Spin : MonoBehaviour
{
	// Token: 0x0600ACC1 RID: 44225 RVA: 0x00406455 File Offset: 0x00404655
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Start()
	{
		this.m_Transform = base.transform;
	}

	// Token: 0x0600ACC2 RID: 44226 RVA: 0x00406463 File Offset: 0x00404663
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Update()
	{
		this.m_Transform.Rotate(this.RotationSpeed * Time.deltaTime);
	}

	// Token: 0x04007FD3 RID: 32723
	public Vector3 RotationSpeed = new Vector3(0f, 90f, 0f);

	// Token: 0x04007FD4 RID: 32724
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform m_Transform;
}
