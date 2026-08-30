using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001565 RID: 5477
public static class vp_AudioUtility
{
	// Token: 0x0600AB3F RID: 43839 RVA: 0x003FCF58 File Offset: 0x003FB158
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

	// Token: 0x0600AB40 RID: 43840 RVA: 0x003FCFD6 File Offset: 0x003FB1D6
	public static void PlayRandomSound(AudioSource audioSource, List<AudioClip> sounds)
	{
		vp_AudioUtility.PlayRandomSound(audioSource, sounds, Vector2.one);
	}
}
