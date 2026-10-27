using System;
using MusicUtils.Enums;

namespace DynamicMusic
{
	// Token: 0x02001ADF RID: 6879
	public interface ISection : IPlayable, IFadeable, ICleanable
	{
		// Token: 0x17001A11 RID: 6673
		// (get) Token: 0x0600CF69 RID: 53097
		bool IsInitialized { get; }

		// Token: 0x17001A12 RID: 6674
		// (get) Token: 0x0600CF6A RID: 53098
		// (set) Token: 0x0600CF6B RID: 53099
		SectionType Sect { get; set; }
	}
}
