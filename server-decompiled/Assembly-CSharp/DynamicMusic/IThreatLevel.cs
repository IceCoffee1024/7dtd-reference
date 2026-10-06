using System;
using MusicUtils.Enums;

namespace DynamicMusic
{
	// Token: 0x02001AB2 RID: 6834
	public interface IThreatLevel
	{
		// Token: 0x170019CB RID: 6603
		// (get) Token: 0x0600CE5A RID: 52826
		ThreatLevelType Category { get; }

		// Token: 0x170019CC RID: 6604
		// (get) Token: 0x0600CE5B RID: 52827
		// (set) Token: 0x0600CE5C RID: 52828
		float Numeric { get; set; }
	}
}
