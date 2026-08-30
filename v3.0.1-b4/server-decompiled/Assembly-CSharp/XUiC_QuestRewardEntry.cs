using System;
using UnityEngine.Scripting;

// Token: 0x02000FD0 RID: 4048
[Preserve]
public class XUiC_QuestRewardEntry : XUiController
{
	// Token: 0x17000EC0 RID: 3776
	// (get) Token: 0x06007F9D RID: 32669 RVA: 0x00306A59 File Offset: 0x00304C59
	// (set) Token: 0x06007F9E RID: 32670 RVA: 0x00306A61 File Offset: 0x00304C61
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

	// Token: 0x17000EC1 RID: 3777
	// (get) Token: 0x06007F9F RID: 32671 RVA: 0x00306A71 File Offset: 0x00304C71
	// (set) Token: 0x06007FA0 RID: 32672 RVA: 0x00306A79 File Offset: 0x00304C79
	public bool ChainQuest { get; set; }

	// Token: 0x17000EC2 RID: 3778
	// (get) Token: 0x06007FA1 RID: 32673 RVA: 0x00306A82 File Offset: 0x00304C82
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

	// Token: 0x17000EC3 RID: 3779
	// (get) Token: 0x06007FA2 RID: 32674 RVA: 0x00306AAB File Offset: 0x00304CAB
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

	// Token: 0x17000EC4 RID: 3780
	// (get) Token: 0x06007FA3 RID: 32675 RVA: 0x00306AD4 File Offset: 0x00304CD4
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

	// Token: 0x17000EC5 RID: 3781
	// (get) Token: 0x06007FA4 RID: 32676 RVA: 0x00306AFD File Offset: 0x00304CFD
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

	// Token: 0x06007FA5 RID: 32677 RVA: 0x00306B28 File Offset: 0x00304D28
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

	// Token: 0x06007FA6 RID: 32678 RVA: 0x0008A069 File Offset: 0x00088269
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleOnCountChanged(XUiController _sender, OnCountChangedEventArgs _e)
	{
		this.IsDirty = true;
	}

	// Token: 0x06007FA7 RID: 32679 RVA: 0x002C0D09 File Offset: 0x002BEF09
	public override void Update(float _dt)
	{
		base.RefreshBindings();
		this.IsDirty = false;
		base.Update(_dt);
	}

	// Token: 0x04005E76 RID: 24182
	[PublicizedFrom(EAccessModifier.Private)]
	public string completeIconName = "";

	// Token: 0x04005E77 RID: 24183
	[PublicizedFrom(EAccessModifier.Private)]
	public string incompleteIconName = "";

	// Token: 0x04005E78 RID: 24184
	[PublicizedFrom(EAccessModifier.Private)]
	public string completeColor = "0,255,0,255";

	// Token: 0x04005E79 RID: 24185
	[PublicizedFrom(EAccessModifier.Private)]
	public string incompleteColor = "255,0,0,255";

	// Token: 0x04005E7A RID: 24186
	[PublicizedFrom(EAccessModifier.Private)]
	public BaseReward reward;

	// Token: 0x04005E7C RID: 24188
	[PublicizedFrom(EAccessModifier.Private)]
	public static string chainQuestTypeKeyword = "";

	// Token: 0x04005E7D RID: 24189
	[PublicizedFrom(EAccessModifier.Private)]
	public static string questTypeKeyword = "";

	// Token: 0x04005E7E RID: 24190
	[PublicizedFrom(EAccessModifier.Private)]
	public static string optionalKeyword = "";

	// Token: 0x04005E7F RID: 24191
	[PublicizedFrom(EAccessModifier.Private)]
	public static string bonusKeyword = "";

	// Token: 0x04005E80 RID: 24192
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<string> rewardOptionalFormatter = new CachedStringFormatter<string>((string _s) => "(" + _s + ") ");

	// Token: 0x04005E81 RID: 24193
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<string, string> rewardTypeFormatter = new CachedStringFormatter<string, string>((string _s1, string _s2) => _s1 + " " + _s2);
}
