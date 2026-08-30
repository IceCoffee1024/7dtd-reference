using System;

namespace DynamicMusic
{
	// Token: 0x02001A58 RID: 6744
	public interface IPlayable
	{
		// Token: 0x170018AF RID: 6319
		// (get) Token: 0x0600CB68 RID: 52072
		// (set) Token: 0x0600CB67 RID: 52071
		float Volume { get; set; }

		// Token: 0x170018B0 RID: 6320
		// (get) Token: 0x0600CB69 RID: 52073
		bool IsDone { get; }

		// Token: 0x170018B1 RID: 6321
		// (get) Token: 0x0600CB6A RID: 52074
		bool IsPaused { get; }

		// Token: 0x170018B2 RID: 6322
		// (get) Token: 0x0600CB6B RID: 52075
		bool IsPlaying { get; }

		// Token: 0x170018B3 RID: 6323
		// (get) Token: 0x0600CB6C RID: 52076
		bool IsReady { get; }

		// Token: 0x0600CB6D RID: 52077
		void Init();

		// Token: 0x0600CB6E RID: 52078
		void Play();

		// Token: 0x0600CB6F RID: 52079
		void Pause();

		// Token: 0x0600CB70 RID: 52080
		void UnPause();

		// Token: 0x0600CB71 RID: 52081
		void Stop();
	}
}
