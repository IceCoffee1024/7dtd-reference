using System;
using UnityEngine.Scripting;

// Token: 0x02000FB1 RID: 4017
[Preserve]
public class XUiC_PartList : XUiC_ItemStackGrid
{
	// Token: 0x06007E6E RID: 32366 RVA: 0x00301234 File Offset: 0x002FF434
	public override void Init()
	{
		base.Init();
		foreach (XUiC_ItemStack xuiC_ItemStack in this.itemControllers)
		{
			xuiC_ItemStack.ViewComponent.IsNavigatable = false;
			xuiC_ItemStack.StackLocation = XUiC_ItemStack.StackLocationTypes.Part;
		}
	}

	// Token: 0x06007E6F RID: 32367 RVA: 0x000200DA File Offset: 0x0001E2DA
	public override ItemStack[] GetSlots()
	{
		return null;
	}

	// Token: 0x06007E70 RID: 32368 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void UpdateBackend(ItemStack[] stackList)
	{
	}

	// Token: 0x06007E71 RID: 32369 RVA: 0x00301274 File Offset: 0x002FF474
	public void SetPart(ItemValue part, int index)
	{
		XUiC_ItemStack xuiC_ItemStack = this.itemControllers[index];
		if (part != null && !part.IsEmpty())
		{
			ItemStack itemStack = new ItemStack(part.Clone(), 1);
			xuiC_ItemStack.ItemStack = itemStack;
			xuiC_ItemStack.GreyedOut = false;
		}
		else
		{
			xuiC_ItemStack.ItemStack = ItemStack.Empty;
			xuiC_ItemStack.GreyedOut = false;
		}
		this.itemControllers[index].ViewComponent.EventOnPress = false;
		this.itemControllers[index].ViewComponent.EventOnHover = false;
	}

	// Token: 0x06007E72 RID: 32370 RVA: 0x003012EC File Offset: 0x002FF4EC
	public void SetAmmoSlot(ItemValue ammo, int count)
	{
		int num = 5;
		int num2 = ((count < 1) ? 1 : count);
		if (ammo != null && !ammo.IsEmpty())
		{
			ItemStack itemStack = new ItemStack(ammo, num2);
			this.itemControllers[num].ItemStack = itemStack;
			this.itemControllers[num].GreyedOut = count == 0;
			return;
		}
		this.itemControllers[num].ItemStack = ItemStack.Empty;
		this.itemControllers[num].GreyedOut = false;
	}

	// Token: 0x06007E73 RID: 32371 RVA: 0x00301357 File Offset: 0x002FF557
	[PublicizedFrom(EAccessModifier.Internal)]
	public void SetMainItem(ItemStack itemStack)
	{
		this.itemClass = itemStack.itemValue.ItemClass;
	}

	// Token: 0x04005DC9 RID: 24009
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemClass itemClass;
}
