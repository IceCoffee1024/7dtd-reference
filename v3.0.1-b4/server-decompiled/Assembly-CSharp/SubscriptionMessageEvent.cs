using System;
using Newtonsoft.Json;

// Token: 0x02000C80 RID: 3200
public class SubscriptionMessageEvent : SubscriptionEventBase
{
	// Token: 0x17000A2E RID: 2606
	// (get) Token: 0x06006248 RID: 25160 RVA: 0x0026A897 File Offset: 0x00268A97
	// (set) Token: 0x06006249 RID: 25161 RVA: 0x0026A89F File Offset: 0x00268A9F
	[JsonProperty("cumulative_months")]
	public int CumulativeMonths { get; set; }

	// Token: 0x17000A2F RID: 2607
	// (get) Token: 0x0600624A RID: 25162 RVA: 0x0026A8A8 File Offset: 0x00268AA8
	// (set) Token: 0x0600624B RID: 25163 RVA: 0x0026A8B0 File Offset: 0x00268AB0
	[JsonProperty("streak_months")]
	public string StreakMonths { get; set; }

	// Token: 0x17000A30 RID: 2608
	// (get) Token: 0x0600624C RID: 25164 RVA: 0x0026A8B9 File Offset: 0x00268AB9
	// (set) Token: 0x0600624D RID: 25165 RVA: 0x0026A8C1 File Offset: 0x00268AC1
	[JsonProperty("duration_months")]
	public string DurationMonths { get; set; } = "";
}
