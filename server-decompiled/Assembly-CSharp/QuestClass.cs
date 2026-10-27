using System;
using System.Collections.Generic;
using Platform;
using Quests.Requirements;
using SandboxOptions;

// Token: 0x020009D3 RID: 2515
public class QuestClass
{
	// Token: 0x1700080E RID: 2062
	// (get) Token: 0x060049E6 RID: 18918 RVA: 0x001D1583 File Offset: 0x001CF783
	// (set) Token: 0x060049E7 RID: 18919 RVA: 0x001D158B File Offset: 0x001CF78B
	public string ID
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x1700080F RID: 2063
	// (get) Token: 0x060049E8 RID: 18920 RVA: 0x001D1594 File Offset: 0x001CF794
	// (set) Token: 0x060049E9 RID: 18921 RVA: 0x001D159C File Offset: 0x001CF79C
	public string Name { get; set; }

	// Token: 0x17000810 RID: 2064
	// (get) Token: 0x060049EA RID: 18922 RVA: 0x001D15A5 File Offset: 0x001CF7A5
	// (set) Token: 0x060049EB RID: 18923 RVA: 0x001D15AD File Offset: 0x001CF7AD
	public string GroupName { get; set; }

	// Token: 0x17000811 RID: 2065
	// (get) Token: 0x060049EC RID: 18924 RVA: 0x001D15B6 File Offset: 0x001CF7B6
	// (set) Token: 0x060049ED RID: 18925 RVA: 0x001D15BE File Offset: 0x001CF7BE
	public string SubTitle { get; set; }

	// Token: 0x17000812 RID: 2066
	// (get) Token: 0x060049EE RID: 18926 RVA: 0x001D15C7 File Offset: 0x001CF7C7
	// (set) Token: 0x060049EF RID: 18927 RVA: 0x001D15CF File Offset: 0x001CF7CF
	public string Description { get; set; }

	// Token: 0x17000813 RID: 2067
	// (get) Token: 0x060049F0 RID: 18928 RVA: 0x001D15D8 File Offset: 0x001CF7D8
	// (set) Token: 0x060049F1 RID: 18929 RVA: 0x001D15E0 File Offset: 0x001CF7E0
	public string Offer { get; set; }

	// Token: 0x17000814 RID: 2068
	// (get) Token: 0x060049F2 RID: 18930 RVA: 0x001D15E9 File Offset: 0x001CF7E9
	// (set) Token: 0x060049F3 RID: 18931 RVA: 0x001D15F1 File Offset: 0x001CF7F1
	public string Difficulty { get; set; }

	// Token: 0x17000815 RID: 2069
	// (get) Token: 0x060049F4 RID: 18932 RVA: 0x001D15FA File Offset: 0x001CF7FA
	// (set) Token: 0x060049F5 RID: 18933 RVA: 0x001D1602 File Offset: 0x001CF802
	public string Icon { get; set; }

	// Token: 0x17000816 RID: 2070
	// (get) Token: 0x060049F6 RID: 18934 RVA: 0x001D160B File Offset: 0x001CF80B
	// (set) Token: 0x060049F7 RID: 18935 RVA: 0x001D1613 File Offset: 0x001CF813
	public bool Repeatable { get; set; }

	// Token: 0x17000817 RID: 2071
	// (get) Token: 0x060049F8 RID: 18936 RVA: 0x001D161C File Offset: 0x001CF81C
	// (set) Token: 0x060049F9 RID: 18937 RVA: 0x001D1624 File Offset: 0x001CF824
	public bool Shareable { get; set; }

	// Token: 0x17000818 RID: 2072
	// (get) Token: 0x060049FA RID: 18938 RVA: 0x001D162D File Offset: 0x001CF82D
	// (set) Token: 0x060049FB RID: 18939 RVA: 0x001D1635 File Offset: 0x001CF835
	public string Category { get; set; }

