using System;
using UnityEngine;

// Token: 0x02001579 RID: 5497
[RequireComponent(typeof(Rigidbody))]
public class vp_RigidbodyImpulse : MonoBehaviour
{
	// Token: 0x0600AC08 RID: 44040 RVA: 0x0040141F File Offset: 0x003FF61F
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Awake()
	{
		this.m_Rigidbody = base.GetComponent<Rigidbody>();
	}

	// Token: 0x0600AC09 RID: 44041 RVA: 0x00401430 File Offset: 0x003FF630
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

	// Token: 0x04007F48 RID: 32584
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Rigidbody m_Rigidbody;

	// Token: 0x04007F49 RID: 32585
	public Vector3 RigidbodyForce = new Vector3(0f, 5f, 0f);

	// Token: 0x04007F4A RID: 32586
	public float RigidbodySpin = 0.2f;
}
