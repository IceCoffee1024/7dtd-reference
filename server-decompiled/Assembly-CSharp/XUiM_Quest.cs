using System;
using Challenges;
using UnityEngine;

// Token: 0x02001156 RID: 4438
public class XUiM_Quest : XUiModel
{
	// Token: 0x140000E8 RID: 232
	// (add) Token: 0x06008BE3 RID: 35811 RVA: 0x00355444 File Offset: 0x00353644
	// (remove) Token: 0x06008BE4 RID: 35812 RVA: 0x0035547C File Offset: 0x0035367C
	public event XUiEvent_TrackedQuestChanged OnTrackedQuestChanged;

	// Token: 0x17001050 RID: 4176
	// (get) Token: 0x06008BE5 RID: 35813 RVA: 0x003554B1 File Offset: 0x003536B1
	// (set) Token: 0x06008BE6 RID: 35814 RVA: 0x003554B9 File Offset: 0x003536B9
	public Quest TrackedQuest
	{
		get
		{
			return this.trackedQuest;
		}
		set
		{
			this.trackedQuest = value;
			XUiEvent_TrackedQuestChanged onTrackedQuestChanged = this.OnTrackedQuestChanged;
			if (onTrackedQuestChanged == null)
			{
				return;
			}
			onTrackedQuestChanged();
		}
	}

	// Token: 0x140000E9 RID: 233
	// (add) Token: 0x06008BE7 RID: 35815 RVA: 0x003554D4 File Offset: 0x003536D4
	// (remove) Token: 0x06008BE8 RID: 35816 RVA: 0x0035550C File Offset: 0x0035370C
	public event XUiEvent_TrackedQuestChanged OnTrackedChallengeChanged;

	// Token: 0x17001051 RID: 4177
	// (get) Token: 0x06008BE9 RID: 35817 RVA: 0x00355541 File Offset: 0x00353741
	// (set) Token: 0x06008BEA RID: 35818 RVA: 0x0035554C File Offset: 0x0035374C
	public Challenge TrackedChallenge
	{
		get
		{
			return this.trackedChallenge;
		}
		set
		{
			if (this.trackedChallenge != null)
			{
				this.trackedChallenge.IsTracked = false;
				this.trackedChallenge.RemovePrerequisiteHooks();
				this.trackedChallenge.HandleTrackingEnded();
			}
			this.trackedChallenge = value;
			if (this.trackedChallenge != null)
			{
				this.trackedChallenge.IsTracked = true;
				this.trackedChallenge.AddPrerequisiteHooks();
				this.trackedChallenge.HandleTrackingStarted();
				this.trackedChallenge.Owner.Player.PlayerUI.xui.Recipes.TrackedRecipe = null;
				this.trackedChallenge.Owner.Player.QuestJournal.TrackedQuest = null;
			}
			XUiEvent_TrackedQuestChanged onTrackedChallengeChanged = this.OnTrackedChallengeChanged;
			if (onTrackedChallengeChanged == null)
			{
				return;
			}
			onTrackedChallengeChanged();
		}
	}

	// Token: 0x06008BEB RID: 35819 RVA: 0x00355604 File Offset: 0x00353804
	public void HandleTrackedChallengeChanged()
	{
		XUiEvent_TrackedQuestChanged onTrackedChallengeChanged = this.OnTrackedChallengeChanged;
		if (onTrackedChallengeChanged == null)
		{
			return;
		}
		onTrackedChallengeChanged();
	}

