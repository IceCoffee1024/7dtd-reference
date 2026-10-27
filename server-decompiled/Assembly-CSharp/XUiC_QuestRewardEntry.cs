using System;
using UnityEngine.Scripting;

// Token: 0x02001008 RID: 4104
[Preserve]
public class XUiC_QuestRewardEntry : XUiController
{
	// Token: 0x17000F2B RID: 3883
	// (get) Token: 0x060080ED RID: 33005 RVA: 0x0030FD91 File Offset: 0x0030DF91
	// (set) Token: 0x060080EE RID: 33006 RVA: 0x0030FD99 File Offset: 0x0030DF99
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

	// Token: 0x17000F2C RID: 3884
	// (get) Token: 0x060080EF RID: 33007 RVA: 0x0030FDA9 File Offset: 0x0030DFA9
	// (set) Token: 0x060080F0 RID: 33008 RVA: 0x0030FDB1 File Offset: 0x0030DFB1
	public bool ChainQuest { get; set; }

	// Token: 0x17000F2D RID: 3885
	// (get) Token: 0x060080F1 RID: 33009 RVA: 0x0030FDBA File Offset: 0x0030DFBA
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

	// Token: 0x17000F2E RID: 3886
	// (get) Token: 0x060080F2 RID: 33010 RVA: 0x0030FDE3 File Offset: 0x0030DFE3
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

	// Token: 0x17000F2F RID: 3887
	// (get) Token: 0x060080F3 RID: 33011 RVA: 0x0030FE0C File Offset: 0x0030E00C
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

	// Token: 0x17000F30 RID: 3888
	// (get) Token: 0x060080F4 RID: 33012 RVA: 0x0030FE35 File Offset: 0x0030E035
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

	// Token: 0x060080F5 RID: 33013 RVA: 0x0030FE60 File Offset: 0x0030E060
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

	// Token: 0x060080F6 RID: 33014 RVA: 0x0008ABC9 File Offset: 0x00088DC9
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleOnCountChanged(XUiController _sender, OnCountChangedEventArgs _e)
	{
		this.IsDirty = true;
	}

	// Token: 0x060080F7 RID: 33015 RVA: 0x002C8B51 File Offset: 0x002C6D51
	public override void Update(float _dt)
	{
		base.RefreshBindings();
		this.IsDirty = false;
		base.Update(_dt);
	}

	// Token: 0x04005FA1 RID: 24481
	[PublicizedFrom(EAccessModifier.Private)]
	public string completeIconName = "";

	// Token: 0x04005FA2 RID: 24482
	[PublicizedFrom(EAccessModifier.Private)]
	public string incompleteIconName = "";

	// Token: 0x04005FA3 RID: 24483
	[PublicizedFrom(EAccessModifier.Private)]
	public string completeColor = "0,255,0,255";

	// Token: 0x04005FA4 RID: 24484
	[PublicizedFrom(EAccessModifier.Private)]
	public string incompleteColor = "255,0,0,255";

	// Token: 0x04005FA5 RID: 24485
	[PublicizedFrom(EAccessModifier.Private)]
	public BaseReward reward;

	// Token: 0x04005FA7 RID: 24487
	[PublicizedFrom(EAccessModifier.Private)]
	public static string chainQuestTypeKeyword = "";

	// Token: 0x04005FA8 RID: 24488
	[PublicizedFrom(EAccessModifier.Private)]
	public static string questTypeKeyword = "";

	// Token: 0x04005FA9 RID: 24489
	[PublicizedFrom(EAccessModifier.Private)]
	public static string optionalKeyword = "";

	// Token: 0x04005FAA RID: 24490
	[PublicizedFrom(EAccessModifier.Private)]
	public static string bonusKeyword = "";

	// Token: 0x04005FAB RID: 24491
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<string> rewardOptionalFormatter = new CachedStringFormatter<string>((string _s) => "(" + _s + ") ");

	// Token: 0x04005FAC RID: 24492
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<string, string> rewardTypeFormatter = new CachedStringFormatter<string, string>((string _s1, string _s2) => _s1 + " " + _s2);
}
