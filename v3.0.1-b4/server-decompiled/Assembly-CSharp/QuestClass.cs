using System;
using System.Collections.Generic;
using Platform;
using Quests.Requirements;
using SandboxOptions;

// Token: 0x020009AF RID: 2479
public class QuestClass
{
	// Token: 0x170007D6 RID: 2006
	// (get) Token: 0x06004972 RID: 18802 RVA: 0x001CCA83 File Offset: 0x001CAC83
	// (set) Token: 0x06004973 RID: 18803 RVA: 0x001CCA8B File Offset: 0x001CAC8B
	public string ID
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x170007D7 RID: 2007
	// (get) Token: 0x06004974 RID: 18804 RVA: 0x001CCA94 File Offset: 0x001CAC94
	// (set) Token: 0x06004975 RID: 18805 RVA: 0x001CCA9C File Offset: 0x001CAC9C
	public string Name { get; set; }

	// Token: 0x170007D8 RID: 2008
	// (get) Token: 0x06004976 RID: 18806 RVA: 0x001CCAA5 File Offset: 0x001CACA5
	// (set) Token: 0x06004977 RID: 18807 RVA: 0x001CCAAD File Offset: 0x001CACAD
	public string GroupName { get; set; }

	// Token: 0x170007D9 RID: 2009
	// (get) Token: 0x06004978 RID: 18808 RVA: 0x001CCAB6 File Offset: 0x001CACB6
	// (set) Token: 0x06004979 RID: 18809 RVA: 0x001CCABE File Offset: 0x001CACBE
	public string SubTitle { get; set; }

	// Token: 0x170007DA RID: 2010
	// (get) Token: 0x0600497A RID: 18810 RVA: 0x001CCAC7 File Offset: 0x001CACC7
	// (set) Token: 0x0600497B RID: 18811 RVA: 0x001CCACF File Offset: 0x001CACCF
	public string Description { get; set; }

	// Token: 0x170007DB RID: 2011
	// (get) Token: 0x0600497C RID: 18812 RVA: 0x001CCAD8 File Offset: 0x001CACD8
	// (set) Token: 0x0600497D RID: 18813 RVA: 0x001CCAE0 File Offset: 0x001CACE0
	public string Offer { get; set; }

	// Token: 0x170007DC RID: 2012
	// (get) Token: 0x0600497E RID: 18814 RVA: 0x001CCAE9 File Offset: 0x001CACE9
	// (set) Token: 0x0600497F RID: 18815 RVA: 0x001CCAF1 File Offset: 0x001CACF1
	public string Difficulty { get; set; }

	// Token: 0x170007DD RID: 2013
	// (get) Token: 0x06004980 RID: 18816 RVA: 0x001CCAFA File Offset: 0x001CACFA
	// (set) Token: 0x06004981 RID: 18817 RVA: 0x001CCB02 File Offset: 0x001CAD02
	public string Icon { get; set; }

	// Token: 0x170007DE RID: 2014
	// (get) Token: 0x06004982 RID: 18818 RVA: 0x001CCB0B File Offset: 0x001CAD0B
	// (set) Token: 0x06004983 RID: 18819 RVA: 0x001CCB13 File Offset: 0x001CAD13
	public bool Repeatable { get; set; }

	// Token: 0x170007DF RID: 2015
	// (get) Token: 0x06004984 RID: 18820 RVA: 0x001CCB1C File Offset: 0x001CAD1C
	// (set) Token: 0x06004985 RID: 18821 RVA: 0x001CCB24 File Offset: 0x001CAD24
	public bool Shareable { get; set; }

	// Token: 0x170007E0 RID: 2016
	// (get) Token: 0x06004986 RID: 18822 RVA: 0x001CCB2D File Offset: 0x001CAD2D
	// (set) Token: 0x06004987 RID: 18823 RVA: 0x001CCB35 File Offset: 0x001CAD35
	public string Category { get; set; }

