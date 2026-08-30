using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020015E4 RID: 5604
public class vp_PlatformSwitch : vp_Interactable
{
	// Token: 0x0600AEF7 RID: 44791 RVA: 0x00413729 File Offset: 0x00411929
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Start()
	{
		base.Start();
		if (this.AudioSource == null)
		{
			this.AudioSource = ((base.GetComponent<AudioSource>() == null) ? base.gameObject.AddComponent<AudioSource>() : base.GetComponent<AudioSource>());
		}
	}

	// Token: 0x0600AEF8 RID: 44792 RVA: 0x00413768 File Offset: 0x00411968
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

	// Token: 0x0600AEF9 RID: 44793 RVA: 0x004137F8 File Offset: 0x004119F8
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

	// Token: 0x0600AEFA RID: 44794 RVA: 0x00413888 File Offset: 0x00411A88
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

	// Token: 0x040082AF RID: 33455
	public float SwitchTimeout;

	// Token: 0x040082B0 RID: 33456
	public vp_MovingPlatform Platform;

	// Token: 0x040082B1 RID: 33457
	public AudioSource AudioSource;

	// Token: 0x040082B2 RID: 33458
	public Vector2 SwitchPitchRange = new Vector2(1f, 1.5f);

	// Token: 0x040082B3 RID: 33459
	public List<AudioClip> SwitchSounds = new List<AudioClip>();

	// Token: 0x040082B4 RID: 33460
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_IsSwitched;

	// Token: 0x040082B5 RID: 33461
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_Timeout;
}
