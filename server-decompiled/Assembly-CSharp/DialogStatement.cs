using System;
using System.Collections.Generic;

// Token: 0x020002F4 RID: 756
public class DialogStatement : BaseStatement
{
	// Token: 0x0600158A RID: 5514 RVA: 0x00081431 File Offset: 0x0007F631
	public DialogStatement(string newID)
	{
		this.ID = newID;
		this.HeaderName = string.Format("Statement : {0}", newID);
	}

	// Token: 0x0600158B RID: 5515 RVA: 0x0008145C File Offset: 0x0007F65C
	public List<BaseResponseEntry> GetResponses()
	{
		List<BaseResponseEntry> list = new List<BaseResponseEntry>();
		if (this.ResponseEntries.Count > 0)
		{
			for (int i = 0; i < this.ResponseEntries.Count; i++)
			{
				BaseResponseEntry.ResponseTypes responseType = this.ResponseEntries[i].ResponseType;
				if (responseType != BaseResponseEntry.ResponseTypes.Response)
				{
					if (responseType == BaseResponseEntry.ResponseTypes.QuestAdd)
					{
						DialogQuestResponseEntry dialogQuestResponseEntry = this.ResponseEntries[i] as DialogQuestResponseEntry;
						DialogResponseQuest dialogResponseQuest = new DialogResponseQuest(dialogQuestResponseEntry.ID, dialogQuestResponseEntry.ReturnStatementID, this.ID, dialogQuestResponseEntry.questType, base.OwnerDialog, dialogQuestResponseEntry.ListIndex, dialogQuestResponseEntry.Tier);
						if (dialogResponseQuest.IsValid)
						{
							this.ResponseEntries[i].Response = dialogResponseQuest;
							list.Add(this.ResponseEntries[i]);
						}
					}
				}
				else
				{
					this.ResponseEntries[i].Response = base.OwnerDialog.GetResponse(this.ResponseEntries[i].ID);
					list.Add(this.ResponseEntries[i]);
				}
			}
		}
		else if (base.NextStatementID != "")
		{
			list.Add(new DialogResponseEntry(base.NextStatementID)
			{
				Response = DialogResponse.NextStatementEntry(base.NextStatementID)
			});
		}
		return list;
	}

	// Token: 0x04000E85 RID: 3717
	public List<BaseResponseEntry> ResponseEntries = new List<BaseResponseEntry>();
}
