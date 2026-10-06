using System;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02001009 RID: 4105
[Preserve]
public class XUiC_QuestTurnInRewardsWindow : XUiController
{
	// Token: 0x17000F39 RID: 3897
	// (get) Token: 0x0600812B RID: 33067 RVA: 0x00311F0E File Offset: 0x0031010E
	// (set) Token: 0x0600812C RID: 33068 RVA: 0x00311F16 File Offset: 0x00310116
	public Quest CurrentQuest
	{
		get
		{
			return this.currentQuest;
		}
		set
		{
			this.currentQuest = value;
			this.questClass = ((value != null) ? QuestClass.GetQuest(this.currentQuest.ID) : null);
			base.RefreshBindings();
			this.SetupOptions();
		}
	}

	// Token: 0x17000F3A RID: 3898
	// (get) Token: 0x0600812D RID: 33069 RVA: 0x00311F47 File Offset: 0x00310147
	// (set) Token: 0x0600812E RID: 33070 RVA: 0x00311F4F File Offset: 0x0031014F
	public XUiC_ItemInfoWindow InfoWindow { get; set; }

	// Token: 0x17000F3B RID: 3899
	// (get) Token: 0x0600812F RID: 33071 RVA: 0x00311F58 File Offset: 0x00310158
	// (set) Token: 0x06008130 RID: 33072 RVA: 0x00311F60 File Offset: 0x00310160
	public XUiC_QuestTurnInEntry SelectedEntry
	{
		get
		{
			return this.selectedEntry;
		}
		set
		{
			if (this.selectedEntry != null)
			{
				this.selectedEntry.IsSelected = false;
			}
			this.selectedEntry = value;
			if (this.selectedEntry == null)
			{
				this.InfoWindow.SetItemStack(null, true);
				return;
			}
			this.selectedEntry.IsSelected = true;
			if (this.selectedEntry.Reward is RewardItem || this.selectedEntry.Reward is RewardLootItem)
			{
				this.InfoWindow.SetItemStack(this.selectedEntry, true);
				return;
			}
			this.InfoWindow.SetItemStack(null, true);
		}
	}

	// Token: 0x06008131 RID: 33073 RVA: 0x00311FF0 File Offset: 0x003101F0
	public override void Init()
	{
		base.Init();
		this.xuiQuestDescriptionLabel = Localization.Get("xuiDescriptionLabel", false, null);
		this.entryList = base.GetChildById("rectOptions").GetChildById("gridOptions").GetChildrenByType<XUiC_QuestTurnInEntry>(null);
		this.btnAccept = base.GetChildById("rectAccept").GetChildById("btnAccept");
		((XUiV_Button)this.btnAccept.GetChildById("clickable").ViewComponent).Controller.OnPress += this.BtnAccept_OnPress;
	}

	// Token: 0x06008132 RID: 33074 RVA: 0x00312084 File Offset: 0x00310284
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnAccept_OnPress(XUiController _sender, int _mouseButton)
	{
		float value = EffectManager.GetValue(PassiveEffects.QuestRewardChoiceCount, null, 1f, this.xui.playerUI.entityPlayer, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false);
		if (this.selectedEntry != null)
		{
			if (this.selectedEntryList.Contains(this.selectedEntry))
			{
				this.selectedEntry.Chosen = false;
				this.selectedEntryList.Remove(this.selectedEntry);
			}
			else
			{
				this.selectedEntry.Chosen = true;
				this.selectedEntryList.Add(this.selectedEntry);
			}
		}
		if (this.optionCount <= 1 || (float)this.selectedEntryList.Count == value)
		{
			List<BaseReward> list = new List<BaseReward>();
			for (int i = 0; i < this.selectedEntryList.Count; i++)
			{
				list.Add(this.selectedEntryList[i].Reward);
			}
			if (this.CurrentQuest.CanTurnInQuest(list))
			{
				this.CurrentQuest.RefreshQuestCompletion(QuestClass.CompletionTypes.TurnIn, list, true, this.NPC);
				(base.WindowGroup.Controller as XUiC_QuestTurnInWindowGroup).TryNextComplete();
			}
			else
			{
				GameManager.ShowTooltip(this.xui.playerUI.entityPlayer, Localization.Get("ttClearInventoryForTurnIn", false, null), string.Empty, "ui_denied", null, false, false, 0f);
				this.selectedEntry.Chosen = false;
				this.selectedEntryList.Remove(this.selectedEntry);
			}
		}
		base.RefreshBindings();
	}

