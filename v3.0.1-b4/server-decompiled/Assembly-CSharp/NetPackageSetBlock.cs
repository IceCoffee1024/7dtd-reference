using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000839 RID: 2105
[Preserve]
public class NetPackageSetBlock : NetPackage
{
	// Token: 0x06003DA6 RID: 15782 RVA: 0x001870AA File Offset: 0x001852AA
	public NetPackageSetBlock Setup(PersistentPlayerData _persistentPlayerData, List<BlockChangeInfo> _blockChanges, int _localPlayerThatChanged)
	{
		this.persistentPlayerId = ((_persistentPlayerData != null) ? _persistentPlayerData.PrimaryId : null);
		this.blockChanges = _blockChanges;
		this.localPlayerThatChanged = _localPlayerThatChanged;
		return this;
	}

	// Token: 0x06003DA7 RID: 15783 RVA: 0x001870D0 File Offset: 0x001852D0
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

	// Token: 0x06003DA8 RID: 15784 RVA: 0x00187130 File Offset: 0x00185330
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

	// Token: 0x06003DA9 RID: 15785 RVA: 0x00187190 File Offset: 0x00185390
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

	// Token: 0x06003DAA RID: 15786 RVA: 0x00187250 File Offset: 0x00185450
	public override int GetLength()
	{
		return this.blockChanges.Count * 16;
	}

	// Token: 0x040031DB RID: 12763
	[PublicizedFrom(EAccessModifier.Private)]
	public List<BlockChangeInfo> blockChanges;

	// Token: 0x040031DC RID: 12764
	[PublicizedFrom(EAccessModifier.Private)]
	public PlatformUserIdentifierAbs persistentPlayerId;

	// Token: 0x040031DD RID: 12765
	[PublicizedFrom(EAccessModifier.Private)]
	public int localPlayerThatChanged;
}
