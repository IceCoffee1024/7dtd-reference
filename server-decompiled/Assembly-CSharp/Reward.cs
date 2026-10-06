using System;
using Newtonsoft.Json;

// Token: 0x02000C94 RID: 3220
public class Reward
{
	// Token: 0x17000A3E RID: 2622
	// (get) Token: 0x0600627B RID: 25211 RVA: 0x002700C1 File Offset: 0x0026E2C1
	// (set) Token: 0x0600627C RID: 25212 RVA: 0x002700C9 File Offset: 0x0026E2C9
	[JsonProperty("id")]
	public string Id { get; set; } = "";

	// Token: 0x17000A3F RID: 2623
	// (get) Token: 0x0600627D RID: 25213 RVA: 0x002700D2 File Offset: 0x0026E2D2
	// (set) Token: 0x0600627E RID: 25214 RVA: 0x002700DA File Offset: 0x0026E2DA
	[JsonProperty("title")]
	public string Title { get; set; } = "";

	// Token: 0x17000A40 RID: 2624
	// (get) Token: 0x0600627F RID: 25215 RVA: 0x002700E3 File Offset: 0x0026E2E3
	// (set) Token: 0x06006280 RID: 25216 RVA: 0x002700EB File Offset: 0x0026E2EB
	[JsonProperty("cost")]
	public int Cost { get; set; }
}
