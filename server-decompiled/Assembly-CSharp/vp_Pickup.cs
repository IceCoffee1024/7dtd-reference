using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020015E8 RID: 5608
[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(AudioSource))]
public abstract class vp_Pickup : MonoBehaviour
{
	// Token: 0x0600AF06 RID: 44806 RVA: 0x00413CA4 File Offset: 0x00411EA4
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Start()
	{
		this.m_Transform = base.transform;
		this.m_Rigidbody = base.GetComponent<Rigidbody>();
		this.m_Audio = base.GetComponent<AudioSource>();
		if (Camera.main != null)
		{
			this.m_CameraMainTransform = Camera.main.transform;
		}
		base.GetComponent<Collider>().isTrigger = true;
		this.m_Audio.clip = this.PickupSound;
		this.m_Audio.playOnAwake = false;
		this.m_Audio.minDistance = 3f;
		this.m_Audio.maxDistance = 150f;
		this.m_Audio.rolloffMode = AudioRolloffMode.Linear;
		this.m_Audio.dopplerLevel = 0f;
		this.m_SpawnPosition = this.m_Transform.position;
		this.m_SpawnScale = this.m_Transform.localScale;
		this.RespawnScaleUpDuration = ((this.m_Rigidbody == null) ? Mathf.Abs(this.RespawnScaleUpDuration) : 0f);
		if (this.BobOffset == -1f)
		{
			this.BobOffset = global::UnityEngine.Random.value;
		}
		if (this.RecipientTags.Count == 0)
		{
			this.RecipientTags.Add("Player");
		}
		if (this.RemoveDuration != 0f)
		{
			vp_Timer.In(this.RemoveDuration, new vp_Timer.Callback(this.Remove), null);
		}
		if (this.m_Rigidbody != null)
		{
			if (this.RigidbodyForce != Vector3.zero)
			{
				this.m_Rigidbody.AddForce(this.RigidbodyForce, ForceMode.Impulse);
			}
			if (this.RigidbodySpin != 0f)
			{
				this.m_Rigidbody.AddTorque(global::UnityEngine.Random.rotation.eulerAngles * this.RigidbodySpin);
			}
		}
	}

	// Token: 0x0600AF07 RID: 44807 RVA: 0x00413E5C File Offset: 0x0041205C
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Update()
	{
		this.UpdateMotion();
		if (this.m_Depleted && !this.m_Audio.isPlaying)
		{
			this.Remove();
		}
		if (!this.m_Depleted && this.m_Rigidbody != null && this.m_Rigidbody.IsSleeping() && !this.m_Rigidbody.isKinematic)
		{
			this.m_Rigidbody.isKinematic = true;
			foreach (Collider collider in base.GetComponents<Collider>())
			{
				if (!collider.isTrigger)
				{
					collider.enabled = false;
				}
			}
		}
	}

	// Token: 0x0600AF08 RID: 44808 RVA: 0x00413EF0 File Offset: 0x004120F0
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void UpdateMotion()
	{
		if (this.m_Rigidbody != null)
		{
			return;
		}
		if (this.Billboard)
		{
			if (this.m_CameraMainTransform != null)
			{
				this.m_Transform.localEulerAngles = this.m_CameraMainTransform.eulerAngles;
			}
		}
		else
		{
			this.m_Transform.localEulerAngles += this.Spin * Time.deltaTime;
		}
		if (this.BobRate != 0f && this.BobAmp != 0f)
		{
			this.m_Transform.position = this.m_SpawnPosition + Vector3.up * (Mathf.Cos((Time.time + this.BobOffset) * (this.BobRate * 10f)) * this.BobAmp);
		}
		if (this.m_Transform.localScale != this.m_SpawnScale)
		{
			this.m_Transform.localScale = Vector3.Lerp(this.m_Transform.localScale, this.m_SpawnScale, Time.deltaTime / this.RespawnScaleUpDuration);
		}
	}

	// Token: 0x0600AF09 RID: 44809 RVA: 0x00414004 File Offset: 0x00412204
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnTriggerEnter(Collider col)
	{
		if (this.m_Depleted)
		{
			return;
		}
		foreach (string text in this.RecipientTags)
		{
			if (col.gameObject.tag == text)
			{
				goto IL_004E;
			}
		}
		return;
		IL_004E:
		if (col != this.m_LastCollider)
		{
			this.m_Recipient = col.gameObject.GetComponent<vp_FPPlayerEventHandler>();
		}
		if (this.m_Recipient == null)
		{
			return;
		}
		if (this.TryGive(this.m_Recipient))
		{
			this.m_Audio.pitch = (this.PickupSoundSlomo ? Time.timeScale : 1f);
			this.m_Audio.Play();
			base.GetComponent<Renderer>().enabled = false;
			this.m_Depleted = true;
			this.m_Recipient.HUDText.Send(this.GiveMessage);
			return;
		}
		if (!this.m_AlreadyFailed)
		{
			this.m_Audio.pitch = (this.FailSoundSlomo ? Time.timeScale : 1f);
			this.m_Audio.PlayOneShot(this.PickupFailSound);
			this.m_AlreadyFailed = true;
			this.m_Recipient.HUDText.Send(this.FailMessage);
		}
	}

