using System;
using System.Globalization;
using UnityEngine.Scripting;

// Token: 0x02000969 RID: 2409
[Preserve]
public class QuestActionSetCVar : BaseQuestAction
{
	// Token: 0x06004694 RID: 18068 RVA: 0x000027FC File Offset: 0x000009FC
	public override void SetupAction()
	{
	}

	// Token: 0x06004695 RID: 18069 RVA: 0x001BE29D File Offset: 0x001BC49D
	public override void PerformAction(Quest ownerQuest)
	{
		ownerQuest.OwnerJournal.OwnerPlayer.Buffs.SetCustomVar(this.ID, StringParsers.ParseFloat(this.Value, 0, -1, NumberStyles.Any), true, CVarOperation.set, false);
	}

	// Token: 0x06004696 RID: 18070 RVA: 0x001BE2D0 File Offset: 0x001BC4D0
	public override BaseQuestAction Clone()
	{
		QuestActionSetCVar questActionSetCVar = new QuestActionSetCVar();
		base.CopyValues(questActionSetCVar);
		return questActionSetCVar;
	}

	// Token: 0x06004697 RID: 18071 RVA: 0x001BE2EB File Offset: 0x001BC4EB
	public override void ParseProperties(DynamicProperties properties)
	{
		base.ParseProperties(properties);
		properties.ParseString(QuestActionSetCVar.PropCVar, ref this.ID);
	}

	// Token: 0x040038CF RID: 14543
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropCVar = "cvar";
}
