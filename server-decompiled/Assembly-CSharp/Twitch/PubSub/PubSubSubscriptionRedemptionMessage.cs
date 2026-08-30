using System;
using Newtonsoft.Json;

namespace Twitch.PubSub
{
	// Token: 0x02001887 RID: 6279
	public class PubSubSubscriptionRedemptionMessage : BasePubSubMessage
	{
		// Token: 0x170017C9 RID: 6089
		// (get) Token: 0x0600C1A3 RID: 49571 RVA: 0x0047949D File Offset: 0x0047769D
		// (set) Token: 0x0600C1A4 RID: 49572 RVA: 0x004794A5 File Offset: 0x004776A5
		public int benefit_end_month { get; set; }

		// Token: 0x170017CA RID: 6090
		// (get) Token: 0x0600C1A5 RID: 49573 RVA: 0x004794AE File Offset: 0x004776AE
		// (set) Token: 0x0600C1A6 RID: 49574 RVA: 0x004794B6 File Offset: 0x004776B6
		public string user_name { get; set; }

		// Token: 0x170017CB RID: 6091
		// (get) Token: 0x0600C1A7 RID: 49575 RVA: 0x004794BF File Offset: 0x004776BF
		// (set) Token: 0x0600C1A8 RID: 49576 RVA: 0x004794C7 File Offset: 0x004776C7
		public string channel_name { get; set; }

		// Token: 0x170017CC RID: 6092
		// (get) Token: 0x0600C1A9 RID: 49577 RVA: 0x004794D0 File Offset: 0x004776D0
		// (set) Token: 0x0600C1AA RID: 49578 RVA: 0x004794D8 File Offset: 0x004776D8
		public string user_id { get; set; }

		// Token: 0x170017CD RID: 6093
		// (get) Token: 0x0600C1AB RID: 49579 RVA: 0x004794E1 File Offset: 0x004776E1
		// (set) Token: 0x0600C1AC RID: 49580 RVA: 0x004794E9 File Offset: 0x004776E9
		public string channel_id { get; set; }

		// Token: 0x170017CE RID: 6094
		// (get) Token: 0x0600C1AD RID: 49581 RVA: 0x004794F2 File Offset: 0x004776F2
		// (set) Token: 0x0600C1AE RID: 49582 RVA: 0x004794FA File Offset: 0x004776FA
		public string sub_plan { get; set; }

		// Token: 0x170017CF RID: 6095
		// (get) Token: 0x0600C1AF RID: 49583 RVA: 0x00479503 File Offset: 0x00477703
		// (set) Token: 0x0600C1B0 RID: 49584 RVA: 0x0047950B File Offset: 0x0047770B
		public string sub_plan_name { get; set; }

		// Token: 0x170017D0 RID: 6096
		// (get) Token: 0x0600C1B1 RID: 49585 RVA: 0x00479514 File Offset: 0x00477714
		// (set) Token: 0x0600C1B2 RID: 49586 RVA: 0x0047951C File Offset: 0x0047771C
		public int months { get; set; }

		// Token: 0x170017D1 RID: 6097
		// (get) Token: 0x0600C1B3 RID: 49587 RVA: 0x00479525 File Offset: 0x00477725
		// (set) Token: 0x0600C1B4 RID: 49588 RVA: 0x0047952D File Offset: 0x0047772D
		public int cumulative_months { get; set; }

		// Token: 0x170017D2 RID: 6098
		// (get) Token: 0x0600C1B5 RID: 49589 RVA: 0x00479536 File Offset: 0x00477736
		// (set) Token: 0x0600C1B6 RID: 49590 RVA: 0x0047953E File Offset: 0x0047773E
		public string context { get; set; }

		// Token: 0x170017D3 RID: 6099
		// (get) Token: 0x0600C1B7 RID: 49591 RVA: 0x00479547 File Offset: 0x00477747
		// (set) Token: 0x0600C1B8 RID: 49592 RVA: 0x0047954F File Offset: 0x0047774F
		public bool is_gift { get; set; }

		// Token: 0x170017D4 RID: 6100
		// (get) Token: 0x0600C1B9 RID: 49593 RVA: 0x00479558 File Offset: 0x00477758
		// (set) Token: 0x0600C1BA RID: 49594 RVA: 0x00479560 File Offset: 0x00477760
		public int multi_month_duration { get; set; }

		// Token: 0x0600C1BB RID: 49595 RVA: 0x00479569 File Offset: 0x00477769
		public static PubSubSubscriptionRedemptionMessage Deserialize(string message)
		{
			return JsonConvert.DeserializeObject<PubSubSubscriptionRedemptionMessage>(message);
		}
	}
}
