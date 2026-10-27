using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020010A8 RID: 4264
[Preserve]
public class XUiC_Toolbelt : XUiC_ItemStackGrid
{
	// Token: 0x17000FE9 RID: 4073
	// (get) Token: 0x06008709 RID: 34569 RVA: 0x003392AA File Offset: 0x003374AA
	// (set) Token: 0x0600870A RID: 34570 RVA: 0x003392B2 File Offset: 0x003374B2
	public int CurrentPage
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000FEA RID: 4074
	// (get) Token: 0x0600870B RID: 34571 RVA: 0x0002F260 File Offset: 0x0002D460
	public override XUiC_ItemStack.StackLocationTypes StackLocation
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return XUiC_ItemStack.StackLocationTypes.ToolBelt;
		}
	}

	// Token: 0x0600870C RID: 34572 RVA: 0x003392BC File Offset: 0x003374BC
	public override void Init()
	{
		base.Init();
		XUiC_Toolbelt.ID = base.WindowGroup.Id;
		XUiController childById = base.GetChildById("btnToolbeltPrev");
		if (childById != null)
		{
			childById.OnPress += this.BtnToolbeltPrev_OnPress;
		}
		XUiController childById2 = base.GetChildById("btnToolbeltNext");
		if (childById2 != null)
		{
			childById2.OnPress += this.BtnToolbeltNext_OnPress;
		}
	}

	// Token: 0x0600870D RID: 34573 RVA: 0x00339324 File Offset: 0x00337524
	public override ItemStack[] GetSlots()
	{
		ItemStack[] toolbeltItemStacks = this.xui.PlayerInventory.GetToolbeltItemStacks();
		XUiC_ItemStack[] itemControllers = this.itemControllers;
		int num = ((itemControllers != null) ? itemControllers.Length : toolbeltItemStacks.Length);
		int num2 = this.CurrentPage * num;
		ItemStack[] array = new ItemStack[num];
		for (int i = 0; i < num; i++)
		{
			array[i] = ((num2 + i < toolbeltItemStacks.Length) ? toolbeltItemStacks[num2 + i] : ItemStack.Empty.Clone());
		}
		return array;
	}

	// Token: 0x0600870E RID: 34574 RVA: 0x00339394 File Offset: 0x00337594
	public override void Update(float _dt)
	{
		if (!XUi.IsGameRunning())
		{
			return;
		}
		Inventory toolbelt = this.xui.PlayerInventory.Toolbelt;
		if (this.currentHoldingIndex != toolbelt.GetFocusedItemIdx() && this.itemControllers.Length != 0)
		{
			int num;
			if (this.TryAbsoluteToView(this.currentHoldingIndex, out num))
			{
				this.itemControllers[num].IsHolding = false;
			}
			this.currentHoldingIndex = toolbelt.GetFocusedItemIdx();
			this.GoToPage(this.currentHoldingIndex / this.itemControllers.Length, false);
			int num2;
			if (this.TryAbsoluteToView(this.currentHoldingIndex, out num2))
			{
				this.itemControllers[num2].IsHolding = true;
			}
		}
		if (Time.time > this.updateTime)
		{
			this.updateTime = Time.time + 0.5f;
			bool flag = toolbelt.IsHoldingItemActionRunning();
			if (this.lastActionRunning != flag)
			{
				this.currentHoldingIndex = toolbelt.holdingItemIdx;
				int holdingItemDataSlotNoBarehandFallback = toolbelt.holdingItemDataSlotNoBarehandFallback;
				if (holdingItemDataSlotNoBarehandFallback >= 0)
				{
					int num4;
					if (flag)
					{
						this.lastActionSlot = holdingItemDataSlotNoBarehandFallback;
						int num3;
						if (this.TryAbsoluteToView(holdingItemDataSlotNoBarehandFallback, out num3))
						{
							this.itemControllers[num3].HiddenLock = true;
						}
					}
					else if (this.TryAbsoluteToView(this.lastActionSlot, out num4))
					{
						this.itemControllers[num4].HiddenLock = false;
					}
				}
				this.lastActionRunning = flag;
			}
			if (!GameManager.Instance.bCursorVisible)
			{
				this.ClearHoveredItems();
			}
		}
		this.UpdateQuickSwap();
		if (this.IsDirty)
		{
			base.RefreshBindings();
			this.IsDirty = false;
		}
		base.Update(_dt);
	}

	// Token: 0x0600870F RID: 34575 RVA: 0x003394FC File Offset: 0x003376FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void UpdateBackend(ItemStack[] stackList)
	{
		ItemStack[] toolbeltItemStacks = this.xui.PlayerInventory.GetToolbeltItemStacks();
		int num = this.CurrentPage * stackList.Length;
		int num2 = 0;
		while (num2 < stackList.Length && num + num2 < toolbeltItemStacks.Length)
		{
			toolbeltItemStacks[num + num2] = stackList[num2];
			num2++;
		}
		this.xui.PlayerInventory.SetToolbeltItemStacks(toolbeltItemStacks);
	}

	// Token: 0x06008710 RID: 34576 RVA: 0x00339554 File Offset: 0x00337754
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void SetStacks(ItemStack[] stackList)
	{
		base.SetStacks(stackList);
		if (this.itemControllers == null)
		{
			return;
		}
		int num = this.CurrentPage * this.itemControllers.Length;
		for (int i = 0; i < this.itemControllers.Length; i++)
		{
			this.itemControllers[i].SlotNumber = num + i;
		}
	}

	// Token: 0x06008711 RID: 34577 RVA: 0x003395A4 File Offset: 0x003377A4
	public bool AddItemToCurrentPage(ItemStack itemStack)
	{
		XUiC_ItemStack[] itemControllers = this.itemControllers;
		int num = ((itemControllers != null) ? itemControllers.Length : 0);
		int num2;
		return num > 0 && this.xui.PlayerInventory.Toolbelt.AddItem(itemStack, this.CurrentPage * num, num, out num2);
	}

	// Token: 0x06008712 RID: 34578 RVA: 0x003395E8 File Offset: 0x003377E8
	[PublicizedFrom(EAccessModifier.Private)]
	public bool TryAbsoluteToView(int absIdx, out int viewIdx)
	{
		if (this.itemControllers == null || this.itemControllers.Length == 0)
		{
			viewIdx = -1;
			return false;
		}
		int num = this.CurrentPage * this.itemControllers.Length;
		viewIdx = absIdx - num;
		return viewIdx >= 0 && viewIdx < this.itemControllers.Length;
	}

	// Token: 0x06008713 RID: 34579 RVA: 0x00339634 File Offset: 0x00337834
	[PublicizedFrom(EAccessModifier.Private)]
	public bool TryViewToAbsolute(int viewIdx, int page, out int absIdx)
	{
		if (this.itemControllers == null || viewIdx < 0 || viewIdx >= this.itemControllers.Length)
		{
			absIdx = -1;
			return false;
		}
		int num = page * this.itemControllers.Length;
		absIdx = viewIdx + num;
		return absIdx >= 0;
	}

	// Token: 0x06008714 RID: 34580 RVA: 0x00339678 File Offset: 0x00337878
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateQuickSwap()
	{
		int quickSwapSlot = this.xui.PlayerInventory.QuickSwapSlot;
		int num = this.CurrentPage * this.itemControllers.Length;
		for (int i = 0; i < this.itemControllers.Length; i++)
		{
			this.itemControllers[i].isQuickSwap = num + i == quickSwapSlot;
		}
	}

	// Token: 0x06008715 RID: 34581 RVA: 0x003396CC File Offset: 0x003378CC
	public void GoToPage(int newPage, bool persistFocus = false)
	{
		if (newPage == this.CurrentPage || newPage < 0 || newPage >= ToolbeltConfig.CurrentConfig.PageCount)
		{
			return;
		}
		int num;
		if (this.TryAbsoluteToView(this.currentHoldingIndex, out num))
		{
			this.itemControllers[num].IsHolding = false;
		}
		int num2;
		if (persistFocus && this.TryViewToAbsolute(num, newPage, out num2))
		{
			this.currentHoldingIndex = num2;
			this.xui.PlayerInventory.Toolbelt.SetFocusedItemIdx(this.currentHoldingIndex);
			this.xui.PlayerInventory.Toolbelt.SetHoldingItemIdx(this.currentHoldingIndex);
		}
		this.CurrentPage = newPage;
		this.PlayerInventory_OnToolbeltItemsChanged();
		this.IsDirty = true;
		int num3;
		if (this.TryAbsoluteToView(this.currentHoldingIndex, out num3))
		{
			this.itemControllers[num3].IsHolding = true;
		}
	}

	// Token: 0x06008716 RID: 34582 RVA: 0x00339791 File Offset: 0x00337991
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnToolbeltPrev_OnPress(XUiController _sender, int _mouseButton)
	{
		this.PrevPage();
	}

	// Token: 0x06008717 RID: 34583 RVA: 0x00339799 File Offset: 0x00337999
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnToolbeltNext_OnPress(XUiController _sender, int _mouseButton)
	{
		this.NextPage();
	}

	// Token: 0x06008718 RID: 34584 RVA: 0x003397A4 File Offset: 0x003379A4
	public void PrevPage()
	{
		if (ToolbeltConfig.CurrentConfig.PageCount <= 0)
		{
			return;
		}
		int num = (this.CurrentPage - 1 + ToolbeltConfig.CurrentConfig.PageCount) % ToolbeltConfig.CurrentConfig.PageCount;
		this.GoToPage(num, true);
	}

	// Token: 0x06008719 RID: 34585 RVA: 0x003397E8 File Offset: 0x003379E8
	public void NextPage()
	{
		if (ToolbeltConfig.CurrentConfig.PageCount <= 0)
		{
			return;
		}
		int num = (this.CurrentPage + 1) % ToolbeltConfig.CurrentConfig.PageCount;
		this.GoToPage(num, true);
	}

	// Token: 0x0600871A RID: 34586 RVA: 0x00339820 File Offset: 0x00337A20
	public override void OnOpen()
	{
		base.OnOpen();
		this.xui.PlayerInventory.OnToolbeltItemsChanged += this.PlayerInventory_OnToolbeltItemsChanged;
		this.PlayerInventory_OnToolbeltItemsChanged();
		this.currentHoldingIndex = this.xui.PlayerInventory.Toolbelt.holdingItemIdx;
		int num;
		if (this.TryAbsoluteToView(this.currentHoldingIndex, out num))
		{
			this.itemControllers[num].IsHolding = true;
		}
		this.xui.playerUI.windowManager.Open(this.xui.DragAndDropWindow.WindowGroup, false);
	}

	// Token: 0x0600871B RID: 34587 RVA: 0x002C9B82 File Offset: 0x002C7D82
	[PublicizedFrom(EAccessModifier.Private)]
	public void PlayerInventory_OnToolbeltItemsChanged()
	{
		this.SetStacks(this.GetSlots());
		this.IsDirty = true;
	}

	// Token: 0x0600871C RID: 34588 RVA: 0x003398B4 File Offset: 0x00337AB4
	public override void OnClose()
	{
		base.OnClose();
		this.xui.PlayerInventory.OnToolbeltItemsChanged -= this.PlayerInventory_OnToolbeltItemsChanged;
		GUIWindowManager windowManager = this.xui.playerUI.windowManager;
		XUiC_DragAndDropWindow dragAndDropWindow = this.xui.DragAndDropWindow;
		windowManager.Close((dragAndDropWindow != null) ? dragAndDropWindow.WindowGroup : null, false);
		int num;
		if (this.TryAbsoluteToView(this.currentHoldingIndex, out num))
		{
			this.itemControllers[num].IsHolding = false;
		}
	}

	// Token: 0x0600871D RID: 34589 RVA: 0x00339930 File Offset: 0x00337B30
	public int ResolveShortcutSlot(int key, bool shiftPressed)
	{
		if (key < 0)
		{
			return key;
		}
		int num = key;
		if (shiftPressed && ToolbeltConfig.CurrentConfig.BarsPerPage > 1)
		{
			num += ToolbeltConfig.CurrentConfig.SlotsPerBar;
		}
		return num + this.CurrentPage * ToolbeltConfig.CurrentConfig.BarsPerPage * ToolbeltConfig.CurrentConfig.SlotsPerBar;
	}

	// Token: 0x0600871E RID: 34590 RVA: 0x00339984 File Offset: 0x00337B84
	public int WrapFocusWithinPage(int currentFocus, int direction)
	{
		XUiC_ItemStack[] itemControllers = this.itemControllers;
		int num = ((itemControllers != null) ? itemControllers.Length : 0);
		if (num <= 0)
		{
			return currentFocus;
		}
		int num2 = this.CurrentPage * num;
		int num3 = num2 + num;
		int num4 = currentFocus + direction;
		if (num4 < num2)
		{
			num4 = num3 - 1;
		}
		else if (num4 >= num3)
		{
			num4 = num2;
		}
		return num4;
	}

	// Token: 0x0600871F RID: 34591 RVA: 0x003399CC File Offset: 0x00337BCC
	[PublicizedFrom(EAccessModifier.Internal)]
	public bool TryGetSlotControl(int slotIdx, out XUiC_ItemStack itemStack)
	{
		int num;
		if (this.TryAbsoluteToView(slotIdx, out num))
		{
			itemStack = this.itemControllers[num];
			return true;
		}
		itemStack = null;
		return false;
	}

	// Token: 0x06008720 RID: 34592 RVA: 0x003399F4 File Offset: 0x00337BF4
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string _value, string _bindingName)
	{
		if (_bindingName == "beltslots")
		{
			_value = ToolbeltConfig.CurrentConfig.SlotsPerBar.ToString();
			return true;
		}
		if (_bindingName == "beltCount")
		{
			_value = ToolbeltConfig.CurrentConfig.BarsPerPage.ToString();
			return true;
		}
		if (_bindingName == "toolbeltpage")
		{
			_value = (this.CurrentPage + 1).ToString() + "/" + ToolbeltConfig.CurrentConfig.PageCount.ToString();
			return true;
		}
		if (!(_bindingName == "showpager"))
		{
			return base.GetBindingValueInternal(ref _value, _bindingName);
		}
		_value = (ToolbeltConfig.CurrentConfig.PageCount > 1).ToString();
		return true;
	}

	// Token: 0x04006462 RID: 25698
	public static string ID = "";

	// Token: 0x04006463 RID: 25699
	[PublicizedFrom(EAccessModifier.Private)]
	public int currentHoldingIndex;

	// Token: 0x04006464 RID: 25700
	[PublicizedFrom(EAccessModifier.Private)]
	public int lastActionSlot;

	// Token: 0x04006465 RID: 25701
	[PublicizedFrom(EAccessModifier.Private)]
	public bool lastActionRunning;

	// Token: 0x04006466 RID: 25702
	[PublicizedFrom(EAccessModifier.Protected)]
	public float updateTime;
}
