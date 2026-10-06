using System;
using UnityEngine.Scripting;

// Token: 0x02000ED3 RID: 3795
[Preserve]
public class XUiC_BackpackWindow : XUiController
{
	// Token: 0x17000E05 RID: 3589
	// (get) Token: 0x0600761D RID: 30237 RVA: 0x002C8A23 File Offset: 0x002C6C23
	// (set) Token: 0x0600761E RID: 30238 RVA: 0x002C8A2C File Offset: 0x002C6C2C
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

	// Token: 0x0600761F RID: 30239 RVA: 0x002C8AB0 File Offset: 0x002C6CB0
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

	// Token: 0x06007620 RID: 30240 RVA: 0x002C8BA8 File Offset: 0x002C6DA8
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

	// Token: 0x06007621 RID: 30241 RVA: 0x002C8C24 File Offset: 0x002C6E24
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnClearInventory_OnPress(XUiController _sender, int _mouseButton)
	{
		this.xui.playerUI.entityPlayer.EmptyBackpack();
	}

	// Token: 0x06007622 RID: 30242 RVA: 0x002C8C3C File Offset: 0x002C6E3C
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

	// Token: 0x06007623 RID: 30243 RVA: 0x002C8C94 File Offset: 0x002C6E94
	[PublicizedFrom(EAccessModifier.Private)]
	public void ApplyLockedSlotStates(PackedBoolArray _lockedSlots)
	{
		XUiC_ItemStack[] itemStackControllers = this.backpackGrid.GetItemStackControllers();
		for (int i = 0; i < itemStackControllers.Length; i++)
		{
			itemStackControllers[i].UserLockedSlot = _lockedSlots != null && i < _lockedSlots.Length && _lockedSlots[i];
		}
	}

	// Token: 0x06007624 RID: 30244 RVA: 0x002C8CDC File Offset: 0x002C6EDC
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

	// Token: 0x06007625 RID: 30245 RVA: 0x002C8D60 File Offset: 0x002C6F60
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

	// Token: 0x06007626 RID: 30246 RVA: 0x002C8E5C File Offset: 0x002C705C
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

	// Token: 0x06007627 RID: 30247 RVA: 0x002C8F04 File Offset: 0x002C7104
	public override void OnClose()
	{
		base.OnClose();
		this.UserLockMode = false;
		if (this.xui != null && this.xui.PlayerInventory != null)
		{
			this.xui.PlayerInventory.OnCurrencyChanged -= this.PlayerInventory_OnCurrencyChanged;
		}
	}

	// Token: 0x06007628 RID: 30248 RVA: 0x002C8F55 File Offset: 0x002C7155
	public override void Update(float _dt)
	{
		base.Update(_dt);
		this.UpdateInput();
	}

	// Token: 0x06007629 RID: 30249 RVA: 0x002C8F64 File Offset: 0x002C7164
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateInput()
	{
		PlayerActionsLocal playerInput = this.xui.playerUI.playerInput;
		if (this.UserLockMode && (playerInput.GUIActions.Cancel.WasPressed || playerInput.PermanentActions.Cancel.WasPressed))
		{
			this.UserLockMode = false;
		}
	}

	// Token: 0x0600762A RID: 30250 RVA: 0x0008B53B File Offset: 0x0008973B
	[PublicizedFrom(EAccessModifier.Private)]
	public void PlayerInventory_OnCurrencyChanged()
	{
		base.RefreshBindings();
	}

	// Token: 0x0400578B RID: 22411
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_Backpack backpackGrid;

	// Token: 0x0400578C RID: 22412
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isHidden;

	// Token: 0x0400578D RID: 22413
	[PublicizedFrom(EAccessModifier.Private)]
	public bool userLockMode;

	// Token: 0x0400578E RID: 22414
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ContainerStandardControls standardControls;

	// Token: 0x0400578F RID: 22415
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterInt currencyFormatter = new CachedStringFormatterInt();

	// Token: 0x04005790 RID: 22416
	public static string defaultSelectedElement;
}
