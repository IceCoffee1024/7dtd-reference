using System;
using Newtonsoft.Json;

namespace Twitch.PubSub
{
	// Token: 0x02001861 RID: 6241
	public class PubSubSubscriptionRedemptionMessage : BasePubSubMessage
	{
		// Token: 0x17001757 RID: 5975
		// (get) Token: 0x0600C01E RID: 49182 RVA: 0x00473701 File Offset: 0x00471901
		// (set) Token: 0x0600C01F RID: 49183 RVA: 0x00473709 File Offset: 0x00471909
		public int benefit_end_month { get; set; }

		// Token: 0x17001758 RID: 5976
		// (get) Token: 0x0600C020 RID: 49184 RVA: 0x00473712 File Offset: 0x00471912
		// (set) Token: 0x0600C021 RID: 49185 RVA: 0x0047371A File Offset: 0x0047191A
		public string user_name { get; set; }

		// Token: 0x17001759 RID: 5977
		// (get) Token: 0x0600C022 RID: 49186 RVA: 0x00473723 File Offset: 0x00471923
		// (set) Token: 0x0600C023 RID: 49187 RVA: 0x0047372B File Offset: 0x0047192B
		public string channel_name { get; set; }

		// Token: 0x1700175A RID: 5978
		// (get) Token: 0x0600C024 RID: 49188 RVA: 0x00473734 File Offset: 0x00471934
		// (set) Token: 0x0600C025 RID: 49189 RVA: 0x0047373C File Offset: 0x0047193C
		public string user_id { get; set; }

		// Token: 0x1700175B RID: 5979
		// (get) Token: 0x0600C026 RID: 49190 RVA: 0x00473745 File Offset: 0x00471945
		// (set) Token: 0x0600C027 RID: 49191 RVA: 0x0047374D File Offset: 0x0047194D
		public string channel_id { get; set; }

		// Token: 0x1700175C RID: 5980
		// (get) Token: 0x0600C028 RID: 49192 RVA: 0x00473756 File Offset: 0x00471956
		// (set) Token: 0x0600C029 RID: 49193 RVA: 0x0047375E File Offset: 0x0047195E
		public string sub_plan { get; set; }

		// Token: 0x1700175D RID: 5981
		// (get) Token: 0x0600C02A RID: 49194 RVA: 0x00473767 File Offset: 0x00471967
		// (set) Token: 0x0600C02B RID: 49195 RVA: 0x0047376F File Offset: 0x0047196F
		public string sub_plan_name { get; set; }

		// Token: 0x1700175E RID: 5982
		// (get) Token: 0x0600C02C RID: 49196 RVA: 0x00473778 File Offset: 0x00471978
		// (set) Token: 0x0600C02D RID: 49197 RVA: 0x00473780 File Offset: 0x00471980
		public int months { get; set; }

		// Token: 0x1700175F RID: 5983
		// (get) Token: 0x0600C02E RID: 49198 RVA: 0x00473789 File Offset: 0x00471989
		// (set) Token: 0x0600C02F RID: 49199 RVA: 0x00473791 File Offset: 0x00471991
		public int cumulative_months { get; set; }

		// Token: 0x17001760 RID: 5984
		// (get) Token: 0x0600C030 RID: 49200 RVA: 0x0047379A File Offset: 0x0047199A
		// (set) Token: 0x0600C031 RID: 49201 RVA: 0x004737A2 File Offset: 0x004719A2
		public string context { get; set; }

		// Token: 0x17001761 RID: 5985
		// (get) Token: 0x0600C032 RID: 49202 RVA: 0x004737AB File Offset: 0x004719AB
		// (set) Token: 0x0600C033 RID: 49203 RVA: 0x004737B3 File Offset: 0x004719B3
		public bool is_gift { get; set; }

		// Token: 0x17001762 RID: 5986
		// (get) Token: 0x0600C034 RID: 49204 RVA: 0x004737BC File Offset: 0x004719BC
		// (set) Token: 0x0600C035 RID: 49205 RVA: 0x004737C4 File Offset: 0x004719C4
		public int multi_month_duration { get; set; }

		// Token: 0x0600C036 RID: 49206 RVA: 0x004737CD File Offset: 0x004719CD
		public static PubSubSubscriptionRedemptionMessage Deserialize(string message)
		{
			return JsonConvert.DeserializeObject<PubSubSubscriptionRedemptionMessage>(message);
		}
	}
}
