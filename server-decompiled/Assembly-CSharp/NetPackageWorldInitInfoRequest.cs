using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000888 RID: 2184
[Preserve]
public class NetPackageWorldInitInfoRequest : NetPackage
{
	// Token: 0x170006BE RID: 1726
	// (get) Token: 0x06003F09 RID: 16137 RVA: 0x00020119 File Offset: 0x0001E319
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToServer;
		}
	}

	// Token: 0x06003F0A RID: 16138 RVA: 0x0012B879 File Offset: 0x00129A79
	public NetPackageWorldInitInfoRequest Setup()
	{
		return this;
	}

	// Token: 0x06003F0B RID: 16139 RVA: 0x000027FC File Offset: 0x000009FC
	public override void read(PooledBinaryReader _br)
	{
	}

	// Token: 0x06003F0C RID: 16140 RVA: 0x0017D267 File Offset: 0x0017B467
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
	}

	// Token: 0x06003F0D RID: 16141 RVA: 0x0018F0F0 File Offset: 0x0018D2F0
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
}
