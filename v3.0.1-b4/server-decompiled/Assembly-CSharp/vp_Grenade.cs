using System;
using UnityEngine;

// Token: 0x02001594 RID: 5524
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(vp_DamageHandler))]
public class vp_Grenade : MonoBehaviour
{
	// Token: 0x0600ACC5 RID: 44229 RVA: 0x00406DF2 File Offset: 0x00404FF2
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Awake()
	{
		this.m_Rigidbody = base.GetComponent<Rigidbody>();
	}

	// Token: 0x0600ACC6 RID: 44230 RVA: 0x00406E00 File Offset: 0x00405000
	[PublicizedFrom(EAccessModifier.Private)]
	public void Start()
	{
		vp_Timer.In(this.LifeTime, delegate
		{
			base.transform.SendMessage("DieBySources", new Transform[] { this.m_Source, this.m_OriginalSource }, SendMessageOptions.DontRequireReceiver);
		}, null);
	}

	// Token: 0x0600ACC7 RID: 44231 RVA: 0x00406E1C File Offset: 0x0040501C
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

	// Token: 0x0600ACC8 RID: 44232 RVA: 0x00406E97 File Offset: 0x00405097
	public void SetSource(Transform source)
	{
		this.m_Source = base.transform;
		this.m_OriginalSource = source;
	}

	// Token: 0x0400806B RID: 32875
	public float LifeTime = 3f;

	// Token: 0x0400806C RID: 32876
	public float RigidbodyForce = 10f;

	// Token: 0x0400806D RID: 32877
	public float RigidbodySpin;

	// Token: 0x0400806E RID: 32878
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Rigidbody m_Rigidbody;

	// Token: 0x0400806F RID: 32879
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform m_Source;

	// Token: 0x04008070 RID: 32880
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform m_OriginalSource;
}
