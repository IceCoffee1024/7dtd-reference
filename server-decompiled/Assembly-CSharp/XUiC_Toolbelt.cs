using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02001088 RID: 4232
[Preserve]
public class XUiC_Toolbelt : XUiC_ItemStackGrid
{
	// Token: 0x17000FA9 RID: 4009
	// (get) Token: 0x06008674 RID: 34420 RVA: 0x00020021 File Offset: 0x0001E221
	public override XUiC_ItemStack.StackLocationTypes StackLocation
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return XUiC_ItemStack.StackLocationTypes.ToolBelt;
		}
	}

	// Token: 0x17000FAA RID: 4010
	// (get) Token: 0x06008675 RID: 34421 RVA: 0x003338EE File Offset: 0x00331AEE
	public bool HasSecondRow
	{
		get
		{
			return this.itemControllers != null && this.backendSlotCount > this.itemControllers.Length / 2;
		}
	}

	// Token: 0x06008676 RID: 34422 RVA: 0x0033390C File Offset: 0x00331B0C
	public override void Init()
	{
		base.Init();
		XUiC_Toolbelt.ID = base.WindowGroup.Id;
	}

	// Token: 0x06008677 RID: 34423 RVA: 0x00333924 File Offset: 0x00331B24
	public override ItemStack[] GetSlots()
	{
		return this.xui.PlayerInventory.GetToolbeltItemStacks();
	}

	// Token: 0x06008678 RID: 34424 RVA: 0x00333938 File Offset: 0x00331B38
	public override void Update(float _dt)
	{
		if (!XUi.IsGameRunning())
		{
			return;
		}
		Inventory toolbelt = this.xui.PlayerInventory.Toolbelt;
		if (this.currentHoldingIndex != toolbelt.GetFocusedItemIdx())
		{
			if (this.currentHoldingIndex != toolbelt.DUMMY_SLOT_IDX)
			{
				this.itemControllers[this.currentHoldingIndex].IsHolding = false;
			}
			this.currentHoldingIndex = toolbelt.GetFocusedItemIdx();
			if (this.currentHoldingIndex != toolbelt.DUMMY_SLOT_IDX)
			{
				this.itemControllers[this.currentHoldingIndex].IsHolding = true;
			}
		}
		if (Time.time > this.updateTime)
		{
			this.updateTime = Time.time + 0.5f;
			bool flag = toolbelt.IsHoldingItemActionRunning();
			if (this.lastActionRunning != flag)
			{
				this.currentHoldingIndex = toolbelt.holdingItemIdx;
				if (this.currentHoldingIndex != toolbelt.DUMMY_SLOT_IDX)
				{
					if (flag)
					{
						this.lastActionSlot = toolbelt.holdingItemIdx;
						this.itemControllers[toolbelt.holdingItemIdx].HiddenLock = true;
					}
					else
					{
						this.itemControllers[this.lastActionSlot].HiddenLock = flag;
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

	// Token: 0x06008679 RID: 34425 RVA: 0x00333A77 File Offset: 0x00331C77
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void UpdateBackend(ItemStack[] stackList)
	{
		this.xui.PlayerInventory.SetToolbeltItemStacks(stackList);
	}

	// Token: 0x0600867A RID: 34426 RVA: 0x00333A8C File Offset: 0x00331C8C
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateQuickSwap()
	{
		int quickSwapSlot = this.xui.PlayerInventory.QuickSwapSlot;
		for (int i = 0; i < this.itemControllers.Length; i++)
		{
			this.itemControllers[i].isQuickSwap = i == quickSwapSlot;
		}
	}

	// Token: 0x0600867B RID: 34427 RVA: 0x00333AD0 File Offset: 0x00331CD0
	public override void OnOpen()
	{
		base.OnOpen();
		this.xui.PlayerInventory.OnToolbeltItemsChanged += this.PlayerInventory_OnToolbeltItemsChanged;
		this.PlayerInventory_OnToolbeltItemsChanged();
		this.currentHoldingIndex = this.xui.PlayerInventory.Toolbelt.holdingItemIdx;
		if (this.currentHoldingIndex != this.xui.PlayerInventory.Toolbelt.DUMMY_SLOT_IDX)
		{
			this.itemControllers[this.currentHoldingIndex].IsHolding = true;
		}
		this.xui.playerUI.windowManager.Open(this.xui.DragAndDropWindow.WindowGroup, false);
		if (this.backendSlotCount < 0)
		{
			this.backendSlotCount = this.xui.PlayerInventory.Toolbelt.PUBLIC_SLOTS;
			this.IsDirty = true;
		}
	}

	// Token: 0x0600867C RID: 34428 RVA: 0x002C5271 File Offset: 0x002C3471
	[PublicizedFrom(EAccessModifier.Private)]
	public void PlayerInventory_OnToolbeltItemsChanged()
	{
		this.SetStacks(this.GetSlots());
		this.IsDirty = true;
	}

	// Token: 0x0600867D RID: 34429 RVA: 0x00333BA4 File Offset: 0x00331DA4
	public override void OnClose()
	{
		base.OnClose();
		this.xui.PlayerInventory.OnToolbeltItemsChanged -= this.PlayerInventory_OnToolbeltItemsChanged;
		GUIWindowManager windowManager = this.xui.playerUI.windowManager;
		XUiC_DragAndDropWindow dragAndDropWindow = this.xui.DragAndDropWindow;
		windowManager.Close((dragAndDropWindow != null) ? dragAndDropWindow.WindowGroup : null, false);
		if (this.currentHoldingIndex != this.xui.PlayerInventory.Toolbelt.DUMMY_SLOT_IDX)
		{
			this.itemControllers[this.currentHoldingIndex].IsHolding = false;
		}
	}

	// Token: 0x0600867E RID: 34430 RVA: 0x00333C30 File Offset: 0x00331E30
	[PublicizedFrom(EAccessModifier.Internal)]
	public XUiC_ItemStack GetSlotControl(int slotIdx)
	{
		return this.itemControllers[slotIdx];
	}

	// Token: 0x0600867F RID: 34431 RVA: 0x00333C3C File Offset: 0x00331E3C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string _value, string _bindingName)
	{
		if (_bindingName == "secondrow")
		{
			_value = this.HasSecondRow.ToString();
			return true;
		}
		return base.GetBindingValueInternal(ref _value, _bindingName);
	}

	// Token: 0x040063E3 RID: 25571
	public static string ID = "";

	// Token: 0x040063E4 RID: 25572
	[PublicizedFrom(EAccessModifier.Private)]
	public int currentHoldingIndex;

	// Token: 0x040063E5 RID: 25573
	[PublicizedFrom(EAccessModifier.Private)]
	public int lastActionSlot;

	// Token: 0x040063E6 RID: 25574
	[PublicizedFrom(EAccessModifier.Private)]
	public bool lastActionRunning;

	// Token: 0x040063E7 RID: 25575
	[PublicizedFrom(EAccessModifier.Protected)]
	public float updateTime;

	// Token: 0x040063E8 RID: 25576
	[PublicizedFrom(EAccessModifier.Private)]
	public int backendSlotCount = -1;
}
