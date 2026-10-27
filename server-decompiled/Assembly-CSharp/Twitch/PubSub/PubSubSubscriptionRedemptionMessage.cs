using System;
using Newtonsoft.Json;

namespace Twitch.PubSub
{
	// Token: 0x020018E4 RID: 6372
	public class PubSubSubscriptionRedemptionMessage : BasePubSubMessage
	{
		// Token: 0x170018B3 RID: 6323
		// (get) Token: 0x0600C40F RID: 50191 RVA: 0x00481385 File Offset: 0x0047F585
		// (set) Token: 0x0600C410 RID: 50192 RVA: 0x0048138D File Offset: 0x0047F58D
		public int benefit_end_month { get; set; }

		// Token: 0x170018B4 RID: 6324
		// (get) Token: 0x0600C411 RID: 50193 RVA: 0x00481396 File Offset: 0x0047F596
		// (set) Token: 0x0600C412 RID: 50194 RVA: 0x0048139E File Offset: 0x0047F59E
		public string user_name { get; set; }

		// Token: 0x170018B5 RID: 6325
		// (get) Token: 0x0600C413 RID: 50195 RVA: 0x004813A7 File Offset: 0x0047F5A7
		// (set) Token: 0x0600C414 RID: 50196 RVA: 0x004813AF File Offset: 0x0047F5AF
		public string channel_name { get; set; }

		// Token: 0x170018B6 RID: 6326
		// (get) Token: 0x0600C415 RID: 50197 RVA: 0x004813B8 File Offset: 0x0047F5B8
		// (set) Token: 0x0600C416 RID: 50198 RVA: 0x004813C0 File Offset: 0x0047F5C0
		public string user_id { get; set; }

		// Token: 0x170018B7 RID: 6327
		// (get) Token: 0x0600C417 RID: 50199 RVA: 0x004813C9 File Offset: 0x0047F5C9
		// (set) Token: 0x0600C418 RID: 50200 RVA: 0x004813D1 File Offset: 0x0047F5D1
		public string channel_id { get; set; }

		// Token: 0x170018B8 RID: 6328
		// (get) Token: 0x0600C419 RID: 50201 RVA: 0x004813DA File Offset: 0x0047F5DA
		// (set) Token: 0x0600C41A RID: 50202 RVA: 0x004813E2 File Offset: 0x0047F5E2
		public string sub_plan { get; set; }

		// Token: 0x170018B9 RID: 6329
		// (get) Token: 0x0600C41B RID: 50203 RVA: 0x004813EB File Offset: 0x0047F5EB
		// (set) Token: 0x0600C41C RID: 50204 RVA: 0x004813F3 File Offset: 0x0047F5F3
		public string sub_plan_name { get; set; }

		// Token: 0x170018BA RID: 6330
		// (get) Token: 0x0600C41D RID: 50205 RVA: 0x004813FC File Offset: 0x0047F5FC
		// (set) Token: 0x0600C41E RID: 50206 RVA: 0x00481404 File Offset: 0x0047F604
		public int months { get; set; }

		// Token: 0x170018BB RID: 6331
		// (get) Token: 0x0600C41F RID: 50207 RVA: 0x0048140D File Offset: 0x0047F60D
		// (set) Token: 0x0600C420 RID: 50208 RVA: 0x00481415 File Offset: 0x0047F615
		public int cumulative_months { get; set; }

		// Token: 0x170018BC RID: 6332
		// (get) Token: 0x0600C421 RID: 50209 RVA: 0x0048141E File Offset: 0x0047F61E
		// (set) Token: 0x0600C422 RID: 50210 RVA: 0x00481426 File Offset: 0x0047F626
		public string context { get; set; }

		// Token: 0x170018BD RID: 6333
		// (get) Token: 0x0600C423 RID: 50211 RVA: 0x0048142F File Offset: 0x0047F62F
		// (set) Token: 0x0600C424 RID: 50212 RVA: 0x00481437 File Offset: 0x0047F637
		public bool is_gift { get; set; }

		// Token: 0x170018BE RID: 6334
		// (get) Token: 0x0600C425 RID: 50213 RVA: 0x00481440 File Offset: 0x0047F640
		// (set) Token: 0x0600C426 RID: 50214 RVA: 0x00481448 File Offset: 0x0047F648
		public int multi_month_duration { get; set; }

		// Token: 0x0600C427 RID: 50215 RVA: 0x00481451 File Offset: 0x0047F651
		public static PubSubSubscriptionRedemptionMessage Deserialize(string message)
		{
			return JsonConvert.DeserializeObject<PubSubSubscriptionRedemptionMessage>(message);
		}
	}
}
