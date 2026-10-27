using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020007BE RID: 1982
[Preserve]
public class NetPackageBossEvent : NetPackage
{
	// Token: 0x06003AE4 RID: 15076 RVA: 0x0017D759 File Offset: 0x0017B959
	public NetPackageBossEvent Setup(NetPackageBossEvent.BossEventTypes _eventType, int _bossGroupID)
	{
		this.bossGroupID = _bossGroupID;
		this.entityID = -1;
		this.minionIDs = null;
		this.eventType = _eventType;
		this.bossIcon1 = "";
		return this;
	}

	// Token: 0x06003AE5 RID: 15077 RVA: 0x0017D783 File Offset: 0x0017B983
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

	// Token: 0x06003AE6 RID: 15078 RVA: 0x0017D7B4 File Offset: 0x0017B9B4
	public NetPackageBossEvent Setup(NetPackageBossEvent.BossEventTypes _eventType, int _bossGroupID, int _entityID)
	{
		this.bossGroupID = _bossGroupID;
		this.entityID = _entityID;
		this.minionIDs = null;
		this.eventType = _eventType;
		this.bossIcon1 = "";
		return this;
	}

	// Token: 0x06003AE7 RID: 15079 RVA: 0x0017D7DE File Offset: 0x0017B9DE
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

	// Token: 0x06003AE8 RID: 15080 RVA: 0x0017D810 File Offset: 0x0017BA10
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

	// Token: 0x06003AE9 RID: 15081 RVA: 0x0017D894 File Offset: 0x0017BA94
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

	// Token: 0x06003AEA RID: 15082 RVA: 0x0017D928 File Offset: 0x0017BB28
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

	// Token: 0x04002FEF RID: 12271
	public int bossGroupID;

	// Token: 0x04002FF0 RID: 12272
	public int entityID;

	// Token: 0x04002FF1 RID: 12273
	public List<int> minionIDs;

	// Token: 0x04002FF2 RID: 12274
	public string bossIcon1;

	// Token: 0x04002FF3 RID: 12275
	public BossGroup.BossGroupTypes bossGroupType;

	// Token: 0x04002FF4 RID: 12276
	public NetPackageBossEvent.BossEventTypes eventType;

	// Token: 0x020007BF RID: 1983
	public enum BossEventTypes
	{
		// Token: 0x04002FF6 RID: 12278
		RequestGroups,
		// Token: 0x04002FF7 RID: 12279
		AddGroup,
		// Token: 0x04002FF8 RID: 12280
		UpdateGroupType,
		// Token: 0x04002FF9 RID: 12281
		RemoveGroup,
		// Token: 0x04002FFA RID: 12282
		RemoveMinion,
		// Token: 0x04002FFB RID: 12283
		RequestStats
	}
}
