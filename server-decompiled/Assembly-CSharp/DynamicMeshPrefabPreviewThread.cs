using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

// Token: 0x02000361 RID: 865
public class DynamicMeshPrefabPreviewThread
{
	// Token: 0x170002EE RID: 750
	// (get) Token: 0x060018EB RID: 6379 RVA: 0x0008CCA6 File Offset: 0x0008AEA6
	public bool HasStopBeenRequested
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.CancelToken.IsCancellationRequested;
		}
	}

	// Token: 0x060018EC RID: 6380 RVA: 0x0008CCB4 File Offset: 0x0008AEB4
	public void StartThread()
	{
		this.StopThread();
		DynamicMeshPrefabPreviewThread.Instance = this;
		this.BuilderManager = DynamicMeshBuilderManager.GetOrCreate();
		this.TokenSource = new CancellationTokenSource();
		this.Wait = new AutoResetEvent(false);
		this.CancelToken = this.TokenSource.Token;
		this.WaitHandles[0] = this.CancelToken.WaitHandle;
		this.WaitHandles[1] = this.Wait;
		this.PreviewCheckThread = new Thread(new ThreadStart(this.ThreadLoop));
		this.PreviewCheckThread.Start();
	}

	// Token: 0x060018ED RID: 6381 RVA: 0x0008CD44 File Offset: 0x0008AF44
	[PublicizedFrom(EAccessModifier.Private)]
	public void ThreadLoop()
	{
		while (!this.HasStopBeenRequested)
		{
			this.BuilderManager.CheckPreviews();
			bool flag = this.ProcessList();
			if (!this.ChunksToProcess.IsEmpty)
			{
				if (!flag)
				{
					Thread.Sleep(100);
				}
			}
			else
			{
				WaitHandle.WaitAny(this.WaitHandles);
			}
		}
	}

	// Token: 0x060018EE RID: 6382 RVA: 0x0008CD94 File Offset: 0x0008AF94
	public void StopThread()
	{
		if (this.PreviewCheckThread == null)
		{
			return;
		}
		this.TokenSource.Cancel();
		DateTime dateTime = DateTime.Now.AddSeconds(3.0);
		while (this.PreviewCheckThread.IsAlive || dateTime > DateTime.Now)
		{
			Thread.Sleep(10);
		}
		Thread previewCheckThread = this.PreviewCheckThread;
		if (previewCheckThread != null && previewCheckThread.IsAlive)
		{
			try
			{
				Thread previewCheckThread2 = this.PreviewCheckThread;
				if (previewCheckThread2 != null)
				{
					previewCheckThread2.Abort();
				}
			}
			catch
			{
			}
		}
	}

	// Token: 0x060018EF RID: 6383 RVA: 0x0008CE2C File Offset: 0x0008B02C
	public void AddChunk(DynamicMeshItem item)
	{
		this.LockGenerationUntil = DateTime.Now.AddMilliseconds(300.0);
		this.ChunksToProcess.TryAdd(item.Key, item);
		this.Wait.Set();
	}

	// Token: 0x060018F0 RID: 6384 RVA: 0x0008CE74 File Offset: 0x0008B074
	public void ClearChunks()
	{
		this.ChunksToProcess.Clear();
	}

	// Token: 0x060018F1 RID: 6385 RVA: 0x0008CE81 File Offset: 0x0008B081
	public void CleanUp()
	{
		this.BuilderManager.StopThreads(true);
		this.ChunksToProcess.Clear();
	}

	// Token: 0x060018F2 RID: 6386 RVA: 0x0008CE9C File Offset: 0x0008B09C
	public bool ProcessList()
	{
		if (this.LockGenerationUntil > DateTime.Now)
		{
			return false;
		}
		KeyValuePair<long, DynamicMeshItem> keyValuePair = this.ChunksToProcess.FirstOrDefault<KeyValuePair<long, DynamicMeshItem>>();
		if (keyValuePair.Value == null)
		{
			return false;
		}
		DynamicMeshItem value = keyValuePair.Value;
		DynamicMeshChunkProcessor nextBuilder = this.BuilderManager.GetNextBuilder();
		if (((nextBuilder != null) ? nextBuilder.AddItemForMeshPreview(value, this.PreviewData) : 0) != 1)
		{
			return false;
		}
		DynamicMeshItem dynamicMeshItem;
		this.ChunksToProcess.TryRemove(value.Key, out dynamicMeshItem);
		return true;
	}

	// Token: 0x04000FE6 RID: 4070
	public ConcurrentDictionary<long, DynamicMeshItem> ChunksToProcess = new ConcurrentDictionary<long, DynamicMeshItem>();

	// Token: 0x04000FE7 RID: 4071
	[PublicizedFrom(EAccessModifier.Private)]
	public DynamicMeshBuilderManager BuilderManager;

	// Token: 0x04000FE8 RID: 4072
	public static DynamicMeshPrefabPreviewThread Instance;

	// Token: 0x04000FE9 RID: 4073
	public ChunkPreviewData PreviewData;

	// Token: 0x04000FEA RID: 4074
	[PublicizedFrom(EAccessModifier.Private)]
	public AutoResetEvent Wait;

	// Token: 0x04000FEB RID: 4075
	[PublicizedFrom(EAccessModifier.Private)]
	public CancellationTokenSource TokenSource;

	// Token: 0x04000FEC RID: 4076
	[PublicizedFrom(EAccessModifier.Private)]
	public CancellationToken CancelToken;

	// Token: 0x04000FED RID: 4077
	[PublicizedFrom(EAccessModifier.Private)]
	public WaitHandle[] WaitHandles = new WaitHandle[2];

	// Token: 0x04000FEE RID: 4078
	[PublicizedFrom(EAccessModifier.Private)]
	public Thread PreviewCheckThread;

	// Token: 0x04000FEF RID: 4079
	[PublicizedFrom(EAccessModifier.Private)]
	public bool chunkAdded;

	// Token: 0x04000FF0 RID: 4080
	[PublicizedFrom(EAccessModifier.Private)]
	public DateTime LockGenerationUntil;
}