	// Token: 0x17000819 RID: 2073
	// (get) Token: 0x060049FC RID: 18940 RVA: 0x001D163E File Offset: 0x001CF83E
	// (set) Token: 0x060049FD RID: 18941 RVA: 0x001D1646 File Offset: 0x001CF846
	public string StatementText { get; set; }

	// Token: 0x1700081A RID: 2074
	// (get) Token: 0x060049FE RID: 18942 RVA: 0x001D164F File Offset: 0x001CF84F
	// (set) Token: 0x060049FF RID: 18943 RVA: 0x001D1657 File Offset: 0x001CF857
	public string ResponseText { get; set; }

	// Token: 0x1700081B RID: 2075
	// (get) Token: 0x06004A00 RID: 18944 RVA: 0x001D1660 File Offset: 0x001CF860
	// (set) Token: 0x06004A01 RID: 18945 RVA: 0x001D1668 File Offset: 0x001CF868
	public string CompleteText { get; set; }

	// Token: 0x1700081C RID: 2076
	// (get) Token: 0x06004A02 RID: 18946 RVA: 0x001D1671 File Offset: 0x001CF871
	// (set) Token: 0x06004A03 RID: 18947 RVA: 0x001D1679 File Offset: 0x001CF879
	public byte CurrentVersion { get; set; }

	// Token: 0x1700081D RID: 2077
	// (get) Token: 0x06004A04 RID: 18948 RVA: 0x001D1682 File Offset: 0x001CF882
	// (set) Token: 0x06004A05 RID: 18949 RVA: 0x001D168A File Offset: 0x001CF88A
	public byte HighestPhase { get; set; }

	// Token: 0x1700081E RID: 2078
	// (get) Token: 0x06004A06 RID: 18950 RVA: 0x001D1693 File Offset: 0x001CF893
	// (set) Token: 0x06004A07 RID: 18951 RVA: 0x001D169B File Offset: 0x001CF89B
	public byte QuestFaction { get; set; }

	// Token: 0x1700081F RID: 2079
	// (get) Token: 0x06004A08 RID: 18952 RVA: 0x001D16A4 File Offset: 0x001CF8A4
	// (set) Token: 0x06004A09 RID: 18953 RVA: 0x001D16AC File Offset: 0x001CF8AC
	public byte DifficultyTier { get; set; }

	// Token: 0x17000820 RID: 2080
	// (get) Token: 0x06004A0A RID: 18954 RVA: 0x001D16B5 File Offset: 0x001CF8B5
	// (set) Token: 0x06004A0B RID: 18955 RVA: 0x001D16BD File Offset: 0x001CF8BD
	public bool LoginRallyReset { get; set; }

	// Token: 0x17000821 RID: 2081
	// (get) Token: 0x06004A0C RID: 18956 RVA: 0x001D16C6 File Offset: 0x001CF8C6
	// (set) Token: 0x06004A0D RID: 18957 RVA: 0x001D16CE File Offset: 0x001CF8CE
	public bool ReturnToQuestGiver { get; set; }

	// Token: 0x17000822 RID: 2082
	// (get) Token: 0x06004A0E RID: 18958 RVA: 0x001D16D7 File Offset: 0x001CF8D7
	// (set) Token: 0x06004A0F RID: 18959 RVA: 0x001D16DF File Offset: 0x001CF8DF
	public string UniqueKey { get; set; }

	// Token: 0x17000823 RID: 2083
	// (get) Token: 0x06004A10 RID: 18960 RVA: 0x001D16E8 File Offset: 0x001CF8E8
	// (set) Token: 0x06004A11 RID: 18961 RVA: 0x001D16F0 File Offset: 0x001CF8F0
	public string QuestType { get; set; }

	// Token: 0x17000824 RID: 2084
	// (get) Token: 0x06004A12 RID: 18962 RVA: 0x001D16F9 File Offset: 0x001CF8F9
	// (set) Token: 0x06004A13 RID: 18963 RVA: 0x001D1701 File Offset: 0x001CF901
	public bool AddsToTierComplete { get; set; }

