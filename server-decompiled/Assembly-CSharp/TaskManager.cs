using System;
using System.Threading;

// Token: 0x020012DB RID: 4827
public class TaskManager
{
	// Token: 0x17001228 RID: 4648
	// (get) Token: 0x0600993A RID: 39226 RVA: 0x003A1EBC File Offset: 0x003A00BC
	public static bool Pending
	{
		get
		{
			return TaskManager.rootGroup.Pending;
		}
	}

	// Token: 0x0600993B RID: 39227 RVA: 0x003A1EC8 File Offset: 0x003A00C8
	public static void Init()
	{
		TaskManager.rootGroup = new TaskManager.TaskGroup(null);
		TaskManager.tasks = new WorkBatch<TaskManager.Task>();
	}

	// Token: 0x0600993C RID: 39228 RVA: 0x003A1EDF File Offset: 0x003A00DF
	public static void Destroy()
	{
		TaskManager.WaitOnGroup(TaskManager.rootGroup);
	}

	// Token: 0x0600993D RID: 39229 RVA: 0x003A1EEB File Offset: 0x003A00EB
	public static void Update()
	{
		TaskManager.tasks.DoWork(new Action<TaskManager.Task>(TaskManager.CompleteTask));
	}

	// Token: 0x0600993E RID: 39230 RVA: 0x003A1F03 File Offset: 0x003A0103
	[PublicizedFrom(EAccessModifier.Private)]
	public static void CompleteTask(TaskManager.Task _task)
	{
		if (_task.Complete != null)
		{
			_task.Complete();
			TaskManager.OnTaskCompleted(_task);
		}
	}

	// Token: 0x0600993F RID: 39231 RVA: 0x003A1F1E File Offset: 0x003A011E
	public static TaskManager.TaskGroup CreateGroup()
	{
		return new TaskManager.TaskGroup(TaskManager.rootGroup);
	}

	// Token: 0x06009940 RID: 39232 RVA: 0x003A1F2A File Offset: 0x003A012A
	public static TaskManager.TaskGroup CreateGroup(TaskManager.TaskGroup _parent)
	{
		return new TaskManager.TaskGroup(_parent);
	}

	// Token: 0x06009941 RID: 39233 RVA: 0x003A1F34 File Offset: 0x003A0134
	public static void Schedule(Action _execute, Action _complete)
	{
		TaskManager.Task task = new TaskManager.Task(TaskManager.rootGroup, _execute, _complete);
		TaskManager.OnTaskCreated(task);
		ThreadManager.AddSingleTask(new ThreadManager.TaskFunctionDelegate(TaskManager.Execute), task, null, false);
	}

	// Token: 0x06009942 RID: 39234 RVA: 0x003A1F6C File Offset: 0x003A016C
	public static void Schedule(TaskManager.TaskGroup _group, Action _execute, Action _complete)
	{
		TaskManager.Task task = new TaskManager.Task(_group, _execute, _complete);
		TaskManager.OnTaskCreated(task);
		ThreadManager.AddSingleTask(new ThreadManager.TaskFunctionDelegate(TaskManager.Execute), task, null, false);
	}

	// Token: 0x06009943 RID: 39235 RVA: 0x003A1F9D File Offset: 0x003A019D
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

	// Token: 0x06009944 RID: 39236 RVA: 0x003A1FD8 File Offset: 0x003A01D8
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

	// Token: 0x06009945 RID: 39237 RVA: 0x003A2018 File Offset: 0x003A0218
	[PublicizedFrom(EAccessModifier.Private)]
	public static void OnTaskCreated(TaskManager.Task task)
	{
		for (TaskManager.TaskGroup taskGroup = task.Group; taskGroup != null; taskGroup = taskGroup.parent)
		{
			Interlocked.Increment(ref taskGroup.pending);
		}
	}

	// Token: 0x06009946 RID: 39238 RVA: 0x003A2044 File Offset: 0x003A0244
	[PublicizedFrom(EAccessModifier.Private)]
	public static void OnTaskCompleted(TaskManager.Task task)
	{
		for (TaskManager.TaskGroup taskGroup = task.Group; taskGroup != null; taskGroup = taskGroup.parent)
		{
			Interlocked.Decrement(ref taskGroup.pending);
		}
	}

	// Token: 0x0400737D RID: 29565
	[PublicizedFrom(EAccessModifier.Private)]
	public static TaskManager.TaskGroup rootGroup;

	// Token: 0x0400737E RID: 29566
	[PublicizedFrom(EAccessModifier.Private)]
	public static WorkBatch<TaskManager.Task> tasks;

	// Token: 0x020012DC RID: 4828
	public class TaskGroup
	{
		// Token: 0x17001229 RID: 4649
		// (get) Token: 0x06009948 RID: 39240 RVA: 0x003A2070 File Offset: 0x003A0270
		public bool Pending
		{
			get
			{
				return Interlocked.CompareExchange(ref this.pending, 0, 0) != 0;
			}
		}

		// Token: 0x06009949 RID: 39241 RVA: 0x003A2082 File Offset: 0x003A0282
		[PublicizedFrom(EAccessModifier.Internal)]
		public TaskGroup(TaskManager.TaskGroup _parent)
		{
			this.parent = _parent;
		}

		// Token: 0x0400737F RID: 29567
		[PublicizedFrom(EAccessModifier.Internal)]
		public TaskManager.TaskGroup parent;

		// Token: 0x04007380 RID: 29568
		[PublicizedFrom(EAccessModifier.Internal)]
		public int pending;
	}

	// Token: 0x020012DD RID: 4829
	[PublicizedFrom(EAccessModifier.Internal)]
	public class Task
	{
		// Token: 0x0600994A RID: 39242 RVA: 0x003A2091 File Offset: 0x003A0291
		[PublicizedFrom(EAccessModifier.Internal)]
		public Task(TaskManager.TaskGroup _group, Action _execute, Action _complete)
		{
			this.Group = _group;
			this.Execute = _execute;
			this.Complete = _complete;
		}

		// Token: 0x04007381 RID: 29569
		[PublicizedFrom(EAccessModifier.Internal)]
		public TaskManager.TaskGroup Group;

		// Token: 0x04007382 RID: 29570
		[PublicizedFrom(EAccessModifier.Internal)]
		public Action Execute;

		// Token: 0x04007383 RID: 29571
		[PublicizedFrom(EAccessModifier.Internal)]
		public Action Complete;
	}
}
