using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x020017BE RID: 6078
	[Serializable]
	public class PubSubStatusMessage
	{
		// Token: 0x04008CED RID: 36077
		public string opaqueId;

		// Token: 0x04008CEE RID: 36078
		public string displayName;

		// Token: 0x04008CEF RID: 36079
		public string status;

		// Token: 0x04008CF0 RID: 36080
		public List<string> party;

		// Token: 0x04008CF1 RID: 36081
		public string commands;

		// Token: 0x04008CF2 RID: 36082
		public string language;

		// Token: 0x04008CF3 RID: 36083
		public List<string> actionTypes;
	}
}