	// Token: 0x06004A14 RID: 18964 RVA: 0x001D170C File Offset: 0x001CF90C
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

	// Token: 0x06004A15 RID: 18965 RVA: 0x001D1744 File Offset: 0x001CF944
	[PublicizedFrom(EAccessModifier.Private)]
	public QuestClass(string id)
	{
		this.ID = id;
		this.Difficulty = "veryeasy";
		this.HighestPhase = 1;
		this.AddsToTierComplete = true;
	}

	// Token: 0x06004A16 RID: 18966 RVA: 0x001D1809 File Offset: 0x001CFA09
	[PublicizedFrom(EAccessModifier.Internal)]
	public static QuestClass GetQuest(string questID)
	{
		if (!QuestClass.s_Quests.ContainsKey(questID))
		{
			return null;
		}
		return QuestClass.s_Quests[questID];
	}

	// Token: 0x06004A17 RID: 18967 RVA: 0x001D1828 File Offset: 0x001CFA28
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

	// Token: 0x06004A18 RID: 18968 RVA: 0x001D1A8C File Offset: 0x001CFC8C
	public static Quest CreateQuest(string ID)
	{
		QuestClass quest = QuestClass.GetQuest(ID);
		if (quest != null)
		{
			return quest.CreateQuest();
		}
		return null;
	}

	// Token: 0x06004A19 RID: 18969 RVA: 0x001D1AAC File Offset: 0x001CFCAC
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

	// Token: 0x06004A1A RID: 18970 RVA: 0x001D1C38 File Offset: 0x001CFE38
	public void ResetObjectives()
	{
		for (int i = 0; i < this.Objectives.Count; i++)
		{
			this.Objectives[i].ResetObjective();
		}
	}

	// Token: 0x06004A1B RID: 18971 RVA: 0x001D1C6C File Offset: 0x001CFE6C
	[PublicizedFrom(EAccessModifier.Internal)]
	public BaseQuestAction AddAction(BaseQuestAction action)
	{
		if (action != null)
		{
			this.Actions.Add(action);
		}
		return action;
	}

	// Token: 0x06004A1C RID: 18972 RVA: 0x001D1C7E File Offset: 0x001CFE7E
	public QuestEvent AddEvent(QuestEvent questEvent)
	{
		if (questEvent != null)
		{
			this.Events.Add(questEvent);
		}
		return questEvent;
	}

	// Token: 0x06004A1D RID: 18973 RVA: 0x001D1C90 File Offset: 0x001CFE90
	[PublicizedFrom(EAccessModifier.Internal)]
	public BaseRequirement AddRequirement(BaseRequirement requirement)
	{
		if (requirement != null)
		{
			this.Requirements.Add(requirement);
		}
		return requirement;
	}

	// Token: 0x06004A1E RID: 18974 RVA: 0x001D1CA2 File Offset: 0x001CFEA2
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

	// Token: 0x06004A1F RID: 18975 RVA: 0x001D1CBB File Offset: 0x001CFEBB
	[PublicizedFrom(EAccessModifier.Internal)]
	public BaseReward AddReward(BaseReward reward)
	{
		if (reward != null)
		{
			this.Rewards.Add(reward);
		}
		return reward;
	}

	// Token: 0x06004A20 RID: 18976 RVA: 0x001D1CCD File Offset: 0x001CFECD
	[PublicizedFrom(EAccessModifier.Internal)]
	public BaseQuestCriteria AddCriteria(BaseQuestCriteria criteria)
	{
		if (criteria != null)
		{
			this.Criteria.Add(criteria);
		}
		return criteria;
	}

	// Token: 0x06004A21 RID: 18977 RVA: 0x001D1CE0 File Offset: 0x001CFEE0
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

	// Token: 0x06004A22 RID: 18978 RVA: 0x001D1D30 File Offset: 0x001CFF30
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

	// Token: 0x06004A23 RID: 18979 RVA: 0x001D1D80 File Offset: 0x001CFF80
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

	// Token: 0x06004A24 RID: 18980 RVA: 0x001D1DC4 File Offset: 0x001CFFC4
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

