using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Unity.Profiling;

// Token: 0x02001417 RID: 5143
public sealed class ManualTaskScheduler : TaskScheduler, IDisposable
{
	// Token: 0x0600A16E RID: 41326 RVA: 0x003C7DEE File Offset: 0x003C5FEE
	public ManualTaskScheduler()
	{
		this.m_taskCancellationSource = new CancellationTokenSource();
		this.m_taskFactory = new TaskFactory(this.m_taskCancellationSource.Token, TaskCreationOptions.None, TaskContinuationOptions.None, this);
	}

	// Token: 0x0600A16F RID: 41327 RVA: 0x003C7E25 File Offset: 0x003C6025
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

	// Token: 0x0600A170 RID: 41328 RVA: 0x003C7E48 File Offset: 0x003C6048
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

	// Token: 0x170012D9 RID: 4825
	// (get) Token: 0x0600A171 RID: 41329 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override int MaximumConcurrencyLevel
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x170012DA RID: 4826
	// (get) Token: 0x0600A172 RID: 41330 RVA: 0x003C7EE0 File Offset: 0x003C60E0
	public TaskFactory Factory
	{
		get
		{
			return this.m_taskFactory;
		}
	}

	// Token: 0x0600A173 RID: 41331 RVA: 0x003C7EE8 File Offset: 0x003C60E8
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
	{
		return (!taskWasPreviouslyQueued || this.TryDequeue(task)) && base.TryExecuteTask(task);
	}

	// Token: 0x0600A174 RID: 41332 RVA: 0x003C7F00 File Offset: 0x003C6100
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void QueueTask(Task task)
	{
		LinkedList<Task> tasks = this.m_tasks;
		lock (tasks)
		{
			this.m_tasks.AddLast(task);
		}
	}

	// Token: 0x0600A175 RID: 41333 RVA: 0x003C7F48 File Offset: 0x003C6148
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

	// Token: 0x0600A176 RID: 41334 RVA: 0x003C7F90 File Offset: 0x003C6190
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

	// Token: 0x0400791A RID: 31002
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ProfilerMarker s_ProcessTasksMarker = new ProfilerMarker("ManualTaskScheduler.ProcessTasks");

	// Token: 0x0400791B RID: 31003
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ProfilerMarker s_TryGetTaskMarker = new ProfilerMarker("ManualTaskScheduler.TryGetTask");

	// Token: 0x0400791C RID: 31004
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ProfilerMarker s_ExecuteTaskMarker = new ProfilerMarker("ManualTaskScheduler.ExecuteTask");

	// Token: 0x0400791D RID: 31005
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly LinkedList<Task> m_tasks = new LinkedList<Task>();

	// Token: 0x0400791E RID: 31006
	[PublicizedFrom(EAccessModifier.Private)]
	public CancellationTokenSource m_taskCancellationSource;

	// Token: 0x0400791F RID: 31007
	[PublicizedFrom(EAccessModifier.Private)]
	public TaskFactory m_taskFactory;
}
