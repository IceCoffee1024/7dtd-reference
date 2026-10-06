using System;
using Newtonsoft.Json;

// Token: 0x02000C9F RID: 3231
public class SubscriptionGiftEvent : SubscriptionEventBase
{
	// Token: 0x17000A55 RID: 2645
	// (get) Token: 0x060062C1 RID: 25281 RVA: 0x00270B91 File Offset: 0x0026ED91
	// (set) Token: 0x060062C2 RID: 25282 RVA: 0x00270B99 File Offset: 0x0026ED99
	[JsonProperty("is_anonymous")]
	public bool IsAnonymous { get; set; }

	// Token: 0x17000A56 RID: 2646
	// (get) Token: 0x060062C3 RID: 25283 RVA: 0x00270BA2 File Offset: 0x0026EDA2
	// (set) Token: 0x060062C4 RID: 25284 RVA: 0x00270BAA File Offset: 0x0026EDAA
	[JsonProperty("total")]
	public int Total { get; set; }
}
