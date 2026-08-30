using System;
using UnityEngine;

namespace Audio
{
	// Token: 0x02001AF4 RID: 6900
	public class Handle
	{
		// Token: 0x0600CEAD RID: 52909 RVA: 0x004B650C File Offset: 0x004B470C
		public Handle(string soundGroupName, AudioSource near, AudioSource far)
		{
			this.name = soundGroupName;
			this.nearSource = near;
			this.farSource = far;
			if (this.nearSource)
			{
				this.basePitch = this.nearSource.pitch;
				this.baseVolume = this.nearSource.volume;
			}
		}

		// Token: 0x0600CEAE RID: 52910 RVA: 0x004B6564 File Offset: 0x004B4764
		public void SetPitch(float pitch)
		{
			if (this.nearSource)
			{
				this.nearSource.pitch = pitch + this.basePitch;
			}
			if (this.farSource)
			{
				this.farSource.pitch = pitch + this.basePitch;
			}
		}

		// Token: 0x0600CEAF RID: 52911 RVA: 0x004B65B4 File Offset: 0x004B47B4
		public void SetVolume(float volume)
		{
			if (this.nearSource)
			{
				this.nearSource.volume = volume * this.baseVolume;
			}
			if (this.farSource)
			{
				this.farSource.volume = volume * this.baseVolume;
			}
		}

		// Token: 0x0600CEB0 RID: 52912 RVA: 0x004B6601 File Offset: 0x004B4801
		public void Stop(int entityId)
		{
			Manager.Stop(entityId, this.name);
		}

		// Token: 0x0600CEB1 RID: 52913 RVA: 0x004B6610 File Offset: 0x004B4810
		public float ClipLength()
		{
			if (this.nearSource)
			{
				return this.nearSource.clip.length;
			}
			if (this.farSource)
			{
				return this.farSource.clip.length;
			}
			return 0f;
		}

		// Token: 0x0600CEB2 RID: 52914 RVA: 0x004B6660 File Offset: 0x004B4860
		public bool IsPlaying()
		{
			bool flag = false;
			if (this.nearSource != null)
			{
				flag = this.nearSource.isPlaying;
			}
			if (this.farSource != null)
			{
				flag |= this.farSource.isPlaying;
			}
			return flag;
		}

		// Token: 0x04009D71 RID: 40305
		[PublicizedFrom(EAccessModifier.Private)]
		public string name;

		// Token: 0x04009D72 RID: 40306
		[PublicizedFrom(EAccessModifier.Private)]
		public AudioSource nearSource;

		// Token: 0x04009D73 RID: 40307
		[PublicizedFrom(EAccessModifier.Private)]
		public AudioSource farSource;

		// Token: 0x04009D74 RID: 40308
		[PublicizedFrom(EAccessModifier.Private)]
		public float basePitch;

		// Token: 0x04009D75 RID: 40309
		[PublicizedFrom(EAccessModifier.Private)]
		public float baseVolume;
	}
}
