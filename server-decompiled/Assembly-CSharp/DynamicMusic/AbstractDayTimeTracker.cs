using System;
using MusicUtils.Enums;

namespace DynamicMusic
{
	// Token: 0x02001A4F RID: 6735
	public abstract class AbstractDayTimeTracker : AbstractFilter, IFilter<SectionType>
	{
		// Token: 0x0600CC7E RID: 52350
		[PublicizedFrom(EAccessModifier.Protected)]
		public abstract int GetCurrentDay();

		// Token: 0x0600CC7F RID: 52351
		[PublicizedFrom(EAccessModifier.Protected)]
		public abstract float GetCurrentTime();

		// Token: 0x0600CC80 RID: 52352 RVA: 0x004AA1FF File Offset: 0x004A83FF
		[PublicizedFrom(EAccessModifier.Protected)]
		public AbstractDayTimeTracker()
		{
		}

		// Token: 0x04009B52 RID: 39762
		[PublicizedFrom(EAccessModifier.Protected)]
		public AbstractDayTimeTracker.DayPeriodType dayPeriod;

		// Token: 0x04009B53 RID: 39763
		[PublicizedFrom(EAccessModifier.Protected)]
		public int currentDay;

		// Token: 0x04009B54 RID: 39764
		[PublicizedFrom(EAccessModifier.Protected)]
		public float currentTime;

		// Token: 0x04009B55 RID: 39765
		[PublicizedFrom(EAccessModifier.Protected)]
		public float dawnTime;

		// Token: 0x04009B56 RID: 39766
		[PublicizedFrom(EAccessModifier.Protected)]
		public float duskTime;

		// Token: 0x02001A50 RID: 6736
		[PublicizedFrom(EAccessModifier.Protected)]
		public enum DayPeriodType : byte
		{
			// Token: 0x04009B58 RID: 39768
			Morning,
			// Token: 0x04009B59 RID: 39769
			Day,
			// Token: 0x04009B5A RID: 39770
			Night,
			// Token: 0x04009B5B RID: 39771
			Dusk,
			// Token: 0x04009B5C RID: 39772
			Dawn
		}
	}
}
