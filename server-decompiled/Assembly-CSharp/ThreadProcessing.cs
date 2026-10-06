using System;
using System.Collections.Generic;

// Token: 0x02000C47 RID: 3143
public class ThreadProcessing
{
	// Token: 0x06005F4A RID: 24394 RVA: 0x0025D97C File Offset: 0x0025BB7C
	public ThreadProcessing(List<ThreadInfoParam> _JobList)
	{
		this.JobList = new List<ThreadInfoParam>(100);
		this.Init(_JobList);
	}

	// Token: 0x06005F4B RID: 24395 RVA: 0x0025D998 File Offset: 0x0025BB98
	public ThreadProcessing()
	{
		this.JobList = new List<ThreadInfoParam>(100);
		this.IsCancelled = false;
		this.IsFinished = false;
	}

	// Token: 0x06005F4C RID: 24396 RVA: 0x0025D9BC File Offset: 0x0025BBBC
	public void Init(List<ThreadInfoParam> _JobList)
	{
		this.RemoveTreatedElement(_JobList);
		if (this.JobList.Count == 0)
		{
			this.IsFinished = true;
			return;
		}
		this.IsCancelled = false;
		this.IsFinished = false;
		for (int i = 0; i < this.JobList.Count; i++)
		{
			for (int j = 0; j < this.JobList[i].LengthThreadContList; j++)
			{
				DistantChunk dchunk = this.JobList[i].ThreadContListA[j].DChunk;
				dchunk.CellMeshData = DistantChunk.SMPool.GetObject(dchunk.BaseChunkMap, dchunk.ResLevel);
			}
		}
		this.TaskInfo = ThreadManager.AddSingleTask(new ThreadManager.TaskFunctionDelegate(this.ThreadJob), this.JobList, null, true);
	}

	// Token: 0x06005F4D RID: 24397 RVA: 0x0025DA7C File Offset: 0x0025BC7C
	public void RemoveTreatedElement(List<ThreadInfoParam> _JobList)
	{
		this.JobList.Clear();
		for (int i = 0; i < _JobList.Count; i++)
		{
			if (!_JobList[i].IsThreadDone)
			{
				this.JobList.Add(_JobList[i]);
			}
		}
	}

	// Token: 0x06005F4E RID: 24398 RVA: 0x0025DAC8 File Offset: 0x0025BCC8
	[PublicizedFrom(EAccessModifier.Private)]
	public void ThreadJob(ThreadManager.TaskInfo _InfoJob)
	{
		List<ThreadInfoParam> list = (List<ThreadInfoParam>)_InfoJob.parameter;
		object obj;
		for (int i = 0; i < list.Count; i++)
		{
			for (int j = 0; j < list[i].LengthThreadContList; j++)
			{
				obj = ThreadProcessing.LockObjectThread;
				lock (obj)
				{
					if (this.IsCancelled)
					{
						break;
					}
					list[i].ThreadContListA[j].ThreadExtraWork();
				}
			}
			obj = ThreadProcessing.LockObjectThread;
			lock (obj)
			{
				if (this.IsCancelled)
				{
					break;
				}
				list[i].IsThreadDone = true;
			}
		}
		obj = ThreadProcessing.LockObjectThread;
		lock (obj)
		{
			this.IsFinished = true;
		}
	}

	// Token: 0x06005F4F RID: 24399 RVA: 0x0025DBCC File Offset: 0x0025BDCC
	public void CancelThread()
	{
		object lockObjectThread = ThreadProcessing.LockObjectThread;
		lock (lockObjectThread)
		{
			this.IsCancelled = true;
		}
	}

	// Token: 0x06005F50 RID: 24400 RVA: 0x0025DC0C File Offset: 0x0025BE0C
	public long CancelThreadAndWaitFinished()
	{
		object lockObjectThread = ThreadProcessing.LockObjectThread;
		lock (lockObjectThread)
		{
			this.IsCancelled = true;
		}
		if (this.TaskInfo != null)
		{
			this.TaskInfo.WaitForEnd();
		}
		return 0L;
	}

	// Token: 0x06005F51 RID: 24401 RVA: 0x0025DC64 File Offset: 0x0025BE64
	public bool IsThreadFinished()
	{
		return this.IsFinished;
	}

	// Token: 0x06005F52 RID: 24402 RVA: 0x0025DC6C File Offset: 0x0025BE6C
	public bool IsThreadDone(int ThreadInfoParamId)
	{
		object lockObjectThread = ThreadProcessing.LockObjectThread;
		bool isThreadDone;
		lock (lockObjectThread)
		{
			isThreadDone = this.JobList[ThreadInfoParamId].IsThreadDone;
		}
		return isThreadDone;
	}

	// Token: 0x04004AE9 RID: 19177
	public bool IsFinished;

	// Token: 0x04004AEA RID: 19178
	[PublicizedFrom(EAccessModifier.Private)]
	public bool IsCancelled;

	// Token: 0x04004AEB RID: 19179
	public ThreadManager.TaskInfo TaskInfo;

	// Token: 0x04004AEC RID: 19180
	[PublicizedFrom(EAccessModifier.Private)]
	public static object LockObjectThread = new object();

	// Token: 0x04004AED RID: 19181
	public List<ThreadInfoParam> JobList;
}
