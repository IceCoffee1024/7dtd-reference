using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020009BF RID: 2495
[Preserve]
public class ObjectiveSpendSkillPoints : BaseObjective
{
	// Token: 0x170007E7 RID: 2023
	// (get) Token: 0x060048F8 RID: 18680 RVA: 0x00020119 File Offset: 0x0001E319
	public override BaseObjective.ObjectiveValueTypes ObjectiveValueType
	{
		get
		{
			return BaseObjective.ObjectiveValueTypes.Number;
		}
	}

	// Token: 0x060048F9 RID: 18681 RVA: 0x001CA89B File Offset: 0x001C8A9B
	public override void SetupObjective()
	{
		this.keyword = Localization.Get("ObjectiveSpendSkillPoints_keyword", false, null);
		this.pointCount = Convert.ToInt32(this.Value);
	}

	// Token: 0x060048FA RID: 18682 RVA: 0x001CA8C0 File Offset: 0x001C8AC0
	public override void SetupDisplay()
	{
		base.Description = string.Format(this.keyword, (this.ID != null) ? this.ID : "Any");
		this.StatusText = string.Format("{0}/{1}", base.CurrentValue, this.pointCount);
	}

	// Token: 0x060048FB RID: 18683 RVA: 0x001CA919 File Offset: 0x001C8B19
	public override void AddHooks()
	{
		QuestEventManager.Current.SkillPointSpent += this.Current_SkillPointSpent;
	}

	// Token: 0x060048FC RID: 18684 RVA: 0x001CA931 File Offset: 0x001C8B31
	public override void RemoveHooks()
	{
		QuestEventManager.Current.SkillPointSpent -= this.Current_SkillPointSpent;
	}

	// Token: 0x060048FD RID: 18685 RVA: 0x001CA94C File Offset: 0x001C8B4C
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

	// Token: 0x060048FE RID: 18686 RVA: 0x001CA99C File Offset: 0x001C8B9C
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

	// Token: 0x060048FF RID: 18687 RVA: 0x000027FC File Offset: 0x000009FC
	public override void RemoveObjectives()
	{
	}

	// Token: 0x06004900 RID: 18688 RVA: 0x001CA9F4 File Offset: 0x001C8BF4
	public override BaseObjective Clone()
	{
		ObjectiveSpendSkillPoints objectiveSpendSkillPoints = new ObjectiveSpendSkillPoints();
		this.CopyValues(objectiveSpendSkillPoints);
		return objectiveSpendSkillPoints;
	}

	// Token: 0x06004901 RID: 18689 RVA: 0x001CAA0F File Offset: 0x001C8C0F
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void InternalToParametersDictionary(ref Dictionary<string, object> parameters)
	{
		parameters.Add("count", this.pointCount);
	}

	// Token: 0x04003A40 RID: 14912
	[PublicizedFrom(EAccessModifier.Private)]
	public int pointCount;
}
