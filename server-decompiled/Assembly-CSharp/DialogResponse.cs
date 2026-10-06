using System;
using System.Collections.Generic;

// Token: 0x020002F0 RID: 752
public class DialogResponse : BaseStatement
{
	// Token: 0x0600157F RID: 5503 RVA: 0x00080E20 File Offset: 0x0007F020
	public DialogResponse(string newID)
	{
		this.ID = newID;
		this.HeaderName = string.Format("Response : {0}", newID);
	}

	// Token: 0x06001580 RID: 5504 RVA: 0x00080E56 File Offset: 0x0007F056
	[PublicizedFrom(EAccessModifier.Internal)]
	public static DialogResponse NextStatementEntry(string nextStatementID)
	{
		DialogResponse.nextStatementEntry.NextStatementID = nextStatementID;
		DialogResponse.nextStatementEntry.Text = "[" + Localization.Get("xuiNext", false, null) + "]";
		return DialogResponse.nextStatementEntry;
	}

	// Token: 0x06001581 RID: 5505 RVA: 0x00080E8D File Offset: 0x0007F08D
	[PublicizedFrom(EAccessModifier.Internal)]
	public void AddRequirement(BaseDialogRequirement requirement)
	{
		this.RequirementList.Add(requirement);
	}

	// Token: 0x06001582 RID: 5506 RVA: 0x00080E9B File Offset: 0x0007F09B
	public string GetRequiredDescription(EntityPlayer player)
	{
		if (this.RequirementList.Count == 0)
		{
			return "";
		}
		return this.RequirementList[0].GetRequiredDescription(player);
	}

	// Token: 0x04000E6C RID: 3692
	public List<BaseDialogRequirement> RequirementList = new List<BaseDialogRequirement>();

	// Token: 0x04000E6D RID: 3693
	public string ReturnStatementID = "";

	// Token: 0x04000E6E RID: 3694
	[PublicizedFrom(EAccessModifier.Private)]
	public static DialogResponse nextStatementEntry = new DialogResponse("__nextStatementEntry");
}
