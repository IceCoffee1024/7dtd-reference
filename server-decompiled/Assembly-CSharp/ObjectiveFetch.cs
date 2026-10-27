using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020009A2 RID: 2466
[Preserve]
public class ObjectiveFetch : BaseObjective
{
	// Token: 0x170007BF RID: 1983
	// (get) Token: 0x060047AF RID: 18351 RVA: 0x00020119 File Offset: 0x0001E319
	public override BaseObjective.ObjectiveValueTypes ObjectiveValueType
	{
		get
		{
			return BaseObjective.ObjectiveValueTypes.Number;
		}
	}

	// Token: 0x060047B0 RID: 18352 RVA: 0x001C3880 File Offset: 0x001C1A80
	public override void SetupObjective()
	{
		this.keyword = Localization.Get("ObjectiveFetch_keyword", false, null);
		this.expectedItem = ItemClass.GetItem(this.ID, false);
		this.expectedItemClass = ItemClass.GetItemClass(this.ID, false);
		this.itemCount = Convert.ToInt32(this.Value);
	}

	// Token: 0x060047B1 RID: 18353 RVA: 0x001C38D4 File Offset: 0x001C1AD4
	public override void SetupDisplay()
	{
		base.Description = string.Format(this.keyword, this.expectedItemClass.GetLocalizedItemName());
		this.StatusText = string.Format("{0}/{1}", this.currentCount, this.itemCount);
	}

	// Token: 0x060047B2 RID: 18354 RVA: 0x001C3924 File Offset: 0x001C1B24
	public override void AddHooks()
	{
		LocalPlayerUI.GetUIForPlayer(base.OwnerQuest.OwnerJournal.OwnerPlayer);
		XUiM_PlayerInventory playerInventory = LocalPlayerUI.GetUIForPlayer(base.OwnerQuest.OwnerJournal.OwnerPlayer).xui.PlayerInventory;
		playerInventory.Backpack.OnBackpackItemsChangedInternal += this.Backpack_OnBackpackItemsChangedInternal;
		playerInventory.Toolbelt.OnToolbeltItemsChangedInternal += this.Toolbelt_OnToolbeltItemsChangedInternal;
		this.Refresh();
	}

	// Token: 0x060047B3 RID: 18355 RVA: 0x001C399C File Offset: 0x001C1B9C
	public override void RemoveHooks()
	{
		XUiM_PlayerInventory playerInventory = LocalPlayerUI.GetUIForPlayer(base.OwnerQuest.OwnerJournal.OwnerPlayer).xui.PlayerInventory;
		if (playerInventory != null)
		{
			playerInventory.Backpack.OnBackpackItemsChangedInternal -= this.Backpack_OnBackpackItemsChangedInternal;
			playerInventory.Toolbelt.OnToolbeltItemsChangedInternal -= this.Toolbelt_OnToolbeltItemsChangedInternal;
		}
	}

	// Token: 0x060047B4 RID: 18356 RVA: 0x001C39FC File Offset: 0x001C1BFC
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

	// Token: 0x060047B5 RID: 18357 RVA: 0x001C3A6C File Offset: 0x001C1C6C
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

	// Token: 0x060047B6 RID: 18358 RVA: 0x001C3AAC File Offset: 0x001C1CAC
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

	// Token: 0x060047B7 RID: 18359 RVA: 0x001C3AEC File Offset: 0x001C1CEC
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

	// Token: 0x060047B8 RID: 18360 RVA: 0x001C3BD4 File Offset: 0x001C1DD4
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

	// Token: 0x060047B9 RID: 18361 RVA: 0x001C3C4C File Offset: 0x001C1E4C
	public override BaseObjective Clone()
	{
		ObjectiveFetch objectiveFetch = new ObjectiveFetch();
		this.CopyValues(objectiveFetch);
		objectiveFetch.KeepItems = this.KeepItems;
		return objectiveFetch;
	}

	// Token: 0x060047BA RID: 18362 RVA: 0x001C3C74 File Offset: 0x001C1E74
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

	// Token: 0x060047BB RID: 18363 RVA: 0x001C3CFC File Offset: 0x001C1EFC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void InternalToParametersDictionary(ref Dictionary<string, object> parameters)
	{
		parameters.Add("expected_item_id", this.expectedItem.ItemClass.Name);
		parameters.Add("count", this.itemCount);
		parameters.Add("keep_items", this.KeepItems);
	}

	// Token: 0x040039A2 RID: 14754
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemValue expectedItem = ItemValue.None;

	// Token: 0x040039A3 RID: 14755
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemClass expectedItemClass;

	// Token: 0x040039A4 RID: 14756
	[PublicizedFrom(EAccessModifier.Private)]
	public int itemCount;

	// Token: 0x040039A5 RID: 14757
	[PublicizedFrom(EAccessModifier.Private)]
	public int currentCount;

	// Token: 0x040039A6 RID: 14758
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool KeepItems;
}
