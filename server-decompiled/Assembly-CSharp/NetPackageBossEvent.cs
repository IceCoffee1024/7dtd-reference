using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020007B3 RID: 1971
[Preserve]
public class NetPackageBossEvent : NetPackage
{
	// Token: 0x06003A99 RID: 15001 RVA: 0x0017C135 File Offset: 0x0017A335
	public NetPackageBossEvent Setup(NetPackageBossEvent.BossEventTypes _eventType, int _bossGroupID)
	{
		this.bossGroupID = _bossGroupID;
		this.entityID = -1;
		this.minionIDs = null;
		this.eventType = _eventType;
		this.bossIcon1 = "";
		return this;
	}

	// Token: 0x06003A9A RID: 15002 RVA: 0x0017C15F File Offset: 0x0017A35F
	public NetPackageBossEvent Setup(NetPackageBossEvent.BossEventTypes _eventType, int _bossGroupID, BossGroup.BossGroupTypes _bossGroupType)
	{
		this.bossGroupID = _bossGroupID;
		this.bossGroupType = _bossGroupType;
		this.entityID = -1;
		this.minionIDs = null;
		this.eventType = _eventType;
		this.bossIcon1 = "";
		return this;
	}

	// Token: 0x06003A9B RID: 15003 RVA: 0x0017C190 File Offset: 0x0017A390
	public NetPackageBossEvent Setup(NetPackageBossEvent.BossEventTypes _eventType, int _bossGroupID, int _entityID)
	{
		this.bossGroupID = _bossGroupID;
		this.entityID = _entityID;
		this.minionIDs = null;
		this.eventType = _eventType;
		this.bossIcon1 = "";
		return this;
	}

	// Token: 0x06003A9C RID: 15004 RVA: 0x0017C1BA File Offset: 0x0017A3BA
	public NetPackageBossEvent Setup(NetPackageBossEvent.BossEventTypes _eventType, int _bossGroupID, BossGroup.BossGroupTypes _bossGroupType, int _bossID, List<int> _minionIDs, string _bossIcon1)
	{
		this.bossGroupID = _bossGroupID;
		this.bossGroupType = _bossGroupType;
		this.entityID = _bossID;
		this.minionIDs = _minionIDs;
		this.eventType = _eventType;
		this.bossIcon1 = _bossIcon1;
		return this;
	}

	// Token: 0x06003A9D RID: 15005 RVA: 0x0017C1EC File Offset: 0x0017A3EC
	public override void read(PooledBinaryReader _reader)
	{
		this.bossGroupID = _reader.ReadInt32();
		this.eventType = (NetPackageBossEvent.BossEventTypes)_reader.ReadByte();
		this.bossGroupType = (BossGroup.BossGroupTypes)_reader.ReadByte();
		this.entityID = _reader.ReadInt32();
		this.bossIcon1 = _reader.ReadString();
		if (this.eventType == NetPackageBossEvent.BossEventTypes.AddGroup)
		{
			int num = _reader.ReadInt32();
			this.minionIDs = new List<int>();
			for (int i = 0; i < num; i++)
			{
				this.minionIDs.Add(_reader.ReadInt32());
			}
		}
	}

	// Token: 0x06003A9E RID: 15006 RVA: 0x0017C270 File Offset: 0x0017A470
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.bossGroupID);
		_writer.Write((byte)this.eventType);
		_writer.Write((byte)this.bossGroupType);
		_writer.Write(this.entityID);
		_writer.Write(this.bossIcon1);
		if (this.eventType == NetPackageBossEvent.BossEventTypes.AddGroup)
		{
			_writer.Write(this.minionIDs.Count);
			for (int i = 0; i < this.minionIDs.Count; i++)
			{
				_writer.Write(this.minionIDs[i]);
			}
		}
	}

	// Token: 0x06003A9F RID: 15007 RVA: 0x0017C304 File Offset: 0x0017A504
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		switch (this.eventType)
		{
		case NetPackageBossEvent.BossEventTypes.RequestGroups:
			GameEventManager.Current.SendBossGroups(base.Sender.entityId);
			return;
		case NetPackageBossEvent.BossEventTypes.AddGroup:
			GameEventManager.Current.SetupClientBossGroup(this.bossGroupID, this.bossGroupType, this.entityID, this.minionIDs, this.bossIcon1);
			return;
		case NetPackageBossEvent.BossEventTypes.UpdateGroupType:
			GameEventManager.Current.UpdateBossGroupType(this.bossGroupID, this.bossGroupType);
			return;
		case NetPackageBossEvent.BossEventTypes.RemoveGroup:
			GameEventManager.Current.RemoveClientBossGroup(this.bossGroupID);
			return;
		case NetPackageBossEvent.BossEventTypes.RemoveMinion:
			GameEventManager.Current.RemoveEntityFromBossGroup(this.bossGroupID, this.entityID);
			return;
		case NetPackageBossEvent.BossEventTypes.RequestStats:
			GameEventManager.Current.RequestBossGroupStatRefresh(this.bossGroupID, base.Sender.entityId);
			return;
		default:
			return;
		}
	}

	// Token: 0x06003AA0 RID: 15008 RVA: 0x000B93F5 File Offset: 0x000B75F5
	public override int GetLength()
	{
		return 20;
	}

	// Token: 0x04002FD9 RID: 12249
	public int bossGroupID;

	// Token: 0x04002FDA RID: 12250
	public int entityID;

	// Token: 0x04002FDB RID: 12251
	public List<int> minionIDs;

	// Token: 0x04002FDC RID: 12252
	public string bossIcon1;

	// Token: 0x04002FDD RID: 12253
	public BossGroup.BossGroupTypes bossGroupType;

	// Token: 0x04002FDE RID: 12254
	public NetPackageBossEvent.BossEventTypes eventType;

	// Token: 0x020007B4 RID: 1972
	public enum BossEventTypes
	{
		// Token: 0x04002FE0 RID: 12256
		RequestGroups,
		// Token: 0x04002FE1 RID: 12257
		AddGroup,
		// Token: 0x04002FE2 RID: 12258
		UpdateGroupType,
		// Token: 0x04002FE3 RID: 12259
		RemoveGroup,
		// Token: 0x04002FE4 RID: 12260
		RemoveMinion,
		// Token: 0x04002FE5 RID: 12261
		RequestStats
	}
}
