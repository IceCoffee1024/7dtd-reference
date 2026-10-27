using System;
using InControl;
using UnityEngine.Scripting;

// Token: 0x02000F7F RID: 3967
[UnityEngine.Scripting.Preserve]
public class XUiC_ItemStackGrid : XUiController
{
	// Token: 0x17000EB3 RID: 3763
	// (get) Token: 0x06007C31 RID: 31793 RVA: 0x00020119 File Offset: 0x0001E319
	public virtual XUiC_ItemStack.StackLocationTypes StackLocation
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return XUiC_ItemStack.StackLocationTypes.Backpack;
		}
	}

	// Token: 0x17000EB4 RID: 3764
	// (get) Token: 0x06007C32 RID: 31794 RVA: 0x002F21A8 File Offset: 0x002F03A8
	// (set) Token: 0x06007C33 RID: 31795 RVA: 0x002F21B0 File Offset: 0x002F03B0
	public XUiC_ContainerStandardControls.MatchesFiltersDelegate MatchesFiltersFunc
	{
		get
		{
			return this.matchesFiltersFunc;
		}
		set
		{
			if (this.matchesFiltersFunc == value)
			{
				return;
			}
			this.matchesFiltersFunc = value;
			this.updateSearch();
		}
	}

	// Token: 0x06007C34 RID: 31796 RVA: 0x002F21D0 File Offset: 0x002F03D0
	public override void Init()
	{
		base.Init();
		if (!base.TryGetParentView<XUiV_ScrollView>(out this.scrollView))
		{
			base.TryGetChildView<XUiV_ScrollView>("", out this.scrollView);
		}
		this.itemControllers = base.GetChildrenByType<XUiC_ItemStack>(null);
		this.bAwakeCalled = true;
		this.IsDirty = false;
		this.handleSlotChangedDelegate = new XUiEvent_SlotChangedEventHandler(this.HandleSlotChangedEvent);
	}

	// Token: 0x06007C35 RID: 31797 RVA: 0x002F2231 File Offset: 0x002F0431
	public virtual XUiC_ItemStack[] GetItemStackControllers()
	{
		return this.itemControllers;
	}

	// Token: 0x06007C36 RID: 31798 RVA: 0x002F2239 File Offset: 0x002F0439
	public virtual ItemStack[] GetSlots()
	{
		return this.getUISlots();
	}

	// Token: 0x06007C37 RID: 31799 RVA: 0x002F2244 File Offset: 0x002F0444
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual ItemStack[] getUISlots()
	{
		ItemStack[] array = new ItemStack[this.itemControllers.Length];
		for (int i = 0; i < this.itemControllers.Length; i++)
		{
			array[i] = this.itemControllers[i].ItemStack.Clone();
		}
		return array;
	}

	// Token: 0x06007C38 RID: 31800 RVA: 0x002F2288 File Offset: 0x002F0488
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void SetStacks(ItemStack[] stackList)
	{
		if (stackList == null)
		{
			return;
		}
		XUiC_ItemInfoWindow childByType = this.xui.GetChildByType<XUiC_ItemInfoWindow>();
		int num = 0;
		while (num < stackList.Length && this.itemControllers.Length > num && stackList.Length > num)
		{
			XUiC_ItemStack xuiC_ItemStack = this.itemControllers[num];
			xuiC_ItemStack.SlotChangedEvent -= this.handleSlotChangedDelegate;
			xuiC_ItemStack.ItemStack = stackList[num].Clone();
			xuiC_ItemStack.SlotChangedEvent += this.handleSlotChangedDelegate;
			xuiC_ItemStack.SlotNumber = num;
			xuiC_ItemStack.InfoWindow = childByType;
			xuiC_ItemStack.StackLocation = this.StackLocation;
			num++;
		}
	}

	// Token: 0x06007C39 RID: 31801 RVA: 0x002F230C File Offset: 0x002F050C
	public void AssembleLockSingleStack(ItemStack stack)
	{
		for (int i = 0; i < this.itemControllers.Length; i++)
		{
			XUiC_ItemStack xuiC_ItemStack = this.itemControllers[i];
			if (xuiC_ItemStack.ItemStack.itemValue.Equals(stack.itemValue))
			{
				this.xui.AssembleItem.CurrentItemStackController = xuiC_ItemStack;
				return;
			}
		}
	}

	// Token: 0x06007C3A RID: 31802 RVA: 0x002F235F File Offset: 0x002F055F
	public virtual void HandleSlotChangedEvent(int slotNumber, ItemStack stack)
	{
		if (this.items != null)
		{
			this.items[slotNumber] = stack.Clone();
		}
		this.UpdateBackend(this.getUISlots());
	}

	// Token: 0x06007C3B RID: 31803 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void UpdateBackend(ItemStack[] stackList)
	{
	}

	// Token: 0x06007C3C RID: 31804 RVA: 0x002F2384 File Offset: 0x002F0584
	public virtual void ClearHoveredItems()
	{
		for (int i = 0; i < this.itemControllers.Length; i++)
		{
			this.itemControllers[i].Hovered(false);
		}
	}

	// Token: 0x06007C3D RID: 31805 RVA: 0x002F23B4 File Offset: 0x002F05B4
	public int FindFirstEmptySlot()
	{
		int num = -1;
		for (int i = 0; i < this.itemControllers.Length; i++)
		{
			if (this.itemControllers[i].ViewComponent.IsActiveInHierarchy && this.itemControllers[i].ItemStack.IsEmpty())
			{
				num = i;
				break;
			}
		}
		return num;
	}

	// Token: 0x06007C3E RID: 31806 RVA: 0x002F2403 File Offset: 0x002F0603
	public override void OnOpen()
	{
		base.OnOpen();
		ThreadManager.RunTaskAfterFrames(new Action(this.updateSearch), 1);
		this.IsDirty = true;
		this.xui.playerUI.RegisterItemStackGrid(this);
	}

	// Token: 0x06007C3F RID: 31807 RVA: 0x002F2435 File Offset: 0x002F0635
	public override void OnClose()
	{
		base.OnClose();
		this.ClearHoveredItems();
		this.xui.playerUI.UnregisterItemStackGrid(this);
	}

	// Token: 0x06007C40 RID: 31808 RVA: 0x002F2454 File Offset: 0x002F0654
	public override void Update(float _dt)
	{
		base.Update(_dt);
		this.handleItemDrop();
		base.handleDirtyUpdateDefault();
	}

	// Token: 0x06007C41 RID: 31809 RVA: 0x002F246A File Offset: 0x002F066A
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnHovered(bool _isOver)
	{
		base.OnHovered(_isOver);
		this.isOver = _isOver;
		this.xui.CalloutWindow.UpdateCalloutsForItemStack(base.ViewComponent.UiTransform.gameObject, null, this.isOver, true, true, true);
	}

	// Token: 0x06007C42 RID: 31810 RVA: 0x002F24A4 File Offset: 0x002F06A4
	[PublicizedFrom(EAccessModifier.Private)]
	public void handleItemDrop()
	{
		PlayerActionsGUI guiactions = this.xui.playerUI.playerInput.GUIActions;
		CursorControllerAbs cursorController = this.xui.playerUI.CursorController;
		bool mouseButtonUp = cursorController.GetMouseButtonUp(UICamera.MouseButton.LeftButton);
		bool mouseButtonDown = cursorController.GetMouseButtonDown(UICamera.MouseButton.LeftButton);
		bool mouseButtonUp2 = cursorController.GetMouseButtonUp(UICamera.MouseButton.RightButton);
		bool mouseButtonDown2 = cursorController.GetMouseButtonDown(UICamera.MouseButton.RightButton);
		if (!this.isOver || !base.ViewComponent.UiTransformIsHovered || !base.ViewComponent.EventOnPress)
		{
			this.lastClicked = false;
			return;
		}
		if (this.xui.DragAndDropWindow.CurrentStack.IsEmpty())
		{
			return;
		}
		if (guiactions.LastInputType == BindingSourceType.DeviceBindingSource)
		{
			bool wasReleased = guiactions.Submit.WasReleased;
			bool wasReleased2 = guiactions.HalfStack.WasReleased;
			if (wasReleased)
			{
				this.dropDragAndDropItem(-1);
				return;
			}
			if (wasReleased2)
			{
				this.dropDragAndDropItem(1);
				return;
			}
		}
		else
		{
			if (mouseButtonDown || mouseButtonDown2)
			{
				this.lastClicked = true;
			}
			if (this.lastClicked)
			{
				if (mouseButtonUp)
				{
					this.dropDragAndDropItem(-1);
					this.lastClicked = false;
					return;
				}
				if (mouseButtonUp2)
				{
					this.dropDragAndDropItem(1);
					this.lastClicked = false;
				}
			}
		}
	}

	// Token: 0x06007C43 RID: 31811 RVA: 0x002F25AC File Offset: 0x002F07AC
	[PublicizedFrom(EAccessModifier.Private)]
	public void dropDragAndDropItem(int _dropCount = -1)
	{
		ItemStack itemStack = this.xui.DragAndDropWindow.CurrentStack;
		int count = itemStack.count;
		if (_dropCount <= 0 || _dropCount > count)
		{
			_dropCount = count;
		}
		ItemStack itemStack2 = itemStack.Clone();
		itemStack2.count = _dropCount;
		this.dropItemToGrid(itemStack2);
		if (_dropCount != itemStack2.count)
		{
			this.itemControllers[0].PlayPlaceSound(itemStack);
			itemStack.count -= _dropCount - itemStack2.count;
			if (itemStack.count == 0)
			{
				itemStack = ItemStack.Empty;
			}
			this.xui.DragAndDropWindow.SetCurrentStack(itemStack, false);
		}
		this.xui.CalloutWindow.UpdateCalloutsForItemStack(base.ViewComponent.UiTransform.gameObject, null, this.isOver, true, true, true);
	}

	// Token: 0x06007C44 RID: 31812 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void dropItemToGrid(ItemStack _dropStack)
	{
	}

	// Token: 0x06007C45 RID: 31813 RVA: 0x002F2668 File Offset: 0x002F0868
	[PublicizedFrom(EAccessModifier.Protected)]
	public void updateSearch()
	{
		XUiC_ItemStack[] array = this.itemControllers;
		for (int i = 0; i < array.Length; i++)
		{
			XUiC_ItemStack uiItemStack = array[i];
			if (uiItemStack != null)
			{
				bool flag = true;
				if (this.MatchesFiltersFunc != null)
				{
					ItemStack itemStack = uiItemStack.ItemStack;
					flag = this.MatchesFiltersFunc(itemStack);
				}
				uiItemStack.MatchesSearch = flag;
				if (flag && uiItemStack.IsSelected && this.scrollView != null)
				{
					ThreadManager.RunTaskAfterFrames(delegate
					{
						this.scrollView.MakeVisible(uiItemStack.ViewComponent);
					}, 1);
				}
			}
		}
	}

	// Token: 0x04005C23 RID: 23587
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiC_ItemStack[] itemControllers;

	// Token: 0x04005C24 RID: 23588
	[PublicizedFrom(EAccessModifier.Protected)]
	public ItemStack[] items;

	// Token: 0x04005C25 RID: 23589
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiV_ScrollView scrollView;

	// Token: 0x04005C26 RID: 23590
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiEvent_SlotChangedEventHandler handleSlotChangedDelegate;

	// Token: 0x04005C27 RID: 23591
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bAwakeCalled;

	// Token: 0x04005C28 RID: 23592
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ContainerStandardControls.MatchesFiltersDelegate matchesFiltersFunc;

	// Token: 0x04005C29 RID: 23593
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isOver;

	// Token: 0x04005C2A RID: 23594
	[PublicizedFrom(EAccessModifier.Private)]
	public bool lastClicked;
}
