using System;
using System.Collections.Generic;
using Platform;
using Quests.Requirements;
using SandboxOptions;

// Token: 0x020009C9 RID: 2505
public class QuestClass
{
	// Token: 0x17000808 RID: 2056
	// (get) Token: 0x060049C2 RID: 18882 RVA: 0x001D04A3 File Offset: 0x001CE6A3
	// (set) Token: 0x060049C3 RID: 18883 RVA: 0x001D04AB File Offset: 0x001CE6AB
	public string ID
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000809 RID: 2057
	// (get) Token: 0x060049C4 RID: 18884 RVA: 0x001D04B4 File Offset: 0x001CE6B4
	// (set) Token: 0x060049C5 RID: 18885 RVA: 0x001D04BC File Offset: 0x001CE6BC
	public string Name { get; set; }

	// Token: 0x1700080A RID: 2058
	// (get) Token: 0x060049C6 RID: 18886 RVA: 0x001D04C5 File Offset: 0x001CE6C5
	// (set) Token: 0x060049C7 RID: 18887 RVA: 0x001D04CD File Offset: 0x001CE6CD
	public string GroupName { get; set; }

	// Token: 0x1700080B RID: 2059
	// (get) Token: 0x060049C8 RID: 18888 RVA: 0x001D04D6 File Offset: 0x001CE6D6
	// (set) Token: 0x060049C9 RID: 18889 RVA: 0x001D04DE File Offset: 0x001CE6DE
	public string SubTitle { get; set; }

	// Token: 0x1700080C RID: 2060
	// (get) Token: 0x060049CA RID: 18890 RVA: 0x001D04E7 File Offset: 0x001CE6E7
	// (set) Token: 0x060049CB RID: 18891 RVA: 0x001D04EF File Offset: 0x001CE6EF
	public string Description { get; set; }

	// Token: 0x1700080D RID: 2061
	// (get) Token: 0x060049CC RID: 18892 RVA: 0x001D04F8 File Offset: 0x001CE6F8
	// (set) Token: 0x060049CD RID: 18893 RVA: 0x001D0500 File Offset: 0x001CE700
	public string Offer { get; set; }

	// Token: 0x1700080E RID: 2062
	// (get) Token: 0x060049CE RID: 18894 RVA: 0x001D0509 File Offset: 0x001CE709
	// (set) Token: 0x060049CF RID: 18895 RVA: 0x001D0511 File Offset: 0x001CE711
	public string Difficulty { get; set; }

	// Token: 0x1700080F RID: 2063
	// (get) Token: 0x060049D0 RID: 18896 RVA: 0x001D051A File Offset: 0x001CE71A
	// (set) Token: 0x060049D1 RID: 18897 RVA: 0x001D0522 File Offset: 0x001CE722
	public string Icon { get; set; }

	// Token: 0x17000810 RID: 2064
	// (get) Token: 0x060049D2 RID: 18898 RVA: 0x001D052B File Offset: 0x001CE72B
	// (set) Token: 0x060049D3 RID: 18899 RVA: 0x001D0533 File Offset: 0x001CE733
	public bool Repeatable { get; set; }

	// Token: 0x17000811 RID: 2065
	// (get) Token: 0x060049D4 RID: 18900 RVA: 0x001D053C File Offset: 0x001CE73C
	// (set) Token: 0x060049D5 RID: 18901 RVA: 0x001D0544 File Offset: 0x001CE744
	public bool Shareable { get; set; }

	// Token: 0x17000812 RID: 2066
	// (get) Token: 0x060049D6 RID: 18902 RVA: 0x001D054D File Offset: 0x001CE74D
	// (set) Token: 0x060049D7 RID: 18903 RVA: 0x001D0555 File Offset: 0x001CE755
	public string Category { get; set; }

	// Token: 0x17000813 RID: 2067
	// (get) Token: 0x060049D8 RID: 18904 RVA: 0x001D055E File Offset: 0x001CE75E
	// (set) Token: 0x060049D9 RID: 18905 RVA: 0x001D0566 File Offset: 0x001CE766
	public string StatementText { get; set; }

	// Token: 0x17000814 RID: 2068
	// (get) Token: 0x060049DA RID: 18906 RVA: 0x001D056F File Offset: 0x001CE76F
	// (set) Token: 0x060049DB RID: 18907 RVA: 0x001D0577 File Offset: 0x001CE777
	public string ResponseText { get; set; }

	// Token: 0x17000815 RID: 2069
	// (get) Token: 0x060049DC RID: 18908 RVA: 0x001D0580 File Offset: 0x001CE780
	// (set) Token: 0x060049DD RID: 18909 RVA: 0x001D0588 File Offset: 0x001CE788
	public string CompleteText { get; set; }

