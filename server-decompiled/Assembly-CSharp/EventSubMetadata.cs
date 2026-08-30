using System;
using Newtonsoft.Json;

// Token: 0x02000C7A RID: 3194
[Serializable]
public class EventSubMetadata
{
	// Token: 0x17000A20 RID: 2592
	// (get) Token: 0x06006226 RID: 25126 RVA: 0x0026A744 File Offset: 0x00268944
	// (set) Token: 0x06006227 RID: 25127 RVA: 0x0026A74C File Offset: 0x0026894C
	[JsonProperty("message_id")]
	public string MessageId { get; set; } = string.Empty;

	// Token: 0x17000A21 RID: 2593
	// (get) Token: 0x06006228 RID: 25128 RVA: 0x0026A755 File Offset: 0x00268955
	// (set) Token: 0x06006229 RID: 25129 RVA: 0x0026A75D File Offset: 0x0026895D
	[JsonProperty("message_type")]
	public string MessageType { get; set; } = string.Empty;

	// Token: 0x17000A22 RID: 2594
	// (get) Token: 0x0600622A RID: 25130 RVA: 0x0026A766 File Offset: 0x00268966
	// (set) Token: 0x0600622B RID: 25131 RVA: 0x0026A76E File Offset: 0x0026896E
	[JsonProperty("message_timestamp")]
	public string MessageTimestamp { get; set; } = string.Empty;
}