	// Token: 0x170007E1 RID: 2017
	// (get) Token: 0x06004988 RID: 18824 RVA: 0x001CCB3E File Offset: 0x001CAD3E
	// (set) Token: 0x06004989 RID: 18825 RVA: 0x001CCB46 File Offset: 0x001CAD46
	public string StatementText { get; set; }

	// Token: 0x170007E2 RID: 2018
	// (get) Token: 0x0600498A RID: 18826 RVA: 0x001CCB4F File Offset: 0x001CAD4F
	// (set) Token: 0x0600498B RID: 18827 RVA: 0x001CCB57 File Offset: 0x001CAD57
	public string ResponseText { get; set; }

	// Token: 0x170007E3 RID: 2019
	// (get) Token: 0x0600498C RID: 18828 RVA: 0x001CCB60 File Offset: 0x001CAD60
	// (set) Token: 0x0600498D RID: 18829 RVA: 0x001CCB68 File Offset: 0x001CAD68
	public string CompleteText { get; set; }

	// Token: 0x170007E4 RID: 2020
	// (get) Token: 0x0600498E RID: 18830 RVA: 0x001CCB71 File Offset: 0x001CAD71
	// (set) Token: 0x0600498F RID: 18831 RVA: 0x001CCB79 File Offset: 0x001CAD79
	public byte CurrentVersion { get; set; }

	// Token: 0x170007E5 RID: 2021
	// (get) Token: 0x06004990 RID: 18832 RVA: 0x001CCB82 File Offset: 0x001CAD82
	// (set) Token: 0x06004991 RID: 18833 RVA: 0x001CCB8A File Offset: 0x001CAD8A
	public byte HighestPhase { get; set; }

	// Token: 0x170007E6 RID: 2022
	// (get) Token: 0x06004992 RID: 18834 RVA: 0x001CCB93 File Offset: 0x001CAD93
	// (set) Token: 0x06004993 RID: 18835 RVA: 0x001CCB9B File Offset: 0x001CAD9B
	public byte QuestFaction { get; set; }

	// Token: 0x170007E7 RID: 2023
	// (get) Token: 0x06004994 RID: 18836 RVA: 0x001CCBA4 File Offset: 0x001CADA4
	// (set) Token: 0x06004995 RID: 18837 RVA: 0x001CCBAC File Offset: 0x001CADAC
	public byte DifficultyTier { get; set; }

	// Token: 0x170007E8 RID: 2024
	// (get) Token: 0x06004996 RID: 18838 RVA: 0x001CCBB5 File Offset: 0x001CADB5
	// (set) Token: 0x06004997 RID: 18839 RVA: 0x001CCBBD File Offset: 0x001CADBD
	public bool LoginRallyReset { get; set; }

	// Token: 0x170007E9 RID: 2025
	// (get) Token: 0x06004998 RID: 18840 RVA: 0x001CCBC6 File Offset: 0x001CADC6
	// (set) Token: 0x06004999 RID: 18841 RVA: 0x001CCBCE File Offset: 0x001CADCE
	public bool ReturnToQuestGiver { get; set; }

	// Token: 0x170007EA RID: 2026
	// (get) Token: 0x0600499A RID: 18842 RVA: 0x001CCBD7 File Offset: 0x001CADD7
	// (set) Token: 0x0600499B RID: 18843 RVA: 0x001CCBDF File Offset: 0x001CADDF
	public string UniqueKey { get; set; }

	// Token: 0x170007EB RID: 2027
	// (get) Token: 0x0600499C RID: 18844 RVA: 0x001CCBE8 File Offset: 0x001CADE8
	// (set) Token: 0x0600499D RID: 18845 RVA: 0x001CCBF0 File Offset: 0x001CADF0
	public string QuestType { get; set; }

	// Token: 0x170007EC RID: 2028
	// (get) Token: 0x0600499E RID: 18846 RVA: 0x001CCBF9 File Offset: 0x001CADF9
	// (set) Token: 0x0600499F RID: 18847 RVA: 0x001CCC01 File Offset: 0x001CAE01
	public bool AddsToTierComplete { get; set; }

