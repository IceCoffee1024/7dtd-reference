using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020007B6 RID: 1974
[Preserve]
public class NetPackageBossEvent : NetPackage
{
	// Token: 0x06003ACD RID: 15053 RVA: 0x0017C7C1 File Offset: 0x0017A9C1
	public NetPackageBossEvent Setup(NetPackageBossEvent.BossEventTypes _eventType, int _bossGroupID)
	{
		this.bossGroupID = _bossGroupID;
		this.entityID = -1;
		this.minionIDs = null;
		this.eventType = _eventType;
		this.bossIcon1 = "";
		return this;
	}

	// Token: 0x06003ACE RID: 15054 RVA: 0x0017C7EB File Offset: 0x0017A9EB
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

	// Token: 0x06003ACF RID: 15055 RVA: 0x0017C81C File Offset: 0x0017AA1C
	public NetPackageBossEvent Setup(NetPackageBossEvent.BossEventTypes _eventType, int _bossGroupID, int _entityID)
	{
		this.bossGroupID = _bossGroupID;
		this.entityID = _entityID;
		this.minionIDs = null;
		this.eventType = _eventType;
		this.bossIcon1 = "";
		return this;
	}

	// Token: 0x06003AD0 RID: 15056 RVA: 0x0017C846 File Offset: 0x0017AA46
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

	// Token: 0x06003AD1 RID: 15057 RVA: 0x0017C878 File Offset: 0x0017AA78
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

	// Token: 0x06003AD2 RID: 15058 RVA: 0x0017C8FC File Offset: 0x0017AAFC
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

	// Token: 0x06003AD3 RID: 15059 RVA: 0x0017C990 File Offset: 0x0017AB90
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

	// Token: 0x04002FD3 RID: 12243
	public int bossGroupID;

	// Token: 0x04002FD4 RID: 12244
	public int entityID;

	// Token: 0x04002FD5 RID: 12245
	public List<int> minionIDs;

	// Token: 0x04002FD6 RID: 12246
	public string bossIcon1;

	// Token: 0x04002FD7 RID: 12247
	public BossGroup.BossGroupTypes bossGroupType;

	// Token: 0x04002FD8 RID: 12248
	public NetPackageBossEvent.BossEventTypes eventType;

	// Token: 0x020007B7 RID: 1975
	public enum BossEventTypes
	{
		// Token: 0x04002FDA RID: 12250
		RequestGroups,
		// Token: 0x04002FDB RID: 12251
		AddGroup,
		// Token: 0x04002FDC RID: 12252
		UpdateGroupType,
		// Token: 0x04002FDD RID: 12253
		RemoveGroup,
		// Token: 0x04002FDE RID: 12254
		RemoveMinion,
		// Token: 0x04002FDF RID: 12255
		RequestStats
	}
}
