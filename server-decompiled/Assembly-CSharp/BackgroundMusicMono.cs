using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200056C RID: 1388
public class BackgroundMusicMono : SingletonMonoBehaviour<BackgroundMusicMono>
{
	// Token: 0x06002CDC RID: 11484 RVA: 0x0011FA8C File Offset: 0x0011DC8C
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

	// Token: 0x06002CDD RID: 11485 RVA: 0x0011FB18 File Offset: 0x0011DD18
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

	// Token: 0x06002CDE RID: 11486 RVA: 0x0011FB84 File Offset: 0x0011DD84
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

	// Token: 0x06002CDF RID: 11487 RVA: 0x0011FBE3 File Offset: 0x0011DDE3
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

	// Token: 0x06002CE0 RID: 11488 RVA: 0x0011FC04 File Offset: 0x0011DE04
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

	// Token: 0x04002345 RID: 9029
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float secondsToFadeOut = 3f;

	// Token: 0x04002346 RID: 9030
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float secondsToFadeIn = 3f;

	// Token: 0x04002347 RID: 9031
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly EnumDictionary<BackgroundMusicMono.MusicTrack, BackgroundMusicMono.MusicTrackState> musicTrackStates = new EnumDictionary<BackgroundMusicMono.MusicTrack, BackgroundMusicMono.MusicTrackState>();

	// Token: 0x04002348 RID: 9032
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly HashSet<BackgroundMusicMono.MusicTrack> activeTracks = new HashSet<BackgroundMusicMono.MusicTrack>();

	// Token: 0x04002349 RID: 9033
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public BackgroundMusicMono.MusicTrack currentlyPlaying;

	// Token: 0x0200056D RID: 1389
	[PublicizedFrom(EAccessModifier.Private)]
	public enum MusicTrack
	{
		// Token: 0x0400234B RID: 9035
		None,
		// Token: 0x0400234C RID: 9036
		BackgroundMusic,
		// Token: 0x0400234D RID: 9037
		CreditsSong
	}

	// Token: 0x0200056E RID: 1390
	[PublicizedFrom(EAccessModifier.Private)]
	public class MusicTrackState
	{
		// Token: 0x06002CE3 RID: 11491 RVA: 0x0011FCDA File Offset: 0x0011DEDA
		public MusicTrackState(AudioSource audioSource)
		{
			this.AudioSource = audioSource;
		}

		// Token: 0x0400234E RID: 9038
		public readonly AudioSource AudioSource;

		// Token: 0x0400234F RID: 9039
		public float CurrentVolume;
	}
}
