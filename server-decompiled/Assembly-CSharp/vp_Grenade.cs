using System;
using UnityEngine;

// Token: 0x020015AF RID: 5551
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(vp_DamageHandler))]
public class vp_Grenade : MonoBehaviour
{
	// Token: 0x0600ADC2 RID: 44482 RVA: 0x0040C132 File Offset: 0x0040A332
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Awake()
	{
		this.m_Rigidbody = base.GetComponent<Rigidbody>();
	}

	// Token: 0x0600ADC3 RID: 44483 RVA: 0x0040C140 File Offset: 0x0040A340
	[PublicizedFrom(EAccessModifier.Private)]
	public void Start()
	{
		vp_Timer.In(this.LifeTime, delegate
		{
			base.transform.SendMessage("DieBySources", new Transform[] { this.m_Source, this.m_OriginalSource }, SendMessageOptions.DontRequireReceiver);
		}, null);
	}

	// Token: 0x0600ADC4 RID: 44484 RVA: 0x0040C15C File Offset: 0x0040A35C
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnEnable()
	{
		if (this.m_Rigidbody == null)
		{
			return;
		}
		if (this.RigidbodyForce != 0f)
		{
			this.m_Rigidbody.AddForce(base.transform.forward * this.RigidbodyForce, ForceMode.Impulse);
		}
		if (this.RigidbodySpin != 0f)
		{
			this.m_Rigidbody.AddTorque(global::UnityEngine.Random.rotation.eulerAngles * this.RigidbodySpin);
		}
	}

	// Token: 0x0600ADC5 RID: 44485 RVA: 0x0040C1D7 File Offset: 0x0040A3D7
	public void SetSource(Transform source)
	{
		this.m_Source = base.transform;
		this.m_OriginalSource = source;
	}

	// Token: 0x04008144 RID: 33092
	public float LifeTime = 3f;

	// Token: 0x04008145 RID: 33093
	public float RigidbodyForce = 10f;

	// Token: 0x04008146 RID: 33094
	public float RigidbodySpin;

	// Token: 0x04008147 RID: 33095
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Rigidbody m_Rigidbody;

	// Token: 0x04008148 RID: 33096
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform m_Source;

	// Token: 0x04008149 RID: 33097
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform m_OriginalSource;
}
