using System;
using Newtonsoft.Json;

// Token: 0x02000CA6 RID: 3238
public class HypeTrainProgressEvent
{
	// Token: 0x17000A52 RID: 2642
	// (get) Token: 0x060062D0 RID: 25296 RVA: 0x00271FE4 File Offset: 0x002701E4
	// (set) Token: 0x060062D1 RID: 25297 RVA: 0x00271FEC File Offset: 0x002701EC
	[JsonProperty("level")]
	public int Level { get; set; }
}
