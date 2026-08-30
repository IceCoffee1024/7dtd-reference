using System;
using System.Collections.Generic;
using Platform;
using Quests.Requirements;
using SandboxOptions;

// Token: 0x020009C0 RID: 2496
public class QuestClass
{
	// Token: 0x170007E8 RID: 2024
	// (get) Token: 0x060049F8 RID: 18936 RVA: 0x001CF26B File Offset: 0x001CD46B
	// (set) Token: 0x060049F9 RID: 18937 RVA: 0x001CF273 File Offset: 0x001CD473
	public string ID
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x170007E9 RID: 2025
	// (get) Token: 0x060049FA RID: 18938 RVA: 0x001CF27C File Offset: 0x001CD47C
	// (set) Token: 0x060049FB RID: 18939 RVA: 0x001CF284 File Offset: 0x001CD484
	public string Name { get; set; }

	// Token: 0x170007EA RID: 2026
	// (get) Token: 0x060049FC RID: 18940 RVA: 0x001CF28D File Offset: 0x001CD48D
	// (set) Token: 0x060049FD RID: 18941 RVA: 0x001CF295 File Offset: 0x001CD495
	public string GroupName { get; set; }

	// Token: 0x170007EB RID: 2027
	// (get) Token: 0x060049FE RID: 18942 RVA: 0x001CF29E File Offset: 0x001CD49E
	// (set) Token: 0x060049FF RID: 18943 RVA: 0x001CF2A6 File Offset: 0x001CD4A6
	public string SubTitle { get; set; }

	// Token: 0x170007EC RID: 2028
	// (get) Token: 0x06004A00 RID: 18944 RVA: 0x001CF2AF File Offset: 0x001CD4AF
	// (set) Token: 0x06004A01 RID: 18945 RVA: 0x001CF2B7 File Offset: 0x001CD4B7
	public string Description { get; set; }

	// Token: 0x170007ED RID: 2029
	// (get) Token: 0x06004A02 RID: 18946 RVA: 0x001CF2C0 File Offset: 0x001CD4C0
	// (set) Token: 0x06004A03 RID: 18947 RVA: 0x001CF2C8 File Offset: 0x001CD4C8
	public string Offer { get; set; }

	// Token: 0x170007EE RID: 2030
	// (get) Token: 0x06004A04 RID: 18948 RVA: 0x001CF2D1 File Offset: 0x001CD4D1
	// (set) Token: 0x06004A05 RID: 18949 RVA: 0x001CF2D9 File Offset: 0x001CD4D9
	public string Difficulty { get; set; }

	// Token: 0x170007EF RID: 2031
	// (get) Token: 0x06004A06 RID: 18950 RVA: 0x001CF2E2 File Offset: 0x001CD4E2
	// (set) Token: 0x06004A07 RID: 18951 RVA: 0x001CF2EA File Offset: 0x001CD4EA
	public string Icon { get; set; }

	// Token: 0x170007F0 RID: 2032
	// (get) Token: 0x06004A08 RID: 18952 RVA: 0x001CF2F3 File Offset: 0x001CD4F3
	// (set) Token: 0x06004A09 RID: 18953 RVA: 0x001CF2FB File Offset: 0x001CD4FB
	public bool Repeatable { get; set; }

	// Token: 0x170007F1 RID: 2033
	// (get) Token: 0x06004A0A RID: 18954 RVA: 0x001CF304 File Offset: 0x001CD504
	// (set) Token: 0x06004A0B RID: 18955 RVA: 0x001CF30C File Offset: 0x001CD50C
	public bool Shareable { get; set; }

	// Token: 0x170007F2 RID: 2034
	// (get) Token: 0x06004A0C RID: 18956 RVA: 0x001CF315 File Offset: 0x001CD515
	// (set) Token: 0x06004A0D RID: 18957 RVA: 0x001CF31D File Offset: 0x001CD51D
	public string Category { get; set; }

