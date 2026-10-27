using System;
using Newtonsoft.Json;

// Token: 0x02000CA7 RID: 3239
public class RaidEvent
{
	// Token: 0x17000A53 RID: 2643
	// (get) Token: 0x060062D3 RID: 25299 RVA: 0x00271FF5 File Offset: 0x002701F5
	// (set) Token: 0x060062D4 RID: 25300 RVA: 0x00271FFD File Offset: 0x002701FD
	[JsonProperty("from_broadcaster_user_id")]
	public string RaiderID { get; set; }

	// Token: 0x17000A54 RID: 2644
	// (get) Token: 0x060062D5 RID: 25301 RVA: 0x00272006 File Offset: 0x00270206
	// (set) Token: 0x060062D6 RID: 25302 RVA: 0x0027200E File Offset: 0x0027020E
	[JsonProperty("from_broadcaster_user_login")]
	public string RaiderUserName { get; set; }

	// Token: 0x17000A55 RID: 2645
	// (get) Token: 0x060062D7 RID: 25303 RVA: 0x00272017 File Offset: 0x00270217
	// (set) Token: 0x060062D8 RID: 25304 RVA: 0x0027201F File Offset: 0x0027021F
	[JsonProperty("viewers")]
	public int viewerCount { get; set; }
}
