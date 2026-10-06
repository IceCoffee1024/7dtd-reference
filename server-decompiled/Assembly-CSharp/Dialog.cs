using System;
using System.Collections.Generic;

// Token: 0x020002EC RID: 748
public class Dialog
{
	// Token: 0x17000258 RID: 600
	// (get) Token: 0x06001563 RID: 5475 RVA: 0x00080A07 File Offset: 0x0007EC07
	// (set) Token: 0x06001564 RID: 5476 RVA: 0x00080A23 File Offset: 0x0007EC23
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

	// Token: 0x06001565 RID: 5477 RVA: 0x00080A44 File Offset: 0x0007EC44
	public Dialog(string newID)
	{
		this.ID = newID;
	}

	// Token: 0x06001566 RID: 5478 RVA: 0x00080AB8 File Offset: 0x0007ECB8
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

	// Token: 0x06001567 RID: 5479 RVA: 0x00080B2C File Offset: 0x0007ED2C
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

	// Token: 0x06001568 RID: 5480 RVA: 0x00080B8C File Offset: 0x0007ED8C
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

	// Token: 0x06001569 RID: 5481 RVA: 0x00080C5C File Offset: 0x0007EE5C
	public void RestartDialog(EntityPlayer player)
	{
		this.CurrentStatement = this.GetFirstStatment(player);
		this.ChildDialog = null;
	}

	// Token: 0x0600156A RID: 5482 RVA: 0x00080C74 File Offset: 0x0007EE74
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

	// Token: 0x0600156B RID: 5483 RVA: 0x00080D38 File Offset: 0x0007EF38
	public static void Cleanup()
	{
		Dialog.DialogList.Clear();
	}

	// Token: 0x0600156C RID: 5484 RVA: 0x00080D44 File Offset: 0x0007EF44
	public static void ReloadDialogs()
	{
		Dialog.Cleanup();
		WorldStaticData.Reset("dialogs");
	}

	// Token: 0x04000E57 RID: 3671
	public static Dictionary<string, Dialog> DialogList = new Dictionary<string, Dialog>();

	// Token: 0x04000E58 RID: 3672
	public string ID = "";

	// Token: 0x04000E59 RID: 3673
	public string StartStatementID = "";

	// Token: 0x04000E5A RID: 3674
	public string StartResponseID = "";

	// Token: 0x04000E5B RID: 3675
	public List<DialogPhase> Phases = new List<DialogPhase>();

	// Token: 0x04000E5C RID: 3676
	public List<DialogStatement> Statements = new List<DialogStatement>();

	// Token: 0x04000E5D RID: 3677
	public List<DialogResponse> Responses = new List<DialogResponse>();

	// Token: 0x04000E5E RID: 3678
	public EntityNPC CurrentOwner;

	// Token: 0x04000E5F RID: 3679
	public Dialog ChildDialog;

	// Token: 0x04000E60 RID: 3680
	public List<QuestEntry> QuestEntryList = new List<QuestEntry>();

	// Token: 0x04000E61 RID: 3681
	[PublicizedFrom(EAccessModifier.Private)]
	public DialogStatement currentStatement;

	// Token: 0x04000E62 RID: 3682
	public string currentReturnStatement = "";
}
