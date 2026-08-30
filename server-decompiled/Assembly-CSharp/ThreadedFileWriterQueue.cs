using System;
using System.Collections.Generic;

// Token: 0x020012E0 RID: 4832
public class ThreadedFileWriterQueue
{
	// Token: 0x0600994E RID: 39246 RVA: 0x003A20AE File Offset: 0x003A02AE
	public ThreadedFileWriterQueue(string taskName, string filepath)
	{
		this.taskName = taskName;
		this.filepath = filepath;
	}

	// Token: 0x0600994F RID: 39247 RVA: 0x003A20DC File Offset: 0x003A02DC
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

	// Token: 0x06009950 RID: 39248 RVA: 0x003A2160 File Offset: 0x003A0360
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

	// Token: 0x04007388 RID: 29576
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly string taskName;

	// Token: 0x04007389 RID: 29577
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly string filepath;

	// Token: 0x0400738A RID: 29578
	[PublicizedFrom(EAccessModifier.Private)]
	public object threadLock = new object();

	// Token: 0x0400738B RID: 29579
	[PublicizedFrom(EAccessModifier.Private)]
	public ThreadManager.ThreadInfo writerThread;

	// Token: 0x0400738C RID: 29580
	[PublicizedFrom(EAccessModifier.Private)]
	public Queue<PooledExpandableMemoryStream> memoryStreams = new Queue<PooledExpandableMemoryStream>();
}
