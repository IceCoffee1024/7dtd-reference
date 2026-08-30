using System;
using UnityEngine;

// Token: 0x0200155E RID: 5470
[RequireComponent(typeof(Rigidbody))]
public class vp_RigidbodyImpulse : MonoBehaviour
{
	// Token: 0x0600AB0B RID: 43787 RVA: 0x003FC0DF File Offset: 0x003FA2DF
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Awake()
	{
		this.m_Rigidbody = base.GetComponent<Rigidbody>();
	}

	// Token: 0x0600AB0C RID: 43788 RVA: 0x003FC0F0 File Offset: 0x003FA2F0
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

	// Token: 0x04007E6F RID: 32367
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Rigidbody m_Rigidbody;

	// Token: 0x04007E70 RID: 32368
	public Vector3 RigidbodyForce = new Vector3(0f, 5f, 0f);

	// Token: 0x04007E71 RID: 32369
	public float RigidbodySpin = 0.2f;
}
