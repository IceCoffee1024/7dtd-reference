using System;
using UnityEngine.Scripting;

// Token: 0x0200095F RID: 2399
[Preserve]
public class QuestActionTrackQuest : BaseQuestAction
{
	// Token: 0x0600463B RID: 17979 RVA: 0x000027FC File Offset: 0x000009FC
	public override void SetupAction()
	{
	}

	// Token: 0x0600463C RID: 17980 RVA: 0x001BC531 File Offset: 0x001BA731
	public override void PerformAction(Quest ownerQuest)
	{
		ownerQuest.Tracked = true;
		ownerQuest.OwnerJournal.RefreshTracked();
	}

	// Token: 0x0600463D RID: 17981 RVA: 0x001BC548 File Offset: 0x001BA748
	public override BaseQuestAction Clone()
	{
		QuestActionTrackQuest questActionTrackQuest = new QuestActionTrackQuest();
		base.CopyValues(questActionTrackQuest);
		return questActionTrackQuest;
	}
}
