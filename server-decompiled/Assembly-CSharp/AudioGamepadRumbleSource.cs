using System;
using UnityEngine;

// Token: 0x020014C7 RID: 5319
public class AudioGamepadRumbleSource
{
	// Token: 0x0600A6DC RID: 42716 RVA: 0x003E196D File Offset: 0x003DFB6D
	public AudioGamepadRumbleSource()
	{
		this.samples = new float[64];
	}

	// Token: 0x0600A6DD RID: 42717 RVA: 0x003E1982 File Offset: 0x003DFB82
	public void SetAudioSource(AudioSource _audioSource, float _strengthMultiplier, bool _locationBased)
	{
		this.audioSrc = _audioSource;
		this.strengthMultiplier = _strengthMultiplier;
		this.locationBased = _locationBased;
		this.timeAdded = Time.time;
	}

	// Token: 0x0600A6DE RID: 42718 RVA: 0x003E19A4 File Offset: 0x003DFBA4
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

	// Token: 0x0600A6DF RID: 42719 RVA: 0x003E19EA File Offset: 0x003DFBEA
	public void Clear()
	{
		this.audioSrc = null;
	}

	// Token: 0x04007BC2 RID: 31682
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cSampleCount = 64;

	// Token: 0x04007BC3 RID: 31683
	public AudioSource audioSrc;

	// Token: 0x04007BC4 RID: 31684
	public float[] samples;

	// Token: 0x04007BC5 RID: 31685
	public float strengthMultiplier;

	// Token: 0x04007BC6 RID: 31686
	public bool locationBased;

	// Token: 0x04007BC7 RID: 31687
	public float timeAdded;
}
