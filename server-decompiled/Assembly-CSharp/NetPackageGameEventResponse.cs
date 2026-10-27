using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000804 RID: 2052
[Preserve]
public class NetPackageGameEventResponse : NetPackage
{
	// Token: 0x06003C48 RID: 15432 RVA: 0x00182D38 File Offset: 0x00180F38
	public NetPackageGameEventResponse Setup(string _event, int _targetEntityID, string _extraData, string _tag, NetPackageGameEventResponse.ResponseTypes _responseType, int _entitySpawnedID = -1, int _index = -1, bool _isDespawn = false, string _actionKey = "")
	{
		this.eventName = _event;
		this.targetEntityID = _targetEntityID;
		this.extraData = _extraData;
		this.tag = _tag;
		this.responseType = _responseType;
		this.entitySpawnedID = _entitySpawnedID;
		this.index = _index;
		this.isDespawn = _isDespawn;
		this.actionKey = _actionKey;
		return this;
	}

	// Token: 0x06003C49 RID: 15433 RVA: 0x00182D8C File Offset: 0x00180F8C
	public NetPackageGameEventResponse Setup(NetPackageGameEventResponse.ResponseTypes _responseType, int _entitySpawnedID = -1, int _index = -1, string _tag = "", bool _isDespawn = false)
	{
		this.eventName = "";
		this.targetEntityID = -1;
		this.extraData = "";
		this.tag = _tag;
		this.responseType = _responseType;
		this.entitySpawnedID = _entitySpawnedID;
		this.index = _index;
		this.isDespawn = _isDespawn;
		return this;
	}

	// Token: 0x06003C4A RID: 15434 RVA: 0x00182DDC File Offset: 0x00180FDC
	public NetPackageGameEventResponse Setup(NetPackageGameEventResponse.ResponseTypes _responseType, string _event, int _blockGroupID, List<Vector3i> _blockList, string _tag = "", bool _isDespawn = false)
	{
		this.eventName = _event;
		this.targetEntityID = -1;
		this.extraData = "";
		this.tag = _tag;
		this.index = _blockGroupID;
		this.responseType = _responseType;
		this.blockList = _blockList;
		this.isDespawn = _isDespawn;
		return this;
	}

	// Token: 0x06003C4B RID: 15435 RVA: 0x00182E29 File Offset: 0x00181029
	public NetPackageGameEventResponse Setup(NetPackageGameEventResponse.ResponseTypes _responseType, Vector3i _blockPos)
	{
		this.eventName = "";
		this.targetEntityID = -1;
		this.extraData = "";
		this.tag = "";
		this.responseType = _responseType;
		this.blockPos = _blockPos;
		return this;
	}

	// Token: 0x06003C4C RID: 15436 RVA: 0x00182E64 File Offset: 0x00181064
	public override void read(PooledBinaryReader _br)
	{
		this.eventName = _br.ReadString();
		this.targetEntityID = _br.ReadInt32();
		this.extraData = _br.ReadString();
		this.tag = _br.ReadString();
		this.responseType = (NetPackageGameEventResponse.ResponseTypes)_br.ReadByte();
		this.entitySpawnedID = _br.ReadInt32();
		if (this.responseType == NetPackageGameEventResponse.ResponseTypes.ClientSequenceAction)
		{
			this.actionKey = _br.ReadString();
			return;
		}
		if (this.responseType == NetPackageGameEventResponse.ResponseTypes.BlocksAdded)
		{
			this.index = _br.ReadInt32();
			int num = _br.ReadInt32();
			this.blockList = new List<Vector3i>();
			for (int i = 0; i < num; i++)
			{
				this.blockList.Add(StreamUtils.ReadVector3i(_br));
			}
			return;
		}
		if (this.responseType == NetPackageGameEventResponse.ResponseTypes.BlocksRemoved)
		{
			this.index = _br.ReadInt32();
			this.isDespawn = _br.ReadBoolean();
			return;
		}
		if (this.responseType == NetPackageGameEventResponse.ResponseTypes.BlocksRemoved || this.responseType == NetPackageGameEventResponse.ResponseTypes.BlockDamaged)
		{
			this.blockPos = StreamUtils.ReadVector3i(_br);
		}
	}

