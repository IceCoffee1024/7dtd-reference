using System;
using Challenges;
using UnityEngine;

// Token: 0x02001166 RID: 4454
public class XUiM_Quest : XUiModel
{
	// Token: 0x140000E8 RID: 232
	// (add) Token: 0x06008C37 RID: 35895 RVA: 0x003574F0 File Offset: 0x003556F0
	// (remove) Token: 0x06008C38 RID: 35896 RVA: 0x00357528 File Offset: 0x00355728
	public event XUiEvent_TrackedQuestChanged OnTrackedQuestChanged;

	// Token: 0x1700105B RID: 4187
	// (get) Token: 0x06008C39 RID: 35897 RVA: 0x0035755D File Offset: 0x0035575D
	// (set) Token: 0x06008C3A RID: 35898 RVA: 0x00357565 File Offset: 0x00355765
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
	// (add) Token: 0x06008C3B RID: 35899 RVA: 0x00357580 File Offset: 0x00355780
	// (remove) Token: 0x06008C3C RID: 35900 RVA: 0x003575B8 File Offset: 0x003557B8
	public event XUiEvent_TrackedQuestChanged OnTrackedChallengeChanged;

	// Token: 0x1700105C RID: 4188
	// (get) Token: 0x06008C3D RID: 35901 RVA: 0x003575ED File Offset: 0x003557ED
	// (set) Token: 0x06008C3E RID: 35902 RVA: 0x003575F8 File Offset: 0x003557F8
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

	// Token: 0x06008C3F RID: 35903 RVA: 0x003576B0 File Offset: 0x003558B0
	public void HandleTrackedChallengeChanged()
	{
		XUiEvent_TrackedQuestChanged onTrackedChallengeChanged = this.OnTrackedChallengeChanged;
		if (onTrackedChallengeChanged == null)
		{
			return;
		}
		onTrackedChallengeChanged();
	}

	// Token: 0x06008C40 RID: 35904 RVA: 0x003576C4 File Offset: 0x003558C4
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

	// Token: 0x06008C41 RID: 35905 RVA: 0x00357828 File Offset: 0x00355A28
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

	// Token: 0x06008C42 RID: 35906 RVA: 0x0035787C File Offset: 0x00355A7C
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

	// Token: 0x06008C43 RID: 35907 RVA: 0x00357B04 File Offset: 0x00355D04
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

	// Token: 0x06008C44 RID: 35908 RVA: 0x00357B8C File Offset: 0x00355D8C
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

	// Token: 0x040067B0 RID: 26544
	[PublicizedFrom(EAccessModifier.Private)]
	public Quest trackedQuest;

	// Token: 0x040067B2 RID: 26546
	[PublicizedFrom(EAccessModifier.Private)]
	public Challenge trackedChallenge;

	// Token: 0x040067B3 RID: 26547
	public static readonly FastTags<TagGroup.Global> QuestTag = FastTags<TagGroup.Global>.Parse("quest");
}