	// Token: 0x170007F3 RID: 2035
	// (get) Token: 0x06004A0E RID: 18958 RVA: 0x001CF326 File Offset: 0x001CD526
	// (set) Token: 0x06004A0F RID: 18959 RVA: 0x001CF32E File Offset: 0x001CD52E
	public string StatementText { get; set; }

	// Token: 0x170007F4 RID: 2036
	// (get) Token: 0x06004A10 RID: 18960 RVA: 0x001CF337 File Offset: 0x001CD537
	// (set) Token: 0x06004A11 RID: 18961 RVA: 0x001CF33F File Offset: 0x001CD53F
	public string ResponseText { get; set; }

	// Token: 0x170007F5 RID: 2037
	// (get) Token: 0x06004A12 RID: 18962 RVA: 0x001CF348 File Offset: 0x001CD548
	// (set) Token: 0x06004A13 RID: 18963 RVA: 0x001CF350 File Offset: 0x001CD550
	public string CompleteText { get; set; }

	// Token: 0x170007F6 RID: 2038
	// (get) Token: 0x06004A14 RID: 18964 RVA: 0x001CF359 File Offset: 0x001CD559
	// (set) Token: 0x06004A15 RID: 18965 RVA: 0x001CF361 File Offset: 0x001CD561
	public byte CurrentVersion { get; set; }

	// Token: 0x170007F7 RID: 2039
	// (get) Token: 0x06004A16 RID: 18966 RVA: 0x001CF36A File Offset: 0x001CD56A
	// (set) Token: 0x06004A17 RID: 18967 RVA: 0x001CF372 File Offset: 0x001CD572
	public byte HighestPhase { get; set; }

	// Token: 0x170007F8 RID: 2040
	// (get) Token: 0x06004A18 RID: 18968 RVA: 0x001CF37B File Offset: 0x001CD57B
	// (set) Token: 0x06004A19 RID: 18969 RVA: 0x001CF383 File Offset: 0x001CD583
	public byte QuestFaction { get; set; }

	// Token: 0x170007F9 RID: 2041
	// (get) Token: 0x06004A1A RID: 18970 RVA: 0x001CF38C File Offset: 0x001CD58C
	// (set) Token: 0x06004A1B RID: 18971 RVA: 0x001CF394 File Offset: 0x001CD594
	public byte DifficultyTier { get; set; }

	// Token: 0x170007FA RID: 2042
	// (get) Token: 0x06004A1C RID: 18972 RVA: 0x001CF39D File Offset: 0x001CD59D
	// (set) Token: 0x06004A1D RID: 18973 RVA: 0x001CF3A5 File Offset: 0x001CD5A5
	public bool LoginRallyReset { get; set; }

	// Token: 0x170007FB RID: 2043
	// (get) Token: 0x06004A1E RID: 18974 RVA: 0x001CF3AE File Offset: 0x001CD5AE
	// (set) Token: 0x06004A1F RID: 18975 RVA: 0x001CF3B6 File Offset: 0x001CD5B6
	public bool ReturnToQuestGiver { get; set; }

	// Token: 0x170007FC RID: 2044
	// (get) Token: 0x06004A20 RID: 18976 RVA: 0x001CF3BF File Offset: 0x001CD5BF
	// (set) Token: 0x06004A21 RID: 18977 RVA: 0x001CF3C7 File Offset: 0x001CD5C7
	public string UniqueKey { get; set; }

	// Token: 0x170007FD RID: 2045
	// (get) Token: 0x06004A22 RID: 18978 RVA: 0x001CF3D0 File Offset: 0x001CD5D0
	// (set) Token: 0x06004A23 RID: 18979 RVA: 0x001CF3D8 File Offset: 0x001CD5D8
	public string QuestType { get; set; }

	// Token: 0x170007FE RID: 2046
	// (get) Token: 0x06004A24 RID: 18980 RVA: 0x001CF3E1 File Offset: 0x001CD5E1
	// (set) Token: 0x06004A25 RID: 18981 RVA: 0x001CF3E9 File Offset: 0x001CD5E9
	public bool AddsToTierComplete { get; set; }

