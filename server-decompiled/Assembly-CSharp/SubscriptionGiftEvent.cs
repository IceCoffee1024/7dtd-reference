using System;
using Newtonsoft.Json;

// Token: 0x02000CAA RID: 3242
public class SubscriptionGiftEvent : SubscriptionEventBase
{
	// Token: 0x17000A5B RID: 2651
	// (get) Token: 0x060062E6 RID: 25318 RVA: 0x002720B9 File Offset: 0x002702B9
	// (set) Token: 0x060062E7 RID: 25319 RVA: 0x002720C1 File Offset: 0x002702C1
	[JsonProperty("is_anonymous")]
	public bool IsAnonymous { get; set; }

	// Token: 0x17000A5C RID: 2652
	// (get) Token: 0x060062E8 RID: 25320 RVA: 0x002720CA File Offset: 0x002702CA
	// (set) Token: 0x060062E9 RID: 25321 RVA: 0x002720D2 File Offset: 0x002702D2
	[JsonProperty("total")]
	public int Total { get; set; }
}