	// Token: 0x060049A0 RID: 18848 RVA: 0x001CCC0C File Offset: 0x001CAE0C
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

	// Token: 0x060049A1 RID: 18849 RVA: 0x001CCC44 File Offset: 0x001CAE44
	[PublicizedFrom(EAccessModifier.Private)]
	public QuestClass(string id)
	{
		this.ID = id;
		this.Difficulty = "veryeasy";
		this.HighestPhase = 1;
		this.AddsToTierComplete = true;
	}

	// Token: 0x060049A2 RID: 18850 RVA: 0x001CCD09 File Offset: 0x001CAF09
	[PublicizedFrom(EAccessModifier.Internal)]
	public static QuestClass GetQuest(string questID)
	{
		if (!QuestClass.s_Quests.ContainsKey(questID))
		{
			return null;
		}
		return QuestClass.s_Quests[questID];
	}

	// Token: 0x060049A3 RID: 18851 RVA: 0x001CCD28 File Offset: 0x001CAF28
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

	// Token: 0x060049A4 RID: 18852 RVA: 0x001CCF8C File Offset: 0x001CB18C
	public static Quest CreateQuest(string ID)
	{
		QuestClass quest = QuestClass.GetQuest(ID);
		if (quest != null)
		{
			return quest.CreateQuest();
		}
		return null;
	}

	// Token: 0x060049A5 RID: 18853 RVA: 0x001CCFAC File Offset: 0x001CB1AC
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

	// Token: 0x060049A6 RID: 18854 RVA: 0x001CD138 File Offset: 0x001CB338
	public void ResetObjectives()
	{
		for (int i = 0; i < this.Objectives.Count; i++)
		{
			this.Objectives[i].ResetObjective();
		}
	}

	// Token: 0x060049A7 RID: 18855 RVA: 0x001CD16C File Offset: 0x001CB36C
	[PublicizedFrom(EAccessModifier.Internal)]
	public BaseQuestAction AddAction(BaseQuestAction action)
	{
		if (action != null)
		{
			this.Actions.Add(action);
		}
		return action;
	}

	// Token: 0x060049A8 RID: 18856 RVA: 0x001CD17E File Offset: 0x001CB37E
	public QuestEvent AddEvent(QuestEvent questEvent)
	{
		if (questEvent != null)
		{
			this.Events.Add(questEvent);
		}
		return questEvent;
	}

	// Token: 0x060049A9 RID: 18857 RVA: 0x001CD190 File Offset: 0x001CB390
	[PublicizedFrom(EAccessModifier.Internal)]
	public BaseRequirement AddRequirement(BaseRequirement requirement)
	{
		if (requirement != null)
		{
			this.Requirements.Add(requirement);
		}
		return requirement;
	}

	// Token: 0x060049AA RID: 18858 RVA: 0x001CD1A2 File Offset: 0x001CB3A2
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

	// Token: 0x060049AB RID: 18859 RVA: 0x001CD1BB File Offset: 0x001CB3BB
	[PublicizedFrom(EAccessModifier.Internal)]
	public BaseReward AddReward(BaseReward reward)
	{
		if (reward != null)
		{
			this.Rewards.Add(reward);
		}
		return reward;
	}

	// Token: 0x060049AC RID: 18860 RVA: 0x001CD1CD File Offset: 0x001CB3CD
	[PublicizedFrom(EAccessModifier.Internal)]
	public BaseQuestCriteria AddCriteria(BaseQuestCriteria criteria)
	{
		if (criteria != null)
		{
			this.Criteria.Add(criteria);
		}
		return criteria;
	}

	// Token: 0x060049AD RID: 18861 RVA: 0x001CD1E0 File Offset: 0x001CB3E0
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

	// Token: 0x060049AE RID: 18862 RVA: 0x001CD230 File Offset: 0x001CB430
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

	// Token: 0x060049AF RID: 18863 RVA: 0x001CD280 File Offset: 0x001CB480
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