	// Token: 0x06008BEC RID: 35820 RVA: 0x00355618 File Offset: 0x00353818
	public static string GetQuestItemRewards(Quest _quest, EntityPlayer _player, string _rewardItemFormat, string _rewardBonusItemFormat)
	{
		if (_quest == null)
		{
			return "";
		}
		string text = "";
		for (int i = 0; i < _quest.Rewards.Count; i++)
		{
			if (!_quest.Rewards[i].isChosenReward && (_quest.Rewards[i] is RewardItem || _quest.Rewards[i] is RewardLootItem))
			{
				BaseReward baseReward = _quest.Rewards[i];
				RewardItem rewardItem = baseReward as RewardItem;
				ItemStack itemStack;
				if (rewardItem == null)
				{
					RewardLootItem rewardLootItem = baseReward as RewardLootItem;
					if (rewardLootItem == null)
					{
						itemStack = null;
					}
					else
					{
						itemStack = rewardLootItem.Item;
					}
				}
				else
				{
					itemStack = rewardItem.Item;
				}
				ItemStack itemStack2 = itemStack;
				int count = itemStack2.count;
				int count2 = _quest.Rewards[i].GetRewardItem().count;
				string localizedItemName = itemStack2.itemValue.ItemClass.GetLocalizedItemName();
				if (count == count2)
				{
					text = text + string.Format(_rewardItemFormat, count2, localizedItemName) + ", ";
				}
				else
				{
					text = text + string.Format(_rewardBonusItemFormat, new object[]
					{
						count2,
						localizedItemName,
						count2 - count,
						Localization.Get("bonus", false, null)
					}) + ", ";
				}
			}
		}
		if (text.Length >= 2)
		{
			text = text.Remove(text.Length - 2);
		}
		return text;
	}

