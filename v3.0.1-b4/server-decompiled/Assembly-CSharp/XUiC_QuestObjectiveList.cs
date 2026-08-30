using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000FCC RID: 4044
[Preserve]
public class XUiC_QuestObjectiveList : XUiController
{
	// Token: 0x06007F7A RID: 32634 RVA: 0x003059FC File Offset: 0x00303BFC
	public void SetIsTracker()
	{
		this.isTracker = true;
		for (int i = 0; i < this.objectiveEntries.Count; i++)
		{
			((XUiC_QuestObjectiveEntry)this.objectiveEntries[i]).SetIsTracker();
		}
	}

	// Token: 0x17000EB9 RID: 3769
	// (get) Token: 0x06007F7B RID: 32635 RVA: 0x00305A3C File Offset: 0x00303C3C
	// (set) Token: 0x06007F7C RID: 32636 RVA: 0x00305A44 File Offset: 0x00303C44
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

	// Token: 0x06007F7D RID: 32637 RVA: 0x00305A54 File Offset: 0x00303C54
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

	// Token: 0x06007F7E RID: 32638 RVA: 0x00292E8D File Offset: 0x0029108D
	public override void OnOpen()
	{
		base.OnOpen();
		this.IsDirty = true;
	}

	// Token: 0x06007F7F RID: 32639 RVA: 0x00305A94 File Offset: 0x00303C94
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

	// Token: 0x04005E57 RID: 24151
	[PublicizedFrom(EAccessModifier.Private)]
	public Quest quest;

	// Token: 0x04005E58 RID: 24152
	[PublicizedFrom(EAccessModifier.Private)]
	public List<XUiController> objectiveEntries = new List<XUiController>();

	// Token: 0x04005E59 RID: 24153
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isTracker;

	// Token: 0x04005E5A RID: 24154
	public string completeHexColor = "FF00FF00";

	// Token: 0x04005E5B RID: 24155
	public string incompleteHexColor = "FFFF0000";

	// Token: 0x04005E5C RID: 24156
	public string warningHexColor = "FFFF00FF";

	// Token: 0x04005E5D RID: 24157
	public string inactiveHexColor = "888888FF";

	// Token: 0x04005E5E RID: 24158
	public string activeHexColor = "FFFFFFFF";
}
