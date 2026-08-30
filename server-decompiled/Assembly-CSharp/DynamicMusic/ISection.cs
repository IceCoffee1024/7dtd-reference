using System;
using MusicUtils.Enums;

namespace DynamicMusic
{
	// Token: 0x02001A82 RID: 6786
	public interface ISection : IPlayable, IFadeable, ICleanable
	{
		// Token: 0x17001927 RID: 6439
		// (get) Token: 0x0600CCFD RID: 52477
		bool IsInitialized { get; }

		// Token: 0x17001928 RID: 6440
		// (get) Token: 0x0600CCFE RID: 52478
		// (set) Token: 0x0600CCFF RID: 52479
		SectionType Sect { get; set; }
	}
}
