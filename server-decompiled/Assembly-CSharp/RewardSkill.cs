using System;
using UnityEngine.Scripting;

// Token: 0x020009E5 RID: 2533
[Preserve]
public class RewardSkill : BaseReward
{
	// Token: 0x06004B98 RID: 19352 RVA: 0x001D575C File Offset: 0x001D395C
	public override void SetupReward()
	{
		string text = Localization.Get("RewardSkill_keyword", false, null);
		base.Description = string.Format("{0} {1}", base.ID, text);
		base.ValueText = base.Value;
		base.Icon = base.OwnerQuest.OwnerJournal.OwnerPlayer.Progression.GetProgressionValue(base.ID).ProgressionClass.Icon;
	}

	// Token: 0x06004B99 RID: 19353 RVA: 0x001D57CC File Offset: 0x001D39CC
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

	// Token: 0x06004B9A RID: 19354 RVA: 0x001D5888 File Offset: 0x001D3A88
	public override BaseReward Clone()
	{
		RewardSkill rewardSkill = new RewardSkill();
		base.CopyValues(rewardSkill);
		return rewardSkill;
	}
}
