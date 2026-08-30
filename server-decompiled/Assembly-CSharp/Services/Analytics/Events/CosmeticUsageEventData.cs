using System;
using System.ComponentModel;
using BhvrAnalyticsServices.Attributes;
using Newtonsoft.Json;

namespace Services.Analytics.Events
{
	// Token: 0x02001686 RID: 5766
	[EventTitle("CosmeticUsage")]
	public class CosmeticUsageEventData : BaseEventData
	{
		// Token: 0x170015CD RID: 5581
		// (get) Token: 0x0600B4C6 RID: 46278 RVA: 0x004377B0 File Offset: 0x004359B0
		public override string EventType
		{
			get
			{
				return "cosmetic_usage";
			}
		}

		// Token: 0x170015CE RID: 5582
		// (get) Token: 0x0600B4C7 RID: 46279 RVA: 0x004377B7 File Offset: 0x004359B7
		// (set) Token: 0x0600B4C8 RID: 46280 RVA: 0x004377BF File Offset: 0x004359BF
		[JsonProperty(PropertyName = "server_id")]
		[Description("Id of the connected server, null if not in one")]
		public string ServerId { get; set; }

		// Token: 0x170015CF RID: 5583
		// (get) Token: 0x0600B4C9 RID: 46281 RVA: 0x004377C8 File Offset: 0x004359C8
		// (set) Token: 0x0600B4CA RID: 46282 RVA: 0x004377D0 File Offset: 0x004359D0
		[JsonProperty(PropertyName = "save_id")]
		[Description("Id of the save")]
		public string SaveId { get; set; }

		// Token: 0x170015D0 RID: 5584
		// (get) Token: 0x0600B4CB RID: 46283 RVA: 0x004377D9 File Offset: 0x004359D9
		// (set) Token: 0x0600B4CC RID: 46284 RVA: 0x004377E1 File Offset: 0x004359E1
		[JsonProperty(PropertyName = "cosmetic_change_ts")]
		[Description("The moment when the player equipped the item.")]
		[DateTimeFormat]
		public string CosmeticChangeTimestamp { get; set; }

		// Token: 0x170015D1 RID: 5585
		// (get) Token: 0x0600B4CD RID: 46285 RVA: 0x004377EA File Offset: 0x004359EA
		// (set) Token: 0x0600B4CE RID: 46286 RVA: 0x004377F2 File Offset: 0x004359F2
		[JsonProperty(PropertyName = "slot")]
		[Description("The slot where the equipment goes.")]
		public string Slot { get; set; }

		// Token: 0x170015D2 RID: 5586
		// (get) Token: 0x0600B4CF RID: 46287 RVA: 0x004377FB File Offset: 0x004359FB
		// (set) Token: 0x0600B4D0 RID: 46288 RVA: 0x00437803 File Offset: 0x00435A03
		[JsonProperty(PropertyName = "cosmetic_name")]
		[Description("The name of the item (truncated to the first 50 characters)")]
		[JsonConverter(typeof(TruncateStringSerializerConverter))]
		public string CosmeticName { get; set; }

		// Token: 0x170015D3 RID: 5587
		// (get) Token: 0x0600B4D1 RID: 46289 RVA: 0x0043780C File Offset: 0x00435A0C
		// (set) Token: 0x0600B4D2 RID: 46290 RVA: 0x00437814 File Offset: 0x00435A14
		[JsonProperty(PropertyName = "set_name")]
		[Description("If the item is part of a set, we would like to know the set name.")]
		public string SetName { get; set; }
	}
}
