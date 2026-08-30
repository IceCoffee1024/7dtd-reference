using System;
using System.Collections.Generic;
using System.Threading;

// Token: 0x0200035F RID: 863
public class DynamicMeshBuilderManager
{
	// Token: 0x060018FE RID: 6398 RVA: 0x0008CD3F File Offset: 0x0008AF3F
	public static DynamicMeshBuilderManager GetOrCreate()
	{
		return DynamicMeshBuilderManager.Instance ?? new DynamicMeshBuilderManager();
	}

	// Token: 0x060018FF RID: 6399 RVA: 0x0008CD4F File Offset: 0x0008AF4F
	public DynamicMeshBuilderManager()
	{
		DynamicMeshBuilderManager.Instance = this;
	}

	// Token: 0x06001900 RID: 6400 RVA: 0x0008CD74 File Offset: 0x0008AF74
	public void StartThreads()
	{
		Log.Out("Starting builder threads: " + DynamicMeshBuilderManager.MaxBuilderThreads.ToString());
		foreach (DynamicMeshChunkProcessor dynamicMeshChunkProcessor in this.BuilderThreads)
		{
			dynamicMeshChunkProcessor.RequestStop(false);
		}
		for (int i = 0; i < DynamicMeshBuilderManager.MaxBuilderThreads; i++)
		{
			this.AddBuilder();
		}
	}

	// Token: 0x06001901 RID: 6401 RVA: 0x0008CDF8 File Offset: 0x0008AFF8
	[PublicizedFrom(EAccessModifier.Private)]
	public DynamicMeshChunkProcessor AddBuilder()
	{
		DynamicMeshChunkProcessor dynamicMeshChunkProcessor = new DynamicMeshChunkProcessor();
		dynamicMeshChunkProcessor.Init(DynamicMeshBuilderManager.ThreadId++);
		dynamicMeshChunkProcessor.Status = DynamicMeshBuilderStatus.Ready;
		this.BuilderThreads.Add(dynamicMeshChunkProcessor);
		dynamicMeshChunkProcessor.StartThread();
		return dynamicMeshChunkProcessor;
	}

	// Token: 0x06001902 RID: 6402 RVA: 0x0008CE38 File Offset: 0x0008B038
	public void MainThreadRunJobs()
	{
		foreach (DynamicMeshChunkProcessor dynamicMeshChunkProcessor in this.BuilderThreads)
		{
			if (dynamicMeshChunkProcessor != null)
			{
				dynamicMeshChunkProcessor.RunJob();
			}
		}
	}

	// Token: 0x06001903 RID: 6403 RVA: 0x0008CE90 File Offset: 0x0008B090
	public void SetNewLimit(int limit)
	{
		DynamicMeshBuilderManager.MaxBuilderThreads = limit;
		this.StartThreads();
	}

	// Token: 0x06001904 RID: 6404 RVA: 0x0008CEA0 File Offset: 0x0008B0A0
	public void StopThreads(bool forceStop)
	{
		foreach (DynamicMeshChunkProcessor dynamicMeshChunkProcessor in this.BuilderThreads)
		{
			dynamicMeshChunkProcessor.RequestStop(forceStop);
		}
	}

	// Token: 0x06001905 RID: 6405 RVA: 0x0008CEF4 File Offset: 0x0008B0F4
	public DynamicMeshChunkProcessor GetRegionBuilder(bool useAllThreads)
	{
		if (!Monitor.TryEnter(this._lock, 1))
		{
			Log.Warning("Build region list locked");
			return null;
		}
		DynamicMeshChunkProcessor dynamicMeshChunkProcessor = null;
		for (int i = 0; i < this.BuilderThreads.Count; i++)
		{
			DynamicMeshChunkProcessor dynamicMeshChunkProcessor2 = this.BuilderThreads[i];
			if (!dynamicMeshChunkProcessor2.StopRequested && dynamicMeshChunkProcessor2.Status == DynamicMeshBuilderStatus.Ready)
			{
				dynamicMeshChunkProcessor = dynamicMeshChunkProcessor2;
				break;
			}
			if (!useAllThreads)
			{
				break;
			}
		}
		if (dynamicMeshChunkProcessor == null && useAllThreads && this.BuilderThreads.Count < DynamicMeshBuilderManager.MaxBuilderThreads)
		{
			dynamicMeshChunkProcessor = this.AddBuilder();
		}
		Monitor.Exit(this._lock);
		return dynamicMeshChunkProcessor;
	}

