using System;
using MusicUtils;
using UnityEngine.Audio;

namespace DynamicMusic
{
	// Token: 0x02001A55 RID: 6741
	public class MixerController : IGamePrefsChangedListener
	{
		// Token: 0x17001901 RID: 6401
		// (get) Token: 0x0600CC4C RID: 52300 RVA: 0x004A9450 File Offset: 0x004A7650
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

		// Token: 0x0600CC4D RID: 52301 RVA: 0x004A9468 File Offset: 0x004A7668
		public void Init()
		{
			MixerController.DmsAbsoluteVolumeCurve = new LogarithmicCurve(2.0, 6.0, -80f, 0f, 0f, 1f);
			MixerController.AllCombatVolumeCurve = new LogarithmicCurve(2.0, 6.0, -4f, 0f, 0.7f, 1f);
			MixerController.Master = DataLoader.LoadAsset<AudioMixer>("@:Sound_Mixers/MasterAudioMixer.mixer", false);
			this.SetDynamicMusicVolume(GamePrefs.GetFloat(EnumGamePrefs.OptionsMusicVolumeLevel));
			GamePrefs.AddChangeListener(this);
		}

		// Token: 0x0600CC4E RID: 52302 RVA: 0x004A94F7 File Offset: 0x004A76F7
		public void Update()
		{
			this.SetAllCombatVolume();
		}

		// Token: 0x0600CC4F RID: 52303 RVA: 0x004A94FF File Offset: 0x004A76FF
		public void OnGamePrefChanged(EnumGamePrefs _enum)
		{
			if (_enum.Equals(EnumGamePrefs.OptionsMusicVolumeLevel))
			{
				this.SetDynamicMusicVolume(GamePrefs.GetFloat(EnumGamePrefs.OptionsMusicVolumeLevel));
			}
		}

		// Token: 0x0600CC50 RID: 52304 RVA: 0x004A9522 File Offset: 0x004A7722
		public void SetDynamicMusicVolume(float _vol)
		{
			if (MixerController.Master)
			{
				MixerController.Master.SetFloat("dmsVol", MixerController.DmsAbsoluteVolumeCurve.GetMixerValue(_vol));
			}
		}

		// Token: 0x0600CC51 RID: 52305 RVA: 0x004A954C File Offset: 0x004A774C
		public void OnSnapshotTransition()
		{
			float @float = GamePrefs.GetFloat(EnumGamePrefs.OptionsMusicVolumeLevel);
			this.SetDynamicMusicVolume(@float);
		}

		// Token: 0x0600CC52 RID: 52306 RVA: 0x004A9568 File Offset: 0x004A7768
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

		// Token: 0x04009B3A RID: 39738
		[PublicizedFrom(EAccessModifier.Private)]
		public static MixerController instance;

		// Token: 0x04009B3B RID: 39739
		[PublicizedFrom(EAccessModifier.Private)]
		public static AudioMixer Master;

		// Token: 0x04009B3C RID: 39740
		[PublicizedFrom(EAccessModifier.Private)]
		public static LogarithmicCurve DmsAbsoluteVolumeCurve;

		// Token: 0x04009B3D RID: 39741
		[PublicizedFrom(EAccessModifier.Private)]
		public static LogarithmicCurve AllCombatVolumeCurve;
	}
}
