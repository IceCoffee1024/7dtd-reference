using System;

namespace Twitch.PubSub
{
	// Token: 0x02001885 RID: 6277
	public class PubSubListenMessage : BasePubSubMessage
	{
		// Token: 0x0600C19D RID: 49565 RVA: 0x00479468 File Offset: 0x00477668
		public PubSubListenMessage()
		{
			base.type = "LISTEN";
		}

		// Token: 0x0400927D RID: 37501
		public PubSubListenMessage.PubSubListenData data;

		// Token: 0x02001886 RID: 6278
		public class PubSubListenData
		{
			// Token: 0x170017C7 RID: 6087
			// (get) Token: 0x0600C19E RID: 49566 RVA: 0x0047947B File Offset: 0x0047767B
			// (set) Token: 0x0600C19F RID: 49567 RVA: 0x00479483 File Offset: 0x00477683
			public string[] topics { get; set; }

			// Token: 0x170017C8 RID: 6088
			// (get) Token: 0x0600C1A0 RID: 49568 RVA: 0x0047948C File Offset: 0x0047768C
			// (set) Token: 0x0600C1A1 RID: 49569 RVA: 0x00479494 File Offset: 0x00477694
			public string auth_token { get; set; }
		}
	}
}
