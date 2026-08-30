using System;
using UnityEngine.Scripting;

// Token: 0x020009F6 RID: 2550
[Preserve]
public class RewardSkill : BaseReward
{
	// Token: 0x06004C1E RID: 19486 RVA: 0x001D7F44 File Offset: 0x001D6144
	public override void SetupReward()
	{
		string text = Localization.Get("RewardSkill_keyword", false, null);
		base.Description = string.Format("{0} {1}", base.ID, text);
		base.ValueText = base.Value;
		base.Icon = base.OwnerQuest.OwnerJournal.OwnerPlayer.Progression.GetProgressionValue(base.ID).ProgressionClass.Icon;
	}

	// Token: 0x06004C1F RID: 19487 RVA: 0x001D7FB4 File Offset: 0x001D61B4
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

	// Token: 0x06004C20 RID: 19488 RVA: 0x001D8070 File Offset: 0x001D6270
	public override BaseReward Clone()
	{
		RewardSkill rewardSkill = new RewardSkill();
		base.CopyValues(rewardSkill);
		return rewardSkill;
	}
}
