using System;
using Audio;
using UnityEngine;
using UnityEngine.Scripting;
using XMLData.Item;

// Token: 0x02000D6F RID: 3439
[Preserve]
public class ItemActionEntryRepair : BaseItemActionEntry
{
	// Token: 0x0600681B RID: 26651 RVA: 0x002942B4 File Offset: 0x002924B4
	public ItemActionEntryRepair(XUiController _controller)
		: base(_controller, "lblContextActionRepair", "ui_game_symbol_wrench", BaseItemActionEntry.GamepadShortCut.DPadLeft, "crafting/craft_click_craft", "ui/ui_denied")
	{
		_controller.xui.PlayerInventory.OnBackpackItemsChanged += this.PlayerInventory_OnBackpackItemsChanged;
		_controller.xui.PlayerInventory.OnToolbeltItemsChanged += this.PlayerInventory_OnToolbeltItemsChanged;
	}

	// Token: 0x0600681C RID: 26652 RVA: 0x00294315 File Offset: 0x00292515
	[PublicizedFrom(EAccessModifier.Private)]
	public void PlayerInventory_OnToolbeltItemsChanged()
	{
		this.RefreshEnabled();
		XUiC_ItemActionEntry parentItem = base.ParentItem;
		if (parentItem == null)
		{
			return;
		}
		parentItem.MarkDirty();
	}

	// Token: 0x0600681D RID: 26653 RVA: 0x00294315 File Offset: 0x00292515
	[PublicizedFrom(EAccessModifier.Private)]
	public void PlayerInventory_OnBackpackItemsChanged()
	{
		this.RefreshEnabled();
		XUiC_ItemActionEntry parentItem = base.ParentItem;
		if (parentItem == null)
		{
			return;
		}
		parentItem.MarkDirty();
	}

	// Token: 0x0600681E RID: 26654 RVA: 0x00294330 File Offset: 0x00292530
	public override void OnDisabledActivate()
	{
		switch (this.state)
		{
		case ItemActionEntryRepair.StateTypes.RecipeLocked:
			GameManager.ShowTooltip(base.ItemController.xui.playerUI.entityPlayer, Localization.Get("xuiRepairMustReadBook", false, null), true, false, 0f);
			return;
		case ItemActionEntryRepair.StateTypes.NotEnoughMaterials:
		{
			GameManager.ShowTooltip(base.ItemController.xui.playerUI.entityPlayer, Localization.Get("xuiRepairMissingMats", false, null), true, false, 0f);
			ItemClass itemClass = null;
			XUiC_EquipmentStack xuiC_EquipmentStack = base.ItemController as XUiC_EquipmentStack;
			if (xuiC_EquipmentStack != null)
			{
				if (xuiC_EquipmentStack.ItemStack.IsEmpty())
				{
					return;
				}
				itemClass = xuiC_EquipmentStack.ItemValue.ItemClass;
			}
			else
			{
				IItemStackController itemStackController = base.ItemController as IItemStackController;
				if (itemStackController != null)
				{
					if (!itemStackController.ItemStack.IsEmpty())
					{
						XUiC_ItemStack xuiC_ItemStack = base.ItemController as XUiC_ItemStack;
						if (xuiC_ItemStack == null || !xuiC_ItemStack.StackLock)
						{
							itemClass = itemStackController.ItemStack.itemValue.ItemClass;
							goto IL_00F0;
						}
					}
					return;
				}
			}
			IL_00F0:
			if (itemClass != null)
			{
				ItemData.DataItemArrayRepairTools repairTools = itemClass.RepairTools;
				int? num = ((repairTools != null) ? new int?(repairTools.Length) : null);
				int num2 = 0;
				if ((num.GetValueOrDefault() > num2) & (num != null))
				{
					ItemClass itemClass2 = ItemClass.GetItemClass(itemClass.RepairTools[0].Value, false);
					if (itemClass2 != null)
					{
						ItemStack itemStack = new ItemStack(new ItemValue(itemClass2.Id, false), 0);
						base.ItemController.xui.playerUI.entityPlayer.AddUIHarvestingItem(itemStack, true);
						return;
					}
				}
			}
			break;
		}
		case ItemActionEntryRepair.StateTypes.NoLongerRepairable:
			GameManager.ShowTooltip(base.ItemController.xui.playerUI.entityPlayer, Localization.Get("ttItemCanNoLongerBeRepaired", false, null), true, false, 0f);
			return;
		case ItemActionEntryRepair.StateTypes.DurabilityWouldBeReduced:
			GameManager.ShowTooltip(base.ItemController.xui.playerUI.entityPlayer, Localization.Get("ttDurabilityWouldBeReduced", false, null), true, false, 0f);
			break;
		default:
			return;
		}
	}

