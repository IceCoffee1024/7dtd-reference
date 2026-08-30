using System;
using Newtonsoft.Json;

namespace Twitch.PubSub
{
	// Token: 0x02001853 RID: 6227
	public class PubSubBitRedemptionMessage : BasePubSubMessage
	{
		// Token: 0x0600BFDB RID: 49115 RVA: 0x00473517 File Offset: 0x00471717
		public static PubSubBitRedemptionMessage Deserialize(string message)
		{
			return JsonConvert.DeserializeObject<PubSubBitRedemptionMessage>(message);
		}

		// Token: 0x04009148 RID: 37192
		public PubSubBitRedemptionMessage.BitRedemptionData data;

		// Token: 0x02001854 RID: 6228
		public class BitRedemptionData : EventArgs
		{
			// Token: 0x1700173E RID: 5950
			// (get) Token: 0x0600BFDD RID: 49117 RVA: 0x00473527 File Offset: 0x00471727
			// (set) Token: 0x0600BFDE RID: 49118 RVA: 0x0047352F File Offset: 0x0047172F
			public string user_name { get; set; }

			// Token: 0x1700173F RID: 5951
			// (get) Token: 0x0600BFDF RID: 49119 RVA: 0x00473538 File Offset: 0x00471738
			// (set) Token: 0x0600BFE0 RID: 49120 RVA: 0x00473540 File Offset: 0x00471740
			public string channel_name { get; set; }

			// Token: 0x17001740 RID: 5952
			// (get) Token: 0x0600BFE1 RID: 49121 RVA: 0x00473549 File Offset: 0x00471749
			// (set) Token: 0x0600BFE2 RID: 49122 RVA: 0x00473551 File Offset: 0x00471751
			public string user_id { get; set; }

			// Token: 0x17001741 RID: 5953
			// (get) Token: 0x0600BFE3 RID: 49123 RVA: 0x0047355A File Offset: 0x0047175A
			// (set) Token: 0x0600BFE4 RID: 49124 RVA: 0x00473562 File Offset: 0x00471762
			public string channel_id { get; set; }

			// Token: 0x17001742 RID: 5954
			// (get) Token: 0x0600BFE5 RID: 49125 RVA: 0x0047356B File Offset: 0x0047176B
			// (set) Token: 0x0600BFE6 RID: 49126 RVA: 0x00473573 File Offset: 0x00471773
			public string chat_message { get; set; }

			// Token: 0x17001743 RID: 5955
			// (get) Token: 0x0600BFE7 RID: 49127 RVA: 0x0047357C File Offset: 0x0047177C
			// (set) Token: 0x0600BFE8 RID: 49128 RVA: 0x00473584 File Offset: 0x00471784
			public int bits_used { get; set; }

			// Token: 0x17001744 RID: 5956
			// (get) Token: 0x0600BFE9 RID: 49129 RVA: 0x0047358D File Offset: 0x0047178D
			// (set) Token: 0x0600BFEA RID: 49130 RVA: 0x00473595 File Offset: 0x00471795
			public int total_bits_used { get; set; }

			// Token: 0x17001745 RID: 5957
			// (get) Token: 0x0600BFEB RID: 49131 RVA: 0x0047359E File Offset: 0x0047179E
			// (set) Token: 0x0600BFEC RID: 49132 RVA: 0x004735A6 File Offset: 0x004717A6
			public bool is_anonymous { get; set; }
		}
	}
}
