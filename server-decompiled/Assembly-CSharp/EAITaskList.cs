using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x0200045F RID: 1119
[Preserve]
public class EAITaskList
{
	// Token: 0x060021E6 RID: 8678 RVA: 0x000CD898 File Offset: 0x000CBA98
	public EAITaskList(EAIManager _manager)
	{
		this.allTasks = new List<EAITaskEntry>();
		this.executingTasks = new List<EAITaskEntry>();
		this.executeDelayScale = 0.85f + _manager.random.RandomFloat * 0.25f;
	}

	// Token: 0x060021E7 RID: 8679 RVA: 0x000CD8E9 File Offset: 0x000CBAE9
	public void AddTask(int _priority, EAIBase _eai)
	{
		this.allTasks.Add(new EAITaskEntry(_priority, _eai));
	}

	// Token: 0x060021E8 RID: 8680 RVA: 0x000CD900 File Offset: 0x000CBB00
	public void AddTaskList(EAITaskList _taskList)
	{
		for (int i = 0; i < _taskList.Tasks.Count; i++)
		{
			this.allTasks.Add(_taskList.Tasks[i]);
		}
	}

	// Token: 0x060021E9 RID: 8681 RVA: 0x000CD93A File Offset: 0x000CBB3A
	public void RemoveTask(EAITaskEntry _entry)
	{
		this.allTasks.Remove(_entry);
		if (this.executingTasks.Contains(_entry))
		{
			this.executingTasks.Remove(_entry);
		}
	}

	// Token: 0x170003DC RID: 988
	// (get) Token: 0x060021EA RID: 8682 RVA: 0x000CD964 File Offset: 0x000CBB64
	public List<EAITaskEntry> Tasks
	{
		get
		{
			return this.allTasks;
		}
	}

	// Token: 0x060021EB RID: 8683 RVA: 0x000CD96C File Offset: 0x000CBB6C
	public List<EAITaskEntry> GetExecutingTasks()
	{
		return this.executingTasks;
	}

	// Token: 0x060021EC RID: 8684 RVA: 0x000CD974 File Offset: 0x000CBB74
	public T GetTask<T>() where T : class
	{
		for (int i = 0; i < this.allTasks.Count; i++)
		{
			T t = this.allTasks[i].action as T;
			if (t != null)
			{
				return t;
			}
		}
		return default(T);
	}

	// Token: 0x060021ED RID: 8685 RVA: 0x000CD9C8 File Offset: 0x000CBBC8
	public void OnUpdateTasks()
	{
		this.startedTasks.Clear();
		int i = 0;
		while (i < this.allTasks.Count)
		{
			EAITaskEntry eaitaskEntry = this.allTasks[i];
			if (!eaitaskEntry.isExecuting)
			{
				goto IL_007A;
			}
			if (!this.isBestTask(eaitaskEntry) || !eaitaskEntry.action.Continue())
			{
				this.executingTasks.Remove(eaitaskEntry);
				eaitaskEntry.isExecuting = false;
				eaitaskEntry.executeTime = eaitaskEntry.action.executeDelay * this.executeDelayScale;
				eaitaskEntry.action.Reset();
				goto IL_007A;
			}
			IL_010D:
			i++;
			continue;
			IL_007A:
			eaitaskEntry.executeTime -= 0.05f;
			eaitaskEntry.action.executeWaitTime += 0.05f;
			if (eaitaskEntry.executeTime > 0f)
			{
				goto IL_010D;
			}
			eaitaskEntry.executeTime = eaitaskEntry.action.executeDelay * this.executeDelayScale;
			if (this.isBestTask(eaitaskEntry))
			{
				if (eaitaskEntry.action.CanExecute())
				{
					this.startedTasks.Add(eaitaskEntry);
					this.executingTasks.Add(eaitaskEntry);
					eaitaskEntry.isExecuting = true;
				}
				eaitaskEntry.action.executeWaitTime = 0f;
				goto IL_010D;
			}
			goto IL_010D;
		}
		for (int j = 0; j < this.startedTasks.Count; j++)
		{
			this.startedTasks[j].action.Start();
		}
		for (int k = 0; k < this.executingTasks.Count; k++)
		{
			this.executingTasks[k].action.Update();
		}
	}

	// Token: 0x060021EE RID: 8686 RVA: 0x000CDB50 File Offset: 0x000CBD50
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isBestTask(EAITaskEntry _task)
	{
		int i = 0;
		while (i < this.executingTasks.Count)
		{
			EAITaskEntry eaitaskEntry = this.executingTasks[i++];
			if (eaitaskEntry != _task)
			{
				if (eaitaskEntry.priority > _task.priority)
				{
					if (eaitaskEntry.action.IsContinuous())
					{
						continue;
					}
				}
				else if (this.areTasksCompatible(_task, eaitaskEntry))
				{
					continue;
				}
				return false;
			}
		}
		return true;
	}

	// Token: 0x060021EF RID: 8687 RVA: 0x000CDBAC File Offset: 0x000CBDAC
	[PublicizedFrom(EAccessModifier.Private)]
	public bool areTasksCompatible(EAITaskEntry _task, EAITaskEntry _other)
	{
		return (_task.action.MutexBits & _other.action.MutexBits) == 0;
	}

	// Token: 0x060021F0 RID: 8688 RVA: 0x000CDBC8 File Offset: 0x000CBDC8
	public void StopAllTasks()
	{
		this.allTasks.Clear();
		this.executingTasks.Clear();
		this.startedTasks.Clear();
	}

	// Token: 0x0400177F RID: 6015
	[PublicizedFrom(EAccessModifier.Private)]
	public List<EAITaskEntry> allTasks;

	// Token: 0x04001780 RID: 6016
	[PublicizedFrom(EAccessModifier.Private)]
	public List<EAITaskEntry> executingTasks;

	// Token: 0x04001781 RID: 6017
	[PublicizedFrom(EAccessModifier.Private)]
	public List<EAITaskEntry> startedTasks = new List<EAITaskEntry>();

	// Token: 0x04001782 RID: 6018
	[PublicizedFrom(EAccessModifier.Private)]
	public float executeDelayScale;
}
