using System;
using System.Collections.Generic;

// Token: 0x020002ED RID: 749
public class DialogResponse : BaseStatement
{
	// Token: 0x06001582 RID: 5506 RVA: 0x00080B6C File Offset: 0x0007ED6C
	public DialogResponse(string newID)
	{
		this.ID = newID;
		this.HeaderName = string.Format("Response : {0}", newID);
	}

	// Token: 0x06001583 RID: 5507 RVA: 0x00080BA2 File Offset: 0x0007EDA2
	[PublicizedFrom(EAccessModifier.Internal)]
	public static DialogResponse NextStatementEntry(string nextStatementID)
	{
		DialogResponse.nextStatementEntry.NextStatementID = nextStatementID;
		DialogResponse.nextStatementEntry.Text = "[" + Localization.Get("xuiNext", false, null) + "]";
		return DialogResponse.nextStatementEntry;
	}

	// Token: 0x06001584 RID: 5508 RVA: 0x00080BD9 File Offset: 0x0007EDD9
	[PublicizedFrom(EAccessModifier.Internal)]
	public void AddRequirement(BaseDialogRequirement requirement)
	{
		this.RequirementList.Add(requirement);
	}

	// Token: 0x06001585 RID: 5509 RVA: 0x00080BE7 File Offset: 0x0007EDE7
	public string GetRequiredDescription(EntityPlayer player)
	{
		if (this.RequirementList.Count == 0)
		{
			return "";
		}
		return this.RequirementList[0].GetRequiredDescription(player);
	}

	// Token: 0x04000E73 RID: 3699
	public List<BaseDialogRequirement> RequirementList = new List<BaseDialogRequirement>();

	// Token: 0x04000E74 RID: 3700
	public string ReturnStatementID = "";

	// Token: 0x04000E75 RID: 3701
	[PublicizedFrom(EAccessModifier.Private)]
	public static DialogResponse nextStatementEntry = new DialogResponse("__nextStatementEntry");
}
