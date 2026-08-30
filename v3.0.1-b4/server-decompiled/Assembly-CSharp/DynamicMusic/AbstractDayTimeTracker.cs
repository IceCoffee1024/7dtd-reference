using System;
using MusicUtils.Enums;

namespace DynamicMusic
{
	// Token: 0x020019FC RID: 6652
	public abstract class AbstractDayTimeTracker : AbstractFilter, IFilter<SectionType>
	{
		// Token: 0x0600C99D RID: 51613
		[PublicizedFrom(EAccessModifier.Protected)]
		public abstract int GetCurrentDay();

		// Token: 0x0600C99E RID: 51614
		[PublicizedFrom(EAccessModifier.Protected)]
		public abstract float GetCurrentTime();

		// Token: 0x0600C99F RID: 51615 RVA: 0x0049F93F File Offset: 0x0049DB3F
		[PublicizedFrom(EAccessModifier.Protected)]
		public AbstractDayTimeTracker()
		{
		}

		// Token: 0x0400995B RID: 39259
		[PublicizedFrom(EAccessModifier.Protected)]
		public AbstractDayTimeTracker.DayPeriodType dayPeriod;

		// Token: 0x0400995C RID: 39260
		[PublicizedFrom(EAccessModifier.Protected)]
		public int currentDay;

		// Token: 0x0400995D RID: 39261
		[PublicizedFrom(EAccessModifier.Protected)]
		public float currentTime;

		// Token: 0x0400995E RID: 39262
		[PublicizedFrom(EAccessModifier.Protected)]
		public float dawnTime;

		// Token: 0x0400995F RID: 39263
		[PublicizedFrom(EAccessModifier.Protected)]
		public float duskTime;

		// Token: 0x020019FD RID: 6653
		[PublicizedFrom(EAccessModifier.Protected)]
		public enum DayPeriodType : byte
		{
			// Token: 0x04009961 RID: 39265
			Morning,
			// Token: 0x04009962 RID: 39266
			Day,
			// Token: 0x04009963 RID: 39267
			Night,
			// Token: 0x04009964 RID: 39268
			Dusk,
			// Token: 0x04009965 RID: 39269
			Dawn
		}
	}
}
