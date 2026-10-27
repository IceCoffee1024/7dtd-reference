using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001616 RID: 5654
public class vp_Switch : vp_Interactable
{
	// Token: 0x0600AFE6 RID: 45030 RVA: 0x00419A64 File Offset: 0x00417C64
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Start()
	{
		base.Start();
		if (this.AudioSource == null)
		{
			this.AudioSource = ((base.GetComponent<AudioSource>() == null) ? base.gameObject.AddComponent<AudioSource>() : base.GetComponent<AudioSource>());
		}
	}

	// Token: 0x0600AFE7 RID: 45031 RVA: 0x00419AA1 File Offset: 0x00417CA1
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

	// Token: 0x0600AFE8 RID: 45032 RVA: 0x00419AE4 File Offset: 0x00417CE4
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

	// Token: 0x0600AFE9 RID: 45033 RVA: 0x00419B68 File Offset: 0x00417D68
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

	// Token: 0x0400834D RID: 33613
	public GameObject Target;

	// Token: 0x0400834E RID: 33614
	public string TargetMessage = "";

	// Token: 0x0400834F RID: 33615
	public AudioSource AudioSource;

	// Token: 0x04008350 RID: 33616
	public Vector2 SwitchPitchRange = new Vector2(1f, 1.5f);

	// Token: 0x04008351 RID: 33617
	public List<AudioClip> SwitchSounds = new List<AudioClip>();
}
