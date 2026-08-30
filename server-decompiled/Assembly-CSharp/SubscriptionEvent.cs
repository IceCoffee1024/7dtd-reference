using System;
using Newtonsoft.Json;

// Token: 0x02000C8F RID: 3215
public class SubscriptionEvent : SubscriptionEventBase
{
	// Token: 0x17000A3A RID: 2618
	// (get) Token: 0x060062C2 RID: 25282 RVA: 0x0026D6BC File Offset: 0x0026B8BC
	// (set) Token: 0x060062C3 RID: 25283 RVA: 0x0026D6C4 File Offset: 0x0026B8C4
	[JsonProperty("is_gift")]
	public bool IsGift { get; set; }
}
