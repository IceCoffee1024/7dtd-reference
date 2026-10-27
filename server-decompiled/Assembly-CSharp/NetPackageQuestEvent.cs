using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000843 RID: 2115
[Preserve]
public class NetPackageQuestEvent : NetPackage
{
	// Token: 0x06003D99 RID: 15769 RVA: 0x00187B2A File Offset: 0x00185D2A
	public NetPackageQuestEvent Setup(NetPackageQuestEvent.QuestEventTypes _eventType, int _entityID)
	{
		this.eventType = _eventType;
		this.entityID = _entityID;
		return this;
	}

	// Token: 0x06003D9A RID: 15770 RVA: 0x00187B3B File Offset: 0x00185D3B
	public NetPackageQuestEvent Setup(NetPackageQuestEvent.QuestEventTypes _eventType, int _entityID, int _traderID, int _overrideFactionPoints)
	{
		this.eventType = _eventType;
		this.entityID = _entityID;
		this.questCode = _traderID;
		this.factionPointOverride = _overrideFactionPoints;
		return this;
	}

	// Token: 0x06003D9B RID: 15771 RVA: 0x00187B5B File Offset: 0x00185D5B
	public NetPackageQuestEvent Setup(NetPackageQuestEvent.QuestEventTypes _eventType, int _entityID, Vector3 _prefabPos, int _questCode)
	{
		this.entityID = _entityID;
		this.prefabPos = _prefabPos;
		this.eventType = _eventType;
		this.questCode = _questCode;
		return this;
	}

	// Token: 0x06003D9C RID: 15772 RVA: 0x00187B7B File Offset: 0x00185D7B
	public NetPackageQuestEvent Setup(NetPackageQuestEvent.QuestEventTypes _eventType, int _entityID, Vector3 _prefabPos, int _questCode, ulong _extraData)
	{
		this.entityID = _entityID;
		this.prefabPos = _prefabPos;
		this.eventType = _eventType;
		this.questCode = _questCode;
		this.extraData = _extraData;
		return this;
	}

	// Token: 0x06003D9D RID: 15773 RVA: 0x00187BA3 File Offset: 0x00185DA3
	public NetPackageQuestEvent Setup(NetPackageQuestEvent.QuestEventTypes _eventType, int _entityID, Vector3 _prefabPos)
	{
		this.entityID = _entityID;
		this.prefabPos = _prefabPos;
		this.eventType = _eventType;
		return this;
	}

	// Token: 0x06003D9E RID: 15774 RVA: 0x00187BBB File Offset: 0x00185DBB
	public NetPackageQuestEvent Setup(NetPackageQuestEvent.QuestEventTypes _eventType, int _entityID, string _questID, FastTags<TagGroup.Global> _questTags, Vector3 _prefabPos, int[] _sharedWithList)
	{
		this.entityID = _entityID;
		this.prefabPos = _prefabPos;
		this.eventType = _eventType;
		this.questTags = _questTags;
		this.questID = _questID;
		this.SharedWithList = _sharedWithList;
		return this;
	}

	// Token: 0x06003D9F RID: 15775 RVA: 0x00187BEB File Offset: 0x00185DEB
	public NetPackageQuestEvent Setup(NetPackageQuestEvent.QuestEventTypes _eventType, int _entityID, Vector3 _prefabPos, ObjectiveFetchFromContainer.FetchModeTypes _fetchModeType)
	{
		this.entityID = _entityID;
		this.prefabPos = _prefabPos;
		this.eventType = _eventType;
		this.FetchModeType = _fetchModeType;
		return this;
	}

	// Token: 0x06003DA0 RID: 15776 RVA: 0x00187C0B File Offset: 0x00185E0B
	public NetPackageQuestEvent Setup(NetPackageQuestEvent.QuestEventTypes _eventType, int _entityID, Vector3 _prefabPos, ObjectiveFetchFromContainer.FetchModeTypes _fetchModeType, int[] _sharedWithList)
	{
		this.entityID = _entityID;
		this.prefabPos = _prefabPos;
		this.eventType = _eventType;
		this.FetchModeType = _fetchModeType;
		this.SharedWithList = _sharedWithList;
		return this;
	}

