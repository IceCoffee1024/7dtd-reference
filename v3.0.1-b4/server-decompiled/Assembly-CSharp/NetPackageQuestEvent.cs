using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200082C RID: 2092
[Preserve]
public class NetPackageQuestEvent : NetPackage
{
	// Token: 0x06003D5C RID: 15708 RVA: 0x00184ECE File Offset: 0x001830CE
	public NetPackageQuestEvent Setup(NetPackageQuestEvent.QuestEventTypes _eventType, int _entityID)
	{
		this.eventType = _eventType;
		this.entityID = _entityID;
		return this;
	}

	// Token: 0x06003D5D RID: 15709 RVA: 0x00184EDF File Offset: 0x001830DF
	public NetPackageQuestEvent Setup(NetPackageQuestEvent.QuestEventTypes _eventType, int _entityID, int _traderID, int _overrideFactionPoints)
	{
		this.eventType = _eventType;
		this.entityID = _entityID;
		this.questCode = _traderID;
		this.factionPointOverride = _overrideFactionPoints;
		return this;
	}

	// Token: 0x06003D5E RID: 15710 RVA: 0x00184EFF File Offset: 0x001830FF
	public NetPackageQuestEvent Setup(NetPackageQuestEvent.QuestEventTypes _eventType, int _entityID, Vector3 _prefabPos, int _questCode)
	{
		this.entityID = _entityID;
		this.prefabPos = _prefabPos;
		this.eventType = _eventType;
		this.questCode = _questCode;
		return this;
	}

	// Token: 0x06003D5F RID: 15711 RVA: 0x00184F1F File Offset: 0x0018311F
	public NetPackageQuestEvent Setup(NetPackageQuestEvent.QuestEventTypes _eventType, int _entityID, Vector3 _prefabPos, int _questCode, ulong _extraData)
	{
		this.entityID = _entityID;
		this.prefabPos = _prefabPos;
		this.eventType = _eventType;
		this.questCode = _questCode;
		this.extraData = _extraData;
		return this;
	}

	// Token: 0x06003D60 RID: 15712 RVA: 0x00184F47 File Offset: 0x00183147
	public NetPackageQuestEvent Setup(NetPackageQuestEvent.QuestEventTypes _eventType, int _entityID, Vector3 _prefabPos)
	{
		this.entityID = _entityID;
		this.prefabPos = _prefabPos;
		this.eventType = _eventType;
		return this;
	}

	// Token: 0x06003D61 RID: 15713 RVA: 0x00184F5F File Offset: 0x0018315F
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

	// Token: 0x06003D62 RID: 15714 RVA: 0x00184F8F File Offset: 0x0018318F
	public NetPackageQuestEvent Setup(NetPackageQuestEvent.QuestEventTypes _eventType, int _entityID, Vector3 _prefabPos, ObjectiveFetchFromContainer.FetchModeTypes _fetchModeType)
	{
		this.entityID = _entityID;
		this.prefabPos = _prefabPos;
		this.eventType = _eventType;
		this.FetchModeType = _fetchModeType;
		return this;
	}

	// Token: 0x06003D63 RID: 15715 RVA: 0x00184FAF File Offset: 0x001831AF
	public NetPackageQuestEvent Setup(NetPackageQuestEvent.QuestEventTypes _eventType, int _entityID, Vector3 _prefabPos, ObjectiveFetchFromContainer.FetchModeTypes _fetchModeType, int[] _sharedWithList)
	{
		this.entityID = _entityID;
		this.prefabPos = _prefabPos;
		this.eventType = _eventType;
		this.FetchModeType = _fetchModeType;
		this.SharedWithList = _sharedWithList;
		return this;
	}

	// Token: 0x06003D64 RID: 15716 RVA: 0x00184FD7 File Offset: 0x001831D7
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

