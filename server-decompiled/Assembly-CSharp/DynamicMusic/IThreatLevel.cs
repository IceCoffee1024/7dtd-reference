using System;
using MusicUtils.Enums;

namespace DynamicMusic
{
	// Token: 0x02001AE3 RID: 6883
	public interface IThreatLevel
	{
		// Token: 0x17001A13 RID: 6675
		// (get) Token: 0x0600CF6E RID: 53102
		ThreatLevelType Category { get; }

		// Token: 0x17001A14 RID: 6676
		// (get) Token: 0x0600CF6F RID: 53103
		// (set) Token: 0x0600CF70 RID: 53104
		float Numeric { get; set; }
	}
}
