using System;
using UnityEngine.Scripting;

// Token: 0x02000F61 RID: 3937
[Preserve]
public class XUiC_ItemStackGrid : XUiController
{
	// Token: 0x17000E88 RID: 3720
	// (get) Token: 0x06007BE5 RID: 31717 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual XUiC_ItemStack.StackLocationTypes StackLocation
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return XUiC_ItemStack.StackLocationTypes.Backpack;
		}
	}

	// Token: 0x06007BE6 RID: 31718 RVA: 0x002ED6C4 File Offset: 0x002EB8C4
	public override void Init()
	{
		base.Init();
		this.itemControllers = base.GetChildrenByType<XUiC_ItemStack>(null);
		this.bAwakeCalled = true;
		this.IsDirty = false;
		this.handleSlotChangedDelegate = new XUiEvent_SlotChangedEventHandler(this.HandleSlotChangedEvent);
	}

	// Token: 0x06007BE7 RID: 31719 RVA: 0x002ED6FA File Offset: 0x002EB8FA
	public virtual XUiC_ItemStack[] GetItemStackControllers()
	{
		return this.itemControllers;
	}

	// Token: 0x06007BE8 RID: 31720 RVA: 0x002ED702 File Offset: 0x002EB902
	public virtual ItemStack[] GetSlots()
	{
		return this.getUISlots();
	}

	// Token: 0x06007BE9 RID: 31721 RVA: 0x002ED70C File Offset: 0x002EB90C
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

	// Token: 0x06007BEA RID: 31722 RVA: 0x002ED750 File Offset: 0x002EB950
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

	// Token: 0x06007BEB RID: 31723 RVA: 0x002ED7D4 File Offset: 0x002EB9D4
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

	// Token: 0x06007BEC RID: 31724 RVA: 0x002ED827 File Offset: 0x002EBA27
	public virtual void HandleSlotChangedEvent(int slotNumber, ItemStack stack)
	{
		if (this.items != null)
		{
			this.items[slotNumber] = stack.Clone();
		}
		this.UpdateBackend(this.getUISlots());
	}

	// Token: 0x06007BED RID: 31725 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void UpdateBackend(ItemStack[] stackList)
	{
	}

	// Token: 0x06007BEE RID: 31726 RVA: 0x002ED84C File Offset: 0x002EBA4C
	public virtual void ClearHoveredItems()
	{
		for (int i = 0; i < this.itemControllers.Length; i++)
		{
			this.itemControllers[i].Hovered(false);
		}
	}

	// Token: 0x06007BEF RID: 31727 RVA: 0x002ED87C File Offset: 0x002EBA7C
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

	// Token: 0x06007BF0 RID: 31728 RVA: 0x002ED8CB File Offset: 0x002EBACB
	public override void OnOpen()
	{
		base.OnOpen();
		this.IsDirty = true;
		this.xui.playerUI.RegisterItemStackGrid(this);
	}

	// Token: 0x06007BF1 RID: 31729 RVA: 0x002ED8EB File Offset: 0x002EBAEB
	public override void OnClose()
	{
		base.OnClose();
		this.ClearHoveredItems();
		this.xui.playerUI.UnregisterItemStackGrid(this);
	}

	// Token: 0x04005BC0 RID: 23488
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiC_ItemStack[] itemControllers;

	// Token: 0x04005BC1 RID: 23489
	[PublicizedFrom(EAccessModifier.Protected)]
	public ItemStack[] items;

	// Token: 0x04005BC2 RID: 23490
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiEvent_SlotChangedEventHandler handleSlotChangedDelegate;

	// Token: 0x04005BC3 RID: 23491
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bAwakeCalled;
}
