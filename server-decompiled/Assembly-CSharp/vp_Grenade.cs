using System;
using UnityEngine;

// Token: 0x020015E3 RID: 5603
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(vp_DamageHandler))]
public class vp_Grenade : MonoBehaviour
{
	// Token: 0x0600AECD RID: 44749 RVA: 0x0041338A File Offset: 0x0041158A
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Awake()
	{
		this.m_Rigidbody = base.GetComponent<Rigidbody>();
	}

	// Token: 0x0600AECE RID: 44750 RVA: 0x00413398 File Offset: 0x00411598
	[PublicizedFrom(EAccessModifier.Private)]
	public void Start()
	{
		vp_Timer.In(this.LifeTime, delegate
		{
			base.transform.SendMessage("DieBySources", new Transform[] { this.m_Source, this.m_OriginalSource }, SendMessageOptions.DontRequireReceiver);
		}, null);
	}

	// Token: 0x0600AECF RID: 44751 RVA: 0x004133B4 File Offset: 0x004115B4
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

	// Token: 0x0600AED0 RID: 44752 RVA: 0x0041342F File Offset: 0x0041162F
	public void SetSource(Transform source)
	{
		this.m_Source = base.transform;
		this.m_OriginalSource = source;
	}

	// Token: 0x04008208 RID: 33288
	public float LifeTime = 3f;

	// Token: 0x04008209 RID: 33289
	public float RigidbodyForce = 10f;

	// Token: 0x0400820A RID: 33290
	public float RigidbodySpin;

	// Token: 0x0400820B RID: 33291
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Rigidbody m_Rigidbody;

	// Token: 0x0400820C RID: 33292
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform m_Source;

	// Token: 0x0400820D RID: 33293
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform m_OriginalSource;
}
