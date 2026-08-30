using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000FE5 RID: 4069
[Preserve]
public class XUiC_QuestObjectiveList : XUiController
{
	// Token: 0x0600806C RID: 32876 RVA: 0x0030A1D0 File Offset: 0x003083D0
	public void SetIsTracker()
	{
		this.isTracker = true;
		for (int i = 0; i < this.objectiveEntries.Count; i++)
		{
			((XUiC_QuestObjectiveEntry)this.objectiveEntries[i]).SetIsTracker();
		}
	}

	// Token: 0x17000EF2 RID: 3826
	// (get) Token: 0x0600806D RID: 32877 RVA: 0x0030A210 File Offset: 0x00308410
	// (set) Token: 0x0600806E RID: 32878 RVA: 0x0030A218 File Offset: 0x00308418
	public Quest Quest
	{
		get
		{
			return this.quest;
		}
		set
		{
			this.quest = value;
			this.IsDirty = true;
		}
	}

	// Token: 0x0600806F RID: 32879 RVA: 0x0030A228 File Offset: 0x00308428
	public override void Init()
	{
		base.Init();
		XUiController[] childrenByType = base.GetChildrenByType<XUiC_QuestObjectiveEntry>(null);
		XUiController[] array = childrenByType;
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i] != null)
			{
				this.objectiveEntries.Add(array[i]);
			}
		}
	}

	// Token: 0x06008070 RID: 32880 RVA: 0x00295FE1 File Offset: 0x002941E1
	public override void OnOpen()
	{
		base.OnOpen();
		this.IsDirty = true;
	}

	// Token: 0x06008071 RID: 32881 RVA: 0x0030A268 File Offset: 0x00308468
	public override void Update(float _dt)
	{
		if (this.IsDirty)
		{
			if (this.quest != null)
			{
				int count = this.objectiveEntries.Count;
				int count2 = this.quest.Objectives.Count;
				int num = 0;
				for (int i = 0; i < count2; i++)
				{
					if (this.quest.Objectives[i].Phase <= this.quest.CurrentPhase && !this.quest.Objectives[i].HiddenObjective && this.quest.Objectives[i].ShowInQuestLog)
					{
						if (this.objectiveEntries[num] is XUiC_QuestObjectiveEntry)
						{
							((XUiC_QuestObjectiveEntry)this.objectiveEntries[num]).Owner = this;
							if (i < count2)
							{
								((XUiC_QuestObjectiveEntry)this.objectiveEntries[num]).Objective = this.quest.Objectives[i];
							}
							else
							{
								((XUiC_QuestObjectiveEntry)this.objectiveEntries[num]).Objective = null;
							}
						}
						num++;
					}
				}
				if (num < count)
				{
					for (int j = num; j < count; j++)
					{
						if (this.objectiveEntries[j] is XUiC_QuestObjectiveEntry)
						{
							((XUiC_QuestObjectiveEntry)this.objectiveEntries[j]).Objective = null;
						}
					}
				}
			}
			else
			{
				int count3 = this.objectiveEntries.Count;
				for (int k = 0; k < count3; k++)
				{
					if (this.objectiveEntries[k] is XUiC_QuestObjectiveEntry)
					{
						((XUiC_QuestObjectiveEntry)this.objectiveEntries[k]).Owner = this;
						((XUiC_QuestObjectiveEntry)this.objectiveEntries[k]).Objective = null;
					}
				}
			}
			this.IsDirty = false;
		}
		base.Update(_dt);
	}

	// Token: 0x04005F1E RID: 24350
	[PublicizedFrom(EAccessModifier.Private)]
	public Quest quest;

	// Token: 0x04005F1F RID: 24351
	[PublicizedFrom(EAccessModifier.Private)]
	public List<XUiController> objectiveEntries = new List<XUiController>();

	// Token: 0x04005F20 RID: 24352
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isTracker;

	// Token: 0x04005F21 RID: 24353
	public string completeHexColor = "FF00FF00";

	// Token: 0x04005F22 RID: 24354
	public string incompleteHexColor = "FFFF0000";

	// Token: 0x04005F23 RID: 24355
	public string warningHexColor = "FFFF00FF";

	// Token: 0x04005F24 RID: 24356
	public string inactiveHexColor = "888888FF";

	// Token: 0x04005F25 RID: 24357
	public string activeHexColor = "FFFFFFFF";
}
