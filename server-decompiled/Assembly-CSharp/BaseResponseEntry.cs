using System;
using UnityEngine.Scripting;

// Token: 0x02000301 RID: 769
[Preserve]
public class BaseResponseEntry
{
	// Token: 0x17000270 RID: 624
	// (get) Token: 0x060015C3 RID: 5571 RVA: 0x00081B24 File Offset: 0x0007FD24
	// (set) Token: 0x060015C4 RID: 5572 RVA: 0x00081B2C File Offset: 0x0007FD2C
	public string ID { get; set; }

	// Token: 0x17000271 RID: 625
	// (get) Token: 0x060015C5 RID: 5573 RVA: 0x00081B35 File Offset: 0x0007FD35
	// (set) Token: 0x060015C6 RID: 5574 RVA: 0x00081B3D File Offset: 0x0007FD3D
	public BaseResponseEntry.ResponseTypes ResponseType { get; set; }

	// Token: 0x04000E97 RID: 3735
	public string UniqueID = "";

	// Token: 0x04000E9A RID: 3738
	public DialogResponse Response;

	// Token: 0x02000302 RID: 770
	public enum ResponseTypes
	{
		// Token: 0x04000E9C RID: 3740
		Response,
		// Token: 0x04000E9D RID: 3741
		QuestAdd
	}
}