	// Token: 0x0600AF0A RID: 44810 RVA: 0x00414160 File Offset: 0x00412360
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnTriggerExit(Collider col)
	{
		this.m_AlreadyFailed = false;
	}

	// Token: 0x0600AF0B RID: 44811 RVA: 0x00414169 File Offset: 0x00412369
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool TryGive(vp_FPPlayerEventHandler player)
	{
		return player.AddItem.Try(new object[] { this.InventoryName, 1 });
	}

	// Token: 0x0600AF0C RID: 44812 RVA: 0x00414198 File Offset: 0x00412398
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Remove()
	{
		if (this == null)
		{
			return;
		}
		if (this.RespawnDuration == 0f)
		{
			vp_Utility.Destroy(base.gameObject);
			return;
		}
		if (!this.m_RespawnTimer.Active)
		{
			vp_Utility.Activate(base.gameObject, false);
			vp_Timer.In(this.RespawnDuration, new vp_Timer.Callback(this.Respawn), this.m_RespawnTimer);
		}
	}

	// Token: 0x0600AF0D RID: 44813 RVA: 0x00414200 File Offset: 0x00412400
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Respawn()
	{
		if (this.m_Transform == null)
		{
			return;
		}
		if (Camera.main != null)
		{
			this.m_CameraMainTransform = Camera.main.transform;
		}
		this.m_RespawnTimer.Cancel();
		this.m_Transform.position = this.m_SpawnPosition;
		if (this.m_Rigidbody == null && this.RespawnScaleUpDuration > 0f)
		{
			this.m_Transform.localScale = Vector3.zero;
		}
		base.GetComponent<Renderer>().enabled = true;
		vp_Utility.Activate(base.gameObject, true);
		this.m_Audio.pitch = (this.RespawnSoundSlomo ? Time.timeScale : 1f);
		this.m_Audio.PlayOneShot(this.RespawnSound);
		this.m_Depleted = false;
		if (this.BobOffset == -1f)
		{
			this.BobOffset = global::UnityEngine.Random.value;
		}
		if (this.m_Rigidbody != null)
		{
			this.m_Rigidbody.isKinematic = false;
			foreach (Collider collider in base.GetComponents<Collider>())
			{
				if (!collider.isTrigger)
				{
					collider.enabled = true;
				}
			}
		}
	}

	// Token: 0x0600AF0E RID: 44814 RVA: 0x0041432C File Offset: 0x0041252C
	[PublicizedFrom(EAccessModifier.Protected)]
	public vp_Pickup()
	{
	}

	// Token: 0x040082BD RID: 33469
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform m_Transform;

	// Token: 0x040082BE RID: 33470
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Rigidbody m_Rigidbody;

	// Token: 0x040082BF RID: 33471
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public AudioSource m_Audio;

	// Token: 0x040082C0 RID: 33472
	public string InventoryName = "Unnamed";

	// Token: 0x040082C1 RID: 33473
	public List<string> RecipientTags = new List<string>();

	// Token: 0x040082C2 RID: 33474
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Collider m_LastCollider;

	// Token: 0x040082C3 RID: 33475
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public vp_FPPlayerEventHandler m_Recipient;

	// Token: 0x040082C4 RID: 33476
	public string GiveMessage = "Picked up an item";

	// Token: 0x040082C5 RID: 33477
	public string FailMessage = "You currently can't pick up this item!";

	// Token: 0x040082C6 RID: 33478
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_SpawnPosition = Vector3.zero;

	// Token: 0x040082C7 RID: 33479
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_SpawnScale = Vector3.zero;

	// Token: 0x040082C8 RID: 33480
	public bool Billboard;

	// Token: 0x040082C9 RID: 33481
	public Vector3 Spin = Vector3.zero;

	// Token: 0x040082CA RID: 33482
	public float BobAmp;

	// Token: 0x040082CB RID: 33483
	public float BobRate;

	// Token: 0x040082CC RID: 33484
	public float BobOffset = -1f;

	// Token: 0x040082CD RID: 33485
	public Vector3 RigidbodyForce = Vector3.zero;

	// Token: 0x040082CE RID: 33486
	public float RigidbodySpin;

	// Token: 0x040082CF RID: 33487
	public float RespawnDuration = 10f;

	// Token: 0x040082D0 RID: 33488
	public float RespawnScaleUpDuration;

	// Token: 0x040082D1 RID: 33489
	public float RemoveDuration;

	// Token: 0x040082D2 RID: 33490
	public AudioClip PickupSound;

	// Token: 0x040082D3 RID: 33491
	public AudioClip PickupFailSound;

	// Token: 0x040082D4 RID: 33492
	public AudioClip RespawnSound;

	// Token: 0x040082D5 RID: 33493
	public bool PickupSoundSlomo = true;

	// Token: 0x040082D6 RID: 33494
	public bool FailSoundSlomo = true;

	// Token: 0x040082D7 RID: 33495
	public bool RespawnSoundSlomo = true;

	// Token: 0x040082D8 RID: 33496
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_Depleted;

	// Token: 0x040082D9 RID: 33497
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_AlreadyFailed;

	// Token: 0x040082DA RID: 33498
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Timer.Handle m_RespawnTimer = new vp_Timer.Handle();

	// Token: 0x040082DB RID: 33499
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform m_CameraMainTransform;
}
