using System;
using System.Collections.Generic;
using DynamicMusic.Factories;
using MusicUtils.Enums;
using UniLinq;

namespace DynamicMusic
{
	// Token: 0x02001A95 RID: 6805
	public class SectionSelector : ISectionSelector, INotifiable<MusicActionType>, ISelector<SectionType>
	{
		// Token: 0x0600CE20 RID: 52768 RVA: 0x004AEBD9 File Offset: 0x004ACDD9
		public SectionSelector()
		{
			this.PlayerTracker = Factory.CreatePlayerTracker();
			this.DayTimeTracker = Factory.CreateDayTimeTracker();
		}

		// Token: 0x0600CE21 RID: 52769 RVA: 0x004AEC0C File Offset: 0x004ACE0C
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

		// Token: 0x0600CE22 RID: 52770 RVA: 0x004AECD0 File Offset: 0x004ACED0
		public void Notify(MusicActionType _state)
		{
			this.DayTimeTracker.Notify(_state);
		}

		// Token: 0x04009C2E RID: 39982
		public static bool IsDMSTempDisabled;

		// Token: 0x04009C2F RID: 39983
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly List<SectionType> sectionTypeEnumValues = Enum.GetValues(typeof(SectionType)).Cast<SectionType>().ToList<SectionType>();

		// Token: 0x04009C30 RID: 39984
		[PublicizedFrom(EAccessModifier.Private)]
		public IFilter<SectionType> PlayerTracker;

		// Token: 0x04009C31 RID: 39985
		[PublicizedFrom(EAccessModifier.Private)]
		public INotifiableFilter<MusicActionType, SectionType> DayTimeTracker;

		// Token: 0x04009C32 RID: 39986
		[PublicizedFrom(EAccessModifier.Private)]
		public List<SectionType> sectionTypesBuffer = new List<SectionType>(SectionSelector.sectionTypeEnumValues.Count);
	}
}