	// Token: 0x06003D65 RID: 15717 RVA: 0x0018500F File Offset: 0x0018320F
	public NetPackageQuestEvent Setup(NetPackageQuestEvent.QuestEventTypes _eventType, int _entityID, Vector3 _prefabPos, bool _subscribeTo)
	{
		this.entityID = _entityID;
		this.prefabPos = _prefabPos;
		this.eventType = _eventType;
		this.SubscribeTo = _subscribeTo;
		return this;
	}

	// Token: 0x06003D66 RID: 15718 RVA: 0x0018502F File Offset: 0x0018322F
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

	// Token: 0x06003D67 RID: 15719 RVA: 0x0018505F File Offset: 0x0018325F
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

	// Token: 0x06003D68 RID: 15720 RVA: 0x00185090 File Offset: 0x00183290
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

	// Token: 0x06003D69 RID: 15721 RVA: 0x00185268 File Offset: 0x00183468
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

	// Token: 0x06003D6A RID: 15722 RVA: 0x0018546C File Offset: 0x0018366C
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

	// Token: 0x06003D6B RID: 15723 RVA: 0x000B8CB1 File Offset: 0x000B6EB1
	public override int GetLength()
	{
		return 20;
	}

	// Token: 0x04003175 RID: 12661
	public int entityID;

	// Token: 0x04003176 RID: 12662
	public Vector3 prefabPos;

	// Token: 0x04003177 RID: 12663
	public FastTags<TagGroup.Global> questTags;

	// Token: 0x04003178 RID: 12664
	public NetPackageQuestEvent.QuestEventTypes eventType;

	// Token: 0x04003179 RID: 12665
	public ObjectiveFetchFromContainer.FetchModeTypes FetchModeType;

	// Token: 0x0400317A RID: 12666
	public bool SubscribeTo;

	// Token: 0x0400317B RID: 12667
	public int PartyCount;

	// Token: 0x0400317C RID: 12668
	public int questCode;

	// Token: 0x0400317D RID: 12669
	public int factionPointOverride;

	// Token: 0x0400317E RID: 12670
	public string blockIndex = "";

	// Token: 0x0400317F RID: 12671
	public string eventName = "";

	// Token: 0x04003180 RID: 12672
	public string questID;

	// Token: 0x04003181 RID: 12673
	[PublicizedFrom(EAccessModifier.Private)]
	public ulong extraData;

	// Token: 0x04003182 RID: 12674
	public List<Vector3i> activateList;

	// Token: 0x04003183 RID: 12675
	public int[] SharedWithList;

	// Token: 0x0200082D RID: 2093
	public enum QuestEventTypes
	{
		// Token: 0x04003185 RID: 12677
		TryRallyMarker,
		// Token: 0x04003186 RID: 12678
		ConfirmRallyMarker,
		// Token: 0x04003187 RID: 12679
		RallyMarkerActivated,
		// Token: 0x04003188 RID: 12680
		RallyMarkerLocked,
		// Token: 0x04003189 RID: 12681
		RallyMarker_PlayerLocked,
		// Token: 0x0400318A RID: 12682
		RallyMarker_BedrollLocked,
		// Token: 0x0400318B RID: 12683
		RallyMarker_LandClaimLocked,
		// Token: 0x0400318C RID: 12684
		LockPOI,
		// Token: 0x0400318D RID: 12685
		UnlockPOI,
		// Token: 0x0400318E RID: 12686
		ClearSleeper,
		// Token: 0x0400318F RID: 12687
		ShowSleeperVolume,
		// Token: 0x04003190 RID: 12688
		HideSleeperVolume,
		// Token: 0x04003191 RID: 12689
		SetupFetch,
		// Token: 0x04003192 RID: 12690
		SetupRestorePower,
		// Token: 0x04003193 RID: 12691
		FinishManagedQuest,
		// Token: 0x04003194 RID: 12692
		POILocked,
		// Token: 0x04003195 RID: 12693
		ResetTraderQuests
	}
}
