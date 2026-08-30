using System;
using System.Collections.Generic;

// Token: 0x020012C6 RID: 4806
public class ThreadedFileWriterQueue
{
	// Token: 0x06009853 RID: 38995 RVA: 0x0039CE5E File Offset: 0x0039B05E
	public ThreadedFileWriterQueue(string taskName, string filepath)
	{
		this.taskName = taskName;
		this.filepath = filepath;
	}

	// Token: 0x06009854 RID: 38996 RVA: 0x0039CE8C File Offset: 0x0039B08C
	public void Write(PooledExpandableMemoryStream stream, bool waitForComplete = false)
	{
		object obj = this.threadLock;
		ThreadManager.ThreadInfo threadInfo;
		lock (obj)
		{
			this.memoryStreams.Enqueue(stream);
			if (this.writerThread == null)
			{
				this.writerThread = ThreadManager.StartThread(this.taskName, new ThreadManager.ThreadFunctionDelegate(this.WriteSaveFileThread), null, null, false, true);
			}
			threadInfo = this.writerThread;
		}
		if (waitForComplete)
		{
			threadInfo.WaitForEnd(30);
		}
	}

	// Token: 0x06009855 RID: 38997 RVA: 0x0039CF10 File Offset: 0x0039B110
	[PublicizedFrom(EAccessModifier.Private)]
	public void WriteSaveFileThread(ThreadManager.ThreadInfo threadInfo)
	{
		for (;;)
		{
			object obj = this.threadLock;
			PooledExpandableMemoryStream pooledExpandableMemoryStream;
			lock (obj)
			{
				if (!this.memoryStreams.TryDequeue(out pooledExpandableMemoryStream))
				{
					this.writerThread = null;
					break;
				}
			}
			try
			{
				StreamUtils.WriteStreamToFile(pooledExpandableMemoryStream, this.filepath);
			}
			catch (Exception ex)
			{
				Log.Error("[" + this.taskName + "] Error while writing to file " + ex.Message);
				Log.Exception(ex);
			}
			MemoryPools.poolMemoryStream.FreeSync(pooledExpandableMemoryStream);
		}
	}

	// Token: 0x040072B1 RID: 29361
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly string taskName;

	// Token: 0x040072B2 RID: 29362
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly string filepath;

	// Token: 0x040072B3 RID: 29363
	[PublicizedFrom(EAccessModifier.Private)]
	public object threadLock = new object();

	// Token: 0x040072B4 RID: 29364
	[PublicizedFrom(EAccessModifier.Private)]
	public ThreadManager.ThreadInfo writerThread;

	// Token: 0x040072B5 RID: 29365
	[PublicizedFrom(EAccessModifier.Private)]
	public Queue<PooledExpandableMemoryStream> memoryStreams = new Queue<PooledExpandableMemoryStream>();
}
