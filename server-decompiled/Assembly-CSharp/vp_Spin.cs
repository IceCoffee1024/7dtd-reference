using System;
using UnityEngine;

// Token: 0x0200157B RID: 5499
public class vp_Spin : MonoBehaviour
{
	// Token: 0x0600AC0E RID: 44046 RVA: 0x00401601 File Offset: 0x003FF801
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Start()
	{
		this.m_Transform = base.transform;
	}

	// Token: 0x0600AC0F RID: 44047 RVA: 0x0040160F File Offset: 0x003FF80F
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Update()
	{
		this.m_Transform.Rotate(this.RotationSpeed * Time.deltaTime);
	}

	// Token: 0x04007F50 RID: 32592
	public Vector3 RotationSpeed = new Vector3(0f, 90f, 0f);

	// Token: 0x04007F51 RID: 32593
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform m_Transform;
}
