using System;
using Newtonsoft.Json;

// Token: 0x02000C9F RID: 3231
public class Reward
{
	// Token: 0x17000A44 RID: 2628
	// (get) Token: 0x060062A0 RID: 25248 RVA: 0x002715E9 File Offset: 0x0026F7E9
	// (set) Token: 0x060062A1 RID: 25249 RVA: 0x002715F1 File Offset: 0x0026F7F1
	[JsonProperty("id")]
	public string Id { get; set; } = "";

	// Token: 0x17000A45 RID: 2629
	// (get) Token: 0x060062A2 RID: 25250 RVA: 0x002715FA File Offset: 0x0026F7FA
	// (set) Token: 0x060062A3 RID: 25251 RVA: 0x00271602 File Offset: 0x0026F802
	[JsonProperty("title")]
	public string Title { get; set; } = "";

	// Token: 0x17000A46 RID: 2630
	// (get) Token: 0x060062A4 RID: 25252 RVA: 0x0027160B File Offset: 0x0026F80B
	// (set) Token: 0x060062A5 RID: 25253 RVA: 0x00271613 File Offset: 0x0026F813
	[JsonProperty("cost")]
	public int Cost { get; set; }
}
