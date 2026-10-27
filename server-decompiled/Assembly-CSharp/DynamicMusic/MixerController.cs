using System;
using MusicUtils;
using UnityEngine.Audio;

namespace DynamicMusic
{
	// Token: 0x02001AB2 RID: 6834
	public class MixerController : IGamePrefsChangedListener
	{
		// Token: 0x170019EB RID: 6635
		// (get) Token: 0x0600CEB8 RID: 52920 RVA: 0x004B1418 File Offset: 0x004AF618
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

		// Token: 0x0600CEB9 RID: 52921 RVA: 0x004B1430 File Offset: 0x004AF630
		public void Init()
		{
			MixerController.DmsAbsoluteVolumeCurve = new LogarithmicCurve(2.0, 6.0, -80f, 0f, 0f, 1f);
			MixerController.AllCombatVolumeCurve = new LogarithmicCurve(2.0, 6.0, -4f, 0f, 0.7f, 1f);
			MixerController.Master = DataLoader.LoadAsset<AudioMixer>("@:Sound_Mixers/MasterAudioMixer.mixer", false);
			this.SetDynamicMusicVolume(GamePrefs.GetFloat(EnumGamePrefs.OptionsMusicVolumeLevel));
			GamePrefs.AddChangeListener(this);
		}

		// Token: 0x0600CEBA RID: 52922 RVA: 0x004B14BF File Offset: 0x004AF6BF
		public void Update()
		{
			this.SetAllCombatVolume();
		}

		// Token: 0x0600CEBB RID: 52923 RVA: 0x004B14C7 File Offset: 0x004AF6C7
		public void OnGamePrefChanged(EnumGamePrefs _enum)
		{
			if (_enum.Equals(EnumGamePrefs.OptionsMusicVolumeLevel))
			{
				this.SetDynamicMusicVolume(GamePrefs.GetFloat(EnumGamePrefs.OptionsMusicVolumeLevel));
			}
		}

		// Token: 0x0600CEBC RID: 52924 RVA: 0x004B14EA File Offset: 0x004AF6EA
		public void SetDynamicMusicVolume(float _vol)
		{
			if (MixerController.Master)
			{
				MixerController.Master.SetFloat("dmsVol", MixerController.DmsAbsoluteVolumeCurve.GetMixerValue(_vol));
			}
		}

		// Token: 0x0600CEBD RID: 52925 RVA: 0x004B1514 File Offset: 0x004AF714
		public void OnSnapshotTransition()
		{
			float @float = GamePrefs.GetFloat(EnumGamePrefs.OptionsMusicVolumeLevel);
			this.SetDynamicMusicVolume(@float);
		}

		// Token: 0x0600CEBE RID: 52926 RVA: 0x004B1530 File Offset: 0x004AF730
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

		// Token: 0x04009CE9 RID: 40169
		[PublicizedFrom(EAccessModifier.Private)]
		public static MixerController instance;

		// Token: 0x04009CEA RID: 40170
		[PublicizedFrom(EAccessModifier.Private)]
		public static AudioMixer Master;

		// Token: 0x04009CEB RID: 40171
		[PublicizedFrom(EAccessModifier.Private)]
		public static LogarithmicCurve DmsAbsoluteVolumeCurve;

		// Token: 0x04009CEC RID: 40172
		[PublicizedFrom(EAccessModifier.Private)]
		public static LogarithmicCurve AllCombatVolumeCurve;
	}
}
