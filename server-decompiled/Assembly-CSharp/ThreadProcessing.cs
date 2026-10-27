using System;
using System.Collections.Generic;

// Token: 0x02000C52 RID: 3154
public class ThreadProcessing
{
	// Token: 0x06005F6F RID: 24431 RVA: 0x0025EEA4 File Offset: 0x0025D0A4
	public ThreadProcessing(List<ThreadInfoParam> _JobList)
	{
		this.JobList = new List<ThreadInfoParam>(100);
		this.Init(_JobList);
	}

	// Token: 0x06005F70 RID: 24432 RVA: 0x0025EEC0 File Offset: 0x0025D0C0
	public ThreadProcessing()
	{
		this.JobList = new List<ThreadInfoParam>(100);
		this.IsCancelled = false;
		this.IsFinished = false;
	}

	// Token: 0x06005F71 RID: 24433 RVA: 0x0025EEE4 File Offset: 0x0025D0E4
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

	// Token: 0x06005F72 RID: 24434 RVA: 0x0025EFA4 File Offset: 0x0025D1A4
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

	// Token: 0x06005F73 RID: 24435 RVA: 0x0025EFF0 File Offset: 0x0025D1F0
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

	// Token: 0x06005F74 RID: 24436 RVA: 0x0025F0F4 File Offset: 0x0025D2F4
	public void CancelThread()
	{
		object lockObjectThread = ThreadProcessing.LockObjectThread;
		lock (lockObjectThread)
		{
			this.IsCancelled = true;
		}
	}

	// Token: 0x06005F75 RID: 24437 RVA: 0x0025F134 File Offset: 0x0025D334
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

	// Token: 0x06005F76 RID: 24438 RVA: 0x0025F18C File Offset: 0x0025D38C
	public bool IsThreadFinished()
	{
		return this.IsFinished;
	}

	// Token: 0x06005F77 RID: 24439 RVA: 0x0025F194 File Offset: 0x0025D394
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

	// Token: 0x04004B0E RID: 19214
	public bool IsFinished;

	// Token: 0x04004B0F RID: 19215
	[PublicizedFrom(EAccessModifier.Private)]
	public bool IsCancelled;

	// Token: 0x04004B10 RID: 19216
	public ThreadManager.TaskInfo TaskInfo;

	// Token: 0x04004B11 RID: 19217
	[PublicizedFrom(EAccessModifier.Private)]
	public static object LockObjectThread = new object();

	// Token: 0x04004B12 RID: 19218
	public List<ThreadInfoParam> JobList;
}
