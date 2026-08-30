using System;
using Newtonsoft.Json;

// Token: 0x02000C7B RID: 3195
public class HypeTrainProgressEvent
{
	// Token: 0x17000A23 RID: 2595
	// (get) Token: 0x0600622D RID: 25133 RVA: 0x0026A7A0 File Offset: 0x002689A0
	// (set) Token: 0x0600622E RID: 25134 RVA: 0x0026A7A8 File Offset: 0x002689A8
	[JsonProperty("level")]
	public int Level { get; set; }
}
