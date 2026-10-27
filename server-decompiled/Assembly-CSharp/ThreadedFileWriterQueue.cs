using System;
using System.Collections.Generic;

// Token: 0x02001312 RID: 4882
public class ThreadedFileWriterQueue
{
	// Token: 0x06009A52 RID: 39506 RVA: 0x003A9062 File Offset: 0x003A7262
	public ThreadedFileWriterQueue(string taskName, string filepath)
	{
		this.taskName = taskName;
		this.filepath = filepath;
	}

	// Token: 0x06009A53 RID: 39507 RVA: 0x003A9090 File Offset: 0x003A7290
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

	// Token: 0x06009A54 RID: 39508 RVA: 0x003A9114 File Offset: 0x003A7314
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

	// Token: 0x04007444 RID: 29764
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly string taskName;

	// Token: 0x04007445 RID: 29765
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly string filepath;

	// Token: 0x04007446 RID: 29766
	[PublicizedFrom(EAccessModifier.Private)]
	public object threadLock = new object();

	// Token: 0x04007447 RID: 29767
	[PublicizedFrom(EAccessModifier.Private)]
	public ThreadManager.ThreadInfo writerThread;

	// Token: 0x04007448 RID: 29768
	[PublicizedFrom(EAccessModifier.Private)]
	public Queue<PooledExpandableMemoryStream> memoryStreams = new Queue<PooledExpandableMemoryStream>();
}
