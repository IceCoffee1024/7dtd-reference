using System;
using UnityEngine;

// Token: 0x020015AF RID: 5551
public class vp_Spin : MonoBehaviour
{
	// Token: 0x0600AD19 RID: 44313 RVA: 0x00408859 File Offset: 0x00406A59
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Start()
	{
		this.m_Transform = base.transform;
	}

	// Token: 0x0600AD1A RID: 44314 RVA: 0x00408867 File Offset: 0x00406A67
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Update()
	{
		this.m_Transform.Rotate(this.RotationSpeed * Time.deltaTime);
	}

	// Token: 0x04008014 RID: 32788
	public Vector3 RotationSpeed = new Vector3(0f, 90f, 0f);

	// Token: 0x04008015 RID: 32789
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform m_Transform;
}
