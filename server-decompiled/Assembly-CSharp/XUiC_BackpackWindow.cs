using System;
using UnityEngine.Scripting;

// Token: 0x02000EAF RID: 3759
[Preserve]
public class XUiC_BackpackWindow : XUiController
{
	// Token: 0x17000DCF RID: 3535
	// (get) Token: 0x06007581 RID: 30081 RVA: 0x002C210F File Offset: 0x002C030F
	// (set) Token: 0x06007582 RID: 30082 RVA: 0x002C2118 File Offset: 0x002C0318
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

	// Token: 0x06007583 RID: 30083 RVA: 0x002C219C File Offset: 0x002C039C
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

	// Token: 0x06007584 RID: 30084 RVA: 0x002C2294 File Offset: 0x002C0494
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

	// Token: 0x06007585 RID: 30085 RVA: 0x002C2310 File Offset: 0x002C0510
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnClearInventory_OnPress(XUiController _sender, int _mouseButton)
	{
		this.xui.playerUI.entityPlayer.EmptyBackpack();
	}

	// Token: 0x06007586 RID: 30086 RVA: 0x002C2328 File Offset: 0x002C0528
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

	// Token: 0x06007587 RID: 30087 RVA: 0x002C2380 File Offset: 0x002C0580
	[PublicizedFrom(EAccessModifier.Private)]
	public void ApplyLockedSlotStates(PackedBoolArray _lockedSlots)
	{
		XUiC_ItemStack[] itemStackControllers = this.backpackGrid.GetItemStackControllers();
		for (int i = 0; i < itemStackControllers.Length; i++)
		{
			itemStackControllers[i].UserLockedSlot = _lockedSlots != null && i < _lockedSlots.Length && _lockedSlots[i];
		}
	}

	// Token: 0x06007588 RID: 30088 RVA: 0x002C23C8 File Offset: 0x002C05C8
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

	// Token: 0x06007589 RID: 30089 RVA: 0x002C244C File Offset: 0x002C064C
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

	// Token: 0x0600758A RID: 30090 RVA: 0x002C2548 File Offset: 0x002C0748
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

	// Token: 0x0600758B RID: 30091 RVA: 0x002C25F0 File Offset: 0x002C07F0
	public override void OnClose()
	{
		base.OnClose();
		this.UserLockMode = false;
		if (this.xui != null && this.xui.PlayerInventory != null)
		{
			this.xui.PlayerInventory.OnCurrencyChanged -= this.PlayerInventory_OnCurrencyChanged;
		}
	}

	// Token: 0x0600758C RID: 30092 RVA: 0x002C2641 File Offset: 0x002C0841
	public override void Update(float _dt)
	{
		base.Update(_dt);
		this.UpdateInput();
	}

	// Token: 0x0600758D RID: 30093 RVA: 0x002C2650 File Offset: 0x002C0850
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateInput()
	{
		PlayerActionsLocal playerInput = this.xui.playerUI.playerInput;
		if (this.UserLockMode && (playerInput.GUIActions.Cancel.WasPressed || playerInput.PermanentActions.Cancel.WasPressed))
		{
			this.UserLockMode = false;
		}
	}

	// Token: 0x0600758E RID: 30094 RVA: 0x0008AB7B File Offset: 0x00088D7B
	[PublicizedFrom(EAccessModifier.Private)]
	public void PlayerInventory_OnCurrencyChanged()
	{
		base.RefreshBindings();
	}

	// Token: 0x040056AE RID: 22190
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_Backpack backpackGrid;

	// Token: 0x040056AF RID: 22191
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isHidden;

	// Token: 0x040056B0 RID: 22192
	[PublicizedFrom(EAccessModifier.Private)]
	public bool userLockMode;

	// Token: 0x040056B1 RID: 22193
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ContainerStandardControls standardControls;

	// Token: 0x040056B2 RID: 22194
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterInt currencyFormatter = new CachedStringFormatterInt();

	// Token: 0x040056B3 RID: 22195
	public static string defaultSelectedElement;
}
