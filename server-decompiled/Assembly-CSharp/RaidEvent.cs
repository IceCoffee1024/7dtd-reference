using System;
using Newtonsoft.Json;

// Token: 0x02000C9C RID: 3228
public class RaidEvent
{
	// Token: 0x17000A4D RID: 2637
	// (get) Token: 0x060062AE RID: 25262 RVA: 0x00270ACD File Offset: 0x0026ECCD
	// (set) Token: 0x060062AF RID: 25263 RVA: 0x00270AD5 File Offset: 0x0026ECD5
	[JsonProperty("from_broadcaster_user_id")]
	public string RaiderID { get; set; }

	// Token: 0x17000A4E RID: 2638
	// (get) Token: 0x060062B0 RID: 25264 RVA: 0x00270ADE File Offset: 0x0026ECDE
	// (set) Token: 0x060062B1 RID: 25265 RVA: 0x00270AE6 File Offset: 0x0026ECE6
	[JsonProperty("from_broadcaster_user_login")]
	public string RaiderUserName { get; set; }

	// Token: 0x17000A4F RID: 2639
	// (get) Token: 0x060062B2 RID: 25266 RVA: 0x00270AEF File Offset: 0x0026ECEF
	// (set) Token: 0x060062B3 RID: 25267 RVA: 0x00270AF7 File Offset: 0x0026ECF7
	[JsonProperty("viewers")]
	public int viewerCount { get; set; }
}
