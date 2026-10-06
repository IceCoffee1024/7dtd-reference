using System;
using MusicUtils.Enums;

namespace DynamicMusic
{
	// Token: 0x02001AB0 RID: 6832
	public interface ISectionSelector : INotifiable<MusicActionType>, ISelector<SectionType>
	{
	}
}
