using System;
using System.Collections.Concurrent;
using System.IO;
using Noemax.GZip;

// Token: 0x02000365 RID: 869
public class DynamicMeshChunkDataStorage<T> where T : DynamicMeshContainer
{
	// Token: 0x06001947 RID: 6471 RVA: 0x0008E90C File Offset: 0x0008CB0C
	public DynamicMeshChunkDataStorage(int purgeInterval)
	{
		this.PurgeInterval = purgeInterval;
	}

	// Token: 0x06001948 RID: 6472 RVA: 0x0008E974 File Offset: 0x0008CB74
	public void ClearQueues()
	{
		Log.Out("Clearing queues.");
		DynamicMeshChunkDataWrapper dynamicMeshChunkDataWrapper;
		while (this.LoadRequests.TryPop(out dynamicMeshChunkDataWrapper))
		{
		}
		this.CleanUpAndSave();
		this.ChunkData.Clear();
		Log.Out("Cleared queues.");
	}

	// Token: 0x06001949 RID: 6473 RVA: 0x0008E9B5 File Offset: 0x0008CBB5
	public bool IsReadyThreaded()
	{
		return this.MaxAllowedItems == 0 || this.LiveItems < this.MaxAllowedItems - 1;
	}

	// Token: 0x0600194A RID: 6474 RVA: 0x0008E9D1 File Offset: 0x0008CBD1
	[PublicizedFrom(EAccessModifier.Private)]
	public bool ForceLoadItem(DynamicMeshChunkDataWrapper wrapper)
	{
		return this.TryLoadItem(wrapper, false);
	}

