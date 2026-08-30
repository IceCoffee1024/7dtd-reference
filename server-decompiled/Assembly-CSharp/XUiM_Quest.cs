using System;
using Challenges;
using UnityEngine;

// Token: 0x0200113E RID: 4414
public class XUiM_Quest : XUiModel
{
	// Token: 0x140000E9 RID: 233
	// (add) Token: 0x06008B78 RID: 35704 RVA: 0x0035101C File Offset: 0x0034F21C
	// (remove) Token: 0x06008B79 RID: 35705 RVA: 0x00351054 File Offset: 0x0034F254
	public event XUiEvent_TrackedQuestChanged OnTrackedQuestChanged;

	// Token: 0x1700101B RID: 4123
	// (get) Token: 0x06008B7A RID: 35706 RVA: 0x00351089 File Offset: 0x0034F289
	// (set) Token: 0x06008B7B RID: 35707 RVA: 0x00351091 File Offset: 0x0034F291
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

	// Token: 0x140000EA RID: 234
	// (add) Token: 0x06008B7C RID: 35708 RVA: 0x003510AC File Offset: 0x0034F2AC
	// (remove) Token: 0x06008B7D RID: 35709 RVA: 0x003510E4 File Offset: 0x0034F2E4
	public event XUiEvent_TrackedQuestChanged OnTrackedChallengeChanged;

	// Token: 0x1700101C RID: 4124
	// (get) Token: 0x06008B7E RID: 35710 RVA: 0x00351119 File Offset: 0x0034F319
	// (set) Token: 0x06008B7F RID: 35711 RVA: 0x00351124 File Offset: 0x0034F324
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

	// Token: 0x06008B80 RID: 35712 RVA: 0x003511DC File Offset: 0x0034F3DC
	public void HandleTrackedChallengeChanged()
	{
		XUiEvent_TrackedQuestChanged onTrackedChallengeChanged = this.OnTrackedChallengeChanged;
		if (onTrackedChallengeChanged == null)
		{
			return;
		}
		onTrackedChallengeChanged();
	}

	// Token: 0x06008B81 RID: 35713 RVA: 0x003511F0 File Offset: 0x0034F3F0
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

	// Token: 0x06008B82 RID: 35714 RVA: 0x00351354 File Offset: 0x0034F554
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

	// Token: 0x06008B83 RID: 35715 RVA: 0x003513A8 File Offset: 0x0034F5A8
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

	// Token: 0x06008B84 RID: 35716 RVA: 0x00351630 File Offset: 0x0034F830
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

	// Token: 0x06008B85 RID: 35717 RVA: 0x003516B8 File Offset: 0x0034F8B8
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

	// Token: 0x0400671B RID: 26395
	[PublicizedFrom(EAccessModifier.Private)]
	public Quest trackedQuest;

	// Token: 0x0400671D RID: 26397
	[PublicizedFrom(EAccessModifier.Private)]
	public Challenge trackedChallenge;

	// Token: 0x0400671E RID: 26398
	public static readonly FastTags<TagGroup.Global> QuestTag = FastTags<TagGroup.Global>.Parse("quest");
}
