using System;
using UnityEngine;

// Token: 0x0200157A RID: 5498
[RequireComponent(typeof(Rigidbody))]
public class vp_RigidbodyImpulse_random : MonoBehaviour
{
	// Token: 0x0600AC0B RID: 44043 RVA: 0x004014CD File Offset: 0x003FF6CD
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Awake()
	{
		this.m_Rigidbody = base.GetComponent<Rigidbody>();
	}

	// Token: 0x0600AC0C RID: 44044 RVA: 0x004014DC File Offset: 0x003FF6DC
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

	// Token: 0x04007F4B RID: 32587
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Rigidbody m_Rigidbody;

	// Token: 0x04007F4C RID: 32588
	public float minRigidBodySpin = 0.2f;

	// Token: 0x04007F4D RID: 32589
	public float maxRigidBodySpin = 0.2f;

	// Token: 0x04007F4E RID: 32590
	public Vector3 minForce = new Vector3(0f, 0f, 0f);

	// Token: 0x04007F4F RID: 32591
	public Vector3 maxForce = new Vector3(0f, 0f, 0f);
}
