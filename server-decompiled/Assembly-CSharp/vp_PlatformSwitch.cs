using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001603 RID: 5635
public class vp_PlatformSwitch : vp_Interactable
{
	// Token: 0x0600AF89 RID: 44937 RVA: 0x00417405 File Offset: 0x00415605
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Start()
	{
		base.Start();
		if (this.AudioSource == null)
		{
			this.AudioSource = ((base.GetComponent<AudioSource>() == null) ? base.gameObject.AddComponent<AudioSource>() : base.GetComponent<AudioSource>());
		}
	}

	// Token: 0x0600AF8A RID: 44938 RVA: 0x00417444 File Offset: 0x00415644
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

	// Token: 0x0600AF8B RID: 44939 RVA: 0x004174D4 File Offset: 0x004156D4
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

	// Token: 0x0600AF8C RID: 44940 RVA: 0x00417564 File Offset: 0x00415764
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

	// Token: 0x04008305 RID: 33541
	public float SwitchTimeout;

	// Token: 0x04008306 RID: 33542
	public vp_MovingPlatform Platform;

	// Token: 0x04008307 RID: 33543
	public AudioSource AudioSource;

	// Token: 0x04008308 RID: 33544
	public Vector2 SwitchPitchRange = new Vector2(1f, 1.5f);

	// Token: 0x04008309 RID: 33545
	public List<AudioClip> SwitchSounds = new List<AudioClip>();

	// Token: 0x0400830A RID: 33546
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_IsSwitched;

	// Token: 0x0400830B RID: 33547
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_Timeout;
}
