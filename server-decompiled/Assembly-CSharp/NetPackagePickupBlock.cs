using System;
using UnityEngine.Scripting;

// Token: 0x02000820 RID: 2080
[Preserve]
public class NetPackagePickupBlock : NetPackage
{
	// Token: 0x06003CEC RID: 15596 RVA: 0x0018517A File Offset: 0x0018337A
	public NetPackagePickupBlock Setup(Vector3i _blockPos, BlockValue _blockValue, int _playerId, PersistentPlayerData _persistentPlayerData)
	{
		this.blockPos = _blockPos;
		this.blockValue = _blockValue;
		this.playerId = _playerId;
		this.persistentPlayerId = ((_persistentPlayerData != null) ? _persistentPlayerData.PrimaryId : null);
		return this;
	}

	// Token: 0x06003CED RID: 15597 RVA: 0x001851A6 File Offset: 0x001833A6
	public override void read(PooledBinaryReader _br)
	{
		this.blockPos = StreamUtils.ReadVector3i(_br);
		this.blockValue = new BlockValue(_br.ReadUInt32());
		this.playerId = _br.ReadInt32();
		this.persistentPlayerId = PlatformUserIdentifierAbs.FromStream(_br, false, false);
	}

	// Token: 0x06003CEE RID: 15598 RVA: 0x001851DF File Offset: 0x001833DF
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		StreamUtils.Write(_bw, this.blockPos);
		_bw.Write(this.blockValue.rawData);
		_bw.Write(this.playerId);
		this.persistentPlayerId.ToStream(_bw, false);
	}

	// Token: 0x06003CEF RID: 15599 RVA: 0x00185220 File Offset: 0x00183420
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

	// Token: 0x04003199 RID: 12697
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3i blockPos;

	// Token: 0x0400319A RID: 12698
	[PublicizedFrom(EAccessModifier.Private)]
	public BlockValue blockValue;

	// Token: 0x0400319B RID: 12699
	[PublicizedFrom(EAccessModifier.Private)]
	public int playerId;

	// Token: 0x0400319C RID: 12700
	[PublicizedFrom(EAccessModifier.Private)]
	public PlatformUserIdentifierAbs persistentPlayerId;
}
