using System;
using MusicUtils.Enums;

namespace DynamicMusic
{
	// Token: 0x02001A23 RID: 6691
	public abstract class AbstractDayTimeTracker : AbstractFilter, IFilter<SectionType>
	{
		// Token: 0x0600CB26 RID: 52006
		[PublicizedFrom(EAccessModifier.Protected)]
		public abstract int GetCurrentDay();

		// Token: 0x0600CB27 RID: 52007
		[PublicizedFrom(EAccessModifier.Protected)]
		public abstract float GetCurrentTime();

		// Token: 0x0600CB28 RID: 52008 RVA: 0x004A5D33 File Offset: 0x004A3F33
		[PublicizedFrom(EAccessModifier.Protected)]
		public AbstractDayTimeTracker()
		{
		}

		// Token: 0x04009A8C RID: 39564
		[PublicizedFrom(EAccessModifier.Protected)]
		public AbstractDayTimeTracker.DayPeriodType dayPeriod;

		// Token: 0x04009A8D RID: 39565
		[PublicizedFrom(EAccessModifier.Protected)]
		public int currentDay;

		// Token: 0x04009A8E RID: 39566
		[PublicizedFrom(EAccessModifier.Protected)]
		public float currentTime;

		// Token: 0x04009A8F RID: 39567
		[PublicizedFrom(EAccessModifier.Protected)]
		public float dawnTime;

		// Token: 0x04009A90 RID: 39568
		[PublicizedFrom(EAccessModifier.Protected)]
		public float duskTime;

		// Token: 0x02001A24 RID: 6692
		[PublicizedFrom(EAccessModifier.Protected)]
		public enum DayPeriodType : byte
		{
			// Token: 0x04009A92 RID: 39570
			Morning,
			// Token: 0x04009A93 RID: 39571
			Day,
			// Token: 0x04009A94 RID: 39572
			Night,
			// Token: 0x04009A95 RID: 39573
			Dusk,
			// Token: 0x04009A96 RID: 39574
			Dawn
		}
	}
}
