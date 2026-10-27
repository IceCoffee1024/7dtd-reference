using System;
using Newtonsoft.Json;

// Token: 0x02000C9D RID: 3229
public class BitsUsedEvent
{
	// Token: 0x17000A38 RID: 2616
	// (get) Token: 0x06006286 RID: 25222 RVA: 0x00271494 File Offset: 0x0026F694
	// (set) Token: 0x06006287 RID: 25223 RVA: 0x0027149C File Offset: 0x0026F69C
	[JsonProperty("is_anonymous")]
	public bool IsAnonymous { get; set; }

	// Token: 0x17000A39 RID: 2617
	// (get) Token: 0x06006288 RID: 25224 RVA: 0x002714A5 File Offset: 0x0026F6A5
	// (set) Token: 0x06006289 RID: 25225 RVA: 0x002714AD File Offset: 0x0026F6AD
	[JsonProperty("user_id")]
	public string UserId { get; set; } = "";

	// Token: 0x17000A3A RID: 2618
	// (get) Token: 0x0600628A RID: 25226 RVA: 0x002714B6 File Offset: 0x0026F6B6
	// (set) Token: 0x0600628B RID: 25227 RVA: 0x002714BE File Offset: 0x0026F6BE
	[JsonProperty("user_login")]
	public string UserLogin { get; set; } = "";

	// Token: 0x17000A3B RID: 2619
	// (get) Token: 0x0600628C RID: 25228 RVA: 0x002714C7 File Offset: 0x0026F6C7
	// (set) Token: 0x0600628D RID: 25229 RVA: 0x002714CF File Offset: 0x0026F6CF
	[JsonProperty("user_name")]
	public string UserName { get; set; } = "";

	// Token: 0x17000A3C RID: 2620
	// (get) Token: 0x0600628E RID: 25230 RVA: 0x002714D8 File Offset: 0x0026F6D8
	// (set) Token: 0x0600628F RID: 25231 RVA: 0x002714E0 File Offset: 0x0026F6E0
	[JsonProperty("broadcaster_user_id")]
	public string BroadcasterUserId { get; set; } = "";

	// Token: 0x17000A3D RID: 2621
	// (get) Token: 0x06006290 RID: 25232 RVA: 0x002714E9 File Offset: 0x0026F6E9
	// (set) Token: 0x06006291 RID: 25233 RVA: 0x002714F1 File Offset: 0x0026F6F1
	[JsonProperty("broadcaster_user_login")]
	public string BroadcasterUserLogin { get; set; } = "";

	// Token: 0x17000A3E RID: 2622
	// (get) Token: 0x06006292 RID: 25234 RVA: 0x002714FA File Offset: 0x0026F6FA
	// (set) Token: 0x06006293 RID: 25235 RVA: 0x00271502 File Offset: 0x0026F702
	[JsonProperty("broadcaster_user_name")]
	public string BroadcasterUserName { get; set; } = "";

	// Token: 0x17000A3F RID: 2623
	// (get) Token: 0x06006294 RID: 25236 RVA: 0x0027150B File Offset: 0x0026F70B
	// (set) Token: 0x06006295 RID: 25237 RVA: 0x00271513 File Offset: 0x0026F713
	[JsonProperty("bits")]
	public int Bits { get; set; }
}