	// Token: 0x06001906 RID: 6406 RVA: 0x0008CF84 File Offset: 0x0008B184
	public DynamicMeshChunkProcessor GetNextBuilder()
	{
		if (!Monitor.TryEnter(this._lock, 1))
		{
			Log.Warning("Build list locked");
			return null;
		}
		DynamicMeshChunkProcessor dynamicMeshChunkProcessor = null;
		foreach (DynamicMeshChunkProcessor dynamicMeshChunkProcessor2 in this.BuilderThreads)
		{
			if (!dynamicMeshChunkProcessor2.StopRequested && dynamicMeshChunkProcessor2.Status == DynamicMeshBuilderStatus.Ready)
			{
				dynamicMeshChunkProcessor = dynamicMeshChunkProcessor2;
				break;
			}
		}
		if (dynamicMeshChunkProcessor == null && this.BuilderThreads.Count < DynamicMeshBuilderManager.MaxBuilderThreads)
		{
			dynamicMeshChunkProcessor = this.AddBuilder();
		}
		Monitor.Exit(this._lock);
		return dynamicMeshChunkProcessor;
	}

	// Token: 0x170002F7 RID: 759
	// (get) Token: 0x06001907 RID: 6407 RVA: 0x0008D028 File Offset: 0x0008B228
	public bool HasThreadAvailable
	{
		get
		{
			return this.GetNextBuilder() != null;
		}
	}

	// Token: 0x06001908 RID: 6408 RVA: 0x0008D034 File Offset: 0x0008B234
	public int AddItemForExport(DynamicMeshItem item, bool isPrimary)
	{
		DynamicMeshChunkProcessor nextBuilder = this.GetNextBuilder();
		if (nextBuilder == null)
		{
			return 0;
		}
		if (DynamicMeshThread.ChunkDataQueue.IsUpdating(item))
		{
			return -1;
		}
		return nextBuilder.AddNewItem(item, isPrimary);
	}

	// Token: 0x06001909 RID: 6409 RVA: 0x0008D064 File Offset: 0x0008B264
	public int AddItemForMeshGeneration(DynamicMeshItem item, bool isPrimary)
	{
		DynamicMeshChunkProcessor nextBuilder = this.GetNextBuilder();
		if (nextBuilder == null)
		{
			return 0;
		}
		if (DynamicMeshThread.ChunkDataQueue.IsUpdating(item))
		{
			return -1;
		}
		DynamicMeshThread.GetThreadRegion(item.WorldPosition);
		return nextBuilder.AddItemForMeshGeneration(item, isPrimary);
	}

	// Token: 0x0600190A RID: 6410 RVA: 0x0008D0A0 File Offset: 0x0008B2A0
	public int AddItemForPreview(DynamicMeshItem item, ChunkPreviewData previewData)
	{
		DynamicMeshChunkProcessor nextBuilder = this.GetNextBuilder();
		if (nextBuilder == null)
		{
			return 0;
		}
		return nextBuilder.AddItemForMeshPreview(item, previewData);
	}

	// Token: 0x0600190B RID: 6411 RVA: 0x0008D0C4 File Offset: 0x0008B2C4
	public int RegenerateRegion(DynamicMeshThread.ThreadRegion region, bool useAllThreads)
	{
		DynamicMeshChunkProcessor regionBuilder = this.GetRegionBuilder(useAllThreads);
		if (regionBuilder == null)
		{
			return 0;
		}
		DynamicMeshThread.GetThreadRegion(region.Key);
		return regionBuilder.AddRegenerateRegion(region);
	}

	// Token: 0x0600190C RID: 6412 RVA: 0x0008D0F4 File Offset: 0x0008B2F4
	public void CheckBuilders()
	{
		for (int i = this.BuilderThreads.Count - 1; i >= 0; i--)
		{
			DynamicMeshChunkProcessor dynamicMeshChunkProcessor = this.BuilderThreads[i];
			if (dynamicMeshChunkProcessor == null)
			{
				this.BuilderThreads.RemoveAt(i);
				return;
			}
			if (dynamicMeshChunkProcessor.Status == DynamicMeshBuilderStatus.Complete)
			{
				this.HandleResult(dynamicMeshChunkProcessor);
			}
			else if (dynamicMeshChunkProcessor.Status == DynamicMeshBuilderStatus.Stopped)
			{
				dynamicMeshChunkProcessor.CleanUp();
				this.BuilderThreads.Remove(dynamicMeshChunkProcessor);
			}
			else if (dynamicMeshChunkProcessor.Status == DynamicMeshBuilderStatus.Error)
			{
				dynamicMeshChunkProcessor.CleanUp();
				this.BuilderThreads.Remove(dynamicMeshChunkProcessor);
			}
		}
	}

