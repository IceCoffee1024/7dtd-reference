using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

// Token: 0x02000C8B RID: 3211
[Serializable]
public class EventSubMessage
{
	// Token: 0x17000A31 RID: 2609
	// (get) Token: 0x060062AC RID: 25260 RVA: 0x0026D5E7 File Offset: 0x0026B7E7
	// (set) Token: 0x060062AD RID: 25261 RVA: 0x0026D5EF File Offset: 0x0026B7EF
	[JsonProperty("metadata")]
	public EventSubMetadata Metadata { get; set; } = new EventSubMetadata();

	// Token: 0x17000A32 RID: 2610
	// (get) Token: 0x060062AE RID: 25262 RVA: 0x0026D5F8 File Offset: 0x0026B7F8
	// (set) Token: 0x060062AF RID: 25263 RVA: 0x0026D600 File Offset: 0x0026B800
	[JsonProperty("payload")]
	public JObject Payload { get; set; }
}
