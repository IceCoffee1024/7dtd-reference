using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001607 RID: 5639
[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(AudioSource))]
public abstract class vp_Pickup : MonoBehaviour
{
	// Token: 0x0600AF98 RID: 44952 RVA: 0x00417980 File Offset: 0x00415B80
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

	// Token: 0x0600AF99 RID: 44953 RVA: 0x00417B38 File Offset: 0x00415D38
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

	// Token: 0x0600AF9A RID: 44954 RVA: 0x00417BCC File Offset: 0x00415DCC
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

	// Token: 0x0600AF9B RID: 44955 RVA: 0x00417CE0 File Offset: 0x00415EE0
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

	// Token: 0x0600AF9C RID: 44956 RVA: 0x00417E3C File Offset: 0x0041603C
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnTriggerExit(Collider col)
	{
		this.m_AlreadyFailed = false;
	}

	// Token: 0x0600AF9D RID: 44957 RVA: 0x00417E45 File Offset: 0x00416045
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool TryGive(vp_FPPlayerEventHandler player)
	{
		return player.AddItem.Try(new object[] { this.InventoryName, 1 });
	}

	// Token: 0x0600AF9E RID: 44958 RVA: 0x00417E74 File Offset: 0x00416074
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

	// Token: 0x0600AF9F RID: 44959 RVA: 0x00417EDC File Offset: 0x004160DC
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

	// Token: 0x0600AFA0 RID: 44960 RVA: 0x00418008 File Offset: 0x00416208
	[PublicizedFrom(EAccessModifier.Protected)]
	public vp_Pickup()
	{
	}

	// Token: 0x04008313 RID: 33555
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform m_Transform;

	// Token: 0x04008314 RID: 33556
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Rigidbody m_Rigidbody;

	// Token: 0x04008315 RID: 33557
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public AudioSource m_Audio;

	// Token: 0x04008316 RID: 33558
	public string InventoryName = "Unnamed";

	// Token: 0x04008317 RID: 33559
	public List<string> RecipientTags = new List<string>();

	// Token: 0x04008318 RID: 33560
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Collider m_LastCollider;

	// Token: 0x04008319 RID: 33561
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public vp_FPPlayerEventHandler m_Recipient;

	// Token: 0x0400831A RID: 33562
	public string GiveMessage = "Picked up an item";

	// Token: 0x0400831B RID: 33563
	public string FailMessage = "You currently can't pick up this item!";

	// Token: 0x0400831C RID: 33564
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_SpawnPosition = Vector3.zero;

	// Token: 0x0400831D RID: 33565
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_SpawnScale = Vector3.zero;

	// Token: 0x0400831E RID: 33566
	public bool Billboard;

	// Token: 0x0400831F RID: 33567
	public Vector3 Spin = Vector3.zero;

	// Token: 0x04008320 RID: 33568
	public float BobAmp;

	// Token: 0x04008321 RID: 33569
	public float BobRate;

	// Token: 0x04008322 RID: 33570
	public float BobOffset = -1f;

	// Token: 0x04008323 RID: 33571
	public Vector3 RigidbodyForce = Vector3.zero;

	// Token: 0x04008324 RID: 33572
	public float RigidbodySpin;

	// Token: 0x04008325 RID: 33573
	public float RespawnDuration = 10f;

	// Token: 0x04008326 RID: 33574
	public float RespawnScaleUpDuration;

	// Token: 0x04008327 RID: 33575
	public float RemoveDuration;

	// Token: 0x04008328 RID: 33576
	public AudioClip PickupSound;

	// Token: 0x04008329 RID: 33577
	public AudioClip PickupFailSound;

	// Token: 0x0400832A RID: 33578
	public AudioClip RespawnSound;

	// Token: 0x0400832B RID: 33579
	public bool PickupSoundSlomo = true;

	// Token: 0x0400832C RID: 33580
	public bool FailSoundSlomo = true;

	// Token: 0x0400832D RID: 33581
	public bool RespawnSoundSlomo = true;

	// Token: 0x0400832E RID: 33582
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_Depleted;

	// Token: 0x0400832F RID: 33583
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_AlreadyFailed;

	// Token: 0x04008330 RID: 33584
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Timer.Handle m_RespawnTimer = new vp_Timer.Handle();

	// Token: 0x04008331 RID: 33585
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform m_CameraMainTransform;
}
