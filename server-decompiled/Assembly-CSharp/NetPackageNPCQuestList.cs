using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200080A RID: 2058
[Preserve]
public class NetPackageNPCQuestList : NetPackage
{
	// Token: 0x06003C90 RID: 15504 RVA: 0x00181C75 File Offset: 0x0017FE75
	public NetPackageNPCQuestList Setup(int _npcEntityID, int _playerEntityID)
	{
		this.npcEntityID = _npcEntityID;
		this.playerEntityID = _playerEntityID;
		this.eventType = NetPackageNPCQuestList.NPCQuestEventTypes.ResetQuests;
		return this;
	}

	// Token: 0x06003C91 RID: 15505 RVA: 0x00181C8D File Offset: 0x0017FE8D
	public NetPackageNPCQuestList Setup(int _playerEntityID, Vector2 _questGiverPos, int _tierLevel, Vector2 _prefabPos)
	{
		this.playerEntityID = _playerEntityID;
		this.tierLevel = _tierLevel;
		this.questGiverPos = _questGiverPos;
		this.prefabPos = _prefabPos;
		this.eventType = NetPackageNPCQuestList.NPCQuestEventTypes.AddUsedPOI;
		return this;
	}

	// Token: 0x06003C92 RID: 15506 RVA: 0x00181CB4 File Offset: 0x0017FEB4
	public NetPackageNPCQuestList SetupClear(int _playerEntityID, Vector2 _questGiverPos, int _tierLevel)
	{
		this.playerEntityID = _playerEntityID;
		this.tierLevel = _tierLevel;
		this.questGiverPos = _questGiverPos;
		this.eventType = NetPackageNPCQuestList.NPCQuestEventTypes.ClearUsedPOI;
		return this;
	}

	// Token: 0x06003C93 RID: 15507 RVA: 0x00181CD3 File Offset: 0x0017FED3
	public NetPackageNPCQuestList Setup(int _npcEntityID, int _playerEntityID, int _tierLevel)
	{
		this.npcEntityID = _npcEntityID;
		this.playerEntityID = _playerEntityID;
		this.tierLevel = _tierLevel;
		this.eventType = NetPackageNPCQuestList.NPCQuestEventTypes.FetchList;
		return this;
	}

	// Token: 0x06003C94 RID: 15508 RVA: 0x00181CF2 File Offset: 0x0017FEF2
	public NetPackageNPCQuestList Setup(int _npcEntityID, int _playerEntityID, int _tierLevel, byte _removeIndex)
	{
		this.npcEntityID = _npcEntityID;
		this.playerEntityID = _playerEntityID;
		this.tierLevel = _tierLevel;
		this.eventType = NetPackageNPCQuestList.NPCQuestEventTypes.RemoveQuest;
		this.removeIndex = _removeIndex;
		return this;
	}

	// Token: 0x06003C95 RID: 15509 RVA: 0x00181D19 File Offset: 0x0017FF19
	public NetPackageNPCQuestList Setup(int _npcEntityID, int _playerEntityID, NetPackageNPCQuestList.QuestPacketEntry[] _questPacketEntries)
	{
		this.npcEntityID = _npcEntityID;
		this.playerEntityID = _playerEntityID;
		this.questPacketEntries = _questPacketEntries;
		this.eventType = NetPackageNPCQuestList.NPCQuestEventTypes.FetchList;
		return this;
	}

	// Token: 0x06003C96 RID: 15510 RVA: 0x00181D38 File Offset: 0x0017FF38
	public override void read(PooledBinaryReader _reader)
	{
		this.npcEntityID = _reader.ReadInt32();
		this.playerEntityID = _reader.ReadInt32();
		this.eventType = (NetPackageNPCQuestList.NPCQuestEventTypes)_reader.ReadByte();
		if (this.eventType == NetPackageNPCQuestList.NPCQuestEventTypes.FetchList)
		{
			this.tierLevel = _reader.ReadInt32();
			int num = _reader.ReadInt32();
			if (num > 0)
			{
				this.questPacketEntries = new NetPackageNPCQuestList.QuestPacketEntry[num];
				for (int i = 0; i < num; i++)
				{
					this.questPacketEntries[i].read(_reader);
				}
				return;
			}
			this.questPacketEntries = null;
			return;
		}
		else
		{
			if (this.eventType == NetPackageNPCQuestList.NPCQuestEventTypes.RemoveQuest)
			{
				this.tierLevel = _reader.ReadInt32();
				this.removeIndex = _reader.ReadByte();
				return;
			}
			if (this.eventType == NetPackageNPCQuestList.NPCQuestEventTypes.AddUsedPOI)
			{
				this.tierLevel = _reader.ReadInt32();
				this.questGiverPos = StreamUtils.ReadVector2(_reader);
				this.prefabPos = StreamUtils.ReadVector2(_reader);
				return;
			}
			if (this.eventType == NetPackageNPCQuestList.NPCQuestEventTypes.ClearUsedPOI)
			{
				this.tierLevel = _reader.ReadInt32();
				this.questGiverPos = StreamUtils.ReadVector2(_reader);
			}
			return;
		}
	}

