using System;
using UnityEngine.Scripting;

// Token: 0x02000978 RID: 2424
[Preserve]
public class QuestActionUnlockPOI : BaseQuestAction
{
	// Token: 0x0600466A RID: 18026 RVA: 0x000027FC File Offset: 0x000009FC
	public override void SetupAction()
	{
	}

	// Token: 0x0600466B RID: 18027 RVA: 0x001BF3A7 File Offset: 0x001BD5A7
	public override void PerformAction(Quest ownerQuest)
	{
		ownerQuest.HandleUnlockPOI(null);
	}

	// Token: 0x0600466C RID: 18028 RVA: 0x001BF3B0 File Offset: 0x001BD5B0
	public override BaseQuestAction Clone()
	{
		QuestActionUnlockPOI questActionUnlockPOI = new QuestActionUnlockPOI();
		base.CopyValues(questActionUnlockPOI);
		return questActionUnlockPOI;
	}
}
