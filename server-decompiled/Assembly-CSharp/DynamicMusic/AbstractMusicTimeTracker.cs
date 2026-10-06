using System;
using MusicUtils.Enums;

namespace DynamicMusic
{
	// Token: 0x02001A52 RID: 6738
	public abstract class AbstractMusicTimeTracker : AbstractFilter, IFilter<SectionType>
	{
		// Token: 0x0600CC83 RID: 52355 RVA: 0x004AA1FF File Offset: 0x004A83FF
		[PublicizedFrom(EAccessModifier.Protected)]
		public AbstractMusicTimeTracker()
		{
		}

		// Token: 0x04009B5D RID: 39773
		[PublicizedFrom(EAccessModifier.Protected)]
		public float dailyAllottedPlayTime;

		// Token: 0x04009B5E RID: 39774
		[PublicizedFrom(EAccessModifier.Protected)]
		public float dailyPlayTimeUsed;

		// Token: 0x04009B5F RID: 39775
		[PublicizedFrom(EAccessModifier.Protected)]
		public float musicStartTime;

		// Token: 0x04009B60 RID: 39776
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool IsMusicPlaying;

		// Token: 0x04009B61 RID: 39777
		[PublicizedFrom(EAccessModifier.Protected)]
		public float pauseDuration;

		// Token: 0x04009B62 RID: 39778
		[PublicizedFrom(EAccessModifier.Protected)]
		public float pauseStartTime;
	}
}
