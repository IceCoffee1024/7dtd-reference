using System;
using Newtonsoft.Json;

namespace Twitch.PubSub
{
	// Token: 0x020018A7 RID: 6311
	public class PubSubChannelPointMessage : BasePubSubMessage
	{
		// Token: 0x0600C2CB RID: 49867 RVA: 0x0047D887 File Offset: 0x0047BA87
		public static PubSubChannelPointMessage Deserialize(string message)
		{
			return JsonConvert.DeserializeObject<PubSubChannelPointMessage>(message);
		}

		// Token: 0x04009333 RID: 37683
		public PubSubChannelPointMessage.ChannelRedemptionData data;

		// Token: 0x020018A8 RID: 6312
		public class ChannelRedemptionData : EventArgs
		{
			// Token: 0x1700185A RID: 6234
			// (get) Token: 0x0600C2CD RID: 49869 RVA: 0x0047D88F File Offset: 0x0047BA8F
			// (set) Token: 0x0600C2CE RID: 49870 RVA: 0x0047D897 File Offset: 0x0047BA97
			public PubSubChannelPointMessage.Redemption redemption { get; set; }
		}

		// Token: 0x020018A9 RID: 6313
		public class Redemption
		{
			// Token: 0x1700185B RID: 6235
			// (get) Token: 0x0600C2D0 RID: 49872 RVA: 0x0047D8A0 File Offset: 0x0047BAA0
			// (set) Token: 0x0600C2D1 RID: 49873 RVA: 0x0047D8A8 File Offset: 0x0047BAA8
			public PubSubChannelPointMessage.User user { get; set; }

			// Token: 0x1700185C RID: 6236
			// (get) Token: 0x0600C2D2 RID: 49874 RVA: 0x0047D8B1 File Offset: 0x0047BAB1
			// (set) Token: 0x0600C2D3 RID: 49875 RVA: 0x0047D8B9 File Offset: 0x0047BAB9
			public PubSubChannelPointMessage.Reward reward { get; set; }
		}

		// Token: 0x020018AA RID: 6314
		public class User
		{
			// Token: 0x1700185D RID: 6237
			// (get) Token: 0x0600C2D5 RID: 49877 RVA: 0x0047D8C2 File Offset: 0x0047BAC2
			// (set) Token: 0x0600C2D6 RID: 49878 RVA: 0x0047D8CA File Offset: 0x0047BACA
			public string login { get; set; }

			// Token: 0x1700185E RID: 6238
			// (get) Token: 0x0600C2D7 RID: 49879 RVA: 0x0047D8D3 File Offset: 0x0047BAD3
			// (set) Token: 0x0600C2D8 RID: 49880 RVA: 0x0047D8DB File Offset: 0x0047BADB
			public string display_name { get; set; }
		}

		// Token: 0x020018AB RID: 6315
		public class Reward
		{
			// Token: 0x1700185F RID: 6239
			// (get) Token: 0x0600C2DA RID: 49882 RVA: 0x0047D8E4 File Offset: 0x0047BAE4
			// (set) Token: 0x0600C2DB RID: 49883 RVA: 0x0047D8EC File Offset: 0x0047BAEC
			public string title { get; set; }
		}
	}
}
