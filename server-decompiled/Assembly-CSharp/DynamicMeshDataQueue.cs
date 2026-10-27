using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using ConcurrentCollections;
using Noemax.GZip;

// Token: 0x02000371 RID: 881
public class DynamicMeshDataQueue<T> where T : DynamicMeshContainer
{
	// Token: 0x060019A5 RID: 6565 RVA: 0x000934E8 File Offset: 0x000916E8
	public DynamicMeshDataQueue(bool isRegion, int purgeInterval)
	{
		this.MeshFolder = DynamicMeshFile.MeshLocation;
		this.IsRegionQueue = isRegion;
		this.PurgeInterval = purgeInterval;
		DynamicMeshDataQueue<T>.Pool.EnforceMaxSize = true;
	}

	// Token: 0x060019A6 RID: 6566 RVA: 0x00093583 File Offset: 0x00091783
	public string GetQueueType()
	{
		if (!this.IsRegionQueue)
		{
			return "Item";
		}
		return "Region";
	}

	// Token: 0x17000303 RID: 771
	// (get) Token: 0x060019A7 RID: 6567 RVA: 0x00093598 File Offset: 0x00091798
	public long MBytesAllocated
	{
		get
		{
			return this.BytesAllocated / 1024L / 1024L;
		}
	}

	// Token: 0x17000304 RID: 772
	// (get) Token: 0x060019A8 RID: 6568 RVA: 0x000935AE File Offset: 0x000917AE
	public long MBytesReleased
	{
		get
		{
			return this.BytesReleased / 1024L / 1024L;
		}
	}

	// Token: 0x17000305 RID: 773
	// (get) Token: 0x060019A9 RID: 6569 RVA: 0x000935C4 File Offset: 0x000917C4
	public long BytesLive
	{
		get
		{
			return this.BytesAllocated - this.BytesReleased;
		}
	}

	// Token: 0x17000306 RID: 774
	// (get) Token: 0x060019AA RID: 6570 RVA: 0x000935D3 File Offset: 0x000917D3
	public long MBytesLive
	{
		get
		{
			return this.BytesLive / 1024L / 1024L;
		}
	}

	// Token: 0x060019AB RID: 6571 RVA: 0x000935EC File Offset: 0x000917EC
	public byte[] GetFromPool(int length)
	{
		byte[] array = DynamicMeshDataQueue<T>.Pool.Alloc(length);
		this.BytesAllocated += (long)array.Length;
		if (array.Length > this.LargestAllocation)
		{
			this.LargestAllocation = array.Length;
		}
		this.LiveItems++;
		return array;
	}

	// Token: 0x060019AC RID: 6572 RVA: 0x0009363C File Offset: 0x0009183C
	public byte[] Clone(byte[] source)
	{
		byte[] fromPool = this.GetFromPool(source.Length);
		Array.Copy(source, fromPool, source.Length);
		return fromPool;
	}

	// Token: 0x060019AD RID: 6573 RVA: 0x00093660 File Offset: 0x00091860
	public void ClearQueues()
	{
		Log.Out("Clearing queues. IsRegion: " + this.IsRegionQueue.ToString());
		DynamicMeshData dynamicMeshData;
		while (this.LoadRequests.TryPop(out dynamicMeshData))
		{
		}
		this.CleanUpAndSave();
		this.Cache.Clear();
		Log.Out("Cleared queues. IsRegion: " + this.IsRegionQueue.ToString());
	}

	// Token: 0x17000307 RID: 775
	// (get) Token: 0x060019AE RID: 6574 RVA: 0x000936C1 File Offset: 0x000918C1
	public bool HasLoadRequests
	{
		get
		{
			return (this.LoadRequests.Count > 0 || this.ImportantLoad != null) && this.IsReadyThreaded();
		}
	}

