using System;
using UnityEngine.Scripting;

// Token: 0x02000976 RID: 2422
[Preserve]
public class QuestActionTrackQuest : BaseQuestAction
{
	// Token: 0x06004662 RID: 18018 RVA: 0x000027FC File Offset: 0x000009FC
	public override void SetupAction()
	{
	}

	// Token: 0x06004663 RID: 18019 RVA: 0x001BF22D File Offset: 0x001BD42D
	public override void PerformAction(Quest ownerQuest)
	{
		ownerQuest.Tracked = true;
		ownerQuest.OwnerJournal.RefreshTracked();
	}

	// Token: 0x06004664 RID: 18020 RVA: 0x001BF244 File Offset: 0x001BD444
	public override BaseQuestAction Clone()
	{
		QuestActionTrackQuest questActionTrackQuest = new QuestActionTrackQuest();
		base.CopyValues(questActionTrackQuest);
		return questActionTrackQuest;
	}
}
