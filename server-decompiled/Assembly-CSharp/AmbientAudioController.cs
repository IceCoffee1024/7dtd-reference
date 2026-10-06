using System;
using MusicUtils;
using UnityEngine.Audio;

// Token: 0x020000B0 RID: 176
public class AmbientAudioController : IGamePrefsChangedListener
{
	// Token: 0x17000048 RID: 72
	// (get) Token: 0x06000357 RID: 855 RVA: 0x000188AE File Offset: 0x00016AAE
	public static AmbientAudioController Instance
	{
		get
		{
			if (AmbientAudioController.instance == null)
			{
				AmbientAudioController.instance = new AmbientAudioController();
			}
			return AmbientAudioController.instance;
		}
	}

	// Token: 0x06000358 RID: 856 RVA: 0x000188C6 File Offset: 0x00016AC6
	[PublicizedFrom(EAccessModifier.Private)]
	public AmbientAudioController()
	{
		GamePrefs.AddChangeListener(this);
	}

	// Token: 0x0600035A RID: 858 RVA: 0x00018921 File Offset: 0x00016B21
	public void SetAmbientVolume(float _val)
	{
		if (AmbientAudioController.master)
		{
			AmbientAudioController.master.SetFloat("ambVol", AmbientAudioController.volumeCurve.GetMixerValue(_val));
		}
	}

	// Token: 0x0600035B RID: 859 RVA: 0x0001894A File Offset: 0x00016B4A
	public void OnGamePrefChanged(EnumGamePrefs _enum)
	{
		if (_enum == EnumGamePrefs.OptionsAmbientVolumeLevel)
		{
			this.SetAmbientVolume(GamePrefs.GetFloat(EnumGamePrefs.OptionsAmbientVolumeLevel));
		}
	}

	// Token: 0x040003CB RID: 971
	[PublicizedFrom(EAccessModifier.Private)]
	public static AmbientAudioController instance;

	// Token: 0x040003CC RID: 972
	[PublicizedFrom(EAccessModifier.Private)]
	public static AudioMixer master = DataLoader.LoadAsset<AudioMixer>("@:Sound_Mixers/MasterAudioMixer.mixer", false);

	// Token: 0x040003CD RID: 973
	[PublicizedFrom(EAccessModifier.Private)]
	public static LogarithmicCurve volumeCurve = new LogarithmicCurve(2.0, 6.0, -80f, 0f, 0f, 1f);
}
