using System;
using System.Collections.Generic;
using Challenges;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000FDC RID: 4060
[Preserve]
public class XUiC_QuestTrackerObjectiveList : XUiController
{
	// Token: 0x17000ED1 RID: 3793
	// (get) Token: 0x06007FFA RID: 32762 RVA: 0x00308BA3 File Offset: 0x00306DA3
	// (set) Token: 0x06007FFB RID: 32763 RVA: 0x00308BAB File Offset: 0x00306DAB
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

	// Token: 0x17000ED2 RID: 3794
	// (get) Token: 0x06007FFC RID: 32764 RVA: 0x00308BBB File Offset: 0x00306DBB
	// (set) Token: 0x06007FFD RID: 32765 RVA: 0x00308BC4 File Offset: 0x00306DC4
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

	// Token: 0x06007FFE RID: 32766 RVA: 0x0008A069 File Offset: 0x00088269
	[PublicizedFrom(EAccessModifier.Private)]
	public void CurrentChallenge_OnChallengeStateChanged(Challenge challenge)
	{
		this.IsDirty = true;
	}

	// Token: 0x06007FFF RID: 32767 RVA: 0x00308C20 File Offset: 0x00306E20
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

	// Token: 0x06008000 RID: 32768 RVA: 0x00308C60 File Offset: 0x00306E60
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

	// Token: 0x06008001 RID: 32769 RVA: 0x00308EA8 File Offset: 0x003070A8
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

	// Token: 0x04005EC1 RID: 24257
	[PublicizedFrom(EAccessModifier.Private)]
	public Quest quest;

	// Token: 0x04005EC2 RID: 24258
	[PublicizedFrom(EAccessModifier.Private)]
	public Challenge challenge;

	// Token: 0x04005EC3 RID: 24259
	[PublicizedFrom(EAccessModifier.Private)]
	public List<XUiController> objectiveEntries = new List<XUiController>();

	// Token: 0x04005EC4 RID: 24260
	public string completeIconName = "";

	// Token: 0x04005EC5 RID: 24261
	public string incompleteIconName = "";

	// Token: 0x04005EC6 RID: 24262
	public string completeHexColor = "FF00FF00";

	// Token: 0x04005EC7 RID: 24263
	public string incompleteHexColor = "FFB400";

	// Token: 0x04005EC8 RID: 24264
	public string warningHexColor = "FFFF00FF";

	// Token: 0x04005EC9 RID: 24265
	public string inactiveHexColor = "888888FF";

	// Token: 0x04005ECA RID: 24266
	public string activeHexColor = "FFFFFFFF";

	// Token: 0x04005ECB RID: 24267
	public string completeColor = "0,255,0,255";

	// Token: 0x04005ECC RID: 24268
	public string incompleteColor = "255, 180, 0, 255";

	// Token: 0x04005ECD RID: 24269
	public string warningColor = "255,255,0,255";
}
