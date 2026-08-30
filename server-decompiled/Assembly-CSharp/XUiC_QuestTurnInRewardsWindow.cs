using System;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000FE1 RID: 4065
[Preserve]
public class XUiC_QuestTurnInRewardsWindow : XUiController
{
	// Token: 0x17000ED9 RID: 3801
	// (get) Token: 0x0600802A RID: 32810 RVA: 0x0030A6F2 File Offset: 0x003088F2
	// (set) Token: 0x0600802B RID: 32811 RVA: 0x0030A6FA File Offset: 0x003088FA
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

	// Token: 0x17000EDA RID: 3802
	// (get) Token: 0x0600802C RID: 32812 RVA: 0x0030A72B File Offset: 0x0030892B
	// (set) Token: 0x0600802D RID: 32813 RVA: 0x0030A733 File Offset: 0x00308933
	public XUiC_ItemInfoWindow InfoWindow { get; set; }

	// Token: 0x17000EDB RID: 3803
	// (get) Token: 0x0600802E RID: 32814 RVA: 0x0030A73C File Offset: 0x0030893C
	// (set) Token: 0x0600802F RID: 32815 RVA: 0x0030A744 File Offset: 0x00308944
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

	// Token: 0x06008030 RID: 32816 RVA: 0x0030A7D4 File Offset: 0x003089D4
	public override void Init()
	{
		base.Init();
		this.xuiQuestDescriptionLabel = Localization.Get("xuiDescriptionLabel", false, null);
		this.entryList = base.GetChildById("rectOptions").GetChildById("gridOptions").GetChildrenByType<XUiC_QuestTurnInEntry>(null);
		this.btnAccept = base.GetChildById("rectAccept").GetChildById("btnAccept");
		((XUiV_Button)this.btnAccept.GetChildById("clickable").ViewComponent).Controller.OnPress += this.BtnAccept_OnPress;
	}

	// Token: 0x06008031 RID: 32817 RVA: 0x0030A868 File Offset: 0x00308A68
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

	// Token: 0x06008032 RID: 32818 RVA: 0x0030A9DE File Offset: 0x00308BDE
	public override void OnOpen()
	{
		base.OnOpen();
		this.SelectedEntry = null;
		this.CurrentQuest = this.xui.Dialog.QuestTurnIn;
	}

	// Token: 0x06008033 RID: 32819 RVA: 0x0030AA04 File Offset: 0x00308C04
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

	// Token: 0x06008034 RID: 32820 RVA: 0x0030AC04 File Offset: 0x00308E04
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

	// Token: 0x06008035 RID: 32821 RVA: 0x0030AC28 File Offset: 0x00308E28
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

	// Token: 0x06008036 RID: 32822 RVA: 0x0030ACC4 File Offset: 0x00308EC4
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

	// Token: 0x06008037 RID: 32823 RVA: 0x0030B394 File Offset: 0x00309594
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

	// Token: 0x04005EED RID: 24301
	[PublicizedFrom(EAccessModifier.Private)]
	public QuestClass questClass;

	// Token: 0x04005EEE RID: 24302
	[PublicizedFrom(EAccessModifier.Private)]
	public string xuiQuestDescriptionLabel;

	// Token: 0x04005EEF RID: 24303
	[PublicizedFrom(EAccessModifier.Private)]
	public string rewardNumberFormat = "[DECEA3]{0}[-]";

	// Token: 0x04005EF0 RID: 24304
	[PublicizedFrom(EAccessModifier.Private)]
	public string rewardNumberBonusFormat = "[DECEA3]{0}[-] ([DECEA3]{1}[-] {2})";

	// Token: 0x04005EF1 RID: 24305
	[PublicizedFrom(EAccessModifier.Private)]
	public string rewardItemFormat = "[DECEA3]{0}[-] {1}";

	// Token: 0x04005EF2 RID: 24306
	[PublicizedFrom(EAccessModifier.Private)]
	public string rewardItemBonusFormat = "[DECEA3]{0}[-] {1} ([DECEA3]{2}[-] {3})";

	// Token: 0x04005EF3 RID: 24307
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_QuestTurnInEntry[] entryList;

	// Token: 0x04005EF4 RID: 24308
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Button acceptButton;

	// Token: 0x04005EF5 RID: 24309
	[PublicizedFrom(EAccessModifier.Private)]
	public List<BaseReward> rewardList = new List<BaseReward>();

	// Token: 0x04005EF6 RID: 24310
	public EntityNPC NPC;

	// Token: 0x04005EF7 RID: 24311
	[PublicizedFrom(EAccessModifier.Private)]
	public int optionCount;

	// Token: 0x04005EF8 RID: 24312
	[PublicizedFrom(EAccessModifier.Private)]
	public Quest currentQuest;

	// Token: 0x04005EFA RID: 24314
	[PublicizedFrom(EAccessModifier.Private)]
	public List<XUiC_QuestTurnInEntry> selectedEntryList = new List<XUiC_QuestTurnInEntry>();

	// Token: 0x04005EFB RID: 24315
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_QuestTurnInEntry selectedEntry;

	// Token: 0x04005EFC RID: 24316
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController btnAccept;

	// Token: 0x04005EFD RID: 24317
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<string, string> questtitleFormatter = new CachedStringFormatter<string, string>((string _s, string _s1) => string.Format("{0} : {1}", _s, _s1));
}
