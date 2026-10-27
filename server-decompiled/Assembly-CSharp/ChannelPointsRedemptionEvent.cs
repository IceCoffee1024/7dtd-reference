using System;
using Newtonsoft.Json;

// Token: 0x02000C9E RID: 3230
public class ChannelPointsRedemptionEvent
{
	// Token: 0x17000A40 RID: 2624
	// (get) Token: 0x06006297 RID: 25239 RVA: 0x00271571 File Offset: 0x0026F771
	// (set) Token: 0x06006298 RID: 25240 RVA: 0x00271579 File Offset: 0x0026F779
	[JsonProperty("user_id")]
	public string UserId { get; set; } = "";

	// Token: 0x17000A41 RID: 2625
	// (get) Token: 0x06006299 RID: 25241 RVA: 0x00271582 File Offset: 0x0026F782
	// (set) Token: 0x0600629A RID: 25242 RVA: 0x0027158A File Offset: 0x0026F78A
	[JsonProperty("user_login")]
	public string UserLogin { get; set; } = "";

	// Token: 0x17000A42 RID: 2626
	// (get) Token: 0x0600629B RID: 25243 RVA: 0x00271593 File Offset: 0x0026F793
	// (set) Token: 0x0600629C RID: 25244 RVA: 0x0027159B File Offset: 0x0026F79B
	[JsonProperty("user_name")]
	public string UserName { get; set; } = "";

	// Token: 0x17000A43 RID: 2627
	// (get) Token: 0x0600629D RID: 25245 RVA: 0x002715A4 File Offset: 0x0026F7A4
	// (set) Token: 0x0600629E RID: 25246 RVA: 0x002715AC File Offset: 0x0026F7AC
	[JsonProperty("reward")]
	public Reward Reward { get; set; } = new Reward();
}
