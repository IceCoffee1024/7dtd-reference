using System;
using Audio;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000D4E RID: 3406
[Preserve]
public class ItemActionEntryScrap : BaseItemActionEntry
{
	// Token: 0x06006790 RID: 26512 RVA: 0x0028E686 File Offset: 0x0028C886
	public ItemActionEntryScrap(XUiController _controller)
		: base(_controller, "lblContextActionScrap", "ui_game_symbol_scrap", BaseItemActionEntry.GamepadShortCut.DPadRight, "crafting/craft_click_craft", "ui/ui_denied")
	{
	}

	// Token: 0x06006791 RID: 26513 RVA: 0x0028E6BC File Offset: 0x0028C8BC
	public override void OnActivated()
	{
		XUi xui = base.ItemController.xui;
		XUiC_ItemStack xuiC_ItemStack = (XUiC_ItemStack)base.ItemController;
		xuiC_ItemStack.LockChangedEvent += this.ItemStackController_LockChangedEvent;
		ItemStack itemStack = xuiC_ItemStack.ItemStack.Clone();
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
		((XUiC_ItemStack)base.ItemController).ItemStack = ((itemStack.count <= 0) ? ItemStack.Empty : itemStack.Clone());
		((XUiC_ItemStack)base.ItemController).WindowGroup.Controller.SetAllChildrenDirty();
	}

	// Token: 0x06006792 RID: 26514 RVA: 0x0028E908 File Offset: 0x0028CB08
	[PublicizedFrom(EAccessModifier.Private)]
	public void warnQueueFull()
	{
		GameManager.ShowTooltip(base.ItemController.xui.playerUI.entityPlayer, Localization.Get("xuiCraftQueueFull", false, null), false, false, 0f);
		Manager.PlayInsidePlayerHead("ui_denied", -1, 0f, false, false);
	}

	// Token: 0x06006793 RID: 26515 RVA: 0x0028E954 File Offset: 0x0028CB54
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

	// Token: 0x06006794 RID: 26516 RVA: 0x0028EA0C File Offset: 0x0028CC0C
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

	// Token: 0x06006795 RID: 26517 RVA: 0x0028EA68 File Offset: 0x0028CC68
	public override void RefreshEnabled()
	{
		ItemStack itemStack = ((XUiC_ItemStack)base.ItemController).ItemStack;
		if (itemStack.itemValue.Modifications.Length != 0)
		{
			for (int i = 0; i < itemStack.itemValue.Modifications.Length; i++)
			{
				ItemValue itemValue = itemStack.itemValue.Modifications[i];
				if (itemValue != null && !itemValue.IsEmpty() && ((ItemClassModifier)itemValue.ItemClass).Type == ItemClassModifier.ModifierTypes.Attachment)
				{
					base.Enabled = false;
					return;
				}
			}
		}
		base.Enabled = true;
	}

	// Token: 0x06006796 RID: 26518 RVA: 0x0028EAE7 File Offset: 0x0028CCE7
	public override void OnDisabledActivate()
	{
		GameManager.ShowTooltip(base.ItemController.xui.playerUI.entityPlayer, Localization.Get("ttCannotScrapWithAttachments", false, null), false, false, 0f);
	}

	// Token: 0x06006797 RID: 26519 RVA: 0x0028EB18 File Offset: 0x0028CD18
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

	// Token: 0x04004F60 RID: 20320
	[PublicizedFrom(EAccessModifier.Private)]
	public Recipe recipe;

	// Token: 0x04004F61 RID: 20321
	[PublicizedFrom(EAccessModifier.Private)]
	public static FastTags<TagGroup.Global> scrap100Tag = FastTags<TagGroup.Global>.Parse("scrap100");

	// Token: 0x04004F62 RID: 20322
	[PublicizedFrom(EAccessModifier.Private)]
	public float scrapItemCount = 1f;

	// Token: 0x04004F63 RID: 20323
	[PublicizedFrom(EAccessModifier.Private)]
	public float numberOfCurrentItemsNeededFor1StackOfOutputItem = -1f;
}
