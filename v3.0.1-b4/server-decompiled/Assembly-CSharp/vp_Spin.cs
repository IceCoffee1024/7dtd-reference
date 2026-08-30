using System;
using UnityEngine;

// Token: 0x02001560 RID: 5472
public class vp_Spin : MonoBehaviour
{
	// Token: 0x0600AB11 RID: 43793 RVA: 0x003FC2C1 File Offset: 0x003FA4C1
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Start()
	{
		this.m_Transform = base.transform;
	}

	// Token: 0x0600AB12 RID: 43794 RVA: 0x003FC2CF File Offset: 0x003FA4CF
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Update()
	{
		this.m_Transform.Rotate(this.RotationSpeed * Time.deltaTime);
	}

	// Token: 0x04007E77 RID: 32375
	public Vector3 RotationSpeed = new Vector3(0f, 90f, 0f);

	// Token: 0x04007E78 RID: 32376
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform m_Transform;
}
