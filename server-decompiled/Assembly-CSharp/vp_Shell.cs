using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020015A8 RID: 5544
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(AudioSource))]
public class vp_Shell : MonoBehaviour
{
	// Token: 0x0600AD78 RID: 44408 RVA: 0x0040AA04 File Offset: 0x00408C04
	[PublicizedFrom(EAccessModifier.Private)]
	public void Awake()
	{
		this.m_Transform = base.transform;
		this.m_Rigidbody = base.GetComponent<Rigidbody>();
		this.m_Audio = base.GetComponent<AudioSource>();
		base.GetComponent<AudioSource>().playOnAwake = false;
		base.GetComponent<AudioSource>().dopplerLevel = 0f;
	}

	// Token: 0x0600AD79 RID: 44409 RVA: 0x0040AA54 File Offset: 0x00408C54
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

	// Token: 0x0600AD7A RID: 44410 RVA: 0x0040AADC File Offset: 0x00408CDC
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

	// Token: 0x0600AD7B RID: 44411 RVA: 0x0040AB70 File Offset: 0x00408D70
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

	// Token: 0x0600AD7C RID: 44412 RVA: 0x0040AC6C File Offset: 0x00408E6C
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

	// Token: 0x0600AD7D RID: 44413 RVA: 0x0040AD04 File Offset: 0x00408F04
	[PublicizedFrom(EAccessModifier.Protected)]
	public void UpRight()
	{
		this.m_Transform.rotation = Quaternion.Lerp(this.m_Transform.rotation, Quaternion.Euler(-90f, this.m_Transform.rotation.y, this.m_Transform.rotation.z), Time.time * (Time.deltaTime * 60f * 0.05f));
	}

	// Token: 0x0600AD7E RID: 44414 RVA: 0x0040AD70 File Offset: 0x00408F70
	[PublicizedFrom(EAccessModifier.Protected)]
	public void TippedOver()
	{
		this.m_Transform.localRotation = Quaternion.Lerp(this.m_Transform.localRotation, Quaternion.Euler(0f, this.m_Transform.localEulerAngles.y, this.m_Transform.localEulerAngles.z), Time.time * (Time.deltaTime * 60f * 0.005f));
	}

	// Token: 0x040080F8 RID: 33016
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform m_Transform;

	// Token: 0x040080F9 RID: 33017
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Rigidbody m_Rigidbody;

	// Token: 0x040080FA RID: 33018
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public AudioSource m_Audio;

	// Token: 0x040080FB RID: 33019
	public float LifeTime = 10f;

	// Token: 0x040080FC RID: 33020
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_RemoveTime;

	// Token: 0x040080FD RID: 33021
	public float m_Persistence = 1f;

	// Token: 0x040080FE RID: 33022
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Shell.RestAngleFunc m_RestAngleFunc;

	// Token: 0x040080FF RID: 33023
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_RestTime;

	// Token: 0x04008100 RID: 33024
	public List<AudioClip> m_BounceSounds = new List<AudioClip>();

	// Token: 0x020015A9 RID: 5545
	// (Invoke) Token: 0x0600AD81 RID: 44417
	public delegate void RestAngleFunc();
}
