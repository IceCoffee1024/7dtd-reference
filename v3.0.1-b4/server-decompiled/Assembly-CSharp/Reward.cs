using System;
using Newtonsoft.Json;

// Token: 0x02000C74 RID: 3188
public class Reward
{
	// Token: 0x17000A15 RID: 2581
	// (get) Token: 0x060061FD RID: 25085 RVA: 0x00269DA5 File Offset: 0x00267FA5
	// (set) Token: 0x060061FE RID: 25086 RVA: 0x00269DAD File Offset: 0x00267FAD
	[JsonProperty("id")]
	public string Id { get; set; } = "";

	// Token: 0x17000A16 RID: 2582
	// (get) Token: 0x060061FF RID: 25087 RVA: 0x00269DB6 File Offset: 0x00267FB6
	// (set) Token: 0x06006200 RID: 25088 RVA: 0x00269DBE File Offset: 0x00267FBE
	[JsonProperty("title")]
	public string Title { get; set; } = "";

	// Token: 0x17000A17 RID: 2583
	// (get) Token: 0x06006201 RID: 25089 RVA: 0x00269DC7 File Offset: 0x00267FC7
	// (set) Token: 0x06006202 RID: 25090 RVA: 0x00269DCF File Offset: 0x00267FCF
	[JsonProperty("cost")]
	public int Cost { get; set; }
}
