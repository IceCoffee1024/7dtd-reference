using System;
using UnityEngine.Scripting;

// Token: 0x02000EC4 RID: 3780
[Preserve]
public class XUiC_BackpackWindow : XUiController
{
	// Token: 0x17000DE9 RID: 3561
	// (get) Token: 0x06007622 RID: 30242 RVA: 0x002C5633 File Offset: 0x002C3833
	// (set) Token: 0x06007623 RID: 30243 RVA: 0x002C563C File Offset: 0x002C383C
	public bool UserLockMode
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.userLockMode;
		}
		[PublicizedFrom(EAccessModifier.Private)]
		set
		{
			if (value == this.userLockMode)
			{
				return;
			}
			if (this.userLockMode)
			{
				this.UpdateLockedSlots(this.standardControls);
			}
			XUiC_ContainerStandardControls xuiC_ContainerStandardControls = this.standardControls;
			if (xuiC_ContainerStandardControls != null)
			{
				xuiC_ContainerStandardControls.LockModeChanged(value);
			}
			this.userLockMode = value;
			base.WindowGroup.isEscClosable = !this.userLockMode;
			this.xui.playerUI.windowManager.GetModalWindow().isEscClosable = !this.userLockMode;
			base.RefreshBindings();
		}
	}

	// Token: 0x06007624 RID: 30244 RVA: 0x002C56C0 File Offset: 0x002C38C0
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
				this.xui.playerUI.entityPlayer.bag.LockedSlots = _slots;
			};
			this.standardControls.ApplyLockedSlotStates = new Action<PackedBoolArray>(this.ApplyLockedSlotStates);
			this.standardControls.UpdateLockedSlotStates = new Action<XUiC_ContainerStandardControls>(this.UpdateLockedSlots);
			this.standardControls.LockModeToggled = delegate
			{
				this.UserLockMode = !this.UserLockMode;
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

	// Token: 0x06007625 RID: 30245 RVA: 0x002C57B8 File Offset: 0x002C39B8
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

	// Token: 0x06007626 RID: 30246 RVA: 0x002C5834 File Offset: 0x002C3A34
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnClearInventory_OnPress(XUiController _sender, int _mouseButton)
	{
		this.xui.playerUI.entityPlayer.EmptyBackpack();
	}

	// Token: 0x06007627 RID: 30247 RVA: 0x002C584C File Offset: 0x002C3A4C
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnSort_OnPress(PackedBoolArray _ignoredSlots)
	{
		ItemStack itemStack = null;
		if (this.xui.AssembleItem.CurrentItemStackController != null)
		{
			itemStack = this.xui.AssembleItem.CurrentItemStackController.ItemStack;
		}
		this.xui.PlayerInventory.SortStacks(0, _ignoredSlots);
		if (itemStack != null)
		{
			base.GetChildByType<XUiC_ItemStackGrid>().AssembleLockSingleStack(itemStack);
		}
	}

	// Token: 0x06007628 RID: 30248 RVA: 0x002C58A4 File Offset: 0x002C3AA4
	[PublicizedFrom(EAccessModifier.Private)]
	public void ApplyLockedSlotStates(PackedBoolArray _lockedSlots)
	{
		XUiC_ItemStack[] itemStackControllers = this.backpackGrid.GetItemStackControllers();
		for (int i = 0; i < itemStackControllers.Length; i++)
		{
			itemStackControllers[i].UserLockedSlot = _lockedSlots != null && i < _lockedSlots.Length && _lockedSlots[i];
		}
	}

	// Token: 0x06007629 RID: 30249 RVA: 0x002C58EC File Offset: 0x002C3AEC
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

	// Token: 0x0600762A RID: 30250 RVA: 0x002C5970 File Offset: 0x002C3B70
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
		value = this.UserLockMode.ToString();
		return true;
	}

	// Token: 0x0600762B RID: 30251 RVA: 0x002C5A6C File Offset: 0x002C3C6C
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

	// Token: 0x0600762C RID: 30252 RVA: 0x002C5B14 File Offset: 0x002C3D14
	public override void OnClose()
	{
		base.OnClose();
		this.UserLockMode = false;
		if (this.xui != null && this.xui.PlayerInventory != null)
		{
			this.xui.PlayerInventory.OnCurrencyChanged -= this.PlayerInventory_OnCurrencyChanged;
		}
	}

	// Token: 0x0600762D RID: 30253 RVA: 0x002C5B65 File Offset: 0x002C3D65
	public override void Update(float _dt)
	{
		base.Update(_dt);
		this.UpdateInput();
	}

	// Token: 0x0600762E RID: 30254 RVA: 0x002C5B74 File Offset: 0x002C3D74
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateInput()
	{
		PlayerActionsLocal playerInput = this.xui.playerUI.playerInput;
		if (this.UserLockMode && (playerInput.GUIActions.Cancel.WasPressed || playerInput.PermanentActions.Cancel.WasPressed))
		{
			this.UserLockMode = false;
		}
	}

	// Token: 0x0600762F RID: 30255 RVA: 0x0008B2BB File Offset: 0x000894BB
	[PublicizedFrom(EAccessModifier.Private)]
	public void PlayerInventory_OnCurrencyChanged()
	{
		base.RefreshBindings();
	}

	// Token: 0x04005753 RID: 22355
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_Backpack backpackGrid;

	// Token: 0x04005754 RID: 22356
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isHidden;

	// Token: 0x04005755 RID: 22357
	[PublicizedFrom(EAccessModifier.Private)]
	public bool userLockMode;

	// Token: 0x04005756 RID: 22358
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ContainerStandardControls standardControls;

	// Token: 0x04005757 RID: 22359
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterInt currencyFormatter = new CachedStringFormatterInt();

	// Token: 0x04005758 RID: 22360
	public static string defaultSelectedElement;
}
