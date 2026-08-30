using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020015E5 RID: 5605
public class vp_Switch : vp_Interactable
{
	// Token: 0x0600AEFC RID: 44796 RVA: 0x00413984 File Offset: 0x00411B84
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Start()
	{
		base.Start();
		if (this.AudioSource == null)
		{
			this.AudioSource = ((base.GetComponent<AudioSource>() == null) ? base.gameObject.AddComponent<AudioSource>() : base.GetComponent<AudioSource>());
		}
	}

	// Token: 0x0600AEFD RID: 44797 RVA: 0x004139C1 File Offset: 0x00411BC1
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

	// Token: 0x0600AEFE RID: 44798 RVA: 0x00413A04 File Offset: 0x00411C04
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

	// Token: 0x0600AEFF RID: 44799 RVA: 0x00413A88 File Offset: 0x00411C88
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

	// Token: 0x040082B6 RID: 33462
	public GameObject Target;

	// Token: 0x040082B7 RID: 33463
	public string TargetMessage = "";

	// Token: 0x040082B8 RID: 33464
	public AudioSource AudioSource;

	// Token: 0x040082B9 RID: 33465
	public Vector2 SwitchPitchRange = new Vector2(1f, 1.5f);

	// Token: 0x040082BA RID: 33466
	public List<AudioClip> SwitchSounds = new List<AudioClip>();
}
