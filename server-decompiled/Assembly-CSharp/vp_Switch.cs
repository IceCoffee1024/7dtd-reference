using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001604 RID: 5636
public class vp_Switch : vp_Interactable
{
	// Token: 0x0600AF8E RID: 44942 RVA: 0x00417660 File Offset: 0x00415860
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Start()
	{
		base.Start();
		if (this.AudioSource == null)
		{
			this.AudioSource = ((base.GetComponent<AudioSource>() == null) ? base.gameObject.AddComponent<AudioSource>() : base.GetComponent<AudioSource>());
		}
	}

	// Token: 0x0600AF8F RID: 44943 RVA: 0x0041769D File Offset: 0x0041589D
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

	// Token: 0x0600AF90 RID: 44944 RVA: 0x004176E0 File Offset: 0x004158E0
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

	// Token: 0x0600AF91 RID: 44945 RVA: 0x00417764 File Offset: 0x00415964
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

	// Token: 0x0400830C RID: 33548
	public GameObject Target;

	// Token: 0x0400830D RID: 33549
	public string TargetMessage = "";

	// Token: 0x0400830E RID: 33550
	public AudioSource AudioSource;

	// Token: 0x0400830F RID: 33551
	public Vector2 SwitchPitchRange = new Vector2(1f, 1.5f);

	// Token: 0x04008310 RID: 33552
	public List<AudioClip> SwitchSounds = new List<AudioClip>();
}
