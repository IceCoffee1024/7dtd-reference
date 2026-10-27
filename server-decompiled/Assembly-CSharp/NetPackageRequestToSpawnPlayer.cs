using System;
using UnityEngine.Scripting;

// Token: 0x0200084E RID: 2126
[Preserve]
public class NetPackageRequestToSpawnPlayer : NetPackage
{
	// Token: 0x1700069C RID: 1692
	// (get) Token: 0x06003DCF RID: 15823 RVA: 0x00020119 File Offset: 0x0001E319
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToServer;
		}
	}

	// Token: 0x06003DD0 RID: 15824 RVA: 0x00189BF6 File Offset: 0x00187DF6
	public NetPackageRequestToSpawnPlayer Setup(int _chunkViewDim, PlayerProfile _playerProfile, int _nearEntityId)
	{
		this.chunkViewDim = _chunkViewDim;
		this.playerProfile = _playerProfile;
		this.nearEntityId = _nearEntityId;
		return this;
	}

	// Token: 0x06003DD1 RID: 15825 RVA: 0x00189C0E File Offset: 0x00187E0E
	public override void read(PooledBinaryReader _reader)
	{
		this.chunkViewDim = (int)_reader.ReadInt16();
		this.playerProfile = PlayerProfile.Read(_reader);
		this.nearEntityId = _reader.ReadInt32();
	}

	// Token: 0x06003DD2 RID: 15826 RVA: 0x00189C34 File Offset: 0x00187E34
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write((short)this.chunkViewDim);
		this.playerProfile.Write(_writer);
		_writer.Write(this.nearEntityId);
	}

	// Token: 0x06003DD3 RID: 15827 RVA: 0x00189C62 File Offset: 0x00187E62
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		_callbacks.RequestToSpawnPlayer(base.Sender, this.chunkViewDim, this.playerProfile, this.nearEntityId);
	}

	// Token: 0x0400325D RID: 12893
	[PublicizedFrom(EAccessModifier.Private)]
	public int chunkViewDim;

	// Token: 0x0400325E RID: 12894
	[PublicizedFrom(EAccessModifier.Private)]
	public PlayerProfile playerProfile;

	// Token: 0x0400325F RID: 12895
	[PublicizedFrom(EAccessModifier.Private)]
	public int nearEntityId;
}
