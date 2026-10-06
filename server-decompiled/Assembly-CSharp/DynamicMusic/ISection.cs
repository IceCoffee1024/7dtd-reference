using System;
using MusicUtils.Enums;

namespace DynamicMusic
{
	// Token: 0x02001AAE RID: 6830
	public interface ISection : IPlayable, IFadeable, ICleanable
	{
		// Token: 0x170019C9 RID: 6601
		// (get) Token: 0x0600CE55 RID: 52821
		bool IsInitialized { get; }

		// Token: 0x170019CA RID: 6602
		// (get) Token: 0x0600CE56 RID: 52822
		// (set) Token: 0x0600CE57 RID: 52823
		SectionType Sect { get; set; }
	}
}
