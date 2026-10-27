using System;
using MusicUtils.Enums;

namespace DynamicMusic
{
	// Token: 0x02001A80 RID: 6784
	public abstract class AbstractDayTimeTracker : AbstractFilter, IFilter<SectionType>
	{
		// Token: 0x0600CD92 RID: 52626
		[PublicizedFrom(EAccessModifier.Protected)]
		public abstract int GetCurrentDay();

		// Token: 0x0600CD93 RID: 52627
		[PublicizedFrom(EAccessModifier.Protected)]
		public abstract float GetCurrentTime();

		// Token: 0x0600CD94 RID: 52628 RVA: 0x004ADCFB File Offset: 0x004ABEFB
		[PublicizedFrom(EAccessModifier.Protected)]
		public AbstractDayTimeTracker()
		{
		}

		// Token: 0x04009C3B RID: 39995
		[PublicizedFrom(EAccessModifier.Protected)]
		public AbstractDayTimeTracker.DayPeriodType dayPeriod;

		// Token: 0x04009C3C RID: 39996
		[PublicizedFrom(EAccessModifier.Protected)]
		public int currentDay;

		// Token: 0x04009C3D RID: 39997
		[PublicizedFrom(EAccessModifier.Protected)]
		public float currentTime;

		// Token: 0x04009C3E RID: 39998
		[PublicizedFrom(EAccessModifier.Protected)]
		public float dawnTime;

		// Token: 0x04009C3F RID: 39999
		[PublicizedFrom(EAccessModifier.Protected)]
		public float duskTime;

		// Token: 0x02001A81 RID: 6785
		[PublicizedFrom(EAccessModifier.Protected)]
		public enum DayPeriodType : byte
		{
			// Token: 0x04009C41 RID: 40001
			Morning,
			// Token: 0x04009C42 RID: 40002
			Day,
			// Token: 0x04009C43 RID: 40003
			Night,
			// Token: 0x04009C44 RID: 40004
			Dusk,
			// Token: 0x04009C45 RID: 40005
			Dawn
		}
	}
}
