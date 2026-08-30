using System;
using System.Collections.Generic;
using System.Globalization;
using Audio;
using GameEvent.GameEventHelpers;
using SandboxOptions;
using UnityEngine;

// Token: 0x02000528 RID: 1320
public class GameEventManager
{
	// Token: 0x170004A5 RID: 1189
	// (get) Token: 0x06002B5D RID: 11101 RVA: 0x00110F0C File Offset: 0x0010F10C
	public static GameEventManager Current
	{
		get
		{
			if (GameEventManager.instance == null)
			{
				GameEventManager.instance = new GameEventManager();
			}
			return GameEventManager.instance;
		}
	}

	// Token: 0x06002B5E RID: 11102 RVA: 0x00110F24 File Offset: 0x0010F124
	[PublicizedFrom(EAccessModifier.Private)]
	public GameEventManager()
	{
		this.Random = GameRandomManager.Instance.CreateGameRandom();
	}

	// Token: 0x170004A6 RID: 1190
	// (get) Token: 0x06002B5F RID: 11103 RVA: 0x00110FDE File Offset: 0x0010F1DE
	public static bool HasInstance
	{
		get
		{
			return GameEventManager.instance != null;
		}
	}

	// Token: 0x06002B60 RID: 11104 RVA: 0x00110FE8 File Offset: 0x0010F1E8
	public void AddSequence(GameEventActionSequence action)
	{
		if (!GameEventManager.GameEventSequences.ContainsKey(action.Name))
		{
			GameEventManager.GameEventSequences.Add(action.Name, action);
		}
	}

	// Token: 0x06002B61 RID: 11105 RVA: 0x0011100D File Offset: 0x0010F20D
	public void Cleanup()
	{
		this.ClearActions();
		this.GameEventFlags.Clear();
		this.BossGroups.Clear();
		this.CurrentBossGroup = null;
		this.HomerunManager.Cleanup();
	}

	// Token: 0x06002B62 RID: 11106 RVA: 0x0011103D File Offset: 0x0010F23D
	public void ClearActions()
	{
		this.ActionSequenceUpdates.Clear();
		GameEventManager.GameEventSequences.Clear();
		this.CategoryList.Clear();
		this.spawnEntries.Clear();
		this.blockEntries.Clear();
	}

