using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Unity.Profiling;
using UnityEngine.Profiling;

// Token: 0x020014D5 RID: 5333
public sealed class SingleThreadTaskScheduler : TaskScheduler, IDisposable
{
	// Token: 0x0600A6B7 RID: 42679 RVA: 0x003E1450 File Offset: 0x003DF650
	public SingleThreadTaskScheduler(string threadGroupName, string threadName)
	{
		this.m_threadGroupName = threadGroupName;
		this.m_threadName = threadName;
		this.m_taskThread = new Thread(new ThreadStart(this.TaskThread))
		{
			Name = this.m_threadName,
			IsBackground = true
		};
		this.m_taskCancellationSource = new CancellationTokenSource();
		this.m_taskFactory = new TaskFactory(this.m_taskCancellationSource.Token, TaskCreationOptions.None, TaskContinuationOptions.None, this);
		this.m_running = true;
		this.m_taskThread.Start();
	}

	// Token: 0x0600A6B8 RID: 42680 RVA: 0x003E14E8 File Offset: 0x003DF6E8
	public void Dispose()
	{
		this.m_taskFactory = null;
		CancellationTokenSource taskCancellationSource = this.m_taskCancellationSource;
		if (taskCancellationSource != null)
		{
			taskCancellationSource.Cancel();
		}
		this.m_taskCancellationSource = null;
		this.m_running = false;
		AutoResetEvent waitHandle = this.m_waitHandle;
		if (waitHandle != null)
		{
			waitHandle.Set();
		}
		Thread taskThread = this.m_taskThread;
		if (taskThread != null)
		{
			taskThread.Interrupt();
		}
		Thread taskThread2 = this.m_taskThread;
		if (taskThread2 != null)
		{
			taskThread2.Join();
		}
		this.m_taskThread = null;
		AutoResetEvent waitHandle2 = this.m_waitHandle;
		if (waitHandle2 != null)
		{
			waitHandle2.Close();
		}
		this.m_waitHandle = null;
	}

	// Token: 0x170013D2 RID: 5074
	// (get) Token: 0x0600A6B9 RID: 42681 RVA: 0x003E156E File Offset: 0x003DF76E
	public Thread Thread
	{
		get
		{
			return this.m_taskThread;
		}
	}

	// Token: 0x170013D3 RID: 5075
	// (get) Token: 0x0600A6BA RID: 42682 RVA: 0x003E1576 File Offset: 0x003DF776
	public TaskFactory Factory
	{
		get
		{
			return this.m_taskFactory;
		}
	}

	// Token: 0x170013D4 RID: 5076
	// (get) Token: 0x0600A6BB RID: 42683 RVA: 0x003E157E File Offset: 0x003DF77E
	public bool IsCurrentThread
	{
		get
		{
			return Thread.CurrentThread == this.m_taskThread;
		}
	}

	// Token: 0x0600A6BC RID: 42684 RVA: 0x003E158D File Offset: 0x003DF78D
	public Task ExecuteNoWait(Action task)
	{
		if (!this.IsCurrentThread)
		{
			return this.m_taskFactory.StartNew(task);
		}
		task();
		return Task.CompletedTask;
	}

	// Token: 0x0600A6BD RID: 42685 RVA: 0x003E15AF File Offset: 0x003DF7AF
	public Task<T> ExecuteNoWait<T>(Func<T> task)
	{
		if (!this.IsCurrentThread)
		{
			return this.m_taskFactory.StartNew<T>(task);
		}
		return Task.FromResult<T>(task());
	}

	// Token: 0x0600A6BE RID: 42686 RVA: 0x003E15D1 File Offset: 0x003DF7D1
	public void ExecuteAndWait(Action task)
	{
		if (this.IsCurrentThread)
		{
			task();
			return;
		}
		this.m_taskFactory.StartNew(task).Wait();
	}

	// Token: 0x0600A6BF RID: 42687 RVA: 0x003E15F3 File Offset: 0x003DF7F3
	public T ExecuteAndWait<T>(Func<T> task)
	{
		if (!this.IsCurrentThread)
		{
			return this.m_taskFactory.StartNew<T>(task).Result;
		}
		return task();
	}

