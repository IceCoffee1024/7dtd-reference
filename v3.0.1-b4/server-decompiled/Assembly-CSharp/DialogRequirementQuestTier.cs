using System;
using System.Globalization;
using UnityEngine.Scripting;

// Token: 0x020002F7 RID: 759
[Preserve]
public class DialogRequirementQuestTier : BaseDialogRequirement
{
	// Token: 0x17000274 RID: 628
	// (get) Token: 0x060015A7 RID: 5543 RVA: 0x00046C7A File Offset: 0x00044E7A
	public override BaseDialogRequirement.RequirementTypes RequirementType
	{
		get
		{
			return BaseDialogRequirement.RequirementTypes.QuestTier;
		}
	}

	// Token: 0x060015A8 RID: 5544 RVA: 0x00031E8B File Offset: 0x0003008B
	public override string GetRequiredDescription(EntityPlayer player)
	{
		return "";
	}

	// Token: 0x060015A9 RID: 5545 RVA: 0x00080FCC File Offset: 0x0007F1CC
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