	// Token: 0x06004A26 RID: 18982 RVA: 0x001CF3F4 File Offset: 0x001CD5F4
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

	// Token: 0x06004A27 RID: 18983 RVA: 0x001CF42C File Offset: 0x001CD62C
	[PublicizedFrom(EAccessModifier.Private)]
	public QuestClass(string id)
	{
		this.ID = id;
		this.Difficulty = "veryeasy";
		this.HighestPhase = 1;
		this.AddsToTierComplete = true;
	}

	// Token: 0x06004A28 RID: 18984 RVA: 0x001CF4F1 File Offset: 0x001CD6F1
	[PublicizedFrom(EAccessModifier.Internal)]
	public static QuestClass GetQuest(string questID)
	{
		if (!QuestClass.s_Quests.ContainsKey(questID))
		{
			return null;
		}
		return QuestClass.s_Quests[questID];
	}

	// Token: 0x06004A29 RID: 18985 RVA: 0x001CF510 File Offset: 0x001CD710
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

	// Token: 0x06004A2A RID: 18986 RVA: 0x001CF774 File Offset: 0x001CD974
	public static Quest CreateQuest(string ID)
	{
		QuestClass quest = QuestClass.GetQuest(ID);
		if (quest != null)
		{
			return quest.CreateQuest();
		}
		return null;
	}

	// Token: 0x06004A2B RID: 18987 RVA: 0x001CF794 File Offset: 0x001CD994
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

	// Token: 0x06004A2C RID: 18988 RVA: 0x001CF920 File Offset: 0x001CDB20
	public void ResetObjectives()
	{
		for (int i = 0; i < this.Objectives.Count; i++)
		{
			this.Objectives[i].ResetObjective();
		}
	}

	// Token: 0x06004A2D RID: 18989 RVA: 0x001CF954 File Offset: 0x001CDB54
	[PublicizedFrom(EAccessModifier.Internal)]
	public BaseQuestAction AddAction(BaseQuestAction action)
	{
		if (action != null)
		{
			this.Actions.Add(action);
		}
		return action;
	}

	// Token: 0x06004A2E RID: 18990 RVA: 0x001CF966 File Offset: 0x001CDB66
	public QuestEvent AddEvent(QuestEvent questEvent)
	{
		if (questEvent != null)
		{
			this.Events.Add(questEvent);
		}
		return questEvent;
	}

	// Token: 0x06004A2F RID: 18991 RVA: 0x001CF978 File Offset: 0x001CDB78
	[PublicizedFrom(EAccessModifier.Internal)]
	public BaseRequirement AddRequirement(BaseRequirement requirement)
	{
		if (requirement != null)
		{
			this.Requirements.Add(requirement);
		}
		return requirement;
	}

	// Token: 0x06004A30 RID: 18992 RVA: 0x001CF98A File Offset: 0x001CDB8A
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

	// Token: 0x06004A31 RID: 18993 RVA: 0x001CF9A3 File Offset: 0x001CDBA3
	[PublicizedFrom(EAccessModifier.Internal)]
	public BaseReward AddReward(BaseReward reward)
	{
		if (reward != null)
		{
			this.Rewards.Add(reward);
		}
		return reward;
	}

	// Token: 0x06004A32 RID: 18994 RVA: 0x001CF9B5 File Offset: 0x001CDBB5
	[PublicizedFrom(EAccessModifier.Internal)]
	public BaseQuestCriteria AddCriteria(BaseQuestCriteria criteria)
	{
		if (criteria != null)
		{
			this.Criteria.Add(criteria);
		}
		return criteria;
	}

	// Token: 0x06004A33 RID: 18995 RVA: 0x001CF9C8 File Offset: 0x001CDBC8
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

	// Token: 0x06004A34 RID: 18996 RVA: 0x001CFA18 File Offset: 0x001CDC18
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

	// Token: 0x06004A35 RID: 18997 RVA: 0x001CFA68 File Offset: 0x001CDC68
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

	// Token: 0x06004A36 RID: 18998 RVA: 0x001CFAAC File Offset: 0x001CDCAC
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