	// Token: 0x0600681F RID: 26655 RVA: 0x00294520 File Offset: 0x00292720
	public override void RefreshEnabled()
	{
		base.RefreshEnabled();
		this.state = ItemActionEntryRepair.StateTypes.Normal;
		XUi xui = base.ItemController.xui;
		ItemClass itemClass = null;
		ItemStack itemStack = ItemStack.Empty;
		XUiC_EquipmentStack xuiC_EquipmentStack = base.ItemController as XUiC_EquipmentStack;
		if (xuiC_EquipmentStack != null)
		{
			if (xuiC_EquipmentStack.ItemStack.IsEmpty())
			{
				return;
			}
			itemClass = xuiC_EquipmentStack.ItemValue.ItemClass;
			itemStack = xuiC_EquipmentStack.ItemStack;
		}
		else
		{
			IItemStackController itemStackController = base.ItemController as IItemStackController;
			if (itemStackController != null)
			{
				if (!itemStackController.ItemStack.IsEmpty())
				{
					XUiC_ItemStack xuiC_ItemStack = base.ItemController as XUiC_ItemStack;
					if (xuiC_ItemStack == null || !xuiC_ItemStack.StackLock)
					{
						itemClass = itemStackController.ItemStack.itemValue.ItemClass;
						itemStack = itemStackController.ItemStack;
						goto IL_00A7;
					}
				}
				return;
			}
		}
		IL_00A7:
		base.Enabled = this.state == ItemActionEntryRepair.StateTypes.Normal;
		if (!base.Enabled)
		{
			base.IconName = "ui_game_symbol_book";
			return;
		}
		ItemValue itemValue = itemStack.itemValue;
		ItemAction.CanRepairValues canRepairValues = ItemAction.CanRepair(itemValue);
		if (canRepairValues == ItemAction.CanRepairValues.NoLongerRepairable)
		{
			this.state = ItemActionEntryRepair.StateTypes.NoLongerRepairable;
			base.Enabled = false;
			return;
		}
		if (canRepairValues == ItemAction.CanRepairValues.DurabilityWouldBeReduced)
		{
			this.state = ItemActionEntryRepair.StateTypes.DurabilityWouldBeReduced;
			base.Enabled = false;
			return;
		}
		if (itemClass != null)
		{
			ItemData.DataItemArrayRepairTools repairTools = itemClass.RepairTools;
			int? num = ((repairTools != null) ? new int?(repairTools.Length) : null);
			int num2 = 0;
			if ((num.GetValueOrDefault() > num2) & (num != null))
			{
				ItemClass itemClass2 = ItemClass.GetItemClass(itemClass.RepairTools[0].Value, false);
				if (itemClass2 == null)
				{
					return;
				}
				int num3 = Convert.ToInt32(Math.Ceiling((double)((float)Mathf.CeilToInt(itemValue.UseTimes) / (float)itemClass2.RepairAmount.Value)));
				if (Mathf.Min(xui.PlayerInventory.GetItemCount(new ItemValue(itemClass2.Id, false)), num3) * itemClass2.RepairAmount.Value > 0)
				{
					return;
				}
				this.state = ItemActionEntryRepair.StateTypes.NotEnoughMaterials;
				base.Enabled = this.state == ItemActionEntryRepair.StateTypes.Normal;
				return;
			}
		}
	}

