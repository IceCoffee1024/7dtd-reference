using System;
using System.Globalization;
using UnityEngine.Scripting;

// Token: 0x020002FE RID: 766
[Preserve]
public class DialogRequirementQuestTier : BaseDialogRequirement
{
	// Token: 0x1700026D RID: 621
	// (get) Token: 0x060015B7 RID: 5559 RVA: 0x000470B6 File Offset: 0x000452B6
	public override BaseDialogRequirement.RequirementTypes RequirementType
	{
		get
		{
			return BaseDialogRequirement.RequirementTypes.QuestTier;
		}
	}

	// Token: 0x060015B8 RID: 5560 RVA: 0x0003223F File Offset: 0x0003043F
	public override string GetRequiredDescription(EntityPlayer player)
	{
		return "";
	}

	// Token: 0x060015B9 RID: 5561 RVA: 0x00081934 File Offset: 0x0007FB34
	public override bool CheckRequirement(EntityPlayer player, EntityNPC talkingTo)
	{
		EntityTrader entityTrader = talkingTo as EntityTrader;
		if (entityTrader == null)
		{
			return false;
		}
		int num = StringParsers.ParseSInt32(base.Value, 0, -1, NumberStyles.Integer);
		if (player.QuestJournal.GetCurrentFactionTier(entityTrader.NPCInfo.QuestFaction, 0, false) < num)
		{
			return false;
		}
		for (int i = 0; i < entityTrader.activeQuests.Count; i++)
		{
			if ((int)entityTrader.activeQuests[i].QuestClass.DifficultyTier == num && entityTrader.activeQuests[i].QuestClass.UniqueKey == base.Tag)
			{
				return true;
			}
		}
		return false;
	}
}
