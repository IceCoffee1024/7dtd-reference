using System;
using System.Collections.Generic;
using MusicUtils;
using UnityEngine.Audio;

namespace DynamicMusic.Legacy
{
	// Token: 0x02001A73 RID: 6771
	public class TransitionManager : IGamePrefsChangedListener
	{
		// Token: 0x170018E9 RID: 6377
		// (get) Token: 0x0600CC14 RID: 52244 RVA: 0x004A6005 File Offset: 0x004A4205
		public float MasterParam
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				if (!this.dynamicMusicManager.IsMusicPlayingThisTick)
				{
					return 0f;
				}
				return this.dynamicMusicManager.ThreatLevelTracker.NumericalThreatLevel;
			}
		}

		// Token: 0x0600CC15 RID: 52245 RVA: 0x004A602C File Offset: 0x004A422C
		public static void Init(DynamicMusicManager _dynamicMusicManager)
		{
			_dynamicMusicManager.TransitionManager = new TransitionManager();
			_dynamicMusicManager.TransitionManager.dynamicMusicManager = _dynamicMusicManager;
			TransitionManager.Master = DataLoader.LoadAsset<AudioMixer>("@:Sound_Mixers/MasterAudioMixer.mixer", false);
			TransitionManager.DmsAbsoluteVolumeCurve = new LogarithmicCurve(2.0, 6.0, -80f, 0f, 0f, 1f);
			TransitionManager.SetDynamicMusicVolume(GamePrefs.GetFloat(EnumGamePrefs.OptionsMusicVolumeLevel));
			GamePrefs.AddChangeListener(_dynamicMusicManager.TransitionManager);
		}

		// Token: 0x0600CC16 RID: 52246 RVA: 0x004A60A8 File Offset: 0x004A42A8
		public void Tick()
		{
			if (TransitionManager.Master == null)
			{
				return;
			}
			foreach (KeyValuePair<string, Curve> keyValuePair in SignalProcessing.DspCurves)
			{
				TransitionManager.Master.SetFloat(keyValuePair.Key, keyValuePair.Value.GetMixerValue(this.MasterParam));
			}
			if (this.dynamicMusicManager.IsInDeadWindow)
			{
				TransitionManager.Master.SetFloat("dmsVol", TransitionManager.currentEventDMSLogVolume = -80f);
				return;
			}
			if (this.dynamicMusicManager.DistanceFromDeadWindow > TransitionManager.dawnDuskFadeTime)
			{
				TransitionManager.Master.SetFloat("dmsVol", TransitionManager.currentEventDMSLogVolume = TransitionManager.currentAbsoluteDMSLogVolume);
				return;
			}
			TransitionManager.Master.SetFloat("dmsVol", TransitionManager.currentEventDMSLogVolume = TransitionManager.DmsEventRangeVolumeCurve.GetMixerValue(this.dynamicMusicManager.DistanceFromDeadWindow));
		}

		// Token: 0x0600CC17 RID: 52247 RVA: 0x004A61A8 File Offset: 0x004A43A8
		public static void SetDynamicMusicVolume(float _value)
		{
			if (TransitionManager.Master != null)
			{
				TransitionManager.currentAbsoluteDMSLogVolume = TransitionManager.DmsAbsoluteVolumeCurve.GetMixerValue(_value);
				TransitionManager.Master.SetFloat("dmsVol", TransitionManager.currentAbsoluteDMSLogVolume);
				TransitionManager.DmsEventRangeVolumeCurve = new LogarithmicCurve(2.0, 6.0, TransitionManager.currentAbsoluteDMSLogVolume, -80f, TransitionManager.dawnDuskFadeTime, 0f);
			}
		}

		// Token: 0x0600CC18 RID: 52248 RVA: 0x004A6217 File Offset: 0x004A4417
		public static void ApplyPauseFilter()
		{
			if (TransitionManager.Master != null)
			{
				TransitionManager.Master.SetFloat("dmsCutOff", 500f);
			}
		}

		// Token: 0x0600CC19 RID: 52249 RVA: 0x004A623B File Offset: 0x004A443B
		public static void RemovePauseFilter()
		{
			if (TransitionManager.Master != null)
			{
				TransitionManager.Master.SetFloat("dmsCutOff", 22000f);
			}
		}

		// Token: 0x0600CC1A RID: 52250 RVA: 0x004A625F File Offset: 0x004A445F
		public void OnGamePrefChanged(EnumGamePrefs _enum)
		{
			if (_enum == EnumGamePrefs.OptionsMusicVolumeLevel)
			{
				TransitionManager.SetDynamicMusicVolume(GamePrefs.GetFloat(EnumGamePrefs.OptionsMusicVolumeLevel));
			}
		}

		// Token: 0x04009A96 RID: 39574
		[PublicizedFrom(EAccessModifier.Private)]
		public DynamicMusicManager dynamicMusicManager;

		// Token: 0x04009A97 RID: 39575
		[PublicizedFrom(EAccessModifier.Private)]
		public static AudioMixer Master;

		// Token: 0x04009A98 RID: 39576
		[PublicizedFrom(EAccessModifier.Private)]
		public static LogarithmicCurve DmsAbsoluteVolumeCurve;

		// Token: 0x04009A99 RID: 39577
		[PublicizedFrom(EAccessModifier.Private)]
		public static LogarithmicCurve DmsEventRangeVolumeCurve;

		// Token: 0x04009A9A RID: 39578
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly float dawnDuskFadeTime = 0.16666667f;

		// Token: 0x04009A9B RID: 39579
		[PublicizedFrom(EAccessModifier.Private)]
		public static float currentAbsoluteDMSLogVolume;

		// Token: 0x04009A9C RID: 39580
		[PublicizedFrom(EAccessModifier.Private)]
		public static float currentEventDMSLogVolume;
	}
}
