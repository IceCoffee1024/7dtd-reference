using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000B7 RID: 183
[PublicizedFrom(EAccessModifier.Internal)]
public class AudioTrigger
{
	// Token: 0x06000387 RID: 903 RVA: 0x0001A03A File Offset: 0x0001823A
	public AudioTrigger(AudioObject.Trigger _trigger)
	{
		this.trigger = _trigger;
	}

	// Token: 0x06000388 RID: 904 RVA: 0x0001A054 File Offset: 0x00018254
	public void Add(AudioObject _audioObject)
	{
		this.sound.Add(_audioObject);
	}

	// Token: 0x06000389 RID: 905 RVA: 0x0001A064 File Offset: 0x00018264
	public void Update()
	{
		float fixedDeltaTime = Time.fixedDeltaTime;
		for (int i = this.sound.Count - 1; i >= 0; i--)
		{
			this.sound[i].Update(fixedDeltaTime);
		}
	}

	// Token: 0x0600038A RID: 906 RVA: 0x0001A0A4 File Offset: 0x000182A4
	public void SetVolume(float _vol)
	{
		for (int i = this.sound.Count - 1; i >= 0; i--)
		{
			this.sound[i].SetBiomeVolume(_vol);
		}
	}

	// Token: 0x0600038B RID: 907 RVA: 0x0001A0DC File Offset: 0x000182DC
	public void Pause()
	{
		for (int i = this.sound.Count - 1; i >= 0; i--)
		{
			this.sound[i].Pause();
		}
	}

	// Token: 0x0600038C RID: 908 RVA: 0x0001A114 File Offset: 0x00018314
	public void UnPause()
	{
		for (int i = this.sound.Count - 1; i >= 0; i--)
		{
			this.sound[i].UnPause();
		}
	}

	// Token: 0x0600038D RID: 909 RVA: 0x0001A14C File Offset: 0x0001834C
	public void TurnOff()
	{
		for (int i = this.sound.Count - 1; i >= 0; i--)
		{
			this.sound[i].TurnOff(false);
		}
	}

	// Token: 0x04000413 RID: 1043
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly AudioObject.Trigger trigger;

	// Token: 0x04000414 RID: 1044
	public List<AudioObject> sound = new List<AudioObject>();
}
