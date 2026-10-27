using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200081D RID: 2077
[Preserve]
public class NetPackageNPCQuestList : NetPackage
{
	// Token: 0x06003CDA RID: 15578 RVA: 0x00184D2D File Offset: 0x00182F2D
	public NetPackageNPCQuestList Setup(int _npcEntityID, int _playerEntityID)
	{
		this.npcEntityID = _npcEntityID;
		this.playerEntityID = _playerEntityID;
		this.eventType = NetPackageNPCQuestList.NPCQuestEventTypes.ResetQuests;
		return this;
	}

	// Token: 0x06003CDB RID: 15579 RVA: 0x00184D45 File Offset: 0x00182F45
	public NetPackageNPCQuestList Setup(int _playerEntityID, Vector2 _questGiverPos, int _tierLevel, Vector2 _prefabPos)
	{
		this.playerEntityID = _playerEntityID;
		this.tierLevel = _tierLevel;
		this.questGiverPos = _questGiverPos;
		this.prefabPos = _prefabPos;
		this.eventType = NetPackageNPCQuestList.NPCQuestEventTypes.AddUsedPOI;
		return this;
	}

	// Token: 0x06003CDC RID: 15580 RVA: 0x00184D6C File Offset: 0x00182F6C
	public NetPackageNPCQuestList SetupClear(int _playerEntityID, Vector2 _questGiverPos, int _tierLevel)
	{
		this.playerEntityID = _playerEntityID;
		this.tierLevel = _tierLevel;
		this.questGiverPos = _questGiverPos;
		this.eventType = NetPackageNPCQuestList.NPCQuestEventTypes.ClearUsedPOI;
		return this;
	}

	// Token: 0x06003CDD RID: 15581 RVA: 0x00184D8B File Offset: 0x00182F8B
	public NetPackageNPCQuestList Setup(int _npcEntityID, int _playerEntityID, int _tierLevel)
	{
		this.npcEntityID = _npcEntityID;
		this.playerEntityID = _playerEntityID;
		this.tierLevel = _tierLevel;
		this.eventType = NetPackageNPCQuestList.NPCQuestEventTypes.FetchList;
		return this;
	}

	// Token: 0x06003CDE RID: 15582 RVA: 0x00184DAA File Offset: 0x00182FAA
	public NetPackageNPCQuestList Setup(int _npcEntityID, int _playerEntityID, int _tierLevel, byte _removeIndex)
	{
		this.npcEntityID = _npcEntityID;
		this.playerEntityID = _playerEntityID;
		this.tierLevel = _tierLevel;
		this.eventType = NetPackageNPCQuestList.NPCQuestEventTypes.RemoveQuest;
		this.removeIndex = _removeIndex;
		return this;
	}

	// Token: 0x06003CDF RID: 15583 RVA: 0x00184DD1 File Offset: 0x00182FD1
	public NetPackageNPCQuestList Setup(int _npcEntityID, int _playerEntityID, NetPackageNPCQuestList.QuestPacketEntry[] _questPacketEntries)
	{
		this.npcEntityID = _npcEntityID;
		this.playerEntityID = _playerEntityID;
		this.questPacketEntries = _questPacketEntries;
		this.eventType = NetPackageNPCQuestList.NPCQuestEventTypes.FetchList;
		return this;
	}

	// Token: 0x06003CE0 RID: 15584 RVA: 0x00184DF0 File Offset: 0x00182FF0
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

	// Token: 0x06003CE1 RID: 15585 RVA: 0x00184EE4 File Offset: 0x001830E4
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

	// Token: 0x06003CE2 RID: 15586 RVA: 0x00184FE8 File Offset: 0x001831E8
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

	// Token: 0x06003CE3 RID: 15587 RVA: 0x001851DC File Offset: 0x001833DC
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

	// Token: 0x04003170 RID: 12656
	public int npcEntityID;

	// Token: 0x04003171 RID: 12657
	public int playerEntityID;

	// Token: 0x04003172 RID: 12658
	[PublicizedFrom(EAccessModifier.Private)]
	public NetPackageNPCQuestList.NPCQuestEventTypes eventType;

	// Token: 0x04003173 RID: 12659
	public NetPackageNPCQuestList.QuestPacketEntry[] questPacketEntries;

	// Token: 0x04003174 RID: 12660
	public int tierLevel = -1;

	// Token: 0x04003175 RID: 12661
	public byte removeIndex;

	// Token: 0x04003176 RID: 12662
	public Vector2 questGiverPos;

	// Token: 0x04003177 RID: 12663
	public Vector2 prefabPos;

	// Token: 0x0200081E RID: 2078
	[PublicizedFrom(EAccessModifier.Private)]
	public enum NPCQuestEventTypes
	{
		// Token: 0x04003179 RID: 12665
		FetchList,
		// Token: 0x0400317A RID: 12666
		RemoveQuest,
		// Token: 0x0400317B RID: 12667
		ResetQuests,
		// Token: 0x0400317C RID: 12668
		AddUsedPOI,
		// Token: 0x0400317D RID: 12669
		ClearUsedPOI
	}

	// Token: 0x0200081F RID: 2079
	public struct QuestPacketEntry
	{
		// Token: 0x06003CE5 RID: 15589 RVA: 0x001852F6 File Offset: 0x001834F6
		public void read(BinaryReader _reader)
		{
			this.QuestID = _reader.ReadString();
			this.QuestLocation = StreamUtils.ReadVector3(_reader);
			this.QuestSize = StreamUtils.ReadVector3(_reader);
			this.POIName = _reader.ReadString();
			this.TraderPos = StreamUtils.ReadVector3(_reader);
		}

		// Token: 0x06003CE6 RID: 15590 RVA: 0x00185334 File Offset: 0x00183534
		public void write(BinaryWriter _writer)
		{
			_writer.Write(this.QuestID);
			StreamUtils.Write(_writer, this.QuestLocation);
			StreamUtils.Write(_writer, this.QuestSize);
			_writer.Write(this.POIName);
			StreamUtils.Write(_writer, this.TraderPos);
		}

		// Token: 0x0400317E RID: 12670
		public string QuestID;

		// Token: 0x0400317F RID: 12671
		public Vector3 QuestLocation;

		// Token: 0x04003180 RID: 12672
		public Vector3 QuestSize;

		// Token: 0x04003181 RID: 12673
		public Vector3 TraderPos;

		// Token: 0x04003182 RID: 12674
		public string POIName;
	}
}
