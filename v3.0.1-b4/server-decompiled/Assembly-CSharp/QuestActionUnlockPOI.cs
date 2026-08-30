using System;
using UnityEngine.Scripting;

// Token: 0x02000961 RID: 2401
[Preserve]
public class QuestActionUnlockPOI : BaseQuestAction
{
	// Token: 0x06004643 RID: 17987 RVA: 0x000027FC File Offset: 0x000009FC
	public override void SetupAction()
	{
	}

	// Token: 0x06004644 RID: 17988 RVA: 0x001BC6AB File Offset: 0x001BA8AB
	public override void PerformAction(Quest ownerQuest)
	{
		ownerQuest.HandleUnlockPOI(null);
	}

	// Token: 0x06004645 RID: 17989 RVA: 0x001BC6B4 File Offset: 0x001BA8B4
	public override BaseQuestAction Clone()
	{
		QuestActionUnlockPOI questActionUnlockPOI = new QuestActionUnlockPOI();
		base.CopyValues(questActionUnlockPOI);
		return questActionUnlockPOI;
	}
}
