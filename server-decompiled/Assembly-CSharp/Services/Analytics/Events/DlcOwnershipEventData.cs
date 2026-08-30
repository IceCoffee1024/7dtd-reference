using System;
using System.Collections.Generic;
using BhvrAnalyticsServices.Attributes;
using Newtonsoft.Json;

namespace Services.Analytics.Events
{
	// Token: 0x02001687 RID: 5767
	[EventTitle("DLC Ownership")]
	public class DlcOwnershipEventData : BaseEventData
	{
		// Token: 0x170015D4 RID: 5588
		// (get) Token: 0x0600B4D4 RID: 46292 RVA: 0x0043781D File Offset: 0x00435A1D
		public override string EventType
		{
			get
			{
				return "dlc_ownership";
			}
		}

		// Token: 0x170015D5 RID: 5589
		// (get) Token: 0x0600B4D5 RID: 46293 RVA: 0x00437824 File Offset: 0x00435A24
		// (set) Token: 0x0600B4D6 RID: 46294 RVA: 0x0043782C File Offset: 0x00435A2C
		[JsonProperty(PropertyName = "dlc_owned")]
		public IEnumerable<DlcOwnershipEventData.EntitlementData> DlcOwned { get; set; }

		// Token: 0x170015D6 RID: 5590
		// (get) Token: 0x0600B4D7 RID: 46295 RVA: 0x00437835 File Offset: 0x00435A35
		// (set) Token: 0x0600B4D8 RID: 46296 RVA: 0x0043783D File Offset: 0x00435A3D
		[JsonProperty(PropertyName = "is_family_share")]
		public bool? IsFamilyShare { get; set; }

		// Token: 0x02001688 RID: 5768
		[JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
		public struct EntitlementData
		{
			// Token: 0x04008789 RID: 34697
			public string Name;

			// Token: 0x0400878A RID: 34698
			public string ID;

			// Token: 0x0400878B RID: 34699
			public string AcquiredDate;
		}
	}
}
