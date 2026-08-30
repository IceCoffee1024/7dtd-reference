using System;
using System.Collections.Generic;
using DynamicMusic.Factories;
using MusicUtils.Enums;
using UniLinq;

namespace DynamicMusic
{
	// Token: 0x02001A69 RID: 6761
	public class SectionSelector : ISectionSelector, INotifiable<MusicActionType>, ISelector<SectionType>
	{
		// Token: 0x0600CCC8 RID: 52424 RVA: 0x004AA70D File Offset: 0x004A890D
		public SectionSelector()
		{
			this.PlayerTracker = Factory.CreatePlayerTracker();
			this.DayTimeTracker = Factory.CreateDayTimeTracker();
		}

		// Token: 0x0600CCC9 RID: 52425 RVA: 0x004AA740 File Offset: 0x004A8940
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

		// Token: 0x0600CCCA RID: 52426 RVA: 0x004AA804 File Offset: 0x004A8A04
		public void Notify(MusicActionType _state)
		{
			this.DayTimeTracker.Notify(_state);
		}

		// Token: 0x04009B68 RID: 39784
		public static bool IsDMSTempDisabled;

		// Token: 0x04009B69 RID: 39785
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly List<SectionType> sectionTypeEnumValues = Enum.GetValues(typeof(SectionType)).Cast<SectionType>().ToList<SectionType>();

		// Token: 0x04009B6A RID: 39786
		[PublicizedFrom(EAccessModifier.Private)]
		public IFilter<SectionType> PlayerTracker;

		// Token: 0x04009B6B RID: 39787
		[PublicizedFrom(EAccessModifier.Private)]
		public INotifiableFilter<MusicActionType, SectionType> DayTimeTracker;

		// Token: 0x04009B6C RID: 39788
		[PublicizedFrom(EAccessModifier.Private)]
		public List<SectionType> sectionTypesBuffer = new List<SectionType>(SectionSelector.sectionTypeEnumValues.Count);
	}
}
