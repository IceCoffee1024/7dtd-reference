using System;
using UnityEngine.Scripting;

// Token: 0x02000303 RID: 771
[Preserve]
public class BaseResponseEntry
{
	// Token: 0x17000270 RID: 624
	// (get) Token: 0x060015C7 RID: 5575 RVA: 0x00081D48 File Offset: 0x0007FF48
	// (set) Token: 0x060015C8 RID: 5576 RVA: 0x00081D50 File Offset: 0x0007FF50
	public string ID { get; set; }

	// Token: 0x17000271 RID: 625
	// (get) Token: 0x060015C9 RID: 5577 RVA: 0x00081D59 File Offset: 0x0007FF59
	// (set) Token: 0x060015CA RID: 5578 RVA: 0x00081D61 File Offset: 0x0007FF61
	public BaseResponseEntry.ResponseTypes ResponseType { get; set; }

	// Token: 0x04000EA7 RID: 3751
	public string UniqueID = "";

	// Token: 0x04000EAA RID: 3754
	public DialogResponse Response;

	// Token: 0x02000304 RID: 772
	public enum ResponseTypes
	{
		// Token: 0x04000EAC RID: 3756
		Response,
		// Token: 0x04000EAD RID: 3757
		QuestAdd
	}
}
