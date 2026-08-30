using System;
using UnityEngine.Scripting;

// Token: 0x02000D41 RID: 3393
[Preserve]
public class ItemActionEntryAssemble : BaseItemActionEntry
{
	// Token: 0x06006766 RID: 26470 RVA: 0x0028C3F8 File Offset: 0x0028A5F8
	public ItemActionEntryAssemble(XUiController _controller)
		: base(_controller, "lblContextActionModify", "ui_game_symbol_assemble", BaseItemActionEntry.GamepadShortCut.None, "crafting/craft_click_craft", "ui/ui_denied")
	{
		XUiC_ItemStack xuiC_ItemStack = base.ItemController as XUiC_ItemStack;
		if (xuiC_ItemStack != null)
		{
			if (xuiC_ItemStack.AssembleLock)
			{
				base.ActionName = Localization.Get("lblContextActionComplete", false, null);
				return;
			}
		}
		else if (base.ItemController is XUiC_EquipmentStack && base.ItemController.xui.AssembleItem.CurrentEquipmentStackController == base.ItemController)
		{
			base.ActionName = Localization.Get("lblContextActionComplete", false, null);
		}
	}

	// Token: 0x06006767 RID: 26471 RVA: 0x0028C487 File Offset: 0x0028A687
	public override void OnDisabledActivate()
	{
		this.setWindowsDirty();
		if (this.recipeUnknown)
		{
			GameManager.ShowTooltip(base.ItemController.xui.playerUI.entityPlayer, Localization.Get("xuiAssembleMustReadBook", false, null), false, false, 0f);
		}
	}

	// Token: 0x06006768 RID: 26472 RVA: 0x0028C4C4 File Offset: 0x0028A6C4
	public override void OnActivated()
	{
		this.setWindowsDirty();
		GUIWindowManager windowManager = base.ItemController.xui.playerUI.windowManager;
		XUiController itemController = base.ItemController;
		XUiC_ItemStack xuiC_ItemStack = itemController as XUiC_ItemStack;
		if (xuiC_ItemStack == null)
		{
			XUiC_EquipmentStack xuiC_EquipmentStack = itemController as XUiC_EquipmentStack;
			if (xuiC_EquipmentStack == null)
			{
				Log.Error("Modify, neither ItemStack nor EquipmentStack");
				return;
			}
			ItemStack itemStack = xuiC_EquipmentStack.ItemStack.Clone();
			base.ItemController.xui.AssembleItem.CurrentItem = itemStack;
			base.ItemController.xui.AssembleItem.CurrentItemStackController = null;
			base.ItemController.xui.AssembleItem.CurrentEquipmentStackController = xuiC_EquipmentStack;
			if (!windowManager.IsWindowOpen(XUiC_AssembleWindowGroup.ID))
			{
				windowManager.Open(XUiC_AssembleWindowGroup.ID, true);
				xuiC_EquipmentStack.InfoWindow.SetItemStack(xuiC_EquipmentStack, true);
				return;
			}
			XUiC_AssembleWindowGroup.GetWindowGroup(base.ItemController.xui).ItemStack = itemStack;
			xuiC_EquipmentStack.InfoWindow.SetItemStack(xuiC_EquipmentStack, true);
			windowManager.Close(XUiC_AssembleWindowGroup.ID);
			return;
		}
		else
		{
			if (xuiC_ItemStack.AssembleLock)
			{
				base.ItemController.xui.playerUI.windowManager.CloseAllOpenModalWindows(null, false);
				return;
			}
			ItemStack itemStack = xuiC_ItemStack.ItemStack.Clone();
			itemStack = ItemActionEntryScrap.HandleRemoveAmmo(itemStack, base.ItemController.xui);
			xuiC_ItemStack.ForceSetItemStack(itemStack);
			base.ItemController.xui.AssembleItem.CurrentItem = itemStack;
			base.ItemController.xui.AssembleItem.CurrentItemStackController = xuiC_ItemStack;
			base.ItemController.xui.AssembleItem.CurrentEquipmentStackController = null;
			if (!windowManager.IsWindowOpen(XUiC_AssembleWindowGroup.ID))
			{
				windowManager.Open(XUiC_AssembleWindowGroup.ID, true);
				xuiC_ItemStack.InfoWindow.SetItemStack(xuiC_ItemStack, true);
				return;
			}
			XUiC_AssembleWindowGroup.GetWindowGroup(base.ItemController.xui).ItemStack = itemStack;
			xuiC_ItemStack.InfoWindow.SetItemStack(xuiC_ItemStack, true);
			return;
		}
	}

	// Token: 0x06006769 RID: 26473 RVA: 0x0028C69C File Offset: 0x0028A89C
	[PublicizedFrom(EAccessModifier.Private)]
	public void setWindowsDirty()
	{
		GUIWindowManager windowManager = base.ItemController.xui.playerUI.windowManager;
		((XUiWindowGroup)windowManager.GetWindow("toolbelt")).Controller.GetChildByType<XUiC_Toolbelt>().SetAllChildrenDirty();
		((XUiWindowGroup)windowManager.GetWindow("backpack")).Controller.GetChildByType<XUiC_Backpack>().SetAllChildrenDirty();
	}

	// Token: 0x0600676A RID: 26474 RVA: 0x0028C6FC File Offset: 0x0028A8FC
	public override void RefreshEnabled()
	{
		this.setWindowsDirty();
		XUiController itemController = base.ItemController;
		XUiC_ItemStack xuiC_ItemStack = itemController as XUiC_ItemStack;
		ItemStack itemStack;
		if (xuiC_ItemStack == null)
		{
			XUiC_EquipmentStack xuiC_EquipmentStack = itemController as XUiC_EquipmentStack;
			if (xuiC_EquipmentStack == null)
			{
				return;
			}
			itemStack = xuiC_EquipmentStack.ItemStack;
		}
		else
		{
			itemStack = xuiC_ItemStack.ItemStack;
		}
		if (itemStack.IsEmpty())
		{
			return;
		}
		ItemClass forId = ItemClass.GetForId(itemStack.itemValue.type);
		if (forId.HasSubItems)
		{
			if (!XUiM_Recipes.GetRecipeIsUnlocked(base.ItemController.xui, forId.Name))
			{
				this.recipeUnknown = true;
			}
		}
		else if (forId.PartParentId != null)
		{
			for (int i = 0; i < forId.PartParentId.Count; i++)
			{
				if (XUiM_Recipes.GetRecipeIsUnlocked(base.ItemController.xui, ItemClass.GetForId(forId.PartParentId[i]).Name))
				{
					this.recipeUnknown = false;
					break;
				}
				this.recipeUnknown = true;
			}
		}
		base.Enabled = base.ItemController.xui.playerUI.entityPlayer.IsAimingGunPossible();
	}

	// Token: 0x04004F47 RID: 20295
	[PublicizedFrom(EAccessModifier.Private)]
	public bool recipeUnknown;
}
