using System;
using UnityEngine.Scripting;

// Token: 0x02000304 RID: 772
[Preserve]
public class DialogQuestResponseEntry : BaseResponseEntry
{
	// Token: 0x060015C9 RID: 5577 RVA: 0x00081B70 File Offset: 0x0007FD70
	public DialogQuestResponseEntry(string _questID, string _type, string _returnStatementID, int _listIndex, int _tier)
	{
		base.ID = _questID;
		this.ListIndex = _listIndex;
		base.ResponseType = BaseResponseEntry.ResponseTypes.QuestAdd;
		this.questType = _type;
		this.Tier = _tier;
		this.ReturnStatementID = _returnStatementID;
	}

	// Token: 0x04000E9E RID: 3742
	public int ListIndex = -1;

	// Token: 0x04000E9F RID: 3743
	public string ReturnStatementID = "";

	// Token: 0x04000EA0 RID: 3744
	public string questType = "";

	// Token: 0x04000EA1 RID: 3745
	public int Tier = -1;
}
