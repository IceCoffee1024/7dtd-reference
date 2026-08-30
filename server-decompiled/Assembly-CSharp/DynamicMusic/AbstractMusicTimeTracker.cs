using System;
using MusicUtils.Enums;

namespace DynamicMusic
{
	// Token: 0x020019FF RID: 6655
	public abstract class AbstractMusicTimeTracker : AbstractFilter, IFilter<SectionType>
	{
		// Token: 0x0600C9A2 RID: 51618 RVA: 0x0049F93F File Offset: 0x0049DB3F
		[PublicizedFrom(EAccessModifier.Protected)]
		public AbstractMusicTimeTracker()
		{
		}

		// Token: 0x04009966 RID: 39270
		[PublicizedFrom(EAccessModifier.Protected)]
		public float dailyAllottedPlayTime;

		// Token: 0x04009967 RID: 39271
		[PublicizedFrom(EAccessModifier.Protected)]
		public float dailyPlayTimeUsed;

		// Token: 0x04009968 RID: 39272
		[PublicizedFrom(EAccessModifier.Protected)]
		public float musicStartTime;

		// Token: 0x04009969 RID: 39273
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool IsMusicPlaying;

		// Token: 0x0400996A RID: 39274
		[PublicizedFrom(EAccessModifier.Protected)]
		public float pauseDuration;

		// Token: 0x0400996B RID: 39275
		[PublicizedFrom(EAccessModifier.Protected)]
		public float pauseStartTime;
	}
}
