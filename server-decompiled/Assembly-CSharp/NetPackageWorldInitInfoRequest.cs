using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x0200087E RID: 2174
[Preserve]
public class NetPackageWorldInitInfoRequest : NetPackage
{
	// Token: 0x170006B8 RID: 1720
	// (get) Token: 0x06003EE6 RID: 16102 RVA: 0x00020119 File Offset: 0x0001E319
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToServer;
		}
	}

	// Token: 0x06003EE7 RID: 16103 RVA: 0x0012AC85 File Offset: 0x00128E85
	public NetPackageWorldInitInfoRequest Setup()
	{
		return this;
	}

	// Token: 0x06003EE8 RID: 16104 RVA: 0x000027FC File Offset: 0x000009FC
	public override void read(PooledBinaryReader _br)
	{
	}

	// Token: 0x06003EE9 RID: 16105 RVA: 0x0017C2CF File Offset: 0x0017A4CF
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
	}

	// Token: 0x06003EEA RID: 16106 RVA: 0x0018E048 File Offset: 0x0018C248
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
