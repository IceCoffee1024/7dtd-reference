using System;
using System.Collections.Generic;

// Token: 0x020002EC RID: 748
public class DialogPhase : BaseDialogItem
{
	// Token: 0x17000264 RID: 612
	// (get) Token: 0x0600157B RID: 5499 RVA: 0x00080B09 File Offset: 0x0007ED09
	// (set) Token: 0x0600157C RID: 5500 RVA: 0x00080B11 File Offset: 0x0007ED11
	public string StartStatementID { get; set; }

	// Token: 0x17000265 RID: 613
	// (get) Token: 0x0600157D RID: 5501 RVA: 0x00080B1A File Offset: 0x0007ED1A
	// (set) Token: 0x0600157E RID: 5502 RVA: 0x00080B22 File Offset: 0x0007ED22
	public string StartResponseID { get; set; }

	// Token: 0x0600157F RID: 5503 RVA: 0x00080B2B File Offset: 0x0007ED2B
	public DialogPhase(string newID)
	{
		this.ID = newID;
		this.HeaderName = string.Format("Phase : {0}", newID);
	}

	// Token: 0x06001580 RID: 5504 RVA: 0x00080B56 File Offset: 0x0007ED56
	[PublicizedFrom(EAccessModifier.Internal)]
	public void AddRequirement(BaseDialogRequirement requirement)
	{
		this.RequirementList.Add(requirement);
	}

	// Token: 0x06001581 RID: 5505 RVA: 0x00080B64 File Offset: 0x0007ED64
	public override string ToString()
	{
		return this.HeaderName;
	}

	// Token: 0x04000E70 RID: 3696
	public List<BaseDialogRequirement> RequirementList = new List<BaseDialogRequirement>();
}
