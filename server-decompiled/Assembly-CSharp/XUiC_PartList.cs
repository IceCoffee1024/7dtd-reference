using System;
using UnityEngine.Scripting;

// Token: 0x02000FA2 RID: 4002
[Preserve]
public class XUiC_PartList : XUiC_ItemStackGrid
{
	// Token: 0x06007E5E RID: 32350 RVA: 0x002FE1CC File Offset: 0x002FC3CC
	public override void Init()
	{
		base.Init();
		foreach (XUiC_ItemStack xuiC_ItemStack in this.itemControllers)
		{
			xuiC_ItemStack.ViewComponent.IsNavigatable = false;
			xuiC_ItemStack.StackLocation = XUiC_ItemStack.StackLocationTypes.Part;
		}
	}

	// Token: 0x06007E5F RID: 32351 RVA: 0x0001FFE2 File Offset: 0x0001E1E2
	public override ItemStack[] GetSlots()
	{
		return null;
	}

	// Token: 0x06007E60 RID: 32352 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void UpdateBackend(ItemStack[] stackList)
	{
	}

	// Token: 0x06007E61 RID: 32353 RVA: 0x002FE20C File Offset: 0x002FC40C
	public void SetSlot(ItemValue part, int index)
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

	// Token: 0x06007E62 RID: 32354 RVA: 0x002FE284 File Offset: 0x002FC484
	public void SetSlots(ItemValue[] parts, int startIndex = 0)
	{
		for (int i = 0; i < this.itemControllers.Length - startIndex; i++)
		{
			int num = i + startIndex;
			XUiC_ItemStack xuiC_ItemStack = this.itemControllers[num];
			if (parts.Length > i && parts[i] != null && !parts[i].IsEmpty())
			{
				ItemStack itemStack = new ItemStack(parts[i].Clone(), 1);
				xuiC_ItemStack.ItemStack = itemStack;
				xuiC_ItemStack.GreyedOut = false;
			}
			else
			{
				xuiC_ItemStack.ItemStack = ItemStack.Empty;
				xuiC_ItemStack.GreyedOut = false;
			}
			xuiC_ItemStack.ViewComponent.EventOnPress = false;
			xuiC_ItemStack.ViewComponent.EventOnHover = false;
		}
	}

	// Token: 0x06007E63 RID: 32355 RVA: 0x002FE314 File Offset: 0x002FC514
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

	// Token: 0x06007E64 RID: 32356 RVA: 0x002FE37F File Offset: 0x002FC57F
	[PublicizedFrom(EAccessModifier.Internal)]
	public void SetMainItem(ItemStack itemStack)
	{
		this.itemClass = itemStack.itemValue.ItemClass;
	}

	// Token: 0x04005DA0 RID: 23968
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemClass itemClass;
}
