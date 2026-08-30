using System;
using Newtonsoft.Json;

// Token: 0x02000C7D RID: 3197
public class SubscriptionEvent : SubscriptionEventBase
{
	// Token: 0x17000A27 RID: 2599
	// (get) Token: 0x06006237 RID: 25143 RVA: 0x0026A7E4 File Offset: 0x002689E4
	// (set) Token: 0x06006238 RID: 25144 RVA: 0x0026A7EC File Offset: 0x002689EC
	[JsonProperty("is_gift")]
	public bool IsGift { get; set; }
}
