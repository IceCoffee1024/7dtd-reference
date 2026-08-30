using System;
using MusicUtils.Enums;

namespace DynamicMusic
{
	// Token: 0x02001A5B RID: 6747
	public interface ISection : IPlayable, IFadeable, ICleanable
	{
		// Token: 0x170018B5 RID: 6325
		// (get) Token: 0x0600CB74 RID: 52084
		bool IsInitialized { get; }

		// Token: 0x170018B6 RID: 6326
		// (get) Token: 0x0600CB75 RID: 52085
		// (set) Token: 0x0600CB76 RID: 52086
		SectionType Sect { get; set; }
	}
}
