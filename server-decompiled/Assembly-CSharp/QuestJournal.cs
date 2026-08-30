using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

// Token: 0x020009D6 RID: 2518
public class QuestJournal
{
	// Token: 0x170007F4 RID: 2036
	// (get) Token: 0x06004ADE RID: 19166 RVA: 0x001D16C0 File Offset: 0x001CF8C0
	// (set) Token: 0x06004ADF RID: 19167 RVA: 0x001D16C8 File Offset: 0x001CF8C8
	public Quest TrackedQuest
	{
		get
		{
			return this.trackedQuest;
		}
		set
		{
			LocalPlayerUI uiforPlayer = LocalPlayerUI.GetUIForPlayer(this.OwnerPlayer);
			if (uiforPlayer != null && uiforPlayer.xui != null && uiforPlayer.xui.QuestTracker != null)
			{
				Quest quest = uiforPlayer.xui.QuestTracker.TrackedQuest;
				if (quest != null)
				{
					quest.Tracked = false;
				}
			}
			this.trackedQuest = value;
			if (uiforPlayer != null && uiforPlayer.xui != null && uiforPlayer.xui.QuestTracker != null)
			{
				if (value != null)
				{
					this.trackedQuest.Tracked = true;
					uiforPlayer.xui.Recipes.TrackedRecipe = null;
					uiforPlayer.xui.QuestTracker.TrackedChallenge = null;
				}
				uiforPlayer.xui.QuestTracker.TrackedQuest = this.trackedQuest;
			}
		}
	}

	// Token: 0x06004AE0 RID: 19168 RVA: 0x001D1794 File Offset: 0x001CF994
	public void AddTraderPOI(Vector2 pos, int factionID)
	{
		if (!this.TraderPOIs.Contains(pos))
		{
			this.TraderPOIs.Add(pos);
		}
		if (!this.TradersByFaction.ContainsKey(factionID))
		{
			this.TradersByFaction.Add(factionID, new List<Vector2>());
		}
		if (!this.TradersByFaction[factionID].Contains(pos))
		{
			this.TradersByFaction[factionID].Add(pos);
		}
	}

	// Token: 0x06004AE1 RID: 19169 RVA: 0x001D1800 File Offset: 0x001CFA00
	public bool HasTraderPOI(Vector2 pos)
	{
		return this.TraderPOIs.Contains(pos);
	}

	// Token: 0x06004AE2 RID: 19170 RVA: 0x001D180E File Offset: 0x001CFA0E
	public List<Vector2> GetTraderList(int factionID)
	{
		if (this.TradersByFaction.ContainsKey(factionID))
		{
			return this.TradersByFaction[factionID];
		}
		return null;
	}

	// Token: 0x06004AE3 RID: 19171 RVA: 0x001D182C File Offset: 0x001CFA2C
	public Quest GetSharedQuest(int questCode)
	{
		for (int i = 0; i < this.quests.Count; i++)
		{
			if (this.quests[i].QuestCode == questCode && this.quests[i].CurrentState == Quest.QuestState.InProgress)
			{
				return this.quests[i];
			}
		}
		return null;
	}

	// Token: 0x06004AE4 RID: 19172 RVA: 0x001D1888 File Offset: 0x001CFA88
	public void RemoveSharedQuestByOwner(int questCode)
	{
		for (int i = 0; i < this.quests.Count; i++)
		{
			if (this.quests[i].QuestCode == questCode && this.quests[i].CurrentState == Quest.QuestState.InProgress && !this.quests[i].RallyMarkerActivated)
			{
				Quest quest = this.quests[i];
				this.RemoveQuest(quest);
				GameManager.ShowTooltip(this.OwnerPlayer, "Shared quest {0} has been removed by quest owner.", quest.QuestClass.Name, null, null, false, false, 0f);
				return;
			}
		}
	}

	// Token: 0x06004AE5 RID: 19173 RVA: 0x001D1924 File Offset: 0x001CFB24
	public void RemoveSharedQuestForOwner(int entityID)
	{
		for (int i = 0; i < this.quests.Count; i++)
		{
			if (this.quests[i].SharedOwnerID == entityID && this.quests[i].CurrentState == Quest.QuestState.InProgress && !this.quests[i].RallyMarkerActivated)
			{
				Quest quest = this.quests[i];
				this.RemoveQuest(quest);
				GameManager.ShowTooltip(this.OwnerPlayer, "Shared quest {0} has been removed.", quest.QuestClass.Name, null, null, false, false, 0f);
			}
		}
	}

	// Token: 0x06004AE6 RID: 19174 RVA: 0x001D19C0 File Offset: 0x001CFBC0
	public void FailAllSharedQuests()
	{
		for (int i = 0; i < this.quests.Count; i++)
		{
			if (this.quests[i].SharedOwnerID != -1 && this.quests[i].CurrentState == Quest.QuestState.InProgress && this.quests[i].CurrentPhase < this.quests[i].QuestClass.HighestPhase)
			{
				this.quests[i].CloseQuest(Quest.QuestState.Failed, null);
			}
		}
	}

	// Token: 0x06004AE7 RID: 19175 RVA: 0x001D1A48 File Offset: 0x001CFC48
	public void FailAllActivatedQuests()
	{
		for (int i = 0; i < this.quests.Count; i++)
		{
			if (this.quests[i].RallyMarkerActivated && this.quests[i].CurrentState == Quest.QuestState.InProgress && this.quests[i].CurrentPhase < this.quests[i].QuestClass.HighestPhase)
			{
				this.quests[i].CloseQuest(Quest.QuestState.Failed, null);
			}
		}
	}

	// Token: 0x06004AE8 RID: 19176 RVA: 0x001D1AD0 File Offset: 0x001CFCD0
	public List<Vector2> GetUsedPOIs(Vector2 traderPOI, int tier)
	{
		QuestTraderData traderData = this.GetTraderData(traderPOI);
		if (traderData != null)
		{
			return traderData.GetTierPOIs(tier);
		}
		return null;
	}

