using System;
using Newtonsoft.Json;

// Token: 0x02000CA5 RID: 3237
[Serializable]
public class EventSubMetadata
{
	// Token: 0x17000A4F RID: 2639
	// (get) Token: 0x060062C9 RID: 25289 RVA: 0x00271F88 File Offset: 0x00270188
	// (set) Token: 0x060062CA RID: 25290 RVA: 0x00271F90 File Offset: 0x00270190
	[JsonProperty("message_id")]
	public string MessageId { get; set; } = string.Empty;

	// Token: 0x17000A50 RID: 2640
	// (get) Token: 0x060062CB RID: 25291 RVA: 0x00271F99 File Offset: 0x00270199
	// (set) Token: 0x060062CC RID: 25292 RVA: 0x00271FA1 File Offset: 0x002701A1
	[JsonProperty("message_type")]
	public string MessageType { get; set; } = string.Empty;

	// Token: 0x17000A51 RID: 2641
	// (get) Token: 0x060062CD RID: 25293 RVA: 0x00271FAA File Offset: 0x002701AA
	// (set) Token: 0x060062CE RID: 25294 RVA: 0x00271FB2 File Offset: 0x002701B2
	[JsonProperty("message_timestamp")]
	public string MessageTimestamp { get; set; } = string.Empty;
}