	// Token: 0x06008133 RID: 33075 RVA: 0x003121FA File Offset: 0x003103FA
	public override void OnOpen()
	{
		base.OnOpen();
		this.SelectedEntry = null;
		this.CurrentQuest = this.xui.Dialog.QuestTurnIn;
	}

	// Token: 0x06008134 RID: 33076 RVA: 0x00312220 File Offset: 0x00310420
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetupOptions()
	{
		float value = EffectManager.GetValue(PassiveEffects.QuestRewardOptionCount, null, (float)this.currentQuest.QuestClass.RewardChoicesCount, this.xui.playerUI.entityPlayer, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false);
		this.optionCount = 0;
		int num = 0;
		if (this.selectedEntry != null)
		{
			this.SelectedEntry = null;
		}
		this.selectedEntryList.Clear();
		this.rewardList.Clear();
		for (int i = 0; i < this.entryList.Length; i++)
		{
			this.entryList[i].OnPress -= this.TurnInEntryPressed;
			this.entryList[i].SetBaseReward(null);
		}
		for (int j = 0; j < this.currentQuest.Rewards.Count; j++)
		{
			this.rewardList.Add(this.currentQuest.Rewards[j]);
		}
		this.rewardList = this.rewardList.OrderBy<BaseReward, byte>((BaseReward o) => o.RewardIndex).ToList<BaseReward>();
		for (int k = 0; k < this.rewardList.Count; k++)
		{
			BaseReward baseReward = this.rewardList[k];
			this.entryList[num].OnPress -= this.TurnInEntryPressed;
			if (baseReward.isChosenReward)
			{
				this.entryList[num].SetBaseReward(baseReward);
				this.entryList[num].Chosen = false;
				this.entryList[num++].OnPress += this.TurnInEntryPressed;
				this.optionCount++;
				if (value == (float)num || num >= this.entryList.Length)
				{
					break;
				}
			}
		}
		this.entryList[0].SelectCursorElement(true, false);
		if (this.optionCount == 1)
		{
			XUiC_QuestTurnInEntry xuiC_QuestTurnInEntry = this.entryList[0];
			this.SelectedEntry = xuiC_QuestTurnInEntry;
			base.RefreshBindings();
		}
	}

	// Token: 0x06008135 RID: 33077 RVA: 0x00312420 File Offset: 0x00310620
	[PublicizedFrom(EAccessModifier.Private)]
	public void TurnInEntryPressed(XUiController _sender, int _mouseButton)
	{
		XUiC_QuestTurnInEntry xuiC_QuestTurnInEntry = _sender as XUiC_QuestTurnInEntry;
		if (xuiC_QuestTurnInEntry != null)
		{
			this.SelectedEntry = xuiC_QuestTurnInEntry;
			base.RefreshBindings();
		}
	}

	// Token: 0x06008136 RID: 33078 RVA: 0x00312444 File Offset: 0x00310644
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleEntryPressed(XUiC_QuestTurnInEntry entry)
	{
		float value = EffectManager.GetValue(PassiveEffects.QuestRewardChoiceCount, null, 1f, this.xui.playerUI.entityPlayer, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false);
		if (this.selectedEntryList.Contains(entry))
		{
			entry.IsSelected = false;
			this.selectedEntryList.Remove(entry);
			if (entry == this.SelectedEntry)
			{
				this.SelectedEntry = null;
				return;
			}
		}
		else if ((float)this.selectedEntryList.Count < value)
		{
			this.selectedEntryList.Add(entry);
			this.SelectedEntry = entry;
			entry.IsSelected = true;
		}
	}

