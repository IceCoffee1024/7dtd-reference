using System;
using Newtonsoft.Json;

namespace Twitch.PubSub
{
	// Token: 0x02001879 RID: 6265
	public class PubSubBitRedemptionMessage : BasePubSubMessage
	{
		// Token: 0x0600C160 RID: 49504 RVA: 0x004792B3 File Offset: 0x004774B3
		public static PubSubBitRedemptionMessage Deserialize(string message)
		{
			return JsonConvert.DeserializeObject<PubSubBitRedemptionMessage>(message);
		}

		// Token: 0x04009264 RID: 37476
		public PubSubBitRedemptionMessage.BitRedemptionData data;

		// Token: 0x0200187A RID: 6266
		public class BitRedemptionData : EventArgs
		{
			// Token: 0x170017B0 RID: 6064
			// (get) Token: 0x0600C162 RID: 49506 RVA: 0x004792C3 File Offset: 0x004774C3
			// (set) Token: 0x0600C163 RID: 49507 RVA: 0x004792CB File Offset: 0x004774CB
			public string user_name { get; set; }

			// Token: 0x170017B1 RID: 6065
			// (get) Token: 0x0600C164 RID: 49508 RVA: 0x004792D4 File Offset: 0x004774D4
			// (set) Token: 0x0600C165 RID: 49509 RVA: 0x004792DC File Offset: 0x004774DC
			public string channel_name { get; set; }

			// Token: 0x170017B2 RID: 6066
			// (get) Token: 0x0600C166 RID: 49510 RVA: 0x004792E5 File Offset: 0x004774E5
			// (set) Token: 0x0600C167 RID: 49511 RVA: 0x004792ED File Offset: 0x004774ED
			public string user_id { get; set; }

			// Token: 0x170017B3 RID: 6067
			// (get) Token: 0x0600C168 RID: 49512 RVA: 0x004792F6 File Offset: 0x004774F6
			// (set) Token: 0x0600C169 RID: 49513 RVA: 0x004792FE File Offset: 0x004774FE
			public string channel_id { get; set; }

			// Token: 0x170017B4 RID: 6068
			// (get) Token: 0x0600C16A RID: 49514 RVA: 0x00479307 File Offset: 0x00477507
			// (set) Token: 0x0600C16B RID: 49515 RVA: 0x0047930F File Offset: 0x0047750F
			public string chat_message { get; set; }

			// Token: 0x170017B5 RID: 6069
			// (get) Token: 0x0600C16C RID: 49516 RVA: 0x00479318 File Offset: 0x00477518
			// (set) Token: 0x0600C16D RID: 49517 RVA: 0x00479320 File Offset: 0x00477520
			public int bits_used { get; set; }

			// Token: 0x170017B6 RID: 6070
			// (get) Token: 0x0600C16E RID: 49518 RVA: 0x00479329 File Offset: 0x00477529
			// (set) Token: 0x0600C16F RID: 49519 RVA: 0x00479331 File Offset: 0x00477531
			public int total_bits_used { get; set; }

			// Token: 0x170017B7 RID: 6071
			// (get) Token: 0x0600C170 RID: 49520 RVA: 0x0047933A File Offset: 0x0047753A
			// (set) Token: 0x0600C171 RID: 49521 RVA: 0x00479342 File Offset: 0x00477542
			public bool is_anonymous { get; set; }
		}
	}
}
