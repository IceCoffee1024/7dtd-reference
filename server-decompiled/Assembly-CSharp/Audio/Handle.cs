using System;
using UnityEngine;

namespace Audio
{
	// Token: 0x02001B47 RID: 6983
	public class Handle
	{
		// Token: 0x0600D18E RID: 53646 RVA: 0x004C0DCC File Offset: 0x004BEFCC
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

		// Token: 0x0600D18F RID: 53647 RVA: 0x004C0E24 File Offset: 0x004BF024
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

		// Token: 0x0600D190 RID: 53648 RVA: 0x004C0E74 File Offset: 0x004BF074
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

		// Token: 0x0600D191 RID: 53649 RVA: 0x004C0EC1 File Offset: 0x004BF0C1
		public void Stop(int entityId)
		{
			Manager.Stop(entityId, this.name);
		}

		// Token: 0x0600D192 RID: 53650 RVA: 0x004C0ED0 File Offset: 0x004BF0D0
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

		// Token: 0x0600D193 RID: 53651 RVA: 0x004C0F20 File Offset: 0x004BF120
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

		// Token: 0x04009F68 RID: 40808
		[PublicizedFrom(EAccessModifier.Private)]
		public string name;

		// Token: 0x04009F69 RID: 40809
		[PublicizedFrom(EAccessModifier.Private)]
		public AudioSource nearSource;

		// Token: 0x04009F6A RID: 40810
		[PublicizedFrom(EAccessModifier.Private)]
		public AudioSource farSource;

		// Token: 0x04009F6B RID: 40811
		[PublicizedFrom(EAccessModifier.Private)]
		public float basePitch;

		// Token: 0x04009F6C RID: 40812
		[PublicizedFrom(EAccessModifier.Private)]
		public float baseVolume;
	}
}
