using System;
using Newtonsoft.Json;

// Token: 0x02000C84 RID: 3204
public class BitsUsedEvent
{
	// Token: 0x17000A1C RID: 2588
	// (get) Token: 0x0600626E RID: 25198 RVA: 0x0026CB28 File Offset: 0x0026AD28
	// (set) Token: 0x0600626F RID: 25199 RVA: 0x0026CB30 File Offset: 0x0026AD30
	[JsonProperty("is_anonymous")]
	public bool IsAnonymous { get; set; }

	// Token: 0x17000A1D RID: 2589
	// (get) Token: 0x06006270 RID: 25200 RVA: 0x0026CB39 File Offset: 0x0026AD39
	// (set) Token: 0x06006271 RID: 25201 RVA: 0x0026CB41 File Offset: 0x0026AD41
	[JsonProperty("user_id")]
	public string UserId { get; set; } = "";

	// Token: 0x17000A1E RID: 2590
	// (get) Token: 0x06006272 RID: 25202 RVA: 0x0026CB4A File Offset: 0x0026AD4A
	// (set) Token: 0x06006273 RID: 25203 RVA: 0x0026CB52 File Offset: 0x0026AD52
	[JsonProperty("user_login")]
	public string UserLogin { get; set; } = "";

	// Token: 0x17000A1F RID: 2591
	// (get) Token: 0x06006274 RID: 25204 RVA: 0x0026CB5B File Offset: 0x0026AD5B
	// (set) Token: 0x06006275 RID: 25205 RVA: 0x0026CB63 File Offset: 0x0026AD63
	[JsonProperty("user_name")]
	public string UserName { get; set; } = "";

	// Token: 0x17000A20 RID: 2592
	// (get) Token: 0x06006276 RID: 25206 RVA: 0x0026CB6C File Offset: 0x0026AD6C
	// (set) Token: 0x06006277 RID: 25207 RVA: 0x0026CB74 File Offset: 0x0026AD74
	[JsonProperty("broadcaster_user_id")]
	public string BroadcasterUserId { get; set; } = "";

	// Token: 0x17000A21 RID: 2593
	// (get) Token: 0x06006278 RID: 25208 RVA: 0x0026CB7D File Offset: 0x0026AD7D
	// (set) Token: 0x06006279 RID: 25209 RVA: 0x0026CB85 File Offset: 0x0026AD85
	[JsonProperty("broadcaster_user_login")]
	public string BroadcasterUserLogin { get; set; } = "";

	// Token: 0x17000A22 RID: 2594
	// (get) Token: 0x0600627A RID: 25210 RVA: 0x0026CB8E File Offset: 0x0026AD8E
	// (set) Token: 0x0600627B RID: 25211 RVA: 0x0026CB96 File Offset: 0x0026AD96
	[JsonProperty("broadcaster_user_name")]
	public string BroadcasterUserName { get; set; } = "";

	// Token: 0x17000A23 RID: 2595
	// (get) Token: 0x0600627C RID: 25212 RVA: 0x0026CB9F File Offset: 0x0026AD9F
	// (set) Token: 0x0600627D RID: 25213 RVA: 0x0026CBA7 File Offset: 0x0026ADA7
	[JsonProperty("bits")]
	public int Bits { get; set; }
}
