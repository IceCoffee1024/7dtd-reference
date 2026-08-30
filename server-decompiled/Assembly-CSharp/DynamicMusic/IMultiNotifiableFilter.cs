using System;
using MusicUtils.Enums;

namespace DynamicMusic
{
	// Token: 0x02001A77 RID: 6775
	public interface IMultiNotifiableFilter : INotifiable, INotifiableFilter<MusicActionType, SectionType>, INotifiable<MusicActionType>, IFilter<SectionType>
	{
	}
}
