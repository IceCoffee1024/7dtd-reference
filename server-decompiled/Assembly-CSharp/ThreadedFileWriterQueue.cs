using System;
using System.Collections.Generic;

// Token: 0x02001300 RID: 4864
public class ThreadedFileWriterQueue
{
	// Token: 0x060099FA RID: 39418 RVA: 0x003A6C5E File Offset: 0x003A4E5E
	public ThreadedFileWriterQueue(string taskName, string filepath)
	{
		this.taskName = taskName;
		this.filepath = filepath;
	}

	// Token: 0x060099FB RID: 39419 RVA: 0x003A6C8C File Offset: 0x003A4E8C
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

	// Token: 0x060099FC RID: 39420 RVA: 0x003A6D10 File Offset: 0x003A4F10
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

	// Token: 0x04007403 RID: 29699
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly string taskName;

	// Token: 0x04007404 RID: 29700
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly string filepath;

	// Token: 0x04007405 RID: 29701
	[PublicizedFrom(EAccessModifier.Private)]
	public object threadLock = new object();

	// Token: 0x04007406 RID: 29702
	[PublicizedFrom(EAccessModifier.Private)]
	public ThreadManager.ThreadInfo writerThread;

	// Token: 0x04007407 RID: 29703
	[PublicizedFrom(EAccessModifier.Private)]
	public Queue<PooledExpandableMemoryStream> memoryStreams = new Queue<PooledExpandableMemoryStream>();
}
