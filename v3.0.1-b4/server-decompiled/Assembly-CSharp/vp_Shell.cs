using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200158D RID: 5517
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(AudioSource))]
public class vp_Shell : MonoBehaviour
{
	// Token: 0x0600AC7B RID: 44155 RVA: 0x004056C4 File Offset: 0x004038C4
	[PublicizedFrom(EAccessModifier.Private)]
	public void Awake()
	{
		this.m_Transform = base.transform;
		this.m_Rigidbody = base.GetComponent<Rigidbody>();
		this.m_Audio = base.GetComponent<AudioSource>();
		base.GetComponent<AudioSource>().playOnAwake = false;
		base.GetComponent<AudioSource>().dopplerLevel = 0f;
	}

	// Token: 0x0600AC7C RID: 44156 RVA: 0x00405714 File Offset: 0x00403914
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

	// Token: 0x0600AC7D RID: 44157 RVA: 0x0040579C File Offset: 0x0040399C
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

	// Token: 0x0600AC7E RID: 44158 RVA: 0x00405830 File Offset: 0x00403A30
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

	// Token: 0x0600AC7F RID: 44159 RVA: 0x0040592C File Offset: 0x00403B2C
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

	// Token: 0x0600AC80 RID: 44160 RVA: 0x004059C4 File Offset: 0x00403BC4
	[PublicizedFrom(EAccessModifier.Protected)]
	public void UpRight()
	{
		this.m_Transform.rotation = Quaternion.Lerp(this.m_Transform.rotation, Quaternion.Euler(-90f, this.m_Transform.rotation.y, this.m_Transform.rotation.z), Time.time * (Time.deltaTime * 60f * 0.05f));
	}

	// Token: 0x0600AC81 RID: 44161 RVA: 0x00405A30 File Offset: 0x00403C30
	[PublicizedFrom(EAccessModifier.Protected)]
	public void TippedOver()
	{
		this.m_Transform.localRotation = Quaternion.Lerp(this.m_Transform.localRotation, Quaternion.Euler(0f, this.m_Transform.localEulerAngles.y, this.m_Transform.localEulerAngles.z), Time.time * (Time.deltaTime * 60f * 0.005f));
	}

	// Token: 0x0400801F RID: 32799
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform m_Transform;

	// Token: 0x04008020 RID: 32800
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Rigidbody m_Rigidbody;

	// Token: 0x04008021 RID: 32801
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public AudioSource m_Audio;

	// Token: 0x04008022 RID: 32802
	public float LifeTime = 10f;

	// Token: 0x04008023 RID: 32803
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_RemoveTime;

	// Token: 0x04008024 RID: 32804
	public float m_Persistence = 1f;

	// Token: 0x04008025 RID: 32805
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Shell.RestAngleFunc m_RestAngleFunc;

	// Token: 0x04008026 RID: 32806
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_RestTime;

	// Token: 0x04008027 RID: 32807
	public List<AudioClip> m_BounceSounds = new List<AudioClip>();

	// Token: 0x0200158E RID: 5518
	// (Invoke) Token: 0x0600AC84 RID: 44164
	public delegate void RestAngleFunc();
}
