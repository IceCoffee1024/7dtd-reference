using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000998 RID: 2456
[Preserve]
public class ObjectiveFetch : BaseObjective
{
	// Token: 0x170007B9 RID: 1977
	// (get) Token: 0x0600478C RID: 18316 RVA: 0x00020119 File Offset: 0x0001E319
	public override BaseObjective.ObjectiveValueTypes ObjectiveValueType
	{
		get
		{
			return BaseObjective.ObjectiveValueTypes.Number;
		}
	}

	// Token: 0x0600478D RID: 18317 RVA: 0x001C27FC File Offset: 0x001C09FC
	public override void SetupObjective()
	{
		this.keyword = Localization.Get("ObjectiveFetch_keyword", false, null);
		this.expectedItem = ItemClass.GetItem(this.ID, false);
		this.expectedItemClass = ItemClass.GetItemClass(this.ID, false);
		this.itemCount = Convert.ToInt32(this.Value);
	}

	// Token: 0x0600478E RID: 18318 RVA: 0x001C2850 File Offset: 0x001C0A50
	public override void SetupDisplay()
	{
		base.Description = string.Format(this.keyword, this.expectedItemClass.GetLocalizedItemName());
		this.StatusText = string.Format("{0}/{1}", this.currentCount, this.itemCount);
	}

	// Token: 0x0600478F RID: 18319 RVA: 0x001C28A0 File Offset: 0x001C0AA0
	public override void AddHooks()
	{
		LocalPlayerUI.GetUIForPlayer(base.OwnerQuest.OwnerJournal.OwnerPlayer);
		XUiM_PlayerInventory playerInventory = LocalPlayerUI.GetUIForPlayer(base.OwnerQuest.OwnerJournal.OwnerPlayer).xui.PlayerInventory;
		playerInventory.Backpack.OnBackpackItemsChangedInternal += this.Backpack_OnBackpackItemsChangedInternal;
		playerInventory.Toolbelt.OnToolbeltItemsChangedInternal += this.Toolbelt_OnToolbeltItemsChangedInternal;
		this.Refresh();
	}

	// Token: 0x06004790 RID: 18320 RVA: 0x001C2918 File Offset: 0x001C0B18
	public override void RemoveHooks()
	{
		XUiM_PlayerInventory playerInventory = LocalPlayerUI.GetUIForPlayer(base.OwnerQuest.OwnerJournal.OwnerPlayer).xui.PlayerInventory;
		if (playerInventory != null)
		{
			playerInventory.Backpack.OnBackpackItemsChangedInternal -= this.Backpack_OnBackpackItemsChangedInternal;
			playerInventory.Toolbelt.OnToolbeltItemsChangedInternal -= this.Toolbelt_OnToolbeltItemsChangedInternal;
		}
	}

	// Token: 0x06004791 RID: 18321 RVA: 0x001C2978 File Offset: 0x001C0B78
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

	// Token: 0x06004792 RID: 18322 RVA: 0x001C29E8 File Offset: 0x001C0BE8
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

	// Token: 0x06004793 RID: 18323 RVA: 0x001C2A28 File Offset: 0x001C0C28
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

	// Token: 0x06004794 RID: 18324 RVA: 0x001C2A68 File Offset: 0x001C0C68
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

	// Token: 0x06004795 RID: 18325 RVA: 0x001C2B50 File Offset: 0x001C0D50
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

	// Token: 0x06004796 RID: 18326 RVA: 0x001C2BC8 File Offset: 0x001C0DC8
	public override BaseObjective Clone()
	{
		ObjectiveFetch objectiveFetch = new ObjectiveFetch();
		this.CopyValues(objectiveFetch);
		objectiveFetch.KeepItems = this.KeepItems;
		return objectiveFetch;
	}

	// Token: 0x06004797 RID: 18327 RVA: 0x001C2BF0 File Offset: 0x001C0DF0
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

	// Token: 0x06004798 RID: 18328 RVA: 0x001C2C78 File Offset: 0x001C0E78
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void InternalToParametersDictionary(ref Dictionary<string, object> parameters)
	{
		parameters.Add("expected_item_id", this.expectedItem.ItemClass.Name);
		parameters.Add("count", this.itemCount);
		parameters.Add("keep_items", this.KeepItems);
	}

	// Token: 0x04003982 RID: 14722
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemValue expectedItem = ItemValue.None;

	// Token: 0x04003983 RID: 14723
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemClass expectedItemClass;

	// Token: 0x04003984 RID: 14724
	[PublicizedFrom(EAccessModifier.Private)]
	public int itemCount;

	// Token: 0x04003985 RID: 14725
	[PublicizedFrom(EAccessModifier.Private)]
	public int currentCount;

	// Token: 0x04003986 RID: 14726
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool KeepItems;
}
