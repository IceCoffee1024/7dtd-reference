using System;
using UnityEngine.Scripting;

// Token: 0x02000822 RID: 2082
[Preserve]
public class NetPackagePickupBlock : NetPackage
{
	// Token: 0x06003D2B RID: 15659 RVA: 0x001855AE File Offset: 0x001837AE
	public NetPackagePickupBlock Setup(Vector3i _blockPos, BlockValue _blockValue, int _playerId, PersistentPlayerData _persistentPlayerData)
	{
		this.blockPos = _blockPos;
		this.blockValue = _blockValue;
		this.playerId = _playerId;
		this.persistentPlayerId = ((_persistentPlayerData != null) ? _persistentPlayerData.PrimaryId : null);
		return this;
	}

	// Token: 0x06003D2C RID: 15660 RVA: 0x001855DA File Offset: 0x001837DA
	public override void read(PooledBinaryReader _br)
	{
		this.blockPos = StreamUtils.ReadVector3i(_br);
		this.blockValue = new BlockValue(_br.ReadUInt32());
		this.playerId = _br.ReadInt32();
		this.persistentPlayerId = PlatformUserIdentifierAbs.FromStream(_br, false, false);
	}

	// Token: 0x06003D2D RID: 15661 RVA: 0x00185613 File Offset: 0x00183813
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		StreamUtils.Write(_bw, this.blockPos);
		_bw.Write(this.blockValue.rawData);
		_bw.Write(this.playerId);
		this.persistentPlayerId.ToStream(_bw, false);
	}

	// Token: 0x06003D2E RID: 15662 RVA: 0x00185654 File Offset: 0x00183854
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

	// Token: 0x06003D2F RID: 15663 RVA: 0x001856CD File Offset: 0x001838CD
	public override int GetLength()
	{
		return 36;
	}

	// Token: 0x040031B5 RID: 12725
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3i blockPos;

	// Token: 0x040031B6 RID: 12726
	[PublicizedFrom(EAccessModifier.Private)]
	public BlockValue blockValue;

	// Token: 0x040031B7 RID: 12727
	[PublicizedFrom(EAccessModifier.Private)]
	public int playerId;

	// Token: 0x040031B8 RID: 12728
	[PublicizedFrom(EAccessModifier.Private)]
	public PlatformUserIdentifierAbs persistentPlayerId;
}
