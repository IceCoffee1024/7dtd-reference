using System;
using System.Globalization;
using SandboxOptions;
using UnityEngine.Scripting;

// Token: 0x02000301 RID: 769
[Preserve]
public class DialogRequirementQuestTierHighest : BaseDialogRequirement
{
	// Token: 0x1700026E RID: 622
	// (get) Token: 0x060015BF RID: 5567 RVA: 0x00047076 File Offset: 0x00045276
	public override BaseDialogRequirement.RequirementTypes RequirementType
	{
		get
		{
			return BaseDialogRequirement.RequirementTypes.QuestTier;
		}
	}

	// Token: 0x060015C0 RID: 5568 RVA: 0x000321FF File Offset: 0x000303FF
	public override string GetRequiredDescription(EntityPlayer player)
	{
		return "";
	}

	// Token: 0x060015C1 RID: 5569 RVA: 0x00081BF8 File Offset: 0x0007FDF8
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