	// Token: 0x0600194B RID: 6475 RVA: 0x0008E9DC File Offset: 0x0008CBDC
	[PublicizedFrom(EAccessModifier.Private)]
	public bool TryLoadItem(DynamicMeshChunkDataWrapper wrapper, bool releaseLock)
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
			DynamicMeshChunkData dynamicMeshChunkData;
			bool flag2 = wrapper.TryGetData(out dynamicMeshChunkData, "tryLoadItem");
			if (num == 0 && flag2)
			{
				using (Stream stream = SdFile.OpenRead(text))
				{
					int num2 = 0;
					this.FileMemoryStream.Position = 0L;
					this.FileMemoryStream.SetLength(0L);
					stream.ReadByte();
					stream.ReadByte();
					stream.ReadByte();
					stream.ReadByte();
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
						DynamicMeshChunkData dynamicMeshChunkData2 = DynamicMeshChunkData.LoadFromStream(this.FileMemoryStream);
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
						this.ReplaceData(wrapper, dynamicMeshChunkData2, "loadItem");
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

	// Token: 0x0600194C RID: 6476 RVA: 0x0008EC10 File Offset: 0x0008CE10
	public bool IsUpdating(T item)
	{
		return this.GetWrapper(item.Key).StateInfo.HasFlag(DynamicMeshStates.ThreadUpdating);
	}

	// Token: 0x0600194D RID: 6477 RVA: 0x0008EC38 File Offset: 0x0008CE38
	public void MarkAsUpdating(T item)
	{
		this.GetWrapper(item.Key).StateInfo |= DynamicMeshStates.ThreadUpdating;
	}

	// Token: 0x0600194E RID: 6478 RVA: 0x0008EC58 File Offset: 0x0008CE58
	public void MarkAsUpdated(T item)
	{
		if (item == null)
		{
			return;
		}
		this.GetWrapper(item.Key).StateInfo &= ~DynamicMeshStates.ThreadUpdating;
	}

	// Token: 0x0600194F RID: 6479 RVA: 0x0008EC82 File Offset: 0x0008CE82
	public void MarkAsGenerating(T item)
	{
		this.GetWrapper(item.Key).StateInfo |= DynamicMeshStates.Generating;
	}

	// Token: 0x06001950 RID: 6480 RVA: 0x0008ECA6 File Offset: 0x0008CEA6
	public void MarkAsGenerated(T item)
	{
		if (item == null)
		{
			return;
		}
		this.GetWrapper(item.Key).StateInfo &= ~DynamicMeshStates.Generating;
	}

	// Token: 0x06001951 RID: 6481 RVA: 0x0008ECD4 File Offset: 0x0008CED4
	public bool SaveNetPackageData(int x, int z, byte[] data, int updateTime)
	{
		long itemKey = DynamicMeshUnity.GetItemKey(x, z);
		if (!DynamicMeshManager.CONTENT_ENABLED)
		{
			return false;
		}
		DynamicMeshChunkDataWrapper wrapper = this.GetWrapper(itemKey);
		if (DynamicMeshManager.DoLog)
		{
			Log.Out("Adding Saving from net " + wrapper.ToDebugLocation() + ":" + ((data != null) ? data.Length : 0).ToString());
		}
		string text = "addSave";
		if (!wrapper.GetLock(text))
		{
			Log.Warning("Could not get lock on save request: " + wrapper.ToDebugLocation());
			return false;
		}
		this.ReleaseData(wrapper, "saveRequestNet");
		bool flag = data == null || data.Length == 0;
		string itemPath = DynamicMeshUnity.GetItemPath(itemKey);
		if (flag)
		{
			SdFile.Delete(itemPath);
		}
		else
		{
			DynamicMeshUnity.EnsureDMDirectoryExists();
			using (Stream stream = SdFile.Create(itemPath))
			{
				using (PooledBinaryWriter pooledBinaryWriter = MemoryPools.poolBinaryWriter.AllocSync(false))
				{
					pooledBinaryWriter.SetBaseStream(stream);
					pooledBinaryWriter.Write(data, 0, data.Length);
				}
			}
		}
		DynamicMeshManager instance = DynamicMeshManager.Instance;
		bool flag2 = instance != null && instance.IsInLoadableArea(wrapper.Key);
		if (flag2)
		{
			DynamicMeshThread.AddRegionUpdateData(wrapper.X, wrapper.Z, false);
			DynamicMeshThread.ChunkReadyForCollection.Add(new Vector2i(wrapper.X, wrapper.Z));
		}
		this.ClearLock(wrapper, "_SAVERELEASE_NET_");
		return flag2;
	}

	// Token: 0x06001952 RID: 6482 RVA: 0x0008EE38 File Offset: 0x0008D038
	public void AddSaveRequest(long key, DynamicMeshChunkData data)
	{
		if (!DynamicMeshManager.CONTENT_ENABLED)
		{
			return;
		}
		DynamicMeshChunkDataWrapper wrapper = this.GetWrapper(key);
		if (DynamicMeshManager.DoLog)
		{
			Log.Out("Adding Saving " + wrapper.ToDebugLocation());
		}
		string text = "addSave";
		if (!wrapper.GetLock(text))
		{
			Log.Warning("Could not get lock on save request: " + wrapper.ToDebugLocation());
			return;
		}
		this.ReplaceData(wrapper, data, "saveRequest");
		this.SaveItem(wrapper);
		DynamicMeshThread.ChunkReadyForCollection.Add(new Vector2i(wrapper.X, wrapper.Z));
		this.ClearLock(wrapper, "_SAVERELEASE_");
	}

	// Token: 0x06001953 RID: 6483 RVA: 0x0008EED4 File Offset: 0x0008D0D4
	[PublicizedFrom(EAccessModifier.Private)]
	public void SaveItem(DynamicMeshChunkDataWrapper wrapper)
	{
		wrapper.GetLock("saveItem");
		DynamicMeshChunkData dynamicMeshChunkData;
		wrapper.TryGetData(out dynamicMeshChunkData, "saveItemTry");
		wrapper.ClearUnloadMarks();
		wrapper.StateInfo &= ~DynamicMeshStates.SaveRequired;
		if (DynamicMeshManager.DoLog)
		{
			Log.Out("Saving " + wrapper.ToDebugLocation() + ":" + ((dynamicMeshChunkData != null) ? dynamicMeshChunkData.GetStreamSize().ToString() : null));
		}
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
				using (PooledBinaryWriter pooledBinaryWriter2 = MemoryPools.poolBinaryWriter.AllocSync(false))
				{
					pooledBinaryWriter2.SetBaseStream(stream);
					pooledBinaryWriter2.Write(dynamicMeshChunkData.UpdateTime);
				}
				using (DeflateOutputStream deflateOutputStream = new DeflateOutputStream(stream, 3, false))
				{
					deflateOutputStream.Write(fromPool, 0, num);
				}
			}
			wrapper.StateInfo &= ~DynamicMeshStates.FileMissing;
			DynamicMeshThread.ChunkDataQueue.ManuallyReleaseBytes(fromPool);
			this.ReleaseData(wrapper, "saveItem");
		}
	}

	// Token: 0x06001954 RID: 6484 RVA: 0x000027FC File Offset: 0x000009FC
	public void CleanUpAndSave()
	{
	}

	// Token: 0x06001955 RID: 6485 RVA: 0x0008F104 File Offset: 0x0008D304
	public void MarkForDeletion(long worldPosition)
	{
		this.GetWrapper(worldPosition).StateInfo |= DynamicMeshStates.MarkedForDelete;
	}

