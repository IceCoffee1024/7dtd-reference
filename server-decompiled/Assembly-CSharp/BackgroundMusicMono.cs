using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000573 RID: 1395
public class BackgroundMusicMono : SingletonMonoBehaviour<BackgroundMusicMono>
{
	// Token: 0x06002CE7 RID: 11495 RVA: 0x0012004C File Offset: 0x0011E24C
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

	// Token: 0x06002CE8 RID: 11496 RVA: 0x001200D8 File Offset: 0x0011E2D8
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

	// Token: 0x06002CE9 RID: 11497 RVA: 0x00120144 File Offset: 0x0011E344
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

	// Token: 0x06002CEA RID: 11498 RVA: 0x001201A3 File Offset: 0x0011E3A3
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

	// Token: 0x06002CEB RID: 11499 RVA: 0x001201C4 File Offset: 0x0011E3C4
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

	// Token: 0x04002360 RID: 9056
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float secondsToFadeOut = 3f;

	// Token: 0x04002361 RID: 9057
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float secondsToFadeIn = 3f;

	// Token: 0x04002362 RID: 9058
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly EnumDictionary<BackgroundMusicMono.MusicTrack, BackgroundMusicMono.MusicTrackState> musicTrackStates = new EnumDictionary<BackgroundMusicMono.MusicTrack, BackgroundMusicMono.MusicTrackState>();

	// Token: 0x04002363 RID: 9059
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly HashSet<BackgroundMusicMono.MusicTrack> activeTracks = new HashSet<BackgroundMusicMono.MusicTrack>();

	// Token: 0x04002364 RID: 9060
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public BackgroundMusicMono.MusicTrack currentlyPlaying;

	// Token: 0x02000574 RID: 1396
	[PublicizedFrom(EAccessModifier.Private)]
	public enum MusicTrack
	{
		// Token: 0x04002366 RID: 9062
		None,
		// Token: 0x04002367 RID: 9063
		BackgroundMusic,
		// Token: 0x04002368 RID: 9064
		CreditsSong
	}

	// Token: 0x02000575 RID: 1397
	[PublicizedFrom(EAccessModifier.Private)]
	public class MusicTrackState
	{
		// Token: 0x06002CEE RID: 11502 RVA: 0x0012029A File Offset: 0x0011E49A
		public MusicTrackState(AudioSource audioSource)
		{
			this.AudioSource = audioSource;
		}

		// Token: 0x04002369 RID: 9065
		public readonly AudioSource AudioSource;

		// Token: 0x0400236A RID: 9066
		public float CurrentVolume;
	}
}
