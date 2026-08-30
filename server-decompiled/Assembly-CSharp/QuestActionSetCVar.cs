using System;
using System.Globalization;
using UnityEngine.Scripting;

// Token: 0x02000958 RID: 2392
[Preserve]
public class QuestActionSetCVar : BaseQuestAction
{
	// Token: 0x0600460E RID: 17934 RVA: 0x000027FC File Offset: 0x000009FC
	public override void SetupAction()
	{
	}

	// Token: 0x0600460F RID: 17935 RVA: 0x001BBAA9 File Offset: 0x001B9CA9
	public override void PerformAction(Quest ownerQuest)
	{
		ownerQuest.OwnerJournal.OwnerPlayer.Buffs.SetCustomVar(this.ID, StringParsers.ParseFloat(this.Value, 0, -1, NumberStyles.Any), true, CVarOperation.set);
	}

	// Token: 0x06004610 RID: 17936 RVA: 0x001BBADC File Offset: 0x001B9CDC
	public override BaseQuestAction Clone()
	{
		QuestActionSetCVar questActionSetCVar = new QuestActionSetCVar();
		base.CopyValues(questActionSetCVar);
		return questActionSetCVar;
	}

	// Token: 0x06004611 RID: 17937 RVA: 0x001BBAF7 File Offset: 0x001B9CF7
	public override void ParseProperties(DynamicProperties properties)
	{
		base.ParseProperties(properties);
		properties.ParseString(QuestActionSetCVar.PropCVar, ref this.ID);
	}

	// Token: 0x0400383E RID: 14398
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropCVar = "cvar";
}
