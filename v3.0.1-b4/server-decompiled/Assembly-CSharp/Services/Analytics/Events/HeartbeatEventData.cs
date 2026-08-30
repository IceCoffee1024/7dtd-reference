using System;
using System.ComponentModel;
using Newtonsoft.Json;

namespace Services.Analytics.Events
{
	// Token: 0x02001665 RID: 5733
	public class HeartbeatEventData : BaseEventData
	{
		// Token: 0x1700157D RID: 5501
		// (get) Token: 0x0600B387 RID: 45959 RVA: 0x00431D19 File Offset: 0x0042FF19
		public override string EventType
		{
			get
			{
				return "heartbeat";
			}
		}

		// Token: 0x1700157E RID: 5502
		// (get) Token: 0x0600B388 RID: 45960 RVA: 0x00431D20 File Offset: 0x0042FF20
		// (set) Token: 0x0600B389 RID: 45961 RVA: 0x00431D28 File Offset: 0x0042FF28
		[JsonProperty(PropertyName = "heartbeat_ts")]
		[Description("Timestamp of the heartbeat. Sent every 5 minutes after login")]
		public string HeartbeatTimestamp { get; set; }

		// Token: 0x1700157F RID: 5503
		// (get) Token: 0x0600B38A RID: 45962 RVA: 0x00431D31 File Offset: 0x0042FF31
		// (set) Token: 0x0600B38B RID: 45963 RVA: 0x00431D39 File Offset: 0x0042FF39
		[JsonProperty(PropertyName = "server_id")]
		[Description("Id of the connected server, null if not in one")]
		public string ServerId { get; set; }
	}
}
