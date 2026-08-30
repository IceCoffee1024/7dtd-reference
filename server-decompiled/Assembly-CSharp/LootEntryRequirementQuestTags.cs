using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x0200061F RID: 1567
[Preserve]
public class LootEntryRequirementQuestTags : BaseLootEntryRequirement
{
	// Token: 0x060032C8 RID: 13000 RVA: 0x0014A490 File Offset: 0x00148690
	public override void Init(XElement e)
	{
		base.Init(e);
		string text = "";
		if (e.ParseAttribute("quest_tags", ref text))
		{
			this.questTags = FastTags<TagGroup.Global>.Parse(text);
		}
	}

	// Token: 0x060032C9 RID: 13001 RVA: 0x0014A4CA File Offset: 0x001486CA
	public override bool CheckRequirement(EntityPlayer player)
	{
		return player.QuestJournal.ActiveQuest != null && player.QuestJournal.ActiveQuest.QuestTags.Test_AnySet(this.questTags);
	}

	// Token: 0x0400286A RID: 10346
	[PublicizedFrom(EAccessModifier.Private)]
	public FastTags<TagGroup.Global> questTags;
}
