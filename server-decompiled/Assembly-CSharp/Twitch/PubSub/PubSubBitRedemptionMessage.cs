using System;
using Newtonsoft.Json;

namespace Twitch.PubSub
{
	// Token: 0x020018D6 RID: 6358
	public class PubSubBitRedemptionMessage : BasePubSubMessage
	{
		// Token: 0x0600C3CC RID: 50124 RVA: 0x0048119B File Offset: 0x0047F39B
		public static PubSubBitRedemptionMessage Deserialize(string message)
		{
			return JsonConvert.DeserializeObject<PubSubBitRedemptionMessage>(message);
		}

		// Token: 0x04009413 RID: 37907
		public PubSubBitRedemptionMessage.BitRedemptionData data;

		// Token: 0x020018D7 RID: 6359
		public class BitRedemptionData : EventArgs
		{
			// Token: 0x1700189A RID: 6298
			// (get) Token: 0x0600C3CE RID: 50126 RVA: 0x004811AB File Offset: 0x0047F3AB
			// (set) Token: 0x0600C3CF RID: 50127 RVA: 0x004811B3 File Offset: 0x0047F3B3
			public string user_name { get; set; }

			// Token: 0x1700189B RID: 6299
			// (get) Token: 0x0600C3D0 RID: 50128 RVA: 0x004811BC File Offset: 0x0047F3BC
			// (set) Token: 0x0600C3D1 RID: 50129 RVA: 0x004811C4 File Offset: 0x0047F3C4
			public string channel_name { get; set; }

			// Token: 0x1700189C RID: 6300
			// (get) Token: 0x0600C3D2 RID: 50130 RVA: 0x004811CD File Offset: 0x0047F3CD
			// (set) Token: 0x0600C3D3 RID: 50131 RVA: 0x004811D5 File Offset: 0x0047F3D5
			public string user_id { get; set; }

			// Token: 0x1700189D RID: 6301
			// (get) Token: 0x0600C3D4 RID: 50132 RVA: 0x004811DE File Offset: 0x0047F3DE
			// (set) Token: 0x0600C3D5 RID: 50133 RVA: 0x004811E6 File Offset: 0x0047F3E6
			public string channel_id { get; set; }

			// Token: 0x1700189E RID: 6302
			// (get) Token: 0x0600C3D6 RID: 50134 RVA: 0x004811EF File Offset: 0x0047F3EF
			// (set) Token: 0x0600C3D7 RID: 50135 RVA: 0x004811F7 File Offset: 0x0047F3F7
			public string chat_message { get; set; }

			// Token: 0x1700189F RID: 6303
			// (get) Token: 0x0600C3D8 RID: 50136 RVA: 0x00481200 File Offset: 0x0047F400
			// (set) Token: 0x0600C3D9 RID: 50137 RVA: 0x00481208 File Offset: 0x0047F408
			public int bits_used { get; set; }

			// Token: 0x170018A0 RID: 6304
			// (get) Token: 0x0600C3DA RID: 50138 RVA: 0x00481211 File Offset: 0x0047F411
			// (set) Token: 0x0600C3DB RID: 50139 RVA: 0x00481219 File Offset: 0x0047F419
			public int total_bits_used { get; set; }

			// Token: 0x170018A1 RID: 6305
			// (get) Token: 0x0600C3DC RID: 50140 RVA: 0x00481222 File Offset: 0x0047F422
			// (set) Token: 0x0600C3DD RID: 50141 RVA: 0x0048122A File Offset: 0x0047F42A
			public bool is_anonymous { get; set; }
		}
	}
}
