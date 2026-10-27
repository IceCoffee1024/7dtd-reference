using System;
using System.Collections.Generic;
using Challenges;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02001014 RID: 4116
[Preserve]
public class XUiC_QuestTrackerObjectiveList : XUiController
{
	// Token: 0x17000F3C RID: 3900
	// (get) Token: 0x0600814A RID: 33098 RVA: 0x00311EDB File Offset: 0x003100DB
	// (set) Token: 0x0600814B RID: 33099 RVA: 0x00311EE3 File Offset: 0x003100E3
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

	// Token: 0x17000F3D RID: 3901
	// (get) Token: 0x0600814C RID: 33100 RVA: 0x00311EF3 File Offset: 0x003100F3
	// (set) Token: 0x0600814D RID: 33101 RVA: 0x00311EFC File Offset: 0x003100FC
	public Challenge Challenge
	{
		get
		{
			return this.challenge;
		}
		set
		{
			if (this.challenge != null)
			{
				this.challenge.OnChallengeStateChanged -= this.CurrentChallenge_OnChallengeStateChanged;
			}
			this.challenge = value;
			if (this.challenge != null)
			{
				this.challenge.OnChallengeStateChanged += this.CurrentChallenge_OnChallengeStateChanged;
			}
			this.IsDirty = true;
		}
	}

	// Token: 0x0600814E RID: 33102 RVA: 0x0008ABC9 File Offset: 0x00088DC9
	[PublicizedFrom(EAccessModifier.Private)]
	public void CurrentChallenge_OnChallengeStateChanged(Challenge challenge)
	{
		this.IsDirty = true;
	}

