using System;
using UnityEngine.Scripting;

// Token: 0x020002FC RID: 764
[Preserve]
public class DialogResponseEntry : BaseResponseEntry
{
	// Token: 0x060015B8 RID: 5560 RVA: 0x000811F1 File Offset: 0x0007F3F1
	public DialogResponseEntry(string _ID)
	{
		base.ID = _ID;
		base.ResponseType = BaseResponseEntry.ResponseTypes.Response;
	}
}
