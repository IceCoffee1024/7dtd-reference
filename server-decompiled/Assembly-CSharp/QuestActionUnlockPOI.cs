using System;
using UnityEngine.Scripting;

// Token: 0x02000972 RID: 2418
[Preserve]
public class QuestActionUnlockPOI : BaseQuestAction
{
	// Token: 0x060046C9 RID: 18121 RVA: 0x000027FC File Offset: 0x000009FC
	public override void SetupAction()
	{
	}

	// Token: 0x060046CA RID: 18122 RVA: 0x001BEE9F File Offset: 0x001BD09F
	public override void PerformAction(Quest ownerQuest)
	{
		ownerQuest.HandleUnlockPOI(null);
	}

	// Token: 0x060046CB RID: 18123 RVA: 0x001BEEA8 File Offset: 0x001BD0A8
	public override BaseQuestAction Clone()
	{
		QuestActionUnlockPOI questActionUnlockPOI = new QuestActionUnlockPOI();
		base.CopyValues(questActionUnlockPOI);
		return questActionUnlockPOI;
	}
}
