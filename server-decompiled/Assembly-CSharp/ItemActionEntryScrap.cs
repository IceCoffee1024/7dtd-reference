using System;
using Audio;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000D72 RID: 3442
[Preserve]
public class ItemActionEntryScrap : BaseItemActionEntry
{
	// Token: 0x06006826 RID: 26662 RVA: 0x00294BC2 File Offset: 0x00292DC2
	public ItemActionEntryScrap(XUiController _controller)
		: base(_controller, "lblContextActionScrap", "ui_game_symbol_scrap", BaseItemActionEntry.GamepadShortCut.DPadRight, "crafting/craft_click_craft", "ui/ui_denied")
	{
	}

	// Token: 0x06006827 RID: 26663 RVA: 0x00294BF8 File Offset: 0x00292DF8
	public override void OnActivated()
	{
		XUi xui = base.ItemController.xui;
		IItemStackController itemStackController = (IItemStackController)base.ItemController;
		XUiC_ItemStack xuiC_ItemStack = base.ItemController as XUiC_ItemStack;
		if (xuiC_ItemStack != null)
		{
			xuiC_ItemStack.LockChangedEvent += this.ItemStackController_LockChangedEvent;
		}
		ItemStack itemStack = itemStackController.ItemStack.Clone();
		Recipe scrapableRecipe = CraftingManager.GetScrapableRecipe(itemStack.itemValue, itemStack.count);
		if (scrapableRecipe == null)
		{
			return;
		}
		this.recipe = scrapableRecipe;
		XUiController xuiController = base.ItemController.xui.FindWindowGroupByName("workstation_workbench");
		if (xuiController == null || !xuiController.WindowGroup.isShowing)
		{
			xuiController = xui.FindWindowGroupByName("crafting");
		}
		XUiC_CraftingWindowGroup childByType = xuiController.GetChildByType<XUiC_CraftingWindowGroup>();
		if (childByType == null)
		{
			return;
		}
		ItemClass forId = ItemClass.GetForId(this.recipe.itemValueType);
		ItemClass forId2 = ItemClass.GetForId(itemStack.itemValue.type);
		int num = forId2.GetWeight() * itemStack.count;
		int weight = forId.GetWeight();
		int num2 = num / weight;
		if (num2 <= 0)
		{
			return;
		}
		int num3 = (int)((float)(num2 * weight) / (float)forId2.GetWeight() + 0.5f);
		Recipe recipe = new Recipe();
		float num4 = ((XUiM_Recipes.DisableSmelter && forId2.HasAnyTags(ItemActionEntryScrap.scrap100Tag)) ? 1f : 0.75f);
		num2 = (int)((float)num2 * (num4 * XUiM_Recipes.ScrappingOutputModifier));
		if (num2 <= 0)
		{
			num2 = 1;
		}
		recipe.count = num2;
		recipe.craftExpGain = this.recipe.craftExpGain;
		recipe.ingredients.Add(new ItemStack(itemStack.itemValue, num3));
		recipe.itemValueType = forId.Id;
		recipe.craftingTime = ((forId2.ScrapTimeOverride > 0f) ? forId2.ScrapTimeOverride : EffectManager.GetValue(PassiveEffects.ScrappingTime, null, forId.CraftComponentTime * (float)num2, xui.playerUI.entityPlayer, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false));
		recipe.IsScrap = true;
		if (!childByType.AddItemToQueue(recipe, 1))
		{
			this.warnQueueFull();
			return;
		}
		itemStack.count -= num3;
		itemStack = ItemActionEntryScrap.HandleRemoveAmmo(itemStack, base.ItemController.xui);
		((IItemStackController)base.ItemController).ItemStack = ((itemStack.count <= 0) ? ItemStack.Empty : itemStack.Clone());
		((IItemStackController)base.ItemController).WindowGroup.Controller.SetAllChildrenDirty();
	}

	// Token: 0x06006828 RID: 26664 RVA: 0x00294E58 File Offset: 0x00293058
	[PublicizedFrom(EAccessModifier.Private)]
	public void warnQueueFull()
	{
		GameManager.ShowTooltip(base.ItemController.xui.playerUI.entityPlayer, Localization.Get("xuiCraftQueueFull", false, null), false, false, 0f);
		Manager.PlayInsidePlayerHead("ui_denied", -1, 0f, false, false);
	}

