using System;
using UnityEngine.Scripting;

// Token: 0x02000992 RID: 2450
[Preserve]
public class ObjectiveFetch : BaseObjective
{
	// Token: 0x17000799 RID: 1945
	// (get) Token: 0x060047DE RID: 18398 RVA: 0x00020021 File Offset: 0x0001E221
	public override BaseObjective.ObjectiveValueTypes ObjectiveValueType
	{
		get
		{
			return BaseObjective.ObjectiveValueTypes.Number;
		}
	}

	// Token: 0x060047DF RID: 18399 RVA: 0x001C20AC File Offset: 0x001C02AC
	public override void SetupObjective()
	{
		this.keyword = Localization.Get("ObjectiveFetch_keyword", false, null);
		this.expectedItem = ItemClass.GetItem(this.ID, false);
		this.expectedItemClass = ItemClass.GetItemClass(this.ID, false);
		this.itemCount = Convert.ToInt32(this.Value);
	}

	// Token: 0x060047E0 RID: 18400 RVA: 0x001C2100 File Offset: 0x001C0300
	public override void SetupDisplay()
	{
		base.Description = string.Format(this.keyword, this.expectedItemClass.GetLocalizedItemName());
		this.StatusText = string.Format("{0}/{1}", this.currentCount, this.itemCount);
	}

	// Token: 0x060047E1 RID: 18401 RVA: 0x001C2150 File Offset: 0x001C0350
	public override void AddHooks()
	{
		LocalPlayerUI.GetUIForPlayer(base.OwnerQuest.OwnerJournal.OwnerPlayer);
		XUiM_PlayerInventory playerInventory = LocalPlayerUI.GetUIForPlayer(base.OwnerQuest.OwnerJournal.OwnerPlayer).xui.PlayerInventory;
		playerInventory.Backpack.OnBackpackItemsChangedInternal += this.Backpack_OnBackpackItemsChangedInternal;
		playerInventory.Toolbelt.OnToolbeltItemsChangedInternal += this.Toolbelt_OnToolbeltItemsChangedInternal;
		this.Refresh();
	}

	// Token: 0x060047E2 RID: 18402 RVA: 0x001C21C8 File Offset: 0x001C03C8
	public override void RemoveHooks()
	{
		XUiM_PlayerInventory playerInventory = LocalPlayerUI.GetUIForPlayer(base.OwnerQuest.OwnerJournal.OwnerPlayer).xui.PlayerInventory;
		if (playerInventory != null)
		{
			playerInventory.Backpack.OnBackpackItemsChangedInternal -= this.Backpack_OnBackpackItemsChangedInternal;
			playerInventory.Toolbelt.OnToolbeltItemsChangedInternal -= this.Toolbelt_OnToolbeltItemsChangedInternal;
		}
	}

	// Token: 0x060047E3 RID: 18403 RVA: 0x001C2228 File Offset: 0x001C0428
	[PublicizedFrom(EAccessModifier.Private)]
	public void Current_AddItem(ItemStack stack)
	{
		if (base.Complete)
		{
			return;
		}
		if (stack.itemValue.type == this.expectedItem.type)
		{
			if ((int)base.CurrentValue + stack.count > this.itemCount)
			{
				base.CurrentValue = (byte)this.itemCount;
			}
			else
			{
				base.CurrentValue += (byte)stack.count;
			}
			this.Refresh();
		}
	}

	// Token: 0x060047E4 RID: 18404 RVA: 0x001C2298 File Offset: 0x001C0498
	[PublicizedFrom(EAccessModifier.Private)]
	public void Backpack_OnBackpackItemsChangedInternal()
	{
		LocalPlayerUI uiforPlayer = LocalPlayerUI.GetUIForPlayer(base.OwnerQuest.OwnerJournal.OwnerPlayer);
		if (base.Complete || uiforPlayer.xui.PlayerInventory == null)
		{
			return;
		}
		this.Refresh();
	}