	// Token: 0x060049B0 RID: 18864 RVA: 0x001CD2C4 File Offset: 0x001CB4C4
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

	// Token: 0x060049B1 RID: 18865 RVA: 0x001CD358 File Offset: 0x001CB558
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

	// Token: 0x060049B2 RID: 18866 RVA: 0x001CDC04 File Offset: 0x001CBE04
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

	// Token: 0x060049B3 RID: 18867 RVA: 0x001CDC88 File Offset: 0x001CBE88
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

	// Token: 0x04003A29 RID: 14889
	public static Dictionary<string, QuestClass> s_Quests = new CaseInsensitiveStringDictionary<QuestClass>();

	// Token: 0x04003A2A RID: 14890
	public static string PropCategory = "category";

	// Token: 0x04003A2B RID: 14891
	public static string PropCategoryKey = "category_key";

	// Token: 0x04003A2C RID: 14892
	public static string PropGroupName = "group_name";

	// Token: 0x04003A2D RID: 14893
	public static string PropGroupNameKey = "group_name_key";

	// Token: 0x04003A2E RID: 14894
	public static string PropName = "name";

	// Token: 0x04003A2F RID: 14895
	public static string PropNameKey = "name_key";

	// Token: 0x04003A30 RID: 14896
	public static string PropSubtitle = "subtitle";

	// Token: 0x04003A31 RID: 14897
	public static string PropSubtitleKey = "subtitle_key";

	// Token: 0x04003A32 RID: 14898
	public static string PropDescription = "description";

	// Token: 0x04003A33 RID: 14899
	public static string PropDescriptionKey = "description_key";

	// Token: 0x04003A34 RID: 14900
	public static string PropOffer = "offer";

	// Token: 0x04003A35 RID: 14901
	public static string PropOfferKey = "offer_key";

	// Token: 0x04003A36 RID: 14902
	public static string PropIcon = "icon";

	// Token: 0x04003A37 RID: 14903
	public static string PropRepeatable = "repeatable";

	// Token: 0x04003A38 RID: 14904
	public static string PropShareable = "shareable";

	// Token: 0x04003A39 RID: 14905
	public static string PropDifficulty = "difficulty";

	// Token: 0x04003A3A RID: 14906
	public static string PropCompletionType = "completiontype";

	// Token: 0x04003A3B RID: 14907
	public static string PropCurrentVersion = "currentversion";

	// Token: 0x04003A3C RID: 14908
	public static string PropStatementText = "statement_text";

	// Token: 0x04003A3D RID: 14909
	public static string PropResponseText = "response_text";

	// Token: 0x04003A3E RID: 14910
	public static string PropCompleteText = "completion_text";

	// Token: 0x04003A3F RID: 14911
	public static string PropStatementKey = "statement_key";

	// Token: 0x04003A40 RID: 14912
	public static string PropResponseKey = "response_key";

	// Token: 0x04003A41 RID: 14913
	public static string PropCompleteKey = "completion_key";

	// Token: 0x04003A42 RID: 14914
	public static string PropVariations = "variations";

	// Token: 0x04003A43 RID: 14915
	public static string PropQuestFaction = "quest_faction";

	// Token: 0x04003A44 RID: 14916
	public static string PropDifficultyTier = "difficulty_tier";

	// Token: 0x04003A45 RID: 14917
	public static string PropLoginRallyReset = "login_rally_reset";

	// Token: 0x04003A46 RID: 14918
	public static string PropUniqueKey = "unique_key";

	// Token: 0x04003A47 RID: 14919
	public static string PropReturnToQuestGiver = "return_to_quest_giver";

	// Token: 0x04003A48 RID: 14920
	public static string PropQuestType = "quest_type";

	// Token: 0x04003A49 RID: 14921
	public static string PropAddsToTierComplete = "add_to_tier_complete";

	// Token: 0x04003A4A RID: 14922
	public static string PropRewardChoicesCount = "reward_choices_count";

	// Token: 0x04003A4B RID: 14923
	public static string PropExtraTags = "extra_tags";

