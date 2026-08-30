using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Unity.Profiling;
using UnityEngine.Profiling;

// Token: 0x02001488 RID: 5256
public sealed class SingleThreadTaskScheduler : TaskScheduler, IDisposable
{
	// Token: 0x0600A4B4 RID: 42164 RVA: 0x003D507C File Offset: 0x003D327C
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

	// Token: 0x0600A4B5 RID: 42165 RVA: 0x003D5114 File Offset: 0x003D3314
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

	// Token: 0x17001348 RID: 4936
	// (get) Token: 0x0600A4B6 RID: 42166 RVA: 0x003D519A File Offset: 0x003D339A
	public Thread Thread
	{
		get
		{
			return this.m_taskThread;
		}
	}

	// Token: 0x17001349 RID: 4937
	// (get) Token: 0x0600A4B7 RID: 42167 RVA: 0x003D51A2 File Offset: 0x003D33A2
	public TaskFactory Factory
	{
		get
		{
			return this.m_taskFactory;
		}
	}

	// Token: 0x1700134A RID: 4938
	// (get) Token: 0x0600A4B8 RID: 42168 RVA: 0x003D51AA File Offset: 0x003D33AA
	public bool IsCurrentThread
	{
		get
		{
			return Thread.CurrentThread == this.m_taskThread;
		}
	}

	// Token: 0x0600A4B9 RID: 42169 RVA: 0x003D51B9 File Offset: 0x003D33B9
	public Task ExecuteNoWait(Action task)
	{
		if (!this.IsCurrentThread)
		{
			return this.m_taskFactory.StartNew(task);
		}
		task();
		return Task.CompletedTask;
	}

	// Token: 0x0600A4BA RID: 42170 RVA: 0x003D51DB File Offset: 0x003D33DB
	public Task<T> ExecuteNoWait<T>(Func<T> task)
	{
		if (!this.IsCurrentThread)
		{
			return this.m_taskFactory.StartNew<T>(task);
		}
		return Task.FromResult<T>(task());
	}

	// Token: 0x0600A4BB RID: 42171 RVA: 0x003D51FD File Offset: 0x003D33FD
	public void ExecuteAndWait(Action task)
	{
		if (this.IsCurrentThread)
		{
			task();
			return;
		}
		this.m_taskFactory.StartNew(task).Wait();
	}

	// Token: 0x0600A4BC RID: 42172 RVA: 0x003D521F File Offset: 0x003D341F
	public T ExecuteAndWait<T>(Func<T> task)
	{
		if (!this.IsCurrentThread)
		{
			return this.m_taskFactory.StartNew<T>(task).Result;
		}
		return task();
	}

	// Token: 0x0600A4BD RID: 42173 RVA: 0x003D5244 File Offset: 0x003D3444
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

	// Token: 0x1700134B RID: 4939
	// (get) Token: 0x0600A4BE RID: 42174 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override int MaximumConcurrencyLevel
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x0600A4BF RID: 42175 RVA: 0x003D5314 File Offset: 0x003D3514
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
	{
		return Thread.CurrentThread == this.m_taskThread && (!taskWasPreviouslyQueued || this.TryDequeue(task)) && base.TryExecuteTask(task);
	}

	// Token: 0x0600A4C0 RID: 42176 RVA: 0x003D533C File Offset: 0x003D353C
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

	// Token: 0x0600A4C1 RID: 42177 RVA: 0x003D5390 File Offset: 0x003D3590
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

	// Token: 0x0600A4C2 RID: 42178 RVA: 0x003D53D8 File Offset: 0x003D35D8
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

	// Token: 0x0600A4C4 RID: 42180 RVA: 0x003D5468 File Offset: 0x003D3668
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

	// Token: 0x04007AA1 RID: 31393
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ProfilerMarker s_ProcessTasksMarker = new ProfilerMarker("SingleThreadTaskScheduler.ProcessTasks");

	// Token: 0x04007AA2 RID: 31394
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ProfilerMarker s_WaitForTasksMarker = new ProfilerMarker("SingleThreadTaskScheduler.WaitForTasks");

	// Token: 0x04007AA3 RID: 31395
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ProfilerMarker s_TryGetTaskMarker = new ProfilerMarker("SingleThreadTaskScheduler.TryGetTask");

	// Token: 0x04007AA4 RID: 31396
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ProfilerMarker s_ExecuteTaskMarker = new ProfilerMarker("SingleThreadTaskScheduler.ExecuteTask");

	// Token: 0x04007AA5 RID: 31397
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly string m_threadGroupName;

	// Token: 0x04007AA6 RID: 31398
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly string m_threadName;

	// Token: 0x04007AA7 RID: 31399
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly LinkedList<Task> m_tasks = new LinkedList<Task>();

	// Token: 0x04007AA8 RID: 31400
	[PublicizedFrom(EAccessModifier.Private)]
	public bool m_running;

	// Token: 0x04007AA9 RID: 31401
	[PublicizedFrom(EAccessModifier.Private)]
	public Thread m_taskThread;

	// Token: 0x04007AAA RID: 31402
	[PublicizedFrom(EAccessModifier.Private)]
	public AutoResetEvent m_waitHandle = new AutoResetEvent(false);

	// Token: 0x04007AAB RID: 31403
	[PublicizedFrom(EAccessModifier.Private)]
	public CancellationTokenSource m_taskCancellationSource;

	// Token: 0x04007AAC RID: 31404
	[PublicizedFrom(EAccessModifier.Private)]
	public TaskFactory m_taskFactory;
}
