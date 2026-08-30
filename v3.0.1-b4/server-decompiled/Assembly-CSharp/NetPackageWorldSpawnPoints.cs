using System;
using UnityEngine.Scripting;

// Token: 0x02000870 RID: 2160
[Preserve]
public class NetPackageWorldSpawnPoints : NetPackage
{
	// Token: 0x06003EF7 RID: 16119 RVA: 0x0018C1DB File Offset: 0x0018A3DB
	public NetPackageWorldSpawnPoints Setup(SpawnPointList _spawnPoints)
	{
		this.spawnPoints = _spawnPoints;
		return this;
	}

	// Token: 0x06003EF8 RID: 16120 RVA: 0x0018C1E5 File Offset: 0x0018A3E5
	public override void read(PooledBinaryReader _reader)
	{
		this.spawnPoints = new SpawnPointList();
		this.spawnPoints.Read(_reader);
	}

	// Token: 0x06003EF9 RID: 16121 RVA: 0x0018C1FE File Offset: 0x0018A3FE
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		this.spawnPoints.Write(_writer);
	}

	// Token: 0x06003EFA RID: 16122 RVA: 0x0018C213 File Offset: 0x0018A413
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		_callbacks.SetSpawnPointList(this.spawnPoints);
	}

	// Token: 0x06003EFB RID: 16123 RVA: 0x0018C221 File Offset: 0x0018A421
	public override int GetLength()
	{
		if (this.spawnPoints == null)
		{
			return 0;
		}
		return this.spawnPoints.Count * 20;
	}

	// Token: 0x1700068E RID: 1678
	// (get) Token: 0x06003EFC RID: 16124 RVA: 0x0002EEAC File Offset: 0x0002D0AC
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x040032BB RID: 12987
	[PublicizedFrom(EAccessModifier.Protected)]
	public SpawnPointList spawnPoints;
}
