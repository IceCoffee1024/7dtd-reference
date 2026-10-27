using System;
using System.Collections.Generic;

// Token: 0x020002EE RID: 750
public class Dialog
{
	// Token: 0x17000258 RID: 600
	// (get) Token: 0x06001567 RID: 5479 RVA: 0x00080C2C File Offset: 0x0007EE2C
	// (set) Token: 0x06001568 RID: 5480 RVA: 0x00080C48 File Offset: 0x0007EE48
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

	// Token: 0x06001569 RID: 5481 RVA: 0x00080C68 File Offset: 0x0007EE68
	public Dialog(string newID)
	{
		this.ID = newID;
	}

	// Token: 0x0600156A RID: 5482 RVA: 0x00080CDC File Offset: 0x0007EEDC
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

	// Token: 0x0600156B RID: 5483 RVA: 0x00080D50 File Offset: 0x0007EF50
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

	// Token: 0x0600156C RID: 5484 RVA: 0x00080DB0 File Offset: 0x0007EFB0
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

	// Token: 0x0600156D RID: 5485 RVA: 0x00080E80 File Offset: 0x0007F080
	public void RestartDialog(EntityPlayer player)
	{
		this.CurrentStatement = this.GetFirstStatment(player);
		this.ChildDialog = null;
	}

	// Token: 0x0600156E RID: 5486 RVA: 0x00080E98 File Offset: 0x0007F098
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

	// Token: 0x0600156F RID: 5487 RVA: 0x00080F5C File Offset: 0x0007F15C
	public static void Cleanup()
	{
		Dialog.DialogList.Clear();
	}

	// Token: 0x06001570 RID: 5488 RVA: 0x00080F68 File Offset: 0x0007F168
	public static void ReloadDialogs()
	{
		Dialog.Cleanup();
		WorldStaticData.Reset("dialogs");
	}

	// Token: 0x04000E67 RID: 3687
	public static Dictionary<string, Dialog> DialogList = new Dictionary<string, Dialog>();

	// Token: 0x04000E68 RID: 3688
	public string ID = "";

	// Token: 0x04000E69 RID: 3689
	public string StartStatementID = "";

	// Token: 0x04000E6A RID: 3690
	public string StartResponseID = "";

	// Token: 0x04000E6B RID: 3691
	public List<DialogPhase> Phases = new List<DialogPhase>();

	// Token: 0x04000E6C RID: 3692
	public List<DialogStatement> Statements = new List<DialogStatement>();

	// Token: 0x04000E6D RID: 3693
	public List<DialogResponse> Responses = new List<DialogResponse>();

	// Token: 0x04000E6E RID: 3694
	public EntityNPC CurrentOwner;

	// Token: 0x04000E6F RID: 3695
	public Dialog ChildDialog;

	// Token: 0x04000E70 RID: 3696
	public List<QuestEntry> QuestEntryList = new List<QuestEntry>();

	// Token: 0x04000E71 RID: 3697
	[PublicizedFrom(EAccessModifier.Private)]
	public DialogStatement currentStatement;

	// Token: 0x04000E72 RID: 3698
	public string currentReturnStatement = "";
}
