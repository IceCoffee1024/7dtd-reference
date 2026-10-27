using System;
using System.Threading;

// Token: 0x0200130D RID: 4877
public class TaskManager
{
	// Token: 0x17001279 RID: 4729
	// (get) Token: 0x06009A3E RID: 39486 RVA: 0x003A8E70 File Offset: 0x003A7070
	public static bool Pending
	{
		get
		{
			return TaskManager.rootGroup.Pending;
		}
	}

	// Token: 0x06009A3F RID: 39487 RVA: 0x003A8E7C File Offset: 0x003A707C
	public static void Init()
	{
		TaskManager.rootGroup = new TaskManager.TaskGroup(null);
		TaskManager.tasks = new WorkBatch<TaskManager.Task>();
	}

	// Token: 0x06009A40 RID: 39488 RVA: 0x003A8E93 File Offset: 0x003A7093
	public static void Destroy()
	{
		TaskManager.WaitOnGroup(TaskManager.rootGroup);
	}

	// Token: 0x06009A41 RID: 39489 RVA: 0x003A8E9F File Offset: 0x003A709F
	public static void Update()
	{
		TaskManager.tasks.DoWork(new Action<TaskManager.Task>(TaskManager.CompleteTask));
	}

	// Token: 0x06009A42 RID: 39490 RVA: 0x003A8EB7 File Offset: 0x003A70B7
	[PublicizedFrom(EAccessModifier.Private)]
	public static void CompleteTask(TaskManager.Task _task)
	{
		if (_task.Complete != null)
		{
			_task.Complete();
			TaskManager.OnTaskCompleted(_task);
		}
	}

	// Token: 0x06009A43 RID: 39491 RVA: 0x003A8ED2 File Offset: 0x003A70D2
	public static TaskManager.TaskGroup CreateGroup()
	{
		return new TaskManager.TaskGroup(TaskManager.rootGroup);
	}

	// Token: 0x06009A44 RID: 39492 RVA: 0x003A8EDE File Offset: 0x003A70DE
	public static TaskManager.TaskGroup CreateGroup(TaskManager.TaskGroup _parent)
	{
		return new TaskManager.TaskGroup(_parent);
	}

	// Token: 0x06009A45 RID: 39493 RVA: 0x003A8EE8 File Offset: 0x003A70E8
	public static void Schedule(Action _execute, Action _complete)
	{
		TaskManager.Task task = new TaskManager.Task(TaskManager.rootGroup, _execute, _complete);
		TaskManager.OnTaskCreated(task);
		ThreadManager.AddSingleTask(new ThreadManager.TaskFunctionDelegate(TaskManager.Execute), task, null, false);
	}

	// Token: 0x06009A46 RID: 39494 RVA: 0x003A8F20 File Offset: 0x003A7120
	public static void Schedule(TaskManager.TaskGroup _group, Action _execute, Action _complete)
	{
		TaskManager.Task task = new TaskManager.Task(_group, _execute, _complete);
		TaskManager.OnTaskCreated(task);
		ThreadManager.AddSingleTask(new ThreadManager.TaskFunctionDelegate(TaskManager.Execute), task, null, false);
	}

	// Token: 0x06009A47 RID: 39495 RVA: 0x003A8F51 File Offset: 0x003A7151
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

	// Token: 0x06009A48 RID: 39496 RVA: 0x003A8F8C File Offset: 0x003A718C
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

	// Token: 0x06009A49 RID: 39497 RVA: 0x003A8FCC File Offset: 0x003A71CC
	[PublicizedFrom(EAccessModifier.Private)]
	public static void OnTaskCreated(TaskManager.Task task)
	{
		for (TaskManager.TaskGroup taskGroup = task.Group; taskGroup != null; taskGroup = taskGroup.parent)
		{
			Interlocked.Increment(ref taskGroup.pending);
		}
	}

	// Token: 0x06009A4A RID: 39498 RVA: 0x003A8FF8 File Offset: 0x003A71F8
	[PublicizedFrom(EAccessModifier.Private)]
	public static void OnTaskCompleted(TaskManager.Task task)
	{
		for (TaskManager.TaskGroup taskGroup = task.Group; taskGroup != null; taskGroup = taskGroup.parent)
		{
			Interlocked.Decrement(ref taskGroup.pending);
		}
	}

	// Token: 0x04007439 RID: 29753
	[PublicizedFrom(EAccessModifier.Private)]
	public static TaskManager.TaskGroup rootGroup;

	// Token: 0x0400743A RID: 29754
	[PublicizedFrom(EAccessModifier.Private)]
	public static WorkBatch<TaskManager.Task> tasks;

	// Token: 0x0200130E RID: 4878
	public class TaskGroup
	{
		// Token: 0x1700127A RID: 4730
		// (get) Token: 0x06009A4C RID: 39500 RVA: 0x003A9024 File Offset: 0x003A7224
		public bool Pending
		{
			get
			{
				return Interlocked.CompareExchange(ref this.pending, 0, 0) != 0;
			}
		}

		// Token: 0x06009A4D RID: 39501 RVA: 0x003A9036 File Offset: 0x003A7236
		[PublicizedFrom(EAccessModifier.Internal)]
		public TaskGroup(TaskManager.TaskGroup _parent)
		{
			this.parent = _parent;
		}

		// Token: 0x0400743B RID: 29755
		[PublicizedFrom(EAccessModifier.Internal)]
		public TaskManager.TaskGroup parent;

		// Token: 0x0400743C RID: 29756
		[PublicizedFrom(EAccessModifier.Internal)]
		public int pending;
	}

	// Token: 0x0200130F RID: 4879
	[PublicizedFrom(EAccessModifier.Internal)]
	public class Task
	{
		// Token: 0x06009A4E RID: 39502 RVA: 0x003A9045 File Offset: 0x003A7245
		[PublicizedFrom(EAccessModifier.Internal)]
		public Task(TaskManager.TaskGroup _group, Action _execute, Action _complete)
		{
			this.Group = _group;
			this.Execute = _execute;
			this.Complete = _complete;
		}

		// Token: 0x0400743D RID: 29757
		[PublicizedFrom(EAccessModifier.Internal)]
		public TaskManager.TaskGroup Group;

		// Token: 0x0400743E RID: 29758
		[PublicizedFrom(EAccessModifier.Internal)]
		public Action Execute;

		// Token: 0x0400743F RID: 29759
		[PublicizedFrom(EAccessModifier.Internal)]
		public Action Complete;
	}
}
