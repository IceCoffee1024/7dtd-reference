using System;
using System.Collections.Generic;

// Token: 0x02000C35 RID: 3125
public class ThreadProcessing
{
	// Token: 0x06005F31 RID: 24369 RVA: 0x0025A680 File Offset: 0x00258880
	public ThreadProcessing(List<ThreadInfoParam> _JobList)
	{
		this.JobList = new List<ThreadInfoParam>(100);
		this.Init(_JobList);
	}

	// Token: 0x06005F32 RID: 24370 RVA: 0x0025A69C File Offset: 0x0025889C
	public ThreadProcessing()
	{
		this.JobList = new List<ThreadInfoParam>(100);
		this.IsCancelled = false;
		this.IsFinished = false;
	}

	// Token: 0x06005F33 RID: 24371 RVA: 0x0025A6C0 File Offset: 0x002588C0
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

	// Token: 0x06005F34 RID: 24372 RVA: 0x0025A780 File Offset: 0x00258980
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

	// Token: 0x06005F35 RID: 24373 RVA: 0x0025A7CC File Offset: 0x002589CC
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

	// Token: 0x06005F36 RID: 24374 RVA: 0x0025A8D0 File Offset: 0x00258AD0
	public void CancelThread()
	{
		object lockObjectThread = ThreadProcessing.LockObjectThread;
		lock (lockObjectThread)
		{
			this.IsCancelled = true;
		}
	}

	// Token: 0x06005F37 RID: 24375 RVA: 0x0025A910 File Offset: 0x00258B10
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

	// Token: 0x06005F38 RID: 24376 RVA: 0x0025A968 File Offset: 0x00258B68
	public bool IsThreadFinished()
	{
		return this.IsFinished;
	}

	// Token: 0x06005F39 RID: 24377 RVA: 0x0025A970 File Offset: 0x00258B70
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

	// Token: 0x04004AAA RID: 19114
	public bool IsFinished;

	// Token: 0x04004AAB RID: 19115
	[PublicizedFrom(EAccessModifier.Private)]
	public bool IsCancelled;

	// Token: 0x04004AAC RID: 19116
	public ThreadManager.TaskInfo TaskInfo;

	// Token: 0x04004AAD RID: 19117
	[PublicizedFrom(EAccessModifier.Private)]
	public static object LockObjectThread = new object();

	// Token: 0x04004AAE RID: 19118
	public List<ThreadInfoParam> JobList;
}
