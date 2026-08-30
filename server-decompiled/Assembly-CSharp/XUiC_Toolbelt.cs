using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200106F RID: 4207
[Preserve]
public class XUiC_Toolbelt : XUiC_ItemStackGrid
{
	// Token: 0x17000F70 RID: 3952
	// (get) Token: 0x06008582 RID: 34178 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override XUiC_ItemStack.StackLocationTypes StackLocation
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return XUiC_ItemStack.StackLocationTypes.ToolBelt;
		}
	}

	// Token: 0x17000F71 RID: 3953
	// (get) Token: 0x06008583 RID: 34179 RVA: 0x0032F0C6 File Offset: 0x0032D2C6
	public bool HasSecondRow
	{
		get
		{
			return this.itemControllers != null && this.backendSlotCount > this.itemControllers.Length / 2;
		}
	}

	// Token: 0x06008584 RID: 34180 RVA: 0x0032F0E4 File Offset: 0x0032D2E4
	public override void Init()
	{
		base.Init();
		XUiC_Toolbelt.ID = base.WindowGroup.Id;
	}

	// Token: 0x06008585 RID: 34181 RVA: 0x0032F0FC File Offset: 0x0032D2FC
	public override ItemStack[] GetSlots()
	{
		return this.xui.PlayerInventory.GetToolbeltItemStacks();
	}

	// Token: 0x06008586 RID: 34182 RVA: 0x0032F110 File Offset: 0x0032D310
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

	// Token: 0x06008587 RID: 34183 RVA: 0x0032F24F File Offset: 0x0032D44F
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void UpdateBackend(ItemStack[] stackList)
	{
		this.xui.PlayerInventory.SetToolbeltItemStacks(stackList);
	}

	// Token: 0x06008588 RID: 34184 RVA: 0x0032F264 File Offset: 0x0032D464
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateQuickSwap()
	{
		int quickSwapSlot = this.xui.PlayerInventory.QuickSwapSlot;
		for (int i = 0; i < this.itemControllers.Length; i++)
		{
			this.itemControllers[i].isQuickSwap = i == quickSwapSlot;
		}
	}

	// Token: 0x06008589 RID: 34185 RVA: 0x0032F2A8 File Offset: 0x0032D4A8
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

	// Token: 0x0600858A RID: 34186 RVA: 0x002C1D4D File Offset: 0x002BFF4D
	[PublicizedFrom(EAccessModifier.Private)]
	public void PlayerInventory_OnToolbeltItemsChanged()
	{
		this.SetStacks(this.GetSlots());
		this.IsDirty = true;
	}

	// Token: 0x0600858B RID: 34187 RVA: 0x0032F37C File Offset: 0x0032D57C
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

	// Token: 0x0600858C RID: 34188 RVA: 0x0032F408 File Offset: 0x0032D608
	[PublicizedFrom(EAccessModifier.Internal)]
	public XUiC_ItemStack GetSlotControl(int slotIdx)
	{
		return this.itemControllers[slotIdx];
	}

	// Token: 0x0600858D RID: 34189 RVA: 0x0032F414 File Offset: 0x0032D614
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

	// Token: 0x0400631C RID: 25372
	public static string ID = "";

	// Token: 0x0400631D RID: 25373
	[PublicizedFrom(EAccessModifier.Private)]
	public int currentHoldingIndex;

	// Token: 0x0400631E RID: 25374
	[PublicizedFrom(EAccessModifier.Private)]
	public int lastActionSlot;

	// Token: 0x0400631F RID: 25375
	[PublicizedFrom(EAccessModifier.Private)]
	public bool lastActionRunning;

	// Token: 0x04006320 RID: 25376
	[PublicizedFrom(EAccessModifier.Protected)]
	public float updateTime;

	// Token: 0x04006321 RID: 25377
	[PublicizedFrom(EAccessModifier.Private)]
	public int backendSlotCount = -1;
}
