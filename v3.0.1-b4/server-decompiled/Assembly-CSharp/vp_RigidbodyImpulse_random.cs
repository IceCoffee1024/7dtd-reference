using System;
using UnityEngine;

// Token: 0x0200155F RID: 5471
[RequireComponent(typeof(Rigidbody))]
public class vp_RigidbodyImpulse_random : MonoBehaviour
{
	// Token: 0x0600AB0E RID: 43790 RVA: 0x003FC18D File Offset: 0x003FA38D
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Awake()
	{
		this.m_Rigidbody = base.GetComponent<Rigidbody>();
	}

	// Token: 0x0600AB0F RID: 43791 RVA: 0x003FC19C File Offset: 0x003FA39C
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

	// Token: 0x04007E72 RID: 32370
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Rigidbody m_Rigidbody;

	// Token: 0x04007E73 RID: 32371
	public float minRigidBodySpin = 0.2f;

	// Token: 0x04007E74 RID: 32372
	public float maxRigidBodySpin = 0.2f;

	// Token: 0x04007E75 RID: 32373
	public Vector3 minForce = new Vector3(0f, 0f, 0f);

	// Token: 0x04007E76 RID: 32374
	public Vector3 maxForce = new Vector3(0f, 0f, 0f);
}
