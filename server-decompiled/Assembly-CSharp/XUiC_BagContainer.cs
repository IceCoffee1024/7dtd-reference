using System;
using System.Collections;
using System.Globalization;
using Audio;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000EC6 RID: 3782
[Preserve]
public class XUiC_BagContainer : XUiC_ItemStackGrid
{
	// Token: 0x17000DEA RID: 3562
	// (get) Token: 0x06007638 RID: 30264 RVA: 0x002C5C42 File Offset: 0x002C3E42
	// (set) Token: 0x06007639 RID: 30265 RVA: 0x002C5C4A File Offset: 0x002C3E4A
	public Bag Bag
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000DEB RID: 3563
	// (get) Token: 0x0600763A RID: 30266 RVA: 0x002C5C53 File Offset: 0x002C3E53
	// (set) Token: 0x0600763B RID: 30267 RVA: 0x002C5C5B File Offset: 0x002C3E5B
	public Vector2i GridCellSize
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000DEC RID: 3564
	// (get) Token: 0x0600763C RID: 30268 RVA: 0x00080719 File Offset: 0x0007E919
	public override XUiC_ItemStack.StackLocationTypes StackLocation
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return XUiC_ItemStack.StackLocationTypes.Vehicle;
		}
	}

	// Token: 0x17000DED RID: 3565
	// (get) Token: 0x0600763D RID: 30269 RVA: 0x002C5C64 File Offset: 0x002C3E64
	// (set) Token: 0x0600763E RID: 30270 RVA: 0x002C5C6C File Offset: 0x002C3E6C
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
			base.RefreshBindings();
		}
	}

	// Token: 0x0600763F RID: 30271 RVA: 0x002C5CCA File Offset: 0x002C3ECA
	public override ItemStack[] GetSlots()
	{
		return this.items;
	}

	// Token: 0x06007640 RID: 30272 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void UpdateBackend(ItemStack[] _stackList)
	{
	}

	// Token: 0x06007641 RID: 30273 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void SetStacks(ItemStack[] _stackList)
	{
	}

	// Token: 0x06007642 RID: 30274 RVA: 0x002C5CD4 File Offset: 0x002C3ED4
	public override void Init()
	{
		base.Init();
		this.window = (XUiV_Window)this.viewComponent;
		this.grid = (XUiV_Grid)base.GetChildById("queue").ViewComponent;
		this.GridCellSize = new Vector2i(this.grid.CellWidth, this.grid.CellHeight);
		this.standardControls = base.GetChildByType<XUiC_ContainerStandardControls>();
		if (this.standardControls != null)
		{
			this.standardControls.GetLockedSlotsFromStorage = delegate
			{
				Bag bag = this.Bag;
				PackedBoolArray packedBoolArray;
				if ((packedBoolArray = bag.LockedSlots) == null)
				{
					packedBoolArray = (bag.LockedSlots = new PackedBoolArray(this.Bag.SlotCount));
				}
				return packedBoolArray;
			};
			this.standardControls.SetLockedSlotsToStorage = delegate(PackedBoolArray _slots)
			{
				this.Bag.LockedSlots = _slots;
			};
			this.standardControls.ApplyLockedSlotStates = new Action<PackedBoolArray>(this.ApplyLockedSlotStates);
			this.standardControls.UpdateLockedSlotStates = new Action<XUiC_ContainerStandardControls>(this.UpdateLockedSlots);
			this.standardControls.LockModeToggled = delegate
			{
				this.UserLockMode = !this.UserLockMode;
			};
			this.standardControls.SortPressed = new Action<PackedBoolArray>(this.btnSort_OnPress);
			this.standardControls.MoveAllowed = delegate(out XUiController _parentWindow, out XUiC_ItemStackGrid _grid, out IInventory _inventory)
			{
				_parentWindow = this;
				_grid = this;
				_inventory = this.xui.PlayerInventory;
				return true;
			};
			this.standardControls.MoveAllDone = delegate(bool _allMoved, bool _anyMoved)
			{
				if (_anyMoved)
				{
					Manager.BroadcastPlayByLocalPlayer(XUiM_Player.GetPlayer().position, "UseActions/takeall1");
				}
				if (_allMoved)
				{
					ThreadManager.StartCoroutine(this.closeInventoryLater());
				}
			};
		}
	}

	// Token: 0x06007643 RID: 30275 RVA: 0x002C5E04 File Offset: 0x002C4004
	[PublicizedFrom(EAccessModifier.Private)]
	public void btnSort_OnPress(PackedBoolArray _ignoredSlots)
	{
		if (this.Bag == null)
		{
			return;
		}
		ItemStack[] array = StackSortUtil.CombineAndSortStacks(this.Bag.GetSlots(), 0, _ignoredSlots);
		this.Bag.SetSlots(array);
	}

	// Token: 0x06007644 RID: 30276 RVA: 0x002C5E3C File Offset: 0x002C403C
	public void SetBag(Bag _bag, LootContainer _lootContainer, string _containerName, Action _onModified = null)
	{
		if (_bag == null || _lootContainer == null)
		{
			return;
		}
		this.Bag = _bag;
		this.containerName = _containerName;
		this.onBagModified = _onModified;
		this.grid.Columns = _lootContainer.size.x;
		this.grid.Rows = _lootContainer.size.y;
		ItemStack[] slots = _bag.GetSlots();
		int num = Mathf.CeilToInt((float)slots.Length / (float)this.grid.Columns);
		Vector2i vector2i = new Vector2i(this.grid.Columns * this.GridCellSize.x, num * this.GridCellSize.y);
		this.windowWidth = vector2i.x + this.windowGridWidthDifference;
		_bag.OnBackpackItemsChangedInternal += this.OnBagItemChangedInternal;
		this.items = slots;
		XUiC_ItemInfoWindow childByType = this.xui.GetChildByType<XUiC_ItemInfoWindow>();
		int num2 = slots.Length;
		for (int i = 0; i < this.itemControllers.Length; i++)
		{
			XUiC_ItemStack xuiC_ItemStack = this.itemControllers[i];
			xuiC_ItemStack.SlotNumber = i;
			xuiC_ItemStack.SlotChangedEvent -= this.HandleBagSlotChangedEvent;
			xuiC_ItemStack.InfoWindow = childByType;
			xuiC_ItemStack.StackLocation = XUiC_ItemStack.StackLocationTypes.LootContainer;
			xuiC_ItemStack.UnlockStack();
			if (i < num2)
			{
				xuiC_ItemStack.ForceSetItemStack(this.items[i]);
				this.itemControllers[i].ViewComponent.IsVisible = true;
				xuiC_ItemStack.SlotChangedEvent += this.HandleBagSlotChangedEvent;
			}
			else
			{
				xuiC_ItemStack.ItemStack = ItemStack.Empty;
				this.itemControllers[i].ViewComponent.IsVisible = false;
			}
		}
		base.RefreshBindings();
	}

	// Token: 0x06007645 RID: 30277 RVA: 0x002C5FD8 File Offset: 0x002C41D8
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateInput()
	{
		PlayerActionsLocal playerInput = this.xui.playerUI.playerInput;
		if (this.UserLockMode && (playerInput.GUIActions.Cancel.WasPressed || playerInput.PermanentActions.Cancel.WasPressed))
		{
			this.UserLockMode = false;
		}
	}

	// Token: 0x06007646 RID: 30278 RVA: 0x002C602C File Offset: 0x002C422C
	public override void Update(float _dt)
	{
		if (GameManager.Instance == null || GameManager.Instance.World == null)
		{
			return;
		}
		if (this.IsDirty)
		{
			base.ViewComponent.IsVisible = this.Bag != null;
			base.RefreshBindings();
			this.IsDirty = false;
		}
		base.Update(_dt);
		if (this.windowGroup.isShowing)
		{
			if (!this.xui.playerUI.playerInput.PermanentActions.Activate.IsPressed)
			{
				this.wasReleased = true;
			}
			if (this.wasReleased)
			{
				if (this.xui.playerUI.playerInput.PermanentActions.Activate.IsPressed)
				{
					this.activeKeyDown = true;
				}
				if (this.xui.playerUI.playerInput.PermanentActions.Activate.WasReleased && this.activeKeyDown)
				{
					this.activeKeyDown = false;
					this.OnClose();
					this.xui.playerUI.windowManager.CloseAllOpenModalWindows(null, false);
				}
			}
			if (!this.isClosing && base.ViewComponent != null && base.ViewComponent.IsVisible && this.items != null && (PlatformManager.NativePlatform.Input.CurrentInputStyle != PlayerInputManager.InputStyle.Keyboard || !this.xui.playerUI.windowManager.IsInputActive()) && (this.xui.playerUI.playerInput.GUIActions.LeftStick.WasPressed || this.xui.playerUI.playerInput.PermanentActions.Reload.WasPressed))
			{
				this.standardControls.MoveAll();
			}
		}
		this.UpdateInput();
	}

	// Token: 0x06007647 RID: 30279 RVA: 0x002C61E0 File Offset: 0x002C43E0
	public void HandleBagSlotChangedEvent(int _slotNumber, ItemStack _stack)
	{
		if (this.Bag == null)
		{
			return;
		}
		this.Bag.SetSlot(_slotNumber, _stack, true);
	}

	// Token: 0x06007648 RID: 30280 RVA: 0x002C61FC File Offset: 0x002C43FC
	public void OnBagItemChangedInternal()
	{
		if (this.Bag == null)
		{
			return;
		}
		ItemStack[] slots = this.Bag.GetSlots();
		for (int i = 0; i < slots.Length; i++)
		{
			this.SetItemInSlot(i, slots[i]);
		}
		Action action = this.onBagModified;
		if (action == null)
		{
			return;
		}
		action();
	}

	// Token: 0x06007649 RID: 30281 RVA: 0x002C6246 File Offset: 0x002C4446
	public void SetItemInSlot(int _i, ItemStack _stack)
	{
		if (_i >= this.itemControllers.Length)
		{
			return;
		}
		this.itemControllers[_i].ItemStack = _stack;
	}

	// Token: 0x0600764A RID: 30282 RVA: 0x002C6262 File Offset: 0x002C4462
	public override void OnOpen()
	{
		base.OnOpen();
		this.wasReleased = false;
		this.activeKeyDown = false;
		this.IsDirty = true;
	}

	// Token: 0x0600764B RID: 30283 RVA: 0x002C627F File Offset: 0x002C447F
	public override void OnClose()
	{
		base.OnClose();
		this.UserLockMode = false;
		if (this.Bag != null)
		{
			this.Bag.OnBackpackItemsChangedInternal -= this.OnBagItemChangedInternal;
		}
		this.Bag = null;
		this.onBagModified = null;
	}

	// Token: 0x0600764C RID: 30284 RVA: 0x002C62BB File Offset: 0x002C44BB
	[PublicizedFrom(EAccessModifier.Protected)]
	public IEnumerator closeInventoryLater()
	{
		this.isClosing = true;
		yield return null;
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
		this.isClosing = false;
		yield break;
	}

	// Token: 0x0600764D RID: 30285 RVA: 0x002C62CA File Offset: 0x002C44CA
	public bool AddItem(ItemStack _itemStack)
	{
		if (!_itemStack.CanMoveTo(XUiC_ItemStack.StackLocationTypes.Backpack, -1))
		{
			return false;
		}
		if (this.Bag == null)
		{
			return false;
		}
		this.Bag.TryStackItem(0, _itemStack);
		return _itemStack.count > 0 && this.Bag.AddItem(_itemStack);
	}

	// Token: 0x0600764E RID: 30286 RVA: 0x002C6307 File Offset: 0x002C4507
	public override bool ParseAttribute(string _name, string _value)
	{
		if (_name == "window_grid_width_difference")
		{
			this.windowGridWidthDifference = StringParsers.ParseSInt32(_value, 0, -1, NumberStyles.Integer);
			return true;
		}
		return base.ParseAttribute(_name, _value);
	}

	// Token: 0x0600764F RID: 30287 RVA: 0x002C6330 File Offset: 0x002C4530
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string _value, string _bindingName)
	{
		uint num = <PrivateImplementationDetails>.ComputeStringHash(_bindingName);
		if (num <= 775821220U)
		{
			if (num != 58930246U)
			{
				if (num != 610671175U)
				{
					if (num == 775821220U)
					{
						if (_bindingName == "lootcontainer_name")
						{
							_value = ((!string.IsNullOrEmpty(this.containerName)) ? this.containerName : Localization.Get("xuiStorage", false, null));
							return true;
						}
					}
				}
				else if (_bindingName == "windowWidth")
				{
					_value = this.windowWidth.ToString();
					return true;
				}
			}
			else if (_bindingName == "container_slots")
			{
				Bag bag = this.Bag;
				_value = ((bag != null) ? bag.SlotCount : 0).ToString();
				return true;
			}
		}
		else if (num <= 1899514017U)
		{
			if (num != 1249315359U)
			{
				if (num == 1899514017U)
				{
					if (_bindingName == "hasslotlocks")
					{
						XUiC_ContainerStandardControls xuiC_ContainerStandardControls = this.standardControls;
						_value = (((xuiC_ContainerStandardControls != null) ? xuiC_ContainerStandardControls.LockedSlots : null) != null).ToString();
						return true;
					}
				}
			}
			else if (_bindingName == "buttons_visible")
			{
				_value = (this.windowWidth >= 450).ToString();
				return true;
			}
		}
		else if (num != 2305669482U)
		{
			if (num == 4041168338U)
			{
				if (_bindingName == "take_all_tooltip")
				{
					_value = string.Format(Localization.Get("xuiLootTakeAllTooltip", false, null), "[action:permanent:Reload:emptystring:KeyboardWithAngleBrackets]");
					return true;
				}
			}
		}
		else if (_bindingName == "userlockmode")
		{
			_value = this.UserLockMode.ToString();
			return true;
		}
		return base.GetBindingValueInternal(ref _value, _bindingName);
	}

	// Token: 0x06007650 RID: 30288 RVA: 0x002C64F8 File Offset: 0x002C46F8
	[PublicizedFrom(EAccessModifier.Private)]
	public void ApplyLockedSlotStates(PackedBoolArray _lockedSlots)
	{
		XUiC_ItemStack[] itemStackControllers = this.GetItemStackControllers();
		for (int i = 0; i < itemStackControllers.Length; i++)
		{
			itemStackControllers[i].UserLockedSlot = _lockedSlots != null && i < _lockedSlots.Length && _lockedSlots[i];
		}
	}

	// Token: 0x06007651 RID: 30289 RVA: 0x002C6538 File Offset: 0x002C4738
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateLockedSlots(XUiC_ContainerStandardControls _csc)
	{
		if (_csc == null || this.Bag == null)
		{
			return;
		}
		int slotCount = this.Bag.SlotCount;
		PackedBoolArray packedBoolArray = _csc.LockedSlots ?? new PackedBoolArray(slotCount);
		if (packedBoolArray.Length != slotCount)
		{
			packedBoolArray.Length = slotCount;
		}
		XUiC_ItemStack[] itemStackControllers = this.GetItemStackControllers();
		int num = 0;
		while (num < itemStackControllers.Length && num < packedBoolArray.Length)
		{
			packedBoolArray[num] = itemStackControllers[num].UserLockedSlot;
			num++;
		}
		_csc.LockedSlots = packedBoolArray;
	}

	// Token: 0x0400575B RID: 22363
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isClosing;

	// Token: 0x0400575C RID: 22364
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Window window;

	// Token: 0x0400575D RID: 22365
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Grid grid;

	// Token: 0x0400575E RID: 22366
	[PublicizedFrom(EAccessModifier.Private)]
	public bool userLockMode;

	// Token: 0x0400575F RID: 22367
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ContainerStandardControls standardControls;

	// Token: 0x04005761 RID: 22369
	[PublicizedFrom(EAccessModifier.Private)]
	public string containerName;

	// Token: 0x04005762 RID: 22370
	[PublicizedFrom(EAccessModifier.Private)]
	public Action onBagModified;

	// Token: 0x04005764 RID: 22372
	[PublicizedFrom(EAccessModifier.Private)]
	public int windowWidth;

	// Token: 0x04005765 RID: 22373
	[PublicizedFrom(EAccessModifier.Private)]
	public int windowGridWidthDifference;

	// Token: 0x04005766 RID: 22374
	[PublicizedFrom(EAccessModifier.Private)]
	public bool activeKeyDown;

	// Token: 0x04005767 RID: 22375
	[PublicizedFrom(EAccessModifier.Private)]
	public bool wasReleased;
}
