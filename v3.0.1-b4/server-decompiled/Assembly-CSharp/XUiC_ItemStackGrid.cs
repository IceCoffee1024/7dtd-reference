using System;
using UnityEngine.Scripting;

// Token: 0x02000F4A RID: 3914
[Preserve]
public class XUiC_ItemStackGrid : XUiController
{
	// Token: 0x17000E6D RID: 3693
	// (get) Token: 0x06007B2C RID: 31532 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual XUiC_ItemStack.StackLocationTypes StackLocation
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return XUiC_ItemStack.StackLocationTypes.Backpack;
		}
	}

	// Token: 0x06007B2D RID: 31533 RVA: 0x002E99E0 File Offset: 0x002E7BE0
	public override void Init()
	{
		base.Init();
		this.itemControllers = base.GetChildrenByType<XUiC_ItemStack>(null);
		this.bAwakeCalled = true;
		this.IsDirty = false;
		this.handleSlotChangedDelegate = new XUiEvent_SlotChangedEventHandler(this.HandleSlotChangedEvent);
	}

	// Token: 0x06007B2E RID: 31534 RVA: 0x002E9A16 File Offset: 0x002E7C16
	public virtual XUiC_ItemStack[] GetItemStackControllers()
	{
		return this.itemControllers;
	}

	// Token: 0x06007B2F RID: 31535 RVA: 0x002E9A1E File Offset: 0x002E7C1E
	public virtual ItemStack[] GetSlots()
	{
		return this.getUISlots();
	}

	// Token: 0x06007B30 RID: 31536 RVA: 0x002E9A28 File Offset: 0x002E7C28
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

	// Token: 0x06007B31 RID: 31537 RVA: 0x002E9A6C File Offset: 0x002E7C6C
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

	// Token: 0x06007B32 RID: 31538 RVA: 0x002E9AF0 File Offset: 0x002E7CF0
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

	// Token: 0x06007B33 RID: 31539 RVA: 0x002E9B43 File Offset: 0x002E7D43
	public virtual void HandleSlotChangedEvent(int slotNumber, ItemStack stack)
	{
		if (this.items != null)
		{
			this.items[slotNumber] = stack.Clone();
		}
		this.UpdateBackend(this.getUISlots());
	}

	// Token: 0x06007B34 RID: 31540 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void UpdateBackend(ItemStack[] stackList)
	{
	}

	// Token: 0x06007B35 RID: 31541 RVA: 0x002E9B68 File Offset: 0x002E7D68
	public virtual void ClearHoveredItems()
	{
		for (int i = 0; i < this.itemControllers.Length; i++)
		{
			this.itemControllers[i].Hovered(false);
		}
	}

	// Token: 0x06007B36 RID: 31542 RVA: 0x002E9B98 File Offset: 0x002E7D98
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

	// Token: 0x06007B37 RID: 31543 RVA: 0x002E9BE7 File Offset: 0x002E7DE7
	public override void OnOpen()
	{
		base.OnOpen();
		this.IsDirty = true;
		this.xui.playerUI.RegisterItemStackGrid(this);
	}

	// Token: 0x06007B38 RID: 31544 RVA: 0x002E9C07 File Offset: 0x002E7E07
	public override void OnClose()
	{
		base.OnClose();
		this.ClearHoveredItems();
		this.xui.playerUI.UnregisterItemStackGrid(this);
	}

	// Token: 0x04005B0D RID: 23309
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiC_ItemStack[] itemControllers;

	// Token: 0x04005B0E RID: 23310
	[PublicizedFrom(EAccessModifier.Protected)]
	public ItemStack[] items;

	// Token: 0x04005B0F RID: 23311
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiEvent_SlotChangedEventHandler handleSlotChangedDelegate;

	// Token: 0x04005B10 RID: 23312
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bAwakeCalled;
}
