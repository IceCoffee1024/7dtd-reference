using System;
using UnityEngine.Scripting;

// Token: 0x02000F2F RID: 3887
[Preserve]
public class XUiC_Creative2Stack : XUiC_ItemStack
{
	// Token: 0x06007944 RID: 31044 RVA: 0x002DF779 File Offset: 0x002DD979
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void setItemStack(ItemStack _stack)
	{
		this.itemStack = _stack;
	}

	// Token: 0x06007945 RID: 31045 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void HandleDropOne()
	{
	}

	// Token: 0x06007946 RID: 31046 RVA: 0x002DF782 File Offset: 0x002DD982
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void HandleClickComplete()
	{
		this.lastClicked = false;
	}

	// Token: 0x06007947 RID: 31047 RVA: 0x002DF78B File Offset: 0x002DD98B
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void SwapItem()
	{
		this.xui.DragAndDropWindow.SetCurrentStack(this.itemStack.Clone(), true);
		this.xui.DragAndDropWindow.PickUpType = base.StackLocation;
		base.HandleSlotChangeEvent();
	}
}
