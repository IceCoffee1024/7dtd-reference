using System;
using Newtonsoft.Json;

// Token: 0x02000C73 RID: 3187
public class ChannelPointsRedemptionEvent
{
	// Token: 0x17000A11 RID: 2577
	// (get) Token: 0x060061F4 RID: 25076 RVA: 0x00269D2D File Offset: 0x00267F2D
	// (set) Token: 0x060061F5 RID: 25077 RVA: 0x00269D35 File Offset: 0x00267F35
	[JsonProperty("user_id")]
	public string UserId { get; set; } = "";

	// Token: 0x17000A12 RID: 2578
	// (get) Token: 0x060061F6 RID: 25078 RVA: 0x00269D3E File Offset: 0x00267F3E
	// (set) Token: 0x060061F7 RID: 25079 RVA: 0x00269D46 File Offset: 0x00267F46
	[JsonProperty("user_login")]
	public string UserLogin { get; set; } = "";

	// Token: 0x17000A13 RID: 2579
	// (get) Token: 0x060061F8 RID: 25080 RVA: 0x00269D4F File Offset: 0x00267F4F
	// (set) Token: 0x060061F9 RID: 25081 RVA: 0x00269D57 File Offset: 0x00267F57
	[JsonProperty("user_name")]
	public string UserName { get; set; } = "";

	// Token: 0x17000A14 RID: 2580
	// (get) Token: 0x060061FA RID: 25082 RVA: 0x00269D60 File Offset: 0x00267F60
	// (set) Token: 0x060061FB RID: 25083 RVA: 0x00269D68 File Offset: 0x00267F68
	[JsonProperty("reward")]
	public Reward Reward { get; set; } = new Reward();
}
