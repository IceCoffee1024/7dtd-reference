using System;
using Newtonsoft.Json;

// Token: 0x02000C92 RID: 3218
public class BitsUsedEvent
{
	// Token: 0x17000A32 RID: 2610
	// (get) Token: 0x06006261 RID: 25185 RVA: 0x0026FF6C File Offset: 0x0026E16C
	// (set) Token: 0x06006262 RID: 25186 RVA: 0x0026FF74 File Offset: 0x0026E174
	[JsonProperty("is_anonymous")]
	public bool IsAnonymous { get; set; }

	// Token: 0x17000A33 RID: 2611
	// (get) Token: 0x06006263 RID: 25187 RVA: 0x0026FF7D File Offset: 0x0026E17D
	// (set) Token: 0x06006264 RID: 25188 RVA: 0x0026FF85 File Offset: 0x0026E185
	[JsonProperty("user_id")]
	public string UserId { get; set; } = "";

	// Token: 0x17000A34 RID: 2612
	// (get) Token: 0x06006265 RID: 25189 RVA: 0x0026FF8E File Offset: 0x0026E18E
	// (set) Token: 0x06006266 RID: 25190 RVA: 0x0026FF96 File Offset: 0x0026E196
	[JsonProperty("user_login")]
	public string UserLogin { get; set; } = "";

	// Token: 0x17000A35 RID: 2613
	// (get) Token: 0x06006267 RID: 25191 RVA: 0x0026FF9F File Offset: 0x0026E19F
	// (set) Token: 0x06006268 RID: 25192 RVA: 0x0026FFA7 File Offset: 0x0026E1A7
	[JsonProperty("user_name")]
	public string UserName { get; set; } = "";

	// Token: 0x17000A36 RID: 2614
	// (get) Token: 0x06006269 RID: 25193 RVA: 0x0026FFB0 File Offset: 0x0026E1B0
	// (set) Token: 0x0600626A RID: 25194 RVA: 0x0026FFB8 File Offset: 0x0026E1B8
	[JsonProperty("broadcaster_user_id")]
	public string BroadcasterUserId { get; set; } = "";

	// Token: 0x17000A37 RID: 2615
	// (get) Token: 0x0600626B RID: 25195 RVA: 0x0026FFC1 File Offset: 0x0026E1C1
	// (set) Token: 0x0600626C RID: 25196 RVA: 0x0026FFC9 File Offset: 0x0026E1C9
	[JsonProperty("broadcaster_user_login")]
	public string BroadcasterUserLogin { get; set; } = "";

	// Token: 0x17000A38 RID: 2616
	// (get) Token: 0x0600626D RID: 25197 RVA: 0x0026FFD2 File Offset: 0x0026E1D2
	// (set) Token: 0x0600626E RID: 25198 RVA: 0x0026FFDA File Offset: 0x0026E1DA
	[JsonProperty("broadcaster_user_name")]
	public string BroadcasterUserName { get; set; } = "";

	// Token: 0x17000A39 RID: 2617
	// (get) Token: 0x0600626F RID: 25199 RVA: 0x0026FFE3 File Offset: 0x0026E1E3
	// (set) Token: 0x06006270 RID: 25200 RVA: 0x0026FFEB File Offset: 0x0026E1EB
	[JsonProperty("bits")]
	public int Bits { get; set; }
}
