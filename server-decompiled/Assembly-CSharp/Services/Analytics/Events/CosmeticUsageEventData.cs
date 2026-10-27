using System;
using System.ComponentModel;
using BhvrAnalyticsServices.Attributes;
using Newtonsoft.Json;

namespace Services.Analytics.Events
{
	// Token: 0x020016B9 RID: 5817
	[EventTitle("CosmeticUsage")]
	public class CosmeticUsageEventData : BaseEventData
	{
		// Token: 0x17001626 RID: 5670
		// (get) Token: 0x0600B5CB RID: 46539 RVA: 0x0043D9F6 File Offset: 0x0043BBF6
		public override string EventType
		{
			get
			{
				return "cosmetic_usage";
			}
		}

		// Token: 0x17001627 RID: 5671
		// (get) Token: 0x0600B5CC RID: 46540 RVA: 0x0043D9FD File Offset: 0x0043BBFD
		// (set) Token: 0x0600B5CD RID: 46541 RVA: 0x0043DA05 File Offset: 0x0043BC05
		[JsonProperty(PropertyName = "server_id")]
		[Description("Id of the connected server, null if not in one")]
		public string ServerId { get; set; }

		// Token: 0x17001628 RID: 5672
		// (get) Token: 0x0600B5CE RID: 46542 RVA: 0x0043DA0E File Offset: 0x0043BC0E
		// (set) Token: 0x0600B5CF RID: 46543 RVA: 0x0043DA16 File Offset: 0x0043BC16
		[JsonProperty(PropertyName = "save_id")]
		[Description("Unique Id of the world save")]
		public string SaveId { get; set; }

		// Token: 0x17001629 RID: 5673
		// (get) Token: 0x0600B5D0 RID: 46544 RVA: 0x0043DA1F File Offset: 0x0043BC1F
		// (set) Token: 0x0600B5D1 RID: 46545 RVA: 0x0043DA27 File Offset: 0x0043BC27
		[JsonProperty(PropertyName = "cosmetic_change_ts")]
		[Description("The moment when the player equipped the item.")]
		[DateTimeFormat]
		public string CosmeticChangeTimestamp { get; set; }

		// Token: 0x1700162A RID: 5674
		// (get) Token: 0x0600B5D2 RID: 46546 RVA: 0x0043DA30 File Offset: 0x0043BC30
		// (set) Token: 0x0600B5D3 RID: 46547 RVA: 0x0043DA38 File Offset: 0x0043BC38
		[JsonProperty(PropertyName = "slot")]
		[Description("The slot where the equipment goes.")]
		public string Slot { get; set; }

		// Token: 0x1700162B RID: 5675
		// (get) Token: 0x0600B5D4 RID: 46548 RVA: 0x0043DA41 File Offset: 0x0043BC41
		// (set) Token: 0x0600B5D5 RID: 46549 RVA: 0x0043DA49 File Offset: 0x0043BC49
		[JsonProperty(PropertyName = "cosmetic_name")]
		[Description("The name of the item (truncated to the first 50 characters)")]
		[JsonConverter(typeof(TruncateStringSerializerConverter))]
		public string CosmeticName { get; set; }

		// Token: 0x1700162C RID: 5676
		// (get) Token: 0x0600B5D6 RID: 46550 RVA: 0x0043DA52 File Offset: 0x0043BC52
		// (set) Token: 0x0600B5D7 RID: 46551 RVA: 0x0043DA5A File Offset: 0x0043BC5A
		[JsonProperty(PropertyName = "set_name")]
		[Description("If the item is part of a set, we would like to know the set name.")]
		public string SetName { get; set; }
	}
}