	// Token: 0x06004A37 RID: 18999 RVA: 0x001CFB40 File Offset: 0x001CDD40
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

	// Token: 0x06004A38 RID: 19000 RVA: 0x001D03EC File Offset: 0x001CE5EC
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

	// Token: 0x06004A39 RID: 19001 RVA: 0x001D0470 File Offset: 0x001CE670
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

	// Token: 0x04003ABA RID: 15034
	public static Dictionary<string, QuestClass> s_Quests = new CaseInsensitiveStringDictionary<QuestClass>();

	// Token: 0x04003ABB RID: 15035
	public static string PropCategory = "category";

	// Token: 0x04003ABC RID: 15036
	public static string PropCategoryKey = "category_key";

	// Token: 0x04003ABD RID: 15037
	public static string PropGroupName = "group_name";

	// Token: 0x04003ABE RID: 15038
	public static string PropGroupNameKey = "group_name_key";

	// Token: 0x04003ABF RID: 15039
	public static string PropName = "name";

	// Token: 0x04003AC0 RID: 15040
	public static string PropNameKey = "name_key";

	// Token: 0x04003AC1 RID: 15041
	public static string PropSubtitle = "subtitle";

	// Token: 0x04003AC2 RID: 15042
	public static string PropSubtitleKey = "subtitle_key";

	// Token: 0x04003AC3 RID: 15043
	public static string PropDescription = "description";

	// Token: 0x04003AC4 RID: 15044
	public static string PropDescriptionKey = "description_key";

	// Token: 0x04003AC5 RID: 15045
	public static string PropOffer = "offer";

	// Token: 0x04003AC6 RID: 15046
	public static string PropOfferKey = "offer_key";

	// Token: 0x04003AC7 RID: 15047
	public static string PropIcon = "icon";

	// Token: 0x04003AC8 RID: 15048
	public static string PropRepeatable = "repeatable";

	// Token: 0x04003AC9 RID: 15049
	public static string PropShareable = "shareable";

	// Token: 0x04003ACA RID: 15050
	public static string PropDifficulty = "difficulty";

	// Token: 0x04003ACB RID: 15051
	public static string PropCompletionType = "completiontype";

	// Token: 0x04003ACC RID: 15052
	public static string PropCurrentVersion = "currentversion";

	// Token: 0x04003ACD RID: 15053
	public static string PropStatementText = "statement_text";

	// Token: 0x04003ACE RID: 15054
	public static string PropResponseText = "response_text";

	// Token: 0x04003ACF RID: 15055
	public static string PropCompleteText = "completion_text";

	// Token: 0x04003AD0 RID: 15056
	public static string PropStatementKey = "statement_key";

	// Token: 0x04003AD1 RID: 15057
	public static string PropResponseKey = "response_key";

	// Token: 0x04003AD2 RID: 15058
	public static string PropCompleteKey = "completion_key";

	// Token: 0x04003AD3 RID: 15059
	public static string PropVariations = "variations";

	// Token: 0x04003AD4 RID: 15060
	public static string PropQuestFaction = "quest_faction";

	// Token: 0x04003AD5 RID: 15061
	public static string PropDifficultyTier = "difficulty_tier";

	// Token: 0x04003AD6 RID: 15062
	public static string PropLoginRallyReset = "login_rally_reset";

	// Token: 0x04003AD7 RID: 15063
	public static string PropUniqueKey = "unique_key";

	// Token: 0x04003AD8 RID: 15064
	public static string PropReturnToQuestGiver = "return_to_quest_giver";

	// Token: 0x04003AD9 RID: 15065
	public static string PropQuestType = "quest_type";

	// Token: 0x04003ADA RID: 15066
	public static string PropAddsToTierComplete = "add_to_tier_complete";

	// Token: 0x04003ADB RID: 15067
	public static string PropRewardChoicesCount = "reward_choices_count";

	// Token: 0x04003ADC RID: 15068
	public static string PropExtraTags = "extra_tags";

