using System;
using Newtonsoft.Json;

namespace Twitch.PubSub
{
	// Token: 0x020018B3 RID: 6323
	public class PubSubSubscriptionRedemptionMessage : BasePubSubMessage
	{
		// Token: 0x1700186B RID: 6251
		// (get) Token: 0x0600C2FB RID: 49915 RVA: 0x0047D9D9 File Offset: 0x0047BBD9
		// (set) Token: 0x0600C2FC RID: 49916 RVA: 0x0047D9E1 File Offset: 0x0047BBE1
		public int benefit_end_month { get; set; }

		// Token: 0x1700186C RID: 6252
		// (get) Token: 0x0600C2FD RID: 49917 RVA: 0x0047D9EA File Offset: 0x0047BBEA
		// (set) Token: 0x0600C2FE RID: 49918 RVA: 0x0047D9F2 File Offset: 0x0047BBF2
		public string user_name { get; set; }

		// Token: 0x1700186D RID: 6253
		// (get) Token: 0x0600C2FF RID: 49919 RVA: 0x0047D9FB File Offset: 0x0047BBFB
		// (set) Token: 0x0600C300 RID: 49920 RVA: 0x0047DA03 File Offset: 0x0047BC03
		public string channel_name { get; set; }

		// Token: 0x1700186E RID: 6254
		// (get) Token: 0x0600C301 RID: 49921 RVA: 0x0047DA0C File Offset: 0x0047BC0C
		// (set) Token: 0x0600C302 RID: 49922 RVA: 0x0047DA14 File Offset: 0x0047BC14
		public string user_id { get; set; }

		// Token: 0x1700186F RID: 6255
		// (get) Token: 0x0600C303 RID: 49923 RVA: 0x0047DA1D File Offset: 0x0047BC1D
		// (set) Token: 0x0600C304 RID: 49924 RVA: 0x0047DA25 File Offset: 0x0047BC25
		public string channel_id { get; set; }

		// Token: 0x17001870 RID: 6256
		// (get) Token: 0x0600C305 RID: 49925 RVA: 0x0047DA2E File Offset: 0x0047BC2E
		// (set) Token: 0x0600C306 RID: 49926 RVA: 0x0047DA36 File Offset: 0x0047BC36
		public string sub_plan { get; set; }

		// Token: 0x17001871 RID: 6257
		// (get) Token: 0x0600C307 RID: 49927 RVA: 0x0047DA3F File Offset: 0x0047BC3F
		// (set) Token: 0x0600C308 RID: 49928 RVA: 0x0047DA47 File Offset: 0x0047BC47
		public string sub_plan_name { get; set; }

		// Token: 0x17001872 RID: 6258
		// (get) Token: 0x0600C309 RID: 49929 RVA: 0x0047DA50 File Offset: 0x0047BC50
		// (set) Token: 0x0600C30A RID: 49930 RVA: 0x0047DA58 File Offset: 0x0047BC58
		public int months { get; set; }

		// Token: 0x17001873 RID: 6259
		// (get) Token: 0x0600C30B RID: 49931 RVA: 0x0047DA61 File Offset: 0x0047BC61
		// (set) Token: 0x0600C30C RID: 49932 RVA: 0x0047DA69 File Offset: 0x0047BC69
		public int cumulative_months { get; set; }

		// Token: 0x17001874 RID: 6260
		// (get) Token: 0x0600C30D RID: 49933 RVA: 0x0047DA72 File Offset: 0x0047BC72
		// (set) Token: 0x0600C30E RID: 49934 RVA: 0x0047DA7A File Offset: 0x0047BC7A
		public string context { get; set; }

		// Token: 0x17001875 RID: 6261
		// (get) Token: 0x0600C30F RID: 49935 RVA: 0x0047DA83 File Offset: 0x0047BC83
		// (set) Token: 0x0600C310 RID: 49936 RVA: 0x0047DA8B File Offset: 0x0047BC8B
		public bool is_gift { get; set; }

		// Token: 0x17001876 RID: 6262
		// (get) Token: 0x0600C311 RID: 49937 RVA: 0x0047DA94 File Offset: 0x0047BC94
		// (set) Token: 0x0600C312 RID: 49938 RVA: 0x0047DA9C File Offset: 0x0047BC9C
		public int multi_month_duration { get; set; }

		// Token: 0x0600C313 RID: 49939 RVA: 0x0047DAA5 File Offset: 0x0047BCA5
		public static PubSubSubscriptionRedemptionMessage Deserialize(string message)
		{
			return JsonConvert.DeserializeObject<PubSubSubscriptionRedemptionMessage>(message);
		}
	}
}
