using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000850 RID: 2128
[Preserve]
public class NetPackageSetBlock : NetPackage
{
	// Token: 0x06003DDA RID: 15834 RVA: 0x00189D06 File Offset: 0x00187F06
	public NetPackageSetBlock Setup(PersistentPlayerData _persistentPlayerData, List<BlockChangeInfo> _blockChanges, int _localPlayerThatChanged)
	{
		this.persistentPlayerId = ((_persistentPlayerData != null) ? _persistentPlayerData.PrimaryId : null);
		this.blockChanges = _blockChanges;
		this.localPlayerThatChanged = _localPlayerThatChanged;
		return this;
	}

	// Token: 0x06003DDB RID: 15835 RVA: 0x00189D2C File Offset: 0x00187F2C
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

	// Token: 0x06003DDC RID: 15836 RVA: 0x00189D8C File Offset: 0x00187F8C
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

	// Token: 0x06003DDD RID: 15837 RVA: 0x00189DEC File Offset: 0x00187FEC
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
		_callbacks.ChangeBlocks(this.persistentPlayerId, this.blockChanges);
	}

	// Token: 0x04003261 RID: 12897
	[PublicizedFrom(EAccessModifier.Private)]
	public List<BlockChangeInfo> blockChanges;

	// Token: 0x04003262 RID: 12898
	[PublicizedFrom(EAccessModifier.Private)]
	public PlatformUserIdentifierAbs persistentPlayerId;

	// Token: 0x04003263 RID: 12899
	[PublicizedFrom(EAccessModifier.Private)]
	public int localPlayerThatChanged;
}