	// Token: 0x06003DA1 RID: 15777 RVA: 0x00187C33 File Offset: 0x00185E33
	public NetPackageQuestEvent Setup(NetPackageQuestEvent.QuestEventTypes _eventType, int _entityID, int _questCode, string _completeEvent, Vector3 _prefabPos, string _blockIndex, int[] _sharedWithList)
	{
		this.entityID = _entityID;
		this.questCode = _questCode;
		this.eventName = _completeEvent;
		this.prefabPos = _prefabPos;
		this.eventType = _eventType;
		this.blockIndex = _blockIndex;
		this.SharedWithList = _sharedWithList;
		return this;
	}

	// Token: 0x06003DA2 RID: 15778 RVA: 0x00187C6B File Offset: 0x00185E6B
	public NetPackageQuestEvent Setup(NetPackageQuestEvent.QuestEventTypes _eventType, int _entityID, Vector3 _prefabPos, bool _subscribeTo)
	{
		this.entityID = _entityID;
		this.prefabPos = _prefabPos;
		this.eventType = _eventType;
		this.SubscribeTo = _subscribeTo;
		return this;
	}

	// Token: 0x06003DA3 RID: 15779 RVA: 0x00187C8B File Offset: 0x00185E8B
	public NetPackageQuestEvent Setup(NetPackageQuestEvent.QuestEventTypes _eventType, int _entityID, int _questCode, string _completeEvent, Vector3 _prefabPos, List<Vector3i> _activateList)
	{
		this.entityID = _entityID;
		this.questCode = _questCode;
		this.eventName = _completeEvent;
		this.prefabPos = _prefabPos;
		this.eventType = _eventType;
		this.activateList = _activateList;
		return this;
	}

	// Token: 0x06003DA4 RID: 15780 RVA: 0x00187CBB File Offset: 0x00185EBB
	public NetPackageQuestEvent Setup(NetPackageQuestEvent.QuestEventTypes _eventType, int _entityID, int _questCode, string _questID, Vector3 _prefabPos, int[] _sharedWithList)
	{
		this.entityID = _entityID;
		this.questCode = _questCode;
		this.prefabPos = _prefabPos;
		this.eventType = _eventType;
		this.questID = _questID;
		this.SharedWithList = _sharedWithList;
		return this;
	}

