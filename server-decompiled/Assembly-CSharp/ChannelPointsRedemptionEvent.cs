using System;
using Newtonsoft.Json;

// Token: 0x02000C85 RID: 3205
public class ChannelPointsRedemptionEvent
{
	// Token: 0x17000A24 RID: 2596
	// (get) Token: 0x0600627F RID: 25215 RVA: 0x0026CC05 File Offset: 0x0026AE05
	// (set) Token: 0x06006280 RID: 25216 RVA: 0x0026CC0D File Offset: 0x0026AE0D
	[JsonProperty("user_id")]
	public string UserId { get; set; } = "";

	// Token: 0x17000A25 RID: 2597
	// (get) Token: 0x06006281 RID: 25217 RVA: 0x0026CC16 File Offset: 0x0026AE16
	// (set) Token: 0x06006282 RID: 25218 RVA: 0x0026CC1E File Offset: 0x0026AE1E
	[JsonProperty("user_login")]
	public string UserLogin { get; set; } = "";

	// Token: 0x17000A26 RID: 2598
	// (get) Token: 0x06006283 RID: 25219 RVA: 0x0026CC27 File Offset: 0x0026AE27
	// (set) Token: 0x06006284 RID: 25220 RVA: 0x0026CC2F File Offset: 0x0026AE2F
	[JsonProperty("user_name")]
	public string UserName { get; set; } = "";

	// Token: 0x17000A27 RID: 2599
	// (get) Token: 0x06006285 RID: 25221 RVA: 0x0026CC38 File Offset: 0x0026AE38
	// (set) Token: 0x06006286 RID: 25222 RVA: 0x0026CC40 File Offset: 0x0026AE40
	[JsonProperty("reward")]
	public Reward Reward { get; set; } = new Reward();
}