	// Token: 0x06001956 RID: 6486 RVA: 0x0008F11B File Offset: 0x0008D31B
	public void MarkForUnload(long worldPosition)
	{
		DynamicMeshChunkDataWrapper wrapper = this.GetWrapper(worldPosition);
		wrapper.StateInfo |= DynamicMeshStates.UnloadMark1;
		wrapper.StateInfo |= DynamicMeshStates.UnloadMark2;
		wrapper.StateInfo |= DynamicMeshStates.UnloadMark3;
	}

	// Token: 0x06001957 RID: 6487 RVA: 0x0008F14F File Offset: 0x0008D34F
	[PublicizedFrom(EAccessModifier.Private)]
	public bool ReplaceData(DynamicMeshChunkDataWrapper wrapper, DynamicMeshChunkData data, string debug)
	{
		while (!this.ReleaseData(wrapper, "replaceData"))
		{
			Log.Out("Waiting for bytes to be released: " + wrapper.ToDebugLocation());
		}
		wrapper.Data = data;
		return true;
	}

	// Token: 0x06001958 RID: 6488 RVA: 0x0008F17E File Offset: 0x0008D37E
	public bool ClearLock(DynamicMeshChunkDataWrapper wrapper, string debug)
	{
		this.ReleaseData(wrapper.Key, "CL_" + debug);
		return wrapper.TryExit("ClearLock " + debug);
	}

	// Token: 0x06001959 RID: 6489 RVA: 0x0008F1B0 File Offset: 0x0008D3B0
	[PublicizedFrom(EAccessModifier.Private)]
	public bool ClearLock(long worldPosition, string debug)
	{
		return this.GetWrapper(worldPosition).TryExit(debug);
	}

	// Token: 0x0600195A RID: 6490 RVA: 0x000027FC File Offset: 0x000009FC
	public void LogMemoryUsage()
	{
	}

	// Token: 0x0600195B RID: 6491 RVA: 0x000027FC File Offset: 0x000009FC
	public void FreeMemory()
	{
	}

	// Token: 0x0600195C RID: 6492 RVA: 0x0008F1C4 File Offset: 0x0008D3C4
	[PublicizedFrom(EAccessModifier.Private)]
	public bool ReleaseData(DynamicMeshChunkDataWrapper wrapper, string debug)
	{
		if (!wrapper.ThreadHasLock())
		{
			Log.Error("You can not release bytes if you do not have the lock: " + wrapper.ToDebugLocation());
			return false;
		}
		DynamicMeshChunkData dynamicMeshChunkData;
		if (wrapper.TryGetData(out dynamicMeshChunkData, "releaseData" + debug))
		{
			if (dynamicMeshChunkData != null)
			{
				dynamicMeshChunkData.Reset();
				DynamicMeshChunkData.AddToCache(dynamicMeshChunkData, "releaseData_" + debug);
				wrapper.Data = null;
			}
			return true;
		}
		return false;
	}

	// Token: 0x0600195D RID: 6493 RVA: 0x0008F22C File Offset: 0x0008D42C
	[PublicizedFrom(EAccessModifier.Private)]
	public bool ReleaseData(long worldPosition, string debug)
	{
		DynamicMeshChunkDataWrapper wrapper = this.GetWrapper(worldPosition);
		if (!wrapper.TryTakeLock("releaseDataQueue"))
		{
			if (DynamicMeshManager.DoLog)
			{
				Log.Out(wrapper.ToDebugLocation() + " Could not clear from queue because item is locked by " + wrapper.lastLock);
			}
			return false;
		}
		if (DynamicMeshManager.DoLog)
		{
			Log.Out("Releasing " + wrapper.ToDebugLocation());
		}
		bool flag = this.ReleaseData(wrapper, "RD_" + debug);
		wrapper.Reset();
		if (wrapper.StateInfo.HasFlag(DynamicMeshStates.MarkedForDelete))
		{
			if (DynamicMeshManager.DoLog)
			{
				Log.Out("Deleting file from disk " + wrapper.ToDebugLocation());
			}
			string text = wrapper.Path();
			if (SdFile.Exists(text))
			{
				SdFile.Delete(text);
			}
		}
		return flag;
	}

	// Token: 0x0600195E RID: 6494 RVA: 0x0008F2F0 File Offset: 0x0008D4F0
	public bool CollectBytes(long key, out byte[] data, out int length)
	{
		DynamicMeshChunkDataWrapper wrapper = this.GetWrapper(key);
		if (!wrapper.TryTakeLock("collectBytes"))
		{
			data = null;
			length = 0;
			return false;
		}
		while (!wrapper.GetLock("collectBytes"))
		{
			Log.Out("failed to get lock on collect");
		}
		string itemPath = DynamicMeshUnity.GetItemPath(key);
		if (!SdFile.Exists(itemPath))
		{
			data = null;
			length = 0;
		}
		else
		{
			length = (int)new SdFileInfo(itemPath).Length;
			data = this.GetFromPool(length);
			using (Stream stream = SdFile.OpenRead(itemPath))
			{
				stream.Read(data, 0, length);
			}
		}
		this.ClearLock(wrapper, "collectBytes");
		return true;
	}

