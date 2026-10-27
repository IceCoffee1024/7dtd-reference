using System;
using MusicUtils.Enums;

namespace DynamicMusic
{
	// Token: 0x02001AE1 RID: 6881
	public interface ISectionSelector : INotifiable<MusicActionType>, ISelector<SectionType>
	{
	}
}
