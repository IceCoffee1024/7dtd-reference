using System;
using MusicUtils.Enums;

namespace DynamicMusic
{
	// Token: 0x02001A84 RID: 6788
	public interface ISectionSelector : INotifiable<MusicActionType>, ISelector<SectionType>
	{
	}
}
