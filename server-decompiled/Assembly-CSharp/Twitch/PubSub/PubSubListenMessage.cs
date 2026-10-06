using System;

namespace Twitch.PubSub
{
	// Token: 0x020018B1 RID: 6321
	public class PubSubListenMessage : BasePubSubMessage
	{
		// Token: 0x0600C2F5 RID: 49909 RVA: 0x0047D9A4 File Offset: 0x0047BBA4
		public PubSubListenMessage()
		{
			base.type = "LISTEN";
		}

		// Token: 0x04009343 RID: 37699
		public PubSubListenMessage.PubSubListenData data;

		// Token: 0x020018B2 RID: 6322
		public class PubSubListenData
		{
			// Token: 0x17001869 RID: 6249
			// (get) Token: 0x0600C2F6 RID: 49910 RVA: 0x0047D9B7 File Offset: 0x0047BBB7
			// (set) Token: 0x0600C2F7 RID: 49911 RVA: 0x0047D9BF File Offset: 0x0047BBBF
			public string[] topics { get; set; }

			// Token: 0x1700186A RID: 6250
			// (get) Token: 0x0600C2F8 RID: 49912 RVA: 0x0047D9C8 File Offset: 0x0047BBC8
			// (set) Token: 0x0600C2F9 RID: 49913 RVA: 0x0047D9D0 File Offset: 0x0047BBD0
			public string auth_token { get; set; }
		}
	}
}
