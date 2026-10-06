using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000813 RID: 2067
[Preserve]
public class NetPackageNPCQuestList : NetPackage
{
	// Token: 0x06003CB7 RID: 15543 RVA: 0x00183C29 File Offset: 0x00181E29
	public NetPackageNPCQuestList Setup(int _npcEntityID, int _playerEntityID)
	{
		this.npcEntityID = _npcEntityID;
		this.playerEntityID = _playerEntityID;
		this.eventType = NetPackageNPCQuestList.NPCQuestEventTypes.ResetQuests;
		return this;
	}

	// Token: 0x06003CB8 RID: 15544 RVA: 0x00183C41 File Offset: 0x00181E41
	public NetPackageNPCQuestList Setup(int _playerEntityID, Vector2 _questGiverPos, int _tierLevel, Vector2 _prefabPos)
	{
		this.playerEntityID = _playerEntityID;
		this.tierLevel = _tierLevel;
		this.questGiverPos = _questGiverPos;
		this.prefabPos = _prefabPos;
		this.eventType = NetPackageNPCQuestList.NPCQuestEventTypes.AddUsedPOI;
		return this;
	}

	// Token: 0x06003CB9 RID: 15545 RVA: 0x00183C68 File Offset: 0x00181E68
	public NetPackageNPCQuestList SetupClear(int _playerEntityID, Vector2 _questGiverPos, int _tierLevel)
	{
		this.playerEntityID = _playerEntityID;
		this.tierLevel = _tierLevel;
		this.questGiverPos = _questGiverPos;
		this.eventType = NetPackageNPCQuestList.NPCQuestEventTypes.ClearUsedPOI;
		return this;
	}

	// Token: 0x06003CBA RID: 15546 RVA: 0x00183C87 File Offset: 0x00181E87
	public NetPackageNPCQuestList Setup(int _npcEntityID, int _playerEntityID, int _tierLevel)
	{
		this.npcEntityID = _npcEntityID;
		this.playerEntityID = _playerEntityID;
		this.tierLevel = _tierLevel;
		this.eventType = NetPackageNPCQuestList.NPCQuestEventTypes.FetchList;
		return this;
	}

	// Token: 0x06003CBB RID: 15547 RVA: 0x00183CA6 File Offset: 0x00181EA6
	public NetPackageNPCQuestList Setup(int _npcEntityID, int _playerEntityID, int _tierLevel, byte _removeIndex)
	{
		this.npcEntityID = _npcEntityID;
		this.playerEntityID = _playerEntityID;
		this.tierLevel = _tierLevel;
		this.eventType = NetPackageNPCQuestList.NPCQuestEventTypes.RemoveQuest;
		this.removeIndex = _removeIndex;
		return this;
	}

	// Token: 0x06003CBC RID: 15548 RVA: 0x00183CCD File Offset: 0x00181ECD
	public NetPackageNPCQuestList Setup(int _npcEntityID, int _playerEntityID, NetPackageNPCQuestList.QuestPacketEntry[] _questPacketEntries)
	{
		this.npcEntityID = _npcEntityID;
		this.playerEntityID = _playerEntityID;
		this.questPacketEntries = _questPacketEntries;
		this.eventType = NetPackageNPCQuestList.NPCQuestEventTypes.FetchList;
		return this;
	}

	// Token: 0x06003CBD RID: 15549 RVA: 0x00183CEC File Offset: 0x00181EEC
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

	// Token: 0x06003CBE RID: 15550 RVA: 0x00183DE0 File Offset: 0x00181FE0
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

	// Token: 0x06003CBF RID: 15551 RVA: 0x00183EE4 File Offset: 0x001820E4
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

	// Token: 0x06003CC0 RID: 15552 RVA: 0x001840D8 File Offset: 0x001822D8
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

	// Token: 0x04003150 RID: 12624
	public int npcEntityID;

	// Token: 0x04003151 RID: 12625
	public int playerEntityID;

	// Token: 0x04003152 RID: 12626
	[PublicizedFrom(EAccessModifier.Private)]
	public NetPackageNPCQuestList.NPCQuestEventTypes eventType;

	// Token: 0x04003153 RID: 12627
	public NetPackageNPCQuestList.QuestPacketEntry[] questPacketEntries;

	// Token: 0x04003154 RID: 12628
	public int tierLevel = -1;

	// Token: 0x04003155 RID: 12629
	public byte removeIndex;

	// Token: 0x04003156 RID: 12630
	public Vector2 questGiverPos;

	// Token: 0x04003157 RID: 12631
	public Vector2 prefabPos;

	// Token: 0x02000814 RID: 2068
	[PublicizedFrom(EAccessModifier.Private)]
	public enum NPCQuestEventTypes
	{
		// Token: 0x04003159 RID: 12633
		FetchList,
		// Token: 0x0400315A RID: 12634
		RemoveQuest,
		// Token: 0x0400315B RID: 12635
		ResetQuests,
		// Token: 0x0400315C RID: 12636
		AddUsedPOI,
		// Token: 0x0400315D RID: 12637
		ClearUsedPOI
	}

	// Token: 0x02000815 RID: 2069
	public struct QuestPacketEntry
	{
		// Token: 0x06003CC2 RID: 15554 RVA: 0x001841F2 File Offset: 0x001823F2
		public void read(BinaryReader _reader)
		{
			this.QuestID = _reader.ReadString();
			this.QuestLocation = StreamUtils.ReadVector3(_reader);
			this.QuestSize = StreamUtils.ReadVector3(_reader);
			this.POIName = _reader.ReadString();
			this.TraderPos = StreamUtils.ReadVector3(_reader);
		}

		// Token: 0x06003CC3 RID: 15555 RVA: 0x00184230 File Offset: 0x00182430
		public void write(BinaryWriter _writer)
		{
			_writer.Write(this.QuestID);
			StreamUtils.Write(_writer, this.QuestLocation);
			StreamUtils.Write(_writer, this.QuestSize);
			_writer.Write(this.POIName);
			StreamUtils.Write(_writer, this.TraderPos);
		}

		// Token: 0x0400315E RID: 12638
		public string QuestID;

		// Token: 0x0400315F RID: 12639
		public Vector3 QuestLocation;

		// Token: 0x04003160 RID: 12640
		public Vector3 QuestSize;

		// Token: 0x04003161 RID: 12641
		public Vector3 TraderPos;

		// Token: 0x04003162 RID: 12642
		public string POIName;
	}
}
