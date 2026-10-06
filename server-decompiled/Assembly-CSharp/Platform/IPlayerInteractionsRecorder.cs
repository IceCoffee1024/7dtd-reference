using System;
using System.Collections.Generic;

namespace Platform
{
	// Token: 0x02001BC2 RID: 7106
	public interface IPlayerInteractionsRecorder
	{
		// Token: 0x0600D46A RID: 54378
		void Init(IPlatform owner);

		// Token: 0x0600D46B RID: 54379
		void RecordPlayerInteraction(PlayerInteraction interaction);

		// Token: 0x0600D46C RID: 54380
		void RecordPlayerInteractions(IEnumerable<PlayerInteraction> interactions);

		// Token: 0x0600D46D RID: 54381
		void Destroy();
	}
}
