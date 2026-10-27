using System;
using UnityEngine;

// Token: 0x02001516 RID: 5398
public class AudioGamepadRumbleSource
{
	// Token: 0x0600A8E4 RID: 43236 RVA: 0x003EDF05 File Offset: 0x003EC105
	public AudioGamepadRumbleSource()
	{
		this.samples = new float[64];
	}

	// Token: 0x0600A8E5 RID: 43237 RVA: 0x003EDF1A File Offset: 0x003EC11A
	public void SetAudioSource(AudioSource _audioSource, float _strengthMultiplier, bool _locationBased)
	{
		this.audioSrc = _audioSource;
		this.strengthMultiplier = _strengthMultiplier;
		this.locationBased = _locationBased;
		this.timeAdded = Time.time;
	}

	// Token: 0x0600A8E6 RID: 43238 RVA: 0x003EDF3C File Offset: 0x003EC13C
	public float GetSample(int channel)
	{
		this.audioSrc.GetOutputData(this.samples, channel);
		float num = 0f;
		for (int i = 0; i < 64; i++)
		{
			num += this.samples[i];
		}
		return num / 64f;
	}

	// Token: 0x0600A8E7 RID: 43239 RVA: 0x003EDF82 File Offset: 0x003EC182
	public void Clear()
	{
		this.audioSrc = null;
	}

	// Token: 0x04007D5F RID: 32095
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cSampleCount = 64;

	// Token: 0x04007D60 RID: 32096
	public AudioSource audioSrc;

	// Token: 0x04007D61 RID: 32097
	public float[] samples;

	// Token: 0x04007D62 RID: 32098
	public float strengthMultiplier;

	// Token: 0x04007D63 RID: 32099
	public bool locationBased;

	// Token: 0x04007D64 RID: 32100
	public float timeAdded;
}
