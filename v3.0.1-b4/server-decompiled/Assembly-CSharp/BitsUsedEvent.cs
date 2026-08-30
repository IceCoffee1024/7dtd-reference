using System;
using Newtonsoft.Json;

// Token: 0x02000C72 RID: 3186
public class BitsUsedEvent
{
	// Token: 0x17000A09 RID: 2569
	// (get) Token: 0x060061E3 RID: 25059 RVA: 0x00269C50 File Offset: 0x00267E50
	// (set) Token: 0x060061E4 RID: 25060 RVA: 0x00269C58 File Offset: 0x00267E58
	[JsonProperty("is_anonymous")]
	public bool IsAnonymous { get; set; }

	// Token: 0x17000A0A RID: 2570
	// (get) Token: 0x060061E5 RID: 25061 RVA: 0x00269C61 File Offset: 0x00267E61
	// (set) Token: 0x060061E6 RID: 25062 RVA: 0x00269C69 File Offset: 0x00267E69
	[JsonProperty("user_id")]
	public string UserId { get; set; } = "";

	// Token: 0x17000A0B RID: 2571
	// (get) Token: 0x060061E7 RID: 25063 RVA: 0x00269C72 File Offset: 0x00267E72
	// (set) Token: 0x060061E8 RID: 25064 RVA: 0x00269C7A File Offset: 0x00267E7A
	[JsonProperty("user_login")]
	public string UserLogin { get; set; } = "";

	// Token: 0x17000A0C RID: 2572
	// (get) Token: 0x060061E9 RID: 25065 RVA: 0x00269C83 File Offset: 0x00267E83
	// (set) Token: 0x060061EA RID: 25066 RVA: 0x00269C8B File Offset: 0x00267E8B
	[JsonProperty("user_name")]
	public string UserName { get; set; } = "";

	// Token: 0x17000A0D RID: 2573
	// (get) Token: 0x060061EB RID: 25067 RVA: 0x00269C94 File Offset: 0x00267E94
	// (set) Token: 0x060061EC RID: 25068 RVA: 0x00269C9C File Offset: 0x00267E9C
	[JsonProperty("broadcaster_user_id")]
	public string BroadcasterUserId { get; set; } = "";

	// Token: 0x17000A0E RID: 2574
	// (get) Token: 0x060061ED RID: 25069 RVA: 0x00269CA5 File Offset: 0x00267EA5
	// (set) Token: 0x060061EE RID: 25070 RVA: 0x00269CAD File Offset: 0x00267EAD
	[JsonProperty("broadcaster_user_login")]
	public string BroadcasterUserLogin { get; set; } = "";

	// Token: 0x17000A0F RID: 2575
	// (get) Token: 0x060061EF RID: 25071 RVA: 0x00269CB6 File Offset: 0x00267EB6
	// (set) Token: 0x060061F0 RID: 25072 RVA: 0x00269CBE File Offset: 0x00267EBE
	[JsonProperty("broadcaster_user_name")]
	public string BroadcasterUserName { get; set; } = "";

	// Token: 0x17000A10 RID: 2576
	// (get) Token: 0x060061F1 RID: 25073 RVA: 0x00269CC7 File Offset: 0x00267EC7
	// (set) Token: 0x060061F2 RID: 25074 RVA: 0x00269CCF File Offset: 0x00267ECF
	[JsonProperty("bits")]
	public int Bits { get; set; }
}
