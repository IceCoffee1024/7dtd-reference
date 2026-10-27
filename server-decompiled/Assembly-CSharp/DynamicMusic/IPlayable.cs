using System;

namespace DynamicMusic
{
	// Token: 0x02001ADC RID: 6876
	public interface IPlayable
	{
		// Token: 0x17001A0B RID: 6667
		// (get) Token: 0x0600CF5D RID: 53085
		// (set) Token: 0x0600CF5C RID: 53084
		float Volume { get; set; }

		// Token: 0x17001A0C RID: 6668
		// (get) Token: 0x0600CF5E RID: 53086
		bool IsDone { get; }

		// Token: 0x17001A0D RID: 6669
		// (get) Token: 0x0600CF5F RID: 53087
		bool IsPaused { get; }

		// Token: 0x17001A0E RID: 6670
		// (get) Token: 0x0600CF60 RID: 53088
		bool IsPlaying { get; }

		// Token: 0x17001A0F RID: 6671
		// (get) Token: 0x0600CF61 RID: 53089
		bool IsReady { get; }

		// Token: 0x0600CF62 RID: 53090
		void Init();

		// Token: 0x0600CF63 RID: 53091
		void Play();

		// Token: 0x0600CF64 RID: 53092
		void Pause();

		// Token: 0x0600CF65 RID: 53093
		void UnPause();

		// Token: 0x0600CF66 RID: 53094
		void Stop();
	}
}
