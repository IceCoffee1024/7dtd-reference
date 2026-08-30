using System;

namespace DynamicMusic
{
	// Token: 0x02001A87 RID: 6791
	public interface IThreatLevelTracker : ICleanable
	{
		// Token: 0x1700192B RID: 6443
		// (get) Token: 0x0600CD05 RID: 52485
		IThreatLevel ThreatLevel { get; }

		// Token: 0x0600CD06 RID: 52486
		void Update(EntityPlayerLocal _player);
	}
}
