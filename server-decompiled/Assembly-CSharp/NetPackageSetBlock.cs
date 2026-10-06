using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000846 RID: 2118
[Preserve]
public class NetPackageSetBlock : NetPackage
{
	// Token: 0x06003DB7 RID: 15799 RVA: 0x00188C02 File Offset: 0x00186E02
	public NetPackageSetBlock Setup(PersistentPlayerData _persistentPlayerData, List<BlockChangeInfo> _blockChanges, int _localPlayerThatChanged)
	{
		this.persistentPlayerId = ((_persistentPlayerData != null) ? _persistentPlayerData.PrimaryId : null);
		this.blockChanges = _blockChanges;
		this.localPlayerThatChanged = _localPlayerThatChanged;
		return this;
	}

	// Token: 0x06003DB8 RID: 15800 RVA: 0x00188C28 File Offset: 0x00186E28
	public override void read(PooledBinaryReader _br)
	{
		this.persistentPlayerId = PlatformUserIdentifierAbs.FromStream(_br, false, false);
		int num = (int)_br.ReadInt16();
		this.blockChanges = new List<BlockChangeInfo>();
		for (int i = 0; i < num; i++)
		{
			BlockChangeInfo blockChangeInfo = new BlockChangeInfo();
			blockChangeInfo.Read(_br);
			this.blockChanges.Add(blockChangeInfo);
		}
		this.localPlayerThatChanged = _br.ReadInt32();
	}

	// Token: 0x06003DB9 RID: 15801 RVA: 0x00188C88 File Offset: 0x00186E88
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		this.persistentPlayerId.ToStream(_bw, false);
		int count = this.blockChanges.Count;
		_bw.Write((short)count);
		for (int i = 0; i < count; i++)
		{
			this.blockChanges[i].Write(_bw);
		}
		_bw.Write(this.localPlayerThatChanged);
	}

	// Token: 0x06003DBA RID: 15802 RVA: 0x00188CE8 File Offset: 0x00186EE8
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (!base.ValidUserIdForSender(this.persistentPlayerId))
		{
			return;
		}
		if (!base.ValidEntityIdForSender(this.localPlayerThatChanged, false))
		{
			return;
		}
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			_callbacks.SetBlocksOnClients(this.localPlayerThatChanged, this);
		}
		if (_world == null || _world.ChunkCache == null)
		{
			return;
		}
		if (DynamicMeshManager.CONTENT_ENABLED)
		{
			foreach (BlockChangeInfo blockChangeInfo in this.blockChanges)
			{
				DynamicMeshManager.ChunkChanged(blockChangeInfo.blockValueRef, -1, blockChangeInfo.blockValue.type);
			}
		}
		_callbacks.ChangeBlocks(this.persistentPlayerId, this.blockChanges);
	}

	// Token: 0x04003241 RID: 12865
	[PublicizedFrom(EAccessModifier.Private)]
	public List<BlockChangeInfo> blockChanges;

	// Token: 0x04003242 RID: 12866
	[PublicizedFrom(EAccessModifier.Private)]
	public PlatformUserIdentifierAbs persistentPlayerId;

	// Token: 0x04003243 RID: 12867
	[PublicizedFrom(EAccessModifier.Private)]
	public int localPlayerThatChanged;
}
