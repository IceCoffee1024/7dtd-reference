using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001619 RID: 5657
[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(AudioSource))]
public abstract class vp_Pickup : MonoBehaviour
{
	// Token: 0x0600AFF0 RID: 45040 RVA: 0x00419D84 File Offset: 0x00417F84
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

	// Token: 0x0600AFF1 RID: 45041 RVA: 0x00419F3C File Offset: 0x0041813C
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

	// Token: 0x0600AFF2 RID: 45042 RVA: 0x00419FD0 File Offset: 0x004181D0
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

	// Token: 0x0600AFF3 RID: 45043 RVA: 0x0041A0E4 File Offset: 0x004182E4
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

	// Token: 0x0600AFF4 RID: 45044 RVA: 0x0041A240 File Offset: 0x00418440
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnTriggerExit(Collider col)
	{
		this.m_AlreadyFailed = false;
	}

	// Token: 0x0600AFF5 RID: 45045 RVA: 0x0041A249 File Offset: 0x00418449
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool TryGive(vp_FPPlayerEventHandler player)
	{
		return player.AddItem.Try(new object[] { this.InventoryName, 1 });
	}

	// Token: 0x0600AFF6 RID: 45046 RVA: 0x0041A278 File Offset: 0x00418478
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

	// Token: 0x0600AFF7 RID: 45047 RVA: 0x0041A2E0 File Offset: 0x004184E0
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

	// Token: 0x0600AFF8 RID: 45048 RVA: 0x0041A40C File Offset: 0x0041860C
	[PublicizedFrom(EAccessModifier.Protected)]
	public vp_Pickup()
	{
	}

	// Token: 0x04008354 RID: 33620
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform m_Transform;

	// Token: 0x04008355 RID: 33621
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Rigidbody m_Rigidbody;

	// Token: 0x04008356 RID: 33622
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public AudioSource m_Audio;

	// Token: 0x04008357 RID: 33623
	public string InventoryName = "Unnamed";

	// Token: 0x04008358 RID: 33624
	public List<string> RecipientTags = new List<string>();

	// Token: 0x04008359 RID: 33625
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Collider m_LastCollider;

	// Token: 0x0400835A RID: 33626
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public vp_FPPlayerEventHandler m_Recipient;

	// Token: 0x0400835B RID: 33627
	public string GiveMessage = "Picked up an item";

	// Token: 0x0400835C RID: 33628
	public string FailMessage = "You currently can't pick up this item!";

	// Token: 0x0400835D RID: 33629
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_SpawnPosition = Vector3.zero;

	// Token: 0x0400835E RID: 33630
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_SpawnScale = Vector3.zero;

	// Token: 0x0400835F RID: 33631
	public bool Billboard;

	// Token: 0x04008360 RID: 33632
	public Vector3 Spin = Vector3.zero;

	// Token: 0x04008361 RID: 33633
	public float BobAmp;

	// Token: 0x04008362 RID: 33634
	public float BobRate;

	// Token: 0x04008363 RID: 33635
	public float BobOffset = -1f;

	// Token: 0x04008364 RID: 33636
	public Vector3 RigidbodyForce = Vector3.zero;

	// Token: 0x04008365 RID: 33637
	public float RigidbodySpin;

	// Token: 0x04008366 RID: 33638
	public float RespawnDuration = 10f;

	// Token: 0x04008367 RID: 33639
	public float RespawnScaleUpDuration;

	// Token: 0x04008368 RID: 33640
	public float RemoveDuration;

	// Token: 0x04008369 RID: 33641
	public AudioClip PickupSound;

	// Token: 0x0400836A RID: 33642
	public AudioClip PickupFailSound;

	// Token: 0x0400836B RID: 33643
	public AudioClip RespawnSound;

	// Token: 0x0400836C RID: 33644
	public bool PickupSoundSlomo = true;

	// Token: 0x0400836D RID: 33645
	public bool FailSoundSlomo = true;

	// Token: 0x0400836E RID: 33646
	public bool RespawnSoundSlomo = true;

	// Token: 0x0400836F RID: 33647
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_Depleted;

	// Token: 0x04008370 RID: 33648
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_AlreadyFailed;

	// Token: 0x04008371 RID: 33649
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Timer.Handle m_RespawnTimer = new vp_Timer.Handle();

	// Token: 0x04008372 RID: 33650
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform m_CameraMainTransform;
}
