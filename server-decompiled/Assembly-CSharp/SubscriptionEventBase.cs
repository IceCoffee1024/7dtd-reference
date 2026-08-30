using System;
using Newtonsoft.Json;

// Token: 0x02000C90 RID: 3216
public class SubscriptionEventBase
{
	// Token: 0x17000A3B RID: 2619
	// (get) Token: 0x060062C5 RID: 25285 RVA: 0x0026D6D5 File Offset: 0x0026B8D5
	// (set) Token: 0x060062C6 RID: 25286 RVA: 0x0026D6DD File Offset: 0x0026B8DD
	[JsonProperty("user_id")]
	public string UserId { get; set; } = "";

	// Token: 0x17000A3C RID: 2620
	// (get) Token: 0x060062C7 RID: 25287 RVA: 0x0026D6E6 File Offset: 0x0026B8E6
	// (set) Token: 0x060062C8 RID: 25288 RVA: 0x0026D6EE File Offset: 0x0026B8EE
	[JsonProperty("user_login")]
	public string UserLogin { get; set; } = "";

	// Token: 0x17000A3D RID: 2621
	// (get) Token: 0x060062C9 RID: 25289 RVA: 0x0026D6F7 File Offset: 0x0026B8F7
	// (set) Token: 0x060062CA RID: 25290 RVA: 0x0026D6FF File Offset: 0x0026B8FF
	[JsonProperty("user_name")]
	public string UserName { get; set; } = "";

	// Token: 0x17000A3E RID: 2622
	// (get) Token: 0x060062CB RID: 25291 RVA: 0x0026D708 File Offset: 0x0026B908
	// (set) Token: 0x060062CC RID: 25292 RVA: 0x0026D710 File Offset: 0x0026B910
	[JsonProperty("tier")]
	public string Tier { get; set; } = "";
}
