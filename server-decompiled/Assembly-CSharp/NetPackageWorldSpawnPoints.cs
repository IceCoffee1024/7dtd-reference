using System;
using UnityEngine.Scripting;

// Token: 0x0200087F RID: 2175
[Preserve]
public class NetPackageWorldSpawnPoints : NetPackage
{
	// Token: 0x06003EEC RID: 16108 RVA: 0x0018E0B7 File Offset: 0x0018C2B7
	public NetPackageWorldSpawnPoints Setup(SpawnPointList _spawnPoints)
	{
		this.spawnPoints = _spawnPoints;
		return this;
	}

	// Token: 0x06003EED RID: 16109 RVA: 0x0018E0C1 File Offset: 0x0018C2C1
	public override void read(PooledBinaryReader _reader)
	{
		this.spawnPoints = new SpawnPointList();
		this.spawnPoints.Read(_reader);
	}

	// Token: 0x06003EEE RID: 16110 RVA: 0x0018E0DA File Offset: 0x0018C2DA
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		this.spawnPoints.Write(_writer);
	}

	// Token: 0x06003EEF RID: 16111 RVA: 0x0018E0EF File Offset: 0x0018C2EF
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		_callbacks.SetSpawnPointList(this.spawnPoints);
	}

	// Token: 0x170006B9 RID: 1721
	// (get) Token: 0x06003EF0 RID: 16112 RVA: 0x0002F260 File Offset: 0x0002D460
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x04003327 RID: 13095
	[PublicizedFrom(EAccessModifier.Protected)]
	public SpawnPointList spawnPoints;
}
