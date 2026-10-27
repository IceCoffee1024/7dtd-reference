using System;
using UnityEngine.Scripting;

// Token: 0x02001005 RID: 4101
[Preserve]
public class XUiC_QuestObjectivesWindow : XUiController
{
	// Token: 0x17000F25 RID: 3877
	// (get) Token: 0x060080D1 RID: 32977 RVA: 0x0030EF85 File Offset: 0x0030D185
	// (set) Token: 0x060080D2 RID: 32978 RVA: 0x0030EF8D File Offset: 0x0030D18D
	public Quest CurrentQuest
	{
		get
		{
			return this.currentQuest;
		}
		set
		{
			this.currentQuest = value;
			this.questClass = ((value != null) ? QuestClass.GetQuest(value.ID) : null);
			base.RefreshBindings();
		}
	}

	// Token: 0x060080D3 RID: 32979 RVA: 0x0030EFB3 File Offset: 0x0030D1B3
	public override void Init()
	{
		base.Init();
		this.objectiveList = base.GetChildByType<XUiC_QuestObjectiveList>();
	}

	// Token: 0x060080D4 RID: 32980 RVA: 0x0030EFC8 File Offset: 0x0030D1C8
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string value, string bindingName)
	{
		uint num = <PrivateImplementationDetails>.ComputeStringHash(bindingName);
		if (num <= 2375080848U)
		{
			if (num <= 196213477U)
			{
				if (num != 72500844U)
				{
					if (num == 196213477U)
					{
						if (bindingName == "questrequirementstitle")
						{
							value = Localization.Get("xuiRequirements", false, null);
							return true;
						}
					}
				}
				else if (bindingName == "showrequirements")
				{
					value = ((this.currentQuest != null) ? (this.currentQuest.Requirements.Count > 0).ToString() : "false");
					return true;
				}
			}
			else if (num != 2061479048U)
			{
				if (num != 2369797097U)
				{
					if (num == 2375080848U)
					{
						if (bindingName == "tieradd")
						{
							if (this.currentQuest != null && this.questClass.AddsToTierComplete && (this.currentQuest.AddsProgression || (this.currentQuest.OwnerJournal.CanAddProgression && this.currentQuest.QuestProgressDay == -2147483648)))
							{
								if (this.questClass.DifficultyTier != 0)
								{
									string text = ((this.questClass.DifficultyTier > 0) ? "+" : "-") + this.questClass.DifficultyTier.ToString();
									value = string.Format(Localization.Get("xuiQuestTierAdd", false, null), text);
								}
								else
								{
									value = "";
								}
							}
							else
							{
								value = "";
							}
							return true;
						}
					}
				}
				else if (bindingName == "questrequirements")
				{
					value = ((this.currentQuest != null) ? this.currentQuest.RequirementsString.Replace("DEFAULT_COLOR", this.defaultReqColor).Replace("MISSING_COLOR", this.missingReqColor) : "");
					return true;
				}
			}
			else if (bindingName == "questdifficulty")
			{
				value = ((this.currentQuest != null) ? this.questClass.Difficulty : "");
				return true;
			}
		}
		else if (num <= 3229612626U)
		{
			if (num != 2730462270U)
			{
				if (num != 3047389681U)
				{
					if (num == 3229612626U)
					{
						if (bindingName == "tieraddlimited")
						{
							if (this.currentQuest != null && this.questClass.AddsToTierComplete && ((!this.currentQuest.OwnerJournal.CanAddProgression && this.currentQuest.QuestProgressDay == -2147483648) || this.currentQuest.QuestProgressDay == -1))
							{
								value = "true";
							}
							else
							{
								value = "false";
							}
							return true;
						}
					}
				}
				else if (bindingName == "questtitle")
				{
					value = ((this.currentQuest != null) ? this.questClass.SubTitle : "");
					return true;
				}
			}
			else if (bindingName == "questname")
			{
				value = ((this.currentQuest != null) ? this.questClass.Name : "");
				return true;
			}
		}
		else if (num != 3231221182U)
		{
			if (num != 3607901221U)
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
			else if (bindingName == "tieraddlimitedcolor")
			{
				if (this.currentQuest != null && this.questClass.AddsToTierComplete)
				{
					if (this.currentQuest.QuestProgressDay == -2147483648 && !this.currentQuest.OwnerJournal.CanAddProgression)
					{
						value = this.questlimitedColor;
					}
					else
					{
						value = this.questlimitedcompleteColor;
					}
				}
				else
				{
					value = "255,255,255,255";
				}
				return true;
			}
		}
		else if (bindingName == "showquest")
		{
			value = (this.currentQuest != null).ToString();
			return true;
		}
		return base.GetBindingValueInternal(ref value, bindingName);
	}

	// Token: 0x060080D5 RID: 32981 RVA: 0x0030F3C8 File Offset: 0x0030D5C8
	public override bool ParseAttribute(string name, string value)
	{
		bool flag = base.ParseAttribute(name, value);
		if (!flag)
		{
			if (name == "default_req_color")
			{
				this.defaultReqColor = Utils.ColorToHex(StringParsers.ParseColor32(value));
				return true;
			}
			if (name == "missing_req_color")
			{
				this.missingReqColor = Utils.ColorToHex(StringParsers.ParseColor32(value));
				return true;
			}
			if (name == "quest_limited_color")
			{
				this.questlimitedColor = value;
				return true;
			}
			if (name == "quest_limited_complete_color")
			{
				this.questlimitedcompleteColor = value;
				return true;
			}
		}
		return flag;
	}

	// Token: 0x060080D6 RID: 32982 RVA: 0x0030F45A File Offset: 0x0030D65A
	public void SetQuest(XUiC_QuestEntry questEntry)
	{
		this.entry = questEntry;
		if (this.entry != null)
		{
			this.CurrentQuest = this.entry.Quest;
			this.objectiveList.Quest = this.entry.Quest;
			return;
		}
		this.CurrentQuest = null;
	}

	// Token: 0x04005F8A RID: 24458
	[PublicizedFrom(EAccessModifier.Private)]
	public string defaultReqColor = "DECEA3";

	// Token: 0x04005F8B RID: 24459
	[PublicizedFrom(EAccessModifier.Private)]
	public string missingReqColor = "FF0000";

	// Token: 0x04005F8C RID: 24460
	[PublicizedFrom(EAccessModifier.Private)]
	public string questlimitedColor = "255,0,0,255";

	// Token: 0x04005F8D RID: 24461
	[PublicizedFrom(EAccessModifier.Private)]
	public string questlimitedcompleteColor = "255,255,255,255";

	// Token: 0x04005F8E RID: 24462
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_QuestEntry entry;

	// Token: 0x04005F8F RID: 24463
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_QuestObjectiveList objectiveList;

	// Token: 0x04005F90 RID: 24464
	[PublicizedFrom(EAccessModifier.Private)]
	public QuestClass questClass;

	// Token: 0x04005F91 RID: 24465
	[PublicizedFrom(EAccessModifier.Private)]
	public Quest currentQuest;
}
