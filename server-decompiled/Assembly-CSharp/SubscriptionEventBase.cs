using System;
using Newtonsoft.Json;

// Token: 0x02000CA9 RID: 3241
public class SubscriptionEventBase
{
	// Token: 0x17000A57 RID: 2647
	// (get) Token: 0x060062DD RID: 25309 RVA: 0x00272041 File Offset: 0x00270241
	// (set) Token: 0x060062DE RID: 25310 RVA: 0x00272049 File Offset: 0x00270249
	[JsonProperty("user_id")]
	public string UserId { get; set; } = "";

	// Token: 0x17000A58 RID: 2648
	// (get) Token: 0x060062DF RID: 25311 RVA: 0x00272052 File Offset: 0x00270252
	// (set) Token: 0x060062E0 RID: 25312 RVA: 0x0027205A File Offset: 0x0027025A
	[JsonProperty("user_login")]
	public string UserLogin { get; set; } = "";

	// Token: 0x17000A59 RID: 2649
	// (get) Token: 0x060062E1 RID: 25313 RVA: 0x00272063 File Offset: 0x00270263
	// (set) Token: 0x060062E2 RID: 25314 RVA: 0x0027206B File Offset: 0x0027026B
	[JsonProperty("user_name")]
	public string UserName { get; set; } = "";

	// Token: 0x17000A5A RID: 2650
	// (get) Token: 0x060062E3 RID: 25315 RVA: 0x00272074 File Offset: 0x00270274
	// (set) Token: 0x060062E4 RID: 25316 RVA: 0x0027207C File Offset: 0x0027027C
	[JsonProperty("tier")]
	public string Tier { get; set; } = "";
}
