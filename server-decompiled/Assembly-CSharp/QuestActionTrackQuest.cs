using System;
using UnityEngine.Scripting;

// Token: 0x02000980 RID: 2432
[Preserve]
public class QuestActionTrackQuest : BaseQuestAction
{
	// Token: 0x06004685 RID: 18053 RVA: 0x000027FC File Offset: 0x000009FC
	public override void SetupAction()
	{
	}

	// Token: 0x06004686 RID: 18054 RVA: 0x001C02B1 File Offset: 0x001BE4B1
	public override void PerformAction(Quest ownerQuest)
	{
		ownerQuest.Tracked = true;
		ownerQuest.OwnerJournal.RefreshTracked();
	}

	// Token: 0x06004687 RID: 18055 RVA: 0x001C02C8 File Offset: 0x001BE4C8
	public override BaseQuestAction Clone()
	{
		QuestActionTrackQuest questActionTrackQuest = new QuestActionTrackQuest();
		base.CopyValues(questActionTrackQuest);
		return questActionTrackQuest;
	}
}
