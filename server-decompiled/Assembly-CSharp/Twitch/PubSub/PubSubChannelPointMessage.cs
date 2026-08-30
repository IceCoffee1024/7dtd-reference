using System;
using Newtonsoft.Json;

namespace Twitch.PubSub
{
	// Token: 0x0200187B RID: 6267
	public class PubSubChannelPointMessage : BasePubSubMessage
	{
		// Token: 0x0600C173 RID: 49523 RVA: 0x0047934B File Offset: 0x0047754B
		public static PubSubChannelPointMessage Deserialize(string message)
		{
			return JsonConvert.DeserializeObject<PubSubChannelPointMessage>(message);
		}

		// Token: 0x0400926D RID: 37485
		public PubSubChannelPointMessage.ChannelRedemptionData data;

		// Token: 0x0200187C RID: 6268
		public class ChannelRedemptionData : EventArgs
		{
			// Token: 0x170017B8 RID: 6072
			// (get) Token: 0x0600C175 RID: 49525 RVA: 0x00479353 File Offset: 0x00477553
			// (set) Token: 0x0600C176 RID: 49526 RVA: 0x0047935B File Offset: 0x0047755B
			public PubSubChannelPointMessage.Redemption redemption { get; set; }
		}

		// Token: 0x0200187D RID: 6269
		public class Redemption
		{
			// Token: 0x170017B9 RID: 6073
			// (get) Token: 0x0600C178 RID: 49528 RVA: 0x00479364 File Offset: 0x00477564
			// (set) Token: 0x0600C179 RID: 49529 RVA: 0x0047936C File Offset: 0x0047756C
			public PubSubChannelPointMessage.User user { get; set; }

			// Token: 0x170017BA RID: 6074
			// (get) Token: 0x0600C17A RID: 49530 RVA: 0x00479375 File Offset: 0x00477575
			// (set) Token: 0x0600C17B RID: 49531 RVA: 0x0047937D File Offset: 0x0047757D
			public PubSubChannelPointMessage.Reward reward { get; set; }
		}

		// Token: 0x0200187E RID: 6270
		public class User
		{
			// Token: 0x170017BB RID: 6075
			// (get) Token: 0x0600C17D RID: 49533 RVA: 0x00479386 File Offset: 0x00477586
			// (set) Token: 0x0600C17E RID: 49534 RVA: 0x0047938E File Offset: 0x0047758E
			public string login { get; set; }

			// Token: 0x170017BC RID: 6076
			// (get) Token: 0x0600C17F RID: 49535 RVA: 0x00479397 File Offset: 0x00477597
			// (set) Token: 0x0600C180 RID: 49536 RVA: 0x0047939F File Offset: 0x0047759F
			public string display_name { get; set; }
		}

		// Token: 0x0200187F RID: 6271
		public class Reward
		{
			// Token: 0x170017BD RID: 6077
			// (get) Token: 0x0600C182 RID: 49538 RVA: 0x004793A8 File Offset: 0x004775A8
			// (set) Token: 0x0600C183 RID: 49539 RVA: 0x004793B0 File Offset: 0x004775B0
			public string title { get; set; }
		}
	}
}
