using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000561 RID: 1377
public class BackgroundMusicMono : SingletonMonoBehaviour<BackgroundMusicMono>
{
	// Token: 0x06002CBC RID: 11452 RVA: 0x0011DB94 File Offset: 0x0011BD94
	[PublicizedFrom(EAccessModifier.Protected)]
	public void Start()
	{
		AudioListener[] array = global::UnityEngine.Object.FindObjectsByType<AudioListener>(FindObjectsSortMode.None);
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].enabled)
			{
				base.transform.position = array[i].transform.position;
				break;
			}
		}
		AudioListener.volume = Mathf.Min(GamePrefs.GetFloat(EnumGamePrefs.OptionsOverallAudioVolumeLevel), 1f);
		this.AddMusicTrack(BackgroundMusicMono.MusicTrack.None, null);
		this.AddMusicTrack(BackgroundMusicMono.MusicTrack.BackgroundMusic, GameManager.Instance.BackgroundMusicClip);
		this.AddMusicTrack(BackgroundMusicMono.MusicTrack.CreditsSong, GameManager.Instance.CreditsSongClip);
		this.Play(BackgroundMusicMono.MusicTrack.BackgroundMusic);
	}

	// Token: 0x06002CBD RID: 11453 RVA: 0x0011DC20 File Offset: 0x0011BE20
	[PublicizedFrom(EAccessModifier.Protected)]
	public void Update()
	{
		if (GameStats.GetInt(EnumGameStats.GameState) == 1 && !GameManager.Instance.IsPaused())
		{
			this.Play(BackgroundMusicMono.MusicTrack.None);
		}
		else if (LocalPlayerUI.primaryUI.windowManager.IsWindowOpen(XUiC_Credits.ID))
		{
			this.Play(BackgroundMusicMono.MusicTrack.CreditsSong);
		}
		else
		{
			this.Play(BackgroundMusicMono.MusicTrack.BackgroundMusic);
		}
		this.activeTracks.RemoveWhere((BackgroundMusicMono.MusicTrack activeTrack) => !this.UpdateTrack(activeTrack));
	}

	// Token: 0x06002CBE RID: 11454 RVA: 0x0011DC8C File Offset: 0x0011BE8C
	[PublicizedFrom(EAccessModifier.Private)]
	public void AddMusicTrack(BackgroundMusicMono.MusicTrack musicTrack, AudioClip audioClip)
	{
		if (!audioClip)
		{
			this.musicTrackStates.Add(musicTrack, new BackgroundMusicMono.MusicTrackState(null));
			return;
		}
		AudioSource audioSource = base.gameObject.AddComponent<AudioSource>();
		audioSource.volume = 0f;
		audioSource.clip = audioClip;
		audioSource.loop = true;
		this.musicTrackStates.Add(musicTrack, new BackgroundMusicMono.MusicTrackState(audioSource));
	}

	// Token: 0x06002CBF RID: 11455 RVA: 0x0011DCEB File Offset: 0x0011BEEB
	[PublicizedFrom(EAccessModifier.Private)]
	public void Play(BackgroundMusicMono.MusicTrack musicTrack)
	{
		if (this.currentlyPlaying == musicTrack)
		{
			return;
		}
		this.currentlyPlaying = musicTrack;
		this.activeTracks.Add(musicTrack);
	}

	// Token: 0x06002CC0 RID: 11456 RVA: 0x0011DD0C File Offset: 0x0011BF0C
	[PublicizedFrom(EAccessModifier.Private)]
	public bool UpdateTrack(BackgroundMusicMono.MusicTrack activeTrack)
	{
		BackgroundMusicMono.MusicTrackState musicTrackState = this.musicTrackStates[activeTrack];
		AudioSource audioSource = musicTrackState.AudioSource;
		if (!audioSource)
		{
			return false;
		}
		float num = musicTrackState.CurrentVolume;
		if (activeTrack == this.currentlyPlaying)
		{
			num += Time.deltaTime / 3f;
		}
		else
		{
			num -= Time.deltaTime / 3f;
		}
		num = Mathf.Clamp01(num);
		musicTrackState.CurrentVolume = num;
		bool flag = activeTrack == this.currentlyPlaying || num > 0f;
		audioSource.volume = Mathf.Clamp01(GamePrefs.GetFloat(EnumGamePrefs.OptionsMenuMusicVolumeLevel) * num);
		if (audioSource.isPlaying == flag)
		{
			return flag;
		}
		if (flag)
		{
			audioSource.Play();
		}
		else
		{
			audioSource.Stop();
		}
		return flag;
	}

	// Token: 0x04002309 RID: 8969
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float secondsToFadeOut = 3f;

	// Token: 0x0400230A RID: 8970
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float secondsToFadeIn = 3f;

	// Token: 0x0400230B RID: 8971
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly EnumDictionary<BackgroundMusicMono.MusicTrack, BackgroundMusicMono.MusicTrackState> musicTrackStates = new EnumDictionary<BackgroundMusicMono.MusicTrack, BackgroundMusicMono.MusicTrackState>();

	// Token: 0x0400230C RID: 8972
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly HashSet<BackgroundMusicMono.MusicTrack> activeTracks = new HashSet<BackgroundMusicMono.MusicTrack>();

	// Token: 0x0400230D RID: 8973
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public BackgroundMusicMono.MusicTrack currentlyPlaying;

	// Token: 0x02000562 RID: 1378
	[PublicizedFrom(EAccessModifier.Private)]
	public enum MusicTrack
	{
		// Token: 0x0400230F RID: 8975
		None,
		// Token: 0x04002310 RID: 8976
		BackgroundMusic,
		// Token: 0x04002311 RID: 8977
		CreditsSong
	}

	// Token: 0x02000563 RID: 1379
	[PublicizedFrom(EAccessModifier.Private)]
	public class MusicTrackState
	{
		// Token: 0x06002CC3 RID: 11459 RVA: 0x0011DDE2 File Offset: 0x0011BFE2
		public MusicTrackState(AudioSource audioSource)
		{
			this.AudioSource = audioSource;
		}

		// Token: 0x04002312 RID: 8978
		public readonly AudioSource AudioSource;

		// Token: 0x04002313 RID: 8979
		public float CurrentVolume;
	}
}
