using System;
using MusicUtils.Enums;

namespace DynamicMusic
{
	// Token: 0x02001A86 RID: 6790
	public interface IThreatLevel
	{
		// Token: 0x17001929 RID: 6441
		// (get) Token: 0x0600CD02 RID: 52482
		ThreatLevelType Category { get; }

		// Token: 0x1700192A RID: 6442
		// (get) Token: 0x0600CD03 RID: 52483
		// (set) Token: 0x0600CD04 RID: 52484
		float Numeric { get; set; }
	}
}
