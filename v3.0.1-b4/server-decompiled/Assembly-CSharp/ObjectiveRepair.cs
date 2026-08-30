using System;
using UnityEngine.Scripting;

// Token: 0x0200099B RID: 2459
[Preserve]
public class ObjectiveRepair : BaseObjective
{
	// Token: 0x170007AC RID: 1964
	// (get) Token: 0x0600487B RID: 18555 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override BaseObjective.ObjectiveValueTypes ObjectiveValueType
	{
		get
		{
			return BaseObjective.ObjectiveValueTypes.Number;
		}
	}

	// Token: 0x0600487C RID: 18556 RVA: 0x001C6188 File Offset: 0x001C4388
	public override void SetupObjective()
	{
		this.keyword = Localization.Get("ObjectiveRepair_keyword", false, null);
		this.expectedItem = ItemClass.GetItem(this.ID, false);
		this.expectedItemClass = ItemClass.GetItemClass(this.ID, false);
		this.repairCount = Convert.ToInt32(this.Value);
	}

	// Token: 0x0600487D RID: 18557 RVA: 0x001C61DC File Offset: 0x001C43DC
	public override void SetupDisplay()
	{
		base.Description = string.Format(this.keyword, this.expectedItemClass.GetLocalizedItemName());
		this.StatusText = string.Format("{0}/{1}", base.CurrentValue, this.repairCount);
	}

	// Token: 0x0600487E RID: 18558 RVA: 0x001C622B File Offset: 0x001C442B
	public override void AddHooks()
	{
		QuestEventManager.Current.RepairItem += this.Current_RepairItem;
	}

	// Token: 0x0600487F RID: 18559 RVA: 0x001C6243 File Offset: 0x001C4443
	public override void RemoveHooks()
	{
		QuestEventManager.Current.RepairItem -= this.Current_RepairItem;
	}

	// Token: 0x06004880 RID: 18560 RVA: 0x001C625C File Offset: 0x001C445C
	[PublicizedFrom(EAccessModifier.Private)]
	public void Current_RepairItem(ItemValue itemValue)
	{
		if (base.Complete)
		{
			return;
		}
		if (itemValue.type == this.expectedItem.type && base.OwnerQuest.CheckRequirements())
		{
			byte currentValue = base.CurrentValue;
			base.CurrentValue = currentValue + 1;
			this.Refresh();
		}
	}

	// Token: 0x06004881 RID: 18561 RVA: 0x001C62A9 File Offset: 0x001C44A9
	public override void Refresh()
	{
		if (base.Complete)
		{
			return;
		}
		base.Complete = (int)base.CurrentValue >= this.repairCount;
		if (base.Complete)
		{
			base.OwnerQuest.RefreshQuestCompletion(QuestClass.CompletionTypes.AutoComplete, null, true, null);
		}
	}

	// Token: 0x06004882 RID: 18562 RVA: 0x001C62E4 File Offset: 0x001C44E4
	public override BaseObjective Clone()
	{
		ObjectiveRepair objectiveRepair = new ObjectiveRepair();
		this.CopyValues(objectiveRepair);
		return objectiveRepair;
	}

	// Token: 0x0400398C RID: 14732
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemValue expectedItem = ItemValue.None;

	// Token: 0x0400398D RID: 14733
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemClass expectedItemClass;

	// Token: 0x0400398E RID: 14734
	[PublicizedFrom(EAccessModifier.Private)]
	public int repairCount;
}
