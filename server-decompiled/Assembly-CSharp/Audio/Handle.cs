using System;
using UnityEngine;

namespace Audio
{
	// Token: 0x02001B78 RID: 7032
	public class Handle
	{
		// Token: 0x0600D2A2 RID: 53922 RVA: 0x004C48F4 File Offset: 0x004C2AF4
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

		// Token: 0x0600D2A3 RID: 53923 RVA: 0x004C494C File Offset: 0x004C2B4C
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

		// Token: 0x0600D2A4 RID: 53924 RVA: 0x004C499C File Offset: 0x004C2B9C
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

		// Token: 0x0600D2A5 RID: 53925 RVA: 0x004C49E9 File Offset: 0x004C2BE9
		public void Stop(int entityId)
		{
			Manager.Stop(entityId, this.name);
		}

		// Token: 0x0600D2A6 RID: 53926 RVA: 0x004C49F8 File Offset: 0x004C2BF8
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

		// Token: 0x0600D2A7 RID: 53927 RVA: 0x004C4A48 File Offset: 0x004C2C48
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

		// Token: 0x0400A051 RID: 41041
		[PublicizedFrom(EAccessModifier.Private)]
		public string name;

		// Token: 0x0400A052 RID: 41042
		[PublicizedFrom(EAccessModifier.Private)]
		public AudioSource nearSource;

		// Token: 0x0400A053 RID: 41043
		[PublicizedFrom(EAccessModifier.Private)]
		public AudioSource farSource;

		// Token: 0x0400A054 RID: 41044
		[PublicizedFrom(EAccessModifier.Private)]
		public float basePitch;

		// Token: 0x0400A055 RID: 41045
		[PublicizedFrom(EAccessModifier.Private)]
		public float baseVolume;
	}
}
