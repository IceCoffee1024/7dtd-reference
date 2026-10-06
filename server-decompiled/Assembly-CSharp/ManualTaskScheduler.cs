using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Unity.Profiling;

// Token: 0x02001452 RID: 5202
public sealed class ManualTaskScheduler : TaskScheduler, IDisposable
{
	// Token: 0x0600A316 RID: 41750 RVA: 0x003D1CCE File Offset: 0x003CFECE
	public ManualTaskScheduler()
	{
		this.m_taskCancellationSource = new CancellationTokenSource();
		this.m_taskFactory = new TaskFactory(this.m_taskCancellationSource.Token, TaskCreationOptions.None, TaskContinuationOptions.None, this);
	}

	// Token: 0x0600A317 RID: 41751 RVA: 0x003D1D05 File Offset: 0x003CFF05
	public void Dispose()
	{
		this.m_taskFactory = null;
		CancellationTokenSource taskCancellationSource = this.m_taskCancellationSource;
		if (taskCancellationSource != null)
		{
			taskCancellationSource.Cancel();
		}
		this.m_taskCancellationSource = null;
	}

	// Token: 0x0600A318 RID: 41752 RVA: 0x003D1D28 File Offset: 0x003CFF28
	public void ProcessTasks()
	{
		try
		{
			for (;;)
			{
				Task value;
				try
				{
					LinkedList<Task> tasks = this.m_tasks;
					lock (tasks)
					{
						if (this.m_tasks.Count == 0)
						{
							break;
						}
						value = this.m_tasks.First.Value;
						this.m_tasks.RemoveFirst();
					}
				}
				finally
				{
				}
				try
				{
					base.TryExecuteTask(value);
				}
				finally
				{
				}
			}
		}
		finally
		{
		}
	}

	// Token: 0x17001357 RID: 4951
	// (get) Token: 0x0600A319 RID: 41753 RVA: 0x00020119 File Offset: 0x0001E319
	public override int MaximumConcurrencyLevel
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x17001358 RID: 4952
	// (get) Token: 0x0600A31A RID: 41754 RVA: 0x003D1DC0 File Offset: 0x003CFFC0
	public TaskFactory Factory
	{
		get
		{
			return this.m_taskFactory;
		}
	}

	// Token: 0x0600A31B RID: 41755 RVA: 0x003D1DC8 File Offset: 0x003CFFC8
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
	{
		return (!taskWasPreviouslyQueued || this.TryDequeue(task)) && base.TryExecuteTask(task);
	}

	// Token: 0x0600A31C RID: 41756 RVA: 0x003D1DE0 File Offset: 0x003CFFE0
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void QueueTask(Task task)
	{
		LinkedList<Task> tasks = this.m_tasks;
		lock (tasks)
		{
			this.m_tasks.AddLast(task);
		}
	}

	// Token: 0x0600A31D RID: 41757 RVA: 0x003D1E28 File Offset: 0x003D0028
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool TryDequeue(Task task)
	{
		LinkedList<Task> tasks = this.m_tasks;
		bool flag2;
		lock (tasks)
		{
			flag2 = this.m_tasks.Remove(task);
		}
		return flag2;
	}

	// Token: 0x0600A31E RID: 41758 RVA: 0x003D1E70 File Offset: 0x003D0070
	[PublicizedFrom(EAccessModifier.Protected)]
	public override IEnumerable<Task> GetScheduledTasks()
	{
		bool flag = false;
		IEnumerable<Task> tasks;
		try
		{
			Monitor.TryEnter(this.m_tasks, ref flag);
			if (!flag)
			{
				throw new NotSupportedException();
			}
			tasks = this.m_tasks;
		}
		finally
		{
			if (flag)
			{
				Monitor.Exit(this.m_tasks);
			}
		}
		return tasks;
	}

	// Token: 0x04007A6E RID: 31342
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ProfilerMarker s_ProcessTasksMarker = new ProfilerMarker("ManualTaskScheduler.ProcessTasks");

	// Token: 0x04007A6F RID: 31343
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ProfilerMarker s_TryGetTaskMarker = new ProfilerMarker("ManualTaskScheduler.TryGetTask");

	// Token: 0x04007A70 RID: 31344
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ProfilerMarker s_ExecuteTaskMarker = new ProfilerMarker("ManualTaskScheduler.ExecuteTask");

	// Token: 0x04007A71 RID: 31345
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly LinkedList<Task> m_tasks = new LinkedList<Task>();

	// Token: 0x04007A72 RID: 31346
	[PublicizedFrom(EAccessModifier.Private)]
	public CancellationTokenSource m_taskCancellationSource;

	// Token: 0x04007A73 RID: 31347
	[PublicizedFrom(EAccessModifier.Private)]
	public TaskFactory m_taskFactory;
}
