using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020007A9 RID: 1961
[Preserve]
public class NetPackageBossEvent : NetPackage
{
	// Token: 0x06003A41 RID: 14913 RVA: 0x00179EB5 File Offset: 0x001780B5
	public NetPackageBossEvent Setup(NetPackageBossEvent.BossEventTypes _eventType, int _bossGroupID)
	{
		this.bossGroupID = _bossGroupID;
		this.entityID = -1;
		this.minionIDs = null;
		this.eventType = _eventType;
		this.bossIcon1 = "";
		return this;
	}

	// Token: 0x06003A42 RID: 14914 RVA: 0x00179EDF File Offset: 0x001780DF
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

	// Token: 0x06003A43 RID: 14915 RVA: 0x00179F10 File Offset: 0x00178110
	public NetPackageBossEvent Setup(NetPackageBossEvent.BossEventTypes _eventType, int _bossGroupID, int _entityID)
	{
		this.bossGroupID = _bossGroupID;
		this.entityID = _entityID;
		this.minionIDs = null;
		this.eventType = _eventType;
		this.bossIcon1 = "";
		return this;
	}

	// Token: 0x06003A44 RID: 14916 RVA: 0x00179F3A File Offset: 0x0017813A
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

	// Token: 0x06003A45 RID: 14917 RVA: 0x00179F6C File Offset: 0x0017816C
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

	// Token: 0x06003A46 RID: 14918 RVA: 0x00179FF0 File Offset: 0x001781F0
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

	// Token: 0x06003A47 RID: 14919 RVA: 0x0017A084 File Offset: 0x00178284
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

	// Token: 0x06003A48 RID: 14920 RVA: 0x000B8CB1 File Offset: 0x000B6EB1
	public override int GetLength()
	{
		return 20;
	}

	// Token: 0x04002F60 RID: 12128
	public int bossGroupID;

	// Token: 0x04002F61 RID: 12129
	public int entityID;

	// Token: 0x04002F62 RID: 12130
	public List<int> minionIDs;

	// Token: 0x04002F63 RID: 12131
	public string bossIcon1;

	// Token: 0x04002F64 RID: 12132
	public BossGroup.BossGroupTypes bossGroupType;

	// Token: 0x04002F65 RID: 12133
	public NetPackageBossEvent.BossEventTypes eventType;

	// Token: 0x020007AA RID: 1962
	public enum BossEventTypes
	{
		// Token: 0x04002F67 RID: 12135
		RequestGroups,
		// Token: 0x04002F68 RID: 12136
		AddGroup,
		// Token: 0x04002F69 RID: 12137
		UpdateGroupType,
		// Token: 0x04002F6A RID: 12138
		RemoveGroup,
		// Token: 0x04002F6B RID: 12139
		RemoveMinion,
		// Token: 0x04002F6C RID: 12140
		RequestStats
	}
}