	// Token: 0x0600A6C0 RID: 42688 RVA: 0x003E1618 File Offset: 0x003DF818
	[PublicizedFrom(EAccessModifier.Private)]
	public void TaskThread()
	{
		Log.Out("[" + this.m_threadGroupName + "] Started SingleThreadTaskScheduler Thread: " + this.m_threadName);
		try
		{
			while (this.m_running)
			{
				try
				{
					this.<TaskThread>g__ProcessTasks|24_0();
				}
				finally
				{
				}
				try
				{
					this.m_waitHandle.WaitOne();
				}
				finally
				{
				}
			}
		}
		catch (ThreadInterruptedException)
		{
			Log.Out("[" + this.m_threadGroupName + "] Interrupted SingleThreadTaskScheduler Thread: " + this.m_threadName);
		}
		finally
		{
			Profiler.EndThreadProfiling();
			Log.Out("[" + this.m_threadGroupName + "] Stopped SingleThreadTaskScheduler Thread: " + this.m_threadName);
		}
	}

	// Token: 0x170013D5 RID: 5077
	// (get) Token: 0x0600A6C1 RID: 42689 RVA: 0x00020119 File Offset: 0x0001E319
	public override int MaximumConcurrencyLevel
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x0600A6C2 RID: 42690 RVA: 0x003E16E8 File Offset: 0x003DF8E8
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
	{
		return Thread.CurrentThread == this.m_taskThread && (!taskWasPreviouslyQueued || this.TryDequeue(task)) && base.TryExecuteTask(task);
	}

	// Token: 0x0600A6C3 RID: 42691 RVA: 0x003E1710 File Offset: 0x003DF910
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void QueueTask(Task task)
	{
		LinkedList<Task> tasks = this.m_tasks;
		lock (tasks)
		{
			this.m_tasks.AddLast(task);
			this.m_waitHandle.Set();
		}
	}

	// Token: 0x0600A6C4 RID: 42692 RVA: 0x003E1764 File Offset: 0x003DF964
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

	// Token: 0x0600A6C5 RID: 42693 RVA: 0x003E17AC File Offset: 0x003DF9AC
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

	// Token: 0x0600A6C7 RID: 42695 RVA: 0x003E183C File Offset: 0x003DFA3C
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Private)]
	public void <TaskThread>g__ProcessTasks|24_0()
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
				continue;
			}
			finally
			{
			}
			break;
		}
	}

	// Token: 0x04007C36 RID: 31798
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ProfilerMarker s_ProcessTasksMarker = new ProfilerMarker("SingleThreadTaskScheduler.ProcessTasks");

	// Token: 0x04007C37 RID: 31799
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ProfilerMarker s_WaitForTasksMarker = new ProfilerMarker("SingleThreadTaskScheduler.WaitForTasks");

	// Token: 0x04007C38 RID: 31800
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ProfilerMarker s_TryGetTaskMarker = new ProfilerMarker("SingleThreadTaskScheduler.TryGetTask");

	// Token: 0x04007C39 RID: 31801
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ProfilerMarker s_ExecuteTaskMarker = new ProfilerMarker("SingleThreadTaskScheduler.ExecuteTask");

	// Token: 0x04007C3A RID: 31802
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly string m_threadGroupName;

	// Token: 0x04007C3B RID: 31803
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly string m_threadName;

	// Token: 0x04007C3C RID: 31804
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly LinkedList<Task> m_tasks = new LinkedList<Task>();

	// Token: 0x04007C3D RID: 31805
	[PublicizedFrom(EAccessModifier.Private)]
	public bool m_running;

	// Token: 0x04007C3E RID: 31806
	[PublicizedFrom(EAccessModifier.Private)]
	public Thread m_taskThread;

	// Token: 0x04007C3F RID: 31807
	[PublicizedFrom(EAccessModifier.Private)]
	public AutoResetEvent m_waitHandle = new AutoResetEvent(false);

	// Token: 0x04007C40 RID: 31808
	[PublicizedFrom(EAccessModifier.Private)]
	public CancellationTokenSource m_taskCancellationSource;

	// Token: 0x04007C41 RID: 31809
	[PublicizedFrom(EAccessModifier.Private)]
	public TaskFactory m_taskFactory;
}
