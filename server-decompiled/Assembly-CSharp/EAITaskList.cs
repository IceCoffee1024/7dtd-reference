using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000457 RID: 1111
[Preserve]
public class EAITaskList
{
	// Token: 0x060021D5 RID: 8661 RVA: 0x000CCC9C File Offset: 0x000CAE9C
	public EAITaskList(EAIManager _manager)
	{
		this.allTasks = new List<EAITaskEntry>();
		this.executingTasks = new List<EAITaskEntry>();
		this.executeDelayScale = 0.85f + _manager.random.RandomFloat * 0.25f;
	}

	// Token: 0x060021D6 RID: 8662 RVA: 0x000CCCED File Offset: 0x000CAEED
	public void AddTask(int _priority, EAIBase _eai)
	{
		this.allTasks.Add(new EAITaskEntry(_priority, _eai));
	}

	// Token: 0x060021D7 RID: 8663 RVA: 0x000CCD04 File Offset: 0x000CAF04
	public void AddTaskList(EAITaskList _taskList)
	{
		for (int i = 0; i < _taskList.Tasks.Count; i++)
		{
			this.allTasks.Add(_taskList.Tasks[i]);
		}
	}

	// Token: 0x060021D8 RID: 8664 RVA: 0x000CCD3E File Offset: 0x000CAF3E
	public void RemoveTask(EAITaskEntry _entry)
	{
		this.allTasks.Remove(_entry);
		if (this.executingTasks.Contains(_entry))
		{
			this.executingTasks.Remove(_entry);
		}
	}

	// Token: 0x170003E4 RID: 996
	// (get) Token: 0x060021D9 RID: 8665 RVA: 0x000CCD68 File Offset: 0x000CAF68
	public List<EAITaskEntry> Tasks
	{
		get
		{
			return this.allTasks;
		}
	}

	// Token: 0x060021DA RID: 8666 RVA: 0x000CCD70 File Offset: 0x000CAF70
	public List<EAITaskEntry> GetExecutingTasks()
	{
		return this.executingTasks;
	}

	// Token: 0x060021DB RID: 8667 RVA: 0x000CCD78 File Offset: 0x000CAF78
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

	// Token: 0x060021DC RID: 8668 RVA: 0x000CCDCC File Offset: 0x000CAFCC
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

	// Token: 0x060021DD RID: 8669 RVA: 0x000CCF54 File Offset: 0x000CB154
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

	// Token: 0x060021DE RID: 8670 RVA: 0x000CCFB0 File Offset: 0x000CB1B0
	[PublicizedFrom(EAccessModifier.Private)]
	public bool areTasksCompatible(EAITaskEntry _task, EAITaskEntry _other)
	{
		return (_task.action.MutexBits & _other.action.MutexBits) == 0;
	}

	// Token: 0x060021DF RID: 8671 RVA: 0x000CCFCC File Offset: 0x000CB1CC
	public void StopAllTasks()
	{
		this.allTasks.Clear();
		this.executingTasks.Clear();
		this.startedTasks.Clear();
	}

	// Token: 0x04001762 RID: 5986
	[PublicizedFrom(EAccessModifier.Private)]
	public List<EAITaskEntry> allTasks;

	// Token: 0x04001763 RID: 5987
	[PublicizedFrom(EAccessModifier.Private)]
	public List<EAITaskEntry> executingTasks;

	// Token: 0x04001764 RID: 5988
	[PublicizedFrom(EAccessModifier.Private)]
	public List<EAITaskEntry> startedTasks = new List<EAITaskEntry>();

	// Token: 0x04001765 RID: 5989
	[PublicizedFrom(EAccessModifier.Private)]
	public float executeDelayScale;
}
