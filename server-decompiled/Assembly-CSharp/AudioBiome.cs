using System;

// Token: 0x020000B2 RID: 178
[PublicizedFrom(EAccessModifier.Internal)]
public class AudioBiome
{
	// Token: 0x0600036B RID: 875 RVA: 0x00018CE4 File Offset: 0x00016EE4
	public AudioBiome()
	{
		int num = Enum.GetNames(typeof(AudioObject.Trigger)).Length;
		this.triggers = new AudioTrigger[num];
		for (int i = 0; i < num; i++)
		{
			this.triggers[i] = new AudioTrigger((AudioObject.Trigger)i);
		}
	}

	// Token: 0x0600036C RID: 876 RVA: 0x00018D2F File Offset: 0x00016F2F
	public void Add(AudioObject _audioObject)
	{
		this.triggers[(int)_audioObject.trigger].Add(_audioObject);
	}

	// Token: 0x0600036D RID: 877 RVA: 0x00018D44 File Offset: 0x00016F44
	public void TransitionFrom(float _biomeTransition)
	{
		for (int i = 3; i < this.triggers.Length - 1; i++)
		{
			AudioTrigger audioTrigger = this.triggers[i];
			if (i != 5 && i != 4)
			{
				audioTrigger.TurnOff();
				audioTrigger.SetVolume(1f - _biomeTransition);
			}
			audioTrigger.Update();
		}
	}

	// Token: 0x0600036E RID: 878 RVA: 0x00018D90 File Offset: 0x00016F90
	public void TransitionTo(float _biomeTransition)
	{
		for (int i = 3; i < this.triggers.Length - 1; i++)
		{
			AudioTrigger audioTrigger = this.triggers[i];
			if (i != 5 && i != 4)
			{
				audioTrigger.SetVolume(_biomeTransition);
			}
			audioTrigger.Update();
		}
	}

	// Token: 0x0600036F RID: 879 RVA: 0x00018DD0 File Offset: 0x00016FD0
	public void Pause()
	{
		for (int i = 0; i < this.triggers.Length; i++)
		{
			this.triggers[i].Pause();
		}
	}

	// Token: 0x06000370 RID: 880 RVA: 0x00018E00 File Offset: 0x00017000
	public void UnPause()
	{
		for (int i = 0; i < this.triggers.Length; i++)
		{
			this.triggers[i].UnPause();
		}
	}

	// Token: 0x06000371 RID: 881 RVA: 0x00018E30 File Offset: 0x00017030
	public void TurnOff()
	{
		for (int i = 0; i < this.triggers.Length; i++)
		{
			this.triggers[i].TurnOff();
		}
	}

	// Token: 0x040003D0 RID: 976
	public AudioTrigger[] triggers;
}
