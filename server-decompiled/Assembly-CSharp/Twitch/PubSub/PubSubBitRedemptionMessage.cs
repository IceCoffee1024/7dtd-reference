using System;
using Newtonsoft.Json;

namespace Twitch.PubSub
{
	// Token: 0x020018A5 RID: 6309
	public class PubSubBitRedemptionMessage : BasePubSubMessage
	{
		// Token: 0x0600C2B8 RID: 49848 RVA: 0x0047D7EF File Offset: 0x0047B9EF
		public static PubSubBitRedemptionMessage Deserialize(string message)
		{
			return JsonConvert.DeserializeObject<PubSubBitRedemptionMessage>(message);
		}

		// Token: 0x0400932A RID: 37674
		public PubSubBitRedemptionMessage.BitRedemptionData data;

		// Token: 0x020018A6 RID: 6310
		public class BitRedemptionData : EventArgs
		{
			// Token: 0x17001852 RID: 6226
			// (get) Token: 0x0600C2BA RID: 49850 RVA: 0x0047D7FF File Offset: 0x0047B9FF
			// (set) Token: 0x0600C2BB RID: 49851 RVA: 0x0047D807 File Offset: 0x0047BA07
			public string user_name { get; set; }

			// Token: 0x17001853 RID: 6227
			// (get) Token: 0x0600C2BC RID: 49852 RVA: 0x0047D810 File Offset: 0x0047BA10
			// (set) Token: 0x0600C2BD RID: 49853 RVA: 0x0047D818 File Offset: 0x0047BA18
			public string channel_name { get; set; }

			// Token: 0x17001854 RID: 6228
			// (get) Token: 0x0600C2BE RID: 49854 RVA: 0x0047D821 File Offset: 0x0047BA21
			// (set) Token: 0x0600C2BF RID: 49855 RVA: 0x0047D829 File Offset: 0x0047BA29
			public string user_id { get; set; }

			// Token: 0x17001855 RID: 6229
			// (get) Token: 0x0600C2C0 RID: 49856 RVA: 0x0047D832 File Offset: 0x0047BA32
			// (set) Token: 0x0600C2C1 RID: 49857 RVA: 0x0047D83A File Offset: 0x0047BA3A
			public string channel_id { get; set; }

			// Token: 0x17001856 RID: 6230
			// (get) Token: 0x0600C2C2 RID: 49858 RVA: 0x0047D843 File Offset: 0x0047BA43
			// (set) Token: 0x0600C2C3 RID: 49859 RVA: 0x0047D84B File Offset: 0x0047BA4B
			public string chat_message { get; set; }

			// Token: 0x17001857 RID: 6231
			// (get) Token: 0x0600C2C4 RID: 49860 RVA: 0x0047D854 File Offset: 0x0047BA54
			// (set) Token: 0x0600C2C5 RID: 49861 RVA: 0x0047D85C File Offset: 0x0047BA5C
			public int bits_used { get; set; }

			// Token: 0x17001858 RID: 6232
			// (get) Token: 0x0600C2C6 RID: 49862 RVA: 0x0047D865 File Offset: 0x0047BA65
			// (set) Token: 0x0600C2C7 RID: 49863 RVA: 0x0047D86D File Offset: 0x0047BA6D
			public int total_bits_used { get; set; }

			// Token: 0x17001859 RID: 6233
			// (get) Token: 0x0600C2C8 RID: 49864 RVA: 0x0047D876 File Offset: 0x0047BA76
			// (set) Token: 0x0600C2C9 RID: 49865 RVA: 0x0047D87E File Offset: 0x0047BA7E
			public bool is_anonymous { get; set; }
		}
	}
}
