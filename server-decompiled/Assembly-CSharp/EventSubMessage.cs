using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

// Token: 0x02000C99 RID: 3225
[Serializable]
public class EventSubMessage
{
	// Token: 0x17000A47 RID: 2631
	// (get) Token: 0x0600629F RID: 25247 RVA: 0x00270A2B File Offset: 0x0026EC2B
	// (set) Token: 0x060062A0 RID: 25248 RVA: 0x00270A33 File Offset: 0x0026EC33
	[JsonProperty("metadata")]
	public EventSubMetadata Metadata { get; set; } = new EventSubMetadata();

	// Token: 0x17000A48 RID: 2632
	// (get) Token: 0x060062A1 RID: 25249 RVA: 0x00270A3C File Offset: 0x0026EC3C
	// (set) Token: 0x060062A2 RID: 25250 RVA: 0x00270A44 File Offset: 0x0026EC44
	[JsonProperty("payload")]
	public JObject Payload { get; set; }
}
