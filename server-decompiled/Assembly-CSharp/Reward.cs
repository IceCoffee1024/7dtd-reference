using System;
using Newtonsoft.Json;

// Token: 0x02000C86 RID: 3206
public class Reward
{
	// Token: 0x17000A28 RID: 2600
	// (get) Token: 0x06006288 RID: 25224 RVA: 0x0026CC7D File Offset: 0x0026AE7D
	// (set) Token: 0x06006289 RID: 25225 RVA: 0x0026CC85 File Offset: 0x0026AE85
	[JsonProperty("id")]
	public string Id { get; set; } = "";

	// Token: 0x17000A29 RID: 2601
	// (get) Token: 0x0600628A RID: 25226 RVA: 0x0026CC8E File Offset: 0x0026AE8E
	// (set) Token: 0x0600628B RID: 25227 RVA: 0x0026CC96 File Offset: 0x0026AE96
	[JsonProperty("title")]
	public string Title { get; set; } = "";

	// Token: 0x17000A2A RID: 2602
	// (get) Token: 0x0600628C RID: 25228 RVA: 0x0026CC9F File Offset: 0x0026AE9F
	// (set) Token: 0x0600628D RID: 25229 RVA: 0x0026CCA7 File Offset: 0x0026AEA7
	[JsonProperty("cost")]
	public int Cost { get; set; }
}
