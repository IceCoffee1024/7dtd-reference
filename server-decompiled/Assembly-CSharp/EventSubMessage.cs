using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

// Token: 0x02000CA4 RID: 3236
[Serializable]
public class EventSubMessage
{
	// Token: 0x17000A4D RID: 2637
	// (get) Token: 0x060062C4 RID: 25284 RVA: 0x00271F53 File Offset: 0x00270153
	// (set) Token: 0x060062C5 RID: 25285 RVA: 0x00271F5B File Offset: 0x0027015B
	[JsonProperty("metadata")]
	public EventSubMetadata Metadata { get; set; } = new EventSubMetadata();

	// Token: 0x17000A4E RID: 2638
	// (get) Token: 0x060062C6 RID: 25286 RVA: 0x00271F64 File Offset: 0x00270164
	// (set) Token: 0x060062C7 RID: 25287 RVA: 0x00271F6C File Offset: 0x0027016C
	[JsonProperty("payload")]
	public JObject Payload { get; set; }
}
