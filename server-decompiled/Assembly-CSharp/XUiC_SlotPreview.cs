using System;
using UnityEngine.Scripting;

// Token: 0x0200108E RID: 4238
[Preserve]
public class XUiC_SlotPreview : XUiController
{
	// Token: 0x06008652 RID: 34386 RVA: 0x00336018 File Offset: 0x00334218
	public override void Init()
	{
		base.Init();
		this.slots = base.GetChildrenById("slot", null);
		XUiController[] childrenByType = this.parent.GetChildrenByType<XUiC_ItemStack>(null);
		this.itemStacks = childrenByType;
	}

	// Token: 0x06008653 RID: 34387 RVA: 0x00336054 File Offset: 0x00334254
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

	// Token: 0x06008654 RID: 34388 RVA: 0x00336114 File Offset: 0x00334314
	[PublicizedFrom(EAccessModifier.Private)]
	public void XUiC_SlotPreview_ToolLockChangedEvent(int slotNumber, ItemStack stack, bool locked)
	{
		this.slots[slotNumber].ViewComponent.IsVisible = stack.IsEmpty() && !locked;
	}

	// Token: 0x06008655 RID: 34389 RVA: 0x00336138 File Offset: 0x00334338
	public override void OnClose()
	{
		for (int i = 0; i < this.itemStacks.Length; i++)
		{
			((XUiC_ItemStack)this.itemStacks[i]).SlotChangedEvent -= this.XUiC_SlotPreview_SlotChangedEvent;
		}
	}

	// Token: 0x06008656 RID: 34390 RVA: 0x00336176 File Offset: 0x00334376
	[PublicizedFrom(EAccessModifier.Private)]
	public void XUiC_SlotPreview_SlotChangedEvent(int slotNumber, ItemStack stack)
	{
		this.slots[slotNumber].ViewComponent.IsVisible = stack.IsEmpty();
	}

	// Token: 0x040063F0 RID: 25584
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController[] slots;

	// Token: 0x040063F1 RID: 25585
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController[] itemStacks;
}
