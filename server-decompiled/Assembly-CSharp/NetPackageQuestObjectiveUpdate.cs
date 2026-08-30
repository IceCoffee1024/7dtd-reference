using System;
using UnityEngine.Scripting;

// Token: 0x02000830 RID: 2096
[Preserve]
public class NetPackageQuestObjectiveUpdate : NetPackage
{
	// Token: 0x06003D73 RID: 15731 RVA: 0x00185EE5 File Offset: 0x001840E5
	public NetPackageQuestObjectiveUpdate Setup(NetPackageQuestObjectiveUpdate.QuestObjectiveEventTypes _eventType, int _entityID, int _questCode)
	{
		this.senderEntityID = _entityID;
		this.questCode = _questCode;
		this.eventType = _eventType;
		this.blockPos = Vector3i.zero;
		return this;
	}

	// Token: 0x06003D74 RID: 15732 RVA: 0x00185F08 File Offset: 0x00184108
	public NetPackageQuestObjectiveUpdate Setup(NetPackageQuestObjectiveUpdate.QuestObjectiveEventTypes _eventType, int _entityID, int _questCode, Vector3i _blockPos)
	{
		this.senderEntityID = _entityID;
		this.questCode = _questCode;
		this.eventType = _eventType;
		this.blockPos = _blockPos;
		return this;
	}

	// Token: 0x06003D75 RID: 15733 RVA: 0x00185F28 File Offset: 0x00184128
	public override void read(PooledBinaryReader _reader)
	{
		this.senderEntityID = _reader.ReadInt32();
		this.questCode = _reader.ReadInt32();
		this.eventType = (NetPackageQuestObjectiveUpdate.QuestObjectiveEventTypes)_reader.ReadByte();
		this.blockPos = StreamUtils.ReadVector3i(_reader);
	}

	// Token: 0x06003D76 RID: 15734 RVA: 0x00185F5A File Offset: 0x0018415A
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.senderEntityID);
		_writer.Write(this.questCode);
		_writer.Write((byte)this.eventType);
		StreamUtils.Write(_writer, this.blockPos);
	}

	// Token: 0x06003D77 RID: 15735 RVA: 0x00185F94 File Offset: 0x00184194
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

	// Token: 0x06003D78 RID: 15736 RVA: 0x001861B4 File Offset: 0x001843B4
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

	// Token: 0x06003D79 RID: 15737 RVA: 0x000B8CB1 File Offset: 0x000B6EB1
	public override int GetLength()
	{
		return 20;
	}

	// Token: 0x040031A4 RID: 12708
	[PublicizedFrom(EAccessModifier.Private)]
	public int senderEntityID;

	// Token: 0x040031A5 RID: 12709
	[PublicizedFrom(EAccessModifier.Private)]
	public int questCode;

	// Token: 0x040031A6 RID: 12710
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3i blockPos;

	// Token: 0x040031A7 RID: 12711
	[PublicizedFrom(EAccessModifier.Private)]
	public NetPackageQuestObjectiveUpdate.QuestObjectiveEventTypes eventType;

	// Token: 0x02000831 RID: 2097
	public enum QuestObjectiveEventTypes
	{
		// Token: 0x040031A9 RID: 12713
		TreasureRadiusBreak,
		// Token: 0x040031AA RID: 12714
		TreasureComplete,
		// Token: 0x040031AB RID: 12715
		BlockActivated
	}
}
