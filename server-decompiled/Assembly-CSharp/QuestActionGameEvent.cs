using System;
using UnityEngine.Scripting;

// Token: 0x02000968 RID: 2408
[Preserve]
public class QuestActionGameEvent : BaseQuestAction
{
	// Token: 0x0600468E RID: 18062 RVA: 0x000027FC File Offset: 0x000009FC
	public override void SetupAction()
	{
	}

	// Token: 0x0600468F RID: 18063 RVA: 0x001BE20C File Offset: 0x001BC40C
	public override void PerformAction(Quest ownerQuest)
	{
		GameEventManager.Current.HandleAction(this.ID, ownerQuest.OwnerJournal.OwnerPlayer, ownerQuest.OwnerJournal.OwnerPlayer, false, "", "", false, true, "", null);
	}

	// Token: 0x06004690 RID: 18064 RVA: 0x001BE254 File Offset: 0x001BC454
	public override BaseQuestAction Clone()
	{
		QuestActionGameEvent questActionGameEvent = new QuestActionGameEvent();
		base.CopyValues(questActionGameEvent);
		return questActionGameEvent;
	}

	// Token: 0x06004691 RID: 18065 RVA: 0x001BE26F File Offset: 0x001BC46F
	public override void ParseProperties(DynamicProperties properties)
	{
		base.ParseProperties(properties);
		properties.ParseString(QuestActionGameEvent.PropEventName, ref this.ID);
	}

	// Token: 0x040038CE RID: 14542
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropEventName = "event";
}
