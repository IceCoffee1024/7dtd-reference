using System;
using UnityEngine.Scripting;

// Token: 0x02000F70 RID: 3952
[Preserve]
public class XUiC_ItemStackGrid : XUiController
{
	// Token: 0x17000EA8 RID: 3752
	// (get) Token: 0x06007BEB RID: 31723 RVA: 0x00020119 File Offset: 0x0001E319
	public virtual XUiC_ItemStack.StackLocationTypes StackLocation
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return XUiC_ItemStack.StackLocationTypes.Backpack;
		}
	}

	// Token: 0x06007BEC RID: 31724 RVA: 0x002F0854 File Offset: 0x002EEA54
	public override void Init()
	{
		base.Init();
		this.itemControllers = base.GetChildrenByType<XUiC_ItemStack>(null);
		this.bAwakeCalled = true;
		this.IsDirty = false;
		this.handleSlotChangedDelegate = new XUiEvent_SlotChangedEventHandler(this.HandleSlotChangedEvent);
	}

	// Token: 0x06007BED RID: 31725 RVA: 0x002F088A File Offset: 0x002EEA8A
	public virtual XUiC_ItemStack[] GetItemStackControllers()
	{
		return this.itemControllers;
	}

	// Token: 0x06007BEE RID: 31726 RVA: 0x002F0892 File Offset: 0x002EEA92
	public virtual ItemStack[] GetSlots()
	{
		return this.getUISlots();
	}

	// Token: 0x06007BEF RID: 31727 RVA: 0x002F089C File Offset: 0x002EEA9C
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

	// Token: 0x06007BF0 RID: 31728 RVA: 0x002F08E0 File Offset: 0x002EEAE0
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

	// Token: 0x06007BF1 RID: 31729 RVA: 0x002F0964 File Offset: 0x002EEB64
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

	// Token: 0x06007BF2 RID: 31730 RVA: 0x002F09B7 File Offset: 0x002EEBB7
	public virtual void HandleSlotChangedEvent(int slotNumber, ItemStack stack)
	{
		if (this.items != null)
		{
			this.items[slotNumber] = stack.Clone();
		}
		this.UpdateBackend(this.getUISlots());
	}

	// Token: 0x06007BF3 RID: 31731 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void UpdateBackend(ItemStack[] stackList)
	{
	}

	// Token: 0x06007BF4 RID: 31732 RVA: 0x002F09DC File Offset: 0x002EEBDC
	public virtual void ClearHoveredItems()
	{
		for (int i = 0; i < this.itemControllers.Length; i++)
		{
			this.itemControllers[i].Hovered(false);
		}
	}

	// Token: 0x06007BF5 RID: 31733 RVA: 0x002F0A0C File Offset: 0x002EEC0C
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

	// Token: 0x06007BF6 RID: 31734 RVA: 0x002F0A5B File Offset: 0x002EEC5B
	public override void OnOpen()
	{
		base.OnOpen();
		this.IsDirty = true;
		this.xui.playerUI.RegisterItemStackGrid(this);
	}

	// Token: 0x06007BF7 RID: 31735 RVA: 0x002F0A7B File Offset: 0x002EEC7B
	public override void OnClose()
	{
		base.OnClose();
		this.ClearHoveredItems();
		this.xui.playerUI.UnregisterItemStackGrid(this);
	}

	// Token: 0x06007BF8 RID: 31736 RVA: 0x00291183 File Offset: 0x0028F383
	public override void Update(float _dt)
	{
		base.Update(_dt);
		base.handleDirtyUpdateDefault();
	}

	// Token: 0x04005BEF RID: 23535
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiC_ItemStack[] itemControllers;

	// Token: 0x04005BF0 RID: 23536
	[PublicizedFrom(EAccessModifier.Protected)]
	public ItemStack[] items;

	// Token: 0x04005BF1 RID: 23537
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiEvent_SlotChangedEventHandler handleSlotChangedDelegate;

	// Token: 0x04005BF2 RID: 23538
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bAwakeCalled;
}