	// Token: 0x0600195F RID: 6495 RVA: 0x0008F3A0 File Offset: 0x0008D5A0
	public bool CollectItem(long worldPosition, out DynamicMeshChunkDataWrapper wrapper, out string debugMessage)
	{
		debugMessage = string.Empty;
		wrapper = this.GetWrapper(worldPosition);
		while (!wrapper.GetLock("collectItem"))
		{
			Log.Out("failed to get lock on collect");
		}
		if (wrapper.StateInfo.HasFlag(DynamicMeshStates.MarkedForDelete))
		{
			debugMessage = "toDelete";
			return true;
		}
		if (wrapper.StateInfo.HasFlag(DynamicMeshStates.ThreadUpdating))
		{
			debugMessage = "threadUpdating";
			return false;
		}
		this.ForceLoadItem(wrapper);
		return true;
	}

	// Token: 0x06001960 RID: 6496 RVA: 0x0008F428 File Offset: 0x0008D628
	public DynamicMeshChunkDataWrapper GetWrapper(long key)
	{
		DynamicMeshChunkDataWrapper dynamicMeshChunkDataWrapper;
		if (!this.ChunkData.TryGetValue(key, out dynamicMeshChunkDataWrapper))
		{
			dynamicMeshChunkDataWrapper = DynamicMeshChunkDataWrapper.Create(key);
			if (!this.ChunkData.TryAdd(key, dynamicMeshChunkDataWrapper))
			{
				this.ChunkData.TryGetValue(key, out dynamicMeshChunkDataWrapper);
				Log.Error("Request failed to add data: " + DynamicMeshUnity.GetDebugPositionKey(key));
			}
		}
		return dynamicMeshChunkDataWrapper;
	}

	// Token: 0x06001961 RID: 6497 RVA: 0x0008F480 File Offset: 0x0008D680
	public byte[] GetFromPool(int length)
	{
		byte[] array = DynamicMeshChunkDataStorage<T>.Pool.Alloc(length);
		this.BytesAllocated += (long)array.Length;
		if (array.Length > this.LargestAllocation)
		{
			this.LargestAllocation = array.Length;
		}
		return array;
	}

	// Token: 0x06001962 RID: 6498 RVA: 0x0008F4BF File Offset: 0x0008D6BF
	public bool ManuallyReleaseBytes(byte[] bytes)
	{
		if (bytes != null)
		{
			DynamicMeshChunkDataStorage<T>.Pool.Free(bytes);
			this.BytesReleased += (long)bytes.Length;
			return true;
		}
		return false;
	}

	// Token: 0x06001963 RID: 6499 RVA: 0x0008F4E3 File Offset: 0x0008D6E3
	public bool IsReadyToCollect(long worldPosition)
	{
		return this.GetWrapper(worldPosition).Data != null;
	}

	// Token: 0x04001006 RID: 4102
	public ConcurrentDictionary<long, DynamicMeshChunkDataWrapper> ChunkData = new ConcurrentDictionary<long, DynamicMeshChunkDataWrapper>();

	// Token: 0x04001007 RID: 4103
	public ConcurrentStack<DynamicMeshChunkDataWrapper> LoadRequests = new ConcurrentStack<DynamicMeshChunkDataWrapper>();

	// Token: 0x04001008 RID: 4104
	[PublicizedFrom(EAccessModifier.Private)]
	public DateTime NextCachePurge = DateTime.MinValue;

	// Token: 0x04001009 RID: 4105
	[PublicizedFrom(EAccessModifier.Private)]
	public int PurgeInterval = 3;

	// Token: 0x0400100A RID: 4106
	public int LiveItems;

	// Token: 0x0400100B RID: 4107
	[PublicizedFrom(EAccessModifier.Private)]
	public object _lock = new object();

	// Token: 0x0400100C RID: 4108
	[PublicizedFrom(EAccessModifier.Private)]
	public MemoryStream FileMemoryStream = new MemoryStream();

	// Token: 0x0400100D RID: 4109
	[PublicizedFrom(EAccessModifier.Private)]
	public byte[] Buffer = new byte[2048];

	// Token: 0x0400100E RID: 4110
	public int MaxAllowedItems;

	// Token: 0x0400100F RID: 4111
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly LoosePool<byte> Pool = new LoosePool<byte>();

	// Token: 0x04001010 RID: 4112
	public long BytesAllocated;

	// Token: 0x04001011 RID: 4113
	public long BytesReleased;

	// Token: 0x04001012 RID: 4114
	public int LargestAllocation;
}
