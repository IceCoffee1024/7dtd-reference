using System;
using MusicUtils.Enums;

namespace DynamicMusic
{
	// Token: 0x02001A83 RID: 6787
	public abstract class AbstractMusicTimeTracker : AbstractFilter, IFilter<SectionType>
	{
		// Token: 0x0600CD97 RID: 52631 RVA: 0x004ADCFB File Offset: 0x004ABEFB
		[PublicizedFrom(EAccessModifier.Protected)]
		public AbstractMusicTimeTracker()
		{
		}

		// Token: 0x04009C46 RID: 40006
		[PublicizedFrom(EAccessModifier.Protected)]
		public float dailyAllottedPlayTime;

		// Token: 0x04009C47 RID: 40007
		[PublicizedFrom(EAccessModifier.Protected)]
		public float dailyPlayTimeUsed;

		// Token: 0x04009C48 RID: 40008
		[PublicizedFrom(EAccessModifier.Protected)]
		public float musicStartTime;

		// Token: 0x04009C49 RID: 40009
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool IsMusicPlaying;

		// Token: 0x04009C4A RID: 40010
		[PublicizedFrom(EAccessModifier.Protected)]
		public float pauseDuration;

		// Token: 0x04009C4B RID: 40011
		[PublicizedFrom(EAccessModifier.Protected)]
		public float pauseStartTime;
	}
}
