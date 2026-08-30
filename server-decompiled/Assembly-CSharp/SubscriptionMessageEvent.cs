using System;
using Newtonsoft.Json;

// Token: 0x02000C92 RID: 3218
public class SubscriptionMessageEvent : SubscriptionEventBase
{
	// Token: 0x17000A41 RID: 2625
	// (get) Token: 0x060062D3 RID: 25299 RVA: 0x0026D76F File Offset: 0x0026B96F
	// (set) Token: 0x060062D4 RID: 25300 RVA: 0x0026D777 File Offset: 0x0026B977
	[JsonProperty("cumulative_months")]
	public int CumulativeMonths { get; set; }

	// Token: 0x17000A42 RID: 2626
	// (get) Token: 0x060062D5 RID: 25301 RVA: 0x0026D780 File Offset: 0x0026B980
	// (set) Token: 0x060062D6 RID: 25302 RVA: 0x0026D788 File Offset: 0x0026B988
	[JsonProperty("streak_months")]
	public string StreakMonths { get; set; }

	// Token: 0x17000A43 RID: 2627
	// (get) Token: 0x060062D7 RID: 25303 RVA: 0x0026D791 File Offset: 0x0026B991
	// (set) Token: 0x060062D8 RID: 25304 RVA: 0x0026D799 File Offset: 0x0026B999
	[JsonProperty("duration_months")]
	public string DurationMonths { get; set; } = "";
}
