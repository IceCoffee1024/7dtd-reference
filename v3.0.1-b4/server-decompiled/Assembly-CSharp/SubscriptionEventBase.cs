using System;
using Newtonsoft.Json;

// Token: 0x02000C7E RID: 3198
public class SubscriptionEventBase
{
	// Token: 0x17000A28 RID: 2600
	// (get) Token: 0x0600623A RID: 25146 RVA: 0x0026A7FD File Offset: 0x002689FD
	// (set) Token: 0x0600623B RID: 25147 RVA: 0x0026A805 File Offset: 0x00268A05
	[JsonProperty("user_id")]
	public string UserId { get; set; } = "";

	// Token: 0x17000A29 RID: 2601
	// (get) Token: 0x0600623C RID: 25148 RVA: 0x0026A80E File Offset: 0x00268A0E
	// (set) Token: 0x0600623D RID: 25149 RVA: 0x0026A816 File Offset: 0x00268A16
	[JsonProperty("user_login")]
	public string UserLogin { get; set; } = "";

	// Token: 0x17000A2A RID: 2602
	// (get) Token: 0x0600623E RID: 25150 RVA: 0x0026A81F File Offset: 0x00268A1F
	// (set) Token: 0x0600623F RID: 25151 RVA: 0x0026A827 File Offset: 0x00268A27
	[JsonProperty("user_name")]
	public string UserName { get; set; } = "";

	// Token: 0x17000A2B RID: 2603
	// (get) Token: 0x06006240 RID: 25152 RVA: 0x0026A830 File Offset: 0x00268A30
	// (set) Token: 0x06006241 RID: 25153 RVA: 0x0026A838 File Offset: 0x00268A38
	[JsonProperty("tier")]
	public string Tier { get; set; } = "";
}
