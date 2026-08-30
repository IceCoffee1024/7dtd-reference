using System;

namespace Twitch.PubSub
{
	// Token: 0x0200185F RID: 6239
	public class PubSubListenMessage : BasePubSubMessage
	{
		// Token: 0x0600C018 RID: 49176 RVA: 0x004736CC File Offset: 0x004718CC
		public PubSubListenMessage()
		{
			base.type = "LISTEN";
		}

		// Token: 0x04009161 RID: 37217
		public PubSubListenMessage.PubSubListenData data;

		// Token: 0x02001860 RID: 6240
		public class PubSubListenData
		{
			// Token: 0x17001755 RID: 5973
			// (get) Token: 0x0600C019 RID: 49177 RVA: 0x004736DF File Offset: 0x004718DF
			// (set) Token: 0x0600C01A RID: 49178 RVA: 0x004736E7 File Offset: 0x004718E7
			public string[] topics { get; set; }

			// Token: 0x17001756 RID: 5974
			// (get) Token: 0x0600C01B RID: 49179 RVA: 0x004736F0 File Offset: 0x004718F0
			// (set) Token: 0x0600C01C RID: 49180 RVA: 0x004736F8 File Offset: 0x004718F8
			public string auth_token { get; set; }
		}
	}
}
