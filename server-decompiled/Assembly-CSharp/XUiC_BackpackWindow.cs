using System;
using UnityEngine.Scripting;

// Token: 0x02000EDE RID: 3806
[Preserve]
public class XUiC_BackpackWindow : XUiController
{
	// Token: 0x06007643 RID: 30275 RVA: 0x002C9FB8 File Offset: 0x002C81B8
	public override void Init()
	{
		base.Init();
		this.backpackGrid = base.GetChildByType<XUiC_Backpack>();
		this.standardControls = base.GetChildByType<XUiC_ContainerStandardControls>();
		if (this.standardControls != null)
		{
			this.standardControls.GetLockedSlotsFromStorage = () => this.xui.playerUI.entityPlayer.bag.LockedSlots;
			this.standardControls.SetLockedSlotsToStorage = delegate(PackedBoolArray _slots)
			{
				this.xui.playerUI.entityPlayer.bag.LockedSlots.CopyFrom(_slots);
			};
			this.standardControls.ApplyLockedSlotStates = new Action<PackedBoolArray>(this.ApplyLockedSlotStates);
			this.standardControls.LockModeToggled = delegate(bool _)
			{
				this.IsDirty = true;
			};
			this.standardControls.SearchFiltersChanged = delegate(XUiC_ContainerStandardControls.MatchesFiltersDelegate filter)
			{
				this.backpackGrid.MatchesFiltersFunc = filter;
			};
			this.standardControls.SortPressed = new Action<PackedBoolArray>(this.BtnSort_OnPress);
			this.standardControls.MoveAllowed = delegate(out XUiController _parentWindow, out XUiC_ItemStackGrid _grid, out IInventory _inventory)
			{
				_parentWindow = this;
				_grid = this.backpackGrid;
				return this.TryGetMoveDestinationInventory(out _inventory);
			};
		}
		XUiController childById = base.GetChildById("btnClearInventory");
		if (childById != null)
		{
			childById.OnPress += this.BtnClearInventory_OnPress;
		}
	}

	// Token: 0x06007644 RID: 30276 RVA: 0x002CA0B0 File Offset: 0x002C82B0
	[PublicizedFrom(EAccessModifier.Private)]
	public bool TryGetMoveDestinationInventory(out IInventory _dstInventory)
	{
		_dstInventory = null;
		XUiM_AssembleItem assembleItem = this.xui.AssembleItem;
		if (((assembleItem != null) ? assembleItem.CurrentItem : null) != null)
		{
			return false;
		}
		if (this.xui.LootContainer != null)
		{
			_dstInventory = this.xui.LootContainer;
			return true;
		}
		XUiC_BagStorageWindowGroup xuiC_BagStorageWindowGroup = this.xui.FindWindowGroupByName(XUiC_BagStorageWindowGroup.ID) as XUiC_BagStorageWindowGroup;
		if (xuiC_BagStorageWindowGroup != null && xuiC_BagStorageWindowGroup.Bag != null)
		{
			_dstInventory = xuiC_BagStorageWindowGroup.Bag;
			return true;
		}
		return false;
	}

	// Token: 0x06007645 RID: 30277 RVA: 0x002CA12C File Offset: 0x002C832C
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnClearInventory_OnPress(XUiController _sender, int _mouseButton)
	{
		this.xui.playerUI.entityPlayer.EmptyBackpack();
	}

	// Token: 0x06007646 RID: 30278 RVA: 0x002CA144 File Offset: 0x002C8344
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnSort_OnPress(PackedBoolArray _ignoredSlots)
	{
		ItemStack itemStack = null;
		if (this.xui.AssembleItem.CurrentItemStackController != null)
		{
			itemStack = this.xui.AssembleItem.CurrentItemStackController.ItemStack;
		}
		this.xui.PlayerInventory.SortStacks();
		if (itemStack != null)
		{
			base.GetChildByType<XUiC_ItemStackGrid>().AssembleLockSingleStack(itemStack);
		}
	}

	// Token: 0x06007647 RID: 30279 RVA: 0x002CA19C File Offset: 0x002C839C
	[PublicizedFrom(EAccessModifier.Private)]
	public void ApplyLockedSlotStates(PackedBoolArray _lockedSlots)
	{
		XUiC_ItemStack[] itemStackControllers = this.backpackGrid.GetItemStackControllers();
		for (int i = 0; i < itemStackControllers.Length; i++)
		{
			itemStackControllers[i].UserLockedSlot = _lockedSlots != null && i < _lockedSlots.Length && _lockedSlots[i];
		}
	}

