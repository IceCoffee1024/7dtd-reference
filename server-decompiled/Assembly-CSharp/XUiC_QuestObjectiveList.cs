using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02001004 RID: 4100
[Preserve]
public class XUiC_QuestObjectiveList : XUiController
{
	// Token: 0x060080CA RID: 32970 RVA: 0x0030ECC4 File Offset: 0x0030CEC4
	public void SetIsTracker()
	{
		this.isTracker = true;
		for (int i = 0; i < this.objectiveEntries.Count; i++)
		{
			((XUiC_QuestObjectiveEntry)this.objectiveEntries[i]).SetIsTracker();
		}
	}

	// Token: 0x17000F24 RID: 3876
	// (get) Token: 0x060080CB RID: 32971 RVA: 0x0030ED04 File Offset: 0x0030CF04
	// (set) Token: 0x060080CC RID: 32972 RVA: 0x0030ED0C File Offset: 0x0030CF0C
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

	// Token: 0x060080CD RID: 32973 RVA: 0x0030ED1C File Offset: 0x0030CF1C
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

	// Token: 0x060080CE RID: 32974 RVA: 0x0029A895 File Offset: 0x00298A95
	public override void OnOpen()
	{
		base.OnOpen();
		this.IsDirty = true;
	}

	// Token: 0x060080CF RID: 32975 RVA: 0x0030ED5C File Offset: 0x0030CF5C
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

	// Token: 0x04005F82 RID: 24450
	[PublicizedFrom(EAccessModifier.Private)]
	public Quest quest;

	// Token: 0x04005F83 RID: 24451
	[PublicizedFrom(EAccessModifier.Private)]
	public List<XUiController> objectiveEntries = new List<XUiController>();

	// Token: 0x04005F84 RID: 24452
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isTracker;

	// Token: 0x04005F85 RID: 24453
	public string completeHexColor = "FF00FF00";

	// Token: 0x04005F86 RID: 24454
	public string incompleteHexColor = "FFFF0000";

	// Token: 0x04005F87 RID: 24455
	public string warningHexColor = "FFFF00FF";

	// Token: 0x04005F88 RID: 24456
	public string inactiveHexColor = "888888FF";

	// Token: 0x04005F89 RID: 24457
	public string activeHexColor = "FFFFFFFF";
}
