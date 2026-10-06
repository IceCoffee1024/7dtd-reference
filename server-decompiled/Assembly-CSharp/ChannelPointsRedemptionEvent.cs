using System;
using Newtonsoft.Json;

// Token: 0x02000C93 RID: 3219
public class ChannelPointsRedemptionEvent
{
	// Token: 0x17000A3A RID: 2618
	// (get) Token: 0x06006272 RID: 25202 RVA: 0x00270049 File Offset: 0x0026E249
	// (set) Token: 0x06006273 RID: 25203 RVA: 0x00270051 File Offset: 0x0026E251
	[JsonProperty("user_id")]
	public string UserId { get; set; } = "";

	// Token: 0x17000A3B RID: 2619
	// (get) Token: 0x06006274 RID: 25204 RVA: 0x0027005A File Offset: 0x0026E25A
	// (set) Token: 0x06006275 RID: 25205 RVA: 0x00270062 File Offset: 0x0026E262
	[JsonProperty("user_login")]
	public string UserLogin { get; set; } = "";

	// Token: 0x17000A3C RID: 2620
	// (get) Token: 0x06006276 RID: 25206 RVA: 0x0027006B File Offset: 0x0026E26B
	// (set) Token: 0x06006277 RID: 25207 RVA: 0x00270073 File Offset: 0x0026E273
	[JsonProperty("user_name")]
	public string UserName { get; set; } = "";

	// Token: 0x17000A3D RID: 2621
	// (get) Token: 0x06006278 RID: 25208 RVA: 0x0027007C File Offset: 0x0026E27C
	// (set) Token: 0x06006279 RID: 25209 RVA: 0x00270084 File Offset: 0x0026E284
	[JsonProperty("reward")]
	public Reward Reward { get; set; } = new Reward();
}