	// Token: 0x06004A25 RID: 18981 RVA: 0x001D1E58 File Offset: 0x001D0058
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

	// Token: 0x06004A26 RID: 18982 RVA: 0x001D2704 File Offset: 0x001D0904
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

	// Token: 0x06004A27 RID: 18983 RVA: 0x001D2788 File Offset: 0x001D0988
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

	// Token: 0x04003AE7 RID: 15079
	public static Dictionary<string, QuestClass> s_Quests = new CaseInsensitiveStringDictionary<QuestClass>();

	// Token: 0x04003AE8 RID: 15080
	public static string PropCategory = "category";

	// Token: 0x04003AE9 RID: 15081
	public static string PropCategoryKey = "category_key";

	// Token: 0x04003AEA RID: 15082
	public static string PropGroupName = "group_name";

	// Token: 0x04003AEB RID: 15083
	public static string PropGroupNameKey = "group_name_key";

	// Token: 0x04003AEC RID: 15084
	public static string PropName = "name";

	// Token: 0x04003AED RID: 15085
	public static string PropNameKey = "name_key";

	// Token: 0x04003AEE RID: 15086
	public static string PropSubtitle = "subtitle";

	// Token: 0x04003AEF RID: 15087
	public static string PropSubtitleKey = "subtitle_key";

	// Token: 0x04003AF0 RID: 15088
	public static string PropDescription = "description";

	// Token: 0x04003AF1 RID: 15089
	public static string PropDescriptionKey = "description_key";

	// Token: 0x04003AF2 RID: 15090
	public static string PropOffer = "offer";

	// Token: 0x04003AF3 RID: 15091
	public static string PropOfferKey = "offer_key";

	// Token: 0x04003AF4 RID: 15092
	public static string PropIcon = "icon";

	// Token: 0x04003AF5 RID: 15093
	public static string PropRepeatable = "repeatable";

	// Token: 0x04003AF6 RID: 15094
	public static string PropShareable = "shareable";

	// Token: 0x04003AF7 RID: 15095
	public static string PropDifficulty = "difficulty";

	// Token: 0x04003AF8 RID: 15096
	public static string PropCompletionType = "completiontype";

	// Token: 0x04003AF9 RID: 15097
	public static string PropCurrentVersion = "currentversion";

	// Token: 0x04003AFA RID: 15098
	public static string PropStatementText = "statement_text";

	// Token: 0x04003AFB RID: 15099
	public static string PropResponseText = "response_text";

	// Token: 0x04003AFC RID: 15100
	public static string PropCompleteText = "completion_text";

	// Token: 0x04003AFD RID: 15101
	public static string PropStatementKey = "statement_key";

	// Token: 0x04003AFE RID: 15102
	public static string PropResponseKey = "response_key";

	// Token: 0x04003AFF RID: 15103
	public static string PropCompleteKey = "completion_key";

	// Token: 0x04003B00 RID: 15104
	public static string PropVariations = "variations";

	// Token: 0x04003B01 RID: 15105
	public static string PropQuestFaction = "quest_faction";

	// Token: 0x04003B02 RID: 15106
	public static string PropDifficultyTier = "difficulty_tier";

	// Token: 0x04003B03 RID: 15107
	public static string PropLoginRallyReset = "login_rally_reset";

	// Token: 0x04003B04 RID: 15108
	public static string PropUniqueKey = "unique_key";

	// Token: 0x04003B05 RID: 15109
	public static string PropReturnToQuestGiver = "return_to_quest_giver";

	// Token: 0x04003B06 RID: 15110
	public static string PropQuestType = "quest_type";

	// Token: 0x04003B07 RID: 15111
	public static string PropAddsToTierComplete = "add_to_tier_complete";

	// Token: 0x04003B08 RID: 15112
	public static string PropRewardChoicesCount = "reward_choices_count";

	// Token: 0x04003B09 RID: 15113
	public static string PropExtraTags = "extra_tags";

