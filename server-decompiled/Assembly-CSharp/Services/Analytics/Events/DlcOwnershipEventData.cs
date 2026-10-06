using System;
using System.Collections.Generic;
using System.ComponentModel;
using BhvrAnalyticsServices.Attributes;
using Newtonsoft.Json;

namespace Services.Analytics.Events
{
	// Token: 0x020016A7 RID: 5799
	[EventTitle("DLC Ownership")]
	public class DlcOwnershipEventData : BaseEventData
	{
		// Token: 0x17001619 RID: 5657
		// (get) Token: 0x0600B56E RID: 46446 RVA: 0x0043B531 File Offset: 0x00439731
		public override string EventType
		{
			get
			{
				return "dlc_ownership";
			}
		}

		// Token: 0x1700161A RID: 5658
		// (get) Token: 0x0600B56F RID: 46447 RVA: 0x0043B538 File Offset: 0x00439738
		// (set) Token: 0x0600B570 RID: 46448 RVA: 0x0043B540 File Offset: 0x00439740
		[JsonProperty(PropertyName = "dlc_owned")]
		[Description("Dlc owned by a player, Acquired data is steam only ")]
		public IEnumerable<DlcOwnershipEventData.EntitlementData> DlcOwned { get; set; }

		// Token: 0x1700161B RID: 5659
		// (get) Token: 0x0600B571 RID: 46449 RVA: 0x0043B549 File Offset: 0x00439749
		// (set) Token: 0x0600B572 RID: 46450 RVA: 0x0043B551 File Offset: 0x00439751
		[JsonProperty(PropertyName = "is_family_share")]
		[Description("Is the user playing form a family library? (Steam Only)")]
		public bool? IsFamilyShare { get; set; }

		// Token: 0x020016A8 RID: 5800
		[JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
		public struct EntitlementData
		{
			// Token: 0x040087E0 RID: 34784
			public string Name;

			// Token: 0x040087E1 RID: 34785
			public string ID;

			// Token: 0x040087E2 RID: 34786
			public string AcquiredDate;
		}
	}
}