	// Token: 0x06004AE9 RID: 19177 RVA: 0x001D1AF4 File Offset: 0x001CFCF4
	public void RemoveAllSharedQuests()
	{
		for (int i = this.quests.Count - 1; i >= 0; i--)
		{
			if (this.quests[i].SharedOwnerID != -1 && this.quests[i].CurrentState == Quest.QuestState.InProgress && this.quests[i].CurrentPhase < this.quests[i].QuestClass.HighestPhase)
			{
				Quest quest = this.quests[i];
				if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
				{
					SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageSharedQuest>().Setup(quest.QuestCode, quest.SharedOwnerID, this.OwnerPlayer.entityId, false), false, quest.SharedOwnerID, -1, -1, null, 192, false);
				}
				else
				{
					SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(NetPackageManager.GetPackage<NetPackageSharedQuest>().Setup(quest.QuestCode, quest.SharedOwnerID, this.OwnerPlayer.entityId, false), false);
				}
				this.RemoveQuest(quest);
				GameManager.ShowTooltip(this.OwnerPlayer, "Shared quest {0} has been removed.", quest.QuestClass.Name, null, null, false, false, 0f);
			}
		}
		for (int j = 0; j < this.sharedQuestEntries.Count; j++)
		{
			this.sharedQuestEntries[j].Quest.RemoveMapObject();
		}
		this.sharedQuestEntries.Clear();
		this.OwnerPlayer.TriggerSharedQuestRemovedEvent(null);
	}

	// Token: 0x06004AEA RID: 19178 RVA: 0x001D1C74 File Offset: 0x001CFE74
	public void RemovePlayerFromSharedWiths(EntityPlayer player)
	{
		for (int i = 0; i < this.quests.Count; i++)
		{
			this.quests[i].RemoveSharedWith(player);
		}
	}

	// Token: 0x06004AEB RID: 19179 RVA: 0x001D1CAC File Offset: 0x001CFEAC
	public Quest FindSharedQuest(int questCode)
	{
		for (int i = 0; i < this.quests.Count; i++)
		{
			if (this.quests[i].QuestCode == questCode)
			{
				return this.quests[i];
			}
		}
		return null;
	}

	// Token: 0x06004AEC RID: 19180 RVA: 0x001D1CF4 File Offset: 0x001CFEF4
	public Quest FindQuest(string questName, int questFaction = -1)
	{
		for (int i = 0; i < this.quests.Count; i++)
		{
			if (this.quests[i].ID == questName && (questFaction == -1 || (int)this.quests[i].QuestFaction == questFaction))
			{
				return this.quests[i];
			}
		}
		return null;
	}

	// Token: 0x06004AED RID: 19181 RVA: 0x001D1D58 File Offset: 0x001CFF58
	public bool FindCompletedQuest(string questName, int questFaction = -1)
	{
		for (int i = 0; i < this.quests.Count; i++)
		{
			Quest quest = this.quests[i];
			if (quest.ID == questName && (questFaction == -1 || (int)quest.QuestFaction == questFaction) && quest.CurrentState == Quest.QuestState.Completed)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06004AEE RID: 19182 RVA: 0x001D1DB0 File Offset: 0x001CFFB0
	public Quest FindNonSharedQuest(string questName)
	{
		for (int i = 0; i < this.quests.Count; i++)
		{
			if (this.quests[i].ID == questName && this.quests[i].SharedOwnerID == -1)
			{
				return this.quests[i];
			}
		}
		return null;
	}

	// Token: 0x06004AEF RID: 19183 RVA: 0x001D1E10 File Offset: 0x001D0010
	public Quest FindNonSharedQuest(int questCode)
	{
		for (int i = 0; i < this.quests.Count; i++)
		{
			if (this.quests[i].QuestCode == questCode && this.quests[i].SharedOwnerID == -1)
			{
				return this.quests[i];
			}
		}
		return null;
	}

	// Token: 0x06004AF0 RID: 19184 RVA: 0x001D1E6C File Offset: 0x001D006C
	public Quest FindLatestNonSharedQuest(string questName)
	{
		Quest quest = null;
		for (int i = 0; i < this.quests.Count; i++)
		{
			if (this.quests[i].ID == questName && (quest == null || this.quests[i].Active || this.quests[i].FinishTime > quest.FinishTime) && this.quests[i].SharedOwnerID == -1)
			{
				quest = this.quests[i];
			}
		}
		return quest;
	}

	// Token: 0x06004AF1 RID: 19185 RVA: 0x001D1EFC File Offset: 0x001D00FC
	public Quest FindActiveQuest()
	{
		for (int i = 0; i < this.quests.Count; i++)
		{
			if (this.quests[i].CurrentState == Quest.QuestState.InProgress || this.quests[i].CurrentState == Quest.QuestState.ReadyForTurnIn)
			{
				return this.quests[i];
			}
		}
		return null;
	}

	// Token: 0x06004AF2 RID: 19186 RVA: 0x001D1F58 File Offset: 0x001D0158
	public bool QuestIsActive(Quest quest)
	{
		bool flag = false;
		for (int i = 0; i < this.quests.Count; i++)
		{
			Quest quest2 = this.quests[i];
			if (quest == quest2)
			{
				flag = quest2.CurrentState == Quest.QuestState.InProgress || quest2.CurrentState == Quest.QuestState.ReadyForTurnIn;
				break;
			}
		}
		return flag;
	}

	// Token: 0x06004AF3 RID: 19187 RVA: 0x001D1FA8 File Offset: 0x001D01A8
	public Quest FindActiveQuest(int questCode)
	{
		for (int i = 0; i < this.quests.Count; i++)
		{
			if (this.quests[i].QuestCode == questCode && (this.quests[i].CurrentState == Quest.QuestState.InProgress || this.quests[i].CurrentState == Quest.QuestState.ReadyForTurnIn))
			{
				return this.quests[i];
			}
		}
		return null;
	}

	// Token: 0x06004AF4 RID: 19188 RVA: 0x001D2018 File Offset: 0x001D0218
	public Quest FindActiveQuest(string questName, int questFaction = -1)
	{
		for (int i = 0; i < this.quests.Count; i++)
		{
			if (this.quests[i].ID == questName && (this.quests[i].CurrentState == Quest.QuestState.InProgress || this.quests[i].CurrentState == Quest.QuestState.ReadyForTurnIn) && (questFaction == -1 || (int)this.quests[i].QuestFaction == questFaction))
			{
				return this.quests[i];
			}
		}
		return null;
	}

	// Token: 0x06004AF5 RID: 19189 RVA: 0x001D20A4 File Offset: 0x001D02A4
	public Quest FindActiveOrCompleteQuest(string questName, int questFaction = -1)
	{
		for (int i = 0; i < this.quests.Count; i++)
		{
			if (this.quests[i].ID == questName && this.quests[i].CurrentState != Quest.QuestState.Failed && (questFaction == -1 || (int)this.quests[i].QuestFaction == questFaction))
			{
				return this.quests[i];
			}
		}
		return null;
	}

	// Token: 0x06004AF6 RID: 19190 RVA: 0x001D211C File Offset: 0x001D031C
	public Quest FindActiveQuestByGiver(int questGiverID)
	{
		for (int i = 0; i < this.quests.Count; i++)
		{
			if (this.quests[i].CheckIsQuestGiver(questGiverID) && this.quests[i].CurrentState == Quest.QuestState.InProgress && this.quests[i].SharedOwnerID == -1)
			{
				return this.quests[i];
			}
		}
		return null;
	}

	// Token: 0x06004AF7 RID: 19191 RVA: 0x001D218C File Offset: 0x001D038C
	public Quest FindActiveQuestByGiver(int questGiverID, string questType)
	{
		for (int i = 0; i < this.quests.Count; i++)
		{
			if (this.quests[i].CheckIsQuestGiver(questGiverID) && (this.quests[i].CurrentState == Quest.QuestState.InProgress || this.quests[i].CurrentState == Quest.QuestState.ReadyForTurnIn) && this.quests[i].SharedOwnerID == -1 && this.quests[i].QuestClass.QuestType == questType)
			{
				return this.quests[i];
			}
		}
		return null;
	}

	// Token: 0x06004AF8 RID: 19192 RVA: 0x001D2230 File Offset: 0x001D0430
	public Quest FindReadyForTurnInQuestByGiver(int questGiverID)
	{
		for (int i = 0; i < this.quests.Count; i++)
		{
			if (this.quests[i].CheckIsQuestGiver(questGiverID) && (this.quests[i].CurrentState == Quest.QuestState.InProgress || this.quests[i].CurrentState == Quest.QuestState.ReadyForTurnIn) && this.quests[i].RallyMarkerActivated)
			{
				return this.quests[i];
			}
		}
		return null;
	}

	// Token: 0x06004AF9 RID: 19193 RVA: 0x001D22B0 File Offset: 0x001D04B0
	public bool HasActiveQuestByQuestCode(int questCode)
	{
		for (int i = 0; i < this.quests.Count; i++)
		{
			if (this.quests[i].QuestCode == questCode && this.quests[i].CurrentState == Quest.QuestState.InProgress)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06004AFA RID: 19194 RVA: 0x001D2300 File Offset: 0x001D0500
	public void AddQuest(Quest q, bool notify = true)
	{
		q.OwnerJournal = this;
		if (this.FindActiveQuest(q.QuestCode) == null)
		{
			q.StartQuest(true, notify);
			this.quests.Add(q);
			this.OwnerPlayer.TriggerQuestAddedEvent(q);
			foreach (KeyValuePair<Quest.PositionDataTypes, Vector3> keyValuePair in q.PositionData)
			{
				GameManager.Instance.persistentPlayers.GetPlayerDataFromEntityID(this.OwnerPlayer.entityId).AddQuestPosition(q.QuestCode, keyValuePair.Key, World.worldToBlockPos(keyValuePair.Value));
			}
		}
	}

	// Token: 0x06004AFB RID: 19195 RVA: 0x001D23C4 File Offset: 0x001D05C4
	[PublicizedFrom(EAccessModifier.Internal)]
	public void RefreshTracked()
	{
		for (int i = 0; i < this.quests.Count; i++)
		{
			if (this.quests[i].Tracked)
			{
				this.TrackedQuest = this.quests[i];
				return;
			}
		}
	}

	// Token: 0x06004AFC RID: 19196 RVA: 0x001D2410 File Offset: 0x001D0610
	public void CompleteQuest(Quest q)
	{
		q.CurrentState = Quest.QuestState.Completed;
		q.FinishTime = GameManager.Instance.World.worldTime;
		this.OwnerPlayer.TriggerQuestChangedEvent(q);
		if (q.QuestClass.AddsToTierComplete && q.AddsProgression)
		{
			this.AddQuestFactionPoint(q.QuestFaction, (int)q.QuestClass.DifficultyTier);
			if (q.PositionData.ContainsKey(Quest.PositionDataTypes.TraderPosition) && q.PositionData.ContainsKey(Quest.PositionDataTypes.POIPosition) && SingletonMonoBehaviour<ConnectionManager>.Instance.IsClient)
			{
				Vector3 vector = q.PositionData[Quest.PositionDataTypes.TraderPosition];
				Vector3 vector2 = q.PositionData[Quest.PositionDataTypes.POIPosition];
				SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(NetPackageManager.GetPackage<NetPackageNPCQuestList>().Setup(this.OwnerPlayer.entityId, new Vector2(vector.x, vector.z), (int)q.QuestClass.DifficultyTier, new Vector2(vector2.x, vector2.z)), false);
			}
			this.ResetAddToProgression();
		}
		if (this.ActiveQuest == q)
		{
			this.ActiveQuest = null;
			this.RefreshRallyMarkerPositions();
		}
		GameManager.Instance.persistentPlayers.GetPlayerDataFromEntityID(this.OwnerPlayer.entityId).RemovePositionsForQuest(q.QuestCode);
	}

	// Token: 0x06004AFD RID: 19197 RVA: 0x001D254C File Offset: 0x001D074C
	public int AddTraderData(Vector2 pos)
	{
		for (int i = 0; i < this.TraderData.Count; i++)
		{
			if (this.TraderData[i].TraderPOI == pos)
			{
				return i;
			}
		}
		this.TraderData.Add(new QuestTraderData(pos)
		{
			Owner = this
		});
		return this.TraderData.Count - 1;
	}

	// Token: 0x06004AFE RID: 19198 RVA: 0x001D25B0 File Offset: 0x001D07B0
	public void AddPOIToTraderData(int tier, Vector3 questGiver, Vector3 poiPosition)
	{
		int num = this.AddTraderData(new Vector2(questGiver.x, questGiver.z));
		this.TraderData[num].AddPOI(tier, new Vector2(poiPosition.x, poiPosition.z));
	}

	// Token: 0x06004AFF RID: 19199 RVA: 0x001D25F8 File Offset: 0x001D07F8
	public void AddPOIToTraderData(int tier, Vector2 questGiver, Vector2 poiPosition)
	{
		int num = this.AddTraderData(questGiver);
		this.TraderData[num].AddPOI(tier, poiPosition);
	}

	// Token: 0x06004B00 RID: 19200 RVA: 0x001D2620 File Offset: 0x001D0820
	public void ClearTraderDataTier(int tier, Vector2 questGiver)
	{
		QuestTraderData traderData = this.GetTraderData(questGiver);
		if (traderData != null)
		{
			traderData.ClearTier(tier);
		}
	}

	// Token: 0x06004B01 RID: 19201 RVA: 0x001D2640 File Offset: 0x001D0840
	public QuestTraderData GetTraderData(Vector2 questGiver)
	{
		for (int i = 0; i < this.TraderData.Count; i++)
		{
			if (this.TraderData[i].TraderPOI == questGiver)
			{
				return this.TraderData[i];
			}
		}
		return null;
	}

	// Token: 0x06004B02 RID: 19202 RVA: 0x001D268C File Offset: 0x001D088C
	public void Clear()
	{
		this.TrackedQuest = null;
		this.ForceRemoveAllQuests();
		this.ActiveQuest = null;
		this.TraderData.Clear();
		this.sharedQuestEntries.Clear();
		this.TradersByFaction.Clear();
		this.QuestFactionPoints.Clear();
		this.GlobalFactionPoints = 0;
		this.TraderPOIs.Clear();
	}

	// Token: 0x06004B03 RID: 19203 RVA: 0x001D26EC File Offset: 0x001D08EC
	public void FailedQuest(Quest q)
	{
		q.CurrentState = Quest.QuestState.Failed;
		q.FinishTime = GameManager.Instance.World.worldTime;
		this.OwnerPlayer.TriggerQuestChangedEvent(q);
		GameManager.Instance.persistentPlayers.GetPlayerDataFromEntityID(this.OwnerPlayer.entityId).RemovePositionsForQuest(q.QuestCode);
	}

	// Token: 0x06004B04 RID: 19204 RVA: 0x001D2748 File Offset: 0x001D0948
	public void RemoveQuest(Quest q)
	{
		if (this.FindActiveQuest(q.QuestCode) != null)
		{
			q.CloseQuest(Quest.QuestState.Failed, null);
			this.quests.Remove(q);
			if (q.SharedOwnerID != -1)
			{
				if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
				{
					SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageSharedQuest>().Setup(q.QuestCode, q.SharedOwnerID, this.OwnerPlayer.entityId, false), false, q.SharedOwnerID, -1, -1, null, 192, false);
				}
				else
				{
					SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(NetPackageManager.GetPackage<NetPackageSharedQuest>().Setup(q.QuestCode, q.SharedOwnerID, this.OwnerPlayer.entityId, false), false);
				}
			}
			this.OwnerPlayer.TriggerQuestRemovedEvent(q);
			this.HandlePartyRemoveQuest(q);
			GameManager.Instance.persistentPlayers.GetPlayerDataFromEntityID(this.OwnerPlayer.entityId).RemovePositionsForQuest(q.QuestCode);
		}
	}

	// Token: 0x06004B05 RID: 19205 RVA: 0x001D283C File Offset: 0x001D0A3C
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandlePartyRemoveQuest(Quest q)
	{
		if (this.OwnerPlayer.IsInParty() && q.SharedOwnerID == -1)
		{
			if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
			{
				for (int i = 0; i < this.OwnerPlayer.Party.MemberList.Count; i++)
				{
					EntityPlayer entityPlayer = this.OwnerPlayer.Party.MemberList[i];
					if (entityPlayer is EntityPlayerLocal)
					{
						entityPlayer.QuestJournal.RemoveSharedQuestByOwner(q.QuestCode);
						entityPlayer.QuestJournal.RemoveSharedQuestEntry(q.QuestCode);
					}
					else
					{
						SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageSharedQuest>().Setup(q.QuestCode, this.OwnerPlayer.entityId), false, entityPlayer.entityId, -1, -1, null, 192, false);
					}
				}
				return;
			}
			SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(NetPackageManager.GetPackage<NetPackageSharedQuest>().Setup(q.QuestCode, this.OwnerPlayer.entityId), false);
		}
	}

	// Token: 0x06004B06 RID: 19206 RVA: 0x001D2944 File Offset: 0x001D0B44
	public void ForceRemoveAllQuests()
	{
		for (int i = this.quests.Count - 1; i >= 0; i--)
		{
			this.ForceRemoveQuest(this.quests[i]);
		}
	}

	// Token: 0x06004B07 RID: 19207 RVA: 0x001D297C File Offset: 0x001D0B7C
	public void ForceRemoveQuest(Quest quest)
	{
		this.quests.Remove(quest);
		quest.UnhookQuest();
		this.OwnerPlayer.TriggerQuestRemovedEvent(quest);
		GameManager.Instance.persistentPlayers.GetPlayerDataFromEntityID(this.OwnerPlayer.entityId).RemovePositionsForQuest(quest.QuestCode);
	}

	// Token: 0x06004B08 RID: 19208 RVA: 0x001D29D0 File Offset: 0x001D0BD0
	public void ForceRemoveQuest(string questID)
	{
		for (int i = 0; i < this.quests.Count; i++)
		{
			if (this.quests[i].ID.EqualsCaseInsensitive(questID))
			{
				Quest quest = this.quests[i];
				this.quests.Remove(quest);
				quest.UnhookQuest();
				this.OwnerPlayer.TriggerQuestRemovedEvent(quest);
				GameManager.Instance.persistentPlayers.GetPlayerDataFromEntityID(this.OwnerPlayer.entityId).RemovePositionsForQuest(quest.QuestCode);
				return;
			}
		}
	}

	// Token: 0x06004B09 RID: 19209 RVA: 0x001D2A5E File Offset: 0x001D0C5E
	public void RefreshQuest(Quest q)
	{
		this.OwnerPlayer.TriggerQuestChangedEvent(q);
	}

	// Token: 0x06004B0A RID: 19210 RVA: 0x001D2A6C File Offset: 0x001D0C6C
	public void UnHookQuests()
	{
		for (int i = 0; i < this.quests.Count; i++)
		{
			this.quests[i].UnhookQuest();
		}
	}

	// Token: 0x06004B0B RID: 19211 RVA: 0x001D2AA0 File Offset: 0x001D0CA0
	public void Read(PooledBinaryReader _br)
	{
		this.quests.Clear();
		byte b = _br.ReadByte();
		if (b >= 2)
		{
			int num = (int)_br.ReadByte();
			this.TraderPOIs.Clear();
			for (int i = 0; i < num; i++)
			{
				this.TraderPOIs.Add(StreamUtils.ReadVector2(_br));
			}
		}
		if (b > 2)
		{
			int num2 = (int)_br.ReadByte();
			this.TradersByFaction.Clear();
			for (int j = 0; j < num2; j++)
			{
				int num3 = _br.ReadInt32();
				List<Vector2> list = new List<Vector2>();
				int num4 = _br.ReadInt32();
				for (int k = 0; k < num4; k++)
				{
					list.Add(StreamUtils.ReadVector2(_br));
				}
				this.TradersByFaction.Add(num3, list);
			}
		}
		int num5 = (int)_br.ReadInt16();
		for (int l = 0; l < num5; l++)
		{
			PooledBinaryReader.StreamReadSizeMarker streamReadSizeMarker = default(PooledBinaryReader.StreamReadSizeMarker);
			if (b >= 5)
			{
				streamReadSizeMarker = _br.ReadSizeMarker(PooledBinaryWriter.EMarkerSize.UInt16);
			}
			string text = _br.ReadString();
			try
			{
				byte b2 = _br.ReadByte();
				if (QuestClass.GetQuest(text) == null)
				{
					Log.Error("Loading player quests: Quest with ID " + text + " not found, ignoring");
					uint num6;
					_br.ValidateSizeMarker(ref streamReadSizeMarker, out num6, true);
				}
				else
				{
					Quest quest = QuestClass.CreateQuest(text);
					Quest quest2 = quest.Clone();
					quest2.CurrentQuestVersion = b2;
					quest2.Read(_br);
					if (quest.CurrentQuestVersion != b2)
					{
						quest2 = quest.Clone();
					}
					quest2.OwnerJournal = this;
					this.quests.Add(quest2);
					if (quest2.CurrentState == Quest.QuestState.Completed && quest2.QuestClass.AddsToTierComplete && quest2.AddsProgression)
					{
						this.AddQuestFactionPoint(quest2.QuestFaction, (int)quest2.QuestClass.DifficultyTier);
					}
				}
			}
			catch (Exception ex)
			{
				Log.Error(ex.ToString());
			}
			finally
			{
				uint num6;
				if (b >= 5 && !_br.ValidateSizeMarker(ref streamReadSizeMarker, out num6, true))
				{
					Log.Error("Loading player quests: Error loading quest " + text + ", ignoring");
				}
			}
		}
		if (b > 3)
		{
			this.TraderData.Clear();
			num5 = (int)_br.ReadByte();
			for (int m = 0; m < num5; m++)
			{
				QuestTraderData questTraderData = new QuestTraderData
				{
					Owner = this
				};
				questTraderData.Read(_br, b);
				this.TraderData.Add(questTraderData);
			}
		}
	}

	// Token: 0x06004B0C RID: 19212 RVA: 0x001D2CF4 File Offset: 0x001D0EF4
	public void AddQuestFactionPoint(byte id, int difficultyTier)
	{
		if (difficultyTier == 0)
		{
			return;
		}
		this.GlobalFactionPoints += difficultyTier;
		if (!this.QuestFactionPoints.ContainsKey(id))
		{
			this.QuestFactionPoints.Add(id, difficultyTier);
			return;
		}
		Dictionary<byte, int> questFactionPoints = this.QuestFactionPoints;
		questFactionPoints[id] += difficultyTier;
	}

	// Token: 0x06004B0D RID: 19213 RVA: 0x001D2D48 File Offset: 0x001D0F48
	public int GetQuestFactionPoints(byte id)
	{
		if (this.QuestFactionPoints.ContainsKey(id))
		{
			return this.QuestFactionPoints[id];
		}
		return 0;
	}

	// Token: 0x06004B0E RID: 19214 RVA: 0x001D2D68 File Offset: 0x001D0F68
	public int GetQuestFactionMax(byte id, int tier)
	{
		int num = 0;
		for (int i = 1; i <= tier; i++)
		{
			num += i * Quest.QuestsPerTier;
		}
		return num;
	}

	// Token: 0x06004B0F RID: 19215 RVA: 0x001D2D90 File Offset: 0x001D0F90
	public void ResetAddToProgression()
	{
		int num = 0;
		int worldDay = GameManager.Instance.World.WorldDay;
		for (int i = 0; i < this.quests.Count; i++)
		{
			if (this.quests[i].QuestProgressDay == worldDay)
			{
				num++;
			}
		}
		int questsPerDay = Quest.QuestsPerDay;
		this.CanAddProgression = questsPerDay == -1 || num < questsPerDay;
	}

	// Token: 0x06004B10 RID: 19216 RVA: 0x001D2DF4 File Offset: 0x001D0FF4
	public void HandleQuestCompleteToday(Quest q)
	{
		int num = 0;
		int worldDay = GameManager.Instance.World.WorldDay;
		for (int i = 0; i < this.quests.Count; i++)
		{
			if (this.quests[i].QuestProgressDay == worldDay)
			{
				num++;
			}
		}
		int questsPerDay = Quest.QuestsPerDay;
		if (questsPerDay == -1 || num < questsPerDay)
		{
			q.QuestProgressDay = worldDay;
		}
		else
		{
			q.QuestProgressDay = -1;
		}
		this.ResetAddToProgression();
		if (!this.CanAddProgression)
		{
			this.OwnerPlayer.Buffs.AddBuff("buffShowQuestLimitReached", -1, true, false, -1f);
		}
	}

	// Token: 0x06004B11 RID: 19217 RVA: 0x001D2E8C File Offset: 0x001D108C
	public int GetCurrentFactionTier(byte id, int offset = 0, bool allowExtraTierOverMax = false)
	{
		int num = this.GetQuestFactionPoints(id) + offset;
		int i = 1;
		if (Quest.QuestsPerTier == 0)
		{
			return Quest.MaxQuestTier + (allowExtraTierOverMax ? 1 : 0);
		}
		while (i < 100)
		{
			num -= i * Quest.QuestsPerTier;
			if (num < 0)
			{
				return Math.Min(i, Quest.MaxQuestTier + (allowExtraTierOverMax ? 1 : 0));
			}
			i++;
		}
		return 1;
	}

	// Token: 0x06004B12 RID: 19218 RVA: 0x001D2EE8 File Offset: 0x001D10E8
	public void Write(PooledBinaryWriter _bw)
	{
		_bw.Write(5);
		int count = this.TraderPOIs.Count;
		_bw.Write((byte)count);
		for (int i = 0; i < count; i++)
		{
			StreamUtils.Write(_bw, this.TraderPOIs[i]);
		}
		_bw.Write((byte)this.TradersByFaction.Count);
		foreach (int num in this.TradersByFaction.Keys)
		{
			_bw.Write(num);
			_bw.Write(this.TradersByFaction[num].Count);
			for (int j = 0; j < this.TradersByFaction[num].Count; j++)
			{
				StreamUtils.Write(_bw, this.TradersByFaction[num][j]);
			}
		}
		int num2 = this.quests.Count;
		_bw.Write((ushort)num2);
		for (int k = 0; k < num2; k++)
		{
			PooledBinaryWriter.StreamWriteSizeMarker streamWriteSizeMarker = _bw.ReserveSizeMarker(PooledBinaryWriter.EMarkerSize.UInt16);
			this.quests[k].Write(_bw);
			_bw.FinalizeSizeMarker(ref streamWriteSizeMarker);
		}
		num2 = this.TraderData.Count;
		_bw.Write((byte)num2);
		for (int l = 0; l < num2; l++)
		{
			this.TraderData[l].Write(_bw);
		}
	}

	// Token: 0x06004B13 RID: 19219 RVA: 0x001D3060 File Offset: 0x001D1260
	public QuestJournal Clone()
	{
		QuestJournal questJournal = new QuestJournal();
		questJournal.OwnerPlayer = this.OwnerPlayer;
		for (int i = 0; i < this.TraderPOIs.Count; i++)
		{
			questJournal.TraderPOIs.Add(this.TraderPOIs[i]);
		}
		foreach (int num in this.TradersByFaction.Keys)
		{
			questJournal.TradersByFaction.Add(num, this.TradersByFaction[num]);
		}
		for (int j = 0; j < this.quests.Count; j++)
		{
			questJournal.quests.Add(this.quests[j].Clone());
		}
		for (int k = 0; k < this.sharedQuestEntries.Count; k++)
		{
			questJournal.sharedQuestEntries.Add(this.sharedQuestEntries[k].Clone());
		}
		foreach (KeyValuePair<byte, int> keyValuePair in this.QuestFactionPoints)
		{
			questJournal.AddQuestFactionPoint(keyValuePair.Key, keyValuePair.Value);
		}
		for (int l = 0; l < this.TraderData.Count; l++)
		{
			questJournal.TraderData.Add(this.TraderData[l]);
		}
		return questJournal;
	}

	// Token: 0x06004B14 RID: 19220 RVA: 0x001D31FC File Offset: 0x001D13FC
	[PublicizedFrom(EAccessModifier.Internal)]
	public void StartQuests()
	{
		if (!GameManager.Instance.World.IsEditor() && !GameUtils.IsWorldEditor() && !GameUtils.IsPlaytesting())
		{
			this.OwnerPlayer.challengeJournal.StartChallenges(this.OwnerPlayer);
		}
		for (int i = 0; i < this.quests.Count; i++)
		{
			if (this.quests[i].RallyMarkerActivated)
			{
				if (this.quests[i].SharedOwnerID != -1 && this.quests[i].CurrentPhase < this.quests[i].QuestClass.HighestPhase && this.quests[i].CurrentState == Quest.QuestState.InProgress)
				{
					this.quests[i].CloseQuest(Quest.QuestState.Failed, null);
				}
				else
				{
					this.quests[i].ResetToRallyPointObjective();
					this.quests[i].StartQuest(false, true);
				}
			}
			else
			{
				this.quests[i].StartQuest(false, true);
			}
		}
	}

	// Token: 0x06004B15 RID: 19221 RVA: 0x001D3310 File Offset: 0x001D1510
	public bool AddSharedQuestEntry(NetPackageSharedQuest.SharedQuestData sqd)
	{
		for (int i = 0; i < this.sharedQuestEntries.Count; i++)
		{
			SharedQuestEntry sharedQuestEntry = this.sharedQuestEntries[i];
			if (sharedQuestEntry.QuestCode == sqd.questCode && sharedQuestEntry.QuestID == sqd.questID && sharedQuestEntry.SharedByPlayerID == sqd.sharedByEntityID && sharedQuestEntry.QuestGiverID == sqd.questGiverID)
			{
				if (PartyQuests.AutoAccept)
				{
					Log.Out(string.Format("Ignoring received quest, already have a sharedquest with the questCode={0}, id={1}, sharer={2}, giver={3}:", new object[] { sqd.questCode, sqd.questID, sqd.sharedByEntityID, sqd.questGiverID }));
					for (int j = 0; j < this.sharedQuestEntries.Count; j++)
					{
						SharedQuestEntry sharedQuestEntry2 = this.sharedQuestEntries[j];
						Log.Out(string.Format("  {0}.: id={1}, code={2}, name={3}, POI={4}, state={5}, owner={6}", new object[]
						{
							j,
							sharedQuestEntry2.QuestID,
							sharedQuestEntry2.QuestCode,
							sharedQuestEntry2.Quest.QuestClass.Name,
							sharedQuestEntry2.Quest.GetParsedText("{poi.name}"),
							sharedQuestEntry2.Quest.CurrentState,
							sharedQuestEntry2.SharedByPlayerID
						}));
					}
					Log.Out("Quests:");
					for (int k = 0; k < this.quests.Count; k++)
					{
						Quest quest = this.quests[k];
						Log.Out(string.Format("  {0}.: id={1}, code={2}, name={3}, POI={4}, state={5}, owner={6}", new object[]
						{
							k,
							quest.ID,
							quest.QuestCode,
							quest.QuestClass.Name,
							quest.GetParsedText("{poi.name}"),
							quest.CurrentState,
							quest.SharedOwnerID
						}));
					}
				}
				return false;
			}
		}
		SharedQuestEntry sharedQuestEntry3 = new SharedQuestEntry(sqd.questCode, sqd.questID, sqd.poiName, sqd.position, sqd.size, sqd.returnPos, sqd.sharedByEntityID, sqd.questGiverID, this, null);
		Log.Out(string.Format("Received shared quest: questCode={0}, id={1}, POI {2}", sqd.questCode, sqd.questID, sqd.poiName));
		this.sharedQuestEntries.Add(sharedQuestEntry3);
		this.OwnerPlayer.TriggerSharedQuestAddedEvent(sharedQuestEntry3);
		return true;
	}

	// Token: 0x06004B16 RID: 19222 RVA: 0x001D35B3 File Offset: 0x001D17B3
	public void RemoveSharedQuestEntry(SharedQuestEntry entry)
	{
		if (this.sharedQuestEntries.Contains(entry))
		{
			this.sharedQuestEntries.Remove(entry);
			entry.Quest.RemoveMapObject();
			this.OwnerPlayer.TriggerSharedQuestRemovedEvent(entry);
		}
	}

	// Token: 0x06004B17 RID: 19223 RVA: 0x001D35E8 File Offset: 0x001D17E8
	public void RemoveSharedQuestEntry(int questCode)
	{
		for (int i = this.sharedQuestEntries.Count - 1; i >= 0; i--)
		{
			if (this.sharedQuestEntries[i].Quest.QuestCode == questCode)
			{
				SharedQuestEntry sharedQuestEntry = this.sharedQuestEntries[i];
				sharedQuestEntry.Quest.RemoveMapObject();
				this.sharedQuestEntries.RemoveAt(i);
				this.OwnerPlayer.TriggerSharedQuestRemovedEvent(sharedQuestEntry);
			}
		}
	}

	// Token: 0x06004B18 RID: 19224 RVA: 0x001D3658 File Offset: 0x001D1858
	public void RemoveSharedQuestEntryByOwner(int entityID)
	{
		for (int i = this.sharedQuestEntries.Count - 1; i >= 0; i--)
		{
			if (this.sharedQuestEntries[i].Quest.SharedOwnerID == entityID)
			{
				SharedQuestEntry sharedQuestEntry = this.sharedQuestEntries[i];
				sharedQuestEntry.Quest.RemoveMapObject();
				this.sharedQuestEntries.RemoveAt(i);
				this.OwnerPlayer.TriggerSharedQuestRemovedEvent(sharedQuestEntry);
			}
		}
	}

	// Token: 0x06004B19 RID: 19225 RVA: 0x001D36C8 File Offset: 0x001D18C8
	public void ClearSharedQuestMarkers()
	{
		for (int i = 0; i < this.sharedQuestEntries.Count; i++)
		{
			this.sharedQuestEntries[i].Quest.RemoveMapObject();
		}
	}

	// Token: 0x06004B1A RID: 19226 RVA: 0x001D3704 File Offset: 0x001D1904
	public Quest GetNextCompletedQuest(Quest lastQuest, int entityId)
	{
		for (int i = 0; i < this.quests.Count; i++)
		{
			if (lastQuest != null)
			{
				if (this.quests[i] == lastQuest)
				{
					lastQuest = null;
				}
			}
			else
			{
				Quest quest = this.quests[i];
				if (quest.CurrentState == Quest.QuestState.ReadyForTurnIn && (!quest.QuestClass.ReturnToQuestGiver || quest.QuestGiverID == -1 || quest.CheckIsQuestGiver(entityId)))
				{
					return quest;
				}
			}
		}
		return null;
	}

	// Token: 0x06004B1B RID: 19227 RVA: 0x001D3778 File Offset: 0x001D1978
	public void SetActivePositionData(Quest.PositionDataTypes dataType, Vector3i position)
	{
		for (int i = 0; i < this.quests.Count; i++)
		{
			if (this.quests[i].Active && this.quests[i].RallyMarkerActivated)
			{
				this.quests[i].SetObjectivePosition(dataType, position);
			}
		}
	}

	// Token: 0x06004B1C RID: 19228 RVA: 0x001D37D4 File Offset: 0x001D19D4
	public void HandleRestorePowerReceived(Vector3 prefabPos, List<Vector3i> activateList)
	{
		int num = 0;
		while (num < this.quests.Count && (!this.quests[num].Active || !this.quests[num].RallyMarkerActivated || !this.quests[num].HandleActivateListReceived(prefabPos, activateList)))
		{
			num++;
		}
	}

	// Token: 0x06004B1D RID: 19229 RVA: 0x001D3834 File Offset: 0x001D1A34
	public void HandleRallyMarkerActivation(int questCode, Vector3 prefabPos, bool rallyMarkerActivated, QuestEventManager.POILockoutReasonTypes lockoutReason, ulong extraData = 0UL)
	{
		int num = 0;
		while (num < this.quests.Count && (this.quests[num].QuestCode != questCode || !this.quests[num].Active || !this.quests[num].HandleRallyMarkerActivation(prefabPos, rallyMarkerActivated, lockoutReason, extraData)))
		{
			num++;
		}
	}

	// Token: 0x06004B1E RID: 19230 RVA: 0x001D3898 File Offset: 0x001D1A98
	public bool CheckRallyMarkerActivation()
	{
		bool flag = false;
		bool flag2 = false;
		for (int i = 0; i < this.quests.Count; i++)
		{
			if (this.quests[i].Active)
			{
				foreach (BaseObjective baseObjective in this.quests[i].Objectives)
				{
					ObjectiveRallyPoint objectiveRallyPoint = baseObjective as ObjectiveRallyPoint;
					if (objectiveRallyPoint != null)
					{
						flag2 = true;
						flag = objectiveRallyPoint.IsActivated();
						break;
					}
				}
			}
		}
		return !flag2 || flag;
	}

	// Token: 0x06004B1F RID: 19231 RVA: 0x001D3938 File Offset: 0x001D1B38
	public Quest HasQuestAtRallyPosition(Vector3 rallyPos, bool mustBeHost = true)
	{
		for (int i = 0; i < this.quests.Count; i++)
		{
			Quest quest = this.quests[i];
			Vector3 vector;
			if (quest.Active && (quest.SharedOwnerID == -1 || !mustBeHost) && !quest.RallyMarkerActivated && quest.GetPositionData(out vector, Quest.PositionDataTypes.Activate) && vector.x == rallyPos.x && vector.z == rallyPos.z)
			{
				return quest;
			}
		}
		return null;
	}

	// Token: 0x06004B20 RID: 19232 RVA: 0x001D39B0 File Offset: 0x001D1BB0
	public void RefreshRallyMarkerPositions()
	{
		for (int i = 0; i < this.quests.Count; i++)
		{
			Quest quest = this.quests[i];
			if (quest.Active && !quest.RallyMarkerActivated)
			{
				quest.RefreshRallyMarker();
			}
		}
	}

	// Token: 0x06004B21 RID: 19233 RVA: 0x001D39F8 File Offset: 0x001D1BF8
	public bool HasCraftingQuest()
	{
		for (int i = 0; i < this.quests.Count; i++)
		{
			Quest quest = this.quests[i];
			if (quest.Active && quest.QuestTags.Test_AnySet(QuestEventManager.craftingTag))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06004B22 RID: 19234 RVA: 0x001D3A48 File Offset: 0x001D1C48
	public List<string> GetQuestRecipes()
	{
		this.questRecipeList.Clear();
		for (int i = 0; i < this.quests.Count; i++)
		{
			Quest quest = this.quests[i];
			if (quest.Active)
			{
				for (int j = 0; j < quest.Objectives.Count; j++)
				{
					if ((quest.Objectives[j].Phase == 0 || quest.Objectives[j].Phase == quest.CurrentPhase) && quest.Objectives[j] is ObjectiveCraft && !quest.Objectives[j].Complete)
					{
						this.questRecipeList.Add((quest.Objectives[j] as ObjectiveCraft).ID);
					}
				}
			}
		}
		return this.questRecipeList;
	}

	// Token: 0x06004B23 RID: 19235 RVA: 0x001D3B28 File Offset: 0x001D1D28
	public T GetObjectiveForQuest<T>(int _questCode) where T : BaseObjective
	{
		Quest quest = this.FindActiveQuest(_questCode);
		if (quest != null)
		{
			for (int i = 0; i < quest.Objectives.Count; i++)
			{
				if (quest.CurrentPhase == quest.Objectives[i].Phase && quest.Objectives[i] is T)
				{
					return quest.Objectives[i] as T;
				}
			}
		}
		return default(T);
	}

	// Token: 0x06004B24 RID: 19236 RVA: 0x001D3BA4 File Offset: 0x001D1DA4
	public int GetRewardedSkillPoints()
	{
		int num = 0;
		for (int i = 0; i < this.quests.Count; i++)
		{
			Quest quest = this.quests[i];
			if (quest.CurrentState == Quest.QuestState.Completed)
			{
				for (int j = 0; j < quest.QuestClass.Rewards.Count; j++)
				{
					RewardSkillPoints rewardSkillPoints = quest.QuestClass.Rewards[j] as RewardSkillPoints;
					if (rewardSkillPoints != null)
					{
						num += StringParsers.ParseSInt32(rewardSkillPoints.Value, 0, -1, NumberStyles.Integer);
					}
				}
			}
		}
		return num;
	}

	// Token: 0x06004B25 RID: 19237 RVA: 0x001D3C2C File Offset: 0x001D1E2C
	public void Update(int worldDay)
	{
		if (this.previousDay != worldDay)
		{
			this.previousDay = worldDay;
			this.ResetAddToProgression();
			if (this.CanAddProgression)
			{
				this.OwnerPlayer.Buffs.RemoveBuff("buffShowQuestLimitReached", -1, true);
				return;
			}
			this.OwnerPlayer.Buffs.AddBuff("buffShowQuestLimitReached", -1, true, false, -1f);
		}
	}

	// Token: 0x04003AE9 RID: 15081
	public EntityPlayerLocal OwnerPlayer;

	// Token: 0x04003AEA RID: 15082
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cCurrentSaveVersion = 5;

	// Token: 0x04003AEB RID: 15083
	public List<Quest> quests = new List<Quest>();

	// Token: 0x04003AEC RID: 15084
	public List<SharedQuestEntry> sharedQuestEntries = new List<SharedQuestEntry>();

	// Token: 0x04003AED RID: 15085
	public List<Vector2> TraderPOIs = new List<Vector2>();

	// Token: 0x04003AEE RID: 15086
	public List<QuestTraderData> TraderData = new List<QuestTraderData>();

	// Token: 0x04003AEF RID: 15087
	public Dictionary<int, List<Vector2>> TradersByFaction = new Dictionary<int, List<Vector2>>();

	// Token: 0x04003AF0 RID: 15088
	public static bool IntroQuestEnabled = true;

	// Token: 0x04003AF1 RID: 15089
	public static bool BuriedQuestsEnabled = true;

	// Token: 0x04003AF2 RID: 15090
	public static bool POIQuestsEnabled = true;

	// Token: 0x04003AF3 RID: 15091
	[PublicizedFrom(EAccessModifier.Private)]
	public Quest trackedQuest;

	// Token: 0x04003AF4 RID: 15092
	public Quest ActiveQuest;

	// Token: 0x04003AF5 RID: 15093
	public Dictionary<byte, int> QuestFactionPoints = new Dictionary<byte, int>();

	// Token: 0x04003AF6 RID: 15094
	public int GlobalFactionPoints;

	// Token: 0x04003AF7 RID: 15095
	public bool CanAddProgression;

	// Token: 0x04003AF8 RID: 15096
	[PublicizedFrom(EAccessModifier.Private)]
	public List<string> questRecipeList = new List<string>();

	// Token: 0x04003AF9 RID: 15097
	[PublicizedFrom(EAccessModifier.Private)]
	public int previousDay = -1;
}