	// Token: 0x06008137 RID: 33079 RVA: 0x003124E0 File Offset: 0x003106E0
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string value, string bindingName)
	{
		uint num = <PrivateImplementationDetails>.ComputeStringHash(bindingName);
		if (num <= 2646005107U)
		{
			if (num <= 1061431296U)
			{
				if (num != 161079706U)
				{
					if (num != 213234470U)
					{
						if (num == 1061431296U)
						{
							if (bindingName == "questitemrewardstitle")
							{
								value = Localization.Get("xuiItems", false, null);
								return true;
							}
						}
					}
					else if (bindingName == "acceptbuttontext")
					{
						if (this.currentQuest == null)
						{
							value = "";
						}
						if (this.SelectedEntry != null)
						{
							if (this.SelectedEntry.Chosen)
							{
								value = Localization.Get("xuiUnSelect", false, null);
							}
							else
							{
								float value2 = EffectManager.GetValue(PassiveEffects.QuestRewardChoiceCount, null, 1f, this.xui.playerUI.entityPlayer, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false);
								if ((float)(this.selectedEntryList.Count + 1) == value2)
								{
									value = Localization.Get("lblContextActionComplete", false, null);
								}
								else
								{
									value = Localization.Get("mmBtnSelect", false, null);
								}
							}
						}
						else
						{
							value = Localization.Get("mmBtnSelect", false, null);
						}
						return true;
					}
				}
				else if (bindingName == "sharedbyname")
				{
					if (this.currentQuest == null)
					{
						value = "";
					}
					else
					{
						PersistentPlayerData playerDataFromEntityID = GameManager.Instance.persistentPlayers.GetPlayerDataFromEntityID(this.currentQuest.SharedOwnerID);
						if (playerDataFromEntityID != null)
						{
							value = GameUtils.SafeStringFormat(playerDataFromEntityID.PlayerName.DisplayName);
						}
						else
						{
							value = "";
						}
					}
					return true;
				}
			}
			else if (num <= 1895377404U)
			{
				if (num != 1468586257U)
				{
					if (num == 1895377404U)
					{
						if (bindingName == "questhasxpreward")
						{
							if (this.currentQuest == null)
							{
								value = "false";
							}
							else
							{
								int num2 = 0;
								for (int i = 0; i < this.currentQuest.Rewards.Count; i++)
								{
									if (this.currentQuest.Rewards[i] is RewardExp && !this.currentQuest.Rewards[i].isChosenReward)
									{
										num2++;
									}
								}
								value = (num2 > 0).ToString();
							}
							return true;
						}
					}
				}
				else if (bindingName == "questsubtitle")
				{
					value = ((this.currentQuest != null) ? this.questClass.SubTitle : "");
					return true;
				}
			}
			else if (num != 1985695849U)
			{
				if (num == 2646005107U)
				{
					if (bindingName == "chosentitle")
					{
						if (this.currentQuest == null)
						{
							value = "";
						}
						if (XUiM_Quest.HasQuestRewards(this.currentQuest, this.xui.playerUI.entityPlayer, true))
						{
							float value3 = EffectManager.GetValue(PassiveEffects.QuestRewardChoiceCount, null, 1f, this.xui.playerUI.entityPlayer, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false);
							value = ((value3 == 1f) ? Localization.Get("xuiChooseOne", false, null) : Localization.Get("xuiChooseTwo", false, null));
						}
						else
						{
							value = "";
						}
						return true;
					}
				}
			}
			else if (bindingName == "questcategory")
			{
				value = ((this.currentQuest != null) ? this.questClass.Category : "");
				return true;
			}
		}
		else if (num <= 2940790865U)
		{
			if (num <= 2718428424U)
			{
				if (num != 2680020434U)
				{
					if (num == 2718428424U)
					{
						if (bindingName == "questhasitemrewards")
						{
							if (this.currentQuest == null)
							{
								value = "false";
							}
							else
							{
								int num3 = 0;
								for (int j = 0; j < this.currentQuest.Rewards.Count; j++)
								{
									if ((this.currentQuest.Rewards[j] is RewardItem || this.currentQuest.Rewards[j] is RewardLootItem) && !this.currentQuest.Rewards[j].isChosenReward)
									{
										num3++;
									}
								}
								value = (num3 > 0).ToString();
							}
							return true;
						}
					}
				}
				else if (bindingName == "questxpreward")
				{
					if (this.currentQuest == null)
					{
						value = "";
					}
					else
					{
						int num4 = 0;
						for (int k = 0; k < this.currentQuest.Rewards.Count; k++)
						{
							if (this.currentQuest.Rewards[k] is RewardExp)
							{
								int num5 = Mathf.FloorToInt((float)Convert.ToInt32(this.currentQuest.Rewards[k].Value) * Progression.XPGain);
								num4 += Mathf.FloorToInt(EffectManager.GetValue(PassiveEffects.PlayerExpGain, null, (float)num5, this.xui.playerUI.entityPlayer, null, XUiM_Quest.QuestTag, true, true, true, true, true, 1, true, false));
							}
						}
						value = string.Format(this.rewardNumberFormat, num4);
					}
					return true;
				}
			}
			else if (num != 2745591350U)
			{
				if (num == 2940790865U)
				{
					if (bindingName == "questdescription")
					{
						value = ((this.currentQuest != null) ? this.currentQuest.GetParsedText(this.questClass.Description) : "");
						return true;
					}
				}
			}
			else if (bindingName == "questitemrewards")
			{
				value = XUiM_Quest.GetQuestItemRewards(this.currentQuest, this.xui.playerUI.entityPlayer, this.rewardItemFormat, this.rewardItemBonusFormat);
				return true;
			}
		}
		else if (num <= 3357817217U)
		{
			if (num != 3047389681U)
			{
				if (num == 3357817217U)
				{
					if (bindingName == "questcompletetext")
					{
						value = ((this.currentQuest != null) ? this.questClass.CompleteText : "Needs real complete text.");
						return true;
					}
				}
			}
			else if (bindingName == "questtitle")
			{
				value = ((this.currentQuest != null) ? this.questtitleFormatter.Format(this.questClass.Category, this.questClass.SubTitle) : this.xuiQuestDescriptionLabel);
				return true;
			}
		}
		else if (num != 3461297649U)
		{
			if (num == 4060322893U)
			{
				if (bindingName == "showempty")
				{
					value = (this.currentQuest == null).ToString();
					return true;
				}
			}
		}
		else if (bindingName == "questxptitle")
		{
			if (this.currentQuest == null)
			{
				value = "";
			}
			else
			{
				value = Localization.Get("RewardXP_keyword", false, null);
			}
			return true;
		}
		return base.GetBindingValueInternal(ref value, bindingName);
	}

	// Token: 0x06008138 RID: 33080 RVA: 0x00312BB0 File Offset: 0x00310DB0
	public override bool ParseAttribute(string name, string value)
	{
		bool flag = base.ParseAttribute(name, value);
		if (!flag)
		{
			if (!(name == "xp_format"))
			{
				if (!(name == "xp_bonus_format"))
				{
					if (!(name == "item_format"))
					{
						if (!(name == "item_bonus_format"))
						{
							return false;
						}
						this.rewardItemBonusFormat = value;
					}
					else
					{
						this.rewardItemFormat = value;
					}
				}
				else
				{
					this.rewardNumberBonusFormat = value;
				}
			}
			else
			{
				this.rewardNumberFormat = value;
			}
			return true;
		}
		return flag;
	}

	// Token: 0x04005FDD RID: 24541
	[PublicizedFrom(EAccessModifier.Private)]
	public QuestClass questClass;

	// Token: 0x04005FDE RID: 24542
	[PublicizedFrom(EAccessModifier.Private)]
	public string xuiQuestDescriptionLabel;

	// Token: 0x04005FDF RID: 24543
	[PublicizedFrom(EAccessModifier.Private)]
	public string rewardNumberFormat = "[DECEA3]{0}[-]";

	// Token: 0x04005FE0 RID: 24544
	[PublicizedFrom(EAccessModifier.Private)]
	public string rewardNumberBonusFormat = "[DECEA3]{0}[-] ([DECEA3]{1}[-] {2})";

	// Token: 0x04005FE1 RID: 24545
	[PublicizedFrom(EAccessModifier.Private)]
	public string rewardItemFormat = "[DECEA3]{0}[-] {1}";

	// Token: 0x04005FE2 RID: 24546
	[PublicizedFrom(EAccessModifier.Private)]
	public string rewardItemBonusFormat = "[DECEA3]{0}[-] {1} ([DECEA3]{2}[-] {3})";

	// Token: 0x04005FE3 RID: 24547
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_QuestTurnInEntry[] entryList;

	// Token: 0x04005FE4 RID: 24548
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Button acceptButton;

	// Token: 0x04005FE5 RID: 24549
	[PublicizedFrom(EAccessModifier.Private)]
	public List<BaseReward> rewardList = new List<BaseReward>();

	// Token: 0x04005FE6 RID: 24550
	public EntityNPC NPC;

	// Token: 0x04005FE7 RID: 24551
	[PublicizedFrom(EAccessModifier.Private)]
	public int optionCount;

	// Token: 0x04005FE8 RID: 24552
	[PublicizedFrom(EAccessModifier.Private)]
	public Quest currentQuest;

	// Token: 0x04005FEA RID: 24554
	[PublicizedFrom(EAccessModifier.Private)]
	public List<XUiC_QuestTurnInEntry> selectedEntryList = new List<XUiC_QuestTurnInEntry>();

	// Token: 0x04005FEB RID: 24555
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_QuestTurnInEntry selectedEntry;

	// Token: 0x04005FEC RID: 24556
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController btnAccept;

	// Token: 0x04005FED RID: 24557
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<string, string> questtitleFormatter = new CachedStringFormatter<string, string>((string _s, string _s1) => string.Format("{0} : {1}", _s, _s1));
}
