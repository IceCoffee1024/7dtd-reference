using System;
using System.Collections.Generic;

// Token: 0x020002E7 RID: 743
public class BaseStatement : BaseDialogItem
{
	// Token: 0x17000262 RID: 610
	// (get) Token: 0x06001563 RID: 5475 RVA: 0x0008041B File Offset: 0x0007E61B
	// (set) Token: 0x06001564 RID: 5476 RVA: 0x00080423 File Offset: 0x0007E623
	public string NextStatementID
	{
		get; [PublicizedFrom(EAccessModifier.Internal)]
		set;
	}

	// Token: 0x06001565 RID: 5477 RVA: 0x0008042C File Offset: 0x0007E62C
	public override string ToString()
	{
		return this.Text;
	}

	// Token: 0x06001566 RID: 5478 RVA: 0x00080434 File Offset: 0x0007E634
	[PublicizedFrom(EAccessModifier.Internal)]
	public void AddAction(BaseDialogAction action)
	{
		this.Actions.Add(action);
	}

	// Token: 0x04000E5C RID: 3676
	public string Text;

	// Token: 0x04000E5E RID: 3678
	public List<BaseDialogAction> Actions = new List<BaseDialogAction>();
}
