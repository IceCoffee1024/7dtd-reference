using System;
using UnityEngine.Scripting;

// Token: 0x02000859 RID: 2137
[Preserve]
public class NetPackageWallVolume : NetPackage
{
	// Token: 0x06003E5F RID: 15967 RVA: 0x00189918 File Offset: 0x00187B18
	public NetPackageWallVolume Setup(int _id, WallVolume _wallVolume)
	{
		this.id = _id;
		this.wallVolume = _wallVolume;
		return this;
	}

	// Token: 0x17000674 RID: 1652
	// (get) Token: 0x06003E60 RID: 15968 RVA: 0x0002EEAC File Offset: 0x0002D0AC
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003E61 RID: 15969 RVA: 0x00189929 File Offset: 0x00187B29
	public override void read(PooledBinaryReader _reader)
	{
		this.id = _reader.ReadInt32();
		this.wallVolume = WallVolume.Read(_reader);
	}

	// Token: 0x06003E62 RID: 15970 RVA: 0x00189943 File Offset: 0x00187B43
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.id);
		this.wallVolume.Write(_writer);
	}

	// Token: 0x06003E63 RID: 15971 RVA: 0x00189964 File Offset: 0x00187B64
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

	// Token: 0x06003E64 RID: 15972 RVA: 0x0017C0EC File Offset: 0x0017A2EC
	public override int GetLength()
	{
		return 29;
	}

	// Token: 0x0400323C RID: 12860
	[PublicizedFrom(EAccessModifier.Private)]
	public int id;

	// Token: 0x0400323D RID: 12861
	[PublicizedFrom(EAccessModifier.Private)]
	public WallVolume wallVolume;
}