	// Token: 0x170004A7 RID: 1191
	// (get) Token: 0x06002B63 RID: 11107 RVA: 0x00111075 File Offset: 0x0010F275
	// (set) Token: 0x06002B64 RID: 11108 RVA: 0x00111080 File Offset: 0x0010F280
	public BossGroup CurrentBossGroup
	{
		get
		{
			return this.currentBossGroup;
		}
		set
		{
			if (this.currentBossGroup == value)
			{
				return;
			}
			if (this.currentBossGroup != null)
			{
				this.currentBossGroup.IsCurrent = false;
				this.currentBossGroup.RemoveNavObjects();
				if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsClient)
				{
					this.currentBossGroup.MinionEntities = null;
				}
			}
			this.currentBossGroup = value;
			if (this.currentBossGroup != null)
			{
				this.currentBossGroup.IsCurrent = true;
				this.currentBossGroup.RequestStatRefresh();
				this.currentBossGroup.AddNavObjects();
			}
		}
	}

	// Token: 0x170004A8 RID: 1192
	// (get) Token: 0x06002B65 RID: 11109 RVA: 0x001110FF File Offset: 0x0010F2FF
	public int CurrentCount
	{
		get
		{
			return this.spawnEntries.Count + this.ReservedCount;
		}
	}

	// Token: 0x06002B66 RID: 11110 RVA: 0x00111113 File Offset: 0x0010F313
	public GameEventActionSequence.TargetTypes GetTargetType(string gameEventName)
	{
		if (GameEventManager.GameEventSequences.ContainsKey(gameEventName))
		{
			return GameEventManager.GameEventSequences[gameEventName].TargetType;
		}
		return GameEventActionSequence.TargetTypes.Entity;
	}

	// Token: 0x06002B67 RID: 11111 RVA: 0x00111134 File Offset: 0x0010F334
	public bool HandleAction(string name, EntityPlayer requester, Entity entity, bool twitchActivated, string extraData = "", string tag = "", bool crateShare = false, bool allowRefunds = true, string sequenceLink = "", GameEventActionSequence ownerSeq = null)
	{
		return this.HandleAction(name, requester, entity, twitchActivated, ActionTarget.None, extraData, tag, crateShare, allowRefunds, sequenceLink, ownerSeq, null);
	}

	// Token: 0x06002B68 RID: 11112 RVA: 0x00111160 File Offset: 0x0010F360
	public bool HandleAction(string name, EntityPlayer requester, Entity entity, bool twitchActivated, ActionTarget target, string extraData = "", string tag = "", bool crateShare = false, bool allowRefunds = true, string sequenceLink = "", GameEventActionSequence ownerSeq = null, List<Tuple<string, string>> variables = null)
	{
		if (name != null && name.Contains(','))
		{
			string[] array = name.Split(',', StringSplitOptions.None);
			bool flag = false;
			for (int i = 0; i < array.Length; i++)
			{
				if (this.HandleAction(array[i], requester, entity, twitchActivated, extraData, tag, crateShare, allowRefunds, sequenceLink, ownerSeq))
				{
					flag = true;
				}
			}
			return flag;
		}
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsClient)
		{
			return this.HandleActionClient(name, entity, twitchActivated, target, variables, extraData, tag, crateShare, allowRefunds, sequenceLink);
		}
		if (GameEventManager.GameEventSequences.ContainsKey(name))
		{
			GameEventActionSequence gameEventActionSequence = GameEventManager.GameEventSequences[name];
			if (variables != null)
			{
				foreach (Tuple<string, string> tuple in variables)
				{
					gameEventActionSequence.EventVariables.EventVariables[tuple.Item1] = tuple.Item2;
				}
			}
			if (gameEventActionSequence.CanPerform(entity))
			{
				if (gameEventActionSequence.SingleInstance)
				{
					for (int j = 0; j < this.ActionSequenceUpdates.Count; j++)
					{
						if (this.ActionSequenceUpdates[j].Name == name)
						{
							return false;
						}
					}
				}
				GameEventActionSequence gameEventActionSequence2 = gameEventActionSequence.Clone();
				gameEventActionSequence2.Target = entity;
				gameEventActionSequence2.TargetPosition = target;
				if (ownerSeq == null && sequenceLink != "" && requester != null)
				{
					ownerSeq = this.GetSequenceLink(requester, sequenceLink);
				}
				if (ownerSeq != null)
				{
					gameEventActionSequence2.Requester = ownerSeq.Requester;
					gameEventActionSequence2.ExtraData = ownerSeq.ExtraData;
					gameEventActionSequence2.CrateShare = ownerSeq.CrateShare;
					gameEventActionSequence2.Tag = ownerSeq.Tag;
					gameEventActionSequence2.AllowRefunds = ownerSeq.AllowRefunds;
					gameEventActionSequence2.TwitchActivated = ownerSeq.TwitchActivated;
				}
				else
				{
					gameEventActionSequence2.Requester = requester;
					gameEventActionSequence2.ExtraData = extraData;
					gameEventActionSequence2.CrateShare = crateShare;
					gameEventActionSequence2.Tag = tag;
					gameEventActionSequence2.AllowRefunds = allowRefunds;
					gameEventActionSequence2.TwitchActivated = twitchActivated;
				}
				gameEventActionSequence2.OwnerSequence = ownerSeq;
				if (gameEventActionSequence2.TargetType != GameEventActionSequence.TargetTypes.Entity)
				{
					gameEventActionSequence2.POIPosition = target;
				}
				gameEventActionSequence2.SetupTarget();
				this.ActionSequenceUpdates.Add(gameEventActionSequence2);
				return true;
			}
		}
		return false;
	}

	// Token: 0x06002B69 RID: 11113 RVA: 0x001113A8 File Offset: 0x0010F5A8
	public bool HandleActionClient(string name, Entity entity, bool twitchActivated, ActionTarget target, List<Tuple<string, string>> variables, string extraData = "", string tag = "", bool crateShare = false, bool allowRefunds = true, string sequenceLink = "")
	{
		SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(NetPackageManager.GetPackage<NetPackageGameEventRequest>().Setup(name, entity ? entity.entityId : (-1), twitchActivated, target, variables, extraData, tag, crateShare, allowRefunds, sequenceLink), false);
		return true;
	}

	// Token: 0x06002B6A RID: 11114 RVA: 0x001113EC File Offset: 0x0010F5EC
	public void Update(float deltaTime)
	{
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer && GameManager.Instance.World != null)
		{
			this.HandleSpawnUpdates(deltaTime);
			this.HandleActionUpdates();
			this.HandleBlockUpdates(deltaTime);
			this.HandleEventFlagUpdates(deltaTime);
			this.HandleBossGroupUpdates(deltaTime);
			this.HomerunManager.Update(deltaTime);
		}
	}

	// Token: 0x06002B6B RID: 11115 RVA: 0x00111440 File Offset: 0x0010F640
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleSpawnUpdates(float deltaTime)
	{
		bool flag = false;
		if (this.spawnEntries.Count > 0)
		{
			this.attackTimerUpdate -= deltaTime;
			if (this.attackTimerUpdate <= 0f)
			{
				flag = true;
				this.attackTimerUpdate = 2f;
			}
		}
		for (int i = this.spawnEntries.Count - 1; i >= 0; i--)
		{
			GameEventManager.SpawnEntry spawnEntry = this.spawnEntries[i];
			if (spawnEntry.SpawnedEntity.IsDespawned)
			{
				spawnEntry.GameEvent.HasDespawn = true;
				this.spawnEntries.RemoveAt(i);
				if (spawnEntry.Requester != null)
				{
					if (spawnEntry.Requester is EntityPlayerLocal)
					{
						GameEventManager.Current.HandleGameEntityDespawned(spawnEntry.SpawnedEntity.entityId);
					}
					else
					{
						SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageGameEventResponse>().Setup(NetPackageGameEventResponse.ResponseTypes.EntityDespawned, spawnEntry.SpawnedEntity.entityId, -1, "", false), false, spawnEntry.Requester.entityId, -1, -1, null, 192, false);
					}
				}
			}
			else if (!spawnEntry.SpawnedEntity.IsAlive() || spawnEntry.SpawnedEntity.emodel == null)
			{
				this.spawnEntries.RemoveAt(i);
				if (spawnEntry.Requester != null)
				{
					if (spawnEntry.Requester is EntityPlayerLocal)
					{
						GameEventManager.Current.HandleGameEntityKilled(spawnEntry.SpawnedEntity.entityId);
					}
					else
					{
						SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageGameEventResponse>().Setup(NetPackageGameEventResponse.ResponseTypes.EntityKilled, spawnEntry.SpawnedEntity.entityId, -1, "", false), false, spawnEntry.Requester.entityId, -1, -1, null, 192, false);
					}
				}
			}
			else if (flag)
			{
				spawnEntry.HandleUpdate();
			}
		}
	}

	// Token: 0x06002B6C RID: 11116 RVA: 0x0011160C File Offset: 0x0010F80C
	public void RemoveSpawnedEntry(Entity spawnedEntity)
	{
		for (int i = this.spawnEntries.Count - 1; i >= 0; i--)
		{
			if (this.spawnEntries[i].SpawnedEntity == spawnedEntity)
			{
				GameEventManager.SpawnEntry spawnEntry = this.spawnEntries[i];
				spawnEntry.GameEvent.HasDespawn = true;
				this.spawnEntries.RemoveAt(i);
				if (spawnEntry.Requester != null)
				{
					if (spawnEntry.Requester is EntityPlayerLocal)
					{
						GameEventManager.Current.HandleGameEntityDespawned(spawnEntry.SpawnedEntity.entityId);
					}
					else
					{
						SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageGameEventResponse>().Setup(NetPackageGameEventResponse.ResponseTypes.EntityDespawned, spawnEntry.SpawnedEntity.entityId, -1, "", false), false, spawnEntry.Requester.entityId, -1, -1, null, 192, false);
					}
				}
			}
		}
	}

	// Token: 0x06002B6D RID: 11117 RVA: 0x001116F0 File Offset: 0x0010F8F0
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleActionUpdates()
	{
		for (int i = 0; i < this.ActionSequenceUpdates.Count; i++)
		{
			GameEventActionSequence gameEventActionSequence = this.ActionSequenceUpdates[i];
			try
			{
				if (gameEventActionSequence.StartTime <= 0f)
				{
					gameEventActionSequence.StartSequence(this);
				}
				gameEventActionSequence.Update();
			}
			catch
			{
				if (gameEventActionSequence != null)
				{
					Log.Error("Exception while updating action sequence " + gameEventActionSequence.Name);
				}
				throw;
			}
		}
		for (int j = this.ActionSequenceUpdates.Count - 1; j >= 0; j--)
		{
			GameEventActionSequence gameEventActionSequence2 = this.ActionSequenceUpdates[j];
			if (!gameEventActionSequence2.HasTarget() && gameEventActionSequence2.AllowRefunds)
			{
				gameEventActionSequence2.IsComplete = true;
			}
			if (gameEventActionSequence2.IsComplete)
			{
				this.ReservedCount -= gameEventActionSequence2.ReservedSpawnCount;
				this.ActionSequenceUpdates.RemoveAt(j);
			}
		}
	}

	// Token: 0x06002B6E RID: 11118 RVA: 0x001117CC File Offset: 0x0010F9CC
	public void RegisterSpawnedEntity(Entity spawned, Entity target, EntityPlayer requester, GameEventActionSequence gameEvent, bool isAggressive = true)
	{
		this.spawnEntries.Add(new GameEventManager.SpawnEntry
		{
			SpawnedEntity = (spawned as EntityAlive),
			Target = (target as EntityAlive),
			Requester = requester,
			GameEvent = gameEvent
		});
	}

	// Token: 0x06002B6F RID: 11119 RVA: 0x00111808 File Offset: 0x0010FA08
	public GameEventManager.SpawnedBlocksEntry RegisterSpawnedBlocks(List<Vector3i> blockList, Entity target, EntityPlayer requester, GameEventActionSequence gameEvent, float timeAlive, string removeSound, Vector3 center, bool refundOnRemove)
	{
		GameEventManager.SpawnedBlocksEntry spawnedBlocksEntry = new GameEventManager.SpawnedBlocksEntry
		{
			BlockList = blockList,
			Target = target,
			Requester = requester,
			GameEvent = gameEvent,
			TimeAlive = timeAlive,
			RemoveSound = removeSound,
			Center = center,
			RefundOnRemove = refundOnRemove
		};
		this.blockEntries.Add(spawnedBlocksEntry);
		return spawnedBlocksEntry;
	}

	// Token: 0x1400002A RID: 42
	// (add) Token: 0x06002B70 RID: 11120 RVA: 0x00111868 File Offset: 0x0010FA68
	// (remove) Token: 0x06002B71 RID: 11121 RVA: 0x001118A0 File Offset: 0x0010FAA0
	public event OnGameEventAccessApproved GameEventAccessApproved;

	// Token: 0x06002B72 RID: 11122 RVA: 0x001118D5 File Offset: 0x0010FAD5
	public void HandleGameEventAccessApproved()
	{
		if (this.GameEventAccessApproved != null)
		{
			this.GameEventAccessApproved();
		}
	}

	// Token: 0x1400002B RID: 43
	// (add) Token: 0x06002B73 RID: 11123 RVA: 0x001118EC File Offset: 0x0010FAEC
	// (remove) Token: 0x06002B74 RID: 11124 RVA: 0x00111924 File Offset: 0x0010FB24
	public event OnGameEntityAdded GameEntitySpawned;

	// Token: 0x06002B75 RID: 11125 RVA: 0x00111959 File Offset: 0x0010FB59
	public void HandleGameEntitySpawned(string gameEventID, int entityID, string tag)
	{
		if (this.GameEntitySpawned != null)
		{
			this.GameEntitySpawned(gameEventID, entityID, tag);
		}
	}

	// Token: 0x1400002C RID: 44
	// (add) Token: 0x06002B76 RID: 11126 RVA: 0x00111974 File Offset: 0x0010FB74
	// (remove) Token: 0x06002B77 RID: 11127 RVA: 0x001119AC File Offset: 0x0010FBAC
	public event OnGameEntityChanged GameEntityDespawned;

	// Token: 0x06002B78 RID: 11128 RVA: 0x001119E1 File Offset: 0x0010FBE1
	public void HandleGameEntityDespawned(int entityID)
	{
		if (this.GameEntityDespawned != null)
		{
			this.GameEntityDespawned(entityID);
		}
	}

	// Token: 0x1400002D RID: 45
	// (add) Token: 0x06002B79 RID: 11129 RVA: 0x001119F8 File Offset: 0x0010FBF8
	// (remove) Token: 0x06002B7A RID: 11130 RVA: 0x00111A30 File Offset: 0x0010FC30
	public event OnGameEntityChanged GameEntityKilled;

	// Token: 0x06002B7B RID: 11131 RVA: 0x00111A65 File Offset: 0x0010FC65
	public void HandleGameEntityKilled(int entityID)
	{
		if (this.GameEntityKilled != null)
		{
			this.GameEntityKilled(entityID);
		}
	}

	// Token: 0x1400002E RID: 46
	// (add) Token: 0x06002B7C RID: 11132 RVA: 0x00111A7C File Offset: 0x0010FC7C
	// (remove) Token: 0x06002B7D RID: 11133 RVA: 0x00111AB4 File Offset: 0x0010FCB4
	public event OnGameBlocksAdded GameBlocksAdded;

	// Token: 0x06002B7E RID: 11134 RVA: 0x00111AE9 File Offset: 0x0010FCE9
	public void HandleGameBlocksAdded(string gameEventID, int blockGroupID, List<Vector3i> blockList, string tag)
	{
		if (this.GameBlocksAdded != null)
		{
			this.GameBlocksAdded(gameEventID, blockGroupID, blockList, tag);
		}
	}

	// Token: 0x1400002F RID: 47
	// (add) Token: 0x06002B7F RID: 11135 RVA: 0x00111B04 File Offset: 0x0010FD04
	// (remove) Token: 0x06002B80 RID: 11136 RVA: 0x00111B3C File Offset: 0x0010FD3C
	public event OnGameBlockRemoved GameBlockRemoved;

	// Token: 0x06002B81 RID: 11137 RVA: 0x00111B74 File Offset: 0x0010FD74
	public void BlockRemoved(Vector3i blockPos)
	{
		for (int i = 0; i < this.blockEntries.Count; i++)
		{
			if (this.blockEntries[i].RemoveBlock(blockPos))
			{
				if (this.blockEntries[i].Requester is EntityPlayerLocal)
				{
					GameEventManager.Current.HandleGameBlockRemoved(blockPos);
				}
				else
				{
					SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageGameEventResponse>().Setup(NetPackageGameEventResponse.ResponseTypes.BlockRemoved, blockPos), false, this.blockEntries[i].Requester.entityId, -1, -1, null, 192, false);
				}
				if (this.blockEntries[i].BlockList.Count == 0)
				{
					this.blockEntries.RemoveAt(i);
				}
				return;
			}
		}
	}

	// Token: 0x06002B82 RID: 11138 RVA: 0x00111C3D File Offset: 0x0010FE3D
	public void HandleGameBlockRemoved(Vector3i blockPos)
	{
		if (this.GameBlockRemoved != null)
		{
			this.GameBlockRemoved(blockPos);
		}
	}

	// Token: 0x14000030 RID: 48
	// (add) Token: 0x06002B83 RID: 11139 RVA: 0x00111C54 File Offset: 0x0010FE54
	// (remove) Token: 0x06002B84 RID: 11140 RVA: 0x00111C8C File Offset: 0x0010FE8C
	public event OnGameBlocksRemoved GameBlocksRemoved;

	// Token: 0x06002B85 RID: 11141 RVA: 0x00111CC1 File Offset: 0x0010FEC1
	public void HandleGameBlocksRemoved(int blockGroupID, bool isDespawn)
	{
		if (this.GameBlocksRemoved != null)
		{
			this.GameBlocksRemoved(blockGroupID, isDespawn);
		}
	}

	// Token: 0x14000031 RID: 49
	// (add) Token: 0x06002B86 RID: 11142 RVA: 0x00111CD8 File Offset: 0x0010FED8
	// (remove) Token: 0x06002B87 RID: 11143 RVA: 0x00111D10 File Offset: 0x0010FF10
	public event OnGameEventStatus GameEventApproved;

	// Token: 0x06002B88 RID: 11144 RVA: 0x00111D45 File Offset: 0x0010FF45
	public void HandleGameEventApproved(string gameEventID, int targetEntityID, string extraData, string tag)
	{
		if (this.GameEventApproved != null)
		{
			this.GameEventApproved(gameEventID, targetEntityID, extraData, tag);
		}
	}

	// Token: 0x14000032 RID: 50
	// (add) Token: 0x06002B89 RID: 11145 RVA: 0x00111D60 File Offset: 0x0010FF60
	// (remove) Token: 0x06002B8A RID: 11146 RVA: 0x00111D98 File Offset: 0x0010FF98
	public event OnGameEventStatus GameEventDenied;

	// Token: 0x06002B8B RID: 11147 RVA: 0x00111DCD File Offset: 0x0010FFCD
	public void HandleGameEventDenied(string gameEventID, int targetEntityID, string extraData, string tag)
	{
		if (this.GameEventDenied != null)
		{
			this.GameEventDenied(gameEventID, targetEntityID, extraData, tag);
		}
	}

	// Token: 0x14000033 RID: 51
	// (add) Token: 0x06002B8C RID: 11148 RVA: 0x00111DE8 File Offset: 0x0010FFE8
	// (remove) Token: 0x06002B8D RID: 11149 RVA: 0x00111E20 File Offset: 0x00110020
	public event OnGameEventStatus TwitchPartyGameEventApproved;

	// Token: 0x06002B8E RID: 11150 RVA: 0x00111E55 File Offset: 0x00110055
	public void HandleTwitchPartyGameEventApproved(string gameEventID, int targetEntityID, string extraData, string tag)
	{
		if (this.TwitchPartyGameEventApproved != null)
		{
			this.TwitchPartyGameEventApproved(gameEventID, targetEntityID, extraData, tag);
		}
	}

	// Token: 0x14000034 RID: 52
	// (add) Token: 0x06002B8F RID: 11151 RVA: 0x00111E70 File Offset: 0x00110070
	// (remove) Token: 0x06002B90 RID: 11152 RVA: 0x00111EA8 File Offset: 0x001100A8
	public event OnGameEventStatus TwitchRefundNeeded;

	// Token: 0x06002B91 RID: 11153 RVA: 0x00111EDD File Offset: 0x001100DD
	public void HandleTwitchRefundNeeded(string gameEventID, int targetEntityID, string extraData, string tag)
	{
		if (this.TwitchRefundNeeded != null)
		{
			this.TwitchRefundNeeded(gameEventID, targetEntityID, extraData, tag);
		}
	}

	// Token: 0x14000035 RID: 53
	// (add) Token: 0x06002B92 RID: 11154 RVA: 0x00111EF8 File Offset: 0x001100F8
	// (remove) Token: 0x06002B93 RID: 11155 RVA: 0x00111F30 File Offset: 0x00110130
	public event OnGameEventStatus GameEventCompleted;

	// Token: 0x06002B94 RID: 11156 RVA: 0x00111F65 File Offset: 0x00110165
	public void HandleGameEventCompleted(string gameEventID, int targetEntityID, string extraData, string tag)
	{
		if (this.GameEventCompleted != null)
		{
			this.GameEventCompleted(gameEventID, targetEntityID, extraData, tag);
		}
	}

	// Token: 0x06002B95 RID: 11157 RVA: 0x00111F80 File Offset: 0x00110180
	public void HandleGameEventSequenceItemForClient(string gameEventID, string key)
	{
		EntityPlayer player = XUiM_Player.GetPlayer();
		GameEventManager.GameEventSequences[gameEventID].HandleClientPerform(player, key);
	}

	// Token: 0x06002B96 RID: 11158 RVA: 0x00111FA8 File Offset: 0x001101A8
	public void HandleTwitchSetOwner(int targetEntityID, int entitySpawnedID, string extraData)
	{
		EntityAlive entityAlive = GameManager.Instance.World.GetEntity(entitySpawnedID) as EntityAlive;
		if (entityAlive != null)
		{
			entityAlive.SetSpawnByData(targetEntityID, extraData);
		}
	}

	// Token: 0x06002B97 RID: 11159 RVA: 0x00111FDC File Offset: 0x001101DC
	[PublicizedFrom(EAccessModifier.Protected)]
	public void HandleBlockUpdates(float deltaTime)
	{
		for (int i = this.blockEntries.Count - 1; i >= 0; i--)
		{
			GameEventManager.SpawnedBlocksEntry spawnedBlocksEntry = this.blockEntries[i];
			if (spawnedBlocksEntry.TimeAlive > 0f)
			{
				spawnedBlocksEntry.TimeAlive -= deltaTime;
			}
			else if (spawnedBlocksEntry.TimeAlive != -1f)
			{
				if (spawnedBlocksEntry.TryRemoveBlocks())
				{
					this.blockEntries.RemoveAt(i);
				}
				else
				{
					spawnedBlocksEntry.TimeAlive = 5f;
				}
			}
			if (spawnedBlocksEntry.IsRefunded)
			{
				this.blockEntries.RemoveAt(i);
			}
		}
	}

	// Token: 0x06002B98 RID: 11160 RVA: 0x00112070 File Offset: 0x00110270
	public void RefundSpawnedBlock(Vector3i blockPos)
	{
		for (int i = 0; i < this.blockEntries.Count; i++)
		{
			GameEventManager.SpawnedBlocksEntry spawnedBlocksEntry = this.blockEntries[i];
			if (spawnedBlocksEntry.BlockList.Contains(blockPos) && !spawnedBlocksEntry.IsRefunded)
			{
				spawnedBlocksEntry.GameEvent.SetRefundNeeded();
				spawnedBlocksEntry.IsRefunded = true;
			}
		}
	}

	// Token: 0x06002B99 RID: 11161 RVA: 0x001120C8 File Offset: 0x001102C8
	public void SendBlockDamageUpdate(Vector3i blockPos)
	{
		for (int i = 0; i < this.blockEntries.Count; i++)
		{
			GameEventManager.SpawnedBlocksEntry spawnedBlocksEntry = this.blockEntries[i];
			if (spawnedBlocksEntry.BlockList.Contains(blockPos))
			{
				spawnedBlocksEntry.GameEvent.EventVariables.ModifyEventVariable("Damaged", GameEventVariables.OperationTypes.Add, 1, int.MinValue, int.MaxValue);
			}
		}
	}

	// Token: 0x06002B9A RID: 11162 RVA: 0x00112128 File Offset: 0x00110328
	public void SetGameEventFlag(GameEventManager.GameEventFlagTypes flag, bool value, float duration, bool isPermanent)
	{
		if (!value)
		{
			for (int i = 0; i < this.GameEventFlags.Count; i++)
			{
				if (this.GameEventFlags[i].FlagType == flag)
				{
					this.GameEventFlags.RemoveAt(i);
					this.HandleFlagChanged(flag, true, false);
					return;
				}
			}
			return;
		}
		bool flag2 = false;
		for (int j = 0; j < this.GameEventFlags.Count; j++)
		{
			GameEventManager.GameEventFlag gameEventFlag = this.GameEventFlags[j];
			if (gameEventFlag.FlagType == flag)
			{
				if (!gameEventFlag.IsPermanent)
				{
					gameEventFlag.Duration = duration;
				}
				else if (!value)
				{
					gameEventFlag.Duration = 0f;
					gameEventFlag.IsPermanent = false;
				}
				flag2 = true;
			}
		}
		if (flag2)
		{
			return;
		}
		this.GameEventFlags.Add(new GameEventManager.GameEventFlag
		{
			FlagType = flag,
			Duration = duration,
			IsPermanent = isPermanent
		});
		this.HandleFlagChanged(flag, false, true);
	}

	// Token: 0x06002B9B RID: 11163 RVA: 0x00112208 File Offset: 0x00110408
	public bool CheckGameEventFlag(GameEventManager.GameEventFlagTypes flag)
	{
		for (int i = 0; i < this.GameEventFlags.Count; i++)
		{
			if (this.GameEventFlags[i].FlagType == flag)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06002B9C RID: 11164 RVA: 0x00112244 File Offset: 0x00110444
	[PublicizedFrom(EAccessModifier.Protected)]
	public void HandleEventFlagUpdates(float deltaTime)
	{
		for (int i = this.GameEventFlags.Count - 1; i >= 0; i--)
		{
			GameEventManager.GameEventFlag gameEventFlag = this.GameEventFlags[i];
			if (gameEventFlag.Duration > 0f || gameEventFlag.IsPermanent)
			{
				gameEventFlag.Duration -= deltaTime;
				if (!gameEventFlag.IsPermanent)
				{
					this.HandleFlagBuffUpdates(gameEventFlag.FlagType, deltaTime);
				}
				if (gameEventFlag.Duration <= 0f && !gameEventFlag.IsPermanent)
				{
					this.GameEventFlags.RemoveAt(i);
					this.HandleFlagChanged(gameEventFlag.FlagType, true, false);
				}
			}
		}
	}

	// Token: 0x06002B9D RID: 11165 RVA: 0x001122E0 File Offset: 0x001104E0
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleFlagBuffUpdates(GameEventManager.GameEventFlagTypes flag, float deltaTime)
	{
		this.gameFlagCheckTime -= deltaTime;
		if (this.gameFlagCheckTime <= 0f)
		{
			string text = "";
			switch (flag)
			{
			case GameEventManager.GameEventFlagTypes.BigHead:
				text = "twitch_buffBigHead";
				break;
			case GameEventManager.GameEventFlagTypes.Dancing:
				text = "twitch_buffDance";
				break;
			case GameEventManager.GameEventFlagTypes.BucketHead:
				text = "twitch_buffBucketHead";
				break;
			case GameEventManager.GameEventFlagTypes.TinyZombies:
				text = "twitch_buffTinyZombies";
				break;
			}
			foreach (EntityPlayer entityPlayer in GameManager.Instance.World.Players.dict.Values)
			{
				if (text != "" && !entityPlayer.Buffs.HasBuff(text))
				{
					entityPlayer.Buffs.AddBuff(text, -1, true, false, -1f);
				}
			}
			this.gameFlagCheckTime = 1f;
		}
	}

	// Token: 0x06002B9E RID: 11166 RVA: 0x001123D8 File Offset: 0x001105D8
	[PublicizedFrom(EAccessModifier.Protected)]
	public void HandleFlagChanged(GameEventManager.GameEventFlagTypes flag, bool oldValue, bool newValue)
	{
		if (GameManager.Instance == null || GameManager.Instance.World == null)
		{
			return;
		}
		switch (flag)
		{
		case GameEventManager.GameEventFlagTypes.BigHeadSandbox:
		{
			using (Dictionary<int, Entity>.ValueCollection.Enumerator enumerator = GameManager.Instance.World.Entities.dict.Values.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Entity entity = enumerator.Current;
					EntityAlive entityAlive = entity as EntityAlive;
					if (entityAlive != null && !(entityAlive is EntityPlayer))
					{
						if (newValue)
						{
							entityAlive.Buffs.AddBuff("sandbox_bighead", -1, true, false, -1f);
						}
						else
						{
							entityAlive.Buffs.RemoveBuff("sandbox_bighead", -1, true);
						}
					}
				}
				return;
			}
			break;
		}
		case GameEventManager.GameEventFlagTypes.BigHead:
			break;
		case GameEventManager.GameEventFlagTypes.Dancing:
			goto IL_01C4;
		case GameEventManager.GameEventFlagTypes.BucketHead:
			goto IL_02C9;
		case GameEventManager.GameEventFlagTypes.TinyZombies:
			goto IL_03D7;
		case GameEventManager.GameEventFlagTypes.TinyZombiesSandbox:
			goto IL_04DC;
		default:
			return;
		}
		foreach (Entity entity2 in GameManager.Instance.World.Entities.dict.Values)
		{
			EntityAlive entityAlive2 = entity2 as EntityAlive;
			if (entityAlive2 != null && !(entityAlive2 is EntityPlayer))
			{
				if (newValue)
				{
					entityAlive2.Buffs.AddBuff("twitch_bighead", -1, true, false, -1f);
				}
				else
				{
					entityAlive2.Buffs.RemoveBuff("twitch_bighead", -1, true);
				}
			}
		}
		using (Dictionary<int, EntityPlayer>.ValueCollection.Enumerator enumerator2 = GameManager.Instance.World.Players.dict.Values.GetEnumerator())
		{
			while (enumerator2.MoveNext())
			{
				EntityPlayer entityPlayer = enumerator2.Current;
				if (newValue)
				{
					entityPlayer.Buffs.AddBuff("twitch_buffBigHead", -1, true, false, -1f);
				}
				else
				{
					entityPlayer.Buffs.RemoveBuff("twitch_buffBigHead", -1, true);
				}
			}
			return;
		}
		IL_01C4:
		foreach (Entity entity3 in GameManager.Instance.World.Entities.dict.Values)
		{
			EntityAlive entityAlive3 = entity3 as EntityAlive;
			if (entityAlive3 != null && !(entityAlive3 is EntityPlayer))
			{
				if (newValue)
				{
					entityAlive3.Buffs.AddBuff("twitch_dance", -1, true, false, -1f);
				}
				else
				{
					entityAlive3.Buffs.RemoveBuff("twitch_dance", -1, true);
				}
			}
		}
		using (Dictionary<int, EntityPlayer>.ValueCollection.Enumerator enumerator2 = GameManager.Instance.World.Players.dict.Values.GetEnumerator())
		{
			while (enumerator2.MoveNext())
			{
				EntityPlayer entityPlayer2 = enumerator2.Current;
				if (newValue)
				{
					entityPlayer2.Buffs.AddBuff("twitch_buffDance", -1, true, false, -1f);
				}
				else
				{
					entityPlayer2.Buffs.RemoveBuff("twitch_buffDance", -1, true);
				}
			}
			return;
		}
		IL_02C9:
		foreach (Entity entity4 in GameManager.Instance.World.Entities.dict.Values)
		{
			EntityAlive entityAlive4 = entity4 as EntityAlive;
			if (entityAlive4 != null && !(entityAlive4 is EntityPlayer) && !(entityAlive4 is EntityVehicle))
			{
				if (newValue)
				{
					entityAlive4.Buffs.AddBuff("twitch_buckethead", -1, true, false, -1f);
				}
				else
				{
					entityAlive4.Buffs.RemoveBuff("twitch_buckethead", -1, true);
				}
			}
		}
		using (Dictionary<int, EntityPlayer>.ValueCollection.Enumerator enumerator2 = GameManager.Instance.World.Players.dict.Values.GetEnumerator())
		{
			while (enumerator2.MoveNext())
			{
				EntityPlayer entityPlayer3 = enumerator2.Current;
				if (newValue)
				{
					entityPlayer3.Buffs.AddBuff("twitch_buffBucketHead", -1, true, false, -1f);
				}
				else
				{
					entityPlayer3.Buffs.RemoveBuff("twitch_buffBucketHead", -1, true);
				}
			}
			return;
		}
		IL_03D7:
		foreach (Entity entity5 in GameManager.Instance.World.Entities.dict.Values)
		{
			EntityAlive entityAlive5 = entity5 as EntityAlive;
			if (entityAlive5 != null && entityAlive5 is EntityZombie)
			{
				if (newValue)
				{
					entityAlive5.Buffs.AddBuff("twitch_tiny", -1, true, false, -1f);
				}
				else
				{
					entityAlive5.Buffs.RemoveBuff("twitch_tiny", -1, true);
				}
			}
		}
		using (Dictionary<int, EntityPlayer>.ValueCollection.Enumerator enumerator2 = GameManager.Instance.World.Players.dict.Values.GetEnumerator())
		{
			while (enumerator2.MoveNext())
			{
				EntityPlayer entityPlayer4 = enumerator2.Current;
				if (newValue)
				{
					entityPlayer4.Buffs.AddBuff("twitch_buffTinyZombies", -1, true, false, -1f);
				}
				else
				{
					entityPlayer4.Buffs.RemoveBuff("twitch_buffTinyZombies", -1, true);
				}
			}
			return;
		}
		IL_04DC:
		foreach (Entity entity6 in GameManager.Instance.World.Entities.dict.Values)
		{
			EntityAlive entityAlive6 = entity6 as EntityAlive;
			if (entityAlive6 != null && entityAlive6 is EntityZombie)
			{
				if (newValue)
				{
					entityAlive6.Buffs.AddBuff("sandbox_tiny", -1, true, false, -1f);
				}
				else
				{
					entityAlive6.Buffs.RemoveBuff("sandbox_tiny", -1, true);
				}
			}
		}
	}

	// Token: 0x06002B9F RID: 11167 RVA: 0x001129CC File Offset: 0x00110BCC
	public void HandleSpawnModifier(EntityAlive alive)
	{
		for (int i = 0; i < this.GameEventFlags.Count; i++)
		{
			switch (this.GameEventFlags[i].FlagType)
			{
			case GameEventManager.GameEventFlagTypes.BigHeadSandbox:
				if (alive != null && !(alive is EntityPlayer))
				{
					alive.Buffs.AddBuff("sandbox_bighead", -1, true, false, -1f);
				}
				break;
			case GameEventManager.GameEventFlagTypes.BigHead:
				if (alive != null && !(alive is EntityPlayer))
				{
					alive.Buffs.AddBuff("twitch_bighead", -1, true, false, -1f);
				}
				break;
			case GameEventManager.GameEventFlagTypes.Dancing:
				if (alive != null && !(alive is EntityPlayer))
				{
					alive.Buffs.AddBuff("twitch_dance", -1, true, false, -1f);
				}
				break;
			case GameEventManager.GameEventFlagTypes.BucketHead:
				if (alive != null && !(alive is EntityPlayer) && !(alive is EntityVehicle))
				{
					alive.Buffs.AddBuff("twitch_buckethead", -1, true, false, -1f);
				}
				break;
			case GameEventManager.GameEventFlagTypes.TinyZombies:
				if (alive != null && alive is EntityZombie)
				{
					alive.Buffs.AddBuff("twitch_tiny", -1, true, false, -1f);
				}
				break;
			case GameEventManager.GameEventFlagTypes.TinyZombiesSandbox:
				if (alive != null && alive is EntityZombie)
				{
					alive.Buffs.AddBuff("sandbox_tiny", -1, true, false, -1f);
				}
				break;
			}
		}
	}

	// Token: 0x06002BA0 RID: 11168 RVA: 0x00112B58 File Offset: 0x00110D58
	public void HandleForceBossDespawn(EntityPlayer player)
	{
		for (int i = 0; i < this.BossGroups.Count; i++)
		{
			if (this.BossGroups[i].TargetPlayer == player)
			{
				this.BossGroups[i].RemoveNavObjects();
				this.BossGroups[i].DespawnAll();
			}
		}
	}

	// Token: 0x06002BA1 RID: 11169 RVA: 0x00112BB8 File Offset: 0x00110DB8
	public int SetupBossGroup(EntityPlayer target, EntityAlive boss, List<EntityAlive> minions, BossGroup.BossGroupTypes bossGroupType, string bossIcon)
	{
		for (int i = 0; i < this.BossGroups.Count; i++)
		{
			if (this.BossGroups[i].BossEntity == boss)
			{
				return this.BossGroups[i].BossGroupID;
			}
		}
		BossGroup bossGroup = new BossGroup(target, boss, minions, bossGroupType, bossIcon);
		this.BossGroups.Add(bossGroup);
		SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageBossEvent>().Setup(NetPackageBossEvent.BossEventTypes.AddGroup, bossGroup.BossGroupID, bossGroup.CurrentGroupType, bossGroup.BossEntityID, bossGroup.MinionEntityIDs, bossGroup.BossIcon), false, -1, -1, -1, null, 192, false);
		return bossGroup.BossGroupID;
	}

	// Token: 0x06002BA2 RID: 11170 RVA: 0x00112C6C File Offset: 0x00110E6C
	public void UpdateBossGroupType(int bossGroupID, BossGroup.BossGroupTypes bossGroupType)
	{
		for (int i = 0; i < this.BossGroups.Count; i++)
		{
			if (bossGroupID == this.BossGroups[i].BossGroupID)
			{
				BossGroup bossGroup = this.BossGroups[i];
				bossGroup.CurrentGroupType = bossGroupType;
				if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
				{
					SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageBossEvent>().Setup(NetPackageBossEvent.BossEventTypes.UpdateGroupType, bossGroupID, bossGroupType), false, -1, -1, -1, null, 192, false);
				}
				if (bossGroup.IsCurrent)
				{
					bossGroup.RemoveNavObjects();
					bossGroup.AddNavObjects();
				}
			}
		}
	}

	// Token: 0x06002BA3 RID: 11171 RVA: 0x00112D04 File Offset: 0x00110F04
	public void SetupClientBossGroup(int bossGroupID, BossGroup.BossGroupTypes bossGroupType, int bossID, List<int> minionIDs, string bossIcon1)
	{
		for (int i = 0; i < this.BossGroups.Count; i++)
		{
			if (bossGroupID == this.BossGroups[i].BossGroupID)
			{
				this.BossGroups[i].CurrentGroupType = bossGroupType;
				return;
			}
		}
		this.BossGroups.Add(new BossGroup(bossGroupID, bossGroupType, bossID, minionIDs, bossIcon1));
	}

	// Token: 0x06002BA4 RID: 11172 RVA: 0x00112D68 File Offset: 0x00110F68
	public void RemoveClientBossGroup(int bossGroupID)
	{
		for (int i = 0; i < this.BossGroups.Count; i++)
		{
			if (bossGroupID == this.BossGroups[i].BossGroupID)
			{
				this.BossGroups[i].RemoveNavObjects();
				this.BossGroups.RemoveAt(i);
				return;
			}
		}
	}

	// Token: 0x06002BA5 RID: 11173 RVA: 0x00112DC0 File Offset: 0x00110FC0
	public void RemoveEntityFromBossGroup(int bossGroupID, int entityID)
	{
		for (int i = 0; i < this.BossGroups.Count; i++)
		{
			if (bossGroupID == this.BossGroups[i].BossGroupID)
			{
				this.BossGroups[i].RemoveMinion(entityID);
			}
		}
	}

	// Token: 0x06002BA6 RID: 11174 RVA: 0x00112E0C File Offset: 0x0011100C
	public void SendBossGroups(int entityID)
	{
		for (int i = 0; i < this.BossGroups.Count; i++)
		{
			BossGroup bossGroup = this.BossGroups[i];
			SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageBossEvent>().Setup(NetPackageBossEvent.BossEventTypes.AddGroup, bossGroup.BossGroupID, bossGroup.CurrentGroupType, bossGroup.BossEntityID, bossGroup.MinionEntityIDs, bossGroup.BossIcon), false, entityID, -1, -1, null, 192, false);
		}
	}

	// Token: 0x06002BA7 RID: 11175 RVA: 0x00112E82 File Offset: 0x00111082
	public void RequestBossGroupStatRefresh(int bossGroupID, int playerID)
	{
		if (this.BossGroups.Count > 0)
		{
			this.BossGroups[0].RefreshStats(playerID);
		}
	}

	// Token: 0x06002BA8 RID: 11176 RVA: 0x00112EA4 File Offset: 0x001110A4
	public void HandleBossGroupUpdates(float deltaTime)
	{
		if (GameManager.Instance.World == null)
		{
			return;
		}
		this.bossCheckTime -= deltaTime;
		for (int i = this.BossGroups.Count - 1; i >= 0; i--)
		{
			this.BossGroups[i].HandleAutoPull();
			this.BossGroups[i].HandleLiveHandling();
			if (this.bossCheckTime <= 0f && this.BossGroups[i].ServerUpdate())
			{
				if (this.CurrentBossGroup == this.BossGroups[i])
				{
					this.CurrentBossGroup = null;
				}
				this.BossGroups.RemoveAt(i);
			}
		}
		if (this.bossCheckTime <= 0f)
		{
			this.bossCheckTime = 1f;
		}
	}

	// Token: 0x06002BA9 RID: 11177 RVA: 0x00112F68 File Offset: 0x00111168
	public void UpdateCurrentBossGroup(EntityPlayerLocal player)
	{
		this.serverBossGroupCheckTime -= Time.deltaTime;
		if (!this.BossGroupInitialized)
		{
			if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsClient)
			{
				SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(NetPackageManager.GetPackage<NetPackageBossEvent>().Setup(NetPackageBossEvent.BossEventTypes.RequestGroups, -1), false);
			}
			this.BossGroupInitialized = true;
			return;
		}
		if (this.serverBossGroupCheckTime <= 0f)
		{
			if (this.CurrentBossGroup != null)
			{
				this.CurrentBossGroup.Update(player);
				if (this.CurrentBossGroup.ReadyForRemove || !this.CurrentBossGroup.IsPlayerWithinRange(player))
				{
					this.CurrentBossGroup = null;
				}
			}
			else
			{
				for (int i = 0; i < this.BossGroups.Count; i++)
				{
					this.BossGroups[i].Update(player);
					if (!this.BossGroups[i].ReadyForRemove && this.BossGroups[i].IsPlayerWithinRange(player))
					{
						this.CurrentBossGroup = this.BossGroups[i];
						this.serverBossGroupCheckTime = 1f;
						return;
					}
				}
				this.CurrentBossGroup = null;
			}
			this.serverBossGroupCheckTime = 1f;
		}
	}

	// Token: 0x06002BAA RID: 11178 RVA: 0x00113084 File Offset: 0x00111284
	public void RegisterLink(EntityPlayer player, GameEventActionSequence seq, string tag)
	{
		for (int i = 0; i < this.SequenceLinks.Count; i++)
		{
			if (this.SequenceLinks[i].CheckLink(player, tag))
			{
				return;
			}
		}
		this.SequenceLinks.Add(new GameEventManager.SequenceLink
		{
			Owner = player,
			OwnerSeq = seq,
			Tag = tag
		});
	}

	// Token: 0x06002BAB RID: 11179 RVA: 0x001130E4 File Offset: 0x001112E4
	public bool HasSequenceLink(GameEventActionSequence seq)
	{
		for (int i = 0; i < this.SequenceLinks.Count; i++)
		{
			if (this.SequenceLinks[i].OwnerSeq == seq)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06002BAC RID: 11180 RVA: 0x00113120 File Offset: 0x00111320
	public GameEventActionSequence GetSequenceLink(EntityPlayer player, string tag)
	{
		if (player == null || tag == "")
		{
			return null;
		}
		for (int i = 0; i < this.SequenceLinks.Count; i++)
		{
			if (this.SequenceLinks[i].CheckLink(player, tag))
			{
				return this.SequenceLinks[i].OwnerSeq;
			}
		}
		return null;
	}

	// Token: 0x06002BAD RID: 11181 RVA: 0x00113184 File Offset: 0x00111384
	public void UnRegisterLink(EntityPlayer player, string tag)
	{
		for (int i = 0; i < this.SequenceLinks.Count; i++)
		{
			if (this.SequenceLinks[i].CheckLink(player, tag))
			{
				this.SequenceLinks.RemoveAt(i);
			}
		}
	}

	// Token: 0x06002BAE RID: 11182 RVA: 0x001131C8 File Offset: 0x001113C8
	public static int GetIntValue(EntityAlive alive, string value, int defaultValue = 0)
	{
		if (string.IsNullOrEmpty(value))
		{
			return defaultValue;
		}
		if (value.StartsWith("@"))
		{
			if (alive != null)
			{
				return (int)alive.Buffs.GetCustomVar(value.Substring(1));
			}
			return defaultValue;
		}
		else if (value.StartsWith("{") && value.EndsWith("}"))
		{
			value.IndexOf('{');
			string[] array = value.Substring(1, value.Length - 2).ToLower().Split('.', StringSplitOptions.None);
			global::SandboxOptions.SandboxOptions sandboxOptions;
			if (array.Length == 2 && array[0] == "sandbox" && Enum.TryParse<global::SandboxOptions.SandboxOptions>(array[1], true, out sandboxOptions))
			{
				return SandboxOptionManager.GetInt(sandboxOptions);
			}
			return 0;
		}
		else
		{
			if (value.Contains("-"))
			{
				string[] array2 = value.Split('-', StringSplitOptions.None);
				int num = StringParsers.ParseSInt32(array2[0], 0, -1, NumberStyles.Integer);
				int num2 = StringParsers.ParseSInt32(array2[1], 0, -1, NumberStyles.Integer) + 1;
				return GameEventManager.instance.Random.RandomRange(num, num2);
			}
			int num3 = 0;
			StringParsers.TryParseSInt32(value, out num3);
			return num3;
		}
	}

	// Token: 0x06002BAF RID: 11183 RVA: 0x001132C8 File Offset: 0x001114C8
	public static float GetFloatValue(EntityAlive alive, string value, float defaultValue = 0f)
	{
		if (string.IsNullOrEmpty(value))
		{
			return defaultValue;
		}
		if (value.StartsWith("@"))
		{
			if (alive != null)
			{
				return alive.Buffs.GetCustomVar(value.Substring(1));
			}
			return defaultValue;
		}
		else if (value.StartsWith("{") && value.EndsWith("}"))
		{
			value.IndexOf('{');
			string[] array = value.Substring(1, value.Length - 2).ToLower().Split('.', StringSplitOptions.None);
			global::SandboxOptions.SandboxOptions sandboxOptions;
			if (array.Length == 2 && array[0] == "sandbox" && Enum.TryParse<global::SandboxOptions.SandboxOptions>(array[1], true, out sandboxOptions))
			{
				return SandboxOptionManager.GetFloat(sandboxOptions);
			}
			return 0f;
		}
		else
		{
			if (value.Contains("-"))
			{
				string[] array2 = value.Split('-', StringSplitOptions.None);
				float num = (float)StringParsers.ParseSInt32(array2[0], 0, -1, NumberStyles.Integer);
				float num2 = (float)(StringParsers.ParseSInt32(array2[1], 0, -1, NumberStyles.Integer) + 1);
				return GameEventManager.instance.Random.RandomRange(num, num2);
			}
			float num3 = 0f;
			StringParsers.TryParseFloat(value, out num3);
			return num3;
		}
	}

	// Token: 0x040020FA RID: 8442
	[PublicizedFrom(EAccessModifier.Private)]
	public static GameEventManager instance = null;

	// Token: 0x040020FB RID: 8443
	public static Dictionary<string, GameEventActionSequence> GameEventSequences = new Dictionary<string, GameEventActionSequence>();

	// Token: 0x040020FC RID: 8444
	public GameRandom Random;

	// Token: 0x040020FD RID: 8445
	public List<string> ActiveRecipients = new List<string>();

	// Token: 0x040020FE RID: 8446
	public List<GameEventActionSequence> ActionSequenceUpdates = new List<GameEventActionSequence>();

	// Token: 0x040020FF RID: 8447
	public List<GameEventManager.SpawnEntry> spawnEntries = new List<GameEventManager.SpawnEntry>();

	// Token: 0x04002100 RID: 8448
	public List<GameEventManager.SpawnedBlocksEntry> blockEntries = new List<GameEventManager.SpawnedBlocksEntry>();

	// Token: 0x04002101 RID: 8449
	public List<GameEventManager.Category> CategoryList = new List<GameEventManager.Category>();

	// Token: 0x04002102 RID: 8450
	public HomerunManager HomerunManager = new HomerunManager();

	// Token: 0x04002103 RID: 8451
	public int MaxSpawnCount = 20;

	// Token: 0x04002104 RID: 8452
	public int ReservedCount;

	// Token: 0x04002105 RID: 8453
	public const int AttackTime = 12000;

	// Token: 0x04002106 RID: 8454
	[PublicizedFrom(EAccessModifier.Private)]
	public List<GameEventManager.GameEventFlag> GameEventFlags = new List<GameEventManager.GameEventFlag>();

	// Token: 0x04002107 RID: 8455
	public bool BossGroupInitialized;

	// Token: 0x04002108 RID: 8456
	public List<BossGroup> BossGroups = new List<BossGroup>();

	// Token: 0x04002109 RID: 8457
	[PublicizedFrom(EAccessModifier.Private)]
	public BossGroup currentBossGroup;

	// Token: 0x0400210A RID: 8458
	[PublicizedFrom(EAccessModifier.Private)]
	public float serverBossGroupCheckTime = 1f;

	// Token: 0x0400210B RID: 8459
	[PublicizedFrom(EAccessModifier.Private)]
	public float bossCheckTime = 1f;

	// Token: 0x0400210C RID: 8460
	[PublicizedFrom(EAccessModifier.Private)]
	public float gameFlagCheckTime = 1f;

	// Token: 0x0400210D RID: 8461
	[PublicizedFrom(EAccessModifier.Private)]
	public float attackTimerUpdate = 2f;

	// Token: 0x0400211A RID: 8474
	public List<GameEventManager.SequenceLink> SequenceLinks = new List<GameEventManager.SequenceLink>();

	// Token: 0x02000529 RID: 1321
	public class Category
	{
		// Token: 0x0400211B RID: 8475
		public string Name;

		// Token: 0x0400211C RID: 8476
		public string DisplayName;

		// Token: 0x0400211D RID: 8477
		public string Icon;
	}

	// Token: 0x0200052A RID: 1322
	public enum GameEventFlagTypes
	{
		// Token: 0x0400211F RID: 8479
		Invalid = -1,
		// Token: 0x04002120 RID: 8480
		BigHeadSandbox,
		// Token: 0x04002121 RID: 8481
		BigHead,
		// Token: 0x04002122 RID: 8482
		Dancing,
		// Token: 0x04002123 RID: 8483
		BucketHead,
		// Token: 0x04002124 RID: 8484
		TinyZombies,
		// Token: 0x04002125 RID: 8485
		TinyZombiesSandbox
	}

	// Token: 0x0200052B RID: 1323
	public class SpawnEntry
	{
		// Token: 0x06002BB2 RID: 11186 RVA: 0x001133E4 File Offset: 0x001115E4
		public void HandleUpdate()
		{
			if (!this.IsAggressive)
			{
				return;
			}
			EntityPlayer entityPlayer = this.SpawnedEntity.GetAttackTarget() as EntityPlayer;
			if (entityPlayer == null)
			{
				this.SpawnedEntity.SetAttackTarget(this.SpawnedEntity.world.GetClosestPlayer(this.SpawnedEntity, 500f, false), 1000);
				return;
			}
			this.SpawnedEntity.SetAttackTarget(entityPlayer, 1000);
		}

		// Token: 0x04002126 RID: 8486
		public EntityAlive SpawnedEntity;

		// Token: 0x04002127 RID: 8487
		public EntityAlive Target;

		// Token: 0x04002128 RID: 8488
		public EntityPlayer Requester;

		// Token: 0x04002129 RID: 8489
		public GameEventActionSequence GameEvent;

		// Token: 0x0400212A RID: 8490
		public bool IsAggressive;
	}

	// Token: 0x0200052C RID: 1324
	public class SpawnedBlocksEntry
	{
		// Token: 0x06002BB4 RID: 11188 RVA: 0x00113452 File Offset: 0x00111652
		public SpawnedBlocksEntry()
		{
			this.BlockGroupID = ++GameEventManager.SpawnedBlocksEntry.newID;
		}

		// Token: 0x06002BB5 RID: 11189 RVA: 0x00113484 File Offset: 0x00111684
		public bool RemoveBlock(Vector3i blockPos)
		{
			bool flag = false;
			for (int i = this.BlockList.Count - 1; i >= 0; i--)
			{
				if (this.BlockList[i] == blockPos)
				{
					this.BlockList.RemoveAt(i);
					flag = true;
				}
			}
			return flag;
		}

		// Token: 0x06002BB6 RID: 11190 RVA: 0x001134D0 File Offset: 0x001116D0
		[PublicizedFrom(EAccessModifier.Internal)]
		public bool TryRemoveBlocks()
		{
			List<BlockChangeInfo> list = null;
			World world = GameManager.Instance.World;
			IChunk chunk = null;
			for (int i = this.BlockList.Count - 1; i >= 0; i--)
			{
				if (world.GetChunkFromWorldPos(this.BlockList[i], ref chunk))
				{
					if (list == null)
					{
						list = new List<BlockChangeInfo>();
					}
					list.Add(new BlockChangeInfo(this.BlockList[i], BlockValue.Air, false));
					this.BlockList.RemoveAt(i);
				}
			}
			if (list != null)
			{
				GameManager.Instance.World.SetBlocksRPC(list);
			}
			if (this.BlockList.Count == 0)
			{
				if (!string.IsNullOrEmpty(this.RemoveSound))
				{
					Manager.BroadcastPlayByLocalPlayer(this.Center, this.RemoveSound);
				}
				if (this.RefundOnRemove)
				{
					this.GameEvent.SetRefundNeeded();
				}
				if (this.Requester is EntityPlayerLocal)
				{
					GameEventManager.Current.HandleGameBlocksRemoved(this.BlockGroupID, this.IsDespawn);
				}
				else
				{
					SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageGameEventResponse>().Setup(NetPackageGameEventResponse.ResponseTypes.BlocksRemoved, -1, this.BlockGroupID, "", this.IsDespawn), false, this.Requester.entityId, -1, -1, null, 192, false);
				}
			}
			return this.BlockList.Count == 0;
		}

		// Token: 0x0400212B RID: 8491
		public int BlockGroupID;

		// Token: 0x0400212C RID: 8492
		[PublicizedFrom(EAccessModifier.Private)]
		public static int newID;

		// Token: 0x0400212D RID: 8493
		public List<Vector3i> BlockList = new List<Vector3i>();

		// Token: 0x0400212E RID: 8494
		public Vector3 Center;

		// Token: 0x0400212F RID: 8495
		public Entity Target;

		// Token: 0x04002130 RID: 8496
		public EntityPlayer Requester;

		// Token: 0x04002131 RID: 8497
		public GameEventActionSequence GameEvent;

		// Token: 0x04002132 RID: 8498
		public float TimeAlive = -1f;

		// Token: 0x04002133 RID: 8499
		public string RemoveSound;

		// Token: 0x04002134 RID: 8500
		public bool RefundOnRemove;

		// Token: 0x04002135 RID: 8501
		public bool IsDespawn;

		// Token: 0x04002136 RID: 8502
		public bool IsRefunded;
	}

	// Token: 0x0200052D RID: 1325
	[PublicizedFrom(EAccessModifier.Private)]
	public class GameEventFlag
	{
		// Token: 0x04002137 RID: 8503
		public GameEventManager.GameEventFlagTypes FlagType;

		// Token: 0x04002138 RID: 8504
		public float Duration = -1f;

		// Token: 0x04002139 RID: 8505
		public bool IsPermanent;
	}

	// Token: 0x0200052E RID: 1326
	public class SequenceLink
	{
		// Token: 0x06002BB8 RID: 11192 RVA: 0x00113631 File Offset: 0x00111831
		public bool CheckLink(EntityPlayer player, string tag)
		{
			return this.Owner == player && this.Tag == tag;
		}

		// Token: 0x0400213A RID: 8506
		public EntityPlayer Owner;

		// Token: 0x0400213B RID: 8507
		public GameEventActionSequence OwnerSeq;

		// Token: 0x0400213C RID: 8508
		public string Tag = "";
	}
}
