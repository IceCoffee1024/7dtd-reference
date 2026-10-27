using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Unity.Profiling;

// Token: 0x02001464 RID: 5220
public sealed class ManualTaskScheduler : TaskScheduler, IDisposable
{
	// Token: 0x0600A36E RID: 41838 RVA: 0x003D40D2 File Offset: 0x003D22D2
	public ManualTaskScheduler()
	{
		this.m_taskCancellationSource = new CancellationTokenSource();
		this.m_taskFactory = new TaskFactory(this.m_taskCancellationSource.Token, TaskCreationOptions.None, TaskContinuationOptions.None, this);
	}

	// Token: 0x0600A36F RID: 41839 RVA: 0x003D4109 File Offset: 0x003D2309
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

	// Token: 0x0600A370 RID: 41840 RVA: 0x003D412C File Offset: 0x003D232C
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

	// Token: 0x17001363 RID: 4963
	// (get) Token: 0x0600A371 RID: 41841 RVA: 0x00020119 File Offset: 0x0001E319
	public override int MaximumConcurrencyLevel
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x17001364 RID: 4964
	// (get) Token: 0x0600A372 RID: 41842 RVA: 0x003D41C4 File Offset: 0x003D23C4
	public TaskFactory Factory
	{
		get
		{
			return this.m_taskFactory;
		}
	}

	// Token: 0x0600A373 RID: 41843 RVA: 0x003D41CC File Offset: 0x003D23CC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
	{
		return (!taskWasPreviouslyQueued || this.TryDequeue(task)) && base.TryExecuteTask(task);
	}

	// Token: 0x0600A374 RID: 41844 RVA: 0x003D41E4 File Offset: 0x003D23E4
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void QueueTask(Task task)
	{
		LinkedList<Task> tasks = this.m_tasks;
		lock (tasks)
		{
			this.m_tasks.AddLast(task);
		}
	}

	// Token: 0x0600A375 RID: 41845 RVA: 0x003D422C File Offset: 0x003D242C
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

	// Token: 0x0600A376 RID: 41846 RVA: 0x003D4274 File Offset: 0x003D2474
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

	// Token: 0x04007AAF RID: 31407
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ProfilerMarker s_ProcessTasksMarker = new ProfilerMarker("ManualTaskScheduler.ProcessTasks");

	// Token: 0x04007AB0 RID: 31408
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ProfilerMarker s_TryGetTaskMarker = new ProfilerMarker("ManualTaskScheduler.TryGetTask");

	// Token: 0x04007AB1 RID: 31409
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ProfilerMarker s_ExecuteTaskMarker = new ProfilerMarker("ManualTaskScheduler.ExecuteTask");

	// Token: 0x04007AB2 RID: 31410
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly LinkedList<Task> m_tasks = new LinkedList<Task>();

	// Token: 0x04007AB3 RID: 31411
	[PublicizedFrom(EAccessModifier.Private)]
	public CancellationTokenSource m_taskCancellationSource;

	// Token: 0x04007AB4 RID: 31412
	[PublicizedFrom(EAccessModifier.Private)]
	public TaskFactory m_taskFactory;
}
