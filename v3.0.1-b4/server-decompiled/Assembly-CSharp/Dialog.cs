using System;
using System.Collections.Generic;

// Token: 0x020002E5 RID: 741
public class Dialog
{
	// Token: 0x1700025F RID: 607
	// (get) Token: 0x06001553 RID: 5459 RVA: 0x0008009F File Offset: 0x0007E29F
	// (set) Token: 0x06001554 RID: 5460 RVA: 0x000800BB File Offset: 0x0007E2BB
	public DialogStatement CurrentStatement
	{
		get
		{
			if (this.ChildDialog != null)
			{
				return this.ChildDialog.CurrentStatement;
			}
			return this.currentStatement;
		}
		set
		{
			if (this.ChildDialog != null)
			{
				this.ChildDialog.CurrentStatement = value;
				return;
			}
			this.currentStatement = value;
		}
	}

	// Token: 0x06001555 RID: 5461 RVA: 0x000800DC File Offset: 0x0007E2DC
	public Dialog(string newID)
	{
		this.ID = newID;
	}

	// Token: 0x06001556 RID: 5462 RVA: 0x00080150 File Offset: 0x0007E350
	[PublicizedFrom(EAccessModifier.Internal)]
	public DialogStatement GetStatement(string currentStatementID)
	{
		if (this.ChildDialog != null)
		{
			return this.ChildDialog.GetStatement(currentStatementID);
		}
		if (currentStatementID == "")
		{
			currentStatementID = this.StartStatementID;
		}
		for (int i = 0; i < this.Statements.Count; i++)
		{
			if (this.Statements[i].ID == currentStatementID)
			{
				return this.Statements[i];
			}
		}
		return null;
	}

	// Token: 0x06001557 RID: 5463 RVA: 0x000801C4 File Offset: 0x0007E3C4
	[PublicizedFrom(EAccessModifier.Internal)]
	public DialogResponse GetResponse(string currentResponseID)
	{
		if (this.ChildDialog != null)
		{
			return this.ChildDialog.GetResponse(currentResponseID);
		}
		for (int i = 0; i < this.Responses.Count; i++)
		{
			if (this.Responses[i].ID == currentResponseID)
			{
				return this.Responses[i];
			}
		}
		return null;
	}

	// Token: 0x06001558 RID: 5464 RVA: 0x00080224 File Offset: 0x0007E424
	[PublicizedFrom(EAccessModifier.Internal)]
	public DialogStatement GetFirstStatment(EntityPlayer player)
	{
		string text = this.StartStatementID;
		for (int i = 0; i < this.Phases.Count; i++)
		{
			bool flag = true;
			for (int j = 0; j < this.Phases[i].RequirementList.Count; j++)
			{
				if (!this.Phases[i].RequirementList[j].CheckRequirement(player, this.CurrentOwner))
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				text = this.Phases[i].StartStatementID;
				break;
			}
		}
		for (int k = 0; k < this.Statements.Count; k++)
		{
			if (this.Statements[k].ID == text)
			{
				return this.Statements[k];
			}
		}
		return null;
	}

	// Token: 0x06001559 RID: 5465 RVA: 0x000802F4 File Offset: 0x0007E4F4
	public void RestartDialog(EntityPlayer player)
	{
		this.CurrentStatement = this.GetFirstStatment(player);
		this.ChildDialog = null;
	}

	// Token: 0x0600155A RID: 5466 RVA: 0x0008030C File Offset: 0x0007E50C
	public void SelectResponse(DialogResponse response, EntityPlayer player)
	{
		if (this.ChildDialog != null)
		{
			this.ChildDialog.SelectResponse(response, player);
			return;
		}
		if (response.Actions.Count > 0)
		{
			for (int i = 0; i < response.Actions.Count; i++)
			{
				response.Actions[i].PerformAction(player);
			}
		}
		if (response is DialogResponseQuest)
		{
			DialogResponseQuest dialogResponseQuest = response as DialogResponseQuest;
			QuestClass questClass = dialogResponseQuest.Quest.QuestClass;
			this.CurrentStatement = new DialogStatement("");
			this.CurrentStatement.NextStatementID = dialogResponseQuest.NextStatementID;
			this.CurrentStatement.Text = dialogResponseQuest.Quest.GetParsedText(questClass.StatementText);
			return;
		}
		this.CurrentStatement = this.GetStatement(response.NextStatementID);
	}

	// Token: 0x0600155B RID: 5467 RVA: 0x000803D0 File Offset: 0x0007E5D0
	public static void Cleanup()
	{
		Dialog.DialogList.Clear();
	}

	// Token: 0x0600155C RID: 5468 RVA: 0x000803DC File Offset: 0x0007E5DC
	public static void ReloadDialogs()
	{
		Dialog.Cleanup();
		WorldStaticData.Reset("dialogs");
	}

	// Token: 0x04000E4D RID: 3661
	public static Dictionary<string, Dialog> DialogList = new Dictionary<string, Dialog>();

	// Token: 0x04000E4E RID: 3662
	public string ID = "";

	// Token: 0x04000E4F RID: 3663
	public string StartStatementID = "";

	// Token: 0x04000E50 RID: 3664
	public string StartResponseID = "";

	// Token: 0x04000E51 RID: 3665
	public List<DialogPhase> Phases = new List<DialogPhase>();

	// Token: 0x04000E52 RID: 3666
	public List<DialogStatement> Statements = new List<DialogStatement>();

	// Token: 0x04000E53 RID: 3667
	public List<DialogResponse> Responses = new List<DialogResponse>();

	// Token: 0x04000E54 RID: 3668
	public EntityNPC CurrentOwner;

	// Token: 0x04000E55 RID: 3669
	public Dialog ChildDialog;

	// Token: 0x04000E56 RID: 3670
	public List<QuestEntry> QuestEntryList = new List<QuestEntry>();

	// Token: 0x04000E57 RID: 3671
	[PublicizedFrom(EAccessModifier.Private)]
	public DialogStatement currentStatement;

	// Token: 0x04000E58 RID: 3672
	public string currentReturnStatement = "";
}
