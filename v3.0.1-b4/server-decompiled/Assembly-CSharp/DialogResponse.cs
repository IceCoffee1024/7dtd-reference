using System;
using System.Collections.Generic;

// Token: 0x020002E9 RID: 745
public class DialogResponse : BaseStatement
{
	// Token: 0x0600156F RID: 5487 RVA: 0x000804B8 File Offset: 0x0007E6B8
	public DialogResponse(string newID)
	{
		this.ID = newID;
		this.HeaderName = string.Format("Response : {0}", newID);
	}

	// Token: 0x06001570 RID: 5488 RVA: 0x000804EE File Offset: 0x0007E6EE
	[PublicizedFrom(EAccessModifier.Internal)]
	public static DialogResponse NextStatementEntry(string nextStatementID)
	{
		DialogResponse.nextStatementEntry.NextStatementID = nextStatementID;
		DialogResponse.nextStatementEntry.Text = "[" + Localization.Get("xuiNext", false, null) + "]";
		return DialogResponse.nextStatementEntry;
	}

	// Token: 0x06001571 RID: 5489 RVA: 0x00080525 File Offset: 0x0007E725
	[PublicizedFrom(EAccessModifier.Internal)]
	public void AddRequirement(BaseDialogRequirement requirement)
	{
		this.RequirementList.Add(requirement);
	}

	// Token: 0x06001572 RID: 5490 RVA: 0x00080533 File Offset: 0x0007E733
	public string GetRequiredDescription(EntityPlayer player)
	{
		if (this.RequirementList.Count == 0)
		{
			return "";
		}
		return this.RequirementList[0].GetRequiredDescription(player);
	}

	// Token: 0x04000E62 RID: 3682
	public List<BaseDialogRequirement> RequirementList = new List<BaseDialogRequirement>();

	// Token: 0x04000E63 RID: 3683
	public string ReturnStatementID = "";

	// Token: 0x04000E64 RID: 3684
	[PublicizedFrom(EAccessModifier.Private)]
	public static DialogResponse nextStatementEntry = new DialogResponse("__nextStatementEntry");
}
