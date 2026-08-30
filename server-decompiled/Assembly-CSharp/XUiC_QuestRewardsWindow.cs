using System;
using UnityEngine.Scripting;

// Token: 0x02000FEC RID: 4076
[Preserve]
public class XUiC_QuestRewardsWindow : XUiController
{
	// Token: 0x17000F00 RID: 3840
	// (get) Token: 0x060080A5 RID: 32933 RVA: 0x0030B8B5 File Offset: 0x00309AB5
	// (set) Token: 0x060080A6 RID: 32934 RVA: 0x0030B8BD File Offset: 0x00309ABD
	public Quest CurrentQuest
	{
		get
		{
			return this.currentQuest;
		}
		set
		{
			this.currentQuest = value;
			this.questClass = ((this.currentQuest != null) ? QuestClass.GetQuest(this.currentQuest.ID) : null);
			base.RefreshBindings();
		}
	}

	// Token: 0x060080A7 RID: 32935 RVA: 0x0030B8ED File Offset: 0x00309AED
	public override void Init()
	{
		base.Init();
		this.rewardList = base.GetChildByType<XUiC_QuestRewardList>();
	}

	// Token: 0x060080A8 RID: 32936 RVA: 0x0030B904 File Offset: 0x00309B04
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string value, string bindingName)
	{
		uint num = <PrivateImplementationDetails>.ComputeStringHash(bindingName);
		if (num <= 2432099586U)
		{
			if (num <= 378804592U)
			{
				if (num != 264092286U)
				{
					if (num == 378804592U)
					{
						if (bindingName == "commonrewards")
						{
							value = "";
							if (this.currentQuest != null)
							{
								string questRewards = XUiM_Quest.GetQuestRewards(this.currentQuest, this.xui.playerUI.entityPlayer, false, this.rewardItemFormat, this.rewardItemBonusFormat, this.rewardNumberFormat, this.rewardNumberBonusFormat);
								if (questRewards == "")
								{
									value = Localization.Get("none", false, null);
								}
								else
								{
									value = questRewards;
								}
							}
							return true;
						}
					}
				}
				else if (bindingName == "chainrewards")
				{
					value = "";
					if (this.currentQuest != null)
					{
						string chainQuestRewards = XUiM_Quest.GetChainQuestRewards(this.currentQuest, this.xui.playerUI.entityPlayer, this.rewardItemFormat, this.rewardItemBonusFormat, this.rewardNumberFormat, this.rewardNumberBonusFormat);
						if (chainQuestRewards == "")
						{
							value = Localization.Get("none", false, null);
						}
						else
						{
							value = chainQuestRewards;
						}
					}
					return true;
				}
			}
			else if (num != 614962720U)
			{
				if (num != 986815653U)
				{
					if (num == 2432099586U)
					{
						if (bindingName == "commontitle")
						{
							value = ((this.currentQuest != null) ? (Localization.Get("xuiRewards", false, null) + ": ") : "");
							return true;
						}
					}
				}
				else if (bindingName == "chosenrewards")
				{
					value = "";
					if (this.currentQuest != null)
					{
						string questRewards2 = XUiM_Quest.GetQuestRewards(this.currentQuest, this.xui.playerUI.entityPlayer, true, this.rewardItemFormat, this.rewardItemBonusFormat, this.rewardNumberFormat, this.rewardNumberBonusFormat);
						if (questRewards2 == "")
						{
							value = Localization.Get("none", false, null);
						}
						else
						{
							value = questRewards2;
						}
					}
					return true;
				}
			}
			else if (bindingName == "chaintitle")
			{
				value = ((this.currentQuest != null) ? (Localization.Get("RewardTypeChainQuest", false, null) + ": ") : "");
				return true;
			}
		}
		else if (num <= 3033834579U)
		{
			if (num != 2646005107U)
			{
				if (num != 2730462270U)
				{
					if (num == 3033834579U)
					{
						if (bindingName == "finishtime")
						{
							value = "";
							if (this.currentQuest != null && this.currentQuest.FinishTime > 0UL)
							{
								ValueTuple<int, int, int> valueTuple = GameUtils.WorldTimeToElements(this.currentQuest.FinishTime);
								int item = valueTuple.Item1;
								int item2 = valueTuple.Item2;
								int item3 = valueTuple.Item3;
								value = this.daytimeFormatter.Format((this.currentQuest.CurrentState == Quest.QuestState.Completed) ? Localization.Get("completed", false, null) : Localization.Get("failed", false, null), item, item2, item3);
							}
							return true;
						}
					}
				}
				else if (bindingName == "questname")
				{
					value = ((this.currentQuest != null) ? this.questClass.Name : "");
					return true;
				}
			}
			else if (bindingName == "chosentitle")
			{
				if (this.currentQuest == null)
				{
					value = "";
				}
				if (XUiM_Quest.HasQuestRewards(this.currentQuest, this.xui.playerUI.entityPlayer, true))
				{
					float value2 = EffectManager.GetValue(PassiveEffects.QuestRewardChoiceCount, null, 1f, this.xui.playerUI.entityPlayer, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false);
					value = ((value2 == 1f) ? Localization.Get("xuiChooseOne", false, null) : Localization.Get("xuiChooseTwo", false, null));
				}
				else
				{
					value = "";
				}
				return true;
			}
		}
		else if (num != 3047389681U)
		{
			if (num != 3231221182U)
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
			else if (bindingName == "showquest")
			{
				value = (this.currentQuest != null).ToString();
				return true;
			}
		}
		else if (bindingName == "questtitle")
		{
			value = ((this.currentQuest != null) ? this.questClass.SubTitle : "");
			return true;
		}
		return base.GetBindingValueInternal(ref value, bindingName);
	}

	// Token: 0x060080A9 RID: 32937 RVA: 0x0030BD9F File Offset: 0x00309F9F
	public void SetQuest(XUiC_QuestEntry questEntry)
	{
		this.entry = questEntry;
		if (this.entry != null)
		{
			this.CurrentQuest = this.entry.Quest;
			return;
		}
		this.CurrentQuest = null;
	}

	// Token: 0x04005F4E RID: 24398
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_QuestEntry entry;

	// Token: 0x04005F4F RID: 24399
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_QuestRewardList rewardList;

	// Token: 0x04005F50 RID: 24400
	[PublicizedFrom(EAccessModifier.Private)]
	public QuestClass questClass;

	// Token: 0x04005F51 RID: 24401
	[PublicizedFrom(EAccessModifier.Private)]
	public Quest currentQuest;

	// Token: 0x04005F52 RID: 24402
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<string, int, int, int> daytimeFormatter = new CachedStringFormatter<string, int, int, int>((string _status, int _day, int _hour, int _min) => string.Format("{0} {1} {2}, {3:00}:{4:00}", new object[]
	{
		_status,
		Localization.Get("xuiDay", false, null),
		_day,
		_hour,
		_min
	}));

	// Token: 0x04005F53 RID: 24403
	[PublicizedFrom(EAccessModifier.Private)]
	public string rewardNumberFormat = "[DECEA3]{0}[-] {1}";

	// Token: 0x04005F54 RID: 24404
	[PublicizedFrom(EAccessModifier.Private)]
	public string rewardNumberBonusFormat = "[DECEA3]{0}[-] {1} ([DECEA3]{2}[-] {3})";

	// Token: 0x04005F55 RID: 24405
	[PublicizedFrom(EAccessModifier.Private)]
	public string rewardItemFormat = "[DECEA3]{0}[-] {1}";

	// Token: 0x04005F56 RID: 24406
	[PublicizedFrom(EAccessModifier.Private)]
	public string rewardItemBonusFormat = "[DECEA3]{0}[-] {1} ([DECEA3]{2}[-] {3})";
}
