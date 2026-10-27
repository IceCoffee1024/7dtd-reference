using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020009C9 RID: 2505
[Preserve]
public class ObjectiveWear : BaseObjective
{
	// Token: 0x06004968 RID: 18792 RVA: 0x001CD6DF File Offset: 0x001CB8DF
	public override void SetupObjective()
	{
		this.keyword = Localization.Get("ObjectiveWear_keyword", false, null);
		this.expectedItem = ItemClass.GetItem(this.ID, false);
		this.expectedItemClass = ItemClass.GetItemClass(this.ID, false);
	}

	// Token: 0x06004969 RID: 18793 RVA: 0x001CD717 File Offset: 0x001CB917
	public override void SetupDisplay()
	{
		byte currentValue = base.CurrentValue;
		base.Description = string.Format(this.keyword, this.expectedItemClass.GetLocalizedItemName());
		this.StatusText = "";
	}

	// Token: 0x0600496A RID: 18794 RVA: 0x001CD748 File Offset: 0x001CB948
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

	// Token: 0x0600496B RID: 18795 RVA: 0x001CD7B8 File Offset: 0x001CB9B8
	public override void RemoveHooks()
	{
		QuestEventManager.Current.WearItem -= this.Current_WearItem;
	}

	// Token: 0x0600496C RID: 18796 RVA: 0x001CD7D0 File Offset: 0x001CB9D0
	[PublicizedFrom(EAccessModifier.Private)]
	public void Current_WearItem(ItemValue itemValue)
	{
		if (itemValue.type == this.expectedItem.type && base.OwnerQuest.CheckRequirements())
		{
			base.CurrentValue = 1;
			this.Refresh();
		}
	}

	// Token: 0x0600496D RID: 18797 RVA: 0x001CD800 File Offset: 0x001CBA00
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

	// Token: 0x0600496E RID: 18798 RVA: 0x001CD840 File Offset: 0x001CBA40
	public override BaseObjective Clone()
	{
		ObjectiveWear objectiveWear = new ObjectiveWear();
		this.CopyValues(objectiveWear);
		return objectiveWear;
	}

	// Token: 0x0600496F RID: 18799 RVA: 0x001CD85B File Offset: 0x001CBA5B
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void InternalToParametersDictionary(ref Dictionary<string, object> parameters)
	{
		parameters.Add("expected_item_id", this.expectedItem.ItemClass.Name);
	}

	// Token: 0x04003A94 RID: 14996
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemValue expectedItem = ItemValue.None;

	// Token: 0x04003A95 RID: 14997
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemClass expectedItemClass;
}
