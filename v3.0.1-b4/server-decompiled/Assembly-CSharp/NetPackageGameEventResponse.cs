using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020007EC RID: 2028
[Preserve]
public class NetPackageGameEventResponse : NetPackage
{
	// Token: 0x06003BC7 RID: 15303 RVA: 0x0017F348 File Offset: 0x0017D548
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

	// Token: 0x06003BC8 RID: 15304 RVA: 0x0017F39C File Offset: 0x0017D59C
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

	// Token: 0x06003BC9 RID: 15305 RVA: 0x0017F3EC File Offset: 0x0017D5EC
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

	// Token: 0x06003BCA RID: 15306 RVA: 0x0017F439 File Offset: 0x0017D639
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

	// Token: 0x06003BCB RID: 15307 RVA: 0x0017F474 File Offset: 0x0017D674
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

	// Token: 0x06003BCC RID: 15308 RVA: 0x0017F568 File Offset: 0x0017D768
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

	// Token: 0x06003BCD RID: 15309 RVA: 0x0017F680 File Offset: 0x0017D880
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

	// Token: 0x17000634 RID: 1588
	// (get) Token: 0x06003BCE RID: 15310 RVA: 0x00010E62 File Offset: 0x0000F062
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.Both;
		}
	}

	// Token: 0x06003BCF RID: 15311 RVA: 0x00104CB5 File Offset: 0x00102EB5
	public override int GetLength()
	{
		return 30;
	}

	// Token: 0x0400306C RID: 12396
	[PublicizedFrom(EAccessModifier.Private)]
	public string eventName;

	// Token: 0x0400306D RID: 12397
	[PublicizedFrom(EAccessModifier.Private)]
	public NetPackageGameEventResponse.ResponseTypes responseType;

	// Token: 0x0400306E RID: 12398
	[PublicizedFrom(EAccessModifier.Private)]
	public int entitySpawnedID = -1;

	// Token: 0x0400306F RID: 12399
	[PublicizedFrom(EAccessModifier.Private)]
	public int targetEntityID = -1;

	// Token: 0x04003070 RID: 12400
	[PublicizedFrom(EAccessModifier.Private)]
	public string extraData;

	// Token: 0x04003071 RID: 12401
	[PublicizedFrom(EAccessModifier.Private)]
	public string tag = "";

	// Token: 0x04003072 RID: 12402
	[PublicizedFrom(EAccessModifier.Private)]
	public int index = -1;

	// Token: 0x04003073 RID: 12403
	[PublicizedFrom(EAccessModifier.Private)]
	public string actionKey = "";

	// Token: 0x04003074 RID: 12404
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isDespawn;

	// Token: 0x04003075 RID: 12405
	[PublicizedFrom(EAccessModifier.Private)]
	public List<Vector3i> blockList;

	// Token: 0x04003076 RID: 12406
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3i blockPos;

	// Token: 0x020007ED RID: 2029
	public enum ResponseTypes
	{
		// Token: 0x04003078 RID: 12408
		Denied,
		// Token: 0x04003079 RID: 12409
		Approved,
		// Token: 0x0400307A RID: 12410
		TwitchPartyActionApproved,
		// Token: 0x0400307B RID: 12411
		TwitchRefundNeeded,
		// Token: 0x0400307C RID: 12412
		TwitchSetOwner,
		// Token: 0x0400307D RID: 12413
		EntitySpawned,
		// Token: 0x0400307E RID: 12414
		EntityDespawned,
		// Token: 0x0400307F RID: 12415
		EntityKilled,
		// Token: 0x04003080 RID: 12416
		BlocksAdded,
		// Token: 0x04003081 RID: 12417
		BlocksRemoved,
		// Token: 0x04003082 RID: 12418
		BlockRemoved,
		// Token: 0x04003083 RID: 12419
		BlockDamaged,
		// Token: 0x04003084 RID: 12420
		ClientSequenceAction,
		// Token: 0x04003085 RID: 12421
		Completed
	}
}
