using System;
using UnityEngine.Scripting;

// Token: 0x02000F63 RID: 3939
[Preserve]
public class XUiC_ItemStackSlotGrid : XUiController
{
	// Token: 0x17000EA6 RID: 3750
	// (get) Token: 0x06007C23 RID: 31779 RVA: 0x002EE1A5 File Offset: 0x002EC3A5
	public ItemStackGrid Source
	{
		get
		{
			return this.source;
		}
	}

	// Token: 0x06007C24 RID: 31780 RVA: 0x002EE1B0 File Offset: 0x002EC3B0
	public override void Init()
	{
		base.Init();
		this.itemControllers = base.GetChildrenByType<XUiC_ItemStackSlot>(null);
		for (int i = 0; i < this.itemControllers.Length; i++)
		{
			this.itemControllers[i].Bind(this, i);
		}
	}

	// Token: 0x06007C25 RID: 31781 RVA: 0x002EE1F4 File Offset: 0x002EC3F4
	public void SetSource(ItemStackGrid _source)
	{
		if (this.source != null)
		{
			this.source.SlotChanged -= this.OnSlotChanged;
		}
		this.source = _source;
		if (this.source == null)
		{
			return;
		}
		if (this.source.SlotCount != this.itemControllers.Length)
		{
			Log.Error(string.Format("[{0}] Length mismatch: Source has {1} slots, expected {2}.", "XUiC_ItemStackSlotGrid", this.source.SlotCount, this.itemControllers.Length));
			return;
		}
		this.source.SlotChanged += this.OnSlotChanged;
	}

	// Token: 0x06007C26 RID: 31782 RVA: 0x002EE28E File Offset: 0x002EC48E
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnSlotChanged(int _index)
	{
		this.itemControllers[_index].IsDirty = true;
	}

	// Token: 0x06007C27 RID: 31783 RVA: 0x002EE2A0 File Offset: 0x002EC4A0
	public void OnCellSelected(int _index)
	{
		if (this.source == null)
		{
			return;
		}
		ItemStack currentStack = this.xui.DragAndDropWindow.CurrentStack;
		if (!currentStack.IsEmpty())
		{
			ItemStack itemStack = this.source[_index].Clone();
			this.source.SetSlot(_index, currentStack.Clone());
			this.xui.DragAndDropWindow.CurrentStack = itemStack;
			return;
		}
		ItemStack itemStack2 = this.source[_index];
		if (itemStack2.IsEmpty())
		{
			return;
		}
		this.xui.DragAndDropWindow.CurrentStack = itemStack2.Clone();
		this.source.SetSlot(_index, ItemStack.Empty);
	}

	// Token: 0x06007C28 RID: 31784 RVA: 0x002EE344 File Offset: 0x002EC544
	public void MoveToPreferred(int _index)
	{
		if (this.source == null || this.source.IsLocked(_index))
		{
			return;
		}
		ItemStack itemStack = this.source[_index].Clone();
		if (itemStack.IsEmpty())
		{
			return;
		}
		IInventory playerInventory = this.xui.PlayerInventory;
		playerInventory.TryStackItem(0, itemStack);
		if (itemStack.count > 0 && playerInventory.AddItem(itemStack))
		{
			itemStack = ItemStack.Empty;
		}
		this.source.SetSlot(_index, (itemStack.count > 0) ? itemStack : ItemStack.Empty);
	}

	// Token: 0x06007C29 RID: 31785 RVA: 0x002EE3CD File Offset: 0x002EC5CD
	public override void OnClose()
	{
		base.OnClose();
		if (this.source != null)
		{
			this.source.SlotChanged -= this.OnSlotChanged;
		}
	}

	// Token: 0x06007C2A RID: 31786 RVA: 0x002EE3F4 File Offset: 0x002EC5F4
	public XUiC_ItemStackSlot[] GetItemStackControllers()
	{
		return this.itemControllers;
	}

	// Token: 0x04005BD6 RID: 23510
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiC_ItemStackSlot[] itemControllers;

	// Token: 0x04005BD7 RID: 23511
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemStackGrid source;
}
