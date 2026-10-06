using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x0200062A RID: 1578
[Preserve]
public class LootEntryRequirementQuestTags : BaseLootEntryRequirement
{
	// Token: 0x0600335E RID: 13150 RVA: 0x0014CF1C File Offset: 0x0014B11C
	public override void Init(XElement e)
	{
		base.Init(e);
		string text = "";
		if (e.ParseAttribute("quest_tags", ref text))
		{
			this.questTags = FastTags<TagGroup.Global>.Parse(text);
		}
	}

	// Token: 0x0600335F RID: 13151 RVA: 0x0014CF56 File Offset: 0x0014B156
	public override bool CheckRequirement(EntityPlayer player)
	{
		return player.QuestJournal.ActiveQuest != null && player.QuestJournal.ActiveQuest.QuestTags.Test_AnySet(this.questTags);
	}

	// Token: 0x040028D6 RID: 10454
	[PublicizedFrom(EAccessModifier.Private)]
	public FastTags<TagGroup.Global> questTags;
}
