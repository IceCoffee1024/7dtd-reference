using System;
using UnityEngine.Scripting;

// Token: 0x020009A8 RID: 2472
[Preserve]
public class ObjectiveWear : BaseObjective
{
	// Token: 0x060048FE RID: 18686 RVA: 0x001C9313 File Offset: 0x001C7513
	public override void SetupObjective()
	{
		this.keyword = Localization.Get("ObjectiveWear_keyword", false, null);
		this.expectedItem = ItemClass.GetItem(this.ID, false);
		this.expectedItemClass = ItemClass.GetItemClass(this.ID, false);
	}

	// Token: 0x060048FF RID: 18687 RVA: 0x001C934B File Offset: 0x001C754B
	public override void SetupDisplay()
	{
		byte currentValue = base.CurrentValue;
		base.Description = string.Format(this.keyword, this.expectedItemClass.GetLocalizedItemName());
		this.StatusText = "";
	}

	// Token: 0x06004900 RID: 18688 RVA: 0x001C937C File Offset: 0x001C757C
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

	// Token: 0x06004901 RID: 18689 RVA: 0x001C93EC File Offset: 0x001C75EC
	public override void RemoveHooks()
	{
		QuestEventManager.Current.WearItem -= this.Current_WearItem;
	}

	// Token: 0x06004902 RID: 18690 RVA: 0x001C9404 File Offset: 0x001C7604
	[PublicizedFrom(EAccessModifier.Private)]
	public void Current_WearItem(ItemValue itemValue)
	{
		if (itemValue.type == this.expectedItem.type && base.OwnerQuest.CheckRequirements())
		{
			base.CurrentValue = 1;
			this.Refresh();
		}
	}

	// Token: 0x06004903 RID: 18691 RVA: 0x001C9434 File Offset: 0x001C7634
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

	// Token: 0x06004904 RID: 18692 RVA: 0x001C9474 File Offset: 0x001C7674
	public override BaseObjective Clone()
	{
		ObjectiveWear objectiveWear = new ObjectiveWear();
		this.CopyValues(objectiveWear);
		return objectiveWear;
	}

	// Token: 0x040039E6 RID: 14822
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemValue expectedItem = ItemValue.None;

	// Token: 0x040039E7 RID: 14823
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemClass expectedItemClass;
}
