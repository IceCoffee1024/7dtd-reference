using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020007FA RID: 2042
[Preserve]
public class NetPackageGameEventResponse : NetPackage
{
	// Token: 0x06003C25 RID: 15397 RVA: 0x00181C34 File Offset: 0x0017FE34
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

	// Token: 0x06003C26 RID: 15398 RVA: 0x00181C88 File Offset: 0x0017FE88
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

	// Token: 0x06003C27 RID: 15399 RVA: 0x00181CD8 File Offset: 0x0017FED8
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

	// Token: 0x06003C28 RID: 15400 RVA: 0x00181D25 File Offset: 0x0017FF25
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

	// Token: 0x06003C29 RID: 15401 RVA: 0x00181D60 File Offset: 0x0017FF60
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

	// Token: 0x06003C2A RID: 15402 RVA: 0x00181E54 File Offset: 0x00180054
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

	// Token: 0x06003C2B RID: 15403 RVA: 0x00181F6C File Offset: 0x0018016C
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

	// Token: 0x17000664 RID: 1636
	// (get) Token: 0x06003C2C RID: 15404 RVA: 0x00010E62 File Offset: 0x0000F062
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.Both;
		}
	}

	// Token: 0x040030E4 RID: 12516
	[PublicizedFrom(EAccessModifier.Private)]
	public string eventName;

	// Token: 0x040030E5 RID: 12517
	[PublicizedFrom(EAccessModifier.Private)]
	public NetPackageGameEventResponse.ResponseTypes responseType;

	// Token: 0x040030E6 RID: 12518
	[PublicizedFrom(EAccessModifier.Private)]
	public int entitySpawnedID = -1;

	// Token: 0x040030E7 RID: 12519
	[PublicizedFrom(EAccessModifier.Private)]
	public int targetEntityID = -1;

	// Token: 0x040030E8 RID: 12520
	[PublicizedFrom(EAccessModifier.Private)]
	public string extraData;

	// Token: 0x040030E9 RID: 12521
	[PublicizedFrom(EAccessModifier.Private)]
	public string tag = "";

	// Token: 0x040030EA RID: 12522
	[PublicizedFrom(EAccessModifier.Private)]
	public int index = -1;

	// Token: 0x040030EB RID: 12523
	[PublicizedFrom(EAccessModifier.Private)]
	public string actionKey = "";

	// Token: 0x040030EC RID: 12524
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isDespawn;

	// Token: 0x040030ED RID: 12525
	[PublicizedFrom(EAccessModifier.Private)]
	public List<Vector3i> blockList;

	// Token: 0x040030EE RID: 12526
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3i blockPos;

	// Token: 0x020007FB RID: 2043
	public enum ResponseTypes
	{
		// Token: 0x040030F0 RID: 12528
		Denied,
		// Token: 0x040030F1 RID: 12529
		Approved,
		// Token: 0x040030F2 RID: 12530
		TwitchPartyActionApproved,
		// Token: 0x040030F3 RID: 12531
		TwitchRefundNeeded,
		// Token: 0x040030F4 RID: 12532
		TwitchSetOwner,
		// Token: 0x040030F5 RID: 12533
		EntitySpawned,
		// Token: 0x040030F6 RID: 12534
		EntityDespawned,
		// Token: 0x040030F7 RID: 12535
		EntityKilled,
		// Token: 0x040030F8 RID: 12536
		BlocksAdded,
		// Token: 0x040030F9 RID: 12537
		BlocksRemoved,
		// Token: 0x040030FA RID: 12538
		BlockRemoved,
		// Token: 0x040030FB RID: 12539
		BlockDamaged,
		// Token: 0x040030FC RID: 12540
		ClientSequenceAction,
		// Token: 0x040030FD RID: 12541
		Completed
	}
}
