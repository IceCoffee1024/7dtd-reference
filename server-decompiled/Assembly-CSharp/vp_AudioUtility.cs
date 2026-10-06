using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020015A2 RID: 5538
public static class vp_AudioUtility
{
	// Token: 0x0600ACEF RID: 44271 RVA: 0x004070EC File Offset: 0x004052EC
	public static void PlayRandomSound(AudioSource audioSource, List<AudioClip> sounds, Vector2 pitchRange)
	{
		if (audioSource == null)
		{
			return;
		}
		if (sounds == null || sounds.Count == 0)
		{
			return;
		}
		AudioClip audioClip = sounds[global::UnityEngine.Random.Range(0, sounds.Count)];
		if (audioClip == null)
		{
			return;
		}
		if (pitchRange == Vector2.one)
		{
			audioSource.pitch = Time.timeScale;
		}
		else
		{
			audioSource.pitch = global::UnityEngine.Random.Range(pitchRange.x, pitchRange.y) * Time.timeScale;
		}
		audioSource.PlayOneShot(audioClip);
	}

	// Token: 0x0600ACF0 RID: 44272 RVA: 0x0040716A File Offset: 0x0040536A
	public static void PlayRandomSound(AudioSource audioSource, List<AudioClip> sounds)
	{
		vp_AudioUtility.PlayRandomSound(audioSource, sounds, Vector2.one);
	}
}
