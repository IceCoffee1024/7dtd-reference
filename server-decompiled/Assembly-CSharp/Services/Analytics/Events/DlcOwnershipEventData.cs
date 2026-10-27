using System;
using System.Collections.Generic;
using System.ComponentModel;
using BhvrAnalyticsServices.Attributes;
using Newtonsoft.Json;

namespace Services.Analytics.Events
{
	// Token: 0x020016BA RID: 5818
	[EventTitle("DLC Ownership")]
	public class DlcOwnershipEventData : BaseEventData
	{
		// Token: 0x1700162D RID: 5677
		// (get) Token: 0x0600B5D9 RID: 46553 RVA: 0x0043DA63 File Offset: 0x0043BC63
		public override string EventType
		{
			get
			{
				return "dlc_ownership";
			}
		}

		// Token: 0x1700162E RID: 5678
		// (get) Token: 0x0600B5DA RID: 46554 RVA: 0x0043DA6A File Offset: 0x0043BC6A
		// (set) Token: 0x0600B5DB RID: 46555 RVA: 0x0043DA72 File Offset: 0x0043BC72
		[JsonProperty(PropertyName = "dlc_owned")]
		[Description("Dlc owned by a player, Acquired data is steam only ")]
		public IEnumerable<DlcOwnershipEventData.EntitlementData> DlcOwned { get; set; }

		// Token: 0x1700162F RID: 5679
		// (get) Token: 0x0600B5DC RID: 46556 RVA: 0x0043DA7B File Offset: 0x0043BC7B
		// (set) Token: 0x0600B5DD RID: 46557 RVA: 0x0043DA83 File Offset: 0x0043BC83
		[JsonProperty(PropertyName = "is_family_share")]
		[Description("Is the user playing form a family library? (Steam Only)")]
		public bool? IsFamilyShare { get; set; }

		// Token: 0x020016BB RID: 5819
		[JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
		public struct EntitlementData
		{
			// Token: 0x0400882F RID: 34863
			public string Name;

			// Token: 0x04008830 RID: 34864
			public string ID;

			// Token: 0x04008831 RID: 34865
			public string AcquiredDate;
		}
	}
}
