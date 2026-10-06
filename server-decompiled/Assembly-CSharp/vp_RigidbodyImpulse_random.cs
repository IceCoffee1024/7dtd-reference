using System;
using UnityEngine;

// Token: 0x0200159C RID: 5532
[RequireComponent(typeof(Rigidbody))]
public class vp_RigidbodyImpulse_random : MonoBehaviour
{
	// Token: 0x0600ACBE RID: 44222 RVA: 0x00406321 File Offset: 0x00404521
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Awake()
	{
		this.m_Rigidbody = base.GetComponent<Rigidbody>();
	}

	// Token: 0x0600ACBF RID: 44223 RVA: 0x00406330 File Offset: 0x00404530
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

	// Token: 0x04007FCE RID: 32718
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Rigidbody m_Rigidbody;

	// Token: 0x04007FCF RID: 32719
	public float minRigidBodySpin = 0.2f;

	// Token: 0x04007FD0 RID: 32720
	public float maxRigidBodySpin = 0.2f;

	// Token: 0x04007FD1 RID: 32721
	public Vector3 minForce = new Vector3(0f, 0f, 0f);

	// Token: 0x04007FD2 RID: 32722
	public Vector3 maxForce = new Vector3(0f, 0f, 0f);
}
