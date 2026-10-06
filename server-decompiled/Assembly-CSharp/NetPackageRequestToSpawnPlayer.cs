using System;
using UnityEngine.Scripting;

// Token: 0x02000844 RID: 2116
[Preserve]
public class NetPackageRequestToSpawnPlayer : NetPackage
{
	// Token: 0x17000696 RID: 1686
	// (get) Token: 0x06003DAC RID: 15788 RVA: 0x00020119 File Offset: 0x0001E319
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToServer;
		}
	}

	// Token: 0x06003DAD RID: 15789 RVA: 0x00188AF2 File Offset: 0x00186CF2
	public NetPackageRequestToSpawnPlayer Setup(int _chunkViewDim, PlayerProfile _playerProfile, int _nearEntityId)
	{
		this.chunkViewDim = _chunkViewDim;
		this.playerProfile = _playerProfile;
		this.nearEntityId = _nearEntityId;
		return this;
	}

	// Token: 0x06003DAE RID: 15790 RVA: 0x00188B0A File Offset: 0x00186D0A
	public override void read(PooledBinaryReader _reader)
	{
		this.chunkViewDim = (int)_reader.ReadInt16();
		this.playerProfile = PlayerProfile.Read(_reader);
		this.nearEntityId = _reader.ReadInt32();
	}

	// Token: 0x06003DAF RID: 15791 RVA: 0x00188B30 File Offset: 0x00186D30
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write((short)this.chunkViewDim);
		this.playerProfile.Write(_writer);
		_writer.Write(this.nearEntityId);
	}

	// Token: 0x06003DB0 RID: 15792 RVA: 0x00188B5E File Offset: 0x00186D5E
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		_callbacks.RequestToSpawnPlayer(base.Sender, this.chunkViewDim, this.playerProfile, this.nearEntityId);
	}

	// Token: 0x0400323D RID: 12861
	[PublicizedFrom(EAccessModifier.Private)]
	public int chunkViewDim;

	// Token: 0x0400323E RID: 12862
	[PublicizedFrom(EAccessModifier.Private)]
	public PlayerProfile playerProfile;

	// Token: 0x0400323F RID: 12863
	[PublicizedFrom(EAccessModifier.Private)]
	public int nearEntityId;
}
