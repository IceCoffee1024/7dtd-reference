using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000B9 RID: 185
[PublicizedFrom(EAccessModifier.Internal)]
public class AudioTrigger
{
	// Token: 0x0600038D RID: 909 RVA: 0x0001A132 File Offset: 0x00018332
	public AudioTrigger(AudioObject.Trigger _trigger)
	{
		this.trigger = _trigger;
	}

	// Token: 0x0600038E RID: 910 RVA: 0x0001A14C File Offset: 0x0001834C
	public void Add(AudioObject _audioObject)
	{
		this.sound.Add(_audioObject);
	}

	// Token: 0x0600038F RID: 911 RVA: 0x0001A15C File Offset: 0x0001835C
	public void Update()
	{
		float fixedDeltaTime = Time.fixedDeltaTime;
		for (int i = this.sound.Count - 1; i >= 0; i--)
		{
			this.sound[i].Update(fixedDeltaTime);
		}
	}

	// Token: 0x06000390 RID: 912 RVA: 0x0001A19C File Offset: 0x0001839C
	public void SetVolume(float _vol)
	{
		for (int i = this.sound.Count - 1; i >= 0; i--)
		{
			this.sound[i].SetBiomeVolume(_vol);
		}
	}

	// Token: 0x06000391 RID: 913 RVA: 0x0001A1D4 File Offset: 0x000183D4
	public void Pause()
	{
		for (int i = this.sound.Count - 1; i >= 0; i--)
		{
			this.sound[i].Pause();
		}
	}

	// Token: 0x06000392 RID: 914 RVA: 0x0001A20C File Offset: 0x0001840C
	public void UnPause()
	{
		for (int i = this.sound.Count - 1; i >= 0; i--)
		{
			this.sound[i].UnPause();
		}
	}

	// Token: 0x06000393 RID: 915 RVA: 0x0001A244 File Offset: 0x00018444
	public void TurnOff()
	{
		for (int i = this.sound.Count - 1; i >= 0; i--)
		{
			this.sound[i].TurnOff(false);
		}
	}

	// Token: 0x04000416 RID: 1046
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly AudioObject.Trigger trigger;

	// Token: 0x04000417 RID: 1047
	public List<AudioObject> sound = new List<AudioObject>();
}
