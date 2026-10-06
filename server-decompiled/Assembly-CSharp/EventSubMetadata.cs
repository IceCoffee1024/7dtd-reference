using System;
using Newtonsoft.Json;

// Token: 0x02000C9A RID: 3226
[Serializable]
public class EventSubMetadata
{
	// Token: 0x17000A49 RID: 2633
	// (get) Token: 0x060062A4 RID: 25252 RVA: 0x00270A60 File Offset: 0x0026EC60
	// (set) Token: 0x060062A5 RID: 25253 RVA: 0x00270A68 File Offset: 0x0026EC68
	[JsonProperty("message_id")]
	public string MessageId { get; set; } = string.Empty;

	// Token: 0x17000A4A RID: 2634
	// (get) Token: 0x060062A6 RID: 25254 RVA: 0x00270A71 File Offset: 0x0026EC71
	// (set) Token: 0x060062A7 RID: 25255 RVA: 0x00270A79 File Offset: 0x0026EC79
	[JsonProperty("message_type")]
	public string MessageType { get; set; } = string.Empty;

	// Token: 0x17000A4B RID: 2635
	// (get) Token: 0x060062A8 RID: 25256 RVA: 0x00270A82 File Offset: 0x0026EC82
	// (set) Token: 0x060062A9 RID: 25257 RVA: 0x00270A8A File Offset: 0x0026EC8A
	[JsonProperty("message_timestamp")]
	public string MessageTimestamp { get; set; } = string.Empty;
}
