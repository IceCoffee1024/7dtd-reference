using System;
using UnityEngine.Scripting;

// Token: 0x02000305 RID: 773
[Preserve]
public class DialogResponseEntry : BaseResponseEntry
{
	// Token: 0x060015CC RID: 5580 RVA: 0x00081D7D File Offset: 0x0007FF7D
	public DialogResponseEntry(string _ID)
	{
		base.ID = _ID;
		base.ResponseType = BaseResponseEntry.ResponseTypes.Response;
	}
}
