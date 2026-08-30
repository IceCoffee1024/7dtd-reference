using System;
using Newtonsoft.Json;

// Token: 0x02000C7F RID: 3199
public class SubscriptionGiftEvent : SubscriptionEventBase
{
	// Token: 0x17000A2C RID: 2604
	// (get) Token: 0x06006243 RID: 25155 RVA: 0x0026A875 File Offset: 0x00268A75
	// (set) Token: 0x06006244 RID: 25156 RVA: 0x0026A87D File Offset: 0x00268A7D
	[JsonProperty("is_anonymous")]
	public bool IsAnonymous { get; set; }

	// Token: 0x17000A2D RID: 2605
	// (get) Token: 0x06006245 RID: 25157 RVA: 0x0026A886 File Offset: 0x00268A86
	// (set) Token: 0x06006246 RID: 25158 RVA: 0x0026A88E File Offset: 0x00268A8E
	[JsonProperty("total")]
	public int Total { get; set; }
}
