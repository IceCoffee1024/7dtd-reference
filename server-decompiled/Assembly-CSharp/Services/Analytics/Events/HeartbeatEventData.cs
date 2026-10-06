using System;
using System.ComponentModel;
using BhvrAnalyticsServices.Attributes;
using Newtonsoft.Json;

namespace Services.Analytics.Events
{
	// Token: 0x020016AC RID: 5804
	[EventTitle("Heartbeat")]
	public class HeartbeatEventData : BaseEventData
	{
		// Token: 0x17001622 RID: 5666
		// (get) Token: 0x0600B581 RID: 46465 RVA: 0x0043B689 File Offset: 0x00439889
		public override string EventType
		{
			get
			{
				return "heartbeat";
			}
		}

		// Token: 0x17001623 RID: 5667
		// (get) Token: 0x0600B582 RID: 46466 RVA: 0x0043B690 File Offset: 0x00439890
		// (set) Token: 0x0600B583 RID: 46467 RVA: 0x0043B698 File Offset: 0x00439898
		[JsonProperty(PropertyName = "heartbeat_ts")]
		[Description("Timestamp of the heartbeat. Sent every 5 minutes after login")]
		[DateTimeFormat]
		public string HeartbeatTimestamp { get; set; }

		// Token: 0x17001624 RID: 5668
		// (get) Token: 0x0600B584 RID: 46468 RVA: 0x0043B6A1 File Offset: 0x004398A1
		// (set) Token: 0x0600B585 RID: 46469 RVA: 0x0043B6A9 File Offset: 0x004398A9
		[JsonProperty(PropertyName = "server_id")]
		[Description("Id of the connected server, null if not in one")]
		public string ServerId { get; set; }

		// Token: 0x17001625 RID: 5669
		// (get) Token: 0x0600B586 RID: 46470 RVA: 0x0043B6B2 File Offset: 0x004398B2
		// (set) Token: 0x0600B587 RID: 46471 RVA: 0x0043B6BA File Offset: 0x004398BA
		[JsonProperty(PropertyName = "save_id")]
		[Description("Unique Id of the world save")]
		public string SaveId { get; set; }

		// Token: 0x17001626 RID: 5670
		// (get) Token: 0x0600B588 RID: 46472 RVA: 0x0043B6C3 File Offset: 0x004398C3
		// (set) Token: 0x0600B589 RID: 46473 RVA: 0x0043B6CB File Offset: 0x004398CB
		[JsonProperty(PropertyName = "player_count")]
		[Description("The player count of the active zero, null when no server or 0 when not a dedicated server")]
		public int? PlayerCount { get; set; }
	}
}
