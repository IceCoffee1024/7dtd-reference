using System;
using System.Collections.Concurrent;
using System.IO;
using Noemax.GZip;

// Token: 0x0200039E RID: 926
public class DynamicMeshRegionDataStorage
{
	// Token: 0x06001B82 RID: 7042 RVA: 0x000A3485 File Offset: 0x000A1685
	public void ClearQueues()
	{
		Log.Out("Clearing queues.");
		this.CleanUpAndSave();
		this.ChunkData.Clear();
		Log.Out("Cleared queues.");
	}

	// Token: 0x06001B83 RID: 7043 RVA: 0x000A34AC File Offset: 0x000A16AC
	[PublicizedFrom(EAccessModifier.Private)]
	public bool TryLoadItem(DynamicMeshRegionDataWrapper wrapper, bool releaseLock)
	{
		object @lock = this._lock;
		lock (@lock)
		{
			string text = wrapper.Path();
			if (!SdFile.Exists(text))
			{
				wrapper.StateInfo |= DynamicMeshStates.FileMissing;
				return false;
			}
			int num = (wrapper.StateInfo.HasFlag(DynamicMeshStates.SaveRequired) ? 1 : 0);
			bool flag2 = false;
			if (num == 0 && flag2)
			{
				using (Stream stream = SdFile.OpenRead(text))
				{
					int num2 = 0;
					this.FileMemoryStream.Position = 0L;
					this.FileMemoryStream.SetLength(0L);
					using (DeflateInputStream deflateInputStream = new DeflateInputStream(stream))
					{
						int num3;
						do
						{
							num3 = deflateInputStream.Read(this.Buffer, 0, this.Buffer.Length);
							num2 += num3;
							if (num3 > 0)
							{
								this.FileMemoryStream.Write(this.Buffer, 0, num3);
							}
						}
						while (num3 == this.Buffer.Length);
						DynamicMeshChunkData.LoadFromStream(this.FileMemoryStream);
						if (DynamicMeshManager.DoLog)
						{
							Log.Out(string.Concat(new string[]
							{
								"LOAD FILE SIZE: ",
								text,
								" @ ",
								this.FileMemoryStream.Length.ToString(),
								" OR ",
								num2.ToString()
							}));
						}
					}
				}
			}
			if (releaseLock)
			{
				wrapper.TryExit("tryLoadItem");
			}
			wrapper.StateInfo &= ~DynamicMeshStates.LoadRequired;
			wrapper.StateInfo &= ~DynamicMeshStates.FileMissing;
			wrapper.StateInfo &= ~DynamicMeshStates.LoadBoosted;
		}
		return true;
	}

	// Token: 0x06001B84 RID: 7044 RVA: 0x000A36A4 File Offset: 0x000A18A4
	public void AddSaveRequest(long key, DynamicMeshChunkData data, int length, bool requestRegionUpdate, bool unloadImmediately, bool loadInWorld)
	{
		if (!DynamicMeshManager.CONTENT_ENABLED)
		{
			return;
		}
		DynamicMeshRegionDataWrapper wrapper = this.GetWrapper(key);
		if (DynamicMeshManager.DoLog)
		{
			Log.Out("Adding Saving " + wrapper.ToDebugLocation() + ":" + length.ToString());
		}
		string text = "addSave";
		if (!wrapper.GetLock(text))
		{
			Log.Warning("Could not get lock on save request: " + wrapper.ToDebugLocation());
			return;
		}
		this.SaveItem(wrapper);
		if (requestRegionUpdate)
		{
			DynamicMeshThread.AddRegionUpdateData(wrapper.X, wrapper.Z, false);
		}
		if (loadInWorld)
		{
			DynamicMeshThread.ChunkReadyForCollection.Add(new Vector2i(wrapper.X, wrapper.Z));
		}
		this.ClearLock(wrapper, "_SAVERELEASE_");
	}

