using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x02000628 RID: 1576
[Preserve]
public class LootEntryRequirementQuestTags : BaseLootEntryRequirement
{
	// Token: 0x0600331B RID: 13083 RVA: 0x0014C5AC File Offset: 0x0014A7AC
	public override void Init(XElement e)
	{
		base.Init(e);
		string text = "";
		if (e.ParseAttribute("quest_tags", ref text))
		{
			this.questTags = FastTags<TagGroup.Global>.Parse(text);
		}
	}

	// Token: 0x0600331C RID: 13084 RVA: 0x0014C5E6 File Offset: 0x0014A7E6
	public override bool CheckRequirement(EntityPlayer player)
	{
		return player.QuestJournal.ActiveQuest != null && player.QuestJournal.ActiveQuest.QuestTags.Test_AnySet(this.questTags);
	}

	// Token: 0x040028DD RID: 10461
	[PublicizedFrom(EAccessModifier.Private)]
	public FastTags<TagGroup.Global> questTags;
}
