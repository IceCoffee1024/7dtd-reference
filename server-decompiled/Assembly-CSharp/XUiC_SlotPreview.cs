using System;
using UnityEngine.Scripting;

// Token: 0x0200107E RID: 4222
[Preserve]
public class XUiC_SlotPreview : XUiController
{
	// Token: 0x060085FF RID: 34303 RVA: 0x00333F94 File Offset: 0x00332194
	public override void Init()
	{
		base.Init();
		this.slots = base.GetChildrenById("slot", null);
		XUiController[] childrenByType = this.parent.GetChildrenByType<XUiC_ItemStack>(null);
		this.itemStacks = childrenByType;
	}

	// Token: 0x06008600 RID: 34304 RVA: 0x00333FD0 File Offset: 0x003321D0
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

	// Token: 0x06008601 RID: 34305 RVA: 0x00334090 File Offset: 0x00332290
	[PublicizedFrom(EAccessModifier.Private)]
	public void XUiC_SlotPreview_ToolLockChangedEvent(int slotNumber, ItemStack stack, bool locked)
	{
		this.slots[slotNumber].ViewComponent.IsVisible = stack.IsEmpty() && !locked;
	}

	// Token: 0x06008602 RID: 34306 RVA: 0x003340B4 File Offset: 0x003322B4
	public override void OnClose()
	{
		for (int i = 0; i < this.itemStacks.Length; i++)
		{
			((XUiC_ItemStack)this.itemStacks[i]).SlotChangedEvent -= this.XUiC_SlotPreview_SlotChangedEvent;
		}
	}

	// Token: 0x06008603 RID: 34307 RVA: 0x003340F2 File Offset: 0x003322F2
	[PublicizedFrom(EAccessModifier.Private)]
	public void XUiC_SlotPreview_SlotChangedEvent(int slotNumber, ItemStack stack)
	{
		this.slots[slotNumber].ViewComponent.IsVisible = stack.IsEmpty();
	}

	// Token: 0x040063B1 RID: 25521
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController[] slots;

	// Token: 0x040063B2 RID: 25522
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController[] itemStacks;
}
