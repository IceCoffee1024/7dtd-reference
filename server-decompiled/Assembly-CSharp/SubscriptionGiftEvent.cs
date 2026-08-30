using System;
using Newtonsoft.Json;

// Token: 0x02000C91 RID: 3217
public class SubscriptionGiftEvent : SubscriptionEventBase
{
	// Token: 0x17000A3F RID: 2623
	// (get) Token: 0x060062CE RID: 25294 RVA: 0x0026D74D File Offset: 0x0026B94D
	// (set) Token: 0x060062CF RID: 25295 RVA: 0x0026D755 File Offset: 0x0026B955
	[JsonProperty("is_anonymous")]
	public bool IsAnonymous { get; set; }

	// Token: 0x17000A40 RID: 2624
	// (get) Token: 0x060062D0 RID: 25296 RVA: 0x0026D75E File Offset: 0x0026B95E
	// (set) Token: 0x060062D1 RID: 25297 RVA: 0x0026D766 File Offset: 0x0026B966
	[JsonProperty("total")]
	public int Total { get; set; }
}
