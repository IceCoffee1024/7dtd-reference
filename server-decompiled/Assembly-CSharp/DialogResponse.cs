using System;
using System.Collections.Generic;

// Token: 0x020002F2 RID: 754
public class DialogResponse : BaseStatement
{
	// Token: 0x06001583 RID: 5507 RVA: 0x00081044 File Offset: 0x0007F244
	public DialogResponse(string newID)
	{
		this.ID = newID;
		this.HeaderName = string.Format("Response : {0}", newID);
	}

	// Token: 0x06001584 RID: 5508 RVA: 0x0008107A File Offset: 0x0007F27A
	[PublicizedFrom(EAccessModifier.Internal)]
	public static DialogResponse NextStatementEntry(string nextStatementID)
	{
		DialogResponse.nextStatementEntry.NextStatementID = nextStatementID;
		DialogResponse.nextStatementEntry.Text = "[" + Localization.Get("xuiNext", false, null) + "]";
		return DialogResponse.nextStatementEntry;
	}

	// Token: 0x06001585 RID: 5509 RVA: 0x000810B1 File Offset: 0x0007F2B1
	[PublicizedFrom(EAccessModifier.Internal)]
	public void AddRequirement(BaseDialogRequirement requirement)
	{
		this.RequirementList.Add(requirement);
	}

	// Token: 0x06001586 RID: 5510 RVA: 0x000810BF File Offset: 0x0007F2BF
	public string GetRequiredDescription(EntityPlayer player)
	{
		if (this.RequirementList.Count == 0)
		{
			return "";
		}
		return this.RequirementList[0].GetRequiredDescription(player);
	}

	// Token: 0x04000E7C RID: 3708
	public List<BaseDialogRequirement> RequirementList = new List<BaseDialogRequirement>();

	// Token: 0x04000E7D RID: 3709
	public string ReturnStatementID = "";

	// Token: 0x04000E7E RID: 3710
	[PublicizedFrom(EAccessModifier.Private)]
	public static DialogResponse nextStatementEntry = new DialogResponse("__nextStatementEntry");
}
