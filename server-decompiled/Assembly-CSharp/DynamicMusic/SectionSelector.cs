using System;
using System.Collections.Generic;
using DynamicMusic.Factories;
using MusicUtils.Enums;
using UniLinq;

namespace DynamicMusic
{
	// Token: 0x02001AC6 RID: 6854
	public class SectionSelector : ISectionSelector, INotifiable<MusicActionType>, ISelector<SectionType>
	{
		// Token: 0x0600CF34 RID: 53044 RVA: 0x004B26D5 File Offset: 0x004B08D5
		public SectionSelector()
		{
			this.PlayerTracker = Factory.CreatePlayerTracker();
			this.DayTimeTracker = Factory.CreateDayTimeTracker();
		}

		// Token: 0x0600CF35 RID: 53045 RVA: 0x004B2708 File Offset: 0x004B0908
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

		// Token: 0x0600CF36 RID: 53046 RVA: 0x004B27CC File Offset: 0x004B09CC
		public void Notify(MusicActionType _state)
		{
			this.DayTimeTracker.Notify(_state);
		}

		// Token: 0x04009D17 RID: 40215
		public static bool IsDMSTempDisabled;

		// Token: 0x04009D18 RID: 40216
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly List<SectionType> sectionTypeEnumValues = Enum.GetValues(typeof(SectionType)).Cast<SectionType>().ToList<SectionType>();

		// Token: 0x04009D19 RID: 40217
		[PublicizedFrom(EAccessModifier.Private)]
		public IFilter<SectionType> PlayerTracker;

		// Token: 0x04009D1A RID: 40218
		[PublicizedFrom(EAccessModifier.Private)]
		public INotifiableFilter<MusicActionType, SectionType> DayTimeTracker;

		// Token: 0x04009D1B RID: 40219
		[PublicizedFrom(EAccessModifier.Private)]
		public List<SectionType> sectionTypesBuffer = new List<SectionType>(SectionSelector.sectionTypeEnumValues.Count);
	}
}
