using System;
using UnityEngine;

namespace Audio
{
	// Token: 0x02001B1B RID: 6939
	public class Handle
	{
		// Token: 0x0600D036 RID: 53302 RVA: 0x004BC900 File Offset: 0x004BAB00
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

		// Token: 0x0600D037 RID: 53303 RVA: 0x004BC958 File Offset: 0x004BAB58
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

		// Token: 0x0600D038 RID: 53304 RVA: 0x004BC9A8 File Offset: 0x004BABA8
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

		// Token: 0x0600D039 RID: 53305 RVA: 0x004BC9F5 File Offset: 0x004BABF5
		public void Stop(int entityId)
		{
			Manager.Stop(entityId, this.name);
		}

		// Token: 0x0600D03A RID: 53306 RVA: 0x004BCA04 File Offset: 0x004BAC04
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

		// Token: 0x0600D03B RID: 53307 RVA: 0x004BCA54 File Offset: 0x004BAC54
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

		// Token: 0x04009EA2 RID: 40610
		[PublicizedFrom(EAccessModifier.Private)]
		public string name;

		// Token: 0x04009EA3 RID: 40611
		[PublicizedFrom(EAccessModifier.Private)]
		public AudioSource nearSource;

		// Token: 0x04009EA4 RID: 40612
		[PublicizedFrom(EAccessModifier.Private)]
		public AudioSource farSource;

		// Token: 0x04009EA5 RID: 40613
		[PublicizedFrom(EAccessModifier.Private)]
		public float basePitch;

		// Token: 0x04009EA6 RID: 40614
		[PublicizedFrom(EAccessModifier.Private)]
		public float baseVolume;
	}
}
