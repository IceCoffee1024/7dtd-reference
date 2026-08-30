using System;
using UnityEngine.Scripting;

// Token: 0x020002FA RID: 762
[Preserve]
public class BaseResponseEntry
{
	// Token: 0x17000277 RID: 631
	// (get) Token: 0x060015B3 RID: 5555 RVA: 0x000811BC File Offset: 0x0007F3BC
	// (set) Token: 0x060015B4 RID: 5556 RVA: 0x000811C4 File Offset: 0x0007F3C4
	public string ID { get; set; }

	// Token: 0x17000278 RID: 632
	// (get) Token: 0x060015B5 RID: 5557 RVA: 0x000811CD File Offset: 0x0007F3CD
	// (set) Token: 0x060015B6 RID: 5558 RVA: 0x000811D5 File Offset: 0x0007F3D5
	public BaseResponseEntry.ResponseTypes ResponseType { get; set; }

	// Token: 0x04000E8D RID: 3725
	public string UniqueID = "";

	// Token: 0x04000E90 RID: 3728
	public DialogResponse Response;

	// Token: 0x020002FB RID: 763
	public enum ResponseTypes
	{
		// Token: 0x04000E92 RID: 3730
		Response,
		// Token: 0x04000E93 RID: 3731
		QuestAdd
	}
}
