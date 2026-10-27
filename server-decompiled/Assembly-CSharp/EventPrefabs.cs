using System;
using System.Collections.Generic;
using System.IO;

// Token: 0x02000936 RID: 2358
public class EventPrefabs
{
	// Token: 0x060043CD RID: 17357 RVA: 0x001A9690 File Offset: 0x001A7890
	public EventPrefabs(World world, DynamicPrefabDecorator prefabDecorator, RegionFileManager regionFileManager)
	{
		this.world = world;
		this.prefabCache = world.m_PrefabCache;
		this.dpd = prefabDecorator;
		this.rfm = regionFileManager;
		this.saveWriter = new ThreadedFileWriterQueue("EventPrefabs.Save", Path.Combine(GameIO.GetSaveGameDir(), "eventprefabs.dat"));
	}

	// Token: 0x060043CE RID: 17358 RVA: 0x001A96EE File Offset: 0x001A78EE
	public void GetPrefabs(List<PrefabInstance> prefabs)
	{
		prefabs.AddRange(this.prefabs);
	}

	// Token: 0x060043CF RID: 17359 RVA: 0x001A96FC File Offset: 0x001A78FC
	public List<PrefabInstance.Serializable> GetPrefabsSerialized()
	{
		List<PrefabInstance.Serializable> list = new List<PrefabInstance.Serializable>();
		foreach (PrefabInstance prefabInstance in this.prefabs)
		{
			list.Add(prefabInstance.GetSerializable());
		}
		return list;
	}