	// Token: 0x06006829 RID: 26665 RVA: 0x00294EA4 File Offset: 0x002930A4
	[PublicizedFrom(EAccessModifier.Private)]
	public void ItemStackController_TimeIntervalElapsedEvent(float _timeLeft, XUiC_ItemStack _uiItemStack)
	{
		EntityPlayerLocal entityPlayer = base.ItemController.xui.playerUI.entityPlayer;
		XUiM_PlayerInventory playerInventory = base.ItemController.xui.PlayerInventory;
		XUiC_ItemStack xuiC_ItemStack = (XUiC_ItemStack)base.ItemController;
		ItemStack itemStack = xuiC_ItemStack.ItemStack.Clone();
		itemStack.count -= (int)this.numberOfCurrentItemsNeededFor1StackOfOutputItem;
		xuiC_ItemStack.ItemStack = itemStack;
		ItemStack itemStack2 = new ItemStack(new ItemValue(this.recipe.itemValueType, false), (int)this.scrapItemCount);
		if (!playerInventory.AddItem(itemStack2))
		{
			GameManager.Instance.ItemDropServer(itemStack2, entityPlayer.GetPosition(), new Vector3(0.5f, 0f, 0.5f), -1, 60f, false);
		}
	}

	// Token: 0x0600682A RID: 26666 RVA: 0x00294F5C File Offset: 0x0029315C
	[PublicizedFrom(EAccessModifier.Private)]
	public void ItemStackController_LockChangedEvent(XUiC_ItemStack.LockTypes _lockType, XUiC_ItemStack _uiItemStack)
	{
		XUiC_ItemStack xuiC_ItemStack = (XUiC_ItemStack)base.ItemController;
		if (xuiC_ItemStack.ItemStack.count == 0)
		{
			xuiC_ItemStack.ItemStack = ItemStack.Empty;
		}
		xuiC_ItemStack.TimeIntervalElapsedEvent -= this.ItemStackController_TimeIntervalElapsedEvent;
		xuiC_ItemStack.UnlockStack();
		xuiC_ItemStack.LockChangedEvent -= this.ItemStackController_LockChangedEvent;
	}

	// Token: 0x0600682B RID: 26667 RVA: 0x00294FB8 File Offset: 0x002931B8
	public override void RefreshEnabled()
	{
		ItemStack itemStack = ((IItemStackController)base.ItemController).ItemStack;
		if (itemStack.itemValue.ModificationCount > 0)
		{
			for (int i = 0; i < itemStack.itemValue.ModificationCount; i++)
			{
				ItemValue modification = itemStack.itemValue.GetModification(i);
				if (modification != null && !modification.IsEmpty() && ((ItemClassModifier)modification.ItemClass).Type == ItemClassModifier.ModifierTypes.Attachment)
				{
					base.Enabled = false;
					return;
				}
			}
		}
		base.Enabled = true;
	}

	// Token: 0x0600682C RID: 26668 RVA: 0x00295034 File Offset: 0x00293234
	public override void OnDisabledActivate()
	{
		GameManager.ShowTooltip(base.ItemController.xui.playerUI.entityPlayer, Localization.Get("ttCannotScrapWithAttachments", false, null), false, false, 0f);
	}

	// Token: 0x0600682D RID: 26669 RVA: 0x00295064 File Offset: 0x00293264
	public static ItemStack HandleRemoveAmmo(ItemStack _stack, XUi _xui)
	{
		if (_stack.itemValue.Meta <= 0)
		{
			return _stack;
		}
		ItemClass forId = ItemClass.GetForId(_stack.itemValue.type);
		for (int i = 0; i < forId.Actions.Length; i++)
		{
			ItemActionRanged itemActionRanged = forId.Actions[i] as ItemActionRanged;
			if (itemActionRanged != null && !(forId.Actions[i] is ItemActionTextureBlock) && itemActionRanged.MagazineItemNames != null && (int)_stack.itemValue.SelectedAmmoTypeIndex < itemActionRanged.MagazineItemNames.Length)
			{
				ItemStack itemStack = new ItemStack(ItemClass.GetItem(itemActionRanged.MagazineItemNames[(int)_stack.itemValue.SelectedAmmoTypeIndex], false), _stack.itemValue.Meta);
				if (!_xui.PlayerInventory.AddItem(itemStack))
				{
					_xui.PlayerInventory.DropItem(itemStack);
				}
				_stack.itemValue.Meta = 0;
			}
		}
		return _stack;
	}

	// Token: 0x0400503D RID: 20541
	[PublicizedFrom(EAccessModifier.Private)]
	public Recipe recipe;

	// Token: 0x0400503E RID: 20542
	[PublicizedFrom(EAccessModifier.Private)]
	public static FastTags<TagGroup.Global> scrap100Tag = FastTags<TagGroup.Global>.Parse("scrap100");

	// Token: 0x0400503F RID: 20543
	[PublicizedFrom(EAccessModifier.Private)]
	public float scrapItemCount = 1f;

	// Token: 0x04005040 RID: 20544
	[PublicizedFrom(EAccessModifier.Private)]
	public float numberOfCurrentItemsNeededFor1StackOfOutputItem = -1f;
}
