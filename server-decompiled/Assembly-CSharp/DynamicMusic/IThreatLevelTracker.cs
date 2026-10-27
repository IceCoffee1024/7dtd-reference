using System;

namespace DynamicMusic
{
	// Token: 0x02001AE4 RID: 6884
	public interface IThreatLevelTracker : ICleanable
	{
		// Token: 0x17001A15 RID: 6677
		// (get) Token: 0x0600CF71 RID: 53105
		IThreatLevel ThreatLevel { get; }

		// Token: 0x0600CF72 RID: 53106
		void Update(EntityPlayerLocal _player);
	}
}
