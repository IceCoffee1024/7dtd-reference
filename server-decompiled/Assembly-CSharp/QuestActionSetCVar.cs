using System;
using System.Globalization;
using UnityEngine.Scripting;

// Token: 0x02000979 RID: 2425
[Preserve]
public class QuestActionSetCVar : BaseQuestAction
{
	// Token: 0x06004658 RID: 18008 RVA: 0x000027FC File Offset: 0x000009FC
	public override void SetupAction()
	{
	}

	// Token: 0x06004659 RID: 18009 RVA: 0x001BF829 File Offset: 0x001BDA29
	public override void PerformAction(Quest ownerQuest)
	{
		ownerQuest.OwnerJournal.OwnerPlayer.Buffs.SetCustomVar(this.ID, StringParsers.ParseFloat(this.Value, 0, -1, NumberStyles.Any), true, CVarOperation.set, false);
	}

	// Token: 0x0600465A RID: 18010 RVA: 0x001BF85C File Offset: 0x001BDA5C
	public override BaseQuestAction Clone()
	{
		QuestActionSetCVar questActionSetCVar = new QuestActionSetCVar();
		base.CopyValues(questActionSetCVar);
		return questActionSetCVar;
	}

	// Token: 0x0600465B RID: 18011 RVA: 0x001BF877 File Offset: 0x001BDA77
	public override void ParseProperties(DynamicProperties properties)
	{
		base.ParseProperties(properties);
		properties.ParseString(QuestActionSetCVar.PropCVar, ref this.ID);
	}

	// Token: 0x040038EB RID: 14571
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropCVar = "cvar";
}
