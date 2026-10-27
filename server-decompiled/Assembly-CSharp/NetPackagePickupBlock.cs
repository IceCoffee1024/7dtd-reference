using System;
using UnityEngine.Scripting;

// Token: 0x0200082A RID: 2090
[Preserve]
public class NetPackagePickupBlock : NetPackage
{
	// Token: 0x06003D0F RID: 15631 RVA: 0x0018627E File Offset: 0x0018447E
	public NetPackagePickupBlock Setup(Vector3i _blockPos, BlockValue _blockValue, int _playerId, PersistentPlayerData _persistentPlayerData)
	{
		this.blockPos = _blockPos;
		this.blockValue = _blockValue;
		this.playerId = _playerId;
		this.persistentPlayerId = ((_persistentPlayerData != null) ? _persistentPlayerData.PrimaryId : null);
		return this;
	}

	// Token: 0x06003D10 RID: 15632 RVA: 0x001862AA File Offset: 0x001844AA
	public override void read(PooledBinaryReader _br)
	{
		this.blockPos = StreamUtils.ReadVector3i(_br);
		this.blockValue = new BlockValue(_br.ReadUInt32());
		this.playerId = _br.ReadInt32();
		this.persistentPlayerId = PlatformUserIdentifierAbs.FromStream(_br, false, false);
	}

	// Token: 0x06003D11 RID: 15633 RVA: 0x001862E3 File Offset: 0x001844E3
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		StreamUtils.Write(_bw, this.blockPos);
		_bw.Write(this.blockValue.rawData);
		_bw.Write(this.playerId);
		this.persistentPlayerId.ToStream(_bw, false);
	}

	// Token: 0x06003D12 RID: 15634 RVA: 0x00186324 File Offset: 0x00184524
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		if (!base.ValidEntityIdForSender(this.playerId, false))
		{
			return;
		}
		if (!base.ValidUserIdForSender(this.persistentPlayerId))
		{
			return;
		}
		if (!_world.IsRemote())
		{
			_world.GetGameManager().PickupBlockServer(this.blockPos, this.blockValue, this.playerId, this.persistentPlayerId);
			return;
		}
		_world.GetGameManager().PickupBlockClient(this.blockPos, this.blockValue, this.playerId);
	}

	// Token: 0x040031B9 RID: 12729
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3i blockPos;

	// Token: 0x040031BA RID: 12730
	[PublicizedFrom(EAccessModifier.Private)]
	public BlockValue blockValue;

	// Token: 0x040031BB RID: 12731
	[PublicizedFrom(EAccessModifier.Private)]
	public int playerId;

	// Token: 0x040031BC RID: 12732
	[PublicizedFrom(EAccessModifier.Private)]
	public PlatformUserIdentifierAbs persistentPlayerId;
}
