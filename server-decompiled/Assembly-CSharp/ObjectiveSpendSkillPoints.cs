using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020009B5 RID: 2485
[Preserve]
public class ObjectiveSpendSkillPoints : BaseObjective
{
	// Token: 0x170007E1 RID: 2017
	// (get) Token: 0x060048D5 RID: 18645 RVA: 0x00020119 File Offset: 0x0001E319
	public override BaseObjective.ObjectiveValueTypes ObjectiveValueType
	{
		get
		{
			return BaseObjective.ObjectiveValueTypes.Number;
		}
	}

	// Token: 0x060048D6 RID: 18646 RVA: 0x001C97C7 File Offset: 0x001C79C7
	public override void SetupObjective()
	{
		this.keyword = Localization.Get("ObjectiveSpendSkillPoints_keyword", false, null);
		this.pointCount = Convert.ToInt32(this.Value);
	}

	// Token: 0x060048D7 RID: 18647 RVA: 0x001C97EC File Offset: 0x001C79EC
	public override void SetupDisplay()
	{
		base.Description = string.Format(this.keyword, (this.ID != null) ? this.ID : "Any");
		this.StatusText = string.Format("{0}/{1}", base.CurrentValue, this.pointCount);
	}

	// Token: 0x060048D8 RID: 18648 RVA: 0x001C9845 File Offset: 0x001C7A45
	public override void AddHooks()
	{
		QuestEventManager.Current.SkillPointSpent += this.Current_SkillPointSpent;
	}

	// Token: 0x060048D9 RID: 18649 RVA: 0x001C985D File Offset: 0x001C7A5D
	public override void RemoveHooks()
	{
		QuestEventManager.Current.SkillPointSpent -= this.Current_SkillPointSpent;
	}

	// Token: 0x060048DA RID: 18650 RVA: 0x001C9878 File Offset: 0x001C7A78
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

	// Token: 0x060048DB RID: 18651 RVA: 0x001C98C8 File Offset: 0x001C7AC8
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

	// Token: 0x060048DC RID: 18652 RVA: 0x000027FC File Offset: 0x000009FC
	public override void RemoveObjectives()
	{
	}

	// Token: 0x060048DD RID: 18653 RVA: 0x001C9920 File Offset: 0x001C7B20
	public override BaseObjective Clone()
	{
		ObjectiveSpendSkillPoints objectiveSpendSkillPoints = new ObjectiveSpendSkillPoints();
		this.CopyValues(objectiveSpendSkillPoints);
		return objectiveSpendSkillPoints;
	}

	// Token: 0x060048DE RID: 18654 RVA: 0x001C993B File Offset: 0x001C7B3B
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void InternalToParametersDictionary(ref Dictionary<string, object> parameters)
	{
		parameters.Add("count", this.pointCount);
	}

	// Token: 0x04003A20 RID: 14880
	[PublicizedFrom(EAccessModifier.Private)]
	public int pointCount;
}
