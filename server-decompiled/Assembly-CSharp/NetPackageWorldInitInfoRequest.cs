using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x0200087B RID: 2171
[Preserve]
public class NetPackageWorldInitInfoRequest : NetPackage
{
	// Token: 0x17000699 RID: 1689
	// (get) Token: 0x06003F55 RID: 16213 RVA: 0x00020021 File Offset: 0x0001E221
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToServer;
		}
	}

	// Token: 0x06003F56 RID: 16214 RVA: 0x0012A165 File Offset: 0x00128365
	public NetPackageWorldInitInfoRequest Setup()
	{
		return this;
	}

	// Token: 0x06003F57 RID: 16215 RVA: 0x000027FC File Offset: 0x000009FC
	public override void read(PooledBinaryReader _br)
	{
	}

	// Token: 0x06003F58 RID: 16216 RVA: 0x0017BC33 File Offset: 0x00179E33
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
	}

	// Token: 0x06003F59 RID: 16217 RVA: 0x0018E23C File Offset: 0x0018C43C
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			Log.Error("[NetPackageWorldInitInfoRequest] world is null");
			return;
		}
		IChunkProvider chunkProvider = _world.ChunkCache.ChunkProvider;
		EventPrefabs eventPrefabs = ((chunkProvider != null) ? chunkProvider.GetEventPrefabs() : null);
		List<PrefabInstance.Serializable> list;
		if (eventPrefabs != null)
		{
			list = eventPrefabs.GetPrefabsSerialized();
		}
		else
		{
			list = new List<PrefabInstance.Serializable>();
			Log.Error("[NetPackageWorldInitInfoRequest] EventPrefabs not available");
		}
		List<ValueTuple<int, WallVolume>> allWallVolumes = _world.GetAllWallVolumes();
		base.Sender.SendPackage(NetPackageManager.GetPackage<NetPackageWorldInitInfo>().Setup(list, allWallVolumes));
	}

	// Token: 0x06003F5A RID: 16218 RVA: 0x00020021 File Offset: 0x0001E221
	public override int GetLength()
	{
		return 1;
	}
}
