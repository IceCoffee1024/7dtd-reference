using System;
using UnityEngine.Scripting;

// Token: 0x02000F20 RID: 3872
[Preserve]
public class XUiC_Creative2Stack : XUiC_ItemStack
{
	// Token: 0x06007901 RID: 30977 RVA: 0x002DDE89 File Offset: 0x002DC089
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void setItemStack(ItemStack _stack)
	{
		this.itemStack = _stack;
	}

	// Token: 0x06007902 RID: 30978 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void HandleDropOne()
	{
	}

	// Token: 0x06007903 RID: 30979 RVA: 0x002DDE92 File Offset: 0x002DC092
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void HandleClickComplete()
	{
		this.lastClicked = false;
	}

	// Token: 0x06007904 RID: 30980 RVA: 0x002DDE9B File Offset: 0x002DC09B
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void SwapItem()
	{
		this.xui.DragAndDropWindow.SetCurrentStack(this.itemStack.Clone(), true);
		this.xui.DragAndDropWindow.PickUpType = base.StackLocation;
		base.HandleSlotChangeEvent();
	}
}
