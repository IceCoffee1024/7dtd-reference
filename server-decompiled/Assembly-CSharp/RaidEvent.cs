using System;
using Newtonsoft.Json;

// Token: 0x02000C8E RID: 3214
public class RaidEvent
{
	// Token: 0x17000A37 RID: 2615
	// (get) Token: 0x060062BB RID: 25275 RVA: 0x0026D689 File Offset: 0x0026B889
	// (set) Token: 0x060062BC RID: 25276 RVA: 0x0026D691 File Offset: 0x0026B891
	[JsonProperty("from_broadcaster_user_id")]
	public string RaiderID { get; set; }

	// Token: 0x17000A38 RID: 2616
	// (get) Token: 0x060062BD RID: 25277 RVA: 0x0026D69A File Offset: 0x0026B89A
	// (set) Token: 0x060062BE RID: 25278 RVA: 0x0026D6A2 File Offset: 0x0026B8A2
	[JsonProperty("from_broadcaster_user_login")]
	public string RaiderUserName { get; set; }

	// Token: 0x17000A39 RID: 2617
	// (get) Token: 0x060062BF RID: 25279 RVA: 0x0026D6AB File Offset: 0x0026B8AB
	// (set) Token: 0x060062C0 RID: 25280 RVA: 0x0026D6B3 File Offset: 0x0026B8B3
	[JsonProperty("viewers")]
	public int viewerCount { get; set; }
}
