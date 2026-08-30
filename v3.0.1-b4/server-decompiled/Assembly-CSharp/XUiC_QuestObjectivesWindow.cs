using System;
using UnityEngine.Scripting;

// Token: 0x02000FCD RID: 4045
[Preserve]
public class XUiC_QuestObjectivesWindow : XUiController
{
	// Token: 0x17000EBA RID: 3770
	// (get) Token: 0x06007F81 RID: 32641 RVA: 0x00305CBD File Offset: 0x00303EBD
	// (set) Token: 0x06007F82 RID: 32642 RVA: 0x00305CC5 File Offset: 0x00303EC5
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

	// Token: 0x06007F83 RID: 32643 RVA: 0x00305CEB File Offset: 0x00303EEB
	public override void Init()
	{
		base.Init();
		this.objectiveList = base.GetChildByType<XUiC_QuestObjectiveList>();
	}

	// Token: 0x06007F84 RID: 32644 RVA: 0x00305D00 File Offset: 0x00303F00
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

	// Token: 0x06007F85 RID: 32645 RVA: 0x00306100 File Offset: 0x00304300
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

	// Token: 0x06007F86 RID: 32646 RVA: 0x00306192 File Offset: 0x00304392
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

	// Token: 0x04005E5F RID: 24159
	[PublicizedFrom(EAccessModifier.Private)]
	public string defaultReqColor = "DECEA3";

	// Token: 0x04005E60 RID: 24160
	[PublicizedFrom(EAccessModifier.Private)]
	public string missingReqColor = "FF0000";

	// Token: 0x04005E61 RID: 24161
	[PublicizedFrom(EAccessModifier.Private)]
	public string questlimitedColor = "255,0,0,255";

	// Token: 0x04005E62 RID: 24162
	[PublicizedFrom(EAccessModifier.Private)]
	public string questlimitedcompleteColor = "255,255,255,255";

	// Token: 0x04005E63 RID: 24163
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_QuestEntry entry;

	// Token: 0x04005E64 RID: 24164
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_QuestObjectiveList objectiveList;

	// Token: 0x04005E65 RID: 24165
	[PublicizedFrom(EAccessModifier.Private)]
	public QuestClass questClass;

	// Token: 0x04005E66 RID: 24166
	[PublicizedFrom(EAccessModifier.Private)]
	public Quest currentQuest;
}
