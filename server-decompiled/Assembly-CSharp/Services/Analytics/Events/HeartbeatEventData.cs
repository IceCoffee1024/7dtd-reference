using System;
using System.ComponentModel;
using BhvrAnalyticsServices.Attributes;
using Newtonsoft.Json;

namespace Services.Analytics.Events
{
	// Token: 0x020016BF RID: 5823
	[EventTitle("Heartbeat")]
	public class HeartbeatEventData : BaseEventData
	{
		// Token: 0x17001636 RID: 5686
		// (get) Token: 0x0600B5EC RID: 46572 RVA: 0x0043DBBD File Offset: 0x0043BDBD
		public override string EventType
		{
			get
			{
				return "heartbeat";
			}
		}

		// Token: 0x17001637 RID: 5687
		// (get) Token: 0x0600B5ED RID: 46573 RVA: 0x0043DBC4 File Offset: 0x0043BDC4
		// (set) Token: 0x0600B5EE RID: 46574 RVA: 0x0043DBCC File Offset: 0x0043BDCC
		[JsonProperty(PropertyName = "heartbeat_ts")]
		[Description("Timestamp of the heartbeat. Sent every 5 minutes after login")]
		[DateTimeFormat]
		public string HeartbeatTimestamp { get; set; }

		// Token: 0x17001638 RID: 5688
		// (get) Token: 0x0600B5EF RID: 46575 RVA: 0x0043DBD5 File Offset: 0x0043BDD5
		// (set) Token: 0x0600B5F0 RID: 46576 RVA: 0x0043DBDD File Offset: 0x0043BDDD
		[JsonProperty(PropertyName = "server_id")]
		[Description("Id of the connected server, null if not in one")]
		public string ServerId { get; set; }

		// Token: 0x17001639 RID: 5689
		// (get) Token: 0x0600B5F1 RID: 46577 RVA: 0x0043DBE6 File Offset: 0x0043BDE6
		// (set) Token: 0x0600B5F2 RID: 46578 RVA: 0x0043DBEE File Offset: 0x0043BDEE
		[JsonProperty(PropertyName = "save_id")]
		[Description("Unique Id of the world save")]
		public string SaveId { get; set; }

		// Token: 0x1700163A RID: 5690
		// (get) Token: 0x0600B5F3 RID: 46579 RVA: 0x0043DBF7 File Offset: 0x0043BDF7
		// (set) Token: 0x0600B5F4 RID: 46580 RVA: 0x0043DBFF File Offset: 0x0043BDFF
		[JsonProperty(PropertyName = "player_count")]
		[Description("The player count of the active zero, null when no server or 0 when not a dedicated server")]
		public int? PlayerCount { get; set; }
	}
}
