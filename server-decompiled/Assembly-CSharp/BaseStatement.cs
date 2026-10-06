using System;
using System.Collections.Generic;

// Token: 0x020002EE RID: 750
public class BaseStatement : BaseDialogItem
{
	// Token: 0x1700025B RID: 603
	// (get) Token: 0x06001573 RID: 5491 RVA: 0x00080D83 File Offset: 0x0007EF83
	// (set) Token: 0x06001574 RID: 5492 RVA: 0x00080D8B File Offset: 0x0007EF8B
	public string NextStatementID
	{
		get; [PublicizedFrom(EAccessModifier.Internal)]
		set;
	}

	// Token: 0x06001575 RID: 5493 RVA: 0x00080D94 File Offset: 0x0007EF94
	public override string ToString()
	{
		return this.Text;
	}

	// Token: 0x06001576 RID: 5494 RVA: 0x00080D9C File Offset: 0x0007EF9C
	[PublicizedFrom(EAccessModifier.Internal)]
	public void AddAction(BaseDialogAction action)
	{
		this.Actions.Add(action);
	}

	// Token: 0x04000E66 RID: 3686
	public string Text;

	// Token: 0x04000E68 RID: 3688
	public List<BaseDialogAction> Actions = new List<BaseDialogAction>();
}
