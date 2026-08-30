using System;
using UnityEngine.Scripting;

// Token: 0x020002FD RID: 765
[Preserve]
public class DialogQuestResponseEntry : BaseResponseEntry
{
	// Token: 0x060015B9 RID: 5561 RVA: 0x00081208 File Offset: 0x0007F408
	public DialogQuestResponseEntry(string _questID, string _type, string _returnStatementID, int _listIndex, int _tier)
	{
		base.ID = _questID;
		this.ListIndex = _listIndex;
		base.ResponseType = BaseResponseEntry.ResponseTypes.QuestAdd;
		this.questType = _type;
		this.Tier = _tier;
		this.ReturnStatementID = _returnStatementID;
	}

	// Token: 0x04000E94 RID: 3732
	public int ListIndex = -1;

	// Token: 0x04000E95 RID: 3733
	public string ReturnStatementID = "";

	// Token: 0x04000E96 RID: 3734
	public string questType = "";

	// Token: 0x04000E97 RID: 3735
	public int Tier = -1;
}