	// Token: 0x04003A4C RID: 14924
	public static string PropFilterTags = "filter_tags";

	// Token: 0x04003A4D RID: 14925
	public static string PropQuestStage = "quest_stage";

	// Token: 0x04003A4E RID: 14926
	public static string PropQuestHints = "quest_hints";

	// Token: 0x04003A4F RID: 14927
	public static string PropQuestHintRequirement = "quest_hint_requirement";

	// Token: 0x04003A50 RID: 14928
	public static string PropQuestGameStageMod = "gamestage_mod";

	// Token: 0x04003A51 RID: 14929
	public static string PropQuestGameStageBonus = "gamestage_bonus";

	// Token: 0x04003A52 RID: 14930
	public static string PropSpawnMultiplier = "spawn_multiplier";

	// Token: 0x04003A53 RID: 14931
	public static string PropResetTraderQuests = "reset_trader_quests";

	// Token: 0x04003A54 RID: 14932
	public static string PropSingleQuest = "single_quest";

	// Token: 0x04003A55 RID: 14933
	public static string PropAlwaysAllow = "always_allow";

	// Token: 0x04003A56 RID: 14934
	public static string PropAllowRemove = "allow_remove";

	// Token: 0x04003A57 RID: 14935
	public static string PropMaxQuestCount = "max_quest_count";

	// Token: 0x04003A6F RID: 14959
	public List<string> QuestHints;

	// Token: 0x04003A70 RID: 14960
	public global::SandboxOptions.SandboxOptions QuestHintRequirement = global::SandboxOptions.SandboxOptions.Max;

	// Token: 0x04003A71 RID: 14961
	public int RewardChoicesCount = 2;

	// Token: 0x04003A72 RID: 14962
	public FastTags<TagGroup.Global> ExtraTags = FastTags<TagGroup.Global>.none;

	// Token: 0x04003A73 RID: 14963
	public FastTags<TagGroup.Global> FilterTags = FastTags<TagGroup.Global>.none;

	// Token: 0x04003A74 RID: 14964
	public float GameStageMod;

	// Token: 0x04003A75 RID: 14965
	public float GameStageBonus;

	// Token: 0x04003A76 RID: 14966
	public float SpawnMultiplier = 1f;

	// Token: 0x04003A77 RID: 14967
	public bool ResetTraderQuests;

	// Token: 0x04003A78 RID: 14968
	public bool SingleQuest;

	// Token: 0x04003A79 RID: 14969
	public bool AlwaysAllow;

	// Token: 0x04003A7A RID: 14970
	public bool AllowRemove = true;

	// Token: 0x04003A7B RID: 14971
	public int MaxQuestCount;

	// Token: 0x04003A7C RID: 14972
	public QuestClass.CompletionTypes CompletionType;

	// Token: 0x04003A7D RID: 14973
	public List<BaseQuestCriteria> Criteria = new List<BaseQuestCriteria>();

	// Token: 0x04003A7E RID: 14974
	public List<BaseQuestAction> Actions = new List<BaseQuestAction>();

	// Token: 0x04003A7F RID: 14975
	public List<QuestEvent> Events = new List<QuestEvent>();

	// Token: 0x04003A80 RID: 14976
	public List<BaseRequirement> Requirements = new List<BaseRequirement>();

	// Token: 0x04003A81 RID: 14977
	public List<BaseObjective> Objectives = new List<BaseObjective>();

	// Token: 0x04003A82 RID: 14978
	public List<BaseReward> Rewards = new List<BaseReward>();

	// Token: 0x04003A83 RID: 14979
	public Dictionary<string, string> Variables = new Dictionary<string, string>();

	// Token: 0x04003A84 RID: 14980
	public DynamicProperties Properties = new DynamicProperties();

	// Token: 0x020009B0 RID: 2480
	public enum CompletionTypes
	{
		// Token: 0x04003A86 RID: 14982
		AutoComplete,
		// Token: 0x04003A87 RID: 14983
		TurnIn
	}
}
