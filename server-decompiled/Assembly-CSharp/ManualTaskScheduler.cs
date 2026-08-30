using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Unity.Profiling;

// Token: 0x02001432 RID: 5170
public sealed class ManualTaskScheduler : TaskScheduler, IDisposable
{
	// Token: 0x0600A26B RID: 41579 RVA: 0x003CD132 File Offset: 0x003CB332
	public ManualTaskScheduler()
	{
		this.m_taskCancellationSource = new CancellationTokenSource();
		this.m_taskFactory = new TaskFactory(this.m_taskCancellationSource.Token, TaskCreationOptions.None, TaskContinuationOptions.None, this);
	}

	// Token: 0x0600A26C RID: 41580 RVA: 0x003CD169 File Offset: 0x003CB369
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

	// Token: 0x0600A26D RID: 41581 RVA: 0x003CD18C File Offset: 0x003CB38C
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

	// Token: 0x17001312 RID: 4882
	// (get) Token: 0x0600A26E RID: 41582 RVA: 0x00020021 File Offset: 0x0001E221
	public override int MaximumConcurrencyLevel
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x17001313 RID: 4883
	// (get) Token: 0x0600A26F RID: 41583 RVA: 0x003CD224 File Offset: 0x003CB424
	public TaskFactory Factory
	{
		get
		{
			return this.m_taskFactory;
		}
	}

	// Token: 0x0600A270 RID: 41584 RVA: 0x003CD22C File Offset: 0x003CB42C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
	{
		return (!taskWasPreviouslyQueued || this.TryDequeue(task)) && base.TryExecuteTask(task);
	}

	// Token: 0x0600A271 RID: 41585 RVA: 0x003CD244 File Offset: 0x003CB444
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void QueueTask(Task task)
	{
		LinkedList<Task> tasks = this.m_tasks;
		lock (tasks)
		{
			this.m_tasks.AddLast(task);
		}
	}

	// Token: 0x0600A272 RID: 41586 RVA: 0x003CD28C File Offset: 0x003CB48C
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

	// Token: 0x0600A273 RID: 41587 RVA: 0x003CD2D4 File Offset: 0x003CB4D4
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

	// Token: 0x040079F3 RID: 31219
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ProfilerMarker s_ProcessTasksMarker = new ProfilerMarker("ManualTaskScheduler.ProcessTasks");

	// Token: 0x040079F4 RID: 31220
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ProfilerMarker s_TryGetTaskMarker = new ProfilerMarker("ManualTaskScheduler.TryGetTask");

	// Token: 0x040079F5 RID: 31221
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ProfilerMarker s_ExecuteTaskMarker = new ProfilerMarker("ManualTaskScheduler.ExecuteTask");

	// Token: 0x040079F6 RID: 31222
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly LinkedList<Task> m_tasks = new LinkedList<Task>();

	// Token: 0x040079F7 RID: 31223
	[PublicizedFrom(EAccessModifier.Private)]
	public CancellationTokenSource m_taskCancellationSource;

	// Token: 0x040079F8 RID: 31224
	[PublicizedFrom(EAccessModifier.Private)]
	public TaskFactory m_taskFactory;
}