	// Token: 0x17000816 RID: 2070
	// (get) Token: 0x060049DE RID: 18910 RVA: 0x001D0591 File Offset: 0x001CE791
	// (set) Token: 0x060049DF RID: 18911 RVA: 0x001D0599 File Offset: 0x001CE799
	public byte CurrentVersion { get; set; }

	// Token: 0x17000817 RID: 2071
	// (get) Token: 0x060049E0 RID: 18912 RVA: 0x001D05A2 File Offset: 0x001CE7A2
	// (set) Token: 0x060049E1 RID: 18913 RVA: 0x001D05AA File Offset: 0x001CE7AA
	public byte HighestPhase { get; set; }

	// Token: 0x17000818 RID: 2072
	// (get) Token: 0x060049E2 RID: 18914 RVA: 0x001D05B3 File Offset: 0x001CE7B3
	// (set) Token: 0x060049E3 RID: 18915 RVA: 0x001D05BB File Offset: 0x001CE7BB
	public byte QuestFaction { get; set; }

	// Token: 0x17000819 RID: 2073
	// (get) Token: 0x060049E4 RID: 18916 RVA: 0x001D05C4 File Offset: 0x001CE7C4
	// (set) Token: 0x060049E5 RID: 18917 RVA: 0x001D05CC File Offset: 0x001CE7CC
	public byte DifficultyTier { get; set; }

	// Token: 0x1700081A RID: 2074
	// (get) Token: 0x060049E6 RID: 18918 RVA: 0x001D05D5 File Offset: 0x001CE7D5
	// (set) Token: 0x060049E7 RID: 18919 RVA: 0x001D05DD File Offset: 0x001CE7DD
	public bool LoginRallyReset { get; set; }

	// Token: 0x1700081B RID: 2075
	// (get) Token: 0x060049E8 RID: 18920 RVA: 0x001D05E6 File Offset: 0x001CE7E6
	// (set) Token: 0x060049E9 RID: 18921 RVA: 0x001D05EE File Offset: 0x001CE7EE
	public bool ReturnToQuestGiver { get; set; }

	// Token: 0x1700081C RID: 2076
	// (get) Token: 0x060049EA RID: 18922 RVA: 0x001D05F7 File Offset: 0x001CE7F7
	// (set) Token: 0x060049EB RID: 18923 RVA: 0x001D05FF File Offset: 0x001CE7FF
	public string UniqueKey { get; set; }

	// Token: 0x1700081D RID: 2077
	// (get) Token: 0x060049EC RID: 18924 RVA: 0x001D0608 File Offset: 0x001CE808
	// (set) Token: 0x060049ED RID: 18925 RVA: 0x001D0610 File Offset: 0x001CE810
	public string QuestType { get; set; }

	// Token: 0x1700081E RID: 2078
	// (get) Token: 0x060049EE RID: 18926 RVA: 0x001D0619 File Offset: 0x001CE819
	// (set) Token: 0x060049EF RID: 18927 RVA: 0x001D0621 File Offset: 0x001CE821
	public bool AddsToTierComplete { get; set; }

	// Token: 0x060049F0 RID: 18928 RVA: 0x001D062C File Offset: 0x001CE82C
	public static QuestClass NewClass(string id)
	{
		if (QuestClass.s_Quests.ContainsKey(id))
		{
			return null;
		}
		QuestClass questClass = new QuestClass(id.ToLower());
		QuestClass.s_Quests[id] = questClass;
		return questClass;
	}

	// Token: 0x060049F1 RID: 18929 RVA: 0x001D0664 File Offset: 0x001CE864
	[PublicizedFrom(EAccessModifier.Private)]
	public QuestClass(string id)
	{
		this.ID = id;
		this.Difficulty = "veryeasy";
		this.HighestPhase = 1;
		this.AddsToTierComplete = true;
	}

	// Token: 0x060049F2 RID: 18930 RVA: 0x001D0729 File Offset: 0x001CE929
	[PublicizedFrom(EAccessModifier.Internal)]
	public static QuestClass GetQuest(string questID)
	{
		if (!QuestClass.s_Quests.ContainsKey(questID))
		{
			return null;
		}
		return QuestClass.s_Quests[questID];
	}

