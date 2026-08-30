using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

// Token: 0x02000C79 RID: 3193
[Serializable]
public class EventSubMessage
{
	// Token: 0x17000A1E RID: 2590
	// (get) Token: 0x06006221 RID: 25121 RVA: 0x0026A70F File Offset: 0x0026890F
	// (set) Token: 0x06006222 RID: 25122 RVA: 0x0026A717 File Offset: 0x00268917
	[JsonProperty("metadata")]
	public EventSubMetadata Metadata { get; set; } = new EventSubMetadata();

	// Token: 0x17000A1F RID: 2591
	// (get) Token: 0x06006223 RID: 25123 RVA: 0x0026A720 File Offset: 0x00268920
	// (set) Token: 0x06006224 RID: 25124 RVA: 0x0026A728 File Offset: 0x00268928
	[JsonProperty("payload")]
	public JObject Payload { get; set; }
}
