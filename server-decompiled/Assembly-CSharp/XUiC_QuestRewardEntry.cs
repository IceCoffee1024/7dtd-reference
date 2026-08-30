using System;
using UnityEngine.Scripting;

// Token: 0x02000FE9 RID: 4073
[Preserve]
public class XUiC_QuestRewardEntry : XUiController
{
	// Token: 0x17000EF9 RID: 3833
	// (get) Token: 0x0600808F RID: 32911 RVA: 0x0030B28D File Offset: 0x0030948D
	// (set) Token: 0x06008090 RID: 32912 RVA: 0x0030B295 File Offset: 0x00309495
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

	// Token: 0x17000EFA RID: 3834
	// (get) Token: 0x06008091 RID: 32913 RVA: 0x0030B2A5 File Offset: 0x003094A5
	// (set) Token: 0x06008092 RID: 32914 RVA: 0x0030B2AD File Offset: 0x003094AD
	public bool ChainQuest { get; set; }

	// Token: 0x17000EFB RID: 3835
	// (get) Token: 0x06008093 RID: 32915 RVA: 0x0030B2B6 File Offset: 0x003094B6
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

	// Token: 0x17000EFC RID: 3836
	// (get) Token: 0x06008094 RID: 32916 RVA: 0x0030B2DF File Offset: 0x003094DF
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

	// Token: 0x17000EFD RID: 3837
	// (get) Token: 0x06008095 RID: 32917 RVA: 0x0030B308 File Offset: 0x00309508
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

	// Token: 0x17000EFE RID: 3838
	// (get) Token: 0x06008096 RID: 32918 RVA: 0x0030B331 File Offset: 0x00309531
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

	// Token: 0x06008097 RID: 32919 RVA: 0x0030B35C File Offset: 0x0030955C
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

	// Token: 0x06008098 RID: 32920 RVA: 0x0008A715 File Offset: 0x00088915
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleOnCountChanged(XUiController _sender, OnCountChangedEventArgs _e)
	{
		this.IsDirty = true;
	}

	// Token: 0x06008099 RID: 32921 RVA: 0x002C422D File Offset: 0x002C242D
	public override void Update(float _dt)
	{
		base.RefreshBindings();
		this.IsDirty = false;
		base.Update(_dt);
	}

	// Token: 0x04005F3D RID: 24381
	[PublicizedFrom(EAccessModifier.Private)]
	public string completeIconName = "";

	// Token: 0x04005F3E RID: 24382
	[PublicizedFrom(EAccessModifier.Private)]
	public string incompleteIconName = "";

	// Token: 0x04005F3F RID: 24383
	[PublicizedFrom(EAccessModifier.Private)]
	public string completeColor = "0,255,0,255";

	// Token: 0x04005F40 RID: 24384
	[PublicizedFrom(EAccessModifier.Private)]
	public string incompleteColor = "255,0,0,255";

	// Token: 0x04005F41 RID: 24385
	[PublicizedFrom(EAccessModifier.Private)]
	public BaseReward reward;

	// Token: 0x04005F43 RID: 24387
	[PublicizedFrom(EAccessModifier.Private)]
	public static string chainQuestTypeKeyword = "";

	// Token: 0x04005F44 RID: 24388
	[PublicizedFrom(EAccessModifier.Private)]
	public static string questTypeKeyword = "";

	// Token: 0x04005F45 RID: 24389
	[PublicizedFrom(EAccessModifier.Private)]
	public static string optionalKeyword = "";

	// Token: 0x04005F46 RID: 24390
	[PublicizedFrom(EAccessModifier.Private)]
	public static string bonusKeyword = "";

	// Token: 0x04005F47 RID: 24391
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<string> rewardOptionalFormatter = new CachedStringFormatter<string>((string _s) => "(" + _s + ") ");

	// Token: 0x04005F48 RID: 24392
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<string, string> rewardTypeFormatter = new CachedStringFormatter<string, string>((string _s1, string _s2) => _s1 + " " + _s2);
}
