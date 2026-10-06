using System;
using UnityEngine.Scripting;

// Token: 0x02000FF8 RID: 4088
[Preserve]
public class XUiC_QuestRewardEntry : XUiController
{
	// Token: 0x17000F20 RID: 3872
	// (get) Token: 0x0600809E RID: 32926 RVA: 0x0030E275 File Offset: 0x0030C475
	// (set) Token: 0x0600809F RID: 32927 RVA: 0x0030E27D File Offset: 0x0030C47D
	public BaseReward Reward
	{
		get
		{
			return this.reward;
		}
		set
		{
			this.reward = value;
			this.IsDirty = true;
		}
	}

	// Token: 0x17000F21 RID: 3873
	// (get) Token: 0x060080A0 RID: 32928 RVA: 0x0030E28D File Offset: 0x0030C48D
	// (set) Token: 0x060080A1 RID: 32929 RVA: 0x0030E295 File Offset: 0x0030C495
	public bool ChainQuest { get; set; }

	// Token: 0x17000F22 RID: 3874
	// (get) Token: 0x060080A2 RID: 32930 RVA: 0x0030E29E File Offset: 0x0030C49E
	public static string ChainQuestTypeKeyword
	{
		get
		{
			if (XUiC_QuestRewardEntry.chainQuestTypeKeyword == "")
			{
				XUiC_QuestRewardEntry.chainQuestTypeKeyword = Localization.Get("RewardTypeChainQuest", false, null);
			}
			return XUiC_QuestRewardEntry.chainQuestTypeKeyword;
		}
	}

	// Token: 0x17000F23 RID: 3875
	// (get) Token: 0x060080A3 RID: 32931 RVA: 0x0030E2C7 File Offset: 0x0030C4C7
	public static string QuestTypeKeyword
	{
		get
		{
			if (XUiC_QuestRewardEntry.questTypeKeyword == "")
			{
				XUiC_QuestRewardEntry.questTypeKeyword = Localization.Get("RewardTypeQuest", false, null);
			}
			return XUiC_QuestRewardEntry.questTypeKeyword;
		}
	}

	// Token: 0x17000F24 RID: 3876
	// (get) Token: 0x060080A4 RID: 32932 RVA: 0x0030E2F0 File Offset: 0x0030C4F0
	public static string OptionalKeyword
	{
		get
		{
			if (XUiC_QuestRewardEntry.optionalKeyword == "")
			{
				XUiC_QuestRewardEntry.optionalKeyword = Localization.Get("optional", false, null);
			}
			return XUiC_QuestRewardEntry.optionalKeyword;
		}
	}

	// Token: 0x17000F25 RID: 3877
	// (get) Token: 0x060080A5 RID: 32933 RVA: 0x0030E319 File Offset: 0x0030C519
	public static string BonusKeyword
	{
		get
		{
			if (XUiC_QuestRewardEntry.bonusKeyword == "")
			{
				XUiC_QuestRewardEntry.bonusKeyword = Localization.Get("bonus", false, null);
			}
			return XUiC_QuestRewardEntry.bonusKeyword;
		}
	}

	// Token: 0x060080A6 RID: 32934 RVA: 0x0030E344 File Offset: 0x0030C544
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string value, string bindingName)
	{
		bool flag = this.reward != null;
		uint num = <PrivateImplementationDetails>.ComputeStringHash(bindingName);
		if (num <= 1771022199U)
		{
			if (num != 463969434U)
			{
				if (num != 1455818684U)
				{
					if (num == 1771022199U)
					{
						if (bindingName == "rewardicon")
						{
							value = (flag ? this.reward.Icon : "");
							return true;
						}
					}
				}
				else if (bindingName == "rewarddescription")
				{
					value = (flag ? this.reward.Description : "");
					return true;
				}
			}
			else if (bindingName == "rewardtype")
			{
				if (flag)
				{
					if (this.ChainQuest)
					{
						value = XUiC_QuestRewardEntry.ChainQuestTypeKeyword;
					}
					else
					{
						string text = (this.reward.Optional ? XUiC_QuestRewardEntry.BonusKeyword : Localization.Get(QuestClass.GetQuest(this.Reward.OwnerQuest.ID).Category, false, null));
						value = this.rewardTypeFormatter.Format(text, XUiC_QuestRewardEntry.QuestTypeKeyword);
					}
				}
				else
				{
					value = "";
				}
				return true;
			}
		}
		else if (num <= 2048631988U)
		{
			if (num != 1864291747U)
			{
				if (num == 2048631988U)
				{
					if (bindingName == "hasreward")
					{
						value = flag.ToString();
						return true;
					}
				}
			}
			else if (bindingName == "rewardvalue")
			{
				value = (flag ? this.reward.ValueText : "");
				return true;
			}
		}
		else if (num != 3261050802U)
		{
			if (num == 3763801510U)
			{
				if (bindingName == "rewardiconatlas")
				{
					value = (flag ? this.reward.IconAtlas : "");
					return true;
				}
			}
		}
		else if (bindingName == "rewardoptional")
		{
			value = (flag ? (this.reward.Optional ? this.rewardOptionalFormatter.Format(XUiC_QuestRewardEntry.OptionalKeyword) : "") : "");
			return true;
		}
		return base.GetBindingValueInternal(ref value, bindingName);
	}

	// Token: 0x060080A7 RID: 32935 RVA: 0x0008A995 File Offset: 0x00088B95
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleOnCountChanged(XUiController _sender, OnCountChangedEventArgs _e)
	{
		this.IsDirty = true;
	}

	// Token: 0x060080A8 RID: 32936 RVA: 0x002C75E5 File Offset: 0x002C57E5
	public override void Update(float _dt)
	{
		base.RefreshBindings();
		this.IsDirty = false;
		base.Update(_dt);
	}

	// Token: 0x04005F66 RID: 24422
	[PublicizedFrom(EAccessModifier.Private)]
	public string completeIconName = "";

	// Token: 0x04005F67 RID: 24423
	[PublicizedFrom(EAccessModifier.Private)]
	public string incompleteIconName = "";

	// Token: 0x04005F68 RID: 24424
	[PublicizedFrom(EAccessModifier.Private)]
	public string completeColor = "0,255,0,255";

	// Token: 0x04005F69 RID: 24425
	[PublicizedFrom(EAccessModifier.Private)]
	public string incompleteColor = "255,0,0,255";

	// Token: 0x04005F6A RID: 24426
	[PublicizedFrom(EAccessModifier.Private)]
	public BaseReward reward;

	// Token: 0x04005F6C RID: 24428
	[PublicizedFrom(EAccessModifier.Private)]
	public static string chainQuestTypeKeyword = "";

	// Token: 0x04005F6D RID: 24429
	[PublicizedFrom(EAccessModifier.Private)]
	public static string questTypeKeyword = "";

	// Token: 0x04005F6E RID: 24430
	[PublicizedFrom(EAccessModifier.Private)]
	public static string optionalKeyword = "";

	// Token: 0x04005F6F RID: 24431
	[PublicizedFrom(EAccessModifier.Private)]
	public static string bonusKeyword = "";

	// Token: 0x04005F70 RID: 24432
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<string> rewardOptionalFormatter = new CachedStringFormatter<string>((string _s) => "(" + _s + ") ");

	// Token: 0x04005F71 RID: 24433
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<string, string> rewardTypeFormatter = new CachedStringFormatter<string, string>((string _s1, string _s2) => _s1 + " " + _s2);
}
