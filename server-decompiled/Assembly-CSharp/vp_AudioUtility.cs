using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001580 RID: 5504
public static class vp_AudioUtility
{
	// Token: 0x0600AC3C RID: 44092 RVA: 0x00402298 File Offset: 0x00400498
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

	// Token: 0x0600AC3D RID: 44093 RVA: 0x00402316 File Offset: 0x00400516
	public static void PlayRandomSound(AudioSource audioSource, List<AudioClip> sounds)
	{
		vp_AudioUtility.PlayRandomSound(audioSource, sounds, Vector2.one);
	}
}
