using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Unity.Profiling;
using UnityEngine.Profiling;

// Token: 0x020014C3 RID: 5315
public sealed class SingleThreadTaskScheduler : TaskScheduler, IDisposable
{
	// Token: 0x0600A65F RID: 42591 RVA: 0x003DF04C File Offset: 0x003DD24C
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

	// Token: 0x0600A660 RID: 42592 RVA: 0x003DF0E4 File Offset: 0x003DD2E4
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

	// Token: 0x170013C6 RID: 5062
	// (get) Token: 0x0600A661 RID: 42593 RVA: 0x003DF16A File Offset: 0x003DD36A
	public Thread Thread
	{
		get
		{
			return this.m_taskThread;
		}
	}

	// Token: 0x170013C7 RID: 5063
	// (get) Token: 0x0600A662 RID: 42594 RVA: 0x003DF172 File Offset: 0x003DD372
	public TaskFactory Factory
	{
		get
		{
			return this.m_taskFactory;
		}
	}

	// Token: 0x170013C8 RID: 5064
	// (get) Token: 0x0600A663 RID: 42595 RVA: 0x003DF17A File Offset: 0x003DD37A
	public bool IsCurrentThread
	{
		get
		{
			return Thread.CurrentThread == this.m_taskThread;
		}
	}

	// Token: 0x0600A664 RID: 42596 RVA: 0x003DF189 File Offset: 0x003DD389
	public Task ExecuteNoWait(Action task)
	{
		if (!this.IsCurrentThread)
		{
			return this.m_taskFactory.StartNew(task);
		}
		task();
		return Task.CompletedTask;
	}

	// Token: 0x0600A665 RID: 42597 RVA: 0x003DF1AB File Offset: 0x003DD3AB
	public Task<T> ExecuteNoWait<T>(Func<T> task)
	{
		if (!this.IsCurrentThread)
		{
			return this.m_taskFactory.StartNew<T>(task);
		}
		return Task.FromResult<T>(task());
	}

	// Token: 0x0600A666 RID: 42598 RVA: 0x003DF1CD File Offset: 0x003DD3CD
	public void ExecuteAndWait(Action task)
	{
		if (this.IsCurrentThread)
		{
			task();
			return;
		}
		this.m_taskFactory.StartNew(task).Wait();
	}

	// Token: 0x0600A667 RID: 42599 RVA: 0x003DF1EF File Offset: 0x003DD3EF
	public T ExecuteAndWait<T>(Func<T> task)
	{
		if (!this.IsCurrentThread)
		{
			return this.m_taskFactory.StartNew<T>(task).Result;
		}
		return task();
	}

	// Token: 0x0600A668 RID: 42600 RVA: 0x003DF214 File Offset: 0x003DD414
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

	// Token: 0x170013C9 RID: 5065
	// (get) Token: 0x0600A669 RID: 42601 RVA: 0x00020119 File Offset: 0x0001E319
	public override int MaximumConcurrencyLevel
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x0600A66A RID: 42602 RVA: 0x003DF2E4 File Offset: 0x003DD4E4
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
	{
		return Thread.CurrentThread == this.m_taskThread && (!taskWasPreviouslyQueued || this.TryDequeue(task)) && base.TryExecuteTask(task);
	}

	// Token: 0x0600A66B RID: 42603 RVA: 0x003DF30C File Offset: 0x003DD50C
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

	// Token: 0x0600A66C RID: 42604 RVA: 0x003DF360 File Offset: 0x003DD560
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

	// Token: 0x0600A66D RID: 42605 RVA: 0x003DF3A8 File Offset: 0x003DD5A8
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

	// Token: 0x0600A66F RID: 42607 RVA: 0x003DF438 File Offset: 0x003DD638
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

	// Token: 0x04007BF5 RID: 31733
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ProfilerMarker s_ProcessTasksMarker = new ProfilerMarker("SingleThreadTaskScheduler.ProcessTasks");

	// Token: 0x04007BF6 RID: 31734
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ProfilerMarker s_WaitForTasksMarker = new ProfilerMarker("SingleThreadTaskScheduler.WaitForTasks");

	// Token: 0x04007BF7 RID: 31735
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ProfilerMarker s_TryGetTaskMarker = new ProfilerMarker("SingleThreadTaskScheduler.TryGetTask");

	// Token: 0x04007BF8 RID: 31736
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ProfilerMarker s_ExecuteTaskMarker = new ProfilerMarker("SingleThreadTaskScheduler.ExecuteTask");

	// Token: 0x04007BF9 RID: 31737
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly string m_threadGroupName;

	// Token: 0x04007BFA RID: 31738
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly string m_threadName;

	// Token: 0x04007BFB RID: 31739
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly LinkedList<Task> m_tasks = new LinkedList<Task>();

	// Token: 0x04007BFC RID: 31740
	[PublicizedFrom(EAccessModifier.Private)]
	public bool m_running;

	// Token: 0x04007BFD RID: 31741
	[PublicizedFrom(EAccessModifier.Private)]
	public Thread m_taskThread;

	// Token: 0x04007BFE RID: 31742
	[PublicizedFrom(EAccessModifier.Private)]
	public AutoResetEvent m_waitHandle = new AutoResetEvent(false);

	// Token: 0x04007BFF RID: 31743
	[PublicizedFrom(EAccessModifier.Private)]
	public CancellationTokenSource m_taskCancellationSource;

	// Token: 0x04007C00 RID: 31744
	[PublicizedFrom(EAccessModifier.Private)]
	public TaskFactory m_taskFactory;
}
