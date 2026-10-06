using System;
using UnityEngine;

// Token: 0x02001504 RID: 5380
public class AudioGamepadRumbleSource
{
	// Token: 0x0600A88C RID: 43148 RVA: 0x003EBB01 File Offset: 0x003E9D01
	public AudioGamepadRumbleSource()
	{
		this.samples = new float[64];
	}

	// Token: 0x0600A88D RID: 43149 RVA: 0x003EBB16 File Offset: 0x003E9D16
	public void SetAudioSource(AudioSource _audioSource, float _strengthMultiplier, bool _locationBased)
	{
		this.audioSrc = _audioSource;
		this.strengthMultiplier = _strengthMultiplier;
		this.locationBased = _locationBased;
		this.timeAdded = Time.time;
	}

	// Token: 0x0600A88E RID: 43150 RVA: 0x003EBB38 File Offset: 0x003E9D38
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

	// Token: 0x0600A88F RID: 43151 RVA: 0x003EBB7E File Offset: 0x003E9D7E
	public void Clear()
	{
		this.audioSrc = null;
	}

	// Token: 0x04007D1E RID: 32030
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cSampleCount = 64;

	// Token: 0x04007D1F RID: 32031
	public AudioSource audioSrc;

	// Token: 0x04007D20 RID: 32032
	public float[] samples;

	// Token: 0x04007D21 RID: 32033
	public float strengthMultiplier;

	// Token: 0x04007D22 RID: 32034
	public bool locationBased;

	// Token: 0x04007D23 RID: 32035
	public float timeAdded;
}
