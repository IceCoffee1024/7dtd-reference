using System;
using System.Collections.Generic;
using MusicUtils;
using UnityEngine.Audio;

namespace DynamicMusic.Legacy
{
	// Token: 0x02001AC6 RID: 6854
	public class TransitionManager : IGamePrefsChangedListener
	{
		// Token: 0x170019FD RID: 6653
		// (get) Token: 0x0600CEF5 RID: 52981 RVA: 0x004B08C5 File Offset: 0x004AEAC5
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

		// Token: 0x0600CEF6 RID: 52982 RVA: 0x004B08EC File Offset: 0x004AEAEC
		public static void Init(DynamicMusicManager _dynamicMusicManager)
		{
			_dynamicMusicManager.TransitionManager = new TransitionManager();
			_dynamicMusicManager.TransitionManager.dynamicMusicManager = _dynamicMusicManager;
			TransitionManager.Master = DataLoader.LoadAsset<AudioMixer>("@:Sound_Mixers/MasterAudioMixer.mixer", false);
			TransitionManager.DmsAbsoluteVolumeCurve = new LogarithmicCurve(2.0, 6.0, -80f, 0f, 0f, 1f);
			TransitionManager.SetDynamicMusicVolume(GamePrefs.GetFloat(EnumGamePrefs.OptionsMusicVolumeLevel));
			GamePrefs.AddChangeListener(_dynamicMusicManager.TransitionManager);
		}

		// Token: 0x0600CEF7 RID: 52983 RVA: 0x004B0968 File Offset: 0x004AEB68
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

		// Token: 0x0600CEF8 RID: 52984 RVA: 0x004B0A68 File Offset: 0x004AEC68
		public static void SetDynamicMusicVolume(float _value)
		{
			if (TransitionManager.Master != null)
			{
				TransitionManager.currentAbsoluteDMSLogVolume = TransitionManager.DmsAbsoluteVolumeCurve.GetMixerValue(_value);
				TransitionManager.Master.SetFloat("dmsVol", TransitionManager.currentAbsoluteDMSLogVolume);
				TransitionManager.DmsEventRangeVolumeCurve = new LogarithmicCurve(2.0, 6.0, TransitionManager.currentAbsoluteDMSLogVolume, -80f, TransitionManager.dawnDuskFadeTime, 0f);
			}
		}

		// Token: 0x0600CEF9 RID: 52985 RVA: 0x004B0AD7 File Offset: 0x004AECD7
		public static void ApplyPauseFilter()
		{
			if (TransitionManager.Master != null)
			{
				TransitionManager.Master.SetFloat("dmsCutOff", 500f);
			}
		}

		// Token: 0x0600CEFA RID: 52986 RVA: 0x004B0AFB File Offset: 0x004AECFB
		public static void RemovePauseFilter()
		{
			if (TransitionManager.Master != null)
			{
				TransitionManager.Master.SetFloat("dmsCutOff", 22000f);
			}
		}

		// Token: 0x0600CEFB RID: 52987 RVA: 0x004B0B1F File Offset: 0x004AED1F
		public void OnGamePrefChanged(EnumGamePrefs _enum)
		{
			if (_enum == EnumGamePrefs.OptionsMusicVolumeLevel)
			{
				TransitionManager.SetDynamicMusicVolume(GamePrefs.GetFloat(EnumGamePrefs.OptionsMusicVolumeLevel));
			}
		}

		// Token: 0x04009C8D RID: 40077
		[PublicizedFrom(EAccessModifier.Private)]
		public DynamicMusicManager dynamicMusicManager;

		// Token: 0x04009C8E RID: 40078
		[PublicizedFrom(EAccessModifier.Private)]
		public static AudioMixer Master;

		// Token: 0x04009C8F RID: 40079
		[PublicizedFrom(EAccessModifier.Private)]
		public static LogarithmicCurve DmsAbsoluteVolumeCurve;

		// Token: 0x04009C90 RID: 40080
		[PublicizedFrom(EAccessModifier.Private)]
		public static LogarithmicCurve DmsEventRangeVolumeCurve;

		// Token: 0x04009C91 RID: 40081
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly float dawnDuskFadeTime = 0.16666667f;

		// Token: 0x04009C92 RID: 40082
		[PublicizedFrom(EAccessModifier.Private)]
		public static float currentAbsoluteDMSLogVolume;

		// Token: 0x04009C93 RID: 40083
		[PublicizedFrom(EAccessModifier.Private)]
		public static float currentEventDMSLogVolume;
	}
}