	// Token: 0x06003DA5 RID: 15781 RVA: 0x00187CEC File Offset: 0x00185EEC
	public override void read(PooledBinaryReader _reader)
	{
		this.entityID = _reader.ReadInt32();
		this.prefabPos = StreamUtils.ReadVector3(_reader);
		this.eventType = (NetPackageQuestEvent.QuestEventTypes)_reader.ReadByte();
		this.questTags = FastTags<TagGroup.Global>.Parse(_reader.ReadString());
		this.questCode = _reader.ReadInt32();
		NetPackageQuestEvent.QuestEventTypes questEventTypes = this.eventType;
		if (questEventTypes != NetPackageQuestEvent.QuestEventTypes.RallyMarkerLocked)
		{
			switch (questEventTypes)
			{
			case NetPackageQuestEvent.QuestEventTypes.LockPOI:
			{
				this.questID = _reader.ReadString();
				int num = (int)_reader.ReadByte();
				if (num > 0)
				{
					this.SharedWithList = new int[num];
					for (int i = 0; i < num; i++)
					{
						this.SharedWithList[i] = _reader.ReadInt32();
					}
					return;
				}
				this.SharedWithList = null;
				return;
			}
			case NetPackageQuestEvent.QuestEventTypes.UnlockPOI:
			case NetPackageQuestEvent.QuestEventTypes.ShowSleeperVolume:
			case NetPackageQuestEvent.QuestEventTypes.HideSleeperVolume:
				break;
			case NetPackageQuestEvent.QuestEventTypes.ClearSleeper:
				this.SubscribeTo = _reader.ReadBoolean();
				return;
			case NetPackageQuestEvent.QuestEventTypes.SetupFetch:
			{
				this.FetchModeType = (ObjectiveFetchFromContainer.FetchModeTypes)_reader.ReadByte();
				int num2 = (int)_reader.ReadByte();
				if (num2 > 0)
				{
					this.SharedWithList = new int[num2];
					for (int j = 0; j < num2; j++)
					{
						this.SharedWithList[j] = _reader.ReadInt32();
					}
					return;
				}
				this.SharedWithList = null;
				return;
			}
			case NetPackageQuestEvent.QuestEventTypes.SetupRestorePower:
			{
				this.blockIndex = _reader.ReadString();
				this.eventName = _reader.ReadString();
				int num3 = (int)_reader.ReadByte();
				if (num3 > 0)
				{
					this.SharedWithList = new int[num3];
					for (int k = 0; k < num3; k++)
					{
						this.SharedWithList[k] = _reader.ReadInt32();
					}
				}
				else
				{
					this.SharedWithList = null;
				}
				num3 = (int)_reader.ReadByte();
				this.activateList = new List<Vector3i>();
				if (num3 > 0)
				{
					for (int l = 0; l < num3; l++)
					{
						this.activateList.Add(StreamUtils.ReadVector3i(_reader));
					}
					return;
				}
				break;
			}
			default:
				if (questEventTypes != NetPackageQuestEvent.QuestEventTypes.ResetTraderQuests)
				{
					return;
				}
				this.factionPointOverride = _reader.ReadInt32();
				break;
			}
			return;
		}
		this.extraData = _reader.ReadUInt64();
	}

