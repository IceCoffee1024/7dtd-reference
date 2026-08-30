using System;
using MusicUtils.Enums;

namespace DynamicMusic
{
	// Token: 0x02001A5D RID: 6749
	public interface ISectionSelector : INotifiable<MusicActionType>, ISelector<SectionType>
	{
	}
}
