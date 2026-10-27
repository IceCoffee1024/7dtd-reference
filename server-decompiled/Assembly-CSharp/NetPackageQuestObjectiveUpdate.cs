using System;
using UnityEngine.Scripting;

// Token: 0x02000847 RID: 2119
[Preserve]
public class NetPackageQuestObjectiveUpdate : NetPackage
{
	// Token: 0x06003DAE RID: 15790 RVA: 0x00188B41 File Offset: 0x00186D41
	public NetPackageQuestObjectiveUpdate Setup(NetPackageQuestObjectiveUpdate.QuestObjectiveEventTypes _eventType, int _entityID, int _questCode)
	{
		this.senderEntityID = _entityID;
		this.questCode = _questCode;
		this.eventType = _eventType;
		this.blockPos = Vector3i.zero;
		return this;
	}

	// Token: 0x06003DAF RID: 15791 RVA: 0x00188B64 File Offset: 0x00186D64
	public NetPackageQuestObjectiveUpdate Setup(NetPackageQuestObjectiveUpdate.QuestObjectiveEventTypes _eventType, int _entityID, int _questCode, Vector3i _blockPos)
	{
		this.senderEntityID = _entityID;
		this.questCode = _questCode;
		this.eventType = _eventType;
		this.blockPos = _blockPos;
		return this;
	}

	// Token: 0x06003DB0 RID: 15792 RVA: 0x00188B84 File Offset: 0x00186D84
	public override void read(PooledBinaryReader _reader)
	{
		this.senderEntityID = _reader.ReadInt32();
		this.questCode = _reader.ReadInt32();
		this.eventType = (NetPackageQuestObjectiveUpdate.QuestObjectiveEventTypes)_reader.ReadByte();
		this.blockPos = StreamUtils.ReadVector3i(_reader);
	}

	// Token: 0x06003DB1 RID: 15793 RVA: 0x00188BB6 File Offset: 0x00186DB6
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.senderEntityID);
		_writer.Write(this.questCode);
		_writer.Write((byte)this.eventType);
		StreamUtils.Write(_writer, this.blockPos);
	}

	// Token: 0x06003DB2 RID: 15794 RVA: 0x00188BF0 File Offset: 0x00186DF0
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		switch (this.eventType)
		{
		case NetPackageQuestObjectiveUpdate.QuestObjectiveEventTypes.TreasureRadiusBreak:
		{
			if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
			{
				EntityPlayerLocal primaryPlayer = _world.GetPrimaryPlayer();
				this.HandlePlayer(_world, primaryPlayer);
				return;
			}
			EntityPlayer entityPlayer = _world.GetEntity(this.senderEntityID) as EntityPlayer;
			if (entityPlayer == null || entityPlayer.Party == null)
			{
				return;
			}
			for (int i = 0; i < entityPlayer.Party.MemberList.Count; i++)
			{
				EntityPlayer entityPlayer2 = entityPlayer.Party.MemberList[i];
				if (entityPlayer2 != entityPlayer)
				{
					if (entityPlayer2 is EntityPlayerLocal)
					{
						this.HandlePlayer(_world, entityPlayer2 as EntityPlayerLocal);
					}
					else
					{
						SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageQuestObjectiveUpdate>().Setup(this.eventType, this.senderEntityID, this.questCode), false, entityPlayer2.entityId, -1, -1, null, 192, false);
					}
				}
			}
			return;
		}
		case NetPackageQuestObjectiveUpdate.QuestObjectiveEventTypes.TreasureComplete:
			if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
			{
				QuestEventManager.Current.FinishTreasureQuest(this.questCode, _world.GetEntity(this.senderEntityID) as EntityPlayer);
				return;
			}
			break;
		case NetPackageQuestObjectiveUpdate.QuestObjectiveEventTypes.BlockActivated:
			if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
			{
				EntityPlayer entityPlayer3 = _world.GetEntity(this.senderEntityID) as EntityPlayer;
				if (entityPlayer3 == null || entityPlayer3.Party == null)
				{
					return;
				}
				for (int j = 0; j < entityPlayer3.Party.MemberList.Count; j++)
				{
					EntityPlayer entityPlayer4 = entityPlayer3.Party.MemberList[j];
					if (entityPlayer4 != entityPlayer3)
					{
						if (entityPlayer4 is EntityPlayerLocal)
						{
							this.HandlePlayer(_world, entityPlayer4 as EntityPlayerLocal);
						}
						else
						{
							SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageQuestObjectiveUpdate>().Setup(this.eventType, this.senderEntityID, this.questCode, this.blockPos), false, entityPlayer4.entityId, -1, -1, null, 192, false);
						}
					}
				}
				return;
			}
			else
			{
				EntityPlayerLocal primaryPlayer2 = _world.GetPrimaryPlayer();
				this.HandlePlayer(_world, primaryPlayer2);
			}
			break;
		default:
			return;
		}
	}

	// Token: 0x06003DB3 RID: 15795 RVA: 0x00188E10 File Offset: 0x00187010
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandlePlayer(World _world, EntityPlayerLocal localPlayer)
	{
		if (localPlayer == null)
		{
			Log.Warning(string.Format("HandlePlayer {0} {1} with no active local player", "NetPackageQuestObjectiveUpdate", this.eventType));
			return;
		}
		EntityPlayer entityPlayer = _world.GetEntity(this.senderEntityID) as EntityPlayer;
		Quest quest = localPlayer.QuestJournal.FindActiveQuest(this.questCode);
		if (quest != null && entityPlayer.GetDistance(localPlayer) < 15f)
		{
			NetPackageQuestObjectiveUpdate.QuestObjectiveEventTypes questObjectiveEventTypes = this.eventType;
			if (questObjectiveEventTypes == NetPackageQuestObjectiveUpdate.QuestObjectiveEventTypes.TreasureRadiusBreak)
			{
				for (int i = 0; i < quest.Objectives.Count; i++)
				{
					if (!quest.Objectives[i].Complete && quest.Objectives[i] is ObjectiveTreasureChest)
					{
						(quest.Objectives[i] as ObjectiveTreasureChest).AddToDestroyCount();
						return;
					}
				}
				return;
			}
			if (questObjectiveEventTypes != NetPackageQuestObjectiveUpdate.QuestObjectiveEventTypes.BlockActivated)
			{
				return;
			}
			for (int j = 0; j < quest.Objectives.Count; j++)
			{
				if (!quest.Objectives[j].Complete && quest.Objectives[j] is ObjectivePOIBlockActivate)
				{
					(quest.Objectives[j] as ObjectivePOIBlockActivate).AddActivatedBlock(this.blockPos);
					return;
				}
			}
		}
	}

	// Token: 0x0400322A RID: 12842
	[PublicizedFrom(EAccessModifier.Private)]
	public int senderEntityID;

	// Token: 0x0400322B RID: 12843
	[PublicizedFrom(EAccessModifier.Private)]
	public int questCode;

	// Token: 0x0400322C RID: 12844
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3i blockPos;

	// Token: 0x0400322D RID: 12845
	[PublicizedFrom(EAccessModifier.Private)]
	public NetPackageQuestObjectiveUpdate.QuestObjectiveEventTypes eventType;

	// Token: 0x02000848 RID: 2120
	public enum QuestObjectiveEventTypes
	{
		// Token: 0x0400322F RID: 12847
		TreasureRadiusBreak,
		// Token: 0x04003230 RID: 12848
		TreasureComplete,
		// Token: 0x04003231 RID: 12849
		BlockActivated
	}
}
