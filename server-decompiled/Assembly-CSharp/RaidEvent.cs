using System;
using Newtonsoft.Json;

// Token: 0x02000C7C RID: 3196
public class RaidEvent
{
	// Token: 0x17000A24 RID: 2596
	// (get) Token: 0x06006230 RID: 25136 RVA: 0x0026A7B1 File Offset: 0x002689B1
	// (set) Token: 0x06006231 RID: 25137 RVA: 0x0026A7B9 File Offset: 0x002689B9
	[JsonProperty("from_broadcaster_user_id")]
	public string RaiderID { get; set; }

	// Token: 0x17000A25 RID: 2597
	// (get) Token: 0x06006232 RID: 25138 RVA: 0x0026A7C2 File Offset: 0x002689C2
	// (set) Token: 0x06006233 RID: 25139 RVA: 0x0026A7CA File Offset: 0x002689CA
	[JsonProperty("from_broadcaster_user_login")]
	public string RaiderUserName { get; set; }

	// Token: 0x17000A26 RID: 2598
	// (get) Token: 0x06006234 RID: 25140 RVA: 0x0026A7D3 File Offset: 0x002689D3
	// (set) Token: 0x06006235 RID: 25141 RVA: 0x0026A7DB File Offset: 0x002689DB
	[JsonProperty("viewers")]
	public int viewerCount { get; set; }
}
