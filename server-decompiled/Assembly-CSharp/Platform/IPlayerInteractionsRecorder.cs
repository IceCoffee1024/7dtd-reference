using System;
using System.Collections.Generic;

namespace Platform
{
	// Token: 0x02001BF3 RID: 7155
	public interface IPlayerInteractionsRecorder
	{
		// Token: 0x0600D57E RID: 54654
		void Init(IPlatform owner);

		// Token: 0x0600D57F RID: 54655
		void RecordPlayerInteraction(PlayerInteraction interaction);

		// Token: 0x0600D580 RID: 54656
		void RecordPlayerInteractions(IEnumerable<PlayerInteraction> interactions);

		// Token: 0x0600D581 RID: 54657
		void Destroy();
	}
}
