using System;
using MusicUtils.Enums;

namespace DynamicMusic
{
	// Token: 0x02001A50 RID: 6736
	public interface IMultiNotifiableFilter : INotifiable, INotifiableFilter<MusicActionType, SectionType>, INotifiable<MusicActionType>, IFilter<SectionType>
	{
	}
}
