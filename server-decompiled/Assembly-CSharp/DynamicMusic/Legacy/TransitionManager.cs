using System;
using System.Collections.Generic;
using MusicUtils;
using UnityEngine.Audio;

namespace DynamicMusic.Legacy
{
	// Token: 0x02001AF7 RID: 6903
	public class TransitionManager : IGamePrefsChangedListener
	{
		// Token: 0x17001A45 RID: 6725
		// (get) Token: 0x0600D009 RID: 53257 RVA: 0x004B43C1 File Offset: 0x004B25C1
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

		// Token: 0x0600D00A RID: 53258 RVA: 0x004B43E8 File Offset: 0x004B25E8
		public static void Init(DynamicMusicManager _dynamicMusicManager)
		{
			_dynamicMusicManager.TransitionManager = new TransitionManager();
			_dynamicMusicManager.TransitionManager.dynamicMusicManager = _dynamicMusicManager;
			TransitionManager.Master = DataLoader.LoadAsset<AudioMixer>("@:Sound_Mixers/MasterAudioMixer.mixer", false);
			TransitionManager.DmsAbsoluteVolumeCurve = new LogarithmicCurve(2.0, 6.0, -80f, 0f, 0f, 1f);
			TransitionManager.SetDynamicMusicVolume(GamePrefs.GetFloat(EnumGamePrefs.OptionsMusicVolumeLevel));
			GamePrefs.AddChangeListener(_dynamicMusicManager.TransitionManager);
		}

		// Token: 0x0600D00B RID: 53259 RVA: 0x004B4464 File Offset: 0x004B2664
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

		// Token: 0x0600D00C RID: 53260 RVA: 0x004B4564 File Offset: 0x004B2764
		public static void SetDynamicMusicVolume(float _value)
		{
			if (TransitionManager.Master != null)
			{
				TransitionManager.currentAbsoluteDMSLogVolume = TransitionManager.DmsAbsoluteVolumeCurve.GetMixerValue(_value);
				TransitionManager.Master.SetFloat("dmsVol", TransitionManager.currentAbsoluteDMSLogVolume);
				TransitionManager.DmsEventRangeVolumeCurve = new LogarithmicCurve(2.0, 6.0, TransitionManager.currentAbsoluteDMSLogVolume, -80f, TransitionManager.dawnDuskFadeTime, 0f);
			}
		}

		// Token: 0x0600D00D RID: 53261 RVA: 0x004B45D3 File Offset: 0x004B27D3
		public static void ApplyPauseFilter()
		{
			if (TransitionManager.Master != null)
			{
				TransitionManager.Master.SetFloat("dmsCutOff", 500f);
			}
		}

		// Token: 0x0600D00E RID: 53262 RVA: 0x004B45F7 File Offset: 0x004B27F7
		public static void RemovePauseFilter()
		{
			if (TransitionManager.Master != null)
			{
				TransitionManager.Master.SetFloat("dmsCutOff", 22000f);
			}
		}

		// Token: 0x0600D00F RID: 53263 RVA: 0x004B461B File Offset: 0x004B281B
		public void OnGamePrefChanged(EnumGamePrefs _enum)
		{
			if (_enum == EnumGamePrefs.OptionsMusicVolumeLevel)
			{
				TransitionManager.SetDynamicMusicVolume(GamePrefs.GetFloat(EnumGamePrefs.OptionsMusicVolumeLevel));
			}
		}

		// Token: 0x04009D76 RID: 40310
		[PublicizedFrom(EAccessModifier.Private)]
		public DynamicMusicManager dynamicMusicManager;

		// Token: 0x04009D77 RID: 40311
		[PublicizedFrom(EAccessModifier.Private)]
		public static AudioMixer Master;

		// Token: 0x04009D78 RID: 40312
		[PublicizedFrom(EAccessModifier.Private)]
		public static LogarithmicCurve DmsAbsoluteVolumeCurve;

		// Token: 0x04009D79 RID: 40313
		[PublicizedFrom(EAccessModifier.Private)]
		public static LogarithmicCurve DmsEventRangeVolumeCurve;

		// Token: 0x04009D7A RID: 40314
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly float dawnDuskFadeTime = 0.16666667f;

		// Token: 0x04009D7B RID: 40315
		[PublicizedFrom(EAccessModifier.Private)]
		public static float currentAbsoluteDMSLogVolume;

		// Token: 0x04009D7C RID: 40316
		[PublicizedFrom(EAccessModifier.Private)]
		public static float currentEventDMSLogVolume;
	}
}
