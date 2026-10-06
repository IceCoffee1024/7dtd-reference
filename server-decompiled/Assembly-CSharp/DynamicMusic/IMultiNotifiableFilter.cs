using System;
using MusicUtils.Enums;

namespace DynamicMusic
{
	// Token: 0x02001AA3 RID: 6819
	public interface IMultiNotifiableFilter : INotifiable, INotifiableFilter<MusicActionType, SectionType>, INotifiable<MusicActionType>, IFilter<SectionType>
	{
	}
}
