using System;
using UnityEngine.Scripting;

// Token: 0x0200087C RID: 2172
[Preserve]
public class NetPackageWorldSpawnPoints : NetPackage
{
	// Token: 0x06003F5C RID: 16220 RVA: 0x0018E2AB File Offset: 0x0018C4AB
	public NetPackageWorldSpawnPoints Setup(SpawnPointList _spawnPoints)
	{
		this.spawnPoints = _spawnPoints;
		return this;
	}

	// Token: 0x06003F5D RID: 16221 RVA: 0x0018E2B5 File Offset: 0x0018C4B5
	public override void read(PooledBinaryReader _reader)
	{
		this.spawnPoints = new SpawnPointList();
		this.spawnPoints.Read(_reader);
	}

	// Token: 0x06003F5E RID: 16222 RVA: 0x0018E2CE File Offset: 0x0018C4CE
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		this.spawnPoints.Write(_writer);
	}

	// Token: 0x06003F5F RID: 16223 RVA: 0x0018E2E3 File Offset: 0x0018C4E3
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		_callbacks.SetSpawnPointList(this.spawnPoints);
	}

	// Token: 0x06003F60 RID: 16224 RVA: 0x0018E2F1 File Offset: 0x0018C4F1
	public override int GetLength()
	{
		if (this.spawnPoints == null)
		{
			return 0;
		}
		return this.spawnPoints.Count * 20;
	}

	// Token: 0x1700069A RID: 1690
	// (get) Token: 0x06003F61 RID: 16225 RVA: 0x0002F168 File Offset: 0x0002D368
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x0400333C RID: 13116
	[PublicizedFrom(EAccessModifier.Protected)]
	public SpawnPointList spawnPoints;
}
