using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020015CA RID: 5578
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(AudioSource))]
public class vp_Shell : MonoBehaviour
{
	// Token: 0x0600AE2B RID: 44587 RVA: 0x0040F858 File Offset: 0x0040DA58
	[PublicizedFrom(EAccessModifier.Private)]
	public void Awake()
	{
		this.m_Transform = base.transform;
		this.m_Rigidbody = base.GetComponent<Rigidbody>();
		this.m_Audio = base.GetComponent<AudioSource>();
		base.GetComponent<AudioSource>().playOnAwake = false;
		base.GetComponent<AudioSource>().dopplerLevel = 0f;
	}

	// Token: 0x0600AE2C RID: 44588 RVA: 0x0040F8A8 File Offset: 0x0040DAA8
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnEnable()
	{
		this.m_RestAngleFunc = null;
		this.m_RemoveTime = Time.time + this.LifeTime;
		this.m_RestTime = Time.time + this.LifeTime * 0.25f;
		this.m_Rigidbody.maxAngularVelocity = 100f;
		this.m_Rigidbody.velocity = Vector3.zero;
		this.m_Rigidbody.angularVelocity = Vector3.zero;
		this.m_Rigidbody.constraints = RigidbodyConstraints.None;
		base.GetComponent<Collider>().enabled = true;
	}

	// Token: 0x0600AE2D RID: 44589 RVA: 0x0040F930 File Offset: 0x0040DB30
	[PublicizedFrom(EAccessModifier.Private)]
	public void Update()
	{
		if (this.m_RestAngleFunc == null)
		{
			if (Time.time > this.m_RestTime)
			{
				this.DecideRestAngle();
			}
		}
		else
		{
			this.m_RestAngleFunc();
		}
		if (Time.time > this.m_RemoveTime)
		{
			this.m_Transform.localScale = Vector3.Lerp(this.m_Transform.localScale, Vector3.zero, Time.deltaTime * 60f * 0.2f);
			if (Time.time > this.m_RemoveTime + 0.5f)
			{
				vp_Utility.Destroy(base.gameObject);
			}
		}
	}

	// Token: 0x0600AE2E RID: 44590 RVA: 0x0040F9C4 File Offset: 0x0040DBC4
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnCollisionEnter(Collision collision)
	{
		if (collision.relativeVelocity.magnitude > 2f)
		{
			if (global::UnityEngine.Random.value > 0.5f)
			{
				this.m_Rigidbody.AddRelativeTorque(-global::UnityEngine.Random.rotation.eulerAngles * 0.15f);
			}
			else
			{
				this.m_Rigidbody.AddRelativeTorque(global::UnityEngine.Random.rotation.eulerAngles * 0.15f);
			}
			if (this.m_Audio != null && this.m_BounceSounds.Count > 0)
			{
				this.m_Audio.pitch = Time.timeScale;
				this.m_Audio.PlayOneShot(this.m_BounceSounds[global::UnityEngine.Random.Range(0, this.m_BounceSounds.Count)]);
				return;
			}
		}
		else if (global::UnityEngine.Random.value > this.m_Persistence)
		{
			base.GetComponent<Collider>().enabled = false;
			this.m_RemoveTime = Time.time + 0.5f;
		}
	}

	// Token: 0x0600AE2F RID: 44591 RVA: 0x0040FAC0 File Offset: 0x0040DCC0
	[PublicizedFrom(EAccessModifier.Protected)]
	public void DecideRestAngle()
	{
		if (Mathf.Abs(this.m_Transform.eulerAngles.x - 270f) < 55f)
		{
			RaycastHit raycastHit;
			if (Physics.Raycast(new Ray(this.m_Transform.position, Vector3.down), out raycastHit, 1f) && raycastHit.normal == Vector3.up)
			{
				this.m_RestAngleFunc = new vp_Shell.RestAngleFunc(this.UpRight);
				this.m_Rigidbody.constraints = (RigidbodyConstraints)80;
			}
			return;
		}
		this.m_RestAngleFunc = new vp_Shell.RestAngleFunc(this.TippedOver);
	}

	// Token: 0x0600AE30 RID: 44592 RVA: 0x0040FB58 File Offset: 0x0040DD58
	[PublicizedFrom(EAccessModifier.Protected)]
	public void UpRight()
	{
		this.m_Transform.rotation = Quaternion.Lerp(this.m_Transform.rotation, Quaternion.Euler(-90f, this.m_Transform.rotation.y, this.m_Transform.rotation.z), Time.time * (Time.deltaTime * 60f * 0.05f));
	}

	// Token: 0x0600AE31 RID: 44593 RVA: 0x0040FBC4 File Offset: 0x0040DDC4
	[PublicizedFrom(EAccessModifier.Protected)]
	public void TippedOver()
	{
		this.m_Transform.localRotation = Quaternion.Lerp(this.m_Transform.localRotation, Quaternion.Euler(0f, this.m_Transform.localEulerAngles.y, this.m_Transform.localEulerAngles.z), Time.time * (Time.deltaTime * 60f * 0.005f));
	}

	// Token: 0x0400817B RID: 33147
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform m_Transform;

	// Token: 0x0400817C RID: 33148
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Rigidbody m_Rigidbody;

	// Token: 0x0400817D RID: 33149
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public AudioSource m_Audio;

	// Token: 0x0400817E RID: 33150
	public float LifeTime = 10f;

	// Token: 0x0400817F RID: 33151
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_RemoveTime;

	// Token: 0x04008180 RID: 33152
	public float m_Persistence = 1f;

	// Token: 0x04008181 RID: 33153
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Shell.RestAngleFunc m_RestAngleFunc;

	// Token: 0x04008182 RID: 33154
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_RestTime;

	// Token: 0x04008183 RID: 33155
	public List<AudioClip> m_BounceSounds = new List<AudioClip>();

	// Token: 0x020015CB RID: 5579
	// (Invoke) Token: 0x0600AE34 RID: 44596
	public delegate void RestAngleFunc();
}
