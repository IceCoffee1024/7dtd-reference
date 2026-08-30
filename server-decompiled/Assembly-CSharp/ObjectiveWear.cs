using System;
using UnityEngine.Scripting;

// Token: 0x020009B9 RID: 2489
[Preserve]
public class ObjectiveWear : BaseObjective
{
	// Token: 0x06004984 RID: 18820 RVA: 0x001CBB07 File Offset: 0x001C9D07
	public override void SetupObjective()
	{
		this.keyword = Localization.Get("ObjectiveWear_keyword", false, null);
		this.expectedItem = ItemClass.GetItem(this.ID, false);
		this.expectedItemClass = ItemClass.GetItemClass(this.ID, false);
	}

	// Token: 0x06004985 RID: 18821 RVA: 0x001CBB3F File Offset: 0x001C9D3F
	public override void SetupDisplay()
	{
		byte currentValue = base.CurrentValue;
		base.Description = string.Format(this.keyword, this.expectedItemClass.GetLocalizedItemName());
		this.StatusText = "";
	}

	// Token: 0x06004986 RID: 18822 RVA: 0x001CBB70 File Offset: 0x001C9D70
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

	// Token: 0x06004987 RID: 18823 RVA: 0x001CBBE0 File Offset: 0x001C9DE0
	public override void RemoveHooks()
	{
		QuestEventManager.Current.WearItem -= this.Current_WearItem;
	}

	// Token: 0x06004988 RID: 18824 RVA: 0x001CBBF8 File Offset: 0x001C9DF8
	[PublicizedFrom(EAccessModifier.Private)]
	public void Current_WearItem(ItemValue itemValue)
	{
		if (itemValue.type == this.expectedItem.type && base.OwnerQuest.CheckRequirements())
		{
			base.CurrentValue = 1;
			this.Refresh();
		}
	}

	// Token: 0x06004989 RID: 18825 RVA: 0x001CBC28 File Offset: 0x001C9E28
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

	// Token: 0x0600498A RID: 18826 RVA: 0x001CBC68 File Offset: 0x001C9E68
	public override BaseObjective Clone()
	{
		ObjectiveWear objectiveWear = new ObjectiveWear();
		this.CopyValues(objectiveWear);
		return objectiveWear;
	}

	// Token: 0x04003A77 RID: 14967
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemValue expectedItem = ItemValue.None;

	// Token: 0x04003A78 RID: 14968
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemClass expectedItemClass;
}