	// Token: 0x060019AF RID: 6575 RVA: 0x000936E4 File Offset: 0x000918E4
	public bool LoadItem()
	{
		if (this.ImportantLoad != null)
		{
			if (!this.TryLoadItem(this.ImportantLoad))
			{
				this.ImportantLoad.StateInfo |= DynamicMeshStates.FileMissing;
				Log.Out("Important load FAILED: " + this.ImportantLoad.ToDebugLocation());
				this.ImportantLoad = null;
			}
			this.ImportantLoad = null;
		}
		DynamicMeshData dynamicMeshData;
		while (this.MainThreadLoadRequests.TryPop(out dynamicMeshData))
		{
			if (!this.TryLoadItem(dynamicMeshData))
			{
				Log.Out("Failed main thread load " + dynamicMeshData.ToDebugLocation());
			}
		}
		if (this.LiveItems >= this.MaxAllowedItems)
		{
			return false;
		}
		if (this.LoadRequests.Count == 0)
		{
			return false;
		}
		if (!this.LoadRequests.TryPop(out dynamicMeshData))
		{
			return false;
		}
		if (dynamicMeshData.StateInfo.HasFlag(DynamicMeshStates.ThreadUpdating))
		{
			return false;
		}
		if (!this.TryLoadItem(dynamicMeshData))
		{
			this.LoadRequests.Push(dynamicMeshData);
		}
		return true;
	}

	// Token: 0x060019B0 RID: 6576 RVA: 0x000937D5 File Offset: 0x000919D5
	public bool IsReadyThreaded()
	{
		return this.MaxAllowedItems == 0 || this.LiveItems < this.MaxAllowedItems - 1;
	}

	// Token: 0x060019B1 RID: 6577 RVA: 0x000937F1 File Offset: 0x000919F1
	[PublicizedFrom(EAccessModifier.Private)]
	public bool ForceLoadItem(DynamicMeshData data, out byte[] bytes)
	{
		if (!data.GetLock("forceLoad"))
		{
			bytes = null;
			return false;
		}
		this.TryLoadItem(data);
		data.TryGetBytes(out bytes, "forceTry");
		return true;
	}