	// Token: 0x0600190D RID: 6413 RVA: 0x0008D184 File Offset: 0x0008B384
	public void CheckPreviews()
	{
		for (int i = this.BuilderThreads.Count - 1; i >= 0; i--)
		{
			DynamicMeshChunkProcessor dynamicMeshChunkProcessor = this.BuilderThreads[i];
			if (dynamicMeshChunkProcessor == null)
			{
				this.BuilderThreads.RemoveAt(i);
				return;
			}
			if (dynamicMeshChunkProcessor.Status == DynamicMeshBuilderStatus.PreviewComplete)
			{
				this.HandleResult(dynamicMeshChunkProcessor);
			}
		}
	}

	// Token: 0x0600190E RID: 6414 RVA: 0x0008D1D8 File Offset: 0x0008B3D8
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleResult(DynamicMeshChunkProcessor builder)
	{
		ExportMeshResult result = builder.Result;
		DynamicMeshItem item = builder.Item;
		DynamicMeshThread.ThreadRegion region = builder.Region;
		if (DynamicMeshManager.DoLog)
		{
			DynamicMeshItem item2 = builder.Item;
			string text = ((item2 != null) ? item2.ToDebugLocation() : null) ?? builder.Region.ToDebugLocation();
			Log.Out("Export result: " + text + ": " + result.ToString());
		}
		if (GameManager.IsDedicatedServer && builder.ChunkData != null)
		{
			builder.ChunkData = DyMeshData.AddToCache(builder.ChunkData);
		}
		if (builder.ChunkData != null)
		{
			string text2;
			if ((text2 = ((item != null) ? item.ToDebugLocation() : null)) == null)
			{
				text2 = ((region != null) ? region.ToDebugLocation() : null) ?? "null";
			}
			string text3 = text2;
			Log.Warning("Chunk data was not cleaned up by thread! " + text3 + ": " + result.ToString());
			builder.ChunkData = DyMeshData.AddToCache(builder.ChunkData);
		}
		if (item != null)
		{
			long key = item.Key;
			if (result == ExportMeshResult.Success)
			{
				DynamicMeshThread.ChunksToProcess.TryRemove(key);
				DynamicMeshThread.ChunksToLoad.Remove(key);
			}
			else if (result != ExportMeshResult.PreviewSuccess)
			{
				if (result == ExportMeshResult.PreviewDelay || result == ExportMeshResult.PreviewMissing)
				{
					DynamicMeshThread.SetNextChunks(item.Key);
					DynamicMeshPrefabPreviewThread.Instance.AddChunk(item);
				}
				else if (result == ExportMeshResult.SuccessNoLoad)
				{
					item.State = DynamicItemState.Empty;
					DynamicMeshThread.ChunksToProcess.TryRemove(key);
					DynamicMeshThread.ChunksToLoad.Remove(key);
				}
				else if (result == ExportMeshResult.Delay)
				{
					DynamicMeshThread.RequestPrimaryQueue(builder.Item);
				}
				else if (result == ExportMeshResult.ChunkMissing)
				{
					item.State = DynamicItemState.Empty;
					Log.Warning("chunk missing " + item.ToDebugLocation());
				}
				else
				{
					item.State = DynamicItemState.Empty;
					DynamicMeshThread.ChunksToProcess.TryRemove(key);
					DynamicMeshThread.ChunksToLoad.Remove(key);
					Log.Error("Failed to export " + item.ToDebugLocation() + ":" + result.ToString());
				}
			}
		}
		else if (result == ExportMeshResult.Delay)
		{
			Log.Out("Re-adding region regen???: " + region.ToDebugLocation());
			DynamicMeshThread.AddRegionUpdateData(region.X, region.Z, false);
		}
		builder.ResetAfterJob();
	}

	// Token: 0x04000FF1 RID: 4081
	public static DynamicMeshBuilderManager Instance;

	// Token: 0x04000FF2 RID: 4082
	public static int MaxBuilderThreads = 1;

	// Token: 0x04000FF3 RID: 4083
	[PublicizedFrom(EAccessModifier.Private)]
	public static int ThreadId = 1;

	// Token: 0x04000FF4 RID: 4084
	[PublicizedFrom(EAccessModifier.Private)]
	public const double MaxInactiveTime = 10.0;

	// Token: 0x04000FF5 RID: 4085
	public List<DynamicMeshChunkProcessor> BuilderThreads = new List<DynamicMeshChunkProcessor>();

	// Token: 0x04000FF6 RID: 4086
	[PublicizedFrom(EAccessModifier.Private)]
	public object _lock = new object();
}