	// Token: 0x060043D0 RID: 17360 RVA: 0x001A975C File Offset: 0x001A795C
	public PrefabInstance TryPlaceAt(string prefabName, byte rotation, Vector3i position, bool yIsGroundLevel = true)
	{
		Prefab prefabRotated = this.prefabCache.GetPrefabRotated(prefabName, (int)rotation, true, true, false, false);
		if (prefabRotated == null)
		{
			Log.Error("[EventPrefabs] cannot place " + prefabName + ", prefab not found");
			return null;
		}
		if (yIsGroundLevel)
		{
			position.y += prefabRotated.yOffset;
		}
		PrefabInstance prefabInstance = new PrefabInstance(this.dpd.GetNextId(), prefabRotated.location, position, rotation, prefabRotated, 0);
		HashSetLong occupiedChunks = prefabInstance.GetOccupiedChunks();
		ChunkProtectionLevel chunkProtectionLevel;
		if (!this.rfm.TryResetChunks(occupiedChunks, ChunkProtectionLevel.All, out chunkProtectionLevel))
		{
			Log.Error(string.Format("[EventPrefabs] cannot place {0} at ({1}), {2}. Chunks could not be reset, protection level: {3}", new object[] { prefabName, position, rotation, chunkProtectionLevel }));
			return null;
		}
		DecoManager.Instance.ClearDecoObjectsInArea(prefabInstance.boundingBoxPosition, prefabInstance.boundingBoxSize);
		Log.Out(string.Format("[EventPrefabs] placing {0} at ({1}), {2}", prefabName, position, rotation));
		this.dpd.AddEventPrefab(prefabInstance);
		this.prefabs.Add(prefabInstance);
		this.rfm.AddGroupedChunks(occupiedChunks);
		SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageEventPrefab>().Setup(NetPackageEventPrefab.Operation.Add, prefabInstance), false, -1, -1, -1, null, 192, false);
		this.needsSaving = true;
		return prefabInstance;
	}

	// Token: 0x060043D1 RID: 17361 RVA: 0x001A989C File Offset: 0x001A7A9C
	public bool Remove(PrefabInstance pi)
	{
		HashSetLong occupiedChunks = pi.GetOccupiedChunks();
		ChunkProtectionLevel chunkProtectionLevel;
		if (!this.rfm.TryResetChunks(occupiedChunks, ChunkProtectionLevel.All, out chunkProtectionLevel))
		{
			Log.Error(string.Format("[EventPrefabs] cannot remove {0}. Chunks could not be reset, protection level: {1}", pi, chunkProtectionLevel));
			return false;
		}
		if (!this.prefabs.Remove(pi))
		{
			Log.Error(string.Format("[EventPrefabs] cannot remove {0}. Not an event prefab", pi));
			return false;
		}
		this.world.RemoveTriggerVolumesFor(pi);
		this.world.RemoveSleeperVolumesFor(pi);
		this.world.RemoveWallVolumesFor(pi);
		this.dpd.RemoveEventPrefab(pi);
		this.rfm.RemoveGroupedChunks(occupiedChunks);
		SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageEventPrefab>().Setup(NetPackageEventPrefab.Operation.Remove, pi), false, -1, -1, -1, null, 192, false);
		this.needsSaving = true;
		return true;
	}

	// Token: 0x060043D2 RID: 17362 RVA: 0x001A9968 File Offset: 0x001A7B68
	public void Save(bool waitForComplete = false)
	{
		if (!this.needsSaving)
		{
			return;
		}
		this.needsSaving = false;
		PooledExpandableMemoryStream pooledExpandableMemoryStream = MemoryPools.poolMemoryStream.AllocSync(true);
		using (PooledBinaryWriter pooledBinaryWriter = MemoryPools.poolBinaryWriter.AllocSync(false))
		{
			pooledBinaryWriter.SetBaseStream(pooledExpandableMemoryStream);
			pooledBinaryWriter.Write(1);
			pooledBinaryWriter.Write(this.prefabs.Count);
			foreach (PrefabInstance prefabInstance in this.prefabs)
			{
				pooledBinaryWriter.Write(prefabInstance.prefab.PrefabName);
				StreamUtils.Write(pooledBinaryWriter, prefabInstance.boundingBoxPosition);
				pooledBinaryWriter.Write(prefabInstance.rotation);
			}
		}
		pooledExpandableMemoryStream.Position = 0L;
		this.saveWriter.Write(pooledExpandableMemoryStream, waitForComplete);
	}

	// Token: 0x060043D3 RID: 17363 RVA: 0x001A9A50 File Offset: 0x001A7C50
	public void Load()
	{
		string text = Path.Combine(GameIO.GetSaveGameDir(), "eventprefabs.dat");
		if (!SdFile.Exists(text))
		{
			return;
		}
		try
		{
			Log.Out("[EventPrefabs] loading prefab list");
			using (Stream stream = SdFile.OpenRead(text))
			{
				using (PooledBinaryReader pooledBinaryReader = MemoryPools.poolBinaryReader.AllocSync(false))
				{
					pooledBinaryReader.SetBaseStream(stream);
					pooledBinaryReader.ReadInt32();
					int num = pooledBinaryReader.ReadInt32();
					for (int i = 0; i < num; i++)
					{
						string text2 = pooledBinaryReader.ReadString();
						Vector3i vector3i = StreamUtils.ReadVector3i(pooledBinaryReader);
						byte b = pooledBinaryReader.ReadByte();
						Prefab prefabRotated = this.prefabCache.GetPrefabRotated(text2, (int)b, true, true, false, false);
						PrefabInstance prefabInstance = new PrefabInstance(this.dpd.GetNextId(), prefabRotated.location, vector3i, b, prefabRotated, 0);
						this.prefabs.Add(prefabInstance);
					}
				}
			}
		}
		catch (Exception ex)
		{
			Log.Error("[EventPrefabs] error while reading save data " + ex.Message);
			Log.Exception(ex);
		}
		foreach (PrefabInstance prefabInstance2 in this.prefabs)
		{
			DecoManager.Instance.ClearDecoObjectsInArea(prefabInstance2.boundingBoxPosition, prefabInstance2.boundingBoxSize);
			this.dpd.AddEventPrefab(prefabInstance2);
			this.rfm.AddGroupedChunks(prefabInstance2.GetOccupiedChunks());
		}
	}

	// Token: 0x040036F5 RID: 14069
	[PublicizedFrom(EAccessModifier.Private)]
	public World world;

	// Token: 0x040036F6 RID: 14070
	[PublicizedFrom(EAccessModifier.Private)]
	public PrefabCache prefabCache;

	// Token: 0x040036F7 RID: 14071
	[PublicizedFrom(EAccessModifier.Private)]
	public DynamicPrefabDecorator dpd;

	// Token: 0x040036F8 RID: 14072
	[PublicizedFrom(EAccessModifier.Private)]
	public RegionFileManager rfm;

	// Token: 0x040036F9 RID: 14073
	[PublicizedFrom(EAccessModifier.Private)]
	public List<PrefabInstance> prefabs = new List<PrefabInstance>();

	// Token: 0x040036FA RID: 14074
	[PublicizedFrom(EAccessModifier.Private)]
	public ThreadedFileWriterQueue saveWriter;

	// Token: 0x040036FB RID: 14075
	[PublicizedFrom(EAccessModifier.Private)]
	public bool needsSaving;

	// Token: 0x040036FC RID: 14076
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cSaveVersion = 1;

	// Token: 0x040036FD RID: 14077
	[PublicizedFrom(EAccessModifier.Private)]
	public const string cSaveFilename = "eventprefabs.dat";
}
