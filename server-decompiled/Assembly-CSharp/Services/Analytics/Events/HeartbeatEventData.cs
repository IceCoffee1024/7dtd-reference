using System;
using System.ComponentModel;
using BhvrAnalyticsServices.Attributes;
using Newtonsoft.Json;

namespace Services.Analytics.Events
{
	// Token: 0x0200168A RID: 5770
	[EventTitle("Heartbeat")]
	public class HeartbeatEventData : BaseEventData
	{
		// Token: 0x170015DC RID: 5596
		// (get) Token: 0x0600B4E4 RID: 46308 RVA: 0x00437961 File Offset: 0x00435B61
		public override string EventType
		{
			get
			{
				return "heartbeat";
			}
		}

		// Token: 0x170015DD RID: 5597
		// (get) Token: 0x0600B4E5 RID: 46309 RVA: 0x00437968 File Offset: 0x00435B68
		// (set) Token: 0x0600B4E6 RID: 46310 RVA: 0x00437970 File Offset: 0x00435B70
		[JsonProperty(PropertyName = "heartbeat_ts")]
		[Description("Timestamp of the heartbeat. Sent every 5 minutes after login")]
		[DateTimeFormat]
		public string HeartbeatTimestamp { get; set; }

		// Token: 0x170015DE RID: 5598
		// (get) Token: 0x0600B4E7 RID: 46311 RVA: 0x00437979 File Offset: 0x00435B79
		// (set) Token: 0x0600B4E8 RID: 46312 RVA: 0x00437981 File Offset: 0x00435B81
		[JsonProperty(PropertyName = "server_id")]
		[Description("Id of the connected server, null if not in one")]
		public string ServerId { get; set; }

		// Token: 0x170015DF RID: 5599
		// (get) Token: 0x0600B4E9 RID: 46313 RVA: 0x0043798A File Offset: 0x00435B8A
		// (set) Token: 0x0600B4EA RID: 46314 RVA: 0x00437992 File Offset: 0x00435B92
		[JsonProperty(PropertyName = "save_id")]
		public string SaveId { get; set; }

		// Token: 0x170015E0 RID: 5600
		// (get) Token: 0x0600B4EB RID: 46315 RVA: 0x0043799B File Offset: 0x00435B9B
		// (set) Token: 0x0600B4EC RID: 46316 RVA: 0x004379A3 File Offset: 0x00435BA3
		[JsonProperty(PropertyName = "player_count")]
		public int? PlayerCount { get; set; }
	}
}