	// Token: 0x04003B0A RID: 15114
	public static string PropFilterTags = "filter_tags";

	// Token: 0x04003B0B RID: 15115
	public static string PropQuestStage = "quest_stage";

	// Token: 0x04003B0C RID: 15116
	public static string PropQuestHints = "quest_hints";

	// Token: 0x04003B0D RID: 15117
	public static string PropQuestHintRequirement = "quest_hint_requirement";

	// Token: 0x04003B0E RID: 15118
	public static string PropQuestGameStageMod = "gamestage_mod";

	// Token: 0x04003B0F RID: 15119
	public static string PropQuestGameStageBonus = "gamestage_bonus";

	// Token: 0x04003B10 RID: 15120
	public static string PropSpawnMultiplier = "spawn_multiplier";

	// Token: 0x04003B11 RID: 15121
	public static string PropResetTraderQuests = "reset_trader_quests";

	// Token: 0x04003B12 RID: 15122
	public static string PropSingleQuest = "single_quest";

	// Token: 0x04003B13 RID: 15123
	public static string PropAlwaysAllow = "always_allow";

	// Token: 0x04003B14 RID: 15124
	public static string PropAllowRemove = "allow_remove";

	// Token: 0x04003B15 RID: 15125
	public static string PropMaxQuestCount = "max_quest_count";

	// Token: 0x04003B2D RID: 15149
	public List<string> QuestHints;

	// Token: 0x04003B2E RID: 15150
	public global::SandboxOptions.SandboxOptions QuestHintRequirement = global::SandboxOptions.SandboxOptions.Max;

	// Token: 0x04003B2F RID: 15151
	public int RewardChoicesCount = 2;

	// Token: 0x04003B30 RID: 15152
	public FastTags<TagGroup.Global> ExtraTags = FastTags<TagGroup.Global>.none;

	// Token: 0x04003B31 RID: 15153
	public FastTags<TagGroup.Global> FilterTags = FastTags<TagGroup.Global>.none;

	// Token: 0x04003B32 RID: 15154
	public float GameStageMod;

	// Token: 0x04003B33 RID: 15155
	public float GameStageBonus;

	// Token: 0x04003B34 RID: 15156
	public float SpawnMultiplier = 1f;

	// Token: 0x04003B35 RID: 15157
	public bool ResetTraderQuests;

	// Token: 0x04003B36 RID: 15158
	public bool SingleQuest;

	// Token: 0x04003B37 RID: 15159
	public bool AlwaysAllow;

	// Token: 0x04003B38 RID: 15160
	public bool AllowRemove = true;

	// Token: 0x04003B39 RID: 15161
	public int MaxQuestCount;

	// Token: 0x04003B3A RID: 15162
	public QuestClass.CompletionTypes CompletionType;

	// Token: 0x04003B3B RID: 15163
	public List<BaseQuestCriteria> Criteria = new List<BaseQuestCriteria>();

	// Token: 0x04003B3C RID: 15164
	public List<BaseQuestAction> Actions = new List<BaseQuestAction>();

	// Token: 0x04003B3D RID: 15165
	public List<QuestEvent> Events = new List<QuestEvent>();

	// Token: 0x04003B3E RID: 15166
	public List<BaseRequirement> Requirements = new List<BaseRequirement>();

	// Token: 0x04003B3F RID: 15167
	public List<BaseObjective> Objectives = new List<BaseObjective>();

	// Token: 0x04003B40 RID: 15168
	public List<BaseReward> Rewards = new List<BaseReward>();

	// Token: 0x04003B41 RID: 15169
	public Dictionary<string, string> Variables = new Dictionary<string, string>();

	// Token: 0x04003B42 RID: 15170
	public DynamicProperties Properties = new DynamicProperties();

	// Token: 0x020009D4 RID: 2516
	public enum CompletionTypes
	{
		// Token: 0x04003B44 RID: 15172
		AutoComplete,
		// Token: 0x04003B45 RID: 15173
		TurnIn
	}
}
