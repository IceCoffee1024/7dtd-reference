using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020009BC RID: 2492
[Preserve]
public class ObjectiveRepair : BaseObjective
{
	// Token: 0x170007E4 RID: 2020
	// (get) Token: 0x060048DD RID: 18653 RVA: 0x00020119 File Offset: 0x0001E319
	public override BaseObjective.ObjectiveValueTypes ObjectiveValueType
	{
		get
		{
			return BaseObjective.ObjectiveValueTypes.Number;
		}
	}

	// Token: 0x060048DE RID: 18654 RVA: 0x001CA3E0 File Offset: 0x001C85E0
	public override void SetupObjective()
	{
		this.keyword = Localization.Get("ObjectiveRepair_keyword", false, null);
		this.expectedItem = ItemClass.GetItem(this.ID, false);
		this.expectedItemClass = ItemClass.GetItemClass(this.ID, false);
		this.repairCount = Convert.ToInt32(this.Value);
	}

	// Token: 0x060048DF RID: 18655 RVA: 0x001CA434 File Offset: 0x001C8634
	public override void SetupDisplay()
	{
		base.Description = string.Format(this.keyword, this.expectedItemClass.GetLocalizedItemName());
		this.StatusText = string.Format("{0}/{1}", base.CurrentValue, this.repairCount);
	}

	// Token: 0x060048E0 RID: 18656 RVA: 0x001CA483 File Offset: 0x001C8683
	public override void AddHooks()
	{
		QuestEventManager.Current.RepairItem += this.Current_RepairItem;
	}

	// Token: 0x060048E1 RID: 18657 RVA: 0x001CA49B File Offset: 0x001C869B
	public override void RemoveHooks()
	{
		QuestEventManager.Current.RepairItem -= this.Current_RepairItem;
	}

	// Token: 0x060048E2 RID: 18658 RVA: 0x001CA4B4 File Offset: 0x001C86B4
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

	// Token: 0x060048E3 RID: 18659 RVA: 0x001CA501 File Offset: 0x001C8701
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

	// Token: 0x060048E4 RID: 18660 RVA: 0x001CA53C File Offset: 0x001C873C
	public override BaseObjective Clone()
	{
		ObjectiveRepair objectiveRepair = new ObjectiveRepair();
		this.CopyValues(objectiveRepair);
		return objectiveRepair;
	}

	// Token: 0x060048E5 RID: 18661 RVA: 0x001CA557 File Offset: 0x001C8757
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void InternalToParametersDictionary(ref Dictionary<string, object> parameters)
	{
		parameters.Add("expected_item_id", this.expectedItem.ItemClass.Name);
		parameters.Add("count", this.repairCount);
	}

	// Token: 0x04003A3A RID: 14906
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemValue expectedItem = ItemValue.None;

	// Token: 0x04003A3B RID: 14907
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemClass expectedItemClass;

	// Token: 0x04003A3C RID: 14908
	[PublicizedFrom(EAccessModifier.Private)]
	public int repairCount;
}
