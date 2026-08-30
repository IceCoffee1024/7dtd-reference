using System;
using Newtonsoft.Json;

namespace Twitch.PubSub
{
	// Token: 0x02001855 RID: 6229
	public class PubSubChannelPointMessage : BasePubSubMessage
	{
		// Token: 0x0600BFEE RID: 49134 RVA: 0x004735AF File Offset: 0x004717AF
		public static PubSubChannelPointMessage Deserialize(string message)
		{
			return JsonConvert.DeserializeObject<PubSubChannelPointMessage>(message);
		}

		// Token: 0x04009151 RID: 37201
		public PubSubChannelPointMessage.ChannelRedemptionData data;

		// Token: 0x02001856 RID: 6230
		public class ChannelRedemptionData : EventArgs
		{
			// Token: 0x17001746 RID: 5958
			// (get) Token: 0x0600BFF0 RID: 49136 RVA: 0x004735B7 File Offset: 0x004717B7
			// (set) Token: 0x0600BFF1 RID: 49137 RVA: 0x004735BF File Offset: 0x004717BF
			public PubSubChannelPointMessage.Redemption redemption { get; set; }
		}

		// Token: 0x02001857 RID: 6231
		public class Redemption
		{
			// Token: 0x17001747 RID: 5959
			// (get) Token: 0x0600BFF3 RID: 49139 RVA: 0x004735C8 File Offset: 0x004717C8
			// (set) Token: 0x0600BFF4 RID: 49140 RVA: 0x004735D0 File Offset: 0x004717D0
			public PubSubChannelPointMessage.User user { get; set; }

			// Token: 0x17001748 RID: 5960
			// (get) Token: 0x0600BFF5 RID: 49141 RVA: 0x004735D9 File Offset: 0x004717D9
			// (set) Token: 0x0600BFF6 RID: 49142 RVA: 0x004735E1 File Offset: 0x004717E1
			public PubSubChannelPointMessage.Reward reward { get; set; }
		}

		// Token: 0x02001858 RID: 6232
		public class User
		{
			// Token: 0x17001749 RID: 5961
			// (get) Token: 0x0600BFF8 RID: 49144 RVA: 0x004735EA File Offset: 0x004717EA
			// (set) Token: 0x0600BFF9 RID: 49145 RVA: 0x004735F2 File Offset: 0x004717F2
			public string login { get; set; }

			// Token: 0x1700174A RID: 5962
			// (get) Token: 0x0600BFFA RID: 49146 RVA: 0x004735FB File Offset: 0x004717FB
			// (set) Token: 0x0600BFFB RID: 49147 RVA: 0x00473603 File Offset: 0x00471803
			public string display_name { get; set; }
		}

		// Token: 0x02001859 RID: 6233
		public class Reward
		{
			// Token: 0x1700174B RID: 5963
			// (get) Token: 0x0600BFFD RID: 49149 RVA: 0x0047360C File Offset: 0x0047180C
			// (set) Token: 0x0600BFFE RID: 49150 RVA: 0x00473614 File Offset: 0x00471814
			public string title { get; set; }
		}
	}
}
