using System;
using UnityEngine.Scripting;

// Token: 0x02000889 RID: 2185
[Preserve]
public class NetPackageWorldSpawnPoints : NetPackage
{
	// Token: 0x06003F0F RID: 16143 RVA: 0x0018F15F File Offset: 0x0018D35F
	public NetPackageWorldSpawnPoints Setup(SpawnPointList _spawnPoints)
	{
		this.spawnPoints = _spawnPoints;
		return this;
	}

	// Token: 0x06003F10 RID: 16144 RVA: 0x0018F169 File Offset: 0x0018D369
	public override void read(PooledBinaryReader _reader)
	{
		this.spawnPoints = new SpawnPointList();
		this.spawnPoints.Read(_reader);
	}

	// Token: 0x06003F11 RID: 16145 RVA: 0x0018F182 File Offset: 0x0018D382
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		this.spawnPoints.Write(_writer);
	}

	// Token: 0x06003F12 RID: 16146 RVA: 0x0018F197 File Offset: 0x0018D397
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		_callbacks.SetSpawnPointList(this.spawnPoints);
	}

	// Token: 0x170006BF RID: 1727
	// (get) Token: 0x06003F13 RID: 16147 RVA: 0x0002F260 File Offset: 0x0002D460
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x04003347 RID: 13127
	[PublicizedFrom(EAccessModifier.Protected)]
	public SpawnPointList spawnPoints;
}
