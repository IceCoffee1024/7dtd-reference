using System;
using System.Collections.Generic;
using DynamicMusic.Factories;
using MusicUtils.Enums;
using UniLinq;

namespace DynamicMusic
{
	// Token: 0x02001A42 RID: 6722
	public class SectionSelector : ISectionSelector, INotifiable<MusicActionType>, ISelector<SectionType>
	{
		// Token: 0x0600CB3F RID: 52031 RVA: 0x004A4319 File Offset: 0x004A2519
		public SectionSelector()
		{
			this.PlayerTracker = Factory.CreatePlayerTracker();
			this.DayTimeTracker = Factory.CreateDayTimeTracker();
		}

		// Token: 0x0600CB40 RID: 52032 RVA: 0x004A434C File Offset: 0x004A254C
		public SectionType Select()
		{
			this.sectionTypesBuffer.Clear();
			this.sectionTypesBuffer.AddRange(SectionSelector.sectionTypeEnumValues);
			if (SectionSelector.IsDMSTempDisabled || GamePrefs.GetFloat(EnumGamePrefs.OptionsMusicVolumeLevel) == 0f || !GamePrefs.GetBool(EnumGamePrefs.OptionsDynamicMusicEnabled))
			{
				this.sectionTypesBuffer.Clear();
				this.sectionTypesBuffer.Add(SectionType.None);
			}
			else
			{
				this.sectionTypesBuffer = this.PlayerTracker.Filter(this.sectionTypesBuffer);
			}
			if (this.sectionTypesBuffer.Count > 1)
			{
				this.sectionTypesBuffer = this.DayTimeTracker.Filter(this.sectionTypesBuffer);
			}
			if (this.sectionTypesBuffer.Count == 2)
			{
				this.sectionTypesBuffer.Remove(SectionType.None);
			}
			return this.sectionTypesBuffer[0];
		}

		// Token: 0x0600CB41 RID: 52033 RVA: 0x004A4410 File Offset: 0x004A2610
		public void Notify(MusicActionType _state)
		{
			this.DayTimeTracker.Notify(_state);
		}

		// Token: 0x04009A37 RID: 39479
		public static bool IsDMSTempDisabled;

		// Token: 0x04009A38 RID: 39480
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly List<SectionType> sectionTypeEnumValues = Enum.GetValues(typeof(SectionType)).Cast<SectionType>().ToList<SectionType>();

		// Token: 0x04009A39 RID: 39481
		[PublicizedFrom(EAccessModifier.Private)]
		public IFilter<SectionType> PlayerTracker;

		// Token: 0x04009A3A RID: 39482
		[PublicizedFrom(EAccessModifier.Private)]
		public INotifiableFilter<MusicActionType, SectionType> DayTimeTracker;

		// Token: 0x04009A3B RID: 39483
		[PublicizedFrom(EAccessModifier.Private)]
		public List<SectionType> sectionTypesBuffer = new List<SectionType>(SectionSelector.sectionTypeEnumValues.Count);
	}
}
