using System;
using Newtonsoft.Json;

// Token: 0x02000C8C RID: 3212
[Serializable]
public class EventSubMetadata
{
	// Token: 0x17000A33 RID: 2611
	// (get) Token: 0x060062B1 RID: 25265 RVA: 0x0026D61C File Offset: 0x0026B81C
	// (set) Token: 0x060062B2 RID: 25266 RVA: 0x0026D624 File Offset: 0x0026B824
	[JsonProperty("message_id")]
	public string MessageId { get; set; } = string.Empty;

	// Token: 0x17000A34 RID: 2612
	// (get) Token: 0x060062B3 RID: 25267 RVA: 0x0026D62D File Offset: 0x0026B82D
	// (set) Token: 0x060062B4 RID: 25268 RVA: 0x0026D635 File Offset: 0x0026B835
	[JsonProperty("message_type")]
	public string MessageType { get; set; } = string.Empty;

	// Token: 0x17000A35 RID: 2613
	// (get) Token: 0x060062B5 RID: 25269 RVA: 0x0026D63E File Offset: 0x0026B83E
	// (set) Token: 0x060062B6 RID: 25270 RVA: 0x0026D646 File Offset: 0x0026B846
	[JsonProperty("message_timestamp")]
	public string MessageTimestamp { get; set; } = string.Empty;
}