	// Token: 0x06001B85 RID: 7045 RVA: 0x000A375C File Offset: 0x000A195C
	[PublicizedFrom(EAccessModifier.Private)]
	public void SaveItem(DynamicMeshRegionDataWrapper wrapper)
	{
		DynamicMeshChunkData dynamicMeshChunkData = null;
		wrapper.GetLock("saveItem");
		wrapper.ClearUnloadMarks();
		wrapper.StateInfo &= ~DynamicMeshStates.SaveRequired;
		string text = wrapper.Path();
		if (dynamicMeshChunkData == null)
		{
			if (DynamicMeshManager.DoLog)
			{
				Log.Out("Deleting null bytes " + wrapper.ToDebugLocation());
			}
			if (SdFile.Exists(text))
			{
				SdFile.Delete(text);
				return;
			}
		}
		else
		{
			if (DynamicMeshManager.DoLog)
			{
				Log.Out("Saving to disk " + wrapper.ToDebugLocation());
			}
			int streamSize = dynamicMeshChunkData.GetStreamSize();
			int num = 0;
			byte[] fromPool = DynamicMeshThread.ChunkDataQueue.GetFromPool(streamSize);
			using (MemoryStream memoryStream = new MemoryStream(fromPool))
			{
				using (PooledBinaryWriter pooledBinaryWriter = MemoryPools.poolBinaryWriter.AllocSync(false))
				{
					pooledBinaryWriter.SetBaseStream(memoryStream);
					dynamicMeshChunkData.UpdateTime = (dynamicMeshChunkData.UpdateTime = (int)(DateTime.UtcNow - DynamicMeshFile.ItemMin).TotalSeconds);
					dynamicMeshChunkData.Write(pooledBinaryWriter);
					num = (int)pooledBinaryWriter.BaseStream.Position;
				}
			}
			DynamicMeshUnity.EnsureDMDirectoryExists();
			using (Stream stream = SdFile.Create(text))
			{
				using (DeflateOutputStream deflateOutputStream = new DeflateOutputStream(stream, 3, false))
				{
					deflateOutputStream.Write(fromPool, 0, num);
				}
			}
			wrapper.StateInfo &= ~DynamicMeshStates.FileMissing;
			DynamicMeshThread.ChunkDataQueue.ManuallyReleaseBytes(fromPool);
		}
	}

	// Token: 0x06001B86 RID: 7046 RVA: 0x000027FC File Offset: 0x000009FC
	public void CleanUpAndSave()
	{
	}

	// Token: 0x06001B87 RID: 7047 RVA: 0x000A3900 File Offset: 0x000A1B00
	public bool ClearLock(DynamicMeshRegionDataWrapper wrapper, string debug)
	{
		return wrapper.TryExit("ClearLock " + debug);
	}

	// Token: 0x06001B88 RID: 7048 RVA: 0x000A391C File Offset: 0x000A1B1C
	[PublicizedFrom(EAccessModifier.Private)]
	public bool ClearLock(long worldPosition, string debug)
	{
		DynamicMeshRegionDataWrapper wrapper = this.GetWrapper(worldPosition);
		return this.ClearLock(wrapper, debug);
	}

	// Token: 0x06001B89 RID: 7049 RVA: 0x000A393C File Offset: 0x000A1B3C
	public DynamicMeshRegionDataWrapper GetWrapper(long key)
	{
		DynamicMeshRegionDataWrapper dynamicMeshRegionDataWrapper;
		if (!this.ChunkData.TryGetValue(key, out dynamicMeshRegionDataWrapper))
		{
			dynamicMeshRegionDataWrapper = DynamicMeshRegionDataWrapper.Create(key);
			if (!this.ChunkData.TryAdd(key, dynamicMeshRegionDataWrapper))
			{
				this.ChunkData.TryGetValue(key, out dynamicMeshRegionDataWrapper);
				Log.Error("Request failed to add data: " + DynamicMeshUnity.GetDebugPositionKey(key));
			}
		}
		return dynamicMeshRegionDataWrapper;
	}

