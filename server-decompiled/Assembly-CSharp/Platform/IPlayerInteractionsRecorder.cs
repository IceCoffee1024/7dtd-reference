using System;
using System.Collections.Generic;

namespace Platform
{
	// Token: 0x02001B90 RID: 7056
	public interface IPlayerInteractionsRecorder
	{
		// Token: 0x0600D2F2 RID: 54002
		void Init(IPlatform owner);

		// Token: 0x0600D2F3 RID: 54003
		void RecordPlayerInteraction(PlayerInteraction interaction);

		// Token: 0x0600D2F4 RID: 54004
		void RecordPlayerInteractions(IEnumerable<PlayerInteraction> interactions);

		// Token: 0x0600D2F5 RID: 54005
		void Destroy();
	}
}
