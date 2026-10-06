using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020009BF RID: 2495
[Preserve]
public class ObjectiveWear : BaseObjective
{
	// Token: 0x06004945 RID: 18757 RVA: 0x001CC60B File Offset: 0x001CA80B
	public override void SetupObjective()
	{
		this.keyword = Localization.Get("ObjectiveWear_keyword", false, null);
		this.expectedItem = ItemClass.GetItem(this.ID, false);
		this.expectedItemClass = ItemClass.GetItemClass(this.ID, false);
	}

	// Token: 0x06004946 RID: 18758 RVA: 0x001CC643 File Offset: 0x001CA843
	public override void SetupDisplay()
	{
		byte currentValue = base.CurrentValue;
		base.Description = string.Format(this.keyword, this.expectedItemClass.GetLocalizedItemName());
		this.StatusText = "";
	}

	// Token: 0x06004947 RID: 18759 RVA: 0x001CC674 File Offset: 0x001CA874
	public override void AddHooks()
	{
		QuestEventManager.Current.WearItem += this.Current_WearItem;
		XUi xui = LocalPlayerUI.GetUIForPlayer(base.OwnerQuest.OwnerJournal.OwnerPlayer).xui;
		XUiM_PlayerInventory playerInventory = xui.PlayerInventory;
		if (xui.PlayerEquipment.IsWearing(this.expectedItem) && base.OwnerQuest.CheckRequirements())
		{
			base.CurrentValue = 1;
			this.Refresh();
		}
	}

	// Token: 0x06004948 RID: 18760 RVA: 0x001CC6E4 File Offset: 0x001CA8E4
	public override void RemoveHooks()
	{
		QuestEventManager.Current.WearItem -= this.Current_WearItem;
	}

	// Token: 0x06004949 RID: 18761 RVA: 0x001CC6FC File Offset: 0x001CA8FC
	[PublicizedFrom(EAccessModifier.Private)]
	public void Current_WearItem(ItemValue itemValue)
	{
		if (itemValue.type == this.expectedItem.type && base.OwnerQuest.CheckRequirements())
		{
			base.CurrentValue = 1;
			this.Refresh();
		}
	}

	// Token: 0x0600494A RID: 18762 RVA: 0x001CC72C File Offset: 0x001CA92C
	public override void Refresh()
	{
		if (base.Complete)
		{
			return;
		}
		bool flag = base.CurrentValue == 1;
		base.Complete = flag;
		if (base.Complete)
		{
			base.OwnerQuest.RefreshQuestCompletion(QuestClass.CompletionTypes.AutoComplete, null, true, null);
		}
	}

	// Token: 0x0600494B RID: 18763 RVA: 0x001CC76C File Offset: 0x001CA96C
	public override BaseObjective Clone()
	{
		ObjectiveWear objectiveWear = new ObjectiveWear();
		this.CopyValues(objectiveWear);
		return objectiveWear;
	}

	// Token: 0x0600494C RID: 18764 RVA: 0x001CC787 File Offset: 0x001CA987
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void InternalToParametersDictionary(ref Dictionary<string, object> parameters)
	{
		parameters.Add("expected_item_id", this.expectedItem.ItemClass.Name);
	}

	// Token: 0x04003A74 RID: 14964
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemValue expectedItem = ItemValue.None;

	// Token: 0x04003A75 RID: 14965
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemClass expectedItemClass;
}