	// Token: 0x06003C97 RID: 15511 RVA: 0x00181E2C File Offset: 0x0018002C
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.npcEntityID);
		_writer.Write(this.playerEntityID);
		_writer.Write((byte)this.eventType);
		if (this.eventType == NetPackageNPCQuestList.NPCQuestEventTypes.FetchList)
		{
			_writer.Write(this.tierLevel);
			if (this.questPacketEntries != null)
			{
				_writer.Write(this.questPacketEntries.Length);
				for (int i = 0; i < this.questPacketEntries.Length; i++)
				{
					this.questPacketEntries[i].write(_writer);
				}
				return;
			}
			_writer.Write(0);
			return;
		}
		else
		{
			if (this.eventType == NetPackageNPCQuestList.NPCQuestEventTypes.RemoveQuest)
			{
				_writer.Write(this.tierLevel);
				_writer.Write(this.removeIndex);
				return;
			}
			if (this.eventType == NetPackageNPCQuestList.NPCQuestEventTypes.AddUsedPOI)
			{
				_writer.Write(this.tierLevel);
				StreamUtils.Write(_writer, this.questGiverPos);
				StreamUtils.Write(_writer, this.prefabPos);
				return;
			}
			if (this.eventType == NetPackageNPCQuestList.NPCQuestEventTypes.ClearUsedPOI)
			{
				_writer.Write(this.tierLevel);
				StreamUtils.Write(_writer, this.questGiverPos);
			}
			return;
		}
	}

	// Token: 0x06003C98 RID: 15512 RVA: 0x00181F30 File Offset: 0x00180130
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			EntityPlayer entityPlayer = _world.GetEntity(this.playerEntityID) as EntityPlayer;
			if (this.eventType == NetPackageNPCQuestList.NPCQuestEventTypes.AddUsedPOI)
			{
				entityPlayer.QuestJournal.AddPOIToTraderData(this.tierLevel, this.questGiverPos, this.prefabPos);
				return;
			}
			EntityTrader entityTrader = _world.GetEntity(this.npcEntityID) as EntityTrader;
			entityTrader.activeQuests = QuestEventManager.Current.GetQuestList(_world, this.npcEntityID, this.playerEntityID);
			if (entityTrader.activeQuests == null)
			{
				entityTrader.activeQuests = entityTrader.PopulateActiveQuests(entityPlayer, this.tierLevel, -1);
			}
			QuestEventManager.Current.SetupQuestList(entityTrader, this.playerEntityID, entityTrader.activeQuests);
			if (this.eventType == NetPackageNPCQuestList.NPCQuestEventTypes.FetchList)
			{
				NetPackageNPCQuestList.SendQuestPacketsToPlayer(entityTrader, this.playerEntityID);
				return;
			}
			if (this.eventType == NetPackageNPCQuestList.NPCQuestEventTypes.RemoveQuest)
			{
				List<Quest> questList = QuestEventManager.Current.GetQuestList(_world, this.npcEntityID, this.playerEntityID);
				int num = 0;
				for (int i = 0; i < questList.Count; i++)
				{
					if ((int)questList[i].QuestClass.DifficultyTier == this.tierLevel)
					{
						if (num == (int)this.removeIndex)
						{
							questList.RemoveAt(i);
							break;
						}
						num++;
					}
				}
				QuestEventManager.Current.SetupQuestList(entityTrader, this.playerEntityID, questList);
				return;
			}
			QuestEventManager.Current.ClearQuestList(this.npcEntityID);
			Log.Out(string.Concat(new string[]
			{
				"Quests Reset for NPC: ",
				this.npcEntityID.ToString(),
				" by Player: ",
				this.playerEntityID.ToString(),
				"."
			}));
			return;
		}
		else
		{
			EntityPlayer entityPlayer2 = _world.GetEntity(this.playerEntityID) as EntityPlayer;
			if (this.eventType == NetPackageNPCQuestList.NPCQuestEventTypes.ClearUsedPOI)
			{
				entityPlayer2.QuestJournal.ClearTraderDataTier(this.tierLevel, this.questGiverPos);
				return;
			}
			(_world.GetEntity(this.npcEntityID) as EntityTrader).SetActiveQuests(entityPlayer2, this.questPacketEntries);
			return;
		}
	}

	// Token: 0x06003C99 RID: 15513 RVA: 0x00182124 File Offset: 0x00180324
	public static void SendQuestPacketsToPlayer(EntityTrader npc, int playerEntityID)
	{
		if (npc.activeQuests != null)
		{
			int count = npc.activeQuests.Count;
			NetPackageNPCQuestList.QuestPacketEntry[] array = new NetPackageNPCQuestList.QuestPacketEntry[count];
			for (int i = 0; i < count; i++)
			{
				Quest quest = npc.activeQuests[i];
				Vector3 vector = ((npc.traderArea != null) ? npc.traderArea.Position : npc.position);
				array[i].QuestID = quest.ID;
				array[i].QuestLocation = quest.GetLocation();
				array[i].QuestSize = quest.GetLocationSize();
				array[i].POIName = ((quest.QuestPrefab != null) ? quest.QuestPrefab.location.Name : "UNNAMED");
				array[i].TraderPos = vector;
			}
			SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageNPCQuestList>().Setup(npc.entityId, playerEntityID, array), false, playerEntityID, -1, -1, null, 192, false);
		}
	}

	// Token: 0x06003C9A RID: 15514 RVA: 0x000B8CB1 File Offset: 0x000B6EB1
	public override int GetLength()
	{
		return 20;
	}

	// Token: 0x040030ED RID: 12525
	public int npcEntityID;

	// Token: 0x040030EE RID: 12526
	public int playerEntityID;

	// Token: 0x040030EF RID: 12527
	[PublicizedFrom(EAccessModifier.Private)]
	public NetPackageNPCQuestList.NPCQuestEventTypes eventType;

	// Token: 0x040030F0 RID: 12528
	public NetPackageNPCQuestList.QuestPacketEntry[] questPacketEntries;

	// Token: 0x040030F1 RID: 12529
	public int tierLevel = -1;

	// Token: 0x040030F2 RID: 12530
	public byte removeIndex;

	// Token: 0x040030F3 RID: 12531
	public Vector2 questGiverPos;

	// Token: 0x040030F4 RID: 12532
	public Vector2 prefabPos;

	// Token: 0x0200080B RID: 2059
	[PublicizedFrom(EAccessModifier.Private)]
	public enum NPCQuestEventTypes
	{
		// Token: 0x040030F6 RID: 12534
		FetchList,
		// Token: 0x040030F7 RID: 12535
		RemoveQuest,
		// Token: 0x040030F8 RID: 12536
		ResetQuests,
		// Token: 0x040030F9 RID: 12537
		AddUsedPOI,
		// Token: 0x040030FA RID: 12538
		ClearUsedPOI
	}

	// Token: 0x0200080C RID: 2060
	public struct QuestPacketEntry
	{
		// Token: 0x06003C9C RID: 15516 RVA: 0x0018223E File Offset: 0x0018043E
		public void read(BinaryReader _reader)
		{
			this.QuestID = _reader.ReadString();
			this.QuestLocation = StreamUtils.ReadVector3(_reader);
			this.QuestSize = StreamUtils.ReadVector3(_reader);
			this.POIName = _reader.ReadString();
			this.TraderPos = StreamUtils.ReadVector3(_reader);
		}

		// Token: 0x06003C9D RID: 15517 RVA: 0x0018227C File Offset: 0x0018047C
		public void write(BinaryWriter _writer)
		{
			_writer.Write(this.QuestID);
			StreamUtils.Write(_writer, this.QuestLocation);
			StreamUtils.Write(_writer, this.QuestSize);
			_writer.Write(this.POIName);
			StreamUtils.Write(_writer, this.TraderPos);
		}

		// Token: 0x040030FB RID: 12539
		public string QuestID;

		// Token: 0x040030FC RID: 12540
		public Vector3 QuestLocation;

		// Token: 0x040030FD RID: 12541
		public Vector3 QuestSize;

		// Token: 0x040030FE RID: 12542
		public Vector3 TraderPos;

		// Token: 0x040030FF RID: 12543
		public string POIName;
	}
}
