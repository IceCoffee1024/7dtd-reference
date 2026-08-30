using System;

namespace DynamicMusic
{
	// Token: 0x02001A7F RID: 6783
	public interface IPlayable
	{
		// Token: 0x17001921 RID: 6433
		// (get) Token: 0x0600CCF1 RID: 52465
		// (set) Token: 0x0600CCF0 RID: 52464
		float Volume { get; set; }

		// Token: 0x17001922 RID: 6434
		// (get) Token: 0x0600CCF2 RID: 52466
		bool IsDone { get; }

		// Token: 0x17001923 RID: 6435
		// (get) Token: 0x0600CCF3 RID: 52467
		bool IsPaused { get; }

		// Token: 0x17001924 RID: 6436
		// (get) Token: 0x0600CCF4 RID: 52468
		bool IsPlaying { get; }

		// Token: 0x17001925 RID: 6437
		// (get) Token: 0x0600CCF5 RID: 52469
		bool IsReady { get; }

		// Token: 0x0600CCF6 RID: 52470
		void Init();

		// Token: 0x0600CCF7 RID: 52471
		void Play();

		// Token: 0x0600CCF8 RID: 52472
		void Pause();

		// Token: 0x0600CCF9 RID: 52473
		void UnPause();

		// Token: 0x0600CCFA RID: 52474
		void Stop();
	}
}
