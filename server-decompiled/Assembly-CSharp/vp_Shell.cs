using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020015DC RID: 5596
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(AudioSource))]
public class vp_Shell : MonoBehaviour
{
	// Token: 0x0600AE83 RID: 44675 RVA: 0x00411C5C File Offset: 0x0040FE5C
	[PublicizedFrom(EAccessModifier.Private)]
	public void Awake()
	{
		this.m_Transform = base.transform;
		this.m_Rigidbody = base.GetComponent<Rigidbody>();
		this.m_Audio = base.GetComponent<AudioSource>();
		base.GetComponent<AudioSource>().playOnAwake = false;
		base.GetComponent<AudioSource>().dopplerLevel = 0f;
	}

	// Token: 0x0600AE84 RID: 44676 RVA: 0x00411CAC File Offset: 0x0040FEAC
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

	// Token: 0x0600AE85 RID: 44677 RVA: 0x00411D34 File Offset: 0x0040FF34
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

	// Token: 0x0600AE86 RID: 44678 RVA: 0x00411DC8 File Offset: 0x0040FFC8
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

	// Token: 0x0600AE87 RID: 44679 RVA: 0x00411EC4 File Offset: 0x004100C4
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

	// Token: 0x0600AE88 RID: 44680 RVA: 0x00411F5C File Offset: 0x0041015C
	[PublicizedFrom(EAccessModifier.Protected)]
	public void UpRight()
	{
		this.m_Transform.rotation = Quaternion.Lerp(this.m_Transform.rotation, Quaternion.Euler(-90f, this.m_Transform.rotation.y, this.m_Transform.rotation.z), Time.time * (Time.deltaTime * 60f * 0.05f));
	}

	// Token: 0x0600AE89 RID: 44681 RVA: 0x00411FC8 File Offset: 0x004101C8
	[PublicizedFrom(EAccessModifier.Protected)]
	public void TippedOver()
	{
		this.m_Transform.localRotation = Quaternion.Lerp(this.m_Transform.localRotation, Quaternion.Euler(0f, this.m_Transform.localEulerAngles.y, this.m_Transform.localEulerAngles.z), Time.time * (Time.deltaTime * 60f * 0.005f));
	}

	// Token: 0x040081BC RID: 33212
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform m_Transform;

	// Token: 0x040081BD RID: 33213
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Rigidbody m_Rigidbody;

	// Token: 0x040081BE RID: 33214
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public AudioSource m_Audio;

	// Token: 0x040081BF RID: 33215
	public float LifeTime = 10f;

	// Token: 0x040081C0 RID: 33216
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_RemoveTime;

	// Token: 0x040081C1 RID: 33217
	public float m_Persistence = 1f;

	// Token: 0x040081C2 RID: 33218
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Shell.RestAngleFunc m_RestAngleFunc;

	// Token: 0x040081C3 RID: 33219
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_RestTime;

	// Token: 0x040081C4 RID: 33220
	public List<AudioClip> m_BounceSounds = new List<AudioClip>();

	// Token: 0x020015DD RID: 5597
	// (Invoke) Token: 0x0600AE8C RID: 44684
	public delegate void RestAngleFunc();
}
