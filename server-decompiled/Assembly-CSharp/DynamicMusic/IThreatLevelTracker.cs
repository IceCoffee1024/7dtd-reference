using System;

namespace DynamicMusic
{
	// Token: 0x02001AB3 RID: 6835
	public interface IThreatLevelTracker : ICleanable
	{
		// Token: 0x170019CD RID: 6605
		// (get) Token: 0x0600CE5D RID: 52829
		IThreatLevel ThreatLevel { get; }

		// Token: 0x0600CE5E RID: 52830
		void Update(EntityPlayerLocal _player);
	}
}
