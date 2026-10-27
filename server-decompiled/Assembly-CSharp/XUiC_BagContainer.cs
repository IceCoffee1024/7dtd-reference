using System;
using System.Collections;
using System.Globalization;
using Audio;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000EE0 RID: 3808
[Preserve]
public class XUiC_BagContainer : XUiC_ItemStackGrid
{
	// Token: 0x17000E0B RID: 3595
	// (get) Token: 0x06007657 RID: 30295 RVA: 0x002CA4D6 File Offset: 0x002C86D6
	// (set) Token: 0x06007658 RID: 30296 RVA: 0x002CA4DE File Offset: 0x002C86DE
	public Bag Bag
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000E0C RID: 3596
	// (get) Token: 0x06007659 RID: 30297 RVA: 0x002CA4E7 File Offset: 0x002C86E7
	[XuiXmlBinding("gridDimensions")]
	public Vector2i GridDimensions
	{
		get
		{
			if (this.grid == null)
			{
				return Vector2i.one;
			}
			return new Vector2i(this.grid.Columns, this.grid.Rows);
		}
	}

	// Token: 0x17000E0D RID: 3597
	// (get) Token: 0x0600765A RID: 30298 RVA: 0x002CA512 File Offset: 0x002C8712
	// (set) Token: 0x0600765B RID: 30299 RVA: 0x002CA51A File Offset: 0x002C871A
	public Vector2i GridCellSize
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000E0E RID: 3598
	// (get) Token: 0x0600765C RID: 30300 RVA: 0x00081CD4 File Offset: 0x0007FED4
	public override XUiC_ItemStack.StackLocationTypes StackLocation
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return XUiC_ItemStack.StackLocationTypes.Vehicle;
		}
	}

	// Token: 0x0600765D RID: 30301 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void UpdateBackend(ItemStack[] _stackList)
	{
	}

	// Token: 0x0600765E RID: 30302 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void SetStacks(ItemStack[] _stackList)
	{
	}

	// Token: 0x0600765F RID: 30303 RVA: 0x002CA524 File Offset: 0x002C8724
	public override void Init()
	{
		base.Init();
		this.window = (XUiV_Window)this.viewComponent;
		this.grid = (XUiV_Grid)base.GetChildById("queue").ViewComponent;
		this.GridCellSize = new Vector2i(this.grid.CellWidth, this.grid.CellHeight);
		this.grid.OnSizeChanged += delegate(Vector2Int _, Vector2 _)
		{
			base.RefreshBindings();
		};
		this.standardControls = base.GetChildByType<XUiC_ContainerStandardControls>();
		if (this.standardControls != null)
		{
			this.standardControls.GetLockedSlotsFromStorage = () => this.Bag.LockedSlots;
			this.standardControls.SetLockedSlotsToStorage = delegate(PackedBoolArray _slots)
			{
				PackedBoolArray lockedSlots = this.Bag.LockedSlots;
				if (lockedSlots == null)
				{
					return;
				}
				lockedSlots.CopyFrom(_slots);
			};
			this.standardControls.ApplyLockedSlotStates = new Action<PackedBoolArray>(this.ApplyLockedSlotStates);
			this.standardControls.LockModeToggled = delegate(bool _)
			{
				this.IsDirty = true;
			};
			this.standardControls.SearchFiltersChanged = delegate(XUiC_ContainerStandardControls.MatchesFiltersDelegate filter)
			{
				base.MatchesFiltersFunc = filter;
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

	// Token: 0x06007660 RID: 30304 RVA: 0x002CA66A File Offset: 0x002C886A
	[PublicizedFrom(EAccessModifier.Private)]
	public void btnSort_OnPress(PackedBoolArray _ignoredSlots)
	{
		if (this.Bag == null)
		{
			return;
		}
		this.Bag.ItemGrid.Sort();
	}

	// Token: 0x06007661 RID: 30305 RVA: 0x002CA688 File Offset: 0x002C8888
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
		ItemStackGrid itemGrid = _bag.ItemGrid;
		int num = Mathf.CeilToInt((float)itemGrid.Length / (float)this.grid.Columns);
		Vector2i vector2i = new Vector2i(this.grid.Columns * this.GridCellSize.x, num * this.GridCellSize.y);
		this.windowWidth = vector2i.x + this.windowGridWidthDifference;
		XUiC_ItemInfoWindow childByType = this.xui.GetChildByType<XUiC_ItemInfoWindow>();
		int length = itemGrid.Length;
		for (int i = 0; i < this.itemControllers.Length; i++)
		{
			XUiC_ItemStack xuiC_ItemStack = this.itemControllers[i];
			xuiC_ItemStack.SlotNumber = i;
			xuiC_ItemStack.SlotChangedEvent -= this.HandleBagSlotChangedEvent;
			xuiC_ItemStack.InfoWindow = childByType;
			xuiC_ItemStack.StackLocation = XUiC_ItemStack.StackLocationTypes.LootContainer;
			xuiC_ItemStack.UnlockStack();
			if (i < length)
			{
				xuiC_ItemStack.ForceSetItemStack(itemGrid[i]);
				xuiC_ItemStack.SlotChangedEvent += this.HandleBagSlotChangedEvent;
			}
			else
			{
				xuiC_ItemStack.ItemStack = ItemStack.Empty;
			}
		}
		base.RefreshBindings();
	}

	// Token: 0x06007662 RID: 30306 RVA: 0x002CA7E8 File Offset: 0x002C89E8
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
			if (this.wasReleased && !this.xui.playerUI.windowManager.IsInputActive())
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
			if (!this.isClosing && base.ViewComponent != null && base.ViewComponent.IsVisible && this.Bag != null && (PlatformManager.NativePlatform.Input.CurrentInputStyle != PlayerInputManager.InputStyle.Keyboard || !this.xui.playerUI.windowManager.IsInputActive()) && (this.xui.playerUI.playerInput.GUIActions.LeftStick.WasPressed || this.xui.playerUI.playerInput.PermanentActions.Reload.WasPressed))
			{
				this.standardControls.MoveAll();
			}
		}
	}

	// Token: 0x06007663 RID: 30307 RVA: 0x002CA9B0 File Offset: 0x002C8BB0
	public void HandleBagSlotChangedEvent(int _slotNumber, ItemStack _stack)
	{
		if (this.Bag == null)
		{
			return;
		}
		this.Bag.SetSlot(_slotNumber, _stack);
		base.updateSearch();
	}

	// Token: 0x06007664 RID: 30308 RVA: 0x002CA9D0 File Offset: 0x002C8BD0
	public void OnBagItemChangedInternal()
	{
		if (this.Bag == null)
		{
			return;
		}
		for (int i = 0; i < this.Bag.SlotCount; i++)
		{
			this.SetItemInSlot(i, this.Bag.ItemGrid[i]);
		}
		Action action = this.onBagModified;
		if (action == null)
		{
			return;
		}
		action();
	}

	// Token: 0x06007665 RID: 30309 RVA: 0x002CAA24 File Offset: 0x002C8C24
	public void SetItemInSlot(int _i, ItemStack _stack)
	{
		if (_i >= this.itemControllers.Length)
		{
			return;
		}
		XUiC_ItemStack xuiC_ItemStack = this.itemControllers[_i];
		xuiC_ItemStack.SlotChangedEvent -= this.HandleBagSlotChangedEvent;
		xuiC_ItemStack.ItemStack = _stack;
		xuiC_ItemStack.SlotChangedEvent += this.HandleBagSlotChangedEvent;
	}

	// Token: 0x06007666 RID: 30310 RVA: 0x002CAA64 File Offset: 0x002C8C64
	public override void OnOpen()
	{
		base.OnOpen();
		this.wasReleased = false;
		this.activeKeyDown = false;
		this.IsDirty = true;
		if (this.Bag != null)
		{
			this.Bag.OnBackpackItemsChangedInternal += this.OnBagItemChangedInternal;
			this.Bag.ItemGrid.SlotChanged += this.OnItemGridSlotChanged;
		}
	}

	// Token: 0x06007667 RID: 30311 RVA: 0x002CAAC8 File Offset: 0x002C8CC8
	public override void OnClose()
	{
		base.OnClose();
		if (this.Bag != null)
		{
			this.Bag.OnBackpackItemsChangedInternal -= this.OnBagItemChangedInternal;
			this.Bag.ItemGrid.SlotChanged -= this.OnItemGridSlotChanged;
		}
		this.Bag = null;
		this.onBagModified = null;
	}

	// Token: 0x06007668 RID: 30312 RVA: 0x002CAB24 File Offset: 0x002C8D24
	[PublicizedFrom(EAccessModifier.Protected)]
	public IEnumerator closeInventoryLater()
	{
		this.isClosing = true;
		yield return null;
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
		this.isClosing = false;
		yield break;
	}

	// Token: 0x06007669 RID: 30313 RVA: 0x002CAB33 File Offset: 0x002C8D33
	public bool AddItem(ItemStack _itemStack)
	{
		if (this.Bag == null)
		{
			return false;
		}
		this.Bag.TryStackItem(0, _itemStack);
		return _itemStack.count > 0 && this.Bag.AddItem(_itemStack);
	}

	// Token: 0x0600766A RID: 30314 RVA: 0x002CAB64 File Offset: 0x002C8D64
	public override bool ParseAttribute(string _name, string _value)
	{
		if (_name == "window_grid_width_difference")
		{
			this.windowGridWidthDifference = StringParsers.ParseSInt32(_value, 0, -1, NumberStyles.Integer);
			return true;
		}
		return base.ParseAttribute(_name, _value);
	}

	// Token: 0x0600766B RID: 30315 RVA: 0x002CAB8C File Offset: 0x002C8D8C
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
			XUiC_ContainerStandardControls xuiC_ContainerStandardControls2 = this.standardControls;
			_value = (xuiC_ContainerStandardControls2 != null && xuiC_ContainerStandardControls2.UserLockMode).ToString();
			return true;
		}
		return base.GetBindingValueInternal(ref _value, _bindingName);
	}

	// Token: 0x0600766C RID: 30316 RVA: 0x002CAD60 File Offset: 0x002C8F60
	[PublicizedFrom(EAccessModifier.Private)]
	public void ApplyLockedSlotStates(PackedBoolArray _lockedSlots)
	{
		XUiC_ItemStack[] itemStackControllers = this.GetItemStackControllers();
		for (int i = 0; i < itemStackControllers.Length; i++)
		{
			itemStackControllers[i].UserLockedSlot = _lockedSlots != null && i < _lockedSlots.Length && _lockedSlots[i];
		}
	}

	// Token: 0x0600766D RID: 30317 RVA: 0x002CADA0 File Offset: 0x002C8FA0
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

	// Token: 0x0600766E RID: 30318 RVA: 0x002CAE1A File Offset: 0x002C901A
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnItemGridSlotChanged(int _slotNumber)
	{
		this.GetItemStackControllers()[_slotNumber].UserLockedSlot = this.Bag.ItemGrid.IsSlotLocked(_slotNumber);
		base.updateSearch();
	}

	// Token: 0x0600766F RID: 30319 RVA: 0x002CAE40 File Offset: 0x002C9040
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void dropItemToGrid(ItemStack _dropStack)
	{
		base.dropItemToGrid(_dropStack);
		if (this.Bag == null || !this.Bag.CanStack(_dropStack))
		{
			return;
		}
		if (this.AddItem(_dropStack))
		{
			_dropStack.count = 0;
		}
	}

	// Token: 0x040057B8 RID: 22456
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isClosing;

	// Token: 0x040057B9 RID: 22457
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Window window;

	// Token: 0x040057BA RID: 22458
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Grid grid;

	// Token: 0x040057BB RID: 22459
	[PublicizedFrom(EAccessModifier.Private)]
	public bool userLockMode;

	// Token: 0x040057BC RID: 22460
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ContainerStandardControls standardControls;

	// Token: 0x040057BE RID: 22462
	[PublicizedFrom(EAccessModifier.Private)]
	public string containerName;

	// Token: 0x040057BF RID: 22463
	[PublicizedFrom(EAccessModifier.Private)]
	public Action onBagModified;

	// Token: 0x040057C1 RID: 22465
	[PublicizedFrom(EAccessModifier.Private)]
	public int windowWidth;

	// Token: 0x040057C2 RID: 22466
	[PublicizedFrom(EAccessModifier.Private)]
	public int windowGridWidthDifference;

	// Token: 0x040057C3 RID: 22467
	[PublicizedFrom(EAccessModifier.Private)]
	public bool activeKeyDown;

	// Token: 0x040057C4 RID: 22468
	[PublicizedFrom(EAccessModifier.Private)]
	public bool wasReleased;
}
