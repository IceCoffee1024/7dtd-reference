using System;
using System.Collections;
using System.Globalization;
using Audio;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000EB1 RID: 3761
[Preserve]
public class XUiC_BagContainer : XUiC_ItemStackGrid
{
	// Token: 0x17000DD0 RID: 3536
	// (get) Token: 0x06007597 RID: 30103 RVA: 0x002C271E File Offset: 0x002C091E
	// (set) Token: 0x06007598 RID: 30104 RVA: 0x002C2726 File Offset: 0x002C0926
	public Bag Bag
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000DD1 RID: 3537
	// (get) Token: 0x06007599 RID: 30105 RVA: 0x002C272F File Offset: 0x002C092F
	// (set) Token: 0x0600759A RID: 30106 RVA: 0x002C2737 File Offset: 0x002C0937
	public Vector2i GridCellSize
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000DD2 RID: 3538
	// (get) Token: 0x0600759B RID: 30107 RVA: 0x00080065 File Offset: 0x0007E265
	public override XUiC_ItemStack.StackLocationTypes StackLocation
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return XUiC_ItemStack.StackLocationTypes.Vehicle;
		}
	}

	// Token: 0x17000DD3 RID: 3539
	// (get) Token: 0x0600759C RID: 30108 RVA: 0x002C2740 File Offset: 0x002C0940
	// (set) Token: 0x0600759D RID: 30109 RVA: 0x002C2748 File Offset: 0x002C0948
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

	// Token: 0x0600759E RID: 30110 RVA: 0x002C27A6 File Offset: 0x002C09A6
	public override ItemStack[] GetSlots()
	{
		return this.items;
	}

	// Token: 0x0600759F RID: 30111 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void UpdateBackend(ItemStack[] _stackList)
	{
	}

	// Token: 0x060075A0 RID: 30112 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void SetStacks(ItemStack[] _stackList)
	{
	}

	// Token: 0x060075A1 RID: 30113 RVA: 0x002C27B0 File Offset: 0x002C09B0
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

	// Token: 0x060075A2 RID: 30114 RVA: 0x002C28E0 File Offset: 0x002C0AE0
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

	// Token: 0x060075A3 RID: 30115 RVA: 0x002C2918 File Offset: 0x002C0B18
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

	// Token: 0x060075A4 RID: 30116 RVA: 0x002C2AB4 File Offset: 0x002C0CB4
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateInput()
	{
		PlayerActionsLocal playerInput = this.xui.playerUI.playerInput;
		if (this.UserLockMode && (playerInput.GUIActions.Cancel.WasPressed || playerInput.PermanentActions.Cancel.WasPressed))
		{
			this.UserLockMode = false;
		}
	}

	// Token: 0x060075A5 RID: 30117 RVA: 0x002C2B08 File Offset: 0x002C0D08
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

	// Token: 0x060075A6 RID: 30118 RVA: 0x002C2CBC File Offset: 0x002C0EBC
	public void HandleBagSlotChangedEvent(int _slotNumber, ItemStack _stack)
	{
		if (this.Bag == null)
		{
			return;
		}
		this.Bag.SetSlot(_slotNumber, _stack, true);
	}

	// Token: 0x060075A7 RID: 30119 RVA: 0x002C2CD8 File Offset: 0x002C0ED8
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

	// Token: 0x060075A8 RID: 30120 RVA: 0x002C2D22 File Offset: 0x002C0F22
	public void SetItemInSlot(int _i, ItemStack _stack)
	{
		if (_i >= this.itemControllers.Length)
		{
			return;
		}
		this.itemControllers[_i].ItemStack = _stack;
	}

	// Token: 0x060075A9 RID: 30121 RVA: 0x002C2D3E File Offset: 0x002C0F3E
	public override void OnOpen()
	{
		base.OnOpen();
		this.wasReleased = false;
		this.activeKeyDown = false;
		this.IsDirty = true;
	}

	// Token: 0x060075AA RID: 30122 RVA: 0x002C2D5B File Offset: 0x002C0F5B
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

	// Token: 0x060075AB RID: 30123 RVA: 0x002C2D97 File Offset: 0x002C0F97
	[PublicizedFrom(EAccessModifier.Protected)]
	public IEnumerator closeInventoryLater()
	{
		this.isClosing = true;
		yield return null;
		this.xui.playerUI.windowManager.Close(this.windowGroup, false);
		this.isClosing = false;
		yield break;
	}

	// Token: 0x060075AC RID: 30124 RVA: 0x002C2DA6 File Offset: 0x002C0FA6
	public bool AddItem(ItemStack _itemStack)
	{
		if (!_itemStack.CanMoveTo(XUiC_ItemStack.StackLocationTypes.Backpack))
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

	// Token: 0x060075AD RID: 30125 RVA: 0x002C2DE2 File Offset: 0x002C0FE2
	public override bool ParseAttribute(string _name, string _value)
	{
		if (_name == "window_grid_width_difference")
		{
			this.windowGridWidthDifference = StringParsers.ParseSInt32(_value, 0, -1, NumberStyles.Integer);
			return true;
		}
		return base.ParseAttribute(_name, _value);
	}

	// Token: 0x060075AE RID: 30126 RVA: 0x002C2E0C File Offset: 0x002C100C
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

	// Token: 0x060075AF RID: 30127 RVA: 0x002C2FD4 File Offset: 0x002C11D4
	[PublicizedFrom(EAccessModifier.Private)]
	public void ApplyLockedSlotStates(PackedBoolArray _lockedSlots)
	{
		XUiC_ItemStack[] itemStackControllers = this.GetItemStackControllers();
		for (int i = 0; i < itemStackControllers.Length; i++)
		{
			itemStackControllers[i].UserLockedSlot = _lockedSlots != null && i < _lockedSlots.Length && _lockedSlots[i];
		}
	}

	// Token: 0x060075B0 RID: 30128 RVA: 0x002C3014 File Offset: 0x002C1214
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

	// Token: 0x040056B6 RID: 22198
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isClosing;

	// Token: 0x040056B7 RID: 22199
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Window window;

	// Token: 0x040056B8 RID: 22200
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Grid grid;

	// Token: 0x040056B9 RID: 22201
	[PublicizedFrom(EAccessModifier.Private)]
	public bool userLockMode;

	// Token: 0x040056BA RID: 22202
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ContainerStandardControls standardControls;

	// Token: 0x040056BC RID: 22204
	[PublicizedFrom(EAccessModifier.Private)]
	public string containerName;

	// Token: 0x040056BD RID: 22205
	[PublicizedFrom(EAccessModifier.Private)]
	public Action onBagModified;

	// Token: 0x040056BF RID: 22207
	[PublicizedFrom(EAccessModifier.Private)]
	public int windowWidth;

	// Token: 0x040056C0 RID: 22208
	[PublicizedFrom(EAccessModifier.Private)]
	public int windowGridWidthDifference;

	// Token: 0x040056C1 RID: 22209
	[PublicizedFrom(EAccessModifier.Private)]
	public bool activeKeyDown;

	// Token: 0x040056C2 RID: 22210
	[PublicizedFrom(EAccessModifier.Private)]
	public bool wasReleased;
}
