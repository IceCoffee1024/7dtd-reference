using System;
using UnityEngine.Scripting;

// Token: 0x02000F11 RID: 3857
[Preserve]
public class XUiC_Creative2Stack : XUiC_ItemStack
{
	// Token: 0x06007906 RID: 30982 RVA: 0x002DAAF5 File Offset: 0x002D8CF5
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void setItemStack(ItemStack _stack)
	{
		this.itemStack = _stack;
	}

	// Token: 0x06007907 RID: 30983 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void HandleDropOne()
	{
	}

	// Token: 0x06007908 RID: 30984 RVA: 0x002DAAFE File Offset: 0x002D8CFE
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void HandleClickComplete()
	{
		this.lastClicked = false;
	}

	// Token: 0x06007909 RID: 30985 RVA: 0x002DAB07 File Offset: 0x002D8D07
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void SwapItem()
	{
		this.xui.DragAndDropWindow.CurrentStack = this.itemStack.Clone();
		this.xui.DragAndDropWindow.PickUpType = base.StackLocation;
		base.HandleSlotChangeEvent();
	}
}
