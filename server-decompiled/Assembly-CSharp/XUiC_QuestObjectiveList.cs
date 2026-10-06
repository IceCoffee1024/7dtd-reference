using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000FF4 RID: 4084
[Preserve]
public class XUiC_QuestObjectiveList : XUiController
{
	// Token: 0x0600807B RID: 32891 RVA: 0x0030D1A8 File Offset: 0x0030B3A8
	public void SetIsTracker()
	{
		this.isTracker = true;
		for (int i = 0; i < this.objectiveEntries.Count; i++)
		{
			((XUiC_QuestObjectiveEntry)this.objectiveEntries[i]).SetIsTracker();
		}
	}

	// Token: 0x17000F19 RID: 3865
	// (get) Token: 0x0600807C RID: 32892 RVA: 0x0030D1E8 File Offset: 0x0030B3E8
	// (set) Token: 0x0600807D RID: 32893 RVA: 0x0030D1F0 File Offset: 0x0030B3F0
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

	// Token: 0x0600807E RID: 32894 RVA: 0x0030D200 File Offset: 0x0030B400
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

	// Token: 0x0600807F RID: 32895 RVA: 0x0029936D File Offset: 0x0029756D
	public override void OnOpen()
	{
		base.OnOpen();
		this.IsDirty = true;
	}

	// Token: 0x06008080 RID: 32896 RVA: 0x0030D240 File Offset: 0x0030B440
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

	// Token: 0x04005F47 RID: 24391
	[PublicizedFrom(EAccessModifier.Private)]
	public Quest quest;

	// Token: 0x04005F48 RID: 24392
	[PublicizedFrom(EAccessModifier.Private)]
	public List<XUiController> objectiveEntries = new List<XUiController>();

	// Token: 0x04005F49 RID: 24393
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isTracker;

	// Token: 0x04005F4A RID: 24394
	public string completeHexColor = "FF00FF00";

	// Token: 0x04005F4B RID: 24395
	public string incompleteHexColor = "FFFF0000";

	// Token: 0x04005F4C RID: 24396
	public string warningHexColor = "FFFF00FF";

	// Token: 0x04005F4D RID: 24397
	public string inactiveHexColor = "888888FF";

	// Token: 0x04005F4E RID: 24398
	public string activeHexColor = "FFFFFFFF";
}
