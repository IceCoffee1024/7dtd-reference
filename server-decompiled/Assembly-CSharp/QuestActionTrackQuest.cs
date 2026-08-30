using System;
using UnityEngine.Scripting;

// Token: 0x02000970 RID: 2416
[Preserve]
public class QuestActionTrackQuest : BaseQuestAction
{
	// Token: 0x060046C1 RID: 18113 RVA: 0x000027FC File Offset: 0x000009FC
	public override void SetupAction()
	{
	}

	// Token: 0x060046C2 RID: 18114 RVA: 0x001BED25 File Offset: 0x001BCF25
	public override void PerformAction(Quest ownerQuest)
	{
		ownerQuest.Tracked = true;
		ownerQuest.OwnerJournal.RefreshTracked();
	}

	// Token: 0x060046C3 RID: 18115 RVA: 0x001BED3C File Offset: 0x001BCF3C
	public override BaseQuestAction Clone()
	{
		QuestActionTrackQuest questActionTrackQuest = new QuestActionTrackQuest();
		base.CopyValues(questActionTrackQuest);
		return questActionTrackQuest;
	}
}
