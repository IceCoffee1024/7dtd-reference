using System;
using UnityEngine.Scripting;

// Token: 0x020009AF RID: 2479
[Preserve]
public class ObjectiveSpendSkillPoints : BaseObjective
{
	// Token: 0x170007C1 RID: 1985
	// (get) Token: 0x0600491A RID: 18714 RVA: 0x00020021 File Offset: 0x0001E221
	public override BaseObjective.ObjectiveValueTypes ObjectiveValueType
	{
		get
		{
			return BaseObjective.ObjectiveValueTypes.Number;
		}
	}

	// Token: 0x0600491B RID: 18715 RVA: 0x001C8DCA File Offset: 0x001C6FCA
	public override void SetupObjective()
	{
		this.keyword = Localization.Get("ObjectiveSpendSkillPoints_keyword", false, null);
		this.pointCount = Convert.ToInt32(this.Value);
	}

	// Token: 0x0600491C RID: 18716 RVA: 0x001C8DF0 File Offset: 0x001C6FF0
	public override void SetupDisplay()
	{
		base.Description = string.Format(this.keyword, (this.ID != null) ? this.ID : "Any");
		this.StatusText = string.Format("{0}/{1}", base.CurrentValue, this.pointCount);
	}

	// Token: 0x0600491D RID: 18717 RVA: 0x001C8E49 File Offset: 0x001C7049
	public override void AddHooks()
	{
		QuestEventManager.Current.SkillPointSpent += this.Current_SkillPointSpent;
	}

	// Token: 0x0600491E RID: 18718 RVA: 0x001C8E61 File Offset: 0x001C7061
	public override void RemoveHooks()
	{
		QuestEventManager.Current.SkillPointSpent -= this.Current_SkillPointSpent;
	}

	// Token: 0x0600491F RID: 18719 RVA: 0x001C8E7C File Offset: 0x001C707C
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

	// Token: 0x06004920 RID: 18720 RVA: 0x001C8ECC File Offset: 0x001C70CC
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

	// Token: 0x06004921 RID: 18721 RVA: 0x000027FC File Offset: 0x000009FC
	public override void RemoveObjectives()
	{
	}

	// Token: 0x06004922 RID: 18722 RVA: 0x001C8F24 File Offset: 0x001C7124
	public override BaseObjective Clone()
	{
		ObjectiveSpendSkillPoints objectiveSpendSkillPoints = new ObjectiveSpendSkillPoints();
		this.CopyValues(objectiveSpendSkillPoints);
		return objectiveSpendSkillPoints;
	}

	// Token: 0x04003A23 RID: 14883
	[PublicizedFrom(EAccessModifier.Private)]
	public int pointCount;
}
