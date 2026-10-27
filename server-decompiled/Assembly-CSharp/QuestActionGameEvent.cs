using System;
using UnityEngine.Scripting;

// Token: 0x02000978 RID: 2424
[Preserve]
public class QuestActionGameEvent : BaseQuestAction
{
	// Token: 0x06004652 RID: 18002 RVA: 0x000027FC File Offset: 0x000009FC
	public override void SetupAction()
	{
	}

	// Token: 0x06004653 RID: 18003 RVA: 0x001BF798 File Offset: 0x001BD998
	public override void PerformAction(Quest ownerQuest)
	{
		GameEventManager.Current.HandleAction(this.ID, ownerQuest.OwnerJournal.OwnerPlayer, ownerQuest.OwnerJournal.OwnerPlayer, false, "", "", false, true, "", null);
	}

	// Token: 0x06004654 RID: 18004 RVA: 0x001BF7E0 File Offset: 0x001BD9E0
	public override BaseQuestAction Clone()
	{
		QuestActionGameEvent questActionGameEvent = new QuestActionGameEvent();
		base.CopyValues(questActionGameEvent);
		return questActionGameEvent;
	}

	// Token: 0x06004655 RID: 18005 RVA: 0x001BF7FB File Offset: 0x001BD9FB
	public override void ParseProperties(DynamicProperties properties)
	{
		base.ParseProperties(properties);
		properties.ParseString(QuestActionGameEvent.PropEventName, ref this.ID);
	}

	// Token: 0x040038EA RID: 14570
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropEventName = "event";
}
