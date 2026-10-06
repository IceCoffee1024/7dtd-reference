using System;
using UnityEngine.Scripting;

// Token: 0x02000FF5 RID: 4085
[Preserve]
public class XUiC_QuestObjectivesWindow : XUiController
{
	// Token: 0x17000F1A RID: 3866
	// (get) Token: 0x06008082 RID: 32898 RVA: 0x0030D469 File Offset: 0x0030B669
	// (set) Token: 0x06008083 RID: 32899 RVA: 0x0030D471 File Offset: 0x0030B671
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

	// Token: 0x06008084 RID: 32900 RVA: 0x0030D497 File Offset: 0x0030B697
	public override void Init()
	{
		base.Init();
		this.objectiveList = base.GetChildByType<XUiC_QuestObjectiveList>();
	}

	// Token: 0x06008085 RID: 32901 RVA: 0x0030D4AC File Offset: 0x0030B6AC
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

	// Token: 0x06008086 RID: 32902 RVA: 0x0030D8AC File Offset: 0x0030BAAC
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

	// Token: 0x06008087 RID: 32903 RVA: 0x0030D93E File Offset: 0x0030BB3E
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

	// Token: 0x04005F4F RID: 24399
	[PublicizedFrom(EAccessModifier.Private)]
	public string defaultReqColor = "DECEA3";

	// Token: 0x04005F50 RID: 24400
	[PublicizedFrom(EAccessModifier.Private)]
	public string missingReqColor = "FF0000";

	// Token: 0x04005F51 RID: 24401
	[PublicizedFrom(EAccessModifier.Private)]
	public string questlimitedColor = "255,0,0,255";

	// Token: 0x04005F52 RID: 24402
	[PublicizedFrom(EAccessModifier.Private)]
	public string questlimitedcompleteColor = "255,255,255,255";

	// Token: 0x04005F53 RID: 24403
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_QuestEntry entry;

	// Token: 0x04005F54 RID: 24404
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_QuestObjectiveList objectiveList;

	// Token: 0x04005F55 RID: 24405
	[PublicizedFrom(EAccessModifier.Private)]
	public QuestClass questClass;

	// Token: 0x04005F56 RID: 24406
	[PublicizedFrom(EAccessModifier.Private)]
	public Quest currentQuest;
}
