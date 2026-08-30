using System;
using UnityEngine.Scripting;

// Token: 0x02000957 RID: 2391
[Preserve]
public class QuestActionGameEvent : BaseQuestAction
{
	// Token: 0x06004608 RID: 17928 RVA: 0x000027FC File Offset: 0x000009FC
	public override void SetupAction()
	{
	}

	// Token: 0x06004609 RID: 17929 RVA: 0x001BBA18 File Offset: 0x001B9C18
	public override void PerformAction(Quest ownerQuest)
	{
		GameEventManager.Current.HandleAction(this.ID, ownerQuest.OwnerJournal.OwnerPlayer, ownerQuest.OwnerJournal.OwnerPlayer, false, "", "", false, true, "", null);
	}

	// Token: 0x0600460A RID: 17930 RVA: 0x001BBA60 File Offset: 0x001B9C60
	public override BaseQuestAction Clone()
	{
		QuestActionGameEvent questActionGameEvent = new QuestActionGameEvent();
		base.CopyValues(questActionGameEvent);
		return questActionGameEvent;
	}

	// Token: 0x0600460B RID: 17931 RVA: 0x001BBA7B File Offset: 0x001B9C7B
	public override void ParseProperties(DynamicProperties properties)
	{
		base.ParseProperties(properties);
		properties.ParseString(QuestActionGameEvent.PropEventName, ref this.ID);
	}

	// Token: 0x0400383D RID: 14397
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropEventName = "event";
}