	// Token: 0x060049F3 RID: 18931 RVA: 0x001D0748 File Offset: 0x001CE948
	[PublicizedFrom(EAccessModifier.Internal)]
	public DynamicProperties AssignValuesFrom(QuestClass oldQuest)
	{
		DynamicProperties dynamicProperties = new DynamicProperties();
		HashSet<string> hashSet = new HashSet<string>();
		if (oldQuest.Properties != null)
		{
			dynamicProperties.CopyFrom(oldQuest.Properties, hashSet);
		}
		for (int i = 0; i < oldQuest.Requirements.Count; i++)
		{
			BaseRequirement baseRequirement = oldQuest.Requirements[i].Clone();
			baseRequirement.Properties = new DynamicProperties();
			if (oldQuest.Requirements[i].Properties != null)
			{
				baseRequirement.Properties.CopyFrom(oldQuest.Requirements[i].Properties, null);
			}
			baseRequirement.Owner = this;
			this.Requirements.Add(baseRequirement);
		}
		for (int j = 0; j < oldQuest.Actions.Count; j++)
		{
			BaseQuestAction baseQuestAction = oldQuest.Actions[j].Clone();
			baseQuestAction.Properties = new DynamicProperties();
			if (oldQuest.Actions[j].Properties != null)
			{
				baseQuestAction.Properties.CopyFrom(oldQuest.Actions[j].Properties, null);
			}
			baseQuestAction.Owner = this;
			this.Actions.Add(baseQuestAction);
		}
		for (int k = 0; k < oldQuest.Objectives.Count; k++)
		{
			BaseObjective baseObjective = oldQuest.Objectives[k].Clone();
			baseObjective.Properties = new DynamicProperties();
			if (oldQuest.Objectives[k].Properties != null)
			{
				baseObjective.Properties.CopyFrom(oldQuest.Objectives[k].Properties, null);
			}
			if (oldQuest.Objectives[k].Phase > this.HighestPhase)
			{
				this.HighestPhase = oldQuest.Objectives[k].Phase;
			}
			baseObjective.OwnerQuestClass = this;
			this.Objectives.Add(baseObjective);
		}
		for (int l = 0; l < oldQuest.Events.Count; l++)
		{
			QuestEvent questEvent = oldQuest.Events[l].Clone();
			questEvent.Properties = new DynamicProperties();
			if (oldQuest.Events[l].Properties != null)
			{
				questEvent.Properties.CopyFrom(oldQuest.Events[l].Properties, null);
			}
			questEvent.Owner = this;
			this.Events.Add(questEvent);
		}
		return dynamicProperties;
	}

	// Token: 0x060049F4 RID: 18932 RVA: 0x001D09AC File Offset: 0x001CEBAC
	public static Quest CreateQuest(string ID)
	{
		QuestClass quest = QuestClass.GetQuest(ID);
		if (quest != null)
		{
			return quest.CreateQuest();
		}
		return null;
	}

	// Token: 0x060049F5 RID: 18933 RVA: 0x001D09CC File Offset: 0x001CEBCC
	public Quest CreateQuest()
	{
		Quest quest = new Quest(this.ID);
		quest.CurrentQuestVersion = this.CurrentVersion;
		quest.Tracked = false;
		quest.FinishTime = 0UL;
		quest.QuestFaction = this.QuestFaction;
		if (!this.ExtraTags.IsEmpty)
		{
			quest.QuestTags |= this.ExtraTags;
		}
		for (int i = 0; i < this.Actions.Count; i++)
		{
			BaseQuestAction baseQuestAction = this.Actions[i].Clone();
			baseQuestAction.OwnerQuest = quest;
			quest.Actions.Add(baseQuestAction);
		}
		for (int j = 0; j < this.Requirements.Count; j++)
		{
			BaseRequirement baseRequirement = this.Requirements[j].Clone();
			baseRequirement.OwnerQuest = quest;
			quest.Requirements.Add(baseRequirement);
		}
		for (int k = 0; k < this.Objectives.Count; k++)
		{
			BaseObjective baseObjective = this.Objectives[k].Clone();
			baseObjective.OwnerQuest = quest;
			quest.Objectives.Add(baseObjective);
		}
		int num = 0;
		for (int l = 0; l < this.Rewards.Count; l++)
		{
			BaseReward baseReward = this.Rewards[l].Clone();
			baseReward.OwnerQuest = quest;
			quest.Rewards.Add(baseReward);
			if (!baseReward.isChainReward && baseReward.isChosenReward && !baseReward.isFixedLocation)
			{
				num++;
			}
		}
		return quest;
	}

	// Token: 0x060049F6 RID: 18934 RVA: 0x001D0B58 File Offset: 0x001CED58
	public void ResetObjectives()
	{
		for (int i = 0; i < this.Objectives.Count; i++)
		{
			this.Objectives[i].ResetObjective();
		}
	}

