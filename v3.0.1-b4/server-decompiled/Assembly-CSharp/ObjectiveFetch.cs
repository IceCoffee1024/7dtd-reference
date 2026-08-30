using System;
using UnityEngine.Scripting;

// Token: 0x02000981 RID: 2433
[Preserve]
public class ObjectiveFetch : BaseObjective
{
	// Token: 0x17000787 RID: 1927
	// (get) Token: 0x06004758 RID: 18264 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override BaseObjective.ObjectiveValueTypes ObjectiveValueType
	{
		get
		{
			return BaseObjective.ObjectiveValueTypes.Number;
		}
	}

	// Token: 0x06004759 RID: 18265 RVA: 0x001BF8B8 File Offset: 0x001BDAB8
	public override void SetupObjective()
	{
		this.keyword = Localization.Get("ObjectiveFetch_keyword", false, null);
		this.expectedItem = ItemClass.GetItem(this.ID, false);
		this.expectedItemClass = ItemClass.GetItemClass(this.ID, false);
		this.itemCount = Convert.ToInt32(this.Value);
	}

	// Token: 0x0600475A RID: 18266 RVA: 0x001BF90C File Offset: 0x001BDB0C
	public override void SetupDisplay()
	{
		base.Description = string.Format(this.keyword, this.expectedItemClass.GetLocalizedItemName());
		this.StatusText = string.Format("{0}/{1}", this.currentCount, this.itemCount);
	}

	// Token: 0x0600475B RID: 18267 RVA: 0x001BF95C File Offset: 0x001BDB5C
	public override void AddHooks()
	{
		LocalPlayerUI.GetUIForPlayer(base.OwnerQuest.OwnerJournal.OwnerPlayer);
		XUiM_PlayerInventory playerInventory = LocalPlayerUI.GetUIForPlayer(base.OwnerQuest.OwnerJournal.OwnerPlayer).xui.PlayerInventory;
		playerInventory.Backpack.OnBackpackItemsChangedInternal += this.Backpack_OnBackpackItemsChangedInternal;
		playerInventory.Toolbelt.OnToolbeltItemsChangedInternal += this.Toolbelt_OnToolbeltItemsChangedInternal;
		this.Refresh();
	}

	// Token: 0x0600475C RID: 18268 RVA: 0x001BF9D4 File Offset: 0x001BDBD4
	public override void RemoveHooks()
	{
		XUiM_PlayerInventory playerInventory = LocalPlayerUI.GetUIForPlayer(base.OwnerQuest.OwnerJournal.OwnerPlayer).xui.PlayerInventory;
		if (playerInventory != null)
		{
			playerInventory.Backpack.OnBackpackItemsChangedInternal -= this.Backpack_OnBackpackItemsChangedInternal;
			playerInventory.Toolbelt.OnToolbeltItemsChangedInternal -= this.Toolbelt_OnToolbeltItemsChangedInternal;
		}
	}

	// Token: 0x0600475D RID: 18269 RVA: 0x001BFA34 File Offset: 0x001BDC34
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

	// Token: 0x0600475E RID: 18270 RVA: 0x001BFAA4 File Offset: 0x001BDCA4
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

	// Token: 0x0600475F RID: 18271 RVA: 0x001BFAE4 File Offset: 0x001BDCE4
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

	// Token: 0x06004760 RID: 18272 RVA: 0x001BFB24 File Offset: 0x001BDD24
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

	// Token: 0x06004761 RID: 18273 RVA: 0x001BFC0C File Offset: 0x001BDE0C
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

	// Token: 0x06004762 RID: 18274 RVA: 0x001BFC84 File Offset: 0x001BDE84
	public override BaseObjective Clone()
	{
		ObjectiveFetch objectiveFetch = new ObjectiveFetch();
		this.CopyValues(objectiveFetch);
		objectiveFetch.KeepItems = this.KeepItems;
		return objectiveFetch;
	}

	// Token: 0x06004763 RID: 18275 RVA: 0x001BFCAC File Offset: 0x001BDEAC
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

	// Token: 0x040038F5 RID: 14581
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemValue expectedItem = ItemValue.None;

	// Token: 0x040038F6 RID: 14582
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemClass expectedItemClass;

	// Token: 0x040038F7 RID: 14583
	[PublicizedFrom(EAccessModifier.Private)]
	public int itemCount;

	// Token: 0x040038F8 RID: 14584
	[PublicizedFrom(EAccessModifier.Private)]
	public int currentCount;

	// Token: 0x040038F9 RID: 14585
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool KeepItems;
}