	// Token: 0x04003ADD RID: 15069
	public static string PropFilterTags = "filter_tags";

	// Token: 0x04003ADE RID: 15070
	public static string PropQuestStage = "quest_stage";

	// Token: 0x04003ADF RID: 15071
	public static string PropQuestHints = "quest_hints";

	// Token: 0x04003AE0 RID: 15072
	public static string PropQuestHintRequirement = "quest_hint_requirement";

	// Token: 0x04003AE1 RID: 15073
	public static string PropQuestGameStageMod = "gamestage_mod";

	// Token: 0x04003AE2 RID: 15074
	public static string PropQuestGameStageBonus = "gamestage_bonus";

	// Token: 0x04003AE3 RID: 15075
	public static string PropSpawnMultiplier = "spawn_multiplier";

	// Token: 0x04003AE4 RID: 15076
	public static string PropResetTraderQuests = "reset_trader_quests";

	// Token: 0x04003AE5 RID: 15077
	public static string PropSingleQuest = "single_quest";

	// Token: 0x04003AE6 RID: 15078
	public static string PropAlwaysAllow = "always_allow";

	// Token: 0x04003AE7 RID: 15079
	public static string PropAllowRemove = "allow_remove";

	// Token: 0x04003AE8 RID: 15080
	public static string PropMaxQuestCount = "max_quest_count";

	// Token: 0x04003B00 RID: 15104
	public List<string> QuestHints;

	// Token: 0x04003B01 RID: 15105
	public global::SandboxOptions.SandboxOptions QuestHintRequirement = global::SandboxOptions.SandboxOptions.Max;

	// Token: 0x04003B02 RID: 15106
	public int RewardChoicesCount = 2;

	// Token: 0x04003B03 RID: 15107
	public FastTags<TagGroup.Global> ExtraTags = FastTags<TagGroup.Global>.none;

	// Token: 0x04003B04 RID: 15108
	public FastTags<TagGroup.Global> FilterTags = FastTags<TagGroup.Global>.none;

	// Token: 0x04003B05 RID: 15109
	public float GameStageMod;

	// Token: 0x04003B06 RID: 15110
	public float GameStageBonus;

	// Token: 0x04003B07 RID: 15111
	public float SpawnMultiplier = 1f;

	// Token: 0x04003B08 RID: 15112
	public bool ResetTraderQuests;

	// Token: 0x04003B09 RID: 15113
	public bool SingleQuest;

	// Token: 0x04003B0A RID: 15114
	public bool AlwaysAllow;

	// Token: 0x04003B0B RID: 15115
	public bool AllowRemove = true;

	// Token: 0x04003B0C RID: 15116
	public int MaxQuestCount;

	// Token: 0x04003B0D RID: 15117
	public QuestClass.CompletionTypes CompletionType;

	// Token: 0x04003B0E RID: 15118
	public List<BaseQuestCriteria> Criteria = new List<BaseQuestCriteria>();

	// Token: 0x04003B0F RID: 15119
	public List<BaseQuestAction> Actions = new List<BaseQuestAction>();

	// Token: 0x04003B10 RID: 15120
	public List<QuestEvent> Events = new List<QuestEvent>();

	// Token: 0x04003B11 RID: 15121
	public List<BaseRequirement> Requirements = new List<BaseRequirement>();

	// Token: 0x04003B12 RID: 15122
	public List<BaseObjective> Objectives = new List<BaseObjective>();

	// Token: 0x04003B13 RID: 15123
	public List<BaseReward> Rewards = new List<BaseReward>();

	// Token: 0x04003B14 RID: 15124
	public Dictionary<string, string> Variables = new Dictionary<string, string>();

	// Token: 0x04003B15 RID: 15125
	public DynamicProperties Properties = new DynamicProperties();

	// Token: 0x020009C1 RID: 2497
	public enum CompletionTypes
	{
		// Token: 0x04003B17 RID: 15127
		AutoComplete,
		// Token: 0x04003B18 RID: 15128
		TurnIn
	}
}
