using System;
using System.ComponentModel;
using BhvrAnalyticsServices.Attributes;
using Newtonsoft.Json;

namespace Services.Analytics.Events
{
	// Token: 0x020016A6 RID: 5798
	[EventTitle("CosmeticUsage")]
	public class CosmeticUsageEventData : BaseEventData
	{
		// Token: 0x17001612 RID: 5650
		// (get) Token: 0x0600B560 RID: 46432 RVA: 0x0043B4C4 File Offset: 0x004396C4
		public override string EventType
		{
			get
			{
				return "cosmetic_usage";
			}
		}

		// Token: 0x17001613 RID: 5651
		// (get) Token: 0x0600B561 RID: 46433 RVA: 0x0043B4CB File Offset: 0x004396CB
		// (set) Token: 0x0600B562 RID: 46434 RVA: 0x0043B4D3 File Offset: 0x004396D3
		[JsonProperty(PropertyName = "server_id")]
		[Description("Id of the connected server, null if not in one")]
		public string ServerId { get; set; }

		// Token: 0x17001614 RID: 5652
		// (get) Token: 0x0600B563 RID: 46435 RVA: 0x0043B4DC File Offset: 0x004396DC
		// (set) Token: 0x0600B564 RID: 46436 RVA: 0x0043B4E4 File Offset: 0x004396E4
		[JsonProperty(PropertyName = "save_id")]
		[Description("Unique Id of the world save")]
		public string SaveId { get; set; }

		// Token: 0x17001615 RID: 5653
		// (get) Token: 0x0600B565 RID: 46437 RVA: 0x0043B4ED File Offset: 0x004396ED
		// (set) Token: 0x0600B566 RID: 46438 RVA: 0x0043B4F5 File Offset: 0x004396F5
		[JsonProperty(PropertyName = "cosmetic_change_ts")]
		[Description("The moment when the player equipped the item.")]
		[DateTimeFormat]
		public string CosmeticChangeTimestamp { get; set; }

		// Token: 0x17001616 RID: 5654
		// (get) Token: 0x0600B567 RID: 46439 RVA: 0x0043B4FE File Offset: 0x004396FE
		// (set) Token: 0x0600B568 RID: 46440 RVA: 0x0043B506 File Offset: 0x00439706
		[JsonProperty(PropertyName = "slot")]
		[Description("The slot where the equipment goes.")]
		public string Slot { get; set; }

		// Token: 0x17001617 RID: 5655
		// (get) Token: 0x0600B569 RID: 46441 RVA: 0x0043B50F File Offset: 0x0043970F
		// (set) Token: 0x0600B56A RID: 46442 RVA: 0x0043B517 File Offset: 0x00439717
		[JsonProperty(PropertyName = "cosmetic_name")]
		[Description("The name of the item (truncated to the first 50 characters)")]
		[JsonConverter(typeof(TruncateStringSerializerConverter))]
		public string CosmeticName { get; set; }

		// Token: 0x17001618 RID: 5656
		// (get) Token: 0x0600B56B RID: 46443 RVA: 0x0043B520 File Offset: 0x00439720
		// (set) Token: 0x0600B56C RID: 46444 RVA: 0x0043B528 File Offset: 0x00439728
		[JsonProperty(PropertyName = "set_name")]
		[Description("If the item is part of a set, we would like to know the set name.")]
		public string SetName { get; set; }
	}
}
