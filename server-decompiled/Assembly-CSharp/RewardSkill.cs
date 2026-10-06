using System;
using UnityEngine.Scripting;

// Token: 0x02000A00 RID: 2560
[Preserve]
public class RewardSkill : BaseReward
{
	// Token: 0x06004BED RID: 19437 RVA: 0x001D91F4 File Offset: 0x001D73F4
	public override void SetupReward()
	{
		string text = Localization.Get("RewardSkill_keyword", false, null);
		base.Description = string.Format("{0} {1}", base.ID, text);
		base.ValueText = base.Value;
		base.Icon = base.OwnerQuest.OwnerJournal.OwnerPlayer.Progression.GetProgressionValue(base.ID).ProgressionClass.Icon;
	}

	// Token: 0x06004BEE RID: 19438 RVA: 0x001D9264 File Offset: 0x001D7464
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

	// Token: 0x06004BEF RID: 19439 RVA: 0x001D9320 File Offset: 0x001D7520
	public override BaseReward Clone()
	{
		RewardSkill rewardSkill = new RewardSkill();
		base.CopyValues(rewardSkill);
		return rewardSkill;
	}
}
