using System;
using Newtonsoft.Json;

namespace Twitch.PubSub
{
	// Token: 0x020018D8 RID: 6360
	public class PubSubChannelPointMessage : BasePubSubMessage
	{
		// Token: 0x0600C3DF RID: 50143 RVA: 0x00481233 File Offset: 0x0047F433
		public static PubSubChannelPointMessage Deserialize(string message)
		{
			return JsonConvert.DeserializeObject<PubSubChannelPointMessage>(message);
		}

		// Token: 0x0400941C RID: 37916
		public PubSubChannelPointMessage.ChannelRedemptionData data;

		// Token: 0x020018D9 RID: 6361
		public class ChannelRedemptionData : EventArgs
		{
			// Token: 0x170018A2 RID: 6306
			// (get) Token: 0x0600C3E1 RID: 50145 RVA: 0x0048123B File Offset: 0x0047F43B
			// (set) Token: 0x0600C3E2 RID: 50146 RVA: 0x00481243 File Offset: 0x0047F443
			public PubSubChannelPointMessage.Redemption redemption { get; set; }
		}

		// Token: 0x020018DA RID: 6362
		public class Redemption
		{
			// Token: 0x170018A3 RID: 6307
			// (get) Token: 0x0600C3E4 RID: 50148 RVA: 0x0048124C File Offset: 0x0047F44C
			// (set) Token: 0x0600C3E5 RID: 50149 RVA: 0x00481254 File Offset: 0x0047F454
			public PubSubChannelPointMessage.User user { get; set; }

			// Token: 0x170018A4 RID: 6308
			// (get) Token: 0x0600C3E6 RID: 50150 RVA: 0x0048125D File Offset: 0x0047F45D
			// (set) Token: 0x0600C3E7 RID: 50151 RVA: 0x00481265 File Offset: 0x0047F465
			public PubSubChannelPointMessage.Reward reward { get; set; }
		}

		// Token: 0x020018DB RID: 6363
		public class User
		{
			// Token: 0x170018A5 RID: 6309
			// (get) Token: 0x0600C3E9 RID: 50153 RVA: 0x0048126E File Offset: 0x0047F46E
			// (set) Token: 0x0600C3EA RID: 50154 RVA: 0x00481276 File Offset: 0x0047F476
			public string login { get; set; }

			// Token: 0x170018A6 RID: 6310
			// (get) Token: 0x0600C3EB RID: 50155 RVA: 0x0048127F File Offset: 0x0047F47F
			// (set) Token: 0x0600C3EC RID: 50156 RVA: 0x00481287 File Offset: 0x0047F487
			public string display_name { get; set; }
		}

		// Token: 0x020018DC RID: 6364
		public class Reward
		{
			// Token: 0x170018A7 RID: 6311
			// (get) Token: 0x0600C3EE RID: 50158 RVA: 0x00481290 File Offset: 0x0047F490
			// (set) Token: 0x0600C3EF RID: 50159 RVA: 0x00481298 File Offset: 0x0047F498
			public string title { get; set; }
		}
	}
}
