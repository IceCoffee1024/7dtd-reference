using System;
using UnityEngine.Scripting;

// Token: 0x02000868 RID: 2152
[Preserve]
public class NetPackageWallVolume : NetPackage
{
	// Token: 0x06003E5F RID: 15967 RVA: 0x0018B874 File Offset: 0x00189A74
	public NetPackageWallVolume Setup(int _id, WallVolume _wallVolume)
	{
		this.id = _id;
		this.wallVolume = _wallVolume;
		return this;
	}

	// Token: 0x1700069F RID: 1695
	// (get) Token: 0x06003E60 RID: 15968 RVA: 0x0002F260 File Offset: 0x0002D460
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003E61 RID: 15969 RVA: 0x0018B885 File Offset: 0x00189A85
	public override void read(PooledBinaryReader _reader)
	{
		this.id = _reader.ReadInt32();
		this.wallVolume = WallVolume.Read(_reader);
	}

	// Token: 0x06003E62 RID: 15970 RVA: 0x0018B89F File Offset: 0x00189A9F
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.id);
		this.wallVolume.Write(_writer);
	}

	// Token: 0x06003E63 RID: 15971 RVA: 0x0018B8C0 File Offset: 0x00189AC0
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

	// Token: 0x040032A8 RID: 12968
	[PublicizedFrom(EAccessModifier.Private)]
	public int id;

	// Token: 0x040032A9 RID: 12969
	[PublicizedFrom(EAccessModifier.Private)]
	public WallVolume wallVolume;
}
