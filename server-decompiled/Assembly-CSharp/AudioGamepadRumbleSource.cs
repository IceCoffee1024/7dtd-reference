using System;
using UnityEngine;

// Token: 0x020014E2 RID: 5346
public class AudioGamepadRumbleSource
{
	// Token: 0x0600A7D9 RID: 42969 RVA: 0x003E6CAD File Offset: 0x003E4EAD
	public AudioGamepadRumbleSource()
	{
		this.samples = new float[64];
	}

	// Token: 0x0600A7DA RID: 42970 RVA: 0x003E6CC2 File Offset: 0x003E4EC2
	public void SetAudioSource(AudioSource _audioSource, float _strengthMultiplier, bool _locationBased)
	{
		this.audioSrc = _audioSource;
		this.strengthMultiplier = _strengthMultiplier;
		this.locationBased = _locationBased;
		this.timeAdded = Time.time;
	}

	// Token: 0x0600A7DB RID: 42971 RVA: 0x003E6CE4 File Offset: 0x003E4EE4
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

	// Token: 0x0600A7DC RID: 42972 RVA: 0x003E6D2A File Offset: 0x003E4F2A
	public void Clear()
	{
		this.audioSrc = null;
	}

	// Token: 0x04007C9B RID: 31899
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cSampleCount = 64;

	// Token: 0x04007C9C RID: 31900
	public AudioSource audioSrc;

	// Token: 0x04007C9D RID: 31901
	public float[] samples;

	// Token: 0x04007C9E RID: 31902
	public float strengthMultiplier;

	// Token: 0x04007C9F RID: 31903
	public bool locationBased;

	// Token: 0x04007CA0 RID: 31904
	public float timeAdded;
}
