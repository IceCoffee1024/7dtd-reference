using System;
using System.ComponentModel;
using BhvrAnalyticsServices.Attributes;
using Newtonsoft.Json;

namespace Services.Analytics.Events
{
	// Token: 0x020016D3 RID: 5843
	[EventTitle("QuestReward")]
	public class QuestRewardEventData : BaseEventData
	{
		// Token: 0x170016DE RID: 5854
		// (get) Token: 0x0600B73D RID: 46909 RVA: 0x0043E870 File Offset: 0x0043CA70
		public override string EventType
		{
			get
			{
				return "quest_reward";
			}
		}

		// Token: 0x0600B73E RID: 46910 RVA: 0x0043E878 File Offset: 0x0043CA78
		public static QuestRewardEventData.RewardData BaseRewardToRewardData(BaseReward reward)
		{
			ItemStack rewardItem = reward.GetRewardItem();
			QuestRewardEventData.RewardData rewardData;
			if (reward is RewardExp)
			{
				rewardData = new QuestRewardEventData.RewardData
				{
					reward_type = "XP",
					reward_id = null,
					reward_count = new int?(Convert.ToInt32(reward.Value))
				};
				return rewardData;
			}
			if (reward is RewardItem || reward is RewardLootItem || reward is RewardTreasureItem)
			{
				rewardData = new QuestRewardEventData.RewardData
				{
					reward_type = "Item",
					reward_id = rewardItem.itemValue.ItemClass.Name,
					reward_count = new int?(rewardItem.count)
				};
				return rewardData;
			}
			if (reward is RewardLevel)
			{
				rewardData = new QuestRewardEventData.RewardData
				{
					reward_type = "Level",
					reward_id = null,
					reward_count = new int?(Convert.ToInt32(reward.Value))
				};
				return rewardData;
			}
			if (reward is RewardQuest)
			{
				rewardData = new QuestRewardEventData.RewardData
				{
					reward_type = "Quest",
					reward_id = reward.ID,
					reward_count = null
				};
				return rewardData;
			}
			if (reward is RewardRecipe)
			{
				rewardData = new QuestRewardEventData.RewardData
				{
					reward_type = "Recipe",
					reward_id = reward.ID,
					reward_count = null
				};
				return rewardData;
			}
			if (reward is RewardShowMessageWindow)
			{
				rewardData = new QuestRewardEventData.RewardData
				{
					reward_type = "MessageWindow",
					reward_id = null,
					reward_count = null
				};
				return rewardData;
			}
			if (reward is RewardSkill)
			{
				rewardData = new QuestRewardEventData.RewardData
				{
					reward_type = "Skill",
					reward_id = reward.ID,
					reward_count = null
				};
				return rewardData;
			}
			if (!(reward is RewardSkillPoints))
			{
				throw new ArgumentOutOfRangeException("reward");
			}
			rewardData = new QuestRewardEventData.RewardData
			{
				reward_type = "SkillPoint",
				reward_id = null,
				reward_count = new int?(Convert.ToInt32(reward.Value))
			};
			return rewardData;
		}

		// Token: 0x040088FB RID: 35067
		[JsonProperty(PropertyName = "server_id")]
		public string ServerId;

		// Token: 0x040088FC RID: 35068
		[JsonProperty(PropertyName = "save_id")]
		[Description("Save/world identity")]
		public string SaveId;

		// Token: 0x040088FD RID: 35069
		[JsonProperty(PropertyName = "quest_reward_ts")]
		[DateTimeFormat]
		public string QuestRewardTimestamp;

		// Token: 0x040088FE RID: 35070
		[JsonProperty(PropertyName = "quest_instance_id")]
		[Description("Primary join key — unique per accepted instance; identical across quest_started → quest_progression → quest_removed.")]
		public string QuestInstanceId;

		// Token: 0x040088FF RID: 35071
		[JsonProperty(PropertyName = "quest_id")]
		[Description("Quest ID")]
		public string QuestId;

		// Token: 0x04008900 RID: 35072
		[JsonProperty(PropertyName = "quest_name")]
		[Description("English localization of the quest name")]
		public string QuestName;

		// Token: 0x04008901 RID: 35073
		[JsonProperty(PropertyName = "quest_group")]
		[Description("The quest group (will most likely be used when story mode gets more fleshed out).)")]
		public string QuestGroup;

		// Token: 0x04008902 RID: 35074
		[JsonProperty(PropertyName = "quest_category")]
		[Description("The category of the quest.")]
		public string QuestCategory;

		// Token: 0x04008903 RID: 35075
		[JsonProperty(PropertyName = "quest_tier")]
		[Description("Quest difficulty tier.")]
		public int? QuestTier;

		// Token: 0x04008904 RID: 35076
		[JsonProperty(PropertyName = "items")]
		[Description("The list of items awarded automatically. Typically XP and Dukes.")]
		public object Items;

		// Token: 0x04008905 RID: 35077
		[JsonProperty(PropertyName = "available_rewards")]
		[Description("The list of rewards the player must choose from.")]
		public object AvailableRewards;

		// Token: 0x04008906 RID: 35078
		[JsonProperty(PropertyName = "count_choices")]
		[Description("The number of choices the player gets. ie if Choose 2 it should return 2.")]
		public int CountChoices;

		// Token: 0x04008907 RID: 35079
		[JsonProperty(PropertyName = "rewards_chosen")]
		[Description("The reward(s) chosen by the player")]
		public object RewardsChosen;

		// Token: 0x020016D4 RID: 5844
		[JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
		public struct RewardData
		{
			// Token: 0x04008908 RID: 35080
			public string reward_type;

			// Token: 0x04008909 RID: 35081
			public string reward_id;

			// Token: 0x0400890A RID: 35082
			public int? reward_count;
		}
	}
}
