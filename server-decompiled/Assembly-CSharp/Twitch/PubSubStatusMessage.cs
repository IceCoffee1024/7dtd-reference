using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x02001841 RID: 6209
	[Serializable]
	public class PubSubStatusMessage
	{
		// Token: 0x04008FB8 RID: 36792
		public string opaqueId;

		// Token: 0x04008FB9 RID: 36793
		public string displayName;

		// Token: 0x04008FBA RID: 36794
		public string status;

		// Token: 0x04008FBB RID: 36795
		public List<string> party;

		// Token: 0x04008FBC RID: 36796
		public string commands;

		// Token: 0x04008FBD RID: 36797
		public string language;

		// Token: 0x04008FBE RID: 36798
		public List<string> actionTypes;
	}
}