	// Token: 0x06003C4D RID: 15437 RVA: 0x00182F58 File Offset: 0x00181158
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.eventName);
		_bw.Write(this.targetEntityID);
		_bw.Write(this.extraData);
		_bw.Write(this.tag);
		_bw.Write((byte)this.responseType);
		_bw.Write(this.entitySpawnedID);
		if (this.responseType == NetPackageGameEventResponse.ResponseTypes.ClientSequenceAction)
		{
			_bw.Write(this.actionKey);
			return;
		}
		if (this.responseType == NetPackageGameEventResponse.ResponseTypes.BlocksAdded)
		{
			_bw.Write(this.index);
			if (this.blockList == null)
			{
				_bw.Write(0);
				return;
			}
			_bw.Write(this.blockList.Count);
			for (int i = 0; i < this.blockList.Count; i++)
			{
				StreamUtils.Write(_bw, this.blockList[i]);
			}
			return;
		}
		else
		{
			if (this.responseType == NetPackageGameEventResponse.ResponseTypes.BlocksRemoved)
			{
				_bw.Write(this.index);
				_bw.Write(this.isDespawn);
				return;
			}
			if (this.responseType == NetPackageGameEventResponse.ResponseTypes.BlocksRemoved || this.responseType == NetPackageGameEventResponse.ResponseTypes.BlockDamaged)
			{
				StreamUtils.Write(_bw, this.blockPos);
			}
			return;
		}
	}

	// Token: 0x06003C4E RID: 15438 RVA: 0x00183070 File Offset: 0x00181270
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		switch (this.responseType)
		{
		case NetPackageGameEventResponse.ResponseTypes.Denied:
			GameEventManager.Current.HandleGameEventDenied(this.eventName, this.targetEntityID, this.extraData, this.tag);
			return;
		case NetPackageGameEventResponse.ResponseTypes.Approved:
			GameEventManager.Current.HandleGameEventApproved(this.eventName, this.targetEntityID, this.extraData, this.tag);
			return;
		case NetPackageGameEventResponse.ResponseTypes.TwitchPartyActionApproved:
			GameEventManager.Current.HandleTwitchPartyGameEventApproved(this.eventName, this.targetEntityID, this.extraData, this.tag);
			return;
		case NetPackageGameEventResponse.ResponseTypes.TwitchRefundNeeded:
			GameEventManager.Current.HandleTwitchRefundNeeded(this.eventName, this.targetEntityID, this.extraData, this.tag);
			return;
		case NetPackageGameEventResponse.ResponseTypes.TwitchSetOwner:
			GameEventManager.Current.HandleGameEntitySpawned(this.eventName, this.entitySpawnedID, this.tag);
			GameEventManager.Current.HandleTwitchSetOwner(this.targetEntityID, this.entitySpawnedID, this.extraData);
			return;
		case NetPackageGameEventResponse.ResponseTypes.EntitySpawned:
			GameEventManager.Current.HandleGameEntitySpawned(this.eventName, this.entitySpawnedID, this.tag);
			return;
		case NetPackageGameEventResponse.ResponseTypes.EntityDespawned:
			GameEventManager.Current.HandleGameEntityDespawned(this.entitySpawnedID);
			return;
		case NetPackageGameEventResponse.ResponseTypes.EntityKilled:
			GameEventManager.Current.HandleGameEntityKilled(this.entitySpawnedID);
			return;
		case NetPackageGameEventResponse.ResponseTypes.BlocksAdded:
			GameEventManager.Current.HandleGameBlocksAdded(this.eventName, this.index, this.blockList, this.tag);
			return;
		case NetPackageGameEventResponse.ResponseTypes.BlocksRemoved:
			GameEventManager.Current.HandleGameBlocksRemoved(this.index, this.isDespawn);
			return;
		case NetPackageGameEventResponse.ResponseTypes.BlockRemoved:
			GameEventManager.Current.HandleGameBlockRemoved(this.blockPos);
			return;
		case NetPackageGameEventResponse.ResponseTypes.BlockDamaged:
			GameEventManager.Current.SendBlockDamageUpdate(this.blockPos);
			return;
		case NetPackageGameEventResponse.ResponseTypes.ClientSequenceAction:
			GameEventManager.Current.HandleGameEventSequenceItemForClient(this.eventName, this.actionKey);
			return;
		case NetPackageGameEventResponse.ResponseTypes.Completed:
			GameEventManager.Current.HandleGameEventCompleted(this.eventName, this.targetEntityID, this.extraData, this.tag);
			return;
		default:
			return;
		}
	}

	// Token: 0x1700066A RID: 1642
	// (get) Token: 0x06003C4F RID: 15439 RVA: 0x00010E62 File Offset: 0x0000F062
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.Both;
		}
	}

	// Token: 0x04003104 RID: 12548
	[PublicizedFrom(EAccessModifier.Private)]
	public string eventName;

	// Token: 0x04003105 RID: 12549
	[PublicizedFrom(EAccessModifier.Private)]
	public NetPackageGameEventResponse.ResponseTypes responseType;

	// Token: 0x04003106 RID: 12550
	[PublicizedFrom(EAccessModifier.Private)]
	public int entitySpawnedID = -1;

	// Token: 0x04003107 RID: 12551
	[PublicizedFrom(EAccessModifier.Private)]
	public int targetEntityID = -1;

	// Token: 0x04003108 RID: 12552
	[PublicizedFrom(EAccessModifier.Private)]
	public string extraData;

	// Token: 0x04003109 RID: 12553
	[PublicizedFrom(EAccessModifier.Private)]
	public string tag = "";

	// Token: 0x0400310A RID: 12554
	[PublicizedFrom(EAccessModifier.Private)]
	public int index = -1;

	// Token: 0x0400310B RID: 12555
	[PublicizedFrom(EAccessModifier.Private)]
	public string actionKey = "";

	// Token: 0x0400310C RID: 12556
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isDespawn;

	// Token: 0x0400310D RID: 12557
	[PublicizedFrom(EAccessModifier.Private)]
	public List<Vector3i> blockList;

	// Token: 0x0400310E RID: 12558
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3i blockPos;

	// Token: 0x02000805 RID: 2053
	public enum ResponseTypes
	{
		// Token: 0x04003110 RID: 12560
		Denied,
		// Token: 0x04003111 RID: 12561
		Approved,
		// Token: 0x04003112 RID: 12562
		TwitchPartyActionApproved,
		// Token: 0x04003113 RID: 12563
		TwitchRefundNeeded,
		// Token: 0x04003114 RID: 12564
		TwitchSetOwner,
		// Token: 0x04003115 RID: 12565
		EntitySpawned,
		// Token: 0x04003116 RID: 12566
		EntityDespawned,
		// Token: 0x04003117 RID: 12567
		EntityKilled,
		// Token: 0x04003118 RID: 12568
		BlocksAdded,
		// Token: 0x04003119 RID: 12569
		BlocksRemoved,
		// Token: 0x0400311A RID: 12570
		BlockRemoved,
		// Token: 0x0400311B RID: 12571
		BlockDamaged,
		// Token: 0x0400311C RID: 12572
		ClientSequenceAction,
		// Token: 0x0400311D RID: 12573
		Completed
	}
}
