using System;

namespace DynamicMusic
{
	// Token: 0x02001AAB RID: 6827
	public interface IPlayable
	{
		// Token: 0x170019C3 RID: 6595
		// (get) Token: 0x0600CE49 RID: 52809
		// (set) Token: 0x0600CE48 RID: 52808
		float Volume { get; set; }

		// Token: 0x170019C4 RID: 6596
		// (get) Token: 0x0600CE4A RID: 52810
		bool IsDone { get; }

		// Token: 0x170019C5 RID: 6597
		// (get) Token: 0x0600CE4B RID: 52811
		bool IsPaused { get; }

		// Token: 0x170019C6 RID: 6598
		// (get) Token: 0x0600CE4C RID: 52812
		bool IsPlaying { get; }

		// Token: 0x170019C7 RID: 6599
		// (get) Token: 0x0600CE4D RID: 52813
		bool IsReady { get; }

		// Token: 0x0600CE4E RID: 52814
		void Init();

		// Token: 0x0600CE4F RID: 52815
		void Play();

		// Token: 0x0600CE50 RID: 52816
		void Pause();

		// Token: 0x0600CE51 RID: 52817
		void UnPause();

		// Token: 0x0600CE52 RID: 52818
		void Stop();
	}
}
