using System;
using MusicUtils.Enums;

namespace DynamicMusic
{
	// Token: 0x02001A5F RID: 6751
	public interface IThreatLevel
	{
		// Token: 0x170018B7 RID: 6327
		// (get) Token: 0x0600CB79 RID: 52089
		ThreatLevelType Category { get; }

		// Token: 0x170018B8 RID: 6328
		// (get) Token: 0x0600CB7A RID: 52090
		// (set) Token: 0x0600CB7B RID: 52091
		float Numeric { get; set; }
	}
}
