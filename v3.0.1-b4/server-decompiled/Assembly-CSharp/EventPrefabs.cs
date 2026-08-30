using System;
using System.Collections.Generic;
using System.IO;

// Token: 0x02000917 RID: 2327
public class EventPrefabs
{
	// Token: 0x06004397 RID: 17303 RVA: 0x001A5F34 File Offset: 0x001A4134
	public EventPrefabs(World world, DynamicPrefabDecorator prefabDecorator, RegionFileManager regionFileManager)
	{
		this.world = world;
		this.prefabCache = world.m_PrefabCache;
		this.dpd = prefabDecorator;
		this.rfm = regionFileManager;
		this.saveWriter = new ThreadedFileWriterQueue("EventPrefabs.Save", Path.Combine(GameIO.GetSaveGameDir(), "eventprefabs.dat"));
	}

	// Token: 0x06004398 RID: 17304 RVA: 0x001A5F92 File Offset: 0x001A4192
	public void GetPrefabs(List<PrefabInstance> prefabs)
	{
		prefabs.AddRange(this.prefabs);
	}

	// Token: 0x06004399 RID: 17305 RVA: 0x001A5FA0 File Offset: 0x001A41A0
	public List<PrefabInstance.Serializable> GetPrefabsSerialized()
	{
		List<PrefabInstance.Serializable> list = new List<PrefabInstance.Serializable>();
		foreach (PrefabInstance prefabInstance in this.prefabs)
		{
			list.Add(prefabInstance.GetSerializable());
		}
		return list;
	}

	// Token: 0x0600439A RID: 17306 RVA: 0x001A6000 File Offset: 0x001A4200
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

	// Token: 0x0600439B RID: 17307 RVA: 0x001A6140 File Offset: 0x001A4340
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

	// Token: 0x0600439C RID: 17308 RVA: 0x001A620C File Offset: 0x001A440C
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

	// Token: 0x0600439D RID: 17309 RVA: 0x001A62F4 File Offset: 0x001A44F4
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

	// Token: 0x04003658 RID: 13912
	[PublicizedFrom(EAccessModifier.Private)]
	public World world;

	// Token: 0x04003659 RID: 13913
	[PublicizedFrom(EAccessModifier.Private)]
	public PrefabCache prefabCache;

	// Token: 0x0400365A RID: 13914
	[PublicizedFrom(EAccessModifier.Private)]
	public DynamicPrefabDecorator dpd;

	// Token: 0x0400365B RID: 13915
	[PublicizedFrom(EAccessModifier.Private)]
	public RegionFileManager rfm;

	// Token: 0x0400365C RID: 13916
	[PublicizedFrom(EAccessModifier.Private)]
	public List<PrefabInstance> prefabs = new List<PrefabInstance>();

	// Token: 0x0400365D RID: 13917
	[PublicizedFrom(EAccessModifier.Private)]
	public ThreadedFileWriterQueue saveWriter;

	// Token: 0x0400365E RID: 13918
	[PublicizedFrom(EAccessModifier.Private)]
	public bool needsSaving;

	// Token: 0x0400365F RID: 13919
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cSaveVersion = 1;

	// Token: 0x04003660 RID: 13920
	[PublicizedFrom(EAccessModifier.Private)]
	public const string cSaveFilename = "eventprefabs.dat";
}
