using System;
using System.Globalization;
using UnityEngine.Scripting;

// Token: 0x02000300 RID: 768
[Preserve]
public class DialogRequirementQuestTier : BaseDialogRequirement
{
	// Token: 0x1700026D RID: 621
	// (get) Token: 0x060015BB RID: 5563 RVA: 0x00047076 File Offset: 0x00045276
	public override BaseDialogRequirement.RequirementTypes RequirementType
	{
		get
		{
			return BaseDialogRequirement.RequirementTypes.QuestTier;
		}
	}

	// Token: 0x060015BC RID: 5564 RVA: 0x000321FF File Offset: 0x000303FF
	public override string GetRequiredDescription(EntityPlayer player)
	{
		return "";
	}

	// Token: 0x060015BD RID: 5565 RVA: 0x00081B58 File Offset: 0x0007FD58
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