	// Token: 0x060047E5 RID: 18405 RVA: 0x001C22D8 File Offset: 0x001C04D8
	[PublicizedFrom(EAccessModifier.Private)]
	public void Toolbelt_OnToolbeltItemsChangedInternal()
	{
		LocalPlayerUI uiforPlayer = LocalPlayerUI.GetUIForPlayer(base.OwnerQuest.OwnerJournal.OwnerPlayer);
		if (base.Complete || uiforPlayer.xui.PlayerInventory == null)
		{
			return;
		}
		this.Refresh();
	}

	// Token: 0x060047E6 RID: 18406 RVA: 0x001C2318 File Offset: 0x001C0518
	public override void Refresh()
	{
		if (base.Complete)
		{
			return;
		}
		XUiM_PlayerInventory playerInventory = LocalPlayerUI.GetUIForPlayer(base.OwnerQuest.OwnerJournal.OwnerPlayer).xui.PlayerInventory;
		this.currentCount = playerInventory.Backpack.GetItemCount(this.expectedItem, -1, -1, true);
		this.currentCount += playerInventory.Toolbelt.GetItemCount(this.expectedItem, false, -1, -1, true);
		if (this.currentCount > this.itemCount)
		{
			this.currentCount = this.itemCount;
		}
		this.SetupDisplay();
		if (this.currentCount != (int)base.CurrentValue)
		{
			base.CurrentValue = (byte)this.currentCount;
		}
		base.Complete = this.currentCount >= this.itemCount && base.OwnerQuest.CheckRequirements();
		if (base.Complete)
		{
			base.OwnerQuest.RefreshQuestCompletion(QuestClass.CompletionTypes.AutoComplete, null, true, null);
		}
	}

	// Token: 0x060047E7 RID: 18407 RVA: 0x001C2400 File Offset: 0x001C0600
	public override void RemoveObjectives()
	{
		if (!this.KeepItems)
		{
			XUiM_PlayerInventory playerInventory = LocalPlayerUI.GetUIForPlayer(base.OwnerQuest.OwnerJournal.OwnerPlayer).xui.PlayerInventory;
			this.itemCount = playerInventory.Backpack.DecItem(this.expectedItem, this.itemCount, false, null);
			if (this.itemCount > 0)
			{
				playerInventory.Toolbelt.DecItem(this.expectedItem, this.itemCount, false, null);
			}
		}
	}

	// Token: 0x060047E8 RID: 18408 RVA: 0x001C2478 File Offset: 0x001C0678
	public override BaseObjective Clone()
	{
		ObjectiveFetch objectiveFetch = new ObjectiveFetch();
		this.CopyValues(objectiveFetch);
		objectiveFetch.KeepItems = this.KeepItems;
		return objectiveFetch;
	}

	// Token: 0x060047E9 RID: 18409 RVA: 0x001C24A0 File Offset: 0x001C06A0
	public override string ParseBinding(string bindingName)
	{
		string id = this.ID;
		string value = this.Value;
		if (!(bindingName == "items"))
		{
			if (!(bindingName == "itemswithcount"))
			{
				return "";
			}
			ItemClass itemClass = ItemClass.GetItemClass(id, false);
			int num = Convert.ToInt32(value);
			if (itemClass == null)
			{
				return "INVALID";
			}
			return num.ToString() + " " + itemClass.GetLocalizedItemName();
		}
		else
		{
			ItemClass itemClass2 = ItemClass.GetItemClass(id, false);
			if (itemClass2 == null)
			{
				return "INVALID";
			}
			return itemClass2.GetLocalizedItemName();
		}
	}

	// Token: 0x04003986 RID: 14726
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemValue expectedItem = ItemValue.None;

	// Token: 0x04003987 RID: 14727
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemClass expectedItemClass;

	// Token: 0x04003988 RID: 14728
	[PublicizedFrom(EAccessModifier.Private)]
	public int itemCount;

	// Token: 0x04003989 RID: 14729
	[PublicizedFrom(EAccessModifier.Private)]
	public int currentCount;

	// Token: 0x0400398A RID: 14730
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool KeepItems;
}
