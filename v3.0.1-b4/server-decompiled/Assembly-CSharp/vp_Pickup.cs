using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020015CD RID: 5581
[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(AudioSource))]
public abstract class vp_Pickup : MonoBehaviour
{
	// Token: 0x0600AE09 RID: 44553 RVA: 0x0040E964 File Offset: 0x0040CB64
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

	// Token: 0x0600AE0A RID: 44554 RVA: 0x0040EB1C File Offset: 0x0040CD1C
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

	// Token: 0x0600AE0B RID: 44555 RVA: 0x0040EBB0 File Offset: 0x0040CDB0
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

	// Token: 0x0600AE0C RID: 44556 RVA: 0x0040ECC4 File Offset: 0x0040CEC4
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

	// Token: 0x0600AE0D RID: 44557 RVA: 0x0040EE20 File Offset: 0x0040D020
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnTriggerExit(Collider col)
	{
		this.m_AlreadyFailed = false;
	}

	// Token: 0x0600AE0E RID: 44558 RVA: 0x0040EE29 File Offset: 0x0040D029
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool TryGive(vp_FPPlayerEventHandler player)
	{
		return player.AddItem.Try(new object[] { this.InventoryName, 1 });
	}

	// Token: 0x0600AE0F RID: 44559 RVA: 0x0040EE58 File Offset: 0x0040D058
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

	// Token: 0x0600AE10 RID: 44560 RVA: 0x0040EEC0 File Offset: 0x0040D0C0
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

	// Token: 0x0600AE11 RID: 44561 RVA: 0x0040EFEC File Offset: 0x0040D1EC
	[PublicizedFrom(EAccessModifier.Protected)]
	public vp_Pickup()
	{
	}

	// Token: 0x040081E4 RID: 33252
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform m_Transform;

	// Token: 0x040081E5 RID: 33253
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Rigidbody m_Rigidbody;

	// Token: 0x040081E6 RID: 33254
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public AudioSource m_Audio;

	// Token: 0x040081E7 RID: 33255
	public string InventoryName = "Unnamed";

	// Token: 0x040081E8 RID: 33256
	public List<string> RecipientTags = new List<string>();

	// Token: 0x040081E9 RID: 33257
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Collider m_LastCollider;

	// Token: 0x040081EA RID: 33258
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public vp_FPPlayerEventHandler m_Recipient;

	// Token: 0x040081EB RID: 33259
	public string GiveMessage = "Picked up an item";

	// Token: 0x040081EC RID: 33260
	public string FailMessage = "You currently can't pick up this item!";

	// Token: 0x040081ED RID: 33261
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_SpawnPosition = Vector3.zero;

	// Token: 0x040081EE RID: 33262
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_SpawnScale = Vector3.zero;

	// Token: 0x040081EF RID: 33263
	public bool Billboard;

	// Token: 0x040081F0 RID: 33264
	public Vector3 Spin = Vector3.zero;

	// Token: 0x040081F1 RID: 33265
	public float BobAmp;

	// Token: 0x040081F2 RID: 33266
	public float BobRate;

	// Token: 0x040081F3 RID: 33267
	public float BobOffset = -1f;

	// Token: 0x040081F4 RID: 33268
	public Vector3 RigidbodyForce = Vector3.zero;

	// Token: 0x040081F5 RID: 33269
	public float RigidbodySpin;

	// Token: 0x040081F6 RID: 33270
	public float RespawnDuration = 10f;

	// Token: 0x040081F7 RID: 33271
	public float RespawnScaleUpDuration;

	// Token: 0x040081F8 RID: 33272
	public float RemoveDuration;

	// Token: 0x040081F9 RID: 33273
	public AudioClip PickupSound;

	// Token: 0x040081FA RID: 33274
	public AudioClip PickupFailSound;

	// Token: 0x040081FB RID: 33275
	public AudioClip RespawnSound;

	// Token: 0x040081FC RID: 33276
	public bool PickupSoundSlomo = true;

	// Token: 0x040081FD RID: 33277
	public bool FailSoundSlomo = true;

	// Token: 0x040081FE RID: 33278
	public bool RespawnSoundSlomo = true;

	// Token: 0x040081FF RID: 33279
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_Depleted;

	// Token: 0x04008200 RID: 33280
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_AlreadyFailed;

	// Token: 0x04008201 RID: 33281
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Timer.Handle m_RespawnTimer = new vp_Timer.Handle();

	// Token: 0x04008202 RID: 33282
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform m_CameraMainTransform;
}
