using System;
using UnityEngine.Scripting;

// Token: 0x0200106D RID: 4205
[Preserve]
public class XUiC_SlotPreview : XUiController
{
	// Token: 0x060085B9 RID: 34233 RVA: 0x00330388 File Offset: 0x0032E588
	public override void Init()
	{
		base.Init();
		this.slots = base.GetChildrenById("slot", null);
		XUiController[] childrenByType = this.parent.GetChildrenByType<XUiC_ItemStack>(null);
		this.itemStacks = childrenByType;
	}

	// Token: 0x060085BA RID: 34234 RVA: 0x003303C4 File Offset: 0x0032E5C4
	public override void OnOpen()
	{
		for (int i = 0; i < this.itemStacks.Length; i++)
		{
			this.XUiC_SlotPreview_SlotChangedEvent(i, ((XUiC_ItemStack)this.itemStacks[i]).ItemStack);
			((XUiC_ItemStack)this.itemStacks[i]).SlotChangedEvent += this.XUiC_SlotPreview_SlotChangedEvent;
			((XUiC_ItemStack)this.itemStacks[i]).ToolLockChangedEvent += this.XUiC_SlotPreview_ToolLockChangedEvent;
			XUiC_ItemStack xuiC_ItemStack = (XUiC_ItemStack)this.itemStacks[i];
			this.slots[i].ViewComponent.IsVisible = ((XUiC_ItemStack)this.itemStacks[i]).ItemStack.IsEmpty() && !xuiC_ItemStack.ToolLock;
		}
	}

	// Token: 0x060085BB RID: 34235 RVA: 0x00330484 File Offset: 0x0032E684
	[PublicizedFrom(EAccessModifier.Private)]
	public void XUiC_SlotPreview_ToolLockChangedEvent(int slotNumber, ItemStack stack, bool locked)
	{
		this.slots[slotNumber].ViewComponent.IsVisible = stack.IsEmpty() && !locked;
	}

	// Token: 0x060085BC RID: 34236 RVA: 0x003304A8 File Offset: 0x0032E6A8
	public override void OnClose()
	{
		for (int i = 0; i < this.itemStacks.Length; i++)
		{
			((XUiC_ItemStack)this.itemStacks[i]).SlotChangedEvent -= this.XUiC_SlotPreview_SlotChangedEvent;
		}
	}

	// Token: 0x060085BD RID: 34237 RVA: 0x003304E6 File Offset: 0x0032E6E6
	[PublicizedFrom(EAccessModifier.Private)]
	public void XUiC_SlotPreview_SlotChangedEvent(int slotNumber, ItemStack stack)
	{
		this.slots[slotNumber].ViewComponent.IsVisible = stack.IsEmpty();
	}

	// Token: 0x0400636C RID: 25452
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController[] slots;

	// Token: 0x0400636D RID: 25453
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController[] itemStacks;
}
