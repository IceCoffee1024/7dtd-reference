using System;
using UnityEngine.Scripting;

// Token: 0x0200099E RID: 2462
[Preserve]
public class ObjectiveSpendSkillPoints : BaseObjective
{
	// Token: 0x170007AF RID: 1967
	// (get) Token: 0x06004894 RID: 18580 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override BaseObjective.ObjectiveValueTypes ObjectiveValueType
	{
		get
		{
			return BaseObjective.ObjectiveValueTypes.Number;
		}
	}

	// Token: 0x06004895 RID: 18581 RVA: 0x001C65D6 File Offset: 0x001C47D6
	public override void SetupObjective()
	{
		this.keyword = Localization.Get("ObjectiveSpendSkillPoints_keyword", false, null);
		this.pointCount = Convert.ToInt32(this.Value);
	}

	// Token: 0x06004896 RID: 18582 RVA: 0x001C65FC File Offset: 0x001C47FC
	public override void SetupDisplay()
	{
		base.Description = string.Format(this.keyword, (this.ID != null) ? this.ID : "Any");
		this.StatusText = string.Format("{0}/{1}", base.CurrentValue, this.pointCount);
	}

	// Token: 0x06004897 RID: 18583 RVA: 0x001C6655 File Offset: 0x001C4855
	public override void AddHooks()
	{
		QuestEventManager.Current.SkillPointSpent += this.Current_SkillPointSpent;
	}

	// Token: 0x06004898 RID: 18584 RVA: 0x001C666D File Offset: 0x001C486D
	public override void RemoveHooks()
	{
		QuestEventManager.Current.SkillPointSpent -= this.Current_SkillPointSpent;
	}

	// Token: 0x06004899 RID: 18585 RVA: 0x001C6688 File Offset: 0x001C4888
	[PublicizedFrom(EAccessModifier.Private)]
	public void Current_SkillPointSpent(string skillName)
	{
		if (base.Complete)
		{
			return;
		}
		if ((this.ID == null || skillName.EqualsCaseInsensitive(this.ID)) && base.OwnerQuest.CheckRequirements())
		{
			byte currentValue = base.CurrentValue;
			base.CurrentValue = currentValue + 1;
		}
		this.Refresh();
	}

	// Token: 0x0600489A RID: 18586 RVA: 0x001C66D8 File Offset: 0x001C48D8
	public override void Refresh()
	{
		if ((int)base.CurrentValue > this.pointCount)
		{
			base.CurrentValue = (byte)this.pointCount;
		}
		base.Complete = (int)base.CurrentValue >= this.pointCount;
		if (base.Complete)
		{
			base.OwnerQuest.RefreshQuestCompletion(QuestClass.CompletionTypes.AutoComplete, null, true, null);
		}
	}

	// Token: 0x0600489B RID: 18587 RVA: 0x000027FC File Offset: 0x000009FC
	public override void RemoveObjectives()
	{
	}

	// Token: 0x0600489C RID: 18588 RVA: 0x001C6730 File Offset: 0x001C4930
	public override BaseObjective Clone()
	{
		ObjectiveSpendSkillPoints objectiveSpendSkillPoints = new ObjectiveSpendSkillPoints();
		this.CopyValues(objectiveSpendSkillPoints);
		return objectiveSpendSkillPoints;
	}

	// Token: 0x04003992 RID: 14738
	[PublicizedFrom(EAccessModifier.Private)]
	public int pointCount;
}
