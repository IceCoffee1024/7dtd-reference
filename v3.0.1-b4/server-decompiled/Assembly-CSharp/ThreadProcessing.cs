using System;
using System.Collections.Generic;

// Token: 0x02000C23 RID: 3107
public class ThreadProcessing
{
	// Token: 0x06005EA7 RID: 24231 RVA: 0x002577D0 File Offset: 0x002559D0
	public ThreadProcessing(List<ThreadInfoParam> _JobList)
	{
		this.JobList = new List<ThreadInfoParam>(100);
		this.Init(_JobList);
	}

	// Token: 0x06005EA8 RID: 24232 RVA: 0x002577EC File Offset: 0x002559EC
	public ThreadProcessing()
	{
		this.JobList = new List<ThreadInfoParam>(100);
		this.IsCancelled = false;
		this.IsFinished = false;
	}

	// Token: 0x06005EA9 RID: 24233 RVA: 0x00257810 File Offset: 0x00255A10
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

	// Token: 0x06005EAA RID: 24234 RVA: 0x002578D0 File Offset: 0x00255AD0
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

	// Token: 0x06005EAB RID: 24235 RVA: 0x0025791C File Offset: 0x00255B1C
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

	// Token: 0x06005EAC RID: 24236 RVA: 0x00257A20 File Offset: 0x00255C20
	public void CancelThread()
	{
		object lockObjectThread = ThreadProcessing.LockObjectThread;
		lock (lockObjectThread)
		{
			this.IsCancelled = true;
		}
	}

	// Token: 0x06005EAD RID: 24237 RVA: 0x00257A60 File Offset: 0x00255C60
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

	// Token: 0x06005EAE RID: 24238 RVA: 0x00257AB8 File Offset: 0x00255CB8
	public bool IsThreadFinished()
	{
		return this.IsFinished;
	}

	// Token: 0x06005EAF RID: 24239 RVA: 0x00257AC0 File Offset: 0x00255CC0
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

	// Token: 0x04004A0D RID: 18957
	public bool IsFinished;

	// Token: 0x04004A0E RID: 18958
	[PublicizedFrom(EAccessModifier.Private)]
	public bool IsCancelled;

	// Token: 0x04004A0F RID: 18959
	public ThreadManager.TaskInfo TaskInfo;

	// Token: 0x04004A10 RID: 18960
	[PublicizedFrom(EAccessModifier.Private)]
	public static object LockObjectThread = new object();

	// Token: 0x04004A11 RID: 18961
	public List<ThreadInfoParam> JobList;
}
