using System;
using UnityEngine.Scripting;

// Token: 0x02000306 RID: 774
[Preserve]
public class DialogQuestResponseEntry : BaseResponseEntry
{
	// Token: 0x060015CD RID: 5581 RVA: 0x00081D94 File Offset: 0x0007FF94
	public DialogQuestResponseEntry(string _questID, string _type, string _returnStatementID, int _listIndex, int _tier)
	{
		base.ID = _questID;
		this.ListIndex = _listIndex;
		base.ResponseType = BaseResponseEntry.ResponseTypes.QuestAdd;
		this.questType = _type;
		this.Tier = _tier;
		this.ReturnStatementID = _returnStatementID;
	}

	// Token: 0x04000EAE RID: 3758
	public int ListIndex = -1;

	// Token: 0x04000EAF RID: 3759
	public string ReturnStatementID = "";

	// Token: 0x04000EB0 RID: 3760
	public string questType = "";

	// Token: 0x04000EB1 RID: 3761
	public int Tier = -1;
}