	// Token: 0x060019B2 RID: 6578 RVA: 0x0009381C File Offset: 0x00091A1C
	[PublicizedFrom(EAccessModifier.Private)]
	public bool TryLoadItem(DynamicMeshData data)
	{
		object @lock = this._lock;
		lock (@lock)
		{
			string text = data.Path(this.IsRegionQueue);
			if (!SdFile.Exists(text))
			{
				data.StateInfo |= DynamicMeshStates.FileMissing;
				return false;
			}
			int num = (data.StateInfo.HasFlag(DynamicMeshStates.SaveRequired) ? 1 : 0);
			byte[] array;
			bool flag2 = data.TryGetBytes(out array, "tryLoadItem");
			if (num == 0 && flag2)
			{
				if (DynamicMeshManager.CompressFiles)
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
							array = this.GetFromPool((int)this.FileMemoryStream.Length);
							this.FileMemoryStream.Position = 0L;
							this.FileMemoryStream.Read(array, 0, (int)this.FileMemoryStream.Length);
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
							this.ReplaceBytes(data, array, "loadItem");
							data.StreamLength = (int)this.FileMemoryStream.Length;
							goto IL_0231;
						}
					}
				}
				using (Stream stream2 = SdFile.OpenRead(text))
				{
					using (PooledBinaryReader pooledBinaryReader = MemoryPools.poolBinaryReader.AllocSync(false))
					{
						pooledBinaryReader.SetBaseStream(stream2);
						int num4 = (int)pooledBinaryReader.BaseStream.Length;
						array = this.GetFromPool(num4);
						pooledBinaryReader.Read(array, 0, num4);
						this.ReplaceBytes(data, array, "loadUncompressed");
						data.StreamLength = num4;
					}
				}
			}
			IL_0231:
			data.TryExit("tryLoadItem");
			data.StateInfo &= ~DynamicMeshStates.LoadRequired;
			data.StateInfo &= ~DynamicMeshStates.FileMissing;
			data.StateInfo &= ~DynamicMeshStates.LoadBoosted;
		}
		return true;
	}

	// Token: 0x060019B3 RID: 6579 RVA: 0x00093B24 File Offset: 0x00091D24
	public string GetCacheSize()
	{
		int num;
		return (this.IsRegionQueue ? "Region Queue\n" : "Item Queue\n") + DynamicMeshDataQueue<T>.Pool.GetSize(out num);
	}

	// Token: 0x060019B4 RID: 6580 RVA: 0x00093B56 File Offset: 0x00091D56
	public bool IsUpdating(T item)
	{
		return this.GetData(item.WorldPosition).StateInfo.HasFlag(DynamicMeshStates.ThreadUpdating);
	}

	// Token: 0x060019B5 RID: 6581 RVA: 0x00093B7E File Offset: 0x00091D7E
	public void MarkAsUpdating(T item)
	{
		this.GetData(item.WorldPosition).StateInfo |= DynamicMeshStates.ThreadUpdating;
	}

	// Token: 0x060019B6 RID: 6582 RVA: 0x00093B9E File Offset: 0x00091D9E
	public void MarkAsUpdated(T item)
	{
		if (item == null)
		{
			return;
		}
		this.GetData(item.WorldPosition).StateInfo &= ~DynamicMeshStates.ThreadUpdating;
	}

	// Token: 0x060019B7 RID: 6583 RVA: 0x00093BC8 File Offset: 0x00091DC8
	public void ClearMainThreadTag(T item)
	{
		if (item == null)
		{
			return;
		}
		this.GetData(item.WorldPosition).StateInfo &= ~DynamicMeshStates.MainThreadLoadRequest;
	}

	// Token: 0x060019B8 RID: 6584 RVA: 0x00093BF5 File Offset: 0x00091DF5
	public void ResetData(T item)
	{
		this.AddSaveRequest(item, null, 0, true, true, false);
	}

	// Token: 0x060019B9 RID: 6585 RVA: 0x00093C03 File Offset: 0x00091E03
	public void AddSaveRequest(T item, byte[] bytes, int length, bool requestRegionUpdate, bool unloadImmediately, bool loadInWorld)
	{
		this.AddSaveRequest(item.WorldPosition, bytes, length, requestRegionUpdate, unloadImmediately, loadInWorld);
	}

	// Token: 0x060019BA RID: 6586 RVA: 0x00093C20 File Offset: 0x00091E20
	public void AddSaveRequest(Vector3i worldPosition, byte[] bytes, int length, bool requestRegionUpdate, bool unloadImmediately, bool loadInWorld)
	{
		if (!DynamicMeshManager.CONTENT_ENABLED)
		{
			return;
		}
		DynamicMeshData data = this.GetData(worldPosition);
		if (DynamicMeshManager.DoLog)
		{
			Log.Out(string.Concat(new string[]
			{
				"Adding Saving ",
				this.GetQueueType(),
				" ",
				data.ToDebugLocation(),
				":",
				length.ToString()
			}));
		}
		string text = "addSave";
		if (!data.GetLock(text))
		{
			Log.Warning("Could not get lock on save request: " + data.ToDebugLocation());
			return;
		}
		data.StreamLength = length;
		this.ReplaceBytes(data, bytes, "saveRequest");
		data.StateInfo |= DynamicMeshStates.SaveRequired;
		data.StateInfo &= ~DynamicMeshStates.MarkedForDelete;
		if (unloadImmediately)
		{
			data.StateInfo |= DynamicMeshStates.UnloadMark1;
			data.StateInfo |= DynamicMeshStates.UnloadMark2;
			data.StateInfo |= DynamicMeshStates.UnloadMark3;
		}
		this.SaveRequests.Add(data);
		data.TryExit(text);
		if (this.IsRegionQueue)
		{
			DynamicMeshManager.Instance.AddUpdateData(data.X, data.Z, false, false);
		}
	}

	// Token: 0x060019BB RID: 6587 RVA: 0x00093D48 File Offset: 0x00091F48
	public void SaveNetworkPackage(Vector3i worldPosition, byte[] bytes, int length)
	{
		try
		{
			string text = DynamicMeshFile.MeshLocation + string.Format("{0},{1}.mesh", worldPosition.x, worldPosition.z);
			if (bytes == null)
			{
				if (SdFile.Exists(text))
				{
					SdFile.Delete(text);
				}
			}
			else
			{
				using (Stream stream = SdFile.Create(text))
				{
					using (DeflateOutputStream deflateOutputStream = new DeflateOutputStream(stream, 3, false))
					{
						deflateOutputStream.Write(bytes, 0, length);
					}
				}
			}
		}
		catch (Exception ex)
		{
			Log.Error("Data queue error: " + ex.Message);
		}
	}

	// Token: 0x060019BC RID: 6588 RVA: 0x00093E08 File Offset: 0x00092008
	public bool TrySave(bool forceSave = false)
	{
		if (this.SaveRequests.Count == 0)
		{
			return false;
		}
		try
		{
			DynamicMeshData dynamicMeshData;
			if (!this.SaveRequests.TryRemoveFirst(out dynamicMeshData))
			{
				return false;
			}
			if (!forceSave && dynamicMeshData.StateInfo.HasFlag(DynamicMeshStates.ThreadUpdating))
			{
				Log.Out("Skipping save on updating item " + dynamicMeshData.ToDebugLocation());
				this.SaveRequests.Add(dynamicMeshData);
				return false;
			}
			this.SaveItem(dynamicMeshData);
			dynamicMeshData.TryExit("saveItem");
			return true;
		}
		catch (Exception ex)
		{
			Log.Error("Data queue error: " + ex.Message);
		}
		return false;
	}

	// Token: 0x060019BD RID: 6589 RVA: 0x00093EBC File Offset: 0x000920BC
	public void SaveItem(DynamicMeshData data)
	{
		data.GetLock("saveItem");
		byte[] array;
		data.TryGetBytes(out array, "saveItemTry");
		data.ClearUnloadMarks();
		data.StateInfo &= ~DynamicMeshStates.SaveRequired;
		if (DynamicMeshManager.DoLog)
		{
			Log.Out(string.Concat(new string[]
			{
				"Saving ",
				this.GetQueueType(),
				" ",
				data.ToDebugLocation(),
				":",
				(array != null) ? array.Length.ToString() : null
			}));
		}
		string text = data.Path(this.IsRegionQueue);
		if (array != null)
		{
			if (DynamicMeshManager.DoLog)
			{
				Log.Out(string.Concat(new string[]
				{
					"Saving ",
					this.GetQueueType(),
					" to disk ",
					data.ToDebugLocation(),
					": ",
					data.StreamLength.ToString()
				}));
			}
			if (DynamicMeshManager.CompressFiles)
			{
				using (Stream stream = SdFile.Create(text))
				{
					using (DeflateOutputStream deflateOutputStream = new DeflateOutputStream(stream, 3, false))
					{
						deflateOutputStream.Write(array, 0, data.StreamLength);
						return;
					}
				}
			}
			throw new NotImplementedException("Compression is not active");
		}
		if (DynamicMeshManager.DoLog)
		{
			Log.Out("Deleting null bytes " + this.GetQueueType() + " " + data.ToDebugLocation());
		}
		if (SdFile.Exists(text))
		{
			SdFile.Delete(text);
			return;
		}
	}

	// Token: 0x060019BE RID: 6590 RVA: 0x0009404C File Offset: 0x0009224C
	public void CleanUpAndSave()
	{
		while (this.SaveRequests.Count > 0)
		{
			this.TrySave(true);
		}
	}

	// Token: 0x060019BF RID: 6591 RVA: 0x00094068 File Offset: 0x00092268
	public void TryRelease()
	{
		if (this.NextCachePurge > DateTime.Now)
		{
			return;
		}
		this.NextCachePurge = DateTime.Now.AddSeconds((double)this.PurgeInterval);
		foreach (KeyValuePair<Vector3i, DynamicMeshData> keyValuePair in this.Cache)
		{
			DynamicMeshData value = keyValuePair.Value;
			if (value.X != 0 || value.Z != 0)
			{
				if (value.StateInfo.HasFlag(DynamicMeshStates.SaveRequired) && this.SaveRequests.Count > 0)
				{
					if (DynamicMeshManager.DebugReleases)
					{
						Log.Out(string.Format("{0},{1} save required", value.X, value.Z));
					}
				}
				else if (value.StateInfo.HasFlag(DynamicMeshStates.ThreadUpdating))
				{
					if (DynamicMeshManager.DebugReleases)
					{
						Log.Out(string.Format("{0},{1} thread updating", value.X, value.Z));
					}
				}
				else if (!value.StateInfo.HasFlag(DynamicMeshStates.UnloadMark1) && !value.StateInfo.HasFlag(DynamicMeshStates.MarkedForDelete))
				{
					value.StateInfo |= DynamicMeshStates.UnloadMark1;
				}
				else
				{
					this.ReleaseData(keyValuePair.Key);
				}
			}
		}
	}

	// Token: 0x060019C0 RID: 6592 RVA: 0x000941FC File Offset: 0x000923FC
	public void MarkForDeletion(Vector3i worldPosition)
	{
		this.GetData(worldPosition).StateInfo |= DynamicMeshStates.MarkedForDelete;
	}

	// Token: 0x060019C1 RID: 6593 RVA: 0x00094213 File Offset: 0x00092413
	public void MarkForUnload(Vector3i worldPosition)
	{
		DynamicMeshData data = this.GetData(worldPosition);
		data.StateInfo |= DynamicMeshStates.UnloadMark1;
		data.StateInfo |= DynamicMeshStates.UnloadMark2;
		data.StateInfo |= DynamicMeshStates.UnloadMark3;
	}

	// Token: 0x060019C2 RID: 6594 RVA: 0x00094248 File Offset: 0x00092448
	public bool ReplaceBytes(DynamicMeshData data, byte[] bytes, string debug)
	{
		if (!data.GetLock("ReplaceBytes"))
		{
			Log.Out("ReplaceBytesLockFailed");
			return false;
		}
		while (!this.ReleaseBytes(data))
		{
			Log.Out("Waiting for bytes to be released: " + data.ToDebugLocation());
		}
		data.Bytes = bytes;
		return true;
	}

	// Token: 0x060019C3 RID: 6595 RVA: 0x00094294 File Offset: 0x00092494
	public bool ClearLock(T item, string debug, bool releasePool)
	{
		if (item == null)
		{
			return true;
		}
		DynamicMeshData data = this.GetData(item.WorldPosition);
		if (releasePool && !data.StateInfo.HasFlag(DynamicMeshStates.SaveRequired))
		{
			this.ReleaseData(item.WorldPosition);
		}
		return data.TryExit("ClearLock " + debug);
	}

	// Token: 0x060019C4 RID: 6596 RVA: 0x00094300 File Offset: 0x00092500
	public bool ClearLock(Vector3i worldPosition, string debug)
	{
		return this.GetData(worldPosition).TryExit(debug);
	}

	// Token: 0x060019C5 RID: 6597 RVA: 0x00094314 File Offset: 0x00092514
	public void LogMemoryUsage()
	{
		int num;
		DynamicMeshDataQueue<T>.Pool.GetSize(out num);
		Log.Out(string.Format("{0}   Allocated: {1}   Released: {2}   {3}x Live: {4}   {5}x CacheMb: {6}   Longest: {7}", new object[]
		{
			this.GetQueueType(),
			this.MBytesAllocated,
			this.MBytesReleased,
			this.LiveItems,
			this.MBytesLive,
			DynamicMeshDataQueue<T>.Pool.GetTotalItems(),
			num / 1024 / 1024,
			this.LargestAllocation
		}));
	}

	// Token: 0x060019C6 RID: 6598 RVA: 0x000943B9 File Offset: 0x000925B9
	public void FreeMemory()
	{
		DynamicMeshDataQueue<T>.Pool.FreeAll();
	}

	// Token: 0x060019C7 RID: 6599 RVA: 0x000943C8 File Offset: 0x000925C8
	public bool ReleaseBytes(DynamicMeshData data)
	{
		if (!data.ThreadHasLock())
		{
			Log.Error("You can not release bytes if you do not have the lock: " + data.ToDebugLocation());
			return false;
		}
		byte[] array;
		if (data.TryGetBytes(out array, "releaseBytes"))
		{
			if (array != null)
			{
				DynamicMeshDataQueue<T>.Pool.Free(array);
				this.BytesReleased += (long)array.Length;
				this.LiveItems--;
				data.Bytes = null;
			}
			return true;
		}
		return false;
	}

	// Token: 0x060019C8 RID: 6600 RVA: 0x0009443A File Offset: 0x0009263A
	public bool ManuallyReleaseBytes(byte[] bytes)
	{
		if (bytes != null)
		{
			DynamicMeshDataQueue<T>.Pool.Free(bytes);
			this.BytesReleased += (long)bytes.Length;
			return true;
		}
		return false;
	}

	// Token: 0x060019C9 RID: 6601 RVA: 0x00094460 File Offset: 0x00092660
	public bool ReleaseData(Vector3i worldPosition)
	{
		worldPosition.y = 0;
		DynamicMeshData data = this.GetData(worldPosition);
		if (!data.TryTakeLock("releaseDataQueue"))
		{
			Log.Out("Could not clear from queue because item is locked by " + data.lastLock);
			return false;
		}
		if (DynamicMeshManager.DoLog)
		{
			Log.Out("Releasing " + this.GetQueueType() + " " + data.ToDebugLocation());
		}
		bool flag = this.ReleaseBytes(data);
		if (!this.Cache.TryRemove(worldPosition, out data))
		{
			string text = "Could not remove item from cache: ";
			Vector3i vector3i = worldPosition;
			Log.Out(text + vector3i.ToString());
		}
		if (data.StateInfo.HasFlag(DynamicMeshStates.MarkedForDelete))
		{
			if (DynamicMeshManager.DoLog)
			{
				Log.Out("Deleting " + this.GetQueueType() + " file from disk " + data.ToDebugLocation());
			}
			string text2 = data.Path(this.IsRegionQueue);
			if (SdFile.Exists(text2))
			{
				SdFile.Delete(text2);
			}
		}
		return flag;
	}

	// Token: 0x060019CA RID: 6602 RVA: 0x00094558 File Offset: 0x00092758
	public bool CollectItem(T item, bool forceLoad, bool mainThreadLoad, bool waitForLock, out byte[] bytes, out int length)
	{
		string text;
		return this.CollectItem(item, forceLoad, mainThreadLoad, waitForLock, out bytes, out text, out length);
	}

	// Token: 0x060019CB RID: 6603 RVA: 0x00094576 File Offset: 0x00092776
	public bool CollectItem(T item, bool forceLoad, bool mainThreadLoad, bool waitForLock, out byte[] bytes, out string debugMessage, out int length)
	{
		if (item == null)
		{
			bytes = null;
			debugMessage = "null";
			length = 0;
			return false;
		}
		return this.CollectItem(item.WorldPosition, forceLoad, mainThreadLoad, waitForLock, out bytes, out debugMessage, out length);
	}

	// Token: 0x060019CC RID: 6604 RVA: 0x000945B0 File Offset: 0x000927B0
	public bool CollectItem(Vector3i worldPosition, bool forceLoad, bool mainThreadLoad, bool waitForLock, out byte[] bytes, out string debugMessage, out int length)
	{
		debugMessage = string.Empty;
		DynamicMeshData data = this.GetData(worldPosition);
		data.ClearUnloadMarks();
		if (data.IsAvailableToLoad())
		{
			while (!data.TryGetBytes(out bytes, "collectItem"))
			{
				if (!waitForLock)
				{
					debugMessage = "locked";
					length = 0;
					return false;
				}
				if (DynamicMeshManager.DoLog)
				{
					Log.Out(string.Concat(new string[]
					{
						"Waiting for lock on CollectItem ",
						data.ToDebugLocation(),
						" (",
						debugMessage,
						")"
					}));
				}
			}
			length = data.StreamLength;
			if (bytes != null || !data.Exists(this.IsRegionQueue))
			{
				return true;
			}
		}
		if (data.StateInfo.HasFlag(DynamicMeshStates.MarkedForDelete))
		{
			debugMessage = "toDelete";
			bytes = null;
			length = 0;
			return true;
		}
		if (data.StateInfo.HasFlag(DynamicMeshStates.ThreadUpdating))
		{
			debugMessage = "threadUpdating";
			bytes = null;
			length = 0;
			return false;
		}
		if (forceLoad)
		{
			this.ForceLoadItem(data, out bytes);
			length = data.StreamLength;
			return true;
		}
		if (data.StateInfo.HasFlag(DynamicMeshStates.FileMissing))
		{
			debugMessage = DynamicMeshManager.FileMissing;
			bytes = null;
			length = 0;
			return false;
		}
		if (mainThreadLoad)
		{
			this.ImportantLoad = data;
		}
		else
		{
			this.LoadRequests.Push(data);
		}
		if (!data.StateInfo.HasFlag(DynamicMeshStates.LoadRequired))
		{
			data.StateInfo |= DynamicMeshStates.LoadRequired;
		}
		else if (data.StateInfo.HasFlag(DynamicMeshStates.LoadRequired) && this.LoadRequests.Count > 0 && !data.StateInfo.HasFlag(DynamicMeshStates.LoadBoosted))
		{
			data.StateInfo |= DynamicMeshStates.LoadBoosted;
			this.LoadRequests.Push(data);
		}
		debugMessage = "dunno";
		length = 0;
		bytes = null;
		return false;
	}

	// Token: 0x060019CD RID: 6605 RVA: 0x000947A8 File Offset: 0x000929A8
	public DynamicMeshData TryGetData(Vector3i worldPosition)
	{
		worldPosition.y = 0;
		DynamicMeshData dynamicMeshData = null;
		this.Cache.TryGetValue(worldPosition, out dynamicMeshData);
		return dynamicMeshData;
	}

	// Token: 0x060019CE RID: 6606 RVA: 0x000947D0 File Offset: 0x000929D0
	public DynamicMeshData GetData(Vector3i worldPosition)
	{
		worldPosition.y = 0;
		DynamicMeshData dynamicMeshData = null;
		if (!this.Cache.TryGetValue(worldPosition, out dynamicMeshData))
		{
			dynamicMeshData = DynamicMeshData.Create(worldPosition.x, worldPosition.z, this.IsRegionQueue);
			if (!this.Cache.TryAdd(worldPosition, dynamicMeshData))
			{
				this.Cache.TryGetValue(worldPosition, out dynamicMeshData);
				Log.Error("Request failed to add data: " + worldPosition.ToDebugLocation());
			}
		}
		return dynamicMeshData;
	}

	// Token: 0x060019CF RID: 6607 RVA: 0x00094844 File Offset: 0x00092A44
	public void AddToLoadListMainThread(Vector3i worldPosition)
	{
		DynamicMeshData data = this.GetData(worldPosition);
		if (!data.Exists(false))
		{
			return;
		}
		data.ClearUnloadMarks();
		data.StateInfo |= DynamicMeshStates.MainThreadLoadRequest;
		if (!data.StateInfo.HasFlag(DynamicMeshStates.LoadRequired))
		{
			data.StateInfo |= DynamicMeshStates.LoadRequired;
			this.MainThreadLoadRequests.Push(data);
		}
	}

	// Token: 0x060019D0 RID: 6608 RVA: 0x000948AD File Offset: 0x00092AAD
	public bool IsReadyToCollect(Vector3i worldPosition)
	{
		return this.GetData(worldPosition).Bytes != null;
	}

	// Token: 0x04001046 RID: 4166
	public ConcurrentDictionary<Vector3i, DynamicMeshData> Cache = new ConcurrentDictionary<Vector3i, DynamicMeshData>();

	// Token: 0x04001047 RID: 4167
	public ConcurrentStack<DynamicMeshData> MainThreadLoadRequests = new ConcurrentStack<DynamicMeshData>();

	// Token: 0x04001048 RID: 4168
	public ConcurrentStack<DynamicMeshData> LoadRequests = new ConcurrentStack<DynamicMeshData>();

	// Token: 0x04001049 RID: 4169
	public ConcurrentHashSet<DynamicMeshData> SaveRequests = new ConcurrentHashSet<DynamicMeshData>();

	// Token: 0x0400104A RID: 4170
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly LoosePool<byte> Pool = new LoosePool<byte>();

	// Token: 0x0400104B RID: 4171
	public DynamicMeshData ImportantLoad;

	// Token: 0x0400104C RID: 4172
	[PublicizedFrom(EAccessModifier.Private)]
	public string MeshFolder;

	// Token: 0x0400104D RID: 4173
	[PublicizedFrom(EAccessModifier.Private)]
	public bool IsRegionQueue;

	// Token: 0x0400104E RID: 4174
	[PublicizedFrom(EAccessModifier.Private)]
	public DateTime NextCachePurge = DateTime.MinValue;

	// Token: 0x0400104F RID: 4175
	[PublicizedFrom(EAccessModifier.Private)]
	public int PurgeInterval = 3;

	// Token: 0x04001050 RID: 4176
	public long BytesAllocated;

	// Token: 0x04001051 RID: 4177
	public long BytesReleased;

	// Token: 0x04001052 RID: 4178
	public int LiveItems;

	// Token: 0x04001053 RID: 4179
	public int LargestAllocation;

	// Token: 0x04001054 RID: 4180
	[PublicizedFrom(EAccessModifier.Private)]
	public object _lock = new object();

	// Token: 0x04001055 RID: 4181
	[PublicizedFrom(EAccessModifier.Private)]
	public MemoryStream FileMemoryStream = new MemoryStream();

	// Token: 0x04001056 RID: 4182
	[PublicizedFrom(EAccessModifier.Private)]
	public byte[] Buffer = new byte[2048];

	// Token: 0x04001057 RID: 4183
	public int MaxAllowedItems;
}
