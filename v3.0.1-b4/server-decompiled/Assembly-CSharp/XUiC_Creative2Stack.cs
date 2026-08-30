using System;
using UnityEngine.Scripting;

// Token: 0x02000EFA RID: 3834
[Preserve]
public class XUiC_Creative2Stack : XUiC_ItemStack
{
	// Token: 0x0600784E RID: 30798 RVA: 0x002D6B69 File Offset: 0x002D4D69
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void setItemStack(ItemStack _stack)
	{
		this.itemStack = _stack;
	}

	// Token: 0x0600784F RID: 30799 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void HandleDropOne()
	{
	}

	// Token: 0x06007850 RID: 30800 RVA: 0x002D6B72 File Offset: 0x002D4D72
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void HandleClickComplete()
	{
		this.lastClicked = false;
	}

	// Token: 0x06007851 RID: 30801 RVA: 0x002D6B7B File Offset: 0x002D4D7B
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void SwapItem()
	{
		this.xui.DragAndDropWindow.CurrentStack = this.itemStack.Clone();
		this.xui.DragAndDropWindow.PickUpType = base.StackLocation;
		base.HandleSlotChangeEvent();
	}
}
