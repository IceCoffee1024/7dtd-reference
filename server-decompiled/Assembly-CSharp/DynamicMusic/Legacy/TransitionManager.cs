using System;
using System.Collections.Generic;
using MusicUtils;
using UnityEngine.Audio;

namespace DynamicMusic.Legacy
{
	// Token: 0x02001A9A RID: 6810
	public class TransitionManager : IGamePrefsChangedListener
	{
		// Token: 0x1700195B RID: 6491
		// (get) Token: 0x0600CD9D RID: 52637 RVA: 0x004AC3F9 File Offset: 0x004AA5F9
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

		// Token: 0x0600CD9E RID: 52638 RVA: 0x004AC420 File Offset: 0x004AA620
		public static void Init(DynamicMusicManager _dynamicMusicManager)
		{
			_dynamicMusicManager.TransitionManager = new TransitionManager();
			_dynamicMusicManager.TransitionManager.dynamicMusicManager = _dynamicMusicManager;
			TransitionManager.Master = DataLoader.LoadAsset<AudioMixer>("@:Sound_Mixers/MasterAudioMixer.mixer", false);
			TransitionManager.DmsAbsoluteVolumeCurve = new LogarithmicCurve(2.0, 6.0, -80f, 0f, 0f, 1f);
			TransitionManager.SetDynamicMusicVolume(GamePrefs.GetFloat(EnumGamePrefs.OptionsMusicVolumeLevel));
			GamePrefs.AddChangeListener(_dynamicMusicManager.TransitionManager);
		}

		// Token: 0x0600CD9F RID: 52639 RVA: 0x004AC49C File Offset: 0x004AA69C
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

		// Token: 0x0600CDA0 RID: 52640 RVA: 0x004AC59C File Offset: 0x004AA79C
		public static void SetDynamicMusicVolume(float _value)
		{
			if (TransitionManager.Master != null)
			{
				TransitionManager.currentAbsoluteDMSLogVolume = TransitionManager.DmsAbsoluteVolumeCurve.GetMixerValue(_value);
				TransitionManager.Master.SetFloat("dmsVol", TransitionManager.currentAbsoluteDMSLogVolume);
				TransitionManager.DmsEventRangeVolumeCurve = new LogarithmicCurve(2.0, 6.0, TransitionManager.currentAbsoluteDMSLogVolume, -80f, TransitionManager.dawnDuskFadeTime, 0f);
			}
		}

		// Token: 0x0600CDA1 RID: 52641 RVA: 0x004AC60B File Offset: 0x004AA80B
		public static void ApplyPauseFilter()
		{
			if (TransitionManager.Master != null)
			{
				TransitionManager.Master.SetFloat("dmsCutOff", 500f);
			}
		}

		// Token: 0x0600CDA2 RID: 52642 RVA: 0x004AC62F File Offset: 0x004AA82F
		public static void RemovePauseFilter()
		{
			if (TransitionManager.Master != null)
			{
				TransitionManager.Master.SetFloat("dmsCutOff", 22000f);
			}
		}

		// Token: 0x0600CDA3 RID: 52643 RVA: 0x004AC653 File Offset: 0x004AA853
		public void OnGamePrefChanged(EnumGamePrefs _enum)
		{
			if (_enum == EnumGamePrefs.OptionsMusicVolumeLevel)
			{
				TransitionManager.SetDynamicMusicVolume(GamePrefs.GetFloat(EnumGamePrefs.OptionsMusicVolumeLevel));
			}
		}

		// Token: 0x04009BC7 RID: 39879
		[PublicizedFrom(EAccessModifier.Private)]
		public DynamicMusicManager dynamicMusicManager;

		// Token: 0x04009BC8 RID: 39880
		[PublicizedFrom(EAccessModifier.Private)]
		public static AudioMixer Master;

		// Token: 0x04009BC9 RID: 39881
		[PublicizedFrom(EAccessModifier.Private)]
		public static LogarithmicCurve DmsAbsoluteVolumeCurve;

		// Token: 0x04009BCA RID: 39882
		[PublicizedFrom(EAccessModifier.Private)]
		public static LogarithmicCurve DmsEventRangeVolumeCurve;

		// Token: 0x04009BCB RID: 39883
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly float dawnDuskFadeTime = 0.16666667f;

		// Token: 0x04009BCC RID: 39884
		[PublicizedFrom(EAccessModifier.Private)]
		public static float currentAbsoluteDMSLogVolume;

		// Token: 0x04009BCD RID: 39885
		[PublicizedFrom(EAccessModifier.Private)]
		public static float currentEventDMSLogVolume;
	}
}
