using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020015CA RID: 5578
public class vp_Switch : vp_Interactable
{
	// Token: 0x0600ADFF RID: 44543 RVA: 0x0040E644 File Offset: 0x0040C844
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Start()
	{
		base.Start();
		if (this.AudioSource == null)
		{
			this.AudioSource = ((base.GetComponent<AudioSource>() == null) ? base.gameObject.AddComponent<AudioSource>() : base.GetComponent<AudioSource>());
		}
	}

	// Token: 0x0600AE00 RID: 44544 RVA: 0x0040E681 File Offset: 0x0040C881
	public override bool TryInteract(vp_FPPlayerEventHandler player)
	{
		if (this.Target == null)
		{
			return false;
		}
		if (this.m_Player == null)
		{
			this.m_Player = player;
		}
		this.PlaySound();
		this.Target.SendMessage(this.TargetMessage, SendMessageOptions.DontRequireReceiver);
		return true;
	}

	// Token: 0x0600AE01 RID: 44545 RVA: 0x0040E6C4 File Offset: 0x0040C8C4
	public virtual void PlaySound()
	{
		if (this.AudioSource == null)
		{
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

	// Token: 0x0600AE02 RID: 44546 RVA: 0x0040E748 File Offset: 0x0040C948
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
		this.TryInteract(this.m_Player);
	}

	// Token: 0x040081DD RID: 33245
	public GameObject Target;

	// Token: 0x040081DE RID: 33246
	public string TargetMessage = "";

	// Token: 0x040081DF RID: 33247
	public AudioSource AudioSource;

	// Token: 0x040081E0 RID: 33248
	public Vector2 SwitchPitchRange = new Vector2(1f, 1.5f);

	// Token: 0x040081E1 RID: 33249
	public List<AudioClip> SwitchSounds = new List<AudioClip>();
}
