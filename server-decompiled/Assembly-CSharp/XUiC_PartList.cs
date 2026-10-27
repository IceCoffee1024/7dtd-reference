using System;
using UnityEngine.Scripting;

// Token: 0x02000FC1 RID: 4033
[Preserve]
public class XUiC_PartList : XUiC_ItemStackGrid
{
	// Token: 0x06007EBD RID: 32445 RVA: 0x00302D50 File Offset: 0x00300F50
	public override void Init()
	{
		base.Init();
		foreach (XUiC_ItemStack xuiC_ItemStack in this.itemControllers)
		{
			xuiC_ItemStack.ViewComponent.IsNavigatable = false;
			xuiC_ItemStack.StackLocation = XUiC_ItemStack.StackLocationTypes.Part;
		}
	}

	// Token: 0x06007EBE RID: 32446 RVA: 0x000200DA File Offset: 0x0001E2DA
	public override ItemStack[] GetSlots()
	{
		return null;
	}

	// Token: 0x06007EBF RID: 32447 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void UpdateBackend(ItemStack[] stackList)
	{
	}

	// Token: 0x06007EC0 RID: 32448 RVA: 0x00302D90 File Offset: 0x00300F90
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

	// Token: 0x06007EC1 RID: 32449 RVA: 0x00302E08 File Offset: 0x00301008
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

	// Token: 0x06007EC2 RID: 32450 RVA: 0x00302E73 File Offset: 0x00301073
	[PublicizedFrom(EAccessModifier.Internal)]
	public void SetMainItem(ItemStack itemStack)
	{
		this.itemClass = itemStack.itemValue.ItemClass;
	}

	// Token: 0x04005E04 RID: 24068
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemClass itemClass;
}
