using System;
using System.Collections.Generic;

namespace Platform
{
	// Token: 0x02001B69 RID: 7017
	public interface IPlayerInteractionsRecorder
	{
		// Token: 0x0600D167 RID: 53607
		void Init(IPlatform owner);

		// Token: 0x0600D168 RID: 53608
		void RecordPlayerInteraction(PlayerInteraction interaction);

		// Token: 0x0600D169 RID: 53609
		void RecordPlayerInteractions(IEnumerable<PlayerInteraction> interactions);

		// Token: 0x0600D16A RID: 53610
		void Destroy();
	}
}
