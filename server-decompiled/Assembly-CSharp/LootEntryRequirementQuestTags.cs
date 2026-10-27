using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x02000631 RID: 1585
[Preserve]
public class LootEntryRequirementQuestTags : BaseLootEntryRequirement
{
	// Token: 0x0600336C RID: 13164 RVA: 0x0014DB10 File Offset: 0x0014BD10
	public override void Init(XElement e)
	{
		base.Init(e);
		string text = "";
		if (e.ParseAttribute("quest_tags", ref text))
		{
			this.questTags = FastTags<TagGroup.Global>.Parse(text);
		}
	}

	// Token: 0x0600336D RID: 13165 RVA: 0x0014DB4A File Offset: 0x0014BD4A
	public override bool CheckRequirement(EntityPlayer player)
	{
		return player.QuestJournal.ActiveQuest != null && player.QuestJournal.ActiveQuest.QuestTags.Test_AnySet(this.questTags);
	}

	// Token: 0x040028F1 RID: 10481
	[PublicizedFrom(EAccessModifier.Private)]
	public FastTags<TagGroup.Global> questTags;
}
