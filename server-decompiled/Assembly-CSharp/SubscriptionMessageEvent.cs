using System;
using Newtonsoft.Json;

// Token: 0x02000CAB RID: 3243
public class SubscriptionMessageEvent : SubscriptionEventBase
{
	// Token: 0x17000A5D RID: 2653
	// (get) Token: 0x060062EB RID: 25323 RVA: 0x002720DB File Offset: 0x002702DB
	// (set) Token: 0x060062EC RID: 25324 RVA: 0x002720E3 File Offset: 0x002702E3
	[JsonProperty("cumulative_months")]
	public int CumulativeMonths { get; set; }

	// Token: 0x17000A5E RID: 2654
	// (get) Token: 0x060062ED RID: 25325 RVA: 0x002720EC File Offset: 0x002702EC
	// (set) Token: 0x060062EE RID: 25326 RVA: 0x002720F4 File Offset: 0x002702F4
	[JsonProperty("streak_months")]
	public string StreakMonths { get; set; }

	// Token: 0x17000A5F RID: 2655
	// (get) Token: 0x060062EF RID: 25327 RVA: 0x002720FD File Offset: 0x002702FD
	// (set) Token: 0x060062F0 RID: 25328 RVA: 0x00272105 File Offset: 0x00270305
	[JsonProperty("duration_months")]
	public string DurationMonths { get; set; } = "";
}
