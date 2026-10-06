using System;
using System.Globalization;
using UnityEngine.Scripting;

// Token: 0x0200096F RID: 2415
[Preserve]
public class QuestActionSetCVar : BaseQuestAction
{
	// Token: 0x06004635 RID: 17973 RVA: 0x000027FC File Offset: 0x000009FC
	public override void SetupAction()
	{
	}

	// Token: 0x06004636 RID: 17974 RVA: 0x001BE7A5 File Offset: 0x001BC9A5
	public override void PerformAction(Quest ownerQuest)
	{
		ownerQuest.OwnerJournal.OwnerPlayer.Buffs.SetCustomVar(this.ID, StringParsers.ParseFloat(this.Value, 0, -1, NumberStyles.Any), true, CVarOperation.set, false);
	}

	// Token: 0x06004637 RID: 17975 RVA: 0x001BE7D8 File Offset: 0x001BC9D8
	public override BaseQuestAction Clone()
	{
		QuestActionSetCVar questActionSetCVar = new QuestActionSetCVar();
		base.CopyValues(questActionSetCVar);
		return questActionSetCVar;
	}

	// Token: 0x06004638 RID: 17976 RVA: 0x001BE7F3 File Offset: 0x001BC9F3
	public override void ParseProperties(DynamicProperties properties)
	{
		base.ParseProperties(properties);
		properties.ParseString(QuestActionSetCVar.PropCVar, ref this.ID);
	}

	// Token: 0x040038CB RID: 14539
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropCVar = "cvar";
}
