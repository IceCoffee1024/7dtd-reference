using System;
using MusicUtils;
using UnityEngine.Audio;

namespace DynamicMusic
{
	// Token: 0x02001A81 RID: 6785
	public class MixerController : IGamePrefsChangedListener
	{
		// Token: 0x170019A3 RID: 6563
		// (get) Token: 0x0600CDA4 RID: 52644 RVA: 0x004AD91C File Offset: 0x004ABB1C
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

		// Token: 0x0600CDA5 RID: 52645 RVA: 0x004AD934 File Offset: 0x004ABB34
		public void Init()
		{
			MixerController.DmsAbsoluteVolumeCurve = new LogarithmicCurve(2.0, 6.0, -80f, 0f, 0f, 1f);
			MixerController.AllCombatVolumeCurve = new LogarithmicCurve(2.0, 6.0, -4f, 0f, 0.7f, 1f);
			MixerController.Master = DataLoader.LoadAsset<AudioMixer>("@:Sound_Mixers/MasterAudioMixer.mixer", false);
			this.SetDynamicMusicVolume(GamePrefs.GetFloat(EnumGamePrefs.OptionsMusicVolumeLevel));
			GamePrefs.AddChangeListener(this);
		}

		// Token: 0x0600CDA6 RID: 52646 RVA: 0x004AD9C3 File Offset: 0x004ABBC3
		public void Update()
		{
			this.SetAllCombatVolume();
		}

		// Token: 0x0600CDA7 RID: 52647 RVA: 0x004AD9CB File Offset: 0x004ABBCB
		public void OnGamePrefChanged(EnumGamePrefs _enum)
		{
			if (_enum.Equals(EnumGamePrefs.OptionsMusicVolumeLevel))
			{
				this.SetDynamicMusicVolume(GamePrefs.GetFloat(EnumGamePrefs.OptionsMusicVolumeLevel));
			}
		}

		// Token: 0x0600CDA8 RID: 52648 RVA: 0x004AD9EE File Offset: 0x004ABBEE
		public void SetDynamicMusicVolume(float _vol)
		{
			if (MixerController.Master)
			{
				MixerController.Master.SetFloat("dmsVol", MixerController.DmsAbsoluteVolumeCurve.GetMixerValue(_vol));
			}
		}

		// Token: 0x0600CDA9 RID: 52649 RVA: 0x004ADA18 File Offset: 0x004ABC18
		public void OnSnapshotTransition()
		{
			float @float = GamePrefs.GetFloat(EnumGamePrefs.OptionsMusicVolumeLevel);
			this.SetDynamicMusicVolume(@float);
		}

		// Token: 0x0600CDAA RID: 52650 RVA: 0x004ADA34 File Offset: 0x004ABC34
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

		// Token: 0x04009C00 RID: 39936
		[PublicizedFrom(EAccessModifier.Private)]
		public static MixerController instance;

		// Token: 0x04009C01 RID: 39937
		[PublicizedFrom(EAccessModifier.Private)]
		public static AudioMixer Master;

		// Token: 0x04009C02 RID: 39938
		[PublicizedFrom(EAccessModifier.Private)]
		public static LogarithmicCurve DmsAbsoluteVolumeCurve;

		// Token: 0x04009C03 RID: 39939
		[PublicizedFrom(EAccessModifier.Private)]
		public static LogarithmicCurve AllCombatVolumeCurve;
	}
}
