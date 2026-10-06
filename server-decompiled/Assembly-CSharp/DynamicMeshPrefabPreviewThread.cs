using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

// Token: 0x0200035F RID: 863
public class DynamicMeshPrefabPreviewThread
{
	// Token: 0x170002EE RID: 750
	// (get) Token: 0x060018E7 RID: 6375 RVA: 0x0008CA72 File Offset: 0x0008AC72
	public bool HasStopBeenRequested
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.CancelToken.IsCancellationRequested;
		}
	}

	// Token: 0x060018E8 RID: 6376 RVA: 0x0008CA80 File Offset: 0x0008AC80
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

	// Token: 0x060018E9 RID: 6377 RVA: 0x0008CB10 File Offset: 0x0008AD10
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

	// Token: 0x060018EA RID: 6378 RVA: 0x0008CB60 File Offset: 0x0008AD60
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

	// Token: 0x060018EB RID: 6379 RVA: 0x0008CBF8 File Offset: 0x0008ADF8
	public void AddChunk(DynamicMeshItem item)
	{
		this.LockGenerationUntil = DateTime.Now.AddMilliseconds(300.0);
		this.ChunksToProcess.TryAdd(item.Key, item);
		this.Wait.Set();
	}

	// Token: 0x060018EC RID: 6380 RVA: 0x0008CC40 File Offset: 0x0008AE40
	public void ClearChunks()
	{
		this.ChunksToProcess.Clear();
	}

	// Token: 0x060018ED RID: 6381 RVA: 0x0008CC4D File Offset: 0x0008AE4D
	public void CleanUp()
	{
		this.BuilderManager.StopThreads(true);
		this.ChunksToProcess.Clear();
	}

	// Token: 0x060018EE RID: 6382 RVA: 0x0008CC68 File Offset: 0x0008AE68
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

	// Token: 0x04000FD6 RID: 4054
	public ConcurrentDictionary<long, DynamicMeshItem> ChunksToProcess = new ConcurrentDictionary<long, DynamicMeshItem>();

	// Token: 0x04000FD7 RID: 4055
	[PublicizedFrom(EAccessModifier.Private)]
	public DynamicMeshBuilderManager BuilderManager;

	// Token: 0x04000FD8 RID: 4056
	public static DynamicMeshPrefabPreviewThread Instance;

	// Token: 0x04000FD9 RID: 4057
	public ChunkPreviewData PreviewData;

	// Token: 0x04000FDA RID: 4058
	[PublicizedFrom(EAccessModifier.Private)]
	public AutoResetEvent Wait;

	// Token: 0x04000FDB RID: 4059
	[PublicizedFrom(EAccessModifier.Private)]
	public CancellationTokenSource TokenSource;

	// Token: 0x04000FDC RID: 4060
	[PublicizedFrom(EAccessModifier.Private)]
	public CancellationToken CancelToken;

	// Token: 0x04000FDD RID: 4061
	[PublicizedFrom(EAccessModifier.Private)]
	public WaitHandle[] WaitHandles = new WaitHandle[2];

	// Token: 0x04000FDE RID: 4062
	[PublicizedFrom(EAccessModifier.Private)]
	public Thread PreviewCheckThread;

	// Token: 0x04000FDF RID: 4063
	[PublicizedFrom(EAccessModifier.Private)]
	public bool chunkAdded;

	// Token: 0x04000FE0 RID: 4064
	[PublicizedFrom(EAccessModifier.Private)]
	public DateTime LockGenerationUntil;
}
