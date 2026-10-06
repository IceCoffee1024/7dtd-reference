using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x0200045D RID: 1117
[Preserve]
public class EAITaskList
{
	// Token: 0x060021E1 RID: 8673 RVA: 0x000CD4EC File Offset: 0x000CB6EC
	public EAITaskList(EAIManager _manager)
	{
		this.allTasks = new List<EAITaskEntry>();
		this.executingTasks = new List<EAITaskEntry>();
		this.executeDelayScale = 0.85f + _manager.random.RandomFloat * 0.25f;
	}

	// Token: 0x060021E2 RID: 8674 RVA: 0x000CD53D File Offset: 0x000CB73D
	public void AddTask(int _priority, EAIBase _eai)
	{
		this.allTasks.Add(new EAITaskEntry(_priority, _eai));
	}

	// Token: 0x060021E3 RID: 8675 RVA: 0x000CD554 File Offset: 0x000CB754
	public void AddTaskList(EAITaskList _taskList)
	{
		for (int i = 0; i < _taskList.Tasks.Count; i++)
		{
			this.allTasks.Add(_taskList.Tasks[i]);
		}
	}

	// Token: 0x060021E4 RID: 8676 RVA: 0x000CD58E File Offset: 0x000CB78E
	public void RemoveTask(EAITaskEntry _entry)
	{
		this.allTasks.Remove(_entry);
		if (this.executingTasks.Contains(_entry))
		{
			this.executingTasks.Remove(_entry);
		}
	}

	// Token: 0x170003DC RID: 988
	// (get) Token: 0x060021E5 RID: 8677 RVA: 0x000CD5B8 File Offset: 0x000CB7B8
	public List<EAITaskEntry> Tasks
	{
		get
		{
			return this.allTasks;
		}
	}

	// Token: 0x060021E6 RID: 8678 RVA: 0x000CD5C0 File Offset: 0x000CB7C0
	public List<EAITaskEntry> GetExecutingTasks()
	{
		return this.executingTasks;
	}

	// Token: 0x060021E7 RID: 8679 RVA: 0x000CD5C8 File Offset: 0x000CB7C8
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

	// Token: 0x060021E8 RID: 8680 RVA: 0x000CD61C File Offset: 0x000CB81C
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

	// Token: 0x060021E9 RID: 8681 RVA: 0x000CD7A4 File Offset: 0x000CB9A4
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

	// Token: 0x060021EA RID: 8682 RVA: 0x000CD800 File Offset: 0x000CBA00
	[PublicizedFrom(EAccessModifier.Private)]
	public bool areTasksCompatible(EAITaskEntry _task, EAITaskEntry _other)
	{
		return (_task.action.MutexBits & _other.action.MutexBits) == 0;
	}

	// Token: 0x060021EB RID: 8683 RVA: 0x000CD81C File Offset: 0x000CBA1C
	public void StopAllTasks()
	{
		this.allTasks.Clear();
		this.executingTasks.Clear();
		this.startedTasks.Clear();
	}

	// Token: 0x0400176F RID: 5999
	[PublicizedFrom(EAccessModifier.Private)]
	public List<EAITaskEntry> allTasks;

	// Token: 0x04001770 RID: 6000
	[PublicizedFrom(EAccessModifier.Private)]
	public List<EAITaskEntry> executingTasks;

	// Token: 0x04001771 RID: 6001
	[PublicizedFrom(EAccessModifier.Private)]
	public List<EAITaskEntry> startedTasks = new List<EAITaskEntry>();

	// Token: 0x04001772 RID: 6002
	[PublicizedFrom(EAccessModifier.Private)]
	public float executeDelayScale;
}
