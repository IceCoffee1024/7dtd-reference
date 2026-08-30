using System;

namespace DynamicMusic
{
	// Token: 0x02001A60 RID: 6752
	public interface IThreatLevelTracker : ICleanable
	{
		// Token: 0x170018B9 RID: 6329
		// (get) Token: 0x0600CB7C RID: 52092
		IThreatLevel ThreatLevel { get; }

		// Token: 0x0600CB7D RID: 52093
		void Update(EntityPlayerLocal _player);
	}
}
