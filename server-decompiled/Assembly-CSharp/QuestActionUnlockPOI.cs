using System;
using UnityEngine.Scripting;

// Token: 0x02000982 RID: 2434
[Preserve]
public class QuestActionUnlockPOI : BaseQuestAction
{
	// Token: 0x0600468D RID: 18061 RVA: 0x000027FC File Offset: 0x000009FC
	public override void SetupAction()
	{
	}

	// Token: 0x0600468E RID: 18062 RVA: 0x001C042B File Offset: 0x001BE62B
	public override void PerformAction(Quest ownerQuest)
	{
		ownerQuest.HandleUnlockPOI(null);
	}

	// Token: 0x0600468F RID: 18063 RVA: 0x001C0434 File Offset: 0x001BE634
	public override BaseQuestAction Clone()
	{
		QuestActionUnlockPOI questActionUnlockPOI = new QuestActionUnlockPOI();
		base.CopyValues(questActionUnlockPOI);
		return questActionUnlockPOI;
	}
}
