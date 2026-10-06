using System;
using System.Collections.Generic;

// Token: 0x020002EF RID: 751
public class DialogPhase : BaseDialogItem
{
	// Token: 0x1700025C RID: 604
	// (get) Token: 0x06001578 RID: 5496 RVA: 0x00080DBD File Offset: 0x0007EFBD
	// (set) Token: 0x06001579 RID: 5497 RVA: 0x00080DC5 File Offset: 0x0007EFC5
	public string StartStatementID { get; set; }

	// Token: 0x1700025D RID: 605
	// (get) Token: 0x0600157A RID: 5498 RVA: 0x00080DCE File Offset: 0x0007EFCE
	// (set) Token: 0x0600157B RID: 5499 RVA: 0x00080DD6 File Offset: 0x0007EFD6
	public string StartResponseID { get; set; }

	// Token: 0x0600157C RID: 5500 RVA: 0x00080DDF File Offset: 0x0007EFDF
	public DialogPhase(string newID)
	{
		this.ID = newID;
		this.HeaderName = string.Format("Phase : {0}", newID);
	}

	// Token: 0x0600157D RID: 5501 RVA: 0x00080E0A File Offset: 0x0007F00A
	[PublicizedFrom(EAccessModifier.Internal)]
	public void AddRequirement(BaseDialogRequirement requirement)
	{
		this.RequirementList.Add(requirement);
	}

	// Token: 0x0600157E RID: 5502 RVA: 0x00080E18 File Offset: 0x0007F018
	public override string ToString()
	{
		return this.HeaderName;
	}

	// Token: 0x04000E69 RID: 3689
	public List<BaseDialogRequirement> RequirementList = new List<BaseDialogRequirement>();
}