	// Token: 0x06003DA6 RID: 15782 RVA: 0x00187EC4 File Offset: 0x001860C4
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.entityID);
		StreamUtils.Write(_writer, this.prefabPos);
		_writer.Write((byte)this.eventType);
		_writer.Write(this.questTags.ToString());
		_writer.Write(this.questCode);
		NetPackageQuestEvent.QuestEventTypes questEventTypes = this.eventType;
		if (questEventTypes != NetPackageQuestEvent.QuestEventTypes.RallyMarkerLocked)
		{
			switch (questEventTypes)
			{
			case NetPackageQuestEvent.QuestEventTypes.LockPOI:
			{
				_writer.Write(this.questID);
				if (this.SharedWithList == null)
				{
					_writer.Write(0);
					return;
				}
				_writer.Write((byte)this.SharedWithList.Length);
				for (int i = 0; i < this.SharedWithList.Length; i++)
				{
					_writer.Write(this.SharedWithList[i]);
				}
				return;
			}
			case NetPackageQuestEvent.QuestEventTypes.UnlockPOI:
			case NetPackageQuestEvent.QuestEventTypes.ShowSleeperVolume:
			case NetPackageQuestEvent.QuestEventTypes.HideSleeperVolume:
				break;
			case NetPackageQuestEvent.QuestEventTypes.ClearSleeper:
				_writer.Write(this.SubscribeTo);
				return;
			case NetPackageQuestEvent.QuestEventTypes.SetupFetch:
			{
				_writer.Write((byte)this.FetchModeType);
				if (this.SharedWithList == null)
				{
					_writer.Write(0);
					return;
				}
				_writer.Write((byte)this.SharedWithList.Length);
				for (int j = 0; j < this.SharedWithList.Length; j++)
				{
					_writer.Write(this.SharedWithList[j]);
				}
				return;
			}
			case NetPackageQuestEvent.QuestEventTypes.SetupRestorePower:
			{
				_writer.Write(this.blockIndex);
				_writer.Write(this.eventName);
				if (this.SharedWithList == null)
				{
					_writer.Write(0);
				}
				else
				{
					_writer.Write((byte)this.SharedWithList.Length);
					for (int k = 0; k < this.SharedWithList.Length; k++)
					{
						_writer.Write(this.SharedWithList[k]);
					}
				}
				if (this.activateList == null)
				{
					_writer.Write(0);
					return;
				}
				_writer.Write((byte)this.activateList.Count);
				for (int l = 0; l < this.activateList.Count; l++)
				{
					StreamUtils.Write(_writer, this.activateList[l]);
				}
				return;
			}
			default:
				if (questEventTypes != NetPackageQuestEvent.QuestEventTypes.ResetTraderQuests)
				{
					return;
				}
				_writer.Write(this.factionPointOverride);
				break;
			}
			return;
		}
		_writer.Write(this.extraData);
	}

	// Token: 0x06003DA7 RID: 15783 RVA: 0x001880C8 File Offset: 0x001862C8
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		switch (this.eventType)
		{
		case NetPackageQuestEvent.QuestEventTypes.TryRallyMarker:
			if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
			{
				Vector2 vector = new Vector2(this.prefabPos.x, this.prefabPos.z);
				NetPackageQuestEvent.QuestEventTypes questEventTypes = NetPackageQuestEvent.QuestEventTypes.RallyMarkerActivated;
				ulong num;
				switch (QuestEventManager.Current.CheckForPOILockouts(this.entityID, vector, out num))
				{
				case QuestEventManager.POILockoutReasonTypes.PlayerInside:
					questEventTypes = NetPackageQuestEvent.QuestEventTypes.RallyMarker_PlayerLocked;
					break;
				case QuestEventManager.POILockoutReasonTypes.Bedroll:
					questEventTypes = NetPackageQuestEvent.QuestEventTypes.RallyMarker_BedrollLocked;
					break;
				case QuestEventManager.POILockoutReasonTypes.LandClaim:
					questEventTypes = NetPackageQuestEvent.QuestEventTypes.RallyMarker_LandClaimLocked;
					break;
				case QuestEventManager.POILockoutReasonTypes.QuestLock:
					questEventTypes = NetPackageQuestEvent.QuestEventTypes.RallyMarkerLocked;
					break;
				}
				SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageQuestEvent>().Setup(questEventTypes, this.entityID, this.prefabPos, this.questCode, num), false, -1, -1, -1, null, 192, false);
				return;
			}
			break;
		case NetPackageQuestEvent.QuestEventTypes.ConfirmRallyMarker:
			break;
		case NetPackageQuestEvent.QuestEventTypes.RallyMarkerActivated:
		{
			EntityPlayer entityPlayer = _world.GetEntity(this.entityID) as EntityPlayer;
			if (entityPlayer != null)
			{
				entityPlayer.QuestJournal.HandleRallyMarkerActivation(this.questCode, this.prefabPos, true, QuestEventManager.POILockoutReasonTypes.None, 0UL);
				return;
			}
			break;
		}
		case NetPackageQuestEvent.QuestEventTypes.RallyMarkerLocked:
		{
			EntityPlayer entityPlayer2 = _world.GetEntity(this.entityID) as EntityPlayer;
			if (entityPlayer2 != null)
			{
				entityPlayer2.QuestJournal.HandleRallyMarkerActivation(this.questCode, this.prefabPos, false, QuestEventManager.POILockoutReasonTypes.QuestLock, this.extraData);
				return;
			}
			break;
		}
		case NetPackageQuestEvent.QuestEventTypes.RallyMarker_PlayerLocked:
		{
			EntityPlayer entityPlayer3 = _world.GetEntity(this.entityID) as EntityPlayer;
			if (entityPlayer3 != null)
			{
				entityPlayer3.QuestJournal.HandleRallyMarkerActivation(this.questCode, this.prefabPos, false, QuestEventManager.POILockoutReasonTypes.PlayerInside, 0UL);
				return;
			}
			break;
		}
		case NetPackageQuestEvent.QuestEventTypes.RallyMarker_BedrollLocked:
		{
			EntityPlayer entityPlayer4 = _world.GetEntity(this.entityID) as EntityPlayer;
			if (entityPlayer4 != null)
			{
				entityPlayer4.QuestJournal.HandleRallyMarkerActivation(this.questCode, this.prefabPos, false, QuestEventManager.POILockoutReasonTypes.Bedroll, 0UL);
				return;
			}
			break;
		}
		case NetPackageQuestEvent.QuestEventTypes.RallyMarker_LandClaimLocked:
		{
			EntityPlayer entityPlayer5 = _world.GetEntity(this.entityID) as EntityPlayer;
			if (entityPlayer5 != null)
			{
				entityPlayer5.QuestJournal.HandleRallyMarkerActivation(this.questCode, this.prefabPos, false, QuestEventManager.POILockoutReasonTypes.LandClaim, 0UL);
				return;
			}
			break;
		}
		case NetPackageQuestEvent.QuestEventTypes.LockPOI:
			ThreadManager.StartCoroutine(QuestEventManager.Current.QuestLockPOI(this.entityID, QuestClass.GetQuest(this.questID), this.prefabPos, this.questTags, this.SharedWithList));
			return;
		case NetPackageQuestEvent.QuestEventTypes.UnlockPOI:
			QuestEventManager.Current.QuestUnlockPOI(this.entityID, this.prefabPos);
			return;
		case NetPackageQuestEvent.QuestEventTypes.ClearSleeper:
			if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
			{
				QuestEventManager.Current.ClearedSleepers(this.prefabPos);
				return;
			}
			if (this.SubscribeTo)
			{
				QuestEventManager.Current.SubscribeToUpdateEvent(this.entityID, this.prefabPos);
				return;
			}
			QuestEventManager.Current.UnSubscribeToUpdateEvent(this.entityID, this.prefabPos);
			return;
		case NetPackageQuestEvent.QuestEventTypes.ShowSleeperVolume:
			QuestEventManager.Current.SleeperVolumePositionAdded(this.prefabPos);
			return;
		case NetPackageQuestEvent.QuestEventTypes.HideSleeperVolume:
			QuestEventManager.Current.SleeperVolumePositionRemoved(this.prefabPos);
			return;
		case NetPackageQuestEvent.QuestEventTypes.SetupFetch:
		{
			if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
			{
				QuestEventManager.Current.SetupFetchForMP(this.entityID, this.prefabPos, this.FetchModeType, this.SharedWithList);
				return;
			}
			EntityPlayer entityPlayer6 = _world.GetEntity(this.entityID) as EntityPlayer;
			Quest.PositionDataTypes positionDataTypes = ((this.FetchModeType == ObjectiveFetchFromContainer.FetchModeTypes.Standard) ? Quest.PositionDataTypes.FetchContainer : Quest.PositionDataTypes.HiddenCache);
			if (entityPlayer6 != null)
			{
				entityPlayer6.QuestJournal.SetActivePositionData(positionDataTypes, new Vector3i(this.prefabPos));
				return;
			}
			break;
		}
		case NetPackageQuestEvent.QuestEventTypes.SetupRestorePower:
		{
			if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
			{
				List<Vector3i> list = new List<Vector3i>();
				QuestEventManager.Current.SetupActivateForMP(this.entityID, this.questCode, this.eventName, list, GameManager.Instance.World, this.prefabPos, this.blockIndex, this.SharedWithList);
				return;
			}
			EntityPlayer entityPlayer7 = _world.GetEntity(this.entityID) as EntityPlayer;
			if (entityPlayer7 != null)
			{
				entityPlayer7.QuestJournal.HandleRestorePowerReceived(this.prefabPos, this.activateList);
				return;
			}
			break;
		}
		case NetPackageQuestEvent.QuestEventTypes.FinishManagedQuest:
			QuestEventManager.Current.FinishManagedQuest(this.questCode, _world.GetEntity(this.entityID) as EntityPlayer);
			return;
		case NetPackageQuestEvent.QuestEventTypes.POILocked:
			if (ObjectiveRallyPoint.OutstandingRallyPoint != null)
			{
				ObjectiveRallyPoint.OutstandingRallyPoint.RallyPointActivated();
				return;
			}
			break;
		case NetPackageQuestEvent.QuestEventTypes.ResetTraderQuests:
			QuestEventManager.Current.AddTraderResetQuestsForPlayer(this.entityID, this.questCode);
			if (this.factionPointOverride != -1)
			{
				EntityPlayer entityPlayer8 = _world.GetEntity(this.entityID) as EntityPlayer;
				if (entityPlayer8 != null)
				{
					EntityTrader entityTrader = _world.GetEntity(this.questCode) as EntityTrader;
					if (entityTrader != null)
					{
						entityTrader.ClearActiveQuests(entityPlayer8.entityId);
						entityTrader.SetupActiveQuestsForPlayer(entityPlayer8, this.factionPointOverride);
					}
				}
			}
			break;
		default:
			return;
		}
	}

	// Token: 0x040031FB RID: 12795
	public int entityID;

	// Token: 0x040031FC RID: 12796
	public Vector3 prefabPos;

	// Token: 0x040031FD RID: 12797
	public FastTags<TagGroup.Global> questTags;

	// Token: 0x040031FE RID: 12798
	public NetPackageQuestEvent.QuestEventTypes eventType;

	// Token: 0x040031FF RID: 12799
	public ObjectiveFetchFromContainer.FetchModeTypes FetchModeType;

	// Token: 0x04003200 RID: 12800
	public bool SubscribeTo;

	// Token: 0x04003201 RID: 12801
	public int PartyCount;

	// Token: 0x04003202 RID: 12802
	public int questCode;

	// Token: 0x04003203 RID: 12803
	public int factionPointOverride;

	// Token: 0x04003204 RID: 12804
	public string blockIndex = "";

	// Token: 0x04003205 RID: 12805
	public string eventName = "";

	// Token: 0x04003206 RID: 12806
	public string questID;

	// Token: 0x04003207 RID: 12807
	[PublicizedFrom(EAccessModifier.Private)]
	public ulong extraData;

	// Token: 0x04003208 RID: 12808
	public List<Vector3i> activateList;

	// Token: 0x04003209 RID: 12809
	public int[] SharedWithList;

	// Token: 0x02000844 RID: 2116
	public enum QuestEventTypes
	{
		// Token: 0x0400320B RID: 12811
		TryRallyMarker,
		// Token: 0x0400320C RID: 12812
		ConfirmRallyMarker,
		// Token: 0x0400320D RID: 12813
		RallyMarkerActivated,
		// Token: 0x0400320E RID: 12814
		RallyMarkerLocked,
		// Token: 0x0400320F RID: 12815
		RallyMarker_PlayerLocked,
		// Token: 0x04003210 RID: 12816
		RallyMarker_BedrollLocked,
		// Token: 0x04003211 RID: 12817
		RallyMarker_LandClaimLocked,
		// Token: 0x04003212 RID: 12818
		LockPOI,
		// Token: 0x04003213 RID: 12819
		UnlockPOI,
		// Token: 0x04003214 RID: 12820
		ClearSleeper,
		// Token: 0x04003215 RID: 12821
		ShowSleeperVolume,
		// Token: 0x04003216 RID: 12822
		HideSleeperVolume,
		// Token: 0x04003217 RID: 12823
		SetupFetch,
		// Token: 0x04003218 RID: 12824
		SetupRestorePower,
		// Token: 0x04003219 RID: 12825
		FinishManagedQuest,
		// Token: 0x0400321A RID: 12826
		POILocked,
		// Token: 0x0400321B RID: 12827
		ResetTraderQuests
	}
}