	// Token: 0x060049F7 RID: 18935 RVA: 0x001D0B8C File Offset: 0x001CED8C
	[PublicizedFrom(EAccessModifier.Internal)]
	public BaseQuestAction AddAction(BaseQuestAction action)
	{
		if (action != null)
		{
			this.Actions.Add(action);
		}
		return action;
	}

	// Token: 0x060049F8 RID: 18936 RVA: 0x001D0B9E File Offset: 0x001CED9E
	public QuestEvent AddEvent(QuestEvent questEvent)
	{
		if (questEvent != null)
		{
			this.Events.Add(questEvent);
		}
		return questEvent;
	}

	// Token: 0x060049F9 RID: 18937 RVA: 0x001D0BB0 File Offset: 0x001CEDB0
	[PublicizedFrom(EAccessModifier.Internal)]
	public BaseRequirement AddRequirement(BaseRequirement requirement)
	{
		if (requirement != null)
		{
			this.Requirements.Add(requirement);
		}
		return requirement;
	}

	// Token: 0x060049FA RID: 18938 RVA: 0x001D0BC2 File Offset: 0x001CEDC2
	[PublicizedFrom(EAccessModifier.Internal)]
	public BaseObjective AddObjective(BaseObjective objective)
	{
		if (objective != null)
		{
			objective.OwnerQuestClass = this;
			this.Objectives.Add(objective);
		}
		return objective;
	}

	// Token: 0x060049FB RID: 18939 RVA: 0x001D0BDB File Offset: 0x001CEDDB
	[PublicizedFrom(EAccessModifier.Internal)]
	public BaseReward AddReward(BaseReward reward)
	{
		if (reward != null)
		{
			this.Rewards.Add(reward);
		}
		return reward;
	}

	// Token: 0x060049FC RID: 18940 RVA: 0x001D0BED File Offset: 0x001CEDED
	[PublicizedFrom(EAccessModifier.Internal)]
	public BaseQuestCriteria AddCriteria(BaseQuestCriteria criteria)
	{
		if (criteria != null)
		{
			this.Criteria.Add(criteria);
		}
		return criteria;
	}

