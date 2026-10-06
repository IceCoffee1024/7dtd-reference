using System;
using UnityEngine;

// Token: 0x0200159B RID: 5531
[RequireComponent(typeof(Rigidbody))]
public class vp_RigidbodyImpulse : MonoBehaviour
{
	// Token: 0x0600ACBB RID: 44219 RVA: 0x00406273 File Offset: 0x00404473
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Awake()
	{
		this.m_Rigidbody = base.GetComponent<Rigidbody>();
	}

	// Token: 0x0600ACBC RID: 44220 RVA: 0x00406284 File Offset: 0x00404484
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

	// Token: 0x04007FCB RID: 32715
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Rigidbody m_Rigidbody;

	// Token: 0x04007FCC RID: 32716
	public Vector3 RigidbodyForce = new Vector3(0f, 5f, 0f);

	// Token: 0x04007FCD RID: 32717
	public float RigidbodySpin = 0.2f;
}
