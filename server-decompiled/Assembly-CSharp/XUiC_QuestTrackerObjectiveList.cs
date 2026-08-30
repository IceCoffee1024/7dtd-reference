using System;
using System.Collections.Generic;
using Challenges;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000FF5 RID: 4085
[Preserve]
public class XUiC_QuestTrackerObjectiveList : XUiController
{
	// Token: 0x17000F0A RID: 3850
	// (get) Token: 0x060080EC RID: 33004 RVA: 0x0030D3D7 File Offset: 0x0030B5D7
	// (set) Token: 0x060080ED RID: 33005 RVA: 0x0030D3DF File Offset: 0x0030B5DF
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

	// Token: 0x17000F0B RID: 3851
	// (get) Token: 0x060080EE RID: 33006 RVA: 0x0030D3EF File Offset: 0x0030B5EF
	// (set) Token: 0x060080EF RID: 33007 RVA: 0x0030D3F8 File Offset: 0x0030B5F8
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

	// Token: 0x060080F0 RID: 33008 RVA: 0x0008A715 File Offset: 0x00088915
	[PublicizedFrom(EAccessModifier.Private)]
	public void CurrentChallenge_OnChallengeStateChanged(Challenge challenge)
	{
		this.IsDirty = true;
	}

	// Token: 0x060080F1 RID: 33009 RVA: 0x0030D454 File Offset: 0x0030B654
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

	// Token: 0x060080F2 RID: 33010 RVA: 0x0030D494 File Offset: 0x0030B694
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

	// Token: 0x060080F3 RID: 33011 RVA: 0x0030D6DC File Offset: 0x0030B8DC
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

	// Token: 0x04005F88 RID: 24456
	[PublicizedFrom(EAccessModifier.Private)]
	public Quest quest;

	// Token: 0x04005F89 RID: 24457
	[PublicizedFrom(EAccessModifier.Private)]
	public Challenge challenge;

	// Token: 0x04005F8A RID: 24458
	[PublicizedFrom(EAccessModifier.Private)]
	public List<XUiController> objectiveEntries = new List<XUiController>();

	// Token: 0x04005F8B RID: 24459
	public string completeIconName = "";

	// Token: 0x04005F8C RID: 24460
	public string incompleteIconName = "";

	// Token: 0x04005F8D RID: 24461
	public string completeHexColor = "FF00FF00";

	// Token: 0x04005F8E RID: 24462
	public string incompleteHexColor = "FFB400";

	// Token: 0x04005F8F RID: 24463
	public string warningHexColor = "FFFF00FF";

	// Token: 0x04005F90 RID: 24464
	public string inactiveHexColor = "888888FF";

	// Token: 0x04005F91 RID: 24465
	public string activeHexColor = "FFFFFFFF";

	// Token: 0x04005F92 RID: 24466
	public string completeColor = "0,255,0,255";

	// Token: 0x04005F93 RID: 24467
	public string incompleteColor = "255, 180, 0, 255";

	// Token: 0x04005F94 RID: 24468
	public string warningColor = "255,255,0,255";
}
