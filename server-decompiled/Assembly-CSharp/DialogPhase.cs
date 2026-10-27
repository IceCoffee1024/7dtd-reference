using System;
using System.Collections.Generic;

// Token: 0x020002F1 RID: 753
public class DialogPhase : BaseDialogItem
{
	// Token: 0x1700025C RID: 604
	// (get) Token: 0x0600157C RID: 5500 RVA: 0x00080FE1 File Offset: 0x0007F1E1
	// (set) Token: 0x0600157D RID: 5501 RVA: 0x00080FE9 File Offset: 0x0007F1E9
	public string StartStatementID { get; set; }

	// Token: 0x1700025D RID: 605
	// (get) Token: 0x0600157E RID: 5502 RVA: 0x00080FF2 File Offset: 0x0007F1F2
	// (set) Token: 0x0600157F RID: 5503 RVA: 0x00080FFA File Offset: 0x0007F1FA
	public string StartResponseID { get; set; }

	// Token: 0x06001580 RID: 5504 RVA: 0x00081003 File Offset: 0x0007F203
	public DialogPhase(string newID)
	{
		this.ID = newID;
		this.HeaderName = string.Format("Phase : {0}", newID);
	}

	// Token: 0x06001581 RID: 5505 RVA: 0x0008102E File Offset: 0x0007F22E
	[PublicizedFrom(EAccessModifier.Internal)]
	public void AddRequirement(BaseDialogRequirement requirement)
	{
		this.RequirementList.Add(requirement);
	}

	// Token: 0x06001582 RID: 5506 RVA: 0x0008103C File Offset: 0x0007F23C
	public override string ToString()
	{
		return this.HeaderName;
	}

	// Token: 0x04000E79 RID: 3705
	public List<BaseDialogRequirement> RequirementList = new List<BaseDialogRequirement>();
}
