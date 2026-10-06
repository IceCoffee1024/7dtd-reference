using System;
using Newtonsoft.Json;

// Token: 0x02000C9D RID: 3229
public class SubscriptionEvent : SubscriptionEventBase
{
	// Token: 0x17000A50 RID: 2640
	// (get) Token: 0x060062B5 RID: 25269 RVA: 0x00270B00 File Offset: 0x0026ED00
	// (set) Token: 0x060062B6 RID: 25270 RVA: 0x00270B08 File Offset: 0x0026ED08
	[JsonProperty("is_gift")]
	public bool IsGift { get; set; }
}
