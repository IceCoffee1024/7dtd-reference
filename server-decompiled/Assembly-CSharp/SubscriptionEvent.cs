using System;
using Newtonsoft.Json;

// Token: 0x02000CA8 RID: 3240
public class SubscriptionEvent : SubscriptionEventBase
{
	// Token: 0x17000A56 RID: 2646
	// (get) Token: 0x060062DA RID: 25306 RVA: 0x00272028 File Offset: 0x00270228
	// (set) Token: 0x060062DB RID: 25307 RVA: 0x00272030 File Offset: 0x00270230
	[JsonProperty("is_gift")]
	public bool IsGift { get; set; }
}
