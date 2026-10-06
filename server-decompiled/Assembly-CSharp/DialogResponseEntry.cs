using System;
using UnityEngine.Scripting;

// Token: 0x02000303 RID: 771
[Preserve]
public class DialogResponseEntry : BaseResponseEntry
{
	// Token: 0x060015C8 RID: 5576 RVA: 0x00081B59 File Offset: 0x0007FD59
	public DialogResponseEntry(string _ID)
	{
		base.ID = _ID;
		base.ResponseType = BaseResponseEntry.ResponseTypes.Response;
	}
}