	// Token: 0x06008BED RID: 35821 RVA: 0x0035577C File Offset: 0x0035397C
	public static bool HasQuestRewards(Quest _quest, EntityPlayer _player, bool _isChosen)
	{
		if (_quest == null)
		{
			return false;
		}
		for (int i = 0; i < _quest.Rewards.Count; i++)
		{
			if (_quest.Rewards[i].isChosenReward == _isChosen && !(_quest.Rewards[i] is RewardQuest))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06008BEE RID: 35822 RVA: 0x003557D0 File Offset: 0x003539D0
	public static string GetQuestRewards(Quest _quest, EntityPlayer _player, bool _isChosen, string _rewardItemFormat, string _rewardItemBonusFormat, string _rewardNumberFormat, string _rewardNumberBonusFormat)
	{
		if (_quest == null)
		{
			return "";
		}
		string text = "";
		int num = (_isChosen ? ((int)EffectManager.GetValue(PassiveEffects.QuestRewardOptionCount, null, 1f, _player, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false) + 1) : (-1));
		for (int i = 0; i < _quest.Rewards.Count; i++)
		{
			if (_quest.Rewards[i].isChosenReward == _isChosen)
			{
				if (_isChosen && num-- <= 0)
				{
					break;
				}
				RewardItem rewardItem = _quest.Rewards[i] as RewardItem;
				if (rewardItem != null)
				{
					int count = rewardItem.Item.count;
					int count2 = _quest.Rewards[i].GetRewardItem().count;
					string localizedItemName = rewardItem.Item.itemValue.ItemClass.GetLocalizedItemName();
					if (count == count2)
					{
						text = text + string.Format(_rewardItemFormat, count2, localizedItemName) + ", ";
					}
					else
					{
						text = text + string.Format(_rewardItemBonusFormat, new object[]
						{
							count2,
							localizedItemName,
							count2 - count,
							Localization.Get("bonus", false, null)
						}) + ", ";
					}
				}
				RewardLootItem rewardLootItem = _quest.Rewards[i] as RewardLootItem;
				if (rewardLootItem != null)
				{
					string localizedItemName2 = rewardLootItem.Item.itemValue.ItemClass.GetLocalizedItemName();
					text = text + string.Format(_rewardItemFormat, rewardLootItem.Item.count, localizedItemName2) + ", ";
				}
				else
				{
					RewardExp rewardExp = _quest.Rewards[i] as RewardExp;
					if (rewardExp != null)
					{
						int num2 = 0;
						int num3 = Mathf.FloorToInt((float)Convert.ToInt32(rewardExp.Value) * Progression.XPGain);
						num2 += Mathf.FloorToInt(EffectManager.GetValue(PassiveEffects.PlayerExpGain, null, (float)num3, _player, null, XUiM_Quest.QuestTag, true, true, true, true, true, 1, true, false));
						text = text + string.Format(_rewardNumberFormat, num2, Localization.Get("RewardXP_keyword", false, null)) + ", ";
					}
					else
					{
						RewardSkillPoints rewardSkillPoints = _quest.Rewards[i] as RewardSkillPoints;
						if (rewardSkillPoints != null)
						{
							int num4 = Convert.ToInt32(rewardSkillPoints.Value);
							text = text + string.Format(_rewardNumberFormat, num4, Localization.Get("RewardSkillPoints_keyword", false, null)) + ", ";
						}
					}
				}
			}
		}
		if (text.Length >= 2)
		{
			text = text.Remove(text.Length - 2);
		}
		return text;
	}

	// Token: 0x06008BEF RID: 35823 RVA: 0x00355A58 File Offset: 0x00353C58
	public static bool HasChainQuestRewards(Quest _quest, EntityPlayer _player)
	{
		Quest quest = _quest;
		while (_quest != null)
		{
			Quest quest2 = null;
			for (int i = 0; i < _quest.Rewards.Count; i++)
			{
				if (!_quest.Rewards[i].isChosenReward && _quest.Rewards[i].isChainReward && _quest != quest)
				{
					return true;
				}
				RewardQuest rewardQuest = _quest.Rewards[i] as RewardQuest;
				if (rewardQuest != null && rewardQuest.IsChainQuest)
				{
					quest2 = QuestClass.CreateQuest(rewardQuest.ID);
				}
			}
			_quest = quest2;
		}
		return false;
	}

	// Token: 0x06008BF0 RID: 35824 RVA: 0x00355AE0 File Offset: 0x00353CE0
	public static string GetChainQuestRewards(Quest _quest, EntityPlayer _player, string _rewardItemFormat, string _rewardItemBonusFormat, string _rewardNumberFormat, string _rewardNumberBonusFormat)
	{
		string text = "";
		Quest quest = _quest;
		while (_quest != null)
		{
			Quest quest2 = null;
			for (int i = 0; i < _quest.Rewards.Count; i++)
			{
				if (!_quest.Rewards[i].isChosenReward && _quest.Rewards[i].isChainReward && _quest != quest)
				{
					RewardItem rewardItem = _quest.Rewards[i] as RewardItem;
					if (rewardItem != null)
					{
						int count = rewardItem.Item.count;
						int count2 = _quest.Rewards[i].GetRewardItem().count;
						string localizedItemName = rewardItem.Item.itemValue.ItemClass.GetLocalizedItemName();
						if (count == count2)
						{
							text = text + string.Format(_rewardItemFormat, count2, localizedItemName) + ", ";
						}
						else
						{
							text = text + string.Format(_rewardItemBonusFormat, new object[]
							{
								count2,
								localizedItemName,
								count2 - count,
								Localization.Get("bonus", false, null)
							}) + ", ";
						}
					}
					else
					{
						RewardExp rewardExp = _quest.Rewards[i] as RewardExp;
						if (rewardExp != null)
						{
							int num = 0;
							int num2 = Mathf.FloorToInt((float)Convert.ToInt32(rewardExp.Value) * Progression.XPGain);
							num += Mathf.FloorToInt(EffectManager.GetValue(PassiveEffects.PlayerExpGain, null, (float)num2, _player, null, XUiM_Quest.QuestTag, true, true, true, true, true, 1, true, false));
							text = text + string.Format(_rewardNumberFormat, num, Localization.Get("RewardXP_keyword", false, null)) + ", ";
						}
						else
						{
							RewardSkillPoints rewardSkillPoints = _quest.Rewards[i] as RewardSkillPoints;
							if (rewardSkillPoints != null)
							{
								int num3 = Convert.ToInt32(rewardSkillPoints.Value);
								text = text + string.Format(_rewardNumberFormat, num3, Localization.Get("RewardSkillPoints_keyword", false, null)) + ", ";
							}
						}
					}
				}
				if (_quest.Rewards[i] is RewardQuest)
				{
					RewardQuest rewardQuest = _quest.Rewards[i] as RewardQuest;
					if (rewardQuest.IsChainQuest)
					{
						quest2 = QuestClass.CreateQuest(rewardQuest.ID);
					}
				}
			}
			_quest = quest2;
			if (text.Length >= 2)
			{
				text = text.Remove(text.Length - 2);
			}
		}
		return text;
	}

	// Token: 0x04006770 RID: 26480
	[PublicizedFrom(EAccessModifier.Private)]
	public Quest trackedQuest;

	// Token: 0x04006772 RID: 26482
	[PublicizedFrom(EAccessModifier.Private)]
	public Challenge trackedChallenge;

	// Token: 0x04006773 RID: 26483
	public static readonly FastTags<TagGroup.Global> QuestTag = FastTags<TagGroup.Global>.Parse("quest");
}
