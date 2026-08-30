using System;
using UnityEngine.Scripting;

// Token: 0x02000817 RID: 2071
[Preserve]
public class NetPackagePickupBlock : NetPackage
{
	// Token: 0x06003CCC RID: 15564 RVA: 0x001831D6 File Offset: 0x001813D6
	public NetPackagePickupBlock Setup(Vector3i _blockPos, BlockValue _blockValue, int _playerId, PersistentPlayerData _persistentPlayerData)
	{
		this.blockPos = _blockPos;
		this.blockValue = _blockValue;
		this.playerId = _playerId;
		this.persistentPlayerId = ((_persistentPlayerData != null) ? _persistentPlayerData.PrimaryId : null);
		return this;
	}

	// Token: 0x06003CCD RID: 15565 RVA: 0x00183202 File Offset: 0x00181402
	public override void read(PooledBinaryReader _br)
	{
		this.blockPos = StreamUtils.ReadVector3i(_br);
		this.blockValue = new BlockValue(_br.ReadUInt32());
		this.playerId = _br.ReadInt32();
		this.persistentPlayerId = PlatformUserIdentifierAbs.FromStream(_br, false, false);
	}

	// Token: 0x06003CCE RID: 15566 RVA: 0x0018323B File Offset: 0x0018143B
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		StreamUtils.Write(_bw, this.blockPos);
		_bw.Write(this.blockValue.rawData);
		_bw.Write(this.playerId);
		this.persistentPlayerId.ToStream(_bw, false);
	}

	// Token: 0x06003CCF RID: 15567 RVA: 0x0018327C File Offset: 0x0018147C
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

	// Token: 0x06003CD0 RID: 15568 RVA: 0x001832F5 File Offset: 0x001814F5
	public override int GetLength()
	{
		return 36;
	}

	// Token: 0x04003136 RID: 12598
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3i blockPos;

	// Token: 0x04003137 RID: 12599
	[PublicizedFrom(EAccessModifier.Private)]
	public BlockValue blockValue;

	// Token: 0x04003138 RID: 12600
	[PublicizedFrom(EAccessModifier.Private)]
	public int playerId;

	// Token: 0x04003139 RID: 12601
	[PublicizedFrom(EAccessModifier.Private)]
	public PlatformUserIdentifierAbs persistentPlayerId;
}