	// Token: 0x060049FD RID: 18941 RVA: 0x001D0C00 File Offset: 0x001CEE00
	public bool CheckCriteriaQuestGiver(EntityNPC entityNPC)
	{
		for (int i = 0; i < this.Criteria.Count; i++)
		{
			if (this.Criteria[i].CriteriaType == BaseQuestCriteria.CriteriaTypes.QuestGiver && !this.Criteria[i].CheckForQuestGiver(entityNPC))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x060049FE RID: 18942 RVA: 0x001D0C50 File Offset: 0x001CEE50
	public bool CheckCriteriaOffer(EntityPlayer player)
	{
		for (int i = 0; i < this.Criteria.Count; i++)
		{
			if (this.Criteria[i].CriteriaType == BaseQuestCriteria.CriteriaTypes.QuestGiver && !this.Criteria[i].CheckForPlayer(player))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x060049FF RID: 18943 RVA: 0x001D0CA0 File Offset: 0x001CEEA0
	public bool CanActivate()
	{
		if (EntityFactory.EnemySpawnMode)
		{
			return true;
		}
		for (int i = 0; i < this.Objectives.Count; i++)
		{
			if (this.Objectives[i].RequiresZombies)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06004A00 RID: 18944 RVA: 0x001D0CE4 File Offset: 0x001CEEE4
	public string GetCurrentHint(int phase)
	{
		if (this.QuestHintRequirement != global::SandboxOptions.SandboxOptions.Max && !SandboxOptionManager.GetBool(this.QuestHintRequirement))
		{
			return "";
		}
		phase--;
		if (this.QuestHints == null || phase >= this.QuestHints.Count)
		{
			return "";
		}
		string text = this.QuestHints[phase];
		if (PlatformManager.NativePlatform.Input.CurrentInputStyle != PlayerInputManager.InputStyle.Keyboard)
		{
			string text2 = text + "_alt";
			if (Localization.Exists(text2, false))
			{
				return Localization.Get(text2, false, null);
			}
		}
		return Localization.Get(text, false, null);
	}

	// Token: 0x06004A01 RID: 18945 RVA: 0x001D0D78 File Offset: 0x001CEF78
	[PublicizedFrom(EAccessModifier.Internal)]
	public void Init()
	{
		if (this.Properties.Values.ContainsKey(QuestClass.PropCategory))
		{
			this.Category = this.Properties.Values[QuestClass.PropCategory];
		}
		if (this.Properties.Values.ContainsKey(QuestClass.PropCategoryKey))
		{
			this.Category = Localization.Get(this.Properties.Values[QuestClass.PropCategoryKey], false, null);
		}
		if (this.Properties.Values.ContainsKey(QuestClass.PropGroupName))
		{
			this.GroupName = this.Properties.Values[QuestClass.PropGroupName];
		}
		if (this.Properties.Values.ContainsKey(QuestClass.PropGroupNameKey))
		{
			this.GroupName = Localization.Get(this.Properties.Values[QuestClass.PropGroupNameKey], false, null);
		}
		if (this.Properties.Values.ContainsKey(QuestClass.PropName))
		{
			this.Name = this.Properties.Values[QuestClass.PropName];
		}
		if (this.Properties.Values.ContainsKey(QuestClass.PropNameKey))
		{
			this.Name = Localization.Get(this.Properties.Values[QuestClass.PropNameKey], false, null);
		}
		if (this.Properties.Values.ContainsKey(QuestClass.PropSubtitle))
		{
			this.SubTitle = this.Properties.Values[QuestClass.PropSubtitle];
		}
		if (this.Properties.Values.ContainsKey(QuestClass.PropSubtitleKey))
		{
			this.SubTitle = Localization.Get(this.Properties.Values[QuestClass.PropSubtitleKey], false, null);
		}
		if (this.Properties.Values.ContainsKey(QuestClass.PropDescription))
		{
			this.Description = this.Properties.Values[QuestClass.PropDescription];
		}
		if (this.Properties.Values.ContainsKey(QuestClass.PropDescriptionKey))
		{
			this.Description = Localization.Get(this.Properties.Values[QuestClass.PropDescriptionKey], false, null);
		}
		if (this.Properties.Values.ContainsKey(QuestClass.PropOffer))
		{
			this.Offer = this.Properties.Values[QuestClass.PropOffer];
		}
		if (this.Properties.Values.ContainsKey(QuestClass.PropOfferKey))
		{
			this.Offer = Localization.Get(this.Properties.Values[QuestClass.PropOfferKey], false, null);
		}
		if (this.Properties.Values.ContainsKey(QuestClass.PropIcon))
		{
			this.Icon = this.Properties.Values[QuestClass.PropIcon];
		}
		if (this.Properties.Values.ContainsKey(QuestClass.PropRepeatable))
		{
			bool flag;
			StringParsers.TryParseBool(this.Properties.Values[QuestClass.PropRepeatable], out flag);
			this.Repeatable = flag;
		}
		if (this.Properties.Values.ContainsKey(QuestClass.PropShareable))
		{
			bool flag2;
			StringParsers.TryParseBool(this.Properties.Values[QuestClass.PropShareable], out flag2);
			this.Shareable = flag2;
		}
		if (this.Properties.Values.ContainsKey(QuestClass.PropDifficulty))
		{
			this.Difficulty = Localization.Get(string.Format("difficulty_{0}", this.Properties.Values[QuestClass.PropDifficulty]), false, null);
		}
		if (this.Properties.Values.ContainsKey(QuestClass.PropCompletionType))
		{
			this.CompletionType = EnumUtils.Parse<QuestClass.CompletionTypes>(this.Properties.Values[QuestClass.PropCompletionType], false);
		}
		if (this.Properties.Values.ContainsKey(QuestClass.PropCurrentVersion))
		{
			this.CurrentVersion = Convert.ToByte(this.Properties.Values[QuestClass.PropCurrentVersion]);
		}
		if (this.Properties.Values.ContainsKey(QuestClass.PropStatementText))
		{
			this.StatementText = this.Properties.Values[QuestClass.PropStatementText];
		}
		if (this.Properties.Values.ContainsKey(QuestClass.PropResponseText))
		{
			this.ResponseText = this.Properties.Values[QuestClass.PropResponseText];
		}
		if (this.Properties.Values.ContainsKey(QuestClass.PropCompleteText))
		{
			this.CompleteText = this.Properties.Values[QuestClass.PropCompleteText];
		}
		if (this.Properties.Values.ContainsKey(QuestClass.PropStatementKey))
		{
			this.StatementText = Localization.Get(this.Properties.Values[QuestClass.PropStatementKey], false, null);
		}
		if (this.Properties.Values.ContainsKey(QuestClass.PropResponseKey))
		{
			this.ResponseText = Localization.Get(this.Properties.Values[QuestClass.PropResponseKey], false, null);
		}
		if (this.Properties.Values.ContainsKey(QuestClass.PropCompleteKey))
		{
			this.CompleteText = Localization.Get(this.Properties.Values[QuestClass.PropCompleteKey], false, null);
		}
		if (this.Properties.Values.ContainsKey(QuestClass.PropQuestFaction))
		{
			this.QuestFaction = Convert.ToByte(this.Properties.Values[QuestClass.PropQuestFaction]);
		}
		if (this.Properties.Values.ContainsKey(QuestClass.PropDifficultyTier))
		{
			this.DifficultyTier = Convert.ToByte(this.Properties.Values[QuestClass.PropDifficultyTier]);
		}
		if (this.Properties.Values.ContainsKey(QuestClass.PropLoginRallyReset))
		{
			this.LoginRallyReset = Convert.ToBoolean(this.Properties.Values[QuestClass.PropLoginRallyReset]);
		}
		if (this.Properties.Values.ContainsKey(QuestClass.PropUniqueKey))
		{
			this.UniqueKey = this.Properties.Values[QuestClass.PropUniqueKey];
		}
		else
		{
			this.UniqueKey = "";
		}
		if (this.Properties.Values.ContainsKey(QuestClass.PropReturnToQuestGiver))
		{
			this.ReturnToQuestGiver = StringParsers.ParseBool(this.Properties.Values[QuestClass.PropReturnToQuestGiver], 0, -1, true);
		}
		else
		{
			this.ReturnToQuestGiver = true;
		}
		if (this.Properties.Values.ContainsKey(QuestClass.PropQuestType))
		{
			this.QuestType = this.Properties.Values[QuestClass.PropQuestType];
		}
		else
		{
			this.QuestType = "";
		}
		if (this.Properties.Values.ContainsKey(QuestClass.PropAddsToTierComplete))
		{
			this.AddsToTierComplete = StringParsers.ParseBool(this.Properties.Values[QuestClass.PropAddsToTierComplete], 0, -1, true);
		}
		this.Properties.ParseInt(QuestClass.PropRewardChoicesCount, ref this.RewardChoicesCount);
		if (this.Properties.Values.ContainsKey(QuestClass.PropExtraTags))
		{
			string text = this.Properties.Values[QuestClass.PropExtraTags];
			if (text != "")
			{
				this.ExtraTags = FastTags<TagGroup.Global>.Parse(text);
			}
		}
		if (this.Properties.Values.ContainsKey(QuestClass.PropFilterTags))
		{
			string text2 = this.Properties.Values[QuestClass.PropFilterTags];
			if (text2 != "")
			{
				this.FilterTags = FastTags<TagGroup.Global>.Parse(text2);
			}
		}
		string text3 = "";
		this.Properties.ParseString(QuestClass.PropQuestHints, ref text3);
		if (text3 != "")
		{
			if (this.QuestHints == null)
			{
				this.QuestHints = new List<string>();
			}
			this.QuestHints.AddRange(text3.Split(',', StringSplitOptions.None));
		}
		string text4 = null;
		this.Properties.ParseString(QuestClass.PropQuestHintRequirement, ref text4);
		if (text4 != null)
		{
			Enum.TryParse<global::SandboxOptions.SandboxOptions>(text4, out this.QuestHintRequirement);
		}
		this.Properties.ParseFloat(QuestClass.PropQuestGameStageMod, ref this.GameStageMod);
		this.Properties.ParseFloat(QuestClass.PropQuestGameStageBonus, ref this.GameStageBonus);
		this.Properties.ParseFloat(QuestClass.PropSpawnMultiplier, ref this.SpawnMultiplier);
		this.Properties.ParseBool(QuestClass.PropResetTraderQuests, ref this.ResetTraderQuests);
		this.Properties.ParseBool(QuestClass.PropSingleQuest, ref this.SingleQuest);
		this.Properties.ParseBool(QuestClass.PropAlwaysAllow, ref this.AlwaysAllow);
		this.Properties.ParseBool(QuestClass.PropAllowRemove, ref this.AllowRemove);
		if (this.Properties.Values.ContainsKey(QuestClass.PropMaxQuestCount))
		{
			this.Properties.ParseInt(QuestClass.PropMaxQuestCount, ref this.MaxQuestCount);
		}
	}

	// Token: 0x06004A02 RID: 18946 RVA: 0x001D1624 File Offset: 0x001CF824
	public void HandleVariablesForProperties(DynamicProperties properties)
	{
		foreach (KeyValuePair<string, string> keyValuePair in properties.Params1)
		{
			if (this.Variables.ContainsKey(keyValuePair.Value))
			{
				properties.Values[keyValuePair.Key] = this.Variables[keyValuePair.Value];
			}
		}
	}

	// Token: 0x06004A03 RID: 18947 RVA: 0x001D16A8 File Offset: 0x001CF8A8
	public void HandleTemplateInit()
	{
		for (int i = 0; i < this.Actions.Count; i++)
		{
			this.HandleVariablesForProperties(this.Actions[i].Properties);
			this.Actions[i].ParseProperties(this.Actions[i].Properties);
		}
		for (int j = 0; j < this.Events.Count; j++)
		{
			this.HandleVariablesForProperties(this.Events[j].Properties);
			this.Events[j].ParseProperties(this.Events[j].Properties);
			for (int k = 0; k < this.Events[j].Actions.Count; k++)
			{
				BaseQuestAction baseQuestAction = this.Events[j].Actions[k];
				this.HandleVariablesForProperties(baseQuestAction.Properties);
				baseQuestAction.ParseProperties(baseQuestAction.Properties);
			}
		}
		for (int l = 0; l < this.Objectives.Count; l++)
		{
			BaseObjective baseObjective = this.Objectives[l];
			this.HandleVariablesForProperties(baseObjective.Properties);
			baseObjective.ParseProperties(baseObjective.Properties);
			if (baseObjective.Modifiers != null)
			{
				for (int m = 0; m < baseObjective.Modifiers.Count; m++)
				{
					BaseObjectiveModifier baseObjectiveModifier = baseObjective.Modifiers[m];
					this.HandleVariablesForProperties(baseObjectiveModifier.Properties);
					baseObjectiveModifier.ParseProperties(baseObjectiveModifier.Properties);
				}
			}
		}
	}

	// Token: 0x04003AC6 RID: 15046
	public static Dictionary<string, QuestClass> s_Quests = new CaseInsensitiveStringDictionary<QuestClass>();

	// Token: 0x04003AC7 RID: 15047
	public static string PropCategory = "category";

	// Token: 0x04003AC8 RID: 15048
	public static string PropCategoryKey = "category_key";

	// Token: 0x04003AC9 RID: 15049
	public static string PropGroupName = "group_name";

	// Token: 0x04003ACA RID: 15050
	public static string PropGroupNameKey = "group_name_key";

	// Token: 0x04003ACB RID: 15051
	public static string PropName = "name";

	// Token: 0x04003ACC RID: 15052
	public static string PropNameKey = "name_key";

	// Token: 0x04003ACD RID: 15053
	public static string PropSubtitle = "subtitle";

	// Token: 0x04003ACE RID: 15054
	public static string PropSubtitleKey = "subtitle_key";

	// Token: 0x04003ACF RID: 15055
	public static string PropDescription = "description";

	// Token: 0x04003AD0 RID: 15056
	public static string PropDescriptionKey = "description_key";

	// Token: 0x04003AD1 RID: 15057
	public static string PropOffer = "offer";

	// Token: 0x04003AD2 RID: 15058
	public static string PropOfferKey = "offer_key";

	// Token: 0x04003AD3 RID: 15059
	public static string PropIcon = "icon";

	// Token: 0x04003AD4 RID: 15060
	public static string PropRepeatable = "repeatable";

	// Token: 0x04003AD5 RID: 15061
	public static string PropShareable = "shareable";

	// Token: 0x04003AD6 RID: 15062
	public static string PropDifficulty = "difficulty";

	// Token: 0x04003AD7 RID: 15063
	public static string PropCompletionType = "completiontype";

	// Token: 0x04003AD8 RID: 15064
	public static string PropCurrentVersion = "currentversion";

	// Token: 0x04003AD9 RID: 15065
	public static string PropStatementText = "statement_text";

	// Token: 0x04003ADA RID: 15066
	public static string PropResponseText = "response_text";

	// Token: 0x04003ADB RID: 15067
	public static string PropCompleteText = "completion_text";

	// Token: 0x04003ADC RID: 15068
	public static string PropStatementKey = "statement_key";

	// Token: 0x04003ADD RID: 15069
	public static string PropResponseKey = "response_key";

	// Token: 0x04003ADE RID: 15070
	public static string PropCompleteKey = "completion_key";

	// Token: 0x04003ADF RID: 15071
	public static string PropVariations = "variations";

	// Token: 0x04003AE0 RID: 15072
	public static string PropQuestFaction = "quest_faction";

	// Token: 0x04003AE1 RID: 15073
	public static string PropDifficultyTier = "difficulty_tier";

	// Token: 0x04003AE2 RID: 15074
	public static string PropLoginRallyReset = "login_rally_reset";

	// Token: 0x04003AE3 RID: 15075
	public static string PropUniqueKey = "unique_key";

	// Token: 0x04003AE4 RID: 15076
	public static string PropReturnToQuestGiver = "return_to_quest_giver";

	// Token: 0x04003AE5 RID: 15077
	public static string PropQuestType = "quest_type";

	// Token: 0x04003AE6 RID: 15078
	public static string PropAddsToTierComplete = "add_to_tier_complete";

	// Token: 0x04003AE7 RID: 15079
	public static string PropRewardChoicesCount = "reward_choices_count";

	// Token: 0x04003AE8 RID: 15080
	public static string PropExtraTags = "extra_tags";

	// Token: 0x04003AE9 RID: 15081
	public static string PropFilterTags = "filter_tags";

	// Token: 0x04003AEA RID: 15082
	public static string PropQuestStage = "quest_stage";

	// Token: 0x04003AEB RID: 15083
	public static string PropQuestHints = "quest_hints";

	// Token: 0x04003AEC RID: 15084
	public static string PropQuestHintRequirement = "quest_hint_requirement";

	// Token: 0x04003AED RID: 15085
	public static string PropQuestGameStageMod = "gamestage_mod";

	// Token: 0x04003AEE RID: 15086
	public static string PropQuestGameStageBonus = "gamestage_bonus";

	// Token: 0x04003AEF RID: 15087
	public static string PropSpawnMultiplier = "spawn_multiplier";

	// Token: 0x04003AF0 RID: 15088
	public static string PropResetTraderQuests = "reset_trader_quests";

	// Token: 0x04003AF1 RID: 15089
	public static string PropSingleQuest = "single_quest";

	// Token: 0x04003AF2 RID: 15090
	public static string PropAlwaysAllow = "always_allow";

	// Token: 0x04003AF3 RID: 15091
	public static string PropAllowRemove = "allow_remove";

	// Token: 0x04003AF4 RID: 15092
	public static string PropMaxQuestCount = "max_quest_count";

	// Token: 0x04003B0C RID: 15116
	public List<string> QuestHints;

	// Token: 0x04003B0D RID: 15117
	public global::SandboxOptions.SandboxOptions QuestHintRequirement = global::SandboxOptions.SandboxOptions.Max;

	// Token: 0x04003B0E RID: 15118
	public int RewardChoicesCount = 2;

	// Token: 0x04003B0F RID: 15119
	public FastTags<TagGroup.Global> ExtraTags = FastTags<TagGroup.Global>.none;

	// Token: 0x04003B10 RID: 15120
	public FastTags<TagGroup.Global> FilterTags = FastTags<TagGroup.Global>.none;

	// Token: 0x04003B11 RID: 15121
	public float GameStageMod;

	// Token: 0x04003B12 RID: 15122
	public float GameStageBonus;

	// Token: 0x04003B13 RID: 15123
	public float SpawnMultiplier = 1f;

	// Token: 0x04003B14 RID: 15124
	public bool ResetTraderQuests;

	// Token: 0x04003B15 RID: 15125
	public bool SingleQuest;

	// Token: 0x04003B16 RID: 15126
	public bool AlwaysAllow;

	// Token: 0x04003B17 RID: 15127
	public bool AllowRemove = true;

	// Token: 0x04003B18 RID: 15128
	public int MaxQuestCount;

	// Token: 0x04003B19 RID: 15129
	public QuestClass.CompletionTypes CompletionType;

	// Token: 0x04003B1A RID: 15130
	public List<BaseQuestCriteria> Criteria = new List<BaseQuestCriteria>();

	// Token: 0x04003B1B RID: 15131
	public List<BaseQuestAction> Actions = new List<BaseQuestAction>();

	// Token: 0x04003B1C RID: 15132
	public List<QuestEvent> Events = new List<QuestEvent>();

	// Token: 0x04003B1D RID: 15133
	public List<BaseRequirement> Requirements = new List<BaseRequirement>();

	// Token: 0x04003B1E RID: 15134
	public List<BaseObjective> Objectives = new List<BaseObjective>();

	// Token: 0x04003B1F RID: 15135
	public List<BaseReward> Rewards = new List<BaseReward>();

	// Token: 0x04003B20 RID: 15136
	public Dictionary<string, string> Variables = new Dictionary<string, string>();

	// Token: 0x04003B21 RID: 15137
	public DynamicProperties Properties = new DynamicProperties();

	// Token: 0x020009CA RID: 2506
	public enum CompletionTypes
	{
		// Token: 0x04003B23 RID: 15139
		AutoComplete,
		// Token: 0x04003B24 RID: 15140
		TurnIn
	}
}
