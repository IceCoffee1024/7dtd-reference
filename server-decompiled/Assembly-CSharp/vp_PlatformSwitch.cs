using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001615 RID: 5653
public class vp_PlatformSwitch : vp_Interactable
{
	// Token: 0x0600AFE1 RID: 45025 RVA: 0x00419809 File Offset: 0x00417A09
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Start()
	{
		base.Start();
		if (this.AudioSource == null)
		{
			this.AudioSource = ((base.GetComponent<AudioSource>() == null) ? base.gameObject.AddComponent<AudioSource>() : base.GetComponent<AudioSource>());
		}
	}

	// Token: 0x0600AFE2 RID: 45026 RVA: 0x00419848 File Offset: 0x00417A48
	public override bool TryInteract(vp_FPPlayerEventHandler player)
	{
		if (this.Platform == null)
		{
			return false;
		}
		if (this.m_Player == null)
		{
			this.m_Player = player;
		}
		if (Time.time < this.m_Timeout)
		{
			return false;
		}
		this.PlaySound();
		this.Platform.SendMessage("GoTo", (this.Platform.TargetedWaypoint == 0) ? 1 : 0, SendMessageOptions.DontRequireReceiver);
		this.m_Timeout = Time.time + this.SwitchTimeout;
		this.m_IsSwitched = !this.m_IsSwitched;
		return true;
	}

	// Token: 0x0600AFE3 RID: 45027 RVA: 0x004198D8 File Offset: 0x00417AD8
	public virtual void PlaySound()
	{
		if (this.AudioSource == null)
		{
			Debug.LogWarning("Audio Source is not set");
			return;
		}
		if (this.SwitchSounds.Count == 0)
		{
			return;
		}
		AudioClip audioClip = this.SwitchSounds[global::UnityEngine.Random.Range(0, this.SwitchSounds.Count)];
		if (audioClip == null)
		{
			return;
		}
		this.AudioSource.pitch = global::UnityEngine.Random.Range(this.SwitchPitchRange.x, this.SwitchPitchRange.y);
		this.AudioSource.PlayOneShot(audioClip);
	}

	// Token: 0x0600AFE4 RID: 45028 RVA: 0x00419968 File Offset: 0x00417B68
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnTriggerEnter(Collider col)
	{
		if (this.InteractType != vp_Interactable.vp_InteractType.Trigger)
		{
			return;
		}
		foreach (string text in this.RecipientTags)
		{
			if (col.gameObject.tag == text)
			{
				goto IL_004F;
			}
		}
		return;
		IL_004F:
		if (this.m_Player == null)
		{
			this.m_Player = global::UnityEngine.Object.FindAnyObjectByType<vp_FPPlayerEventHandler>();
		}
		if (this.m_Player.Interactable.Get() != null && this.m_Player.Interactable.Get().GetComponent<Collider>() == col)
		{
			return;
		}
		this.TryInteract(this.m_Player);
	}

	// Token: 0x04008346 RID: 33606
	public float SwitchTimeout;

	// Token: 0x04008347 RID: 33607
	public vp_MovingPlatform Platform;

	// Token: 0x04008348 RID: 33608
	public AudioSource AudioSource;

	// Token: 0x04008349 RID: 33609
	public Vector2 SwitchPitchRange = new Vector2(1f, 1.5f);

	// Token: 0x0400834A RID: 33610
	public List<AudioClip> SwitchSounds = new List<AudioClip>();

	// Token: 0x0400834B RID: 33611
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_IsSwitched;

	// Token: 0x0400834C RID: 33612
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_Timeout;
}
