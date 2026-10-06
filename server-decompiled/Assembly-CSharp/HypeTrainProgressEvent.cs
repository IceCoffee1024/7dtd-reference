using System;
using Newtonsoft.Json;

// Token: 0x02000C9B RID: 3227
public class HypeTrainProgressEvent
{
	// Token: 0x17000A4C RID: 2636
	// (get) Token: 0x060062AB RID: 25259 RVA: 0x00270ABC File Offset: 0x0026ECBC
	// (set) Token: 0x060062AC RID: 25260 RVA: 0x00270AC4 File Offset: 0x0026ECC4
	[JsonProperty("level")]
	public int Level { get; set; }
}
