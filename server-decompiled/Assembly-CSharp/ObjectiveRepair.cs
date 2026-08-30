using System;
using UnityEngine.Scripting;

// Token: 0x020009AC RID: 2476
[Preserve]
public class ObjectiveRepair : BaseObjective
{
	// Token: 0x170007BE RID: 1982
	// (get) Token: 0x06004901 RID: 18689 RVA: 0x00020021 File Offset: 0x0001E221
	public override BaseObjective.ObjectiveValueTypes ObjectiveValueType
	{
		get
		{
			return BaseObjective.ObjectiveValueTypes.Number;
		}
	}

	// Token: 0x06004902 RID: 18690 RVA: 0x001C897C File Offset: 0x001C6B7C
	public override void SetupObjective()
	{
		this.keyword = Localization.Get("ObjectiveRepair_keyword", false, null);
		this.expectedItem = ItemClass.GetItem(this.ID, false);
		this.expectedItemClass = ItemClass.GetItemClass(this.ID, false);
		this.repairCount = Convert.ToInt32(this.Value);
	}

	// Token: 0x06004903 RID: 18691 RVA: 0x001C89D0 File Offset: 0x001C6BD0
	public override void SetupDisplay()
	{
		base.Description = string.Format(this.keyword, this.expectedItemClass.GetLocalizedItemName());
		this.StatusText = string.Format("{0}/{1}", base.CurrentValue, this.repairCount);
	}

	// Token: 0x06004904 RID: 18692 RVA: 0x001C8A1F File Offset: 0x001C6C1F
	public override void AddHooks()
	{
		QuestEventManager.Current.RepairItem += this.Current_RepairItem;
	}

	// Token: 0x06004905 RID: 18693 RVA: 0x001C8A37 File Offset: 0x001C6C37
	public override void RemoveHooks()
	{
		QuestEventManager.Current.RepairItem -= this.Current_RepairItem;
	}

	// Token: 0x06004906 RID: 18694 RVA: 0x001C8A50 File Offset: 0x001C6C50
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

	// Token: 0x06004907 RID: 18695 RVA: 0x001C8A9D File Offset: 0x001C6C9D
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

	// Token: 0x06004908 RID: 18696 RVA: 0x001C8AD8 File Offset: 0x001C6CD8
	public override BaseObjective Clone()
	{
		ObjectiveRepair objectiveRepair = new ObjectiveRepair();
		this.CopyValues(objectiveRepair);
		return objectiveRepair;
	}

	// Token: 0x04003A1D RID: 14877
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemValue expectedItem = ItemValue.None;

	// Token: 0x04003A1E RID: 14878
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemClass expectedItemClass;

	// Token: 0x04003A1F RID: 14879
	[PublicizedFrom(EAccessModifier.Private)]
	public int repairCount;
}
