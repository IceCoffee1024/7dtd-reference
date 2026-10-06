using System;
using Newtonsoft.Json;

// Token: 0x02000CA0 RID: 3232
public class SubscriptionMessageEvent : SubscriptionEventBase
{
	// Token: 0x17000A57 RID: 2647
	// (get) Token: 0x060062C6 RID: 25286 RVA: 0x00270BB3 File Offset: 0x0026EDB3
	// (set) Token: 0x060062C7 RID: 25287 RVA: 0x00270BBB File Offset: 0x0026EDBB
	[JsonProperty("cumulative_months")]
	public int CumulativeMonths { get; set; }

	// Token: 0x17000A58 RID: 2648
	// (get) Token: 0x060062C8 RID: 25288 RVA: 0x00270BC4 File Offset: 0x0026EDC4
	// (set) Token: 0x060062C9 RID: 25289 RVA: 0x00270BCC File Offset: 0x0026EDCC
	[JsonProperty("streak_months")]
	public string StreakMonths { get; set; }

	// Token: 0x17000A59 RID: 2649
	// (get) Token: 0x060062CA RID: 25290 RVA: 0x00270BD5 File Offset: 0x0026EDD5
	// (set) Token: 0x060062CB RID: 25291 RVA: 0x00270BDD File Offset: 0x0026EDDD
	[JsonProperty("duration_months")]
	public string DurationMonths { get; set; } = "";
}