	// Token: 0x0600814F RID: 33103 RVA: 0x00311F58 File Offset: 0x00310158
	public override void Init()
	{
		base.Init();
		XUiController[] childrenByType = base.GetChildrenByType<XUiC_QuestTrackerObjectiveEntry>(null);
		XUiController[] array = childrenByType;
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i] != null)
			{
				this.objectiveEntries.Add(array[i]);
			}
		}
	}

	// Token: 0x06008150 RID: 33104 RVA: 0x00311F98 File Offset: 0x00310198
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
					if ((this.quest.Objectives[i].Phase == this.quest.CurrentPhase || this.quest.Objectives[i].Phase == 0) && !this.quest.Objectives[i].HiddenObjective)
					{
						if (num < count)
						{
							XUiC_QuestTrackerObjectiveEntry xuiC_QuestTrackerObjectiveEntry = this.objectiveEntries[num] as XUiC_QuestTrackerObjectiveEntry;
							if (xuiC_QuestTrackerObjectiveEntry != null)
							{
								xuiC_QuestTrackerObjectiveEntry.Owner = this;
								if (i < count2)
								{
									xuiC_QuestTrackerObjectiveEntry.QuestObjective = this.quest.Objectives[i];
								}
								else
								{
									xuiC_QuestTrackerObjectiveEntry.ClearObjective();
								}
							}
						}
						num++;
					}
				}
				if (num < count)
				{
					for (int j = num; j < count; j++)
					{
						XUiC_QuestTrackerObjectiveEntry xuiC_QuestTrackerObjectiveEntry2 = this.objectiveEntries[j] as XUiC_QuestTrackerObjectiveEntry;
						if (xuiC_QuestTrackerObjectiveEntry2 != null)
						{
							xuiC_QuestTrackerObjectiveEntry2.ClearObjective();
						}
					}
				}
			}
			else if (this.challenge != null)
			{
				List<BaseChallengeObjective> objectiveList = this.challenge.GetObjectiveList();
				int count3 = this.objectiveEntries.Count;
				int count4 = objectiveList.Count;
				int num2 = 0;
				for (int k = 0; k < count4; k++)
				{
					if (num2 < count3)
					{
						XUiC_QuestTrackerObjectiveEntry xuiC_QuestTrackerObjectiveEntry3 = this.objectiveEntries[num2] as XUiC_QuestTrackerObjectiveEntry;
						if (xuiC_QuestTrackerObjectiveEntry3 != null)
						{
							xuiC_QuestTrackerObjectiveEntry3.Owner = this;
							if (k < count4)
							{
								xuiC_QuestTrackerObjectiveEntry3.ChallengeObjective = objectiveList[k];
							}
							else
							{
								xuiC_QuestTrackerObjectiveEntry3.ClearObjective();
							}
						}
					}
					num2++;
				}
				if (num2 < count3)
				{
					for (int l = num2; l < count3; l++)
					{
						XUiC_QuestTrackerObjectiveEntry xuiC_QuestTrackerObjectiveEntry4 = this.objectiveEntries[l] as XUiC_QuestTrackerObjectiveEntry;
						if (xuiC_QuestTrackerObjectiveEntry4 != null)
						{
							xuiC_QuestTrackerObjectiveEntry4.ClearObjective();
						}
					}
				}
			}
			else
			{
				int count5 = this.objectiveEntries.Count;
				for (int m = 0; m < count5; m++)
				{
					XUiC_QuestTrackerObjectiveEntry xuiC_QuestTrackerObjectiveEntry5 = this.objectiveEntries[m] as XUiC_QuestTrackerObjectiveEntry;
					if (xuiC_QuestTrackerObjectiveEntry5 != null)
					{
						xuiC_QuestTrackerObjectiveEntry5.Owner = this;
						xuiC_QuestTrackerObjectiveEntry5.ClearObjective();
					}
				}
			}
			this.IsDirty = false;
		}
		base.Update(_dt);
	}

	// Token: 0x06008151 RID: 33105 RVA: 0x003121E0 File Offset: 0x003103E0
	public override bool ParseAttribute(string name, string value)
	{
		if (name == "complete_icon")
		{
			this.completeIconName = value;
			return true;
		}
		if (name == "incomplete_icon")
		{
			this.incompleteIconName = value;
			return true;
		}
		if (name == "complete_color")
		{
			Color32 color = StringParsers.ParseColor(value);
			this.completeColor = string.Format("{0},{1},{2},{3}", new object[] { color.r, color.g, color.b, color.a });
			this.completeHexColor = Utils.ColorToHex(color);
			return true;
		}
		if (!(name == "incomplete_color"))
		{
			return base.ParseAttribute(name, value);
		}
		Color32 color2 = StringParsers.ParseColor(value);
		this.incompleteColor = string.Format("{0},{1},{2},{3}", new object[] { color2.r, color2.g, color2.b, color2.a });
		this.incompleteHexColor = Utils.ColorToHex(color2);
		return true;
	}

	// Token: 0x04005FEC RID: 24556
	[PublicizedFrom(EAccessModifier.Private)]
	public Quest quest;

	// Token: 0x04005FED RID: 24557
	[PublicizedFrom(EAccessModifier.Private)]
	public Challenge challenge;

	// Token: 0x04005FEE RID: 24558
	[PublicizedFrom(EAccessModifier.Private)]
	public List<XUiController> objectiveEntries = new List<XUiController>();

	// Token: 0x04005FEF RID: 24559
	public string completeIconName = "";

	// Token: 0x04005FF0 RID: 24560
	public string incompleteIconName = "";

	// Token: 0x04005FF1 RID: 24561
	public string completeHexColor = "FF00FF00";

	// Token: 0x04005FF2 RID: 24562
	public string incompleteHexColor = "FFB400";

	// Token: 0x04005FF3 RID: 24563
	public string warningHexColor = "FFFF00FF";

	// Token: 0x04005FF4 RID: 24564
	public string inactiveHexColor = "888888FF";

	// Token: 0x04005FF5 RID: 24565
	public string activeHexColor = "FFFFFFFF";

	// Token: 0x04005FF6 RID: 24566
	public string completeColor = "0,255,0,255";

	// Token: 0x04005FF7 RID: 24567
	public string incompleteColor = "255, 180, 0, 255";

	// Token: 0x04005FF8 RID: 24568
	public string warningColor = "255,255,0,255";
}
