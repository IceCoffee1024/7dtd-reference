using System;
using System.Threading;

// Token: 0x020012C1 RID: 4801
public class TaskManager
{
	// Token: 0x170011EF RID: 4591
	// (get) Token: 0x0600983F RID: 38975 RVA: 0x0039CC6C File Offset: 0x0039AE6C
	public static bool Pending
	{
		get
		{
			return TaskManager.rootGroup.Pending;
		}
	}

	// Token: 0x06009840 RID: 38976 RVA: 0x0039CC78 File Offset: 0x0039AE78
	public static void Init()
	{
		TaskManager.rootGroup = new TaskManager.TaskGroup(null);
		TaskManager.tasks = new WorkBatch<TaskManager.Task>();
	}

	// Token: 0x06009841 RID: 38977 RVA: 0x0039CC8F File Offset: 0x0039AE8F
	public static void Destroy()
	{
		TaskManager.WaitOnGroup(TaskManager.rootGroup);
	}

	// Token: 0x06009842 RID: 38978 RVA: 0x0039CC9B File Offset: 0x0039AE9B
	public static void Update()
	{
		TaskManager.tasks.DoWork(new Action<TaskManager.Task>(TaskManager.CompleteTask));
	}

	// Token: 0x06009843 RID: 38979 RVA: 0x0039CCB3 File Offset: 0x0039AEB3
	[PublicizedFrom(EAccessModifier.Private)]
	public static void CompleteTask(TaskManager.Task _task)
	{
		if (_task.Complete != null)
		{
			_task.Complete();
			TaskManager.OnTaskCompleted(_task);
		}
	}

	// Token: 0x06009844 RID: 38980 RVA: 0x0039CCCE File Offset: 0x0039AECE
	public static TaskManager.TaskGroup CreateGroup()
	{
		return new TaskManager.TaskGroup(TaskManager.rootGroup);
	}

	// Token: 0x06009845 RID: 38981 RVA: 0x0039CCDA File Offset: 0x0039AEDA
	public static TaskManager.TaskGroup CreateGroup(TaskManager.TaskGroup _parent)
	{
		return new TaskManager.TaskGroup(_parent);
	}

	// Token: 0x06009846 RID: 38982 RVA: 0x0039CCE4 File Offset: 0x0039AEE4
	public static void Schedule(Action _execute, Action _complete)
	{
		TaskManager.Task task = new TaskManager.Task(TaskManager.rootGroup, _execute, _complete);
		TaskManager.OnTaskCreated(task);
		ThreadManager.AddSingleTask(new ThreadManager.TaskFunctionDelegate(TaskManager.Execute), task, null, false);
	}

	// Token: 0x06009847 RID: 38983 RVA: 0x0039CD1C File Offset: 0x0039AF1C
	public static void Schedule(TaskManager.TaskGroup _group, Action _execute, Action _complete)
	{
		TaskManager.Task task = new TaskManager.Task(_group, _execute, _complete);
		TaskManager.OnTaskCreated(task);
		ThreadManager.AddSingleTask(new ThreadManager.TaskFunctionDelegate(TaskManager.Execute), task, null, false);
	}

	// Token: 0x06009848 RID: 38984 RVA: 0x0039CD4D File Offset: 0x0039AF4D
	public static void WaitOnGroup(TaskManager.TaskGroup _group)
	{
		if (!ThreadManager.MainThreadRef.Equals(Thread.CurrentThread))
		{
			throw new Exception("TaskManager.WaitOnGroup should only be called from the main thread.");
		}
		TaskManager.Update();
		while (_group.Pending)
		{
			Thread.Sleep(1);
			TaskManager.Update();
		}
	}

	// Token: 0x06009849 RID: 38985 RVA: 0x0039CD88 File Offset: 0x0039AF88
	[PublicizedFrom(EAccessModifier.Private)]
	public static void Execute(ThreadManager.TaskInfo _info)
	{
		TaskManager.Task task = _info.parameter as TaskManager.Task;
		task.Execute();
		if (task.Complete != null)
		{
			TaskManager.tasks.Add(task);
			return;
		}
		TaskManager.OnTaskCompleted(task);
	}

	// Token: 0x0600984A RID: 38986 RVA: 0x0039CDC8 File Offset: 0x0039AFC8
	[PublicizedFrom(EAccessModifier.Private)]
	public static void OnTaskCreated(TaskManager.Task task)
	{
		for (TaskManager.TaskGroup taskGroup = task.Group; taskGroup != null; taskGroup = taskGroup.parent)
		{
			Interlocked.Increment(ref taskGroup.pending);
		}
	}

	// Token: 0x0600984B RID: 38987 RVA: 0x0039CDF4 File Offset: 0x0039AFF4
	[PublicizedFrom(EAccessModifier.Private)]
	public static void OnTaskCompleted(TaskManager.Task task)
	{
		for (TaskManager.TaskGroup taskGroup = task.Group; taskGroup != null; taskGroup = taskGroup.parent)
		{
			Interlocked.Decrement(ref taskGroup.pending);
		}
	}

	// Token: 0x040072A6 RID: 29350
	[PublicizedFrom(EAccessModifier.Private)]
	public static TaskManager.TaskGroup rootGroup;

	// Token: 0x040072A7 RID: 29351
	[PublicizedFrom(EAccessModifier.Private)]
	public static WorkBatch<TaskManager.Task> tasks;

	// Token: 0x020012C2 RID: 4802
	public class TaskGroup
	{
		// Token: 0x170011F0 RID: 4592
		// (get) Token: 0x0600984D RID: 38989 RVA: 0x0039CE20 File Offset: 0x0039B020
		public bool Pending
		{
			get
			{
				return Interlocked.CompareExchange(ref this.pending, 0, 0) != 0;
			}
		}

		// Token: 0x0600984E RID: 38990 RVA: 0x0039CE32 File Offset: 0x0039B032
		[PublicizedFrom(EAccessModifier.Internal)]
		public TaskGroup(TaskManager.TaskGroup _parent)
		{
			this.parent = _parent;
		}

		// Token: 0x040072A8 RID: 29352
		[PublicizedFrom(EAccessModifier.Internal)]
		public TaskManager.TaskGroup parent;

		// Token: 0x040072A9 RID: 29353
		[PublicizedFrom(EAccessModifier.Internal)]
		public int pending;
	}

	// Token: 0x020012C3 RID: 4803
	[PublicizedFrom(EAccessModifier.Internal)]
	public class Task
	{
		// Token: 0x0600984F RID: 38991 RVA: 0x0039CE41 File Offset: 0x0039B041
		[PublicizedFrom(EAccessModifier.Internal)]
		public Task(TaskManager.TaskGroup _group, Action _execute, Action _complete)
		{
			this.Group = _group;
			this.Execute = _execute;
			this.Complete = _complete;
		}

		// Token: 0x040072AA RID: 29354
		[PublicizedFrom(EAccessModifier.Internal)]
		public TaskManager.TaskGroup Group;

		// Token: 0x040072AB RID: 29355
		[PublicizedFrom(EAccessModifier.Internal)]
		public Action Execute;

		// Token: 0x040072AC RID: 29356
		[PublicizedFrom(EAccessModifier.Internal)]
		public Action Complete;
	}
}