	// Token: 0x06006820 RID: 26656 RVA: 0x002946FC File Offset: 0x002928FC
	public override void OnActivated()
	{
		XUi xui = base.ItemController.xui;
		XUiM_PlayerInventory playerInventory = xui.PlayerInventory;
		ItemStack itemStack = ItemStack.Empty;
		XUiC_EquipmentStack xuiC_EquipmentStack = base.ItemController as XUiC_EquipmentStack;
		if (xuiC_EquipmentStack != null)
		{
			itemStack = xuiC_EquipmentStack.ItemStack;
		}
		else
		{
			XUiC_ItemStack xuiC_ItemStack = base.ItemController as XUiC_ItemStack;
			if (xuiC_ItemStack != null)
			{
				xuiC_ItemStack.TimeIntervalElapsedEvent += this.ItemActionEntryRepair_TimeIntervalElapsedEvent;
				itemStack = xuiC_ItemStack.ItemStack;
			}
		}
		ItemValue itemValue = itemStack.itemValue;
		ItemClass forId = ItemClass.GetForId(itemValue.type);
		if (((forId != null) ? forId.RepairTools : null) == null || forId.RepairTools.Length <= 0)
		{
			return;
		}
		ItemClass itemClass = ItemClass.GetItemClass(forId.RepairTools[0].Value, false);
		if (itemClass == null)
		{
			return;
		}
		int num = Convert.ToInt32(Math.Ceiling((double)((float)Mathf.CeilToInt(itemValue.UseTimes) / (float)itemClass.RepairAmount.Value)));
		int num2 = Mathf.Min(playerInventory.GetItemCount(new ItemValue(itemClass.Id, false)), num);
		int num3 = num2 * itemClass.RepairAmount.Value;
		XUiC_CraftingWindowGroup childByType = xui.FindWindowGroupByName("crafting").GetChildByType<XUiC_CraftingWindowGroup>();
		if (childByType == null || num3 <= 0)
		{
			return;
		}
		Recipe recipe = new Recipe();
		recipe.count = 1;
		recipe.craftExpGain = Mathf.CeilToInt(forId.RepairExpMultiplier * (float)num2);
		recipe.ingredients.Add(new ItemStack(new ItemValue(itemClass.Id, false), num2));
		recipe.itemValueType = itemValue.type;
		recipe.craftingTime = itemClass.RepairTime.Value * (float)num2;
		num3 = (int)EffectManager.GetValue(PassiveEffects.RepairAmount, null, (float)num3, xui.playerUI.entityPlayer, recipe, FastTags<TagGroup.Global>.Parse(recipe.GetName()), true, true, true, true, true, 1, true, false);
		recipe.craftingTime = (float)((int)EffectManager.GetValue(PassiveEffects.CraftingTime, null, recipe.craftingTime, xui.playerUI.entityPlayer, recipe, FastTags<TagGroup.Global>.Parse(recipe.GetName()), true, true, true, true, true, 1, true, false));
		if (!childByType.AddRepairItemToQueue(recipe.craftingTime, itemValue.Clone(), num3))
		{
			this.warnQueueFull();
			return;
		}
		XUiC_EquipmentStack xuiC_EquipmentStack2 = base.ItemController as XUiC_EquipmentStack;
		if (xuiC_EquipmentStack2 != null)
		{
			xuiC_EquipmentStack2.ItemStack = ItemStack.Empty;
			xui.PlayerEquipment.Equipment.SetPreferredItemSlot(xuiC_EquipmentStack2.SlotNumber, itemValue);
		}
		else
		{
			IItemStackController itemStackController = base.ItemController as IItemStackController;
			if (itemStackController != null)
			{
				itemStackController.ItemStack = ItemStack.Empty;
			}
		}
		playerInventory.RemoveItems(recipe.ingredients, 1, null);
	}

	// Token: 0x06006821 RID: 26657 RVA: 0x00294984 File Offset: 0x00292B84
	[PublicizedFrom(EAccessModifier.Private)]
	public void ItemActionEntryRepair_TimeIntervalElapsedEvent(float _timeLeft, XUiC_ItemStack _uiItemStack)
	{
		if (_timeLeft > 0f)
		{
			return;
		}
		ItemStack itemStack = _uiItemStack.ItemStack.Clone();
		itemStack.itemValue.UseTimes = Mathf.Max(0f, itemStack.itemValue.UseTimes - (float)_uiItemStack.RepairAmount);
		_uiItemStack.ItemStack = itemStack;
		_uiItemStack.TimeIntervalElapsedEvent -= this.ItemActionEntryRepair_TimeIntervalElapsedEvent;
		_uiItemStack.UnlockStack();
	}

	// Token: 0x06006822 RID: 26658 RVA: 0x002949F0 File Offset: 0x00292BF0
	[PublicizedFrom(EAccessModifier.Private)]
	public void warnQueueFull()
	{
		string text = "No room in queue!";
		if (Localization.Exists("wrnQueueFull", false))
		{
			text = Localization.Get("wrnQueueFull", false, null);
		}
		GameManager.ShowTooltip(base.ItemController.xui.playerUI.entityPlayer, text, false, false, 0f);
		Manager.PlayInsidePlayerHead("ui_denied", -1, 0f, false, false);
	}

	// Token: 0x06006823 RID: 26659 RVA: 0x00294A54 File Offset: 0x00292C54
	public override void DisableEvents()
	{
		base.ItemController.xui.PlayerInventory.OnBackpackItemsChanged -= this.PlayerInventory_OnBackpackItemsChanged;
		base.ItemController.xui.PlayerInventory.OnToolbeltItemsChanged -= this.PlayerInventory_OnToolbeltItemsChanged;
	}

	// Token: 0x04005035 RID: 20533
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemActionEntryRepair.StateTypes state;

	// Token: 0x02000D70 RID: 3440
	[PublicizedFrom(EAccessModifier.Private)]
	public enum StateTypes
	{
		// Token: 0x04005037 RID: 20535
		Normal,
		// Token: 0x04005038 RID: 20536
		RecipeLocked,
		// Token: 0x04005039 RID: 20537
		NotEnoughMaterials,
		// Token: 0x0400503A RID: 20538
		NoLongerRepairable,
		// Token: 0x0400503B RID: 20539
		DurabilityWouldBeReduced
	}
}
