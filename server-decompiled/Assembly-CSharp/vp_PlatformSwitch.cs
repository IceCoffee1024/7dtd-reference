using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020015C9 RID: 5577
public class vp_PlatformSwitch : vp_Interactable
{
	// Token: 0x0600ADFA RID: 44538 RVA: 0x0040E3E9 File Offset: 0x0040C5E9
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Start()
	{
		base.Start();
		if (this.AudioSource == null)
		{
			this.AudioSource = ((base.GetComponent<AudioSource>() == null) ? base.gameObject.AddComponent<AudioSource>() : base.GetComponent<AudioSource>());
		}
	}

	// Token: 0x0600ADFB RID: 44539 RVA: 0x0040E428 File Offset: 0x0040C628
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

	// Token: 0x0600ADFC RID: 44540 RVA: 0x0040E4B8 File Offset: 0x0040C6B8
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

	// Token: 0x0600ADFD RID: 44541 RVA: 0x0040E548 File Offset: 0x0040C748
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

	// Token: 0x040081D6 RID: 33238
	public float SwitchTimeout;

	// Token: 0x040081D7 RID: 33239
	public vp_MovingPlatform Platform;

	// Token: 0x040081D8 RID: 33240
	public AudioSource AudioSource;

	// Token: 0x040081D9 RID: 33241
	public Vector2 SwitchPitchRange = new Vector2(1f, 1.5f);

	// Token: 0x040081DA RID: 33242
	public List<AudioClip> SwitchSounds = new List<AudioClip>();

	// Token: 0x040081DB RID: 33243
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_IsSwitched;

	// Token: 0x040081DC RID: 33244
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_Timeout;
}
