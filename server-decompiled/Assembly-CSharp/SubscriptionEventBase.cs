using System;
using Newtonsoft.Json;

// Token: 0x02000C9E RID: 3230
public class SubscriptionEventBase
{
	// Token: 0x17000A51 RID: 2641
	// (get) Token: 0x060062B8 RID: 25272 RVA: 0x00270B19 File Offset: 0x0026ED19
	// (set) Token: 0x060062B9 RID: 25273 RVA: 0x00270B21 File Offset: 0x0026ED21
	[JsonProperty("user_id")]
	public string UserId { get; set; } = "";

	// Token: 0x17000A52 RID: 2642
	// (get) Token: 0x060062BA RID: 25274 RVA: 0x00270B2A File Offset: 0x0026ED2A
	// (set) Token: 0x060062BB RID: 25275 RVA: 0x00270B32 File Offset: 0x0026ED32
	[JsonProperty("user_login")]
	public string UserLogin { get; set; } = "";

	// Token: 0x17000A53 RID: 2643
	// (get) Token: 0x060062BC RID: 25276 RVA: 0x00270B3B File Offset: 0x0026ED3B
	// (set) Token: 0x060062BD RID: 25277 RVA: 0x00270B43 File Offset: 0x0026ED43
	[JsonProperty("user_name")]
	public string UserName { get; set; } = "";

	// Token: 0x17000A54 RID: 2644
	// (get) Token: 0x060062BE RID: 25278 RVA: 0x00270B4C File Offset: 0x0026ED4C
	// (set) Token: 0x060062BF RID: 25279 RVA: 0x00270B54 File Offset: 0x0026ED54
	[JsonProperty("tier")]
	public string Tier { get; set; } = "";
}
