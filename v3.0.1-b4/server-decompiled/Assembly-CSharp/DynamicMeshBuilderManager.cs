using System;
using System.Collections.Generic;
using System.Threading;

// Token: 0x0200035B RID: 859
public class DynamicMeshBuilderManager
{
	// Token: 0x060018E1 RID: 6369 RVA: 0x0008C5FF File Offset: 0x0008A7FF
	public static DynamicMeshBuilderManager GetOrCreate()
	{
		return DynamicMeshBuilderManager.Instance ?? new DynamicMeshBuilderManager();
	}

	// Token: 0x060018E2 RID: 6370 RVA: 0x0008C60F File Offset: 0x0008A80F
	public DynamicMeshBuilderManager()
	{
		DynamicMeshBuilderManager.Instance = this;
	}

	// Token: 0x060018E3 RID: 6371 RVA: 0x0008C634 File Offset: 0x0008A834
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

	// Token: 0x060018E4 RID: 6372 RVA: 0x0008C6B8 File Offset: 0x0008A8B8
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

	// Token: 0x060018E5 RID: 6373 RVA: 0x0008C6F8 File Offset: 0x0008A8F8
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

	// Token: 0x060018E6 RID: 6374 RVA: 0x0008C750 File Offset: 0x0008A950
	public void SetNewLimit(int limit)
	{
		DynamicMeshBuilderManager.MaxBuilderThreads = limit;
		this.StartThreads();
	}

	// Token: 0x060018E7 RID: 6375 RVA: 0x0008C760 File Offset: 0x0008A960
	public void StopThreads(bool forceStop)
	{
		foreach (DynamicMeshChunkProcessor dynamicMeshChunkProcessor in this.BuilderThreads)
		{
			dynamicMeshChunkProcessor.RequestStop(forceStop);
		}
	}

	// Token: 0x060018E8 RID: 6376 RVA: 0x0008C7B4 File Offset: 0x0008A9B4
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

	// Token: 0x060018E9 RID: 6377 RVA: 0x0008C844 File Offset: 0x0008AA44
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

	// Token: 0x170002F3 RID: 755
	// (get) Token: 0x060018EA RID: 6378 RVA: 0x0008C8E8 File Offset: 0x0008AAE8
	public bool HasThreadAvailable
	{
		get
		{
			return this.GetNextBuilder() != null;
		}
	}

	// Token: 0x060018EB RID: 6379 RVA: 0x0008C8F4 File Offset: 0x0008AAF4
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

	// Token: 0x060018EC RID: 6380 RVA: 0x0008C924 File Offset: 0x0008AB24
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

	// Token: 0x060018ED RID: 6381 RVA: 0x0008C960 File Offset: 0x0008AB60
	public int AddItemForPreview(DynamicMeshItem item, ChunkPreviewData previewData)
	{
		DynamicMeshChunkProcessor nextBuilder = this.GetNextBuilder();
		if (nextBuilder == null)
		{
			return 0;
		}
		return nextBuilder.AddItemForMeshPreview(item, previewData);
	}

	// Token: 0x060018EE RID: 6382 RVA: 0x0008C984 File Offset: 0x0008AB84
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

	// Token: 0x060018EF RID: 6383 RVA: 0x0008C9B4 File Offset: 0x0008ABB4
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

	// Token: 0x060018F0 RID: 6384 RVA: 0x0008CA44 File Offset: 0x0008AC44
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

	// Token: 0x060018F1 RID: 6385 RVA: 0x0008CA98 File Offset: 0x0008AC98
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

	// Token: 0x04000FDC RID: 4060
	public static DynamicMeshBuilderManager Instance;

	// Token: 0x04000FDD RID: 4061
	public static int MaxBuilderThreads = 1;

	// Token: 0x04000FDE RID: 4062
	[PublicizedFrom(EAccessModifier.Private)]
	public static int ThreadId = 1;

	// Token: 0x04000FDF RID: 4063
	[PublicizedFrom(EAccessModifier.Private)]
	public const double MaxInactiveTime = 10.0;

	// Token: 0x04000FE0 RID: 4064
	public List<DynamicMeshChunkProcessor> BuilderThreads = new List<DynamicMeshChunkProcessor>();

	// Token: 0x04000FE1 RID: 4065
	[PublicizedFrom(EAccessModifier.Private)]
	public object _lock = new object();
}
