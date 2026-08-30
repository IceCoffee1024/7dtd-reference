using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x020017E4 RID: 6116
	[Serializable]
	public class PubSubStatusMessage
	{
		// Token: 0x04008E09 RID: 36361
		public string opaqueId;

		// Token: 0x04008E0A RID: 36362
		public string displayName;

		// Token: 0x04008E0B RID: 36363
		public string status;

		// Token: 0x04008E0C RID: 36364
		public List<string> party;

		// Token: 0x04008E0D RID: 36365
		public string commands;

		// Token: 0x04008E0E RID: 36366
		public string language;

		// Token: 0x04008E0F RID: 36367
		public List<string> actionTypes;
	}
}
