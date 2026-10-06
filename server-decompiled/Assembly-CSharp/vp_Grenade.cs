using System;
using UnityEngine;

// Token: 0x020015D1 RID: 5585
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(vp_DamageHandler))]
public class vp_Grenade : MonoBehaviour
{
	// Token: 0x0600AE75 RID: 44661 RVA: 0x00410F86 File Offset: 0x0040F186
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Awake()
	{
		this.m_Rigidbody = base.GetComponent<Rigidbody>();
	}

	// Token: 0x0600AE76 RID: 44662 RVA: 0x00410F94 File Offset: 0x0040F194
	[PublicizedFrom(EAccessModifier.Private)]
	public void Start()
	{
		vp_Timer.In(this.LifeTime, delegate
		{
			base.transform.SendMessage("DieBySources", new Transform[] { this.m_Source, this.m_OriginalSource }, SendMessageOptions.DontRequireReceiver);
		}, null);
	}

	// Token: 0x0600AE77 RID: 44663 RVA: 0x00410FB0 File Offset: 0x0040F1B0
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

	// Token: 0x0600AE78 RID: 44664 RVA: 0x0041102B File Offset: 0x0040F22B
	public void SetSource(Transform source)
	{
		this.m_Source = base.transform;
		this.m_OriginalSource = source;
	}

	// Token: 0x040081C7 RID: 33223
	public float LifeTime = 3f;

	// Token: 0x040081C8 RID: 33224
	public float RigidbodyForce = 10f;

	// Token: 0x040081C9 RID: 33225
	public float RigidbodySpin;

	// Token: 0x040081CA RID: 33226
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Rigidbody m_Rigidbody;

	// Token: 0x040081CB RID: 33227
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform m_Source;

	// Token: 0x040081CC RID: 33228
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform m_OriginalSource;
}
