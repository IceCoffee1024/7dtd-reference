using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02001098 RID: 4248
[Preserve]
public class XUiC_Toolbelt : XUiC_ItemStackGrid
{
	// Token: 0x17000FDE RID: 4062
	// (get) Token: 0x060086B6 RID: 34486 RVA: 0x00337226 File Offset: 0x00335426
	// (set) Token: 0x060086B7 RID: 34487 RVA: 0x0033722E File Offset: 0x0033542E
	public int CurrentPage
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000FDF RID: 4063
	// (get) Token: 0x060086B8 RID: 34488 RVA: 0x0002F260 File Offset: 0x0002D460
	public override XUiC_ItemStack.StackLocationTypes StackLocation
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return XUiC_ItemStack.StackLocationTypes.ToolBelt;
		}
	}

	// Token: 0x060086B9 RID: 34489 RVA: 0x00337238 File Offset: 0x00335438
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

	// Token: 0x060086BA RID: 34490 RVA: 0x003372A0 File Offset: 0x003354A0
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

	// Token: 0x060086BB RID: 34491 RVA: 0x00337310 File Offset: 0x00335510
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

	// Token: 0x060086BC RID: 34492 RVA: 0x00337478 File Offset: 0x00335678
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

	// Token: 0x060086BD RID: 34493 RVA: 0x003374D0 File Offset: 0x003356D0
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

	// Token: 0x060086BE RID: 34494 RVA: 0x00337520 File Offset: 0x00335720
	public bool AddItemToCurrentPage(ItemStack itemStack)
	{
		XUiC_ItemStack[] itemControllers = this.itemControllers;
		int num = ((itemControllers != null) ? itemControllers.Length : 0);
		int num2;
		return num > 0 && this.xui.PlayerInventory.Toolbelt.AddItem(itemStack, this.CurrentPage * num, num, out num2);
	}

	// Token: 0x060086BF RID: 34495 RVA: 0x00337564 File Offset: 0x00335764
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

	// Token: 0x060086C0 RID: 34496 RVA: 0x003375B0 File Offset: 0x003357B0
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

	// Token: 0x060086C1 RID: 34497 RVA: 0x003375F4 File Offset: 0x003357F4
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

	// Token: 0x060086C2 RID: 34498 RVA: 0x00337648 File Offset: 0x00335848
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

	// Token: 0x060086C3 RID: 34499 RVA: 0x0033770D File Offset: 0x0033590D
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnToolbeltPrev_OnPress(XUiController _sender, int _mouseButton)
	{
		this.PrevPage();
	}

	// Token: 0x060086C4 RID: 34500 RVA: 0x00337715 File Offset: 0x00335915
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnToolbeltNext_OnPress(XUiController _sender, int _mouseButton)
	{
		this.NextPage();
	}

	// Token: 0x060086C5 RID: 34501 RVA: 0x00337720 File Offset: 0x00335920
	public void PrevPage()
	{
		if (ToolbeltConfig.CurrentConfig.PageCount <= 0)
		{
			return;
		}
		int num = (this.CurrentPage - 1 + ToolbeltConfig.CurrentConfig.PageCount) % ToolbeltConfig.CurrentConfig.PageCount;
		this.GoToPage(num, true);
	}

	// Token: 0x060086C6 RID: 34502 RVA: 0x00337764 File Offset: 0x00335964
	public void NextPage()
	{
		if (ToolbeltConfig.CurrentConfig.PageCount <= 0)
		{
			return;
		}
		int num = (this.CurrentPage + 1) % ToolbeltConfig.CurrentConfig.PageCount;
		this.GoToPage(num, true);
	}

	// Token: 0x060086C7 RID: 34503 RVA: 0x0033779C File Offset: 0x0033599C
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

	// Token: 0x060086C8 RID: 34504 RVA: 0x002C8616 File Offset: 0x002C6816
	[PublicizedFrom(EAccessModifier.Private)]
	public void PlayerInventory_OnToolbeltItemsChanged()
	{
		this.SetStacks(this.GetSlots());
		this.IsDirty = true;
	}

	// Token: 0x060086C9 RID: 34505 RVA: 0x00337830 File Offset: 0x00335A30
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

	// Token: 0x060086CA RID: 34506 RVA: 0x003378AC File Offset: 0x00335AAC
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

	// Token: 0x060086CB RID: 34507 RVA: 0x00337900 File Offset: 0x00335B00
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

	// Token: 0x060086CC RID: 34508 RVA: 0x00337948 File Offset: 0x00335B48
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

	// Token: 0x060086CD RID: 34509 RVA: 0x00337970 File Offset: 0x00335B70
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

	// Token: 0x04006423 RID: 25635
	public static string ID = "";

	// Token: 0x04006424 RID: 25636
	[PublicizedFrom(EAccessModifier.Private)]
	public int currentHoldingIndex;

	// Token: 0x04006425 RID: 25637
	[PublicizedFrom(EAccessModifier.Private)]
	public int lastActionSlot;

	// Token: 0x04006426 RID: 25638
	[PublicizedFrom(EAccessModifier.Private)]
	public bool lastActionRunning;

	// Token: 0x04006427 RID: 25639
	[PublicizedFrom(EAccessModifier.Protected)]
	public float updateTime;
}