	// Token: 0x06007648 RID: 30280 RVA: 0x002CA1E4 File Offset: 0x002C83E4
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateLockedSlots(XUiC_ContainerStandardControls _csc)
	{
		if (_csc == null)
		{
			return;
		}
		int slotCount = this.xui.PlayerInventory.Backpack.SlotCount;
		PackedBoolArray packedBoolArray = _csc.LockedSlots ?? new PackedBoolArray(slotCount);
		if (packedBoolArray.Length != slotCount)
		{
			packedBoolArray.Length = slotCount;
		}
		XUiC_ItemStack[] itemStackControllers = this.backpackGrid.GetItemStackControllers();
		int num = 0;
		while (num < itemStackControllers.Length && num < packedBoolArray.Length)
		{
			packedBoolArray[num] = itemStackControllers[num].UserLockedSlot;
			num++;
		}
		_csc.LockedSlots = packedBoolArray;
	}

	// Token: 0x06007649 RID: 30281 RVA: 0x002CA268 File Offset: 0x002C8468
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string value, string bindingName)
	{
		if (bindingName == "currencyamount")
		{
			value = "0";
			if (XUi.IsGameRunning() && this.xui != null && this.xui.PlayerInventory != null)
			{
				value = this.currencyFormatter.Format(this.xui.PlayerInventory.CurrencyAmount);
			}
			return true;
		}
		if (bindingName == "currencyicon")
		{
			value = TraderInfo.CurrencyItem;
			return true;
		}
		if (bindingName == "lootingorvehiclestorage")
		{
			IInventory inventory;
			value = this.TryGetMoveDestinationInventory(out inventory).ToString();
			return true;
		}
		if (bindingName == "creativewindowopen")
		{
			value = this.xui.playerUI.windowManager.IsWindowOpen("creative").ToString();
			return true;
		}
		if (!(bindingName == "userlockmode"))
		{
			return base.GetBindingValueInternal(ref value, bindingName);
		}
		XUiC_ContainerStandardControls xuiC_ContainerStandardControls = this.standardControls;
		value = (xuiC_ContainerStandardControls != null && xuiC_ContainerStandardControls.UserLockMode).ToString();
		return true;
	}

	// Token: 0x0600764A RID: 30282 RVA: 0x002CA370 File Offset: 0x002C8570
	public override void OnOpen()
	{
		base.OnOpen();
		this.xui.PlayerInventory.RefreshCurrency();
		this.xui.PlayerInventory.OnCurrencyChanged += this.PlayerInventory_OnCurrencyChanged;
		if (!string.IsNullOrEmpty(XUiC_BackpackWindow.defaultSelectedElement))
		{
			base.GetChildById(XUiC_BackpackWindow.defaultSelectedElement).SelectCursorElement(true, false);
			XUiC_BackpackWindow.defaultSelectedElement = "";
		}
		GameManager.Instance.StartCoroutine(this.xui.playerUI.entityPlayer.CancelInventoryActions(delegate
		{
		}, false));
	}

	// Token: 0x0600764B RID: 30283 RVA: 0x002CA418 File Offset: 0x002C8618
	public override void OnClose()
	{
		base.OnClose();
		if (this.xui != null && this.xui.PlayerInventory != null)
		{
			this.xui.PlayerInventory.OnCurrencyChanged -= this.PlayerInventory_OnCurrencyChanged;
		}
	}

	// Token: 0x0600764C RID: 30284 RVA: 0x002926AB File Offset: 0x002908AB
	public override void Update(float _dt)
	{
		base.Update(_dt);
		base.handleDirtyUpdateDefault();
	}

	// Token: 0x0600764D RID: 30285 RVA: 0x0008B76F File Offset: 0x0008996F
	[PublicizedFrom(EAccessModifier.Private)]
	public void PlayerInventory_OnCurrencyChanged()
	{
		base.RefreshBindings();
	}

	// Token: 0x040057B0 RID: 22448
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_Backpack backpackGrid;

	// Token: 0x040057B1 RID: 22449
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isHidden;

	// Token: 0x040057B2 RID: 22450
	[PublicizedFrom(EAccessModifier.Private)]
	public bool userLockMode;

	// Token: 0x040057B3 RID: 22451
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ContainerStandardControls standardControls;

	// Token: 0x040057B4 RID: 22452
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterInt currencyFormatter = new CachedStringFormatterInt();

	// Token: 0x040057B5 RID: 22453
	public static string defaultSelectedElement;
}
