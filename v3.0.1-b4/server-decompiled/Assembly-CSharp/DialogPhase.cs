using System;
using System.Collections.Generic;

// Token: 0x020002E8 RID: 744
public class DialogPhase : BaseDialogItem
{
	// Token: 0x17000263 RID: 611
	// (get) Token: 0x06001568 RID: 5480 RVA: 0x00080455 File Offset: 0x0007E655
	// (set) Token: 0x06001569 RID: 5481 RVA: 0x0008045D File Offset: 0x0007E65D
	public string StartStatementID { get; set; }

	// Token: 0x17000264 RID: 612
	// (get) Token: 0x0600156A RID: 5482 RVA: 0x00080466 File Offset: 0x0007E666
	// (set) Token: 0x0600156B RID: 5483 RVA: 0x0008046E File Offset: 0x0007E66E
	public string StartResponseID { get; set; }

	// Token: 0x0600156C RID: 5484 RVA: 0x00080477 File Offset: 0x0007E677
	public DialogPhase(string newID)
	{
		this.ID = newID;
		this.HeaderName = string.Format("Phase : {0}", newID);
	}

	// Token: 0x0600156D RID: 5485 RVA: 0x000804A2 File Offset: 0x0007E6A2
	[PublicizedFrom(EAccessModifier.Internal)]
	public void AddRequirement(BaseDialogRequirement requirement)
	{
		this.RequirementList.Add(requirement);
	}

	// Token: 0x0600156E RID: 5486 RVA: 0x000804B0 File Offset: 0x0007E6B0
	public override string ToString()
	{
		return this.HeaderName;
	}

	// Token: 0x04000E5F RID: 3679
	public List<BaseDialogRequirement> RequirementList = new List<BaseDialogRequirement>();
}
