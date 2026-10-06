using System;
using UnityEngine.Scripting;

// Token: 0x02000F72 RID: 3954
[Preserve]
public class XUiC_ItemStackSlotGrid : XUiController
{
	// Token: 0x17000ECC RID: 3788
	// (get) Token: 0x06007C33 RID: 31795 RVA: 0x002F12B6 File Offset: 0x002EF4B6
	public ItemStackGrid Source
	{
		get
		{
			return this.source;
		}
	}

	// Token: 0x06007C34 RID: 31796 RVA: 0x002F12C0 File Offset: 0x002EF4C0
	public override void Init()
	{
		base.Init();
		this.itemControllers = base.GetChildrenByType<XUiC_ItemStackSlot>(null);
		for (int i = 0; i < this.itemControllers.Length; i++)
		{
			this.itemControllers[i].Bind(this, i);
		}
	}

	// Token: 0x06007C35 RID: 31797 RVA: 0x002F1304 File Offset: 0x002EF504
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
		if (this.source.Length != this.itemControllers.Length)
		{
			Log.Error(string.Format("[{0}] Length mismatch: Source has {1} slots, expected {2}.", "XUiC_ItemStackSlotGrid", this.source.Length, this.itemControllers.Length));
			return;
		}
		this.source.SlotChanged += this.OnSlotChanged;
	}

	// Token: 0x06007C36 RID: 31798 RVA: 0x002F139E File Offset: 0x002EF59E
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnSlotChanged(int _index)
	{
		this.itemControllers[_index].IsDirty = true;
	}

	// Token: 0x06007C37 RID: 31799 RVA: 0x002F13B0 File Offset: 0x002EF5B0
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
			this.source.SetItem(_index, currentStack.Clone());
			this.xui.DragAndDropWindow.SetCurrentStack(itemStack, true);
			return;
		}
		ItemStack itemStack2 = this.source[_index];
		if (itemStack2.IsEmpty())
		{
			return;
		}
		this.xui.DragAndDropWindow.SetCurrentStack(itemStack2.Clone(), true);
		this.source.SetItem(_index, ItemStack.Empty);
	}

	// Token: 0x06007C38 RID: 31800 RVA: 0x002F1454 File Offset: 0x002EF654
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
		this.source.SetItem(_index, (itemStack.count > 0) ? itemStack : ItemStack.Empty);
	}

	// Token: 0x06007C39 RID: 31801 RVA: 0x002F14DD File Offset: 0x002EF6DD
	public override void OnClose()
	{
		base.OnClose();
		if (this.source != null)
		{
			this.source.SlotChanged -= this.OnSlotChanged;
		}
	}

	// Token: 0x06007C3A RID: 31802 RVA: 0x002F1504 File Offset: 0x002EF704
	public XUiC_ItemStackSlot[] GetItemStackControllers()
	{
		return this.itemControllers;
	}

	// Token: 0x04005BFF RID: 23551
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiC_ItemStackSlot[] itemControllers;

	// Token: 0x04005C00 RID: 23552
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemStackGrid source;
}
