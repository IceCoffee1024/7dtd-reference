using System;
using UnityEngine.Scripting;

// Token: 0x0200096E RID: 2414
[Preserve]
public class QuestActionGameEvent : BaseQuestAction
{
	// Token: 0x0600462F RID: 17967 RVA: 0x000027FC File Offset: 0x000009FC
	public override void SetupAction()
	{
	}

	// Token: 0x06004630 RID: 17968 RVA: 0x001BE714 File Offset: 0x001BC914
	public override void PerformAction(Quest ownerQuest)
	{
		GameEventManager.Current.HandleAction(this.ID, ownerQuest.OwnerJournal.OwnerPlayer, ownerQuest.OwnerJournal.OwnerPlayer, false, "", "", false, true, "", null);
	}

	// Token: 0x06004631 RID: 17969 RVA: 0x001BE75C File Offset: 0x001BC95C
	public override BaseQuestAction Clone()
	{
		QuestActionGameEvent questActionGameEvent = new QuestActionGameEvent();
		base.CopyValues(questActionGameEvent);
		return questActionGameEvent;
	}

	// Token: 0x06004632 RID: 17970 RVA: 0x001BE777 File Offset: 0x001BC977
	public override void ParseProperties(DynamicProperties properties)
	{
		base.ParseProperties(properties);
		properties.ParseString(QuestActionGameEvent.PropEventName, ref this.ID);
	}

	// Token: 0x040038CA RID: 14538
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropEventName = "event";
}
