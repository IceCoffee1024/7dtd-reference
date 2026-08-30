using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Unity.Profiling;
using UnityEngine.Profiling;

// Token: 0x020014A3 RID: 5283
public sealed class SingleThreadTaskScheduler : TaskScheduler, IDisposable
{
	// Token: 0x0600A5B1 RID: 42417 RVA: 0x003DA3C0 File Offset: 0x003D85C0
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

	// Token: 0x0600A5B2 RID: 42418 RVA: 0x003DA458 File Offset: 0x003D8658
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

	// Token: 0x17001381 RID: 4993
	// (get) Token: 0x0600A5B3 RID: 42419 RVA: 0x003DA4DE File Offset: 0x003D86DE
	public Thread Thread
	{
		get
		{
			return this.m_taskThread;
		}
	}

	// Token: 0x17001382 RID: 4994
	// (get) Token: 0x0600A5B4 RID: 42420 RVA: 0x003DA4E6 File Offset: 0x003D86E6
	public TaskFactory Factory
	{
		get
		{
			return this.m_taskFactory;
		}
	}

	// Token: 0x17001383 RID: 4995
	// (get) Token: 0x0600A5B5 RID: 42421 RVA: 0x003DA4EE File Offset: 0x003D86EE
	public bool IsCurrentThread
	{
		get
		{
			return Thread.CurrentThread == this.m_taskThread;
		}
	}

	// Token: 0x0600A5B6 RID: 42422 RVA: 0x003DA4FD File Offset: 0x003D86FD
	public Task ExecuteNoWait(Action task)
	{
		if (!this.IsCurrentThread)
		{
			return this.m_taskFactory.StartNew(task);
		}
		task();
		return Task.CompletedTask;
	}

	// Token: 0x0600A5B7 RID: 42423 RVA: 0x003DA51F File Offset: 0x003D871F
	public Task<T> ExecuteNoWait<T>(Func<T> task)
	{
		if (!this.IsCurrentThread)
		{
			return this.m_taskFactory.StartNew<T>(task);
		}
		return Task.FromResult<T>(task());
	}

	// Token: 0x0600A5B8 RID: 42424 RVA: 0x003DA541 File Offset: 0x003D8741
	public void ExecuteAndWait(Action task)
	{
		if (this.IsCurrentThread)
		{
			task();
			return;
		}
		this.m_taskFactory.StartNew(task).Wait();
	}

	// Token: 0x0600A5B9 RID: 42425 RVA: 0x003DA563 File Offset: 0x003D8763
	public T ExecuteAndWait<T>(Func<T> task)
	{
		if (!this.IsCurrentThread)
		{
			return this.m_taskFactory.StartNew<T>(task).Result;
		}
		return task();
	}

	// Token: 0x0600A5BA RID: 42426 RVA: 0x003DA588 File Offset: 0x003D8788
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

	// Token: 0x17001384 RID: 4996
	// (get) Token: 0x0600A5BB RID: 42427 RVA: 0x00020021 File Offset: 0x0001E221
	public override int MaximumConcurrencyLevel
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x0600A5BC RID: 42428 RVA: 0x003DA658 File Offset: 0x003D8858
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
	{
		return Thread.CurrentThread == this.m_taskThread && (!taskWasPreviouslyQueued || this.TryDequeue(task)) && base.TryExecuteTask(task);
	}

	// Token: 0x0600A5BD RID: 42429 RVA: 0x003DA680 File Offset: 0x003D8880
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

	// Token: 0x0600A5BE RID: 42430 RVA: 0x003DA6D4 File Offset: 0x003D88D4
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

	// Token: 0x0600A5BF RID: 42431 RVA: 0x003DA71C File Offset: 0x003D891C
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

	// Token: 0x0600A5C1 RID: 42433 RVA: 0x003DA7AC File Offset: 0x003D89AC
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

	// Token: 0x04007B7A RID: 31610
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ProfilerMarker s_ProcessTasksMarker = new ProfilerMarker("SingleThreadTaskScheduler.ProcessTasks");

	// Token: 0x04007B7B RID: 31611
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ProfilerMarker s_WaitForTasksMarker = new ProfilerMarker("SingleThreadTaskScheduler.WaitForTasks");

	// Token: 0x04007B7C RID: 31612
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ProfilerMarker s_TryGetTaskMarker = new ProfilerMarker("SingleThreadTaskScheduler.TryGetTask");

	// Token: 0x04007B7D RID: 31613
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ProfilerMarker s_ExecuteTaskMarker = new ProfilerMarker("SingleThreadTaskScheduler.ExecuteTask");

	// Token: 0x04007B7E RID: 31614
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly string m_threadGroupName;

	// Token: 0x04007B7F RID: 31615
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly string m_threadName;

	// Token: 0x04007B80 RID: 31616
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly LinkedList<Task> m_tasks = new LinkedList<Task>();

	// Token: 0x04007B81 RID: 31617
	[PublicizedFrom(EAccessModifier.Private)]
	public bool m_running;

	// Token: 0x04007B82 RID: 31618
	[PublicizedFrom(EAccessModifier.Private)]
	public Thread m_taskThread;

	// Token: 0x04007B83 RID: 31619
	[PublicizedFrom(EAccessModifier.Private)]
	public AutoResetEvent m_waitHandle = new AutoResetEvent(false);

	// Token: 0x04007B84 RID: 31620
	[PublicizedFrom(EAccessModifier.Private)]
	public CancellationTokenSource m_taskCancellationSource;

	// Token: 0x04007B85 RID: 31621
	[PublicizedFrom(EAccessModifier.Private)]
	public TaskFactory m_taskFactory;
}
