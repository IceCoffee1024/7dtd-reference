using System;
using System.Collections.Generic;

// Token: 0x020002F0 RID: 752
public class BaseStatement : BaseDialogItem
{
	// Token: 0x1700025B RID: 603
	// (get) Token: 0x06001577 RID: 5495 RVA: 0x00080FA7 File Offset: 0x0007F1A7
	// (set) Token: 0x06001578 RID: 5496 RVA: 0x00080FAF File Offset: 0x0007F1AF
	public string NextStatementID
	{
		get; [PublicizedFrom(EAccessModifier.Internal)]
		set;
	}

	// Token: 0x06001579 RID: 5497 RVA: 0x00080FB8 File Offset: 0x0007F1B8
	public override string ToString()
	{
		return this.Text;
	}

	// Token: 0x0600157A RID: 5498 RVA: 0x00080FC0 File Offset: 0x0007F1C0
	[PublicizedFrom(EAccessModifier.Internal)]
	public void AddAction(BaseDialogAction action)
	{
		this.Actions.Add(action);
	}

	// Token: 0x04000E76 RID: 3702
	public string Text;

	// Token: 0x04000E78 RID: 3704
	public List<BaseDialogAction> Actions = new List<BaseDialogAction>();
}
