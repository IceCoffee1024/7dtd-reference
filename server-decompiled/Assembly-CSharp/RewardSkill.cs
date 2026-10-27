using System;
using UnityEngine.Scripting;

// Token: 0x02000A0A RID: 2570
[Preserve]
public class RewardSkill : BaseReward
{
	// Token: 0x06004C11 RID: 19473 RVA: 0x001DA2DC File Offset: 0x001D84DC
	public override void SetupReward()
	{
		string text = Localization.Get("RewardSkill_keyword", false, null);
		base.Description = string.Format("{0} {1}", base.ID, text);
		base.ValueText = base.Value;
		base.Icon = base.OwnerQuest.OwnerJournal.OwnerPlayer.Progression.GetProgressionValue(base.ID).ProgressionClass.Icon;
	}

	// Token: 0x06004C12 RID: 19474 RVA: 0x001DA34C File Offset: 0x001D854C
	public override void GiveReward(EntityPlayer player)
	{
		ProgressionValue progressionValue = player.Progression.GetProgressionValue(base.ID);
		int num = Convert.ToInt32(base.Value);
		if (progressionValue != null)
		{
			if (progressionValue.Level + num > progressionValue.ProgressionClass.MaxLevel)
			{
				progressionValue.Level = progressionValue.ProgressionClass.MaxLevel;
			}
			else
			{
				progressionValue.Level += num;
			}
			if (progressionValue.ProgressionClass.IsPerk)
			{
				player.MinEventContext.ProgressionValue = progressionValue;
				player.FireEvent(MinEventTypes.onPerkLevelChanged, true);
			}
			if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
			{
				SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(NetPackageManager.GetPackage<NetPackageEntitySetSkillLevelServer>().Setup(player.entityId, progressionValue.Name, progressionValue.Level), false);
			}
		}
	}

	// Token: 0x06004C13 RID: 19475 RVA: 0x001DA408 File Offset: 0x001D8608
	public override BaseReward Clone()
	{
		RewardSkill rewardSkill = new RewardSkill();
		base.CopyValues(rewardSkill);
		return rewardSkill;
	}
}
