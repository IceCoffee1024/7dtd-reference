using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020009B2 RID: 2482
[Preserve]
public class ObjectiveRepair : BaseObjective
{
	// Token: 0x170007DE RID: 2014
	// (get) Token: 0x060048BA RID: 18618 RVA: 0x00020119 File Offset: 0x0001E319
	public override BaseObjective.ObjectiveValueTypes ObjectiveValueType
	{
		get
		{
			return BaseObjective.ObjectiveValueTypes.Number;
		}
	}

	// Token: 0x060048BB RID: 18619 RVA: 0x001C930C File Offset: 0x001C750C
	public override void SetupObjective()
	{
		this.keyword = Localization.Get("ObjectiveRepair_keyword", false, null);
		this.expectedItem = ItemClass.GetItem(this.ID, false);
		this.expectedItemClass = ItemClass.GetItemClass(this.ID, false);
		this.repairCount = Convert.ToInt32(this.Value);
	}

	// Token: 0x060048BC RID: 18620 RVA: 0x001C9360 File Offset: 0x001C7560
	public override void SetupDisplay()
	{
		base.Description = string.Format(this.keyword, this.expectedItemClass.GetLocalizedItemName());
		this.StatusText = string.Format("{0}/{1}", base.CurrentValue, this.repairCount);
	}

	// Token: 0x060048BD RID: 18621 RVA: 0x001C93AF File Offset: 0x001C75AF
	public override void AddHooks()
	{
		QuestEventManager.Current.RepairItem += this.Current_RepairItem;
	}

	// Token: 0x060048BE RID: 18622 RVA: 0x001C93C7 File Offset: 0x001C75C7
	public override void RemoveHooks()
	{
		QuestEventManager.Current.RepairItem -= this.Current_RepairItem;
	}

	// Token: 0x060048BF RID: 18623 RVA: 0x001C93E0 File Offset: 0x001C75E0
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

	// Token: 0x060048C0 RID: 18624 RVA: 0x001C942D File Offset: 0x001C762D
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

	// Token: 0x060048C1 RID: 18625 RVA: 0x001C9468 File Offset: 0x001C7668
	public override BaseObjective Clone()
	{
		ObjectiveRepair objectiveRepair = new ObjectiveRepair();
		this.CopyValues(objectiveRepair);
		return objectiveRepair;
	}

	// Token: 0x060048C2 RID: 18626 RVA: 0x001C9483 File Offset: 0x001C7683
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void InternalToParametersDictionary(ref Dictionary<string, object> parameters)
	{
		parameters.Add("expected_item_id", this.expectedItem.ItemClass.Name);
		parameters.Add("count", this.repairCount);
	}

	// Token: 0x04003A1A RID: 14874
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemValue expectedItem = ItemValue.None;

	// Token: 0x04003A1B RID: 14875
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemClass expectedItemClass;

	// Token: 0x04003A1C RID: 14876
	[PublicizedFrom(EAccessModifier.Private)]
	public int repairCount;
}
