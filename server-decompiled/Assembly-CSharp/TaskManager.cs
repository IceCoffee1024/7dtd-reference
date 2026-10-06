using System;
using System.Threading;

// Token: 0x020012FB RID: 4859
public class TaskManager
{
	// Token: 0x1700126D RID: 4717
	// (get) Token: 0x060099E6 RID: 39398 RVA: 0x003A6A6C File Offset: 0x003A4C6C
	public static bool Pending
	{
		get
		{
			return TaskManager.rootGroup.Pending;
		}
	}

	// Token: 0x060099E7 RID: 39399 RVA: 0x003A6A78 File Offset: 0x003A4C78
	public static void Init()
	{
		TaskManager.rootGroup = new TaskManager.TaskGroup(null);
		TaskManager.tasks = new WorkBatch<TaskManager.Task>();
	}

	// Token: 0x060099E8 RID: 39400 RVA: 0x003A6A8F File Offset: 0x003A4C8F
	public static void Destroy()
	{
		TaskManager.WaitOnGroup(TaskManager.rootGroup);
	}

	// Token: 0x060099E9 RID: 39401 RVA: 0x003A6A9B File Offset: 0x003A4C9B
	public static void Update()
	{
		TaskManager.tasks.DoWork(new Action<TaskManager.Task>(TaskManager.CompleteTask));
	}

	// Token: 0x060099EA RID: 39402 RVA: 0x003A6AB3 File Offset: 0x003A4CB3
	[PublicizedFrom(EAccessModifier.Private)]
	public static void CompleteTask(TaskManager.Task _task)
	{
		if (_task.Complete != null)
		{
			_task.Complete();
			TaskManager.OnTaskCompleted(_task);
		}
	}

	// Token: 0x060099EB RID: 39403 RVA: 0x003A6ACE File Offset: 0x003A4CCE
	public static TaskManager.TaskGroup CreateGroup()
	{
		return new TaskManager.TaskGroup(TaskManager.rootGroup);
	}

	// Token: 0x060099EC RID: 39404 RVA: 0x003A6ADA File Offset: 0x003A4CDA
	public static TaskManager.TaskGroup CreateGroup(TaskManager.TaskGroup _parent)
	{
		return new TaskManager.TaskGroup(_parent);
	}

	// Token: 0x060099ED RID: 39405 RVA: 0x003A6AE4 File Offset: 0x003A4CE4
	public static void Schedule(Action _execute, Action _complete)
	{
		TaskManager.Task task = new TaskManager.Task(TaskManager.rootGroup, _execute, _complete);
		TaskManager.OnTaskCreated(task);
		ThreadManager.AddSingleTask(new ThreadManager.TaskFunctionDelegate(TaskManager.Execute), task, null, false);
	}

	// Token: 0x060099EE RID: 39406 RVA: 0x003A6B1C File Offset: 0x003A4D1C
	public static void Schedule(TaskManager.TaskGroup _group, Action _execute, Action _complete)
	{
		TaskManager.Task task = new TaskManager.Task(_group, _execute, _complete);
		TaskManager.OnTaskCreated(task);
		ThreadManager.AddSingleTask(new ThreadManager.TaskFunctionDelegate(TaskManager.Execute), task, null, false);
	}

	// Token: 0x060099EF RID: 39407 RVA: 0x003A6B4D File Offset: 0x003A4D4D
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

	// Token: 0x060099F0 RID: 39408 RVA: 0x003A6B88 File Offset: 0x003A4D88
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

	// Token: 0x060099F1 RID: 39409 RVA: 0x003A6BC8 File Offset: 0x003A4DC8
	[PublicizedFrom(EAccessModifier.Private)]
	public static void OnTaskCreated(TaskManager.Task task)
	{
		for (TaskManager.TaskGroup taskGroup = task.Group; taskGroup != null; taskGroup = taskGroup.parent)
		{
			Interlocked.Increment(ref taskGroup.pending);
		}
	}

	// Token: 0x060099F2 RID: 39410 RVA: 0x003A6BF4 File Offset: 0x003A4DF4
	[PublicizedFrom(EAccessModifier.Private)]
	public static void OnTaskCompleted(TaskManager.Task task)
	{
		for (TaskManager.TaskGroup taskGroup = task.Group; taskGroup != null; taskGroup = taskGroup.parent)
		{
			Interlocked.Decrement(ref taskGroup.pending);
		}
	}

	// Token: 0x040073F8 RID: 29688
	[PublicizedFrom(EAccessModifier.Private)]
	public static TaskManager.TaskGroup rootGroup;

	// Token: 0x040073F9 RID: 29689
	[PublicizedFrom(EAccessModifier.Private)]
	public static WorkBatch<TaskManager.Task> tasks;

	// Token: 0x020012FC RID: 4860
	public class TaskGroup
	{
		// Token: 0x1700126E RID: 4718
		// (get) Token: 0x060099F4 RID: 39412 RVA: 0x003A6C20 File Offset: 0x003A4E20
		public bool Pending
		{
			get
			{
				return Interlocked.CompareExchange(ref this.pending, 0, 0) != 0;
			}
		}

		// Token: 0x060099F5 RID: 39413 RVA: 0x003A6C32 File Offset: 0x003A4E32
		[PublicizedFrom(EAccessModifier.Internal)]
		public TaskGroup(TaskManager.TaskGroup _parent)
		{
			this.parent = _parent;
		}

		// Token: 0x040073FA RID: 29690
		[PublicizedFrom(EAccessModifier.Internal)]
		public TaskManager.TaskGroup parent;

		// Token: 0x040073FB RID: 29691
		[PublicizedFrom(EAccessModifier.Internal)]
		public int pending;
	}

	// Token: 0x020012FD RID: 4861
	[PublicizedFrom(EAccessModifier.Internal)]
	public class Task
	{
		// Token: 0x060099F6 RID: 39414 RVA: 0x003A6C41 File Offset: 0x003A4E41
		[PublicizedFrom(EAccessModifier.Internal)]
		public Task(TaskManager.TaskGroup _group, Action _execute, Action _complete)
		{
			this.Group = _group;
			this.Execute = _execute;
			this.Complete = _complete;
		}

		// Token: 0x040073FC RID: 29692
		[PublicizedFrom(EAccessModifier.Internal)]
		public TaskManager.TaskGroup Group;

		// Token: 0x040073FD RID: 29693
		[PublicizedFrom(EAccessModifier.Internal)]
		public Action Execute;

		// Token: 0x040073FE RID: 29694
		[PublicizedFrom(EAccessModifier.Internal)]
		public Action Complete;
	}
}
