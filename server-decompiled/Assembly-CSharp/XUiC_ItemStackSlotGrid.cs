using System;
using UnityEngine.Scripting;

// Token: 0x02000F82 RID: 3970
[Preserve]
public class XUiC_ItemStackSlotGrid : XUiController
{
	// Token: 0x17000ED8 RID: 3800
	// (get) Token: 0x06007C82 RID: 31874 RVA: 0x002F2F3E File Offset: 0x002F113E
	public ItemStackGrid Source
	{
		get
		{
			return this.source;
		}
	}

	// Token: 0x06007C83 RID: 31875 RVA: 0x002F2F48 File Offset: 0x002F1148
	public override void Init()
	{
		base.Init();
		this.itemControllers = base.GetChildrenByType<XUiC_ItemStackSlot>(null);
		for (int i = 0; i < this.itemControllers.Length; i++)
		{
			this.itemControllers[i].Bind(this, i);
		}
	}

	// Token: 0x06007C84 RID: 31876 RVA: 0x002F2F8C File Offset: 0x002F118C
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

	// Token: 0x06007C85 RID: 31877 RVA: 0x002F3026 File Offset: 0x002F1226
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnSlotChanged(int _index)
	{
		this.itemControllers[_index].IsDirty = true;
	}

	// Token: 0x06007C86 RID: 31878 RVA: 0x002F3038 File Offset: 0x002F1238
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

	// Token: 0x06007C87 RID: 31879 RVA: 0x002F30DC File Offset: 0x002F12DC
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

	// Token: 0x06007C88 RID: 31880 RVA: 0x002F3165 File Offset: 0x002F1365
	public override void OnClose()
	{
		base.OnClose();
		if (this.source != null)
		{
			this.source.SlotChanged -= this.OnSlotChanged;
		}
	}

	// Token: 0x06007C89 RID: 31881 RVA: 0x002F318C File Offset: 0x002F138C
	public XUiC_ItemStackSlot[] GetItemStackControllers()
	{
		return this.itemControllers;
	}

	// Token: 0x04005C39 RID: 23609
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiC_ItemStackSlot[] itemControllers;

	// Token: 0x04005C3A RID: 23610
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemStackGrid source;
}
