using System;
using UnityEngine.Scripting;

// Token: 0x02000837 RID: 2103
[Preserve]
public class NetPackageRequestToSpawnPlayer : NetPackage
{
	// Token: 0x1700066B RID: 1643
	// (get) Token: 0x06003D99 RID: 15769 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToServer;
		}
	}

	// Token: 0x06003D9A RID: 15770 RVA: 0x00186F9A File Offset: 0x0018519A
	public NetPackageRequestToSpawnPlayer Setup(int _chunkViewDim, PlayerProfile _playerProfile, int _nearEntityId)
	{
		this.chunkViewDim = _chunkViewDim;
		this.playerProfile = _playerProfile;
		this.nearEntityId = _nearEntityId;
		return this;
	}

	// Token: 0x06003D9B RID: 15771 RVA: 0x00186FB2 File Offset: 0x001851B2
	public override void read(PooledBinaryReader _reader)
	{
		this.chunkViewDim = (int)_reader.ReadInt16();
		this.playerProfile = PlayerProfile.Read(_reader);
		this.nearEntityId = _reader.ReadInt32();
	}

	// Token: 0x06003D9C RID: 15772 RVA: 0x00186FD8 File Offset: 0x001851D8
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write((short)this.chunkViewDim);
		this.playerProfile.Write(_writer);
		_writer.Write(this.nearEntityId);
	}

	// Token: 0x06003D9D RID: 15773 RVA: 0x00187006 File Offset: 0x00185206
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		_callbacks.RequestToSpawnPlayer(base.Sender, this.chunkViewDim, this.playerProfile, this.nearEntityId);
	}

	// Token: 0x06003D9E RID: 15774 RVA: 0x001757C8 File Offset: 0x001739C8
	public override int GetLength()
	{
		return 50;
	}

	// Token: 0x040031D7 RID: 12759
	[PublicizedFrom(EAccessModifier.Private)]
	public int chunkViewDim;

	// Token: 0x040031D8 RID: 12760
	[PublicizedFrom(EAccessModifier.Private)]
	public PlayerProfile playerProfile;

	// Token: 0x040031D9 RID: 12761
	[PublicizedFrom(EAccessModifier.Private)]
	public int nearEntityId;
}
