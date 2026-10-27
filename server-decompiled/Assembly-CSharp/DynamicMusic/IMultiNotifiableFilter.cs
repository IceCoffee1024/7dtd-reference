using System;
using MusicUtils.Enums;

namespace DynamicMusic
{
	// Token: 0x02001AD4 RID: 6868
	public interface IMultiNotifiableFilter : INotifiable, INotifiableFilter<MusicActionType, SectionType>, INotifiable<MusicActionType>, IFilter<SectionType>
	{
	}
}
