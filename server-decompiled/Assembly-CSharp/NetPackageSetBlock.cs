using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000845 RID: 2117
[Preserve]
public class NetPackageSetBlock : NetPackage
{
	// Token: 0x06003E0B RID: 15883 RVA: 0x001890CA File Offset: 0x001872CA
	public NetPackageSetBlock Setup(PersistentPlayerData _persistentPlayerData, List<BlockChangeInfo> _blockChanges, int _localPlayerThatChanged)
	{
		this.persistentPlayerId = ((_persistentPlayerData != null) ? _persistentPlayerData.PrimaryId : null);
		this.blockChanges = _blockChanges;
		this.localPlayerThatChanged = _localPlayerThatChanged;
		return this;
	}

	// Token: 0x06003E0C RID: 15884 RVA: 0x001890F0 File Offset: 0x001872F0
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

	// Token: 0x06003E0D RID: 15885 RVA: 0x00189150 File Offset: 0x00187350
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

	// Token: 0x06003E0E RID: 15886 RVA: 0x001891B0 File Offset: 0x001873B0
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

	// Token: 0x06003E0F RID: 15887 RVA: 0x00189270 File Offset: 0x00187470
	public override int GetLength()
	{
		return this.blockChanges.Count * 16;
	}

	// Token: 0x0400325A RID: 12890
	[PublicizedFrom(EAccessModifier.Private)]
	public List<BlockChangeInfo> blockChanges;

	// Token: 0x0400325B RID: 12891
	[PublicizedFrom(EAccessModifier.Private)]
	public PlatformUserIdentifierAbs persistentPlayerId;

	// Token: 0x0400325C RID: 12892
	[PublicizedFrom(EAccessModifier.Private)]
	public int localPlayerThatChanged;
}
