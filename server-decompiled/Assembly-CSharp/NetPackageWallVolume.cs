using System;
using UnityEngine.Scripting;

// Token: 0x02000872 RID: 2162
[Preserve]
public class NetPackageWallVolume : NetPackage
{
	// Token: 0x06003E82 RID: 16002 RVA: 0x0018C91C File Offset: 0x0018AB1C
	public NetPackageWallVolume Setup(int _id, WallVolume _wallVolume)
	{
		this.id = _id;
		this.wallVolume = _wallVolume;
		return this;
	}

	// Token: 0x170006A5 RID: 1701
	// (get) Token: 0x06003E83 RID: 16003 RVA: 0x0002F260 File Offset: 0x0002D460
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003E84 RID: 16004 RVA: 0x0018C92D File Offset: 0x0018AB2D
	public override void read(PooledBinaryReader _reader)
	{
		this.id = _reader.ReadInt32();
		this.wallVolume = WallVolume.Read(_reader);
	}

	// Token: 0x06003E85 RID: 16005 RVA: 0x0018C947 File Offset: 0x0018AB47
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.id);
		this.wallVolume.Write(_writer);
	}

	// Token: 0x06003E86 RID: 16006 RVA: 0x0018C968 File Offset: 0x0018AB68
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null || !_callbacks.worldInitInfoReceived)
		{
			return;
		}
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsClient)
		{
			_world.AddWallVolumeAt(this.id, this.wallVolume);
		}
	}

	// Token: 0x040032C8 RID: 13000
	[PublicizedFrom(EAccessModifier.Private)]
	public int id;

	// Token: 0x040032C9 RID: 13001
	[PublicizedFrom(EAccessModifier.Private)]
	public WallVolume wallVolume;
}
