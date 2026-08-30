using System;
using MusicUtils.Enums;

namespace DynamicMusic
{
	// Token: 0x02001A26 RID: 6694
	public abstract class AbstractMusicTimeTracker : AbstractFilter, IFilter<SectionType>
	{
		// Token: 0x0600CB2B RID: 52011 RVA: 0x004A5D33 File Offset: 0x004A3F33
		[PublicizedFrom(EAccessModifier.Protected)]
		public AbstractMusicTimeTracker()
		{
		}

		// Token: 0x04009A97 RID: 39575
		[PublicizedFrom(EAccessModifier.Protected)]
		public float dailyAllottedPlayTime;

		// Token: 0x04009A98 RID: 39576
		[PublicizedFrom(EAccessModifier.Protected)]
		public float dailyPlayTimeUsed;

		// Token: 0x04009A99 RID: 39577
		[PublicizedFrom(EAccessModifier.Protected)]
		public float musicStartTime;

		// Token: 0x04009A9A RID: 39578
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool IsMusicPlaying;

		// Token: 0x04009A9B RID: 39579
		[PublicizedFrom(EAccessModifier.Protected)]
		public float pauseDuration;

		// Token: 0x04009A9C RID: 39580
		[PublicizedFrom(EAccessModifier.Protected)]
		public float pauseStartTime;
	}
}
