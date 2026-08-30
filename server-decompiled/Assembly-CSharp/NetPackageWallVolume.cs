using System;
using UnityEngine.Scripting;

// Token: 0x02000865 RID: 2149
[Preserve]
public class NetPackageWallVolume : NetPackage
{
	// Token: 0x06003EC4 RID: 16068 RVA: 0x0018B9E8 File Offset: 0x00189BE8
	public NetPackageWallVolume Setup(int _id, WallVolume _wallVolume)
	{
		this.id = _id;
		this.wallVolume = _wallVolume;
		return this;
	}

	// Token: 0x17000680 RID: 1664
	// (get) Token: 0x06003EC5 RID: 16069 RVA: 0x0002F168 File Offset: 0x0002D368
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003EC6 RID: 16070 RVA: 0x0018B9F9 File Offset: 0x00189BF9
	public override void read(PooledBinaryReader _reader)
	{
		this.id = _reader.ReadInt32();
		this.wallVolume = WallVolume.Read(_reader);
	}

	// Token: 0x06003EC7 RID: 16071 RVA: 0x0018BA13 File Offset: 0x00189C13
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.id);
		this.wallVolume.Write(_writer);
	}

	// Token: 0x06003EC8 RID: 16072 RVA: 0x0018BA34 File Offset: 0x00189C34
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

	// Token: 0x06003EC9 RID: 16073 RVA: 0x0017E4C4 File Offset: 0x0017C6C4
	public override int GetLength()
	{
		return 29;
	}

	// Token: 0x040032BD RID: 12989
	[PublicizedFrom(EAccessModifier.Private)]
	public int id;

	// Token: 0x040032BE RID: 12990
	[PublicizedFrom(EAccessModifier.Private)]
	public WallVolume wallVolume;
}
