using System;
using UnityEngine.Scripting;

// Token: 0x02001054 RID: 4180
[Preserve]
public class XUiC_SlotPreview : XUiController
{
	// Token: 0x060084C7 RID: 33991 RVA: 0x0032BB60 File Offset: 0x00329D60
	public override void Init()
	{
		base.Init();
		this.slots = base.GetChildrenById("slot", null);
		XUiController[] childrenByType = this.parent.GetChildrenByType<XUiC_ItemStack>(null);
		this.itemStacks = childrenByType;
	}

	// Token: 0x060084C8 RID: 33992 RVA: 0x0032BB9C File Offset: 0x00329D9C
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

	// Token: 0x060084C9 RID: 33993 RVA: 0x0032BC5C File Offset: 0x00329E5C
	[PublicizedFrom(EAccessModifier.Private)]
	public void XUiC_SlotPreview_ToolLockChangedEvent(int slotNumber, ItemStack stack, bool locked)
	{
		this.slots[slotNumber].ViewComponent.IsVisible = stack.IsEmpty() && !locked;
	}

	// Token: 0x060084CA RID: 33994 RVA: 0x0032BC80 File Offset: 0x00329E80
	public override void OnClose()
	{
		for (int i = 0; i < this.itemStacks.Length; i++)
		{
			((XUiC_ItemStack)this.itemStacks[i]).SlotChangedEvent -= this.XUiC_SlotPreview_SlotChangedEvent;
		}
	}

	// Token: 0x060084CB RID: 33995 RVA: 0x0032BCBE File Offset: 0x00329EBE
	[PublicizedFrom(EAccessModifier.Private)]
	public void XUiC_SlotPreview_SlotChangedEvent(int slotNumber, ItemStack stack)
	{
		this.slots[slotNumber].ViewComponent.IsVisible = stack.IsEmpty();
	}

	// Token: 0x040062A5 RID: 25253
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController[] slots;

	// Token: 0x040062A6 RID: 25254
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController[] itemStacks;
}
