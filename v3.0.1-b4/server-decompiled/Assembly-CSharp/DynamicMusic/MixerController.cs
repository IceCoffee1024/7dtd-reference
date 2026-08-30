using System;
using MusicUtils;
using UnityEngine.Audio;

namespace DynamicMusic
{
	// Token: 0x02001A2E RID: 6702
	public class MixerController : IGamePrefsChangedListener
	{
		// Token: 0x1700188F RID: 6287
		// (get) Token: 0x0600CAC3 RID: 51907 RVA: 0x004A305C File Offset: 0x004A125C
		public static MixerController Instance
		{
			get
			{
				if (MixerController.instance == null)
				{
					MixerController.instance = new MixerController();
				}
				return MixerController.instance;
			}
		}

		// Token: 0x0600CAC4 RID: 51908 RVA: 0x004A3074 File Offset: 0x004A1274
		public void Init()
		{
			MixerController.DmsAbsoluteVolumeCurve = new LogarithmicCurve(2.0, 6.0, -80f, 0f, 0f, 1f);
			MixerController.AllCombatVolumeCurve = new LogarithmicCurve(2.0, 6.0, -4f, 0f, 0.7f, 1f);
			MixerController.Master = DataLoader.LoadAsset<AudioMixer>("@:Sound_Mixers/MasterAudioMixer.mixer", false);
			this.SetDynamicMusicVolume(GamePrefs.GetFloat(EnumGamePrefs.OptionsMusicVolumeLevel));
			GamePrefs.AddChangeListener(this);
		}

		// Token: 0x0600CAC5 RID: 51909 RVA: 0x004A3103 File Offset: 0x004A1303
		public void Update()
		{
			this.SetAllCombatVolume();
		}

		// Token: 0x0600CAC6 RID: 51910 RVA: 0x004A310B File Offset: 0x004A130B
		public void OnGamePrefChanged(EnumGamePrefs _enum)
		{
			if (_enum.Equals(EnumGamePrefs.OptionsMusicVolumeLevel))
			{
				this.SetDynamicMusicVolume(GamePrefs.GetFloat(EnumGamePrefs.OptionsMusicVolumeLevel));
			}
		}

		// Token: 0x0600CAC7 RID: 51911 RVA: 0x004A312E File Offset: 0x004A132E
		public void SetDynamicMusicVolume(float _vol)
		{
			if (MixerController.Master)
			{
				MixerController.Master.SetFloat("dmsVol", MixerController.DmsAbsoluteVolumeCurve.GetMixerValue(_vol));
			}
		}

		// Token: 0x0600CAC8 RID: 51912 RVA: 0x004A3158 File Offset: 0x004A1358
		public void OnSnapshotTransition()
		{
			float @float = GamePrefs.GetFloat(EnumGamePrefs.OptionsMusicVolumeLevel);
			this.SetDynamicMusicVolume(@float);
		}

		// Token: 0x0600CAC9 RID: 51913 RVA: 0x004A3174 File Offset: 0x004A1374
		[PublicizedFrom(EAccessModifier.Private)]
		public void SetAllCombatVolume()
		{
			if (MixerController.Master)
			{
				float mixerValue = MixerController.AllCombatVolumeCurve.GetMixerValue(GameManager.Instance.World.GetPrimaryPlayer().ThreatLevel.Numeric);
				AudioMixer master = MixerController.Master;
				if (master == null)
				{
					return;
				}
				master.SetFloat("AllCbtVol", mixerValue);
			}
		}

		// Token: 0x04009A09 RID: 39433
		[PublicizedFrom(EAccessModifier.Private)]
		public static MixerController instance;

		// Token: 0x04009A0A RID: 39434
		[PublicizedFrom(EAccessModifier.Private)]
		public static AudioMixer Master;

		// Token: 0x04009A0B RID: 39435
		[PublicizedFrom(EAccessModifier.Private)]
		public static LogarithmicCurve DmsAbsoluteVolumeCurve;

		// Token: 0x04009A0C RID: 39436
		[PublicizedFrom(EAccessModifier.Private)]
		public static LogarithmicCurve AllCombatVolumeCurve;
	}
}
