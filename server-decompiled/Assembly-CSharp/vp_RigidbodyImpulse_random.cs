using System;
using UnityEngine;

// Token: 0x020015AE RID: 5550
[RequireComponent(typeof(Rigidbody))]
public class vp_RigidbodyImpulse_random : MonoBehaviour
{
	// Token: 0x0600AD16 RID: 44310 RVA: 0x00408725 File Offset: 0x00406925
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Awake()
	{
		this.m_Rigidbody = base.GetComponent<Rigidbody>();
	}

	// Token: 0x0600AD17 RID: 44311 RVA: 0x00408734 File Offset: 0x00406934
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnEnable()
	{
		if (this.m_Rigidbody == null)
		{
			return;
		}
		Vector3 vector = new Vector3(global::UnityEngine.Random.Range(this.minForce.x, this.maxForce.x), global::UnityEngine.Random.Range(this.minForce.y, this.maxForce.y), global::UnityEngine.Random.Range(this.minForce.z, this.maxForce.z));
		float num = global::UnityEngine.Random.Range(this.minRigidBodySpin, this.maxRigidBodySpin);
		if (vector != Vector3.zero)
		{
			this.m_Rigidbody.AddForce(vector, ForceMode.Impulse);
		}
		if (num != 0f)
		{
			this.m_Rigidbody.AddTorque(global::UnityEngine.Random.rotation.eulerAngles * num);
		}
	}

	// Token: 0x0400800F RID: 32783
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Rigidbody m_Rigidbody;

	// Token: 0x04008010 RID: 32784
	public float minRigidBodySpin = 0.2f;

	// Token: 0x04008011 RID: 32785
	public float maxRigidBodySpin = 0.2f;

	// Token: 0x04008012 RID: 32786
	public Vector3 minForce = new Vector3(0f, 0f, 0f);

	// Token: 0x04008013 RID: 32787
	public Vector3 maxForce = new Vector3(0f, 0f, 0f);
}
