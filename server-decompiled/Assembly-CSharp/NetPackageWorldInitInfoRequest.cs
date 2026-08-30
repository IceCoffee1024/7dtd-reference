using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x0200086F RID: 2159
[Preserve]
public class NetPackageWorldInitInfoRequest : NetPackage
{
	// Token: 0x1700068D RID: 1677
	// (get) Token: 0x06003EF0 RID: 16112 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToServer;
		}
	}

	// Token: 0x06003EF1 RID: 16113 RVA: 0x001290F5 File Offset: 0x001272F5
	public NetPackageWorldInitInfoRequest Setup()
	{
		return this;
	}

	// Token: 0x06003EF2 RID: 16114 RVA: 0x000027FC File Offset: 0x000009FC
	public override void read(PooledBinaryReader _br)
	{
	}

	// Token: 0x06003EF3 RID: 16115 RVA: 0x001799B3 File Offset: 0x00177BB3
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
	}

	// Token: 0x06003EF4 RID: 16116 RVA: 0x0018C16C File Offset: 0x0018A36C
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

	// Token: 0x06003EF5 RID: 16117 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override int GetLength()
	{
		return 1;
	}
}
