using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

// Token: 0x0200035C RID: 860
public class DynamicMeshPrefabPreviewThread
{
	// Token: 0x170002F6 RID: 758
	// (get) Token: 0x060018EC RID: 6380 RVA: 0x0008C7F2 File Offset: 0x0008A9F2
	public bool HasStopBeenRequested
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.CancelToken.IsCancellationRequested;
		}
	}

	// Token: 0x060018ED RID: 6381 RVA: 0x0008C800 File Offset: 0x0008AA00
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

	// Token: 0x060018EE RID: 6382 RVA: 0x0008C890 File Offset: 0x0008AA90
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

	// Token: 0x060018EF RID: 6383 RVA: 0x0008C8E0 File Offset: 0x0008AAE0
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

	// Token: 0x060018F0 RID: 6384 RVA: 0x0008C978 File Offset: 0x0008AB78
	public void AddChunk(DynamicMeshItem item)
	{
		this.LockGenerationUntil = DateTime.Now.AddMilliseconds(300.0);
		this.ChunksToProcess.TryAdd(item.Key, item);
		this.Wait.Set();
	}

	// Token: 0x060018F1 RID: 6385 RVA: 0x0008C9C0 File Offset: 0x0008ABC0
	public void ClearChunks()
	{
		this.ChunksToProcess.Clear();
	}

	// Token: 0x060018F2 RID: 6386 RVA: 0x0008C9CD File Offset: 0x0008ABCD
	public void CleanUp()
	{
		this.BuilderManager.StopThreads(true);
		this.ChunksToProcess.Clear();
	}

	// Token: 0x060018F3 RID: 6387 RVA: 0x0008C9E8 File Offset: 0x0008ABE8
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

	// Token: 0x04000FDD RID: 4061
	public ConcurrentDictionary<long, DynamicMeshItem> ChunksToProcess = new ConcurrentDictionary<long, DynamicMeshItem>();

	// Token: 0x04000FDE RID: 4062
	[PublicizedFrom(EAccessModifier.Private)]
	public DynamicMeshBuilderManager BuilderManager;

	// Token: 0x04000FDF RID: 4063
	public static DynamicMeshPrefabPreviewThread Instance;

	// Token: 0x04000FE0 RID: 4064
	public ChunkPreviewData PreviewData;

	// Token: 0x04000FE1 RID: 4065
	[PublicizedFrom(EAccessModifier.Private)]
	public AutoResetEvent Wait;

	// Token: 0x04000FE2 RID: 4066
	[PublicizedFrom(EAccessModifier.Private)]
	public CancellationTokenSource TokenSource;

	// Token: 0x04000FE3 RID: 4067
	[PublicizedFrom(EAccessModifier.Private)]
	public CancellationToken CancelToken;

	// Token: 0x04000FE4 RID: 4068
	[PublicizedFrom(EAccessModifier.Private)]
	public WaitHandle[] WaitHandles = new WaitHandle[2];

	// Token: 0x04000FE5 RID: 4069
	[PublicizedFrom(EAccessModifier.Private)]
	public Thread PreviewCheckThread;

	// Token: 0x04000FE6 RID: 4070
	[PublicizedFrom(EAccessModifier.Private)]
	public bool chunkAdded;

	// Token: 0x04000FE7 RID: 4071
	[PublicizedFrom(EAccessModifier.Private)]
	public DateTime LockGenerationUntil;
}