	// Token: 0x06001B8A RID: 7050 RVA: 0x000A3994 File Offset: 0x000A1B94
	public void LoadRegion(DyMeshRegionLoadRequest load)
	{
		DynamicMeshRegionDataWrapper wrapper = this.GetWrapper(load.Key);
		wrapper.GetLock("loadRegion");
		string text = wrapper.Path();
		if (SdFile.Exists(text))
		{
			try
			{
				using (Stream stream = SdFile.OpenRead(text))
				{
					int num = 0;
					this.FileMemoryStream.Position = 0L;
					this.FileMemoryStream.SetLength(0L);
					using (DeflateInputStream deflateInputStream = new DeflateInputStream(stream))
					{
						int num2;
						do
						{
							num2 = deflateInputStream.Read(this.Buffer, 0, this.Buffer.Length);
							num += num2;
							if (num2 > 0)
							{
								this.FileMemoryStream.Write(this.Buffer, 0, num2);
							}
						}
						while (num2 == this.Buffer.Length);
						this.FileMemoryStream.Position = 0L;
						using (PooledBinaryReader pooledBinaryReader = MemoryPools.poolBinaryReader.AllocSync(false))
						{
							pooledBinaryReader.SetBaseStream(this.FileMemoryStream);
							double totalSeconds = (DateTime.UtcNow - DynamicMeshFile.ItemMin).TotalSeconds;
							DynamicMeshVoxelRegionLoad.LoadRegionFromFile(pooledBinaryReader, load);
						}
						if (DynamicMeshManager.DoLog)
						{
							Log.Out(string.Concat(new string[]
							{
								"LOAD FILE SIZE: ",
								text,
								" @ ",
								this.FileMemoryStream.Length.ToString(),
								" OR ",
								num.ToString()
							}));
						}
					}
				}
			}
			catch (Exception ex)
			{
				Log.Out("Read file error on dymesh: " + ex.Message + ". Deleting corrupted file.");
				try
				{
					SdFile.Delete(text);
				}
				catch (Exception)
				{
					Log.Out("Unable to delete dymesh file. You should manually delete: " + text);
				}
			}
		}
		this.ClearLock(wrapper, "loadRegionRelease");
	}

	// Token: 0x06001B8B RID: 7051 RVA: 0x000A3BC8 File Offset: 0x000A1DC8
	public void SaveRegion(DynamicMeshThread.ThreadRegion region, Vector3i worldPosition, VoxelMesh opaque, VoxelMeshTerrain terrain)
	{
		long key = region.Key;
		DynamicMeshRegionDataWrapper wrapper = this.GetWrapper(key);
		wrapper.GetLock("saveRegion");
		DynamicMeshUnity.EnsureDMDirectoryExists();
		using (Stream stream = SdFile.Open(wrapper.Path(), FileMode.Create, FileAccess.Write, FileShare.Read))
		{
			using (DeflateOutputStream deflateOutputStream = new DeflateOutputStream(stream, 3, false))
			{
				using (PooledBinaryWriter pooledBinaryWriter = MemoryPools.poolBinaryWriter.AllocSync(false))
				{
					pooledBinaryWriter.SetBaseStream(deflateOutputStream);
					int num = (int)(DateTime.UtcNow - DynamicMeshFile.ItemMin).TotalSeconds;
					DynamicMeshVoxelRegionLoad.SaveRegionToFile(pooledBinaryWriter, opaque, terrain, worldPosition, num, null, null);
				}
			}
		}
		this.ClearLock(wrapper, "saveRegionRelease");
	}

	// Token: 0x040011BB RID: 4539
	public ConcurrentDictionary<long, DynamicMeshRegionDataWrapper> ChunkData = new ConcurrentDictionary<long, DynamicMeshRegionDataWrapper>();

	// Token: 0x040011BC RID: 4540
	[PublicizedFrom(EAccessModifier.Private)]
	public object _lock = new object();

	// Token: 0x040011BD RID: 4541
	[PublicizedFrom(EAccessModifier.Private)]
	public MemoryStream FileMemoryStream = new MemoryStream();

	// Token: 0x040011BE RID: 4542
	[PublicizedFrom(EAccessModifier.Private)]
	public byte[] Buffer = new byte[2048];

	// Token: 0x040011BF RID: 4543
	public int MaxAllowedItems;
}
