using System;
using UnityEngine.Scripting;

// Token: 0x02000843 RID: 2115
[Preserve]
public class NetPackageRequestToSpawnPlayer : NetPackage
{
	// Token: 0x17000677 RID: 1655
	// (get) Token: 0x06003DFE RID: 15870 RVA: 0x00020021 File Offset: 0x0001E221
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToServer;
		}
	}

	// Token: 0x06003DFF RID: 15871 RVA: 0x00188FBA File Offset: 0x001871BA
	public NetPackageRequestToSpawnPlayer Setup(int _chunkViewDim, PlayerProfile _playerProfile, int _nearEntityId)
	{
		this.chunkViewDim = _chunkViewDim;
		this.playerProfile = _playerProfile;
		this.nearEntityId = _nearEntityId;
		return this;
	}

	// Token: 0x06003E00 RID: 15872 RVA: 0x00188FD2 File Offset: 0x001871D2
	public override void read(PooledBinaryReader _reader)
	{
		this.chunkViewDim = (int)_reader.ReadInt16();
		this.playerProfile = PlayerProfile.Read(_reader);
		this.nearEntityId = _reader.ReadInt32();
	}

	// Token: 0x06003E01 RID: 15873 RVA: 0x00188FF8 File Offset: 0x001871F8
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write((short)this.chunkViewDim);
		this.playerProfile.Write(_writer);
		_writer.Write(this.nearEntityId);
	}

	// Token: 0x06003E02 RID: 15874 RVA: 0x00189026 File Offset: 0x00187226
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		_callbacks.RequestToSpawnPlayer(base.Sender, this.chunkViewDim, this.playerProfile, this.nearEntityId);
	}

	// Token: 0x06003E03 RID: 15875 RVA: 0x00177A48 File Offset: 0x00175C48
	public override int GetLength()
	{
		return 50;
	}

	// Token: 0x04003256 RID: 12886
	[PublicizedFrom(EAccessModifier.Private)]
	public int chunkViewDim;

	// Token: 0x04003257 RID: 12887
	[PublicizedFrom(EAccessModifier.Private)]
	public PlayerProfile playerProfile;

	// Token: 0x04003258 RID: 12888
	[PublicizedFrom(EAccessModifier.Private)]
	public int nearEntityId;
}
