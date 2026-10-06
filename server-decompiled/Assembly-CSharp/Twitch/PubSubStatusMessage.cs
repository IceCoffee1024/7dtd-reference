using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x02001810 RID: 6160
	[Serializable]
	public class PubSubStatusMessage
	{
		// Token: 0x04008ECF RID: 36559
		public string opaqueId;

		// Token: 0x04008ED0 RID: 36560
		public string displayName;

		// Token: 0x04008ED1 RID: 36561
		public string status;

		// Token: 0x04008ED2 RID: 36562
		public List<string> party;

		// Token: 0x04008ED3 RID: 36563
		public string commands;

		// Token: 0x04008ED4 RID: 36564
		public string language;

		// Token: 0x04008ED5 RID: 36565
		public List<string> actionTypes;
	}
}
