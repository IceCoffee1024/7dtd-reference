using System;

namespace Twitch.PubSub
{
	// Token: 0x020018E2 RID: 6370
	public class PubSubListenMessage : BasePubSubMessage
	{
		// Token: 0x0600C409 RID: 50185 RVA: 0x00481350 File Offset: 0x0047F550
		public PubSubListenMessage()
		{
			base.type = "LISTEN";
		}

		// Token: 0x0400942C RID: 37932
		public PubSubListenMessage.PubSubListenData data;

		// Token: 0x020018E3 RID: 6371
		public class PubSubListenData
		{
			// Token: 0x170018B1 RID: 6321
			// (get) Token: 0x0600C40A RID: 50186 RVA: 0x00481363 File Offset: 0x0047F563
			// (set) Token: 0x0600C40B RID: 50187 RVA: 0x0048136B File Offset: 0x0047F56B
			public string[] topics { get; set; }

			// Token: 0x170018B2 RID: 6322
			// (get) Token: 0x0600C40C RID: 50188 RVA: 0x00481374 File Offset: 0x0047F574
			// (set) Token: 0x0600C40D RID: 50189 RVA: 0x0048137C File Offset: 0x0047F57C
			public string auth_token { get; set; }
		}
	}
}
