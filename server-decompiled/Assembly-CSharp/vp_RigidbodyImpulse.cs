using System;
using UnityEngine;

// Token: 0x020015AD RID: 5549
[RequireComponent(typeof(Rigidbody))]
public class vp_RigidbodyImpulse : MonoBehaviour
{
	// Token: 0x0600AD13 RID: 44307 RVA: 0x00408677 File Offset: 0x00406877
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Awake()
	{
		this.m_Rigidbody = base.GetComponent<Rigidbody>();
	}

	// Token: 0x0600AD14 RID: 44308 RVA: 0x00408688 File Offset: 0x00406888
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnEnable()
	{
		if (this.m_Rigidbody == null)
		{
			return;
		}
		if (this.RigidbodyForce != Vector3.zero)
		{
			this.m_Rigidbody.AddForce(this.RigidbodyForce, ForceMode.Impulse);
		}
		if (this.RigidbodySpin != 0f)
		{
			this.m_Rigidbody.AddTorque(global::UnityEngine.Random.rotation.eulerAngles * this.RigidbodySpin);
		}
	}

	// Token: 0x0400800C RID: 32780
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Rigidbody m_Rigidbody;

	// Token: 0x0400800D RID: 32781
	public Vector3 RigidbodyForce = new Vector3(0f, 5f, 0f);

	// Token: 0x0400800E RID: 32782
	public float RigidbodySpin = 0.2f;
}
