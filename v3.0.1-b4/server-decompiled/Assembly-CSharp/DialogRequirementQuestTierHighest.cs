using System;
using System.Globalization;
using SandboxOptions;
using UnityEngine.Scripting;

// Token: 0x020002F8 RID: 760
[Preserve]
public class DialogRequirementQuestTierHighest : BaseDialogRequirement
{
	// Token: 0x17000275 RID: 629
	// (get) Token: 0x060015AB RID: 5547 RVA: 0x00046C7A File Offset: 0x00044E7A
	public override BaseDialogRequirement.RequirementTypes RequirementType
	{
		get
		{
			return BaseDialogRequirement.RequirementTypes.QuestTier;
		}
	}

	// Token: 0x060015AC RID: 5548 RVA: 0x00031E8B File Offset: 0x0003008B
	public override string GetRequiredDescription(EntityPlayer player)
	{
		return "";
	}

	// Token: 0x060015AD RID: 5549 RVA: 0x0008106C File Offset: 0x0007F26C
	public override bool CheckRequirement(EntityPlayer player, EntityNPC talkingTo)
	{
		int num = StringParsers.ParseSInt32(base.Value, 0, -1, NumberStyles.Integer);
		EntityTrader entityTrader = talkingTo as EntityTrader;
		if (!SandboxOptionManager.GetBool(global::SandboxOptions.SandboxOptions.QuestsEnabled))
		{
			return false;
		}
		if (entityTrader == null)
		{
			return false;
		}
		if (player.QuestJournal.GetCurrentFactionTier(entityTrader.NPCInfo.QuestFaction, 0, false) < num)
		{
			return false;
		}
		int num2 = -1;
		if (entityTrader.activeQuests == null)
		{
			return false;
		}
		bool flag = player.GetCVar("DisableQuesting") == 0f;
		for (int i = 0; i < entityTrader.activeQuests.Count; i++)
		{
			QuestClass questClass = entityTrader.activeQuests[i].QuestClass;
			if ((int)questClass.DifficultyTier > num2 && questClass.UniqueKey == base.Tag && (flag || questClass.AlwaysAllow))
			{
				num2 = (int)questClass.DifficultyTier;
			}
		}
		return num == num2;
	}
}
