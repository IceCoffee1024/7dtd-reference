using System;
using UnityEngine.Scripting;

// Token: 0x02000F89 RID: 3977
[Preserve]
public class XUiC_PartList : XUiC_ItemStackGrid
{
	// Token: 0x06007D6C RID: 32108 RVA: 0x002F99F8 File Offset: 0x002F7BF8
	public override void Init()
	{
		base.Init();
		foreach (XUiC_ItemStack xuiC_ItemStack in this.itemControllers)
		{
			xuiC_ItemStack.ViewComponent.IsNavigatable = false;
			xuiC_ItemStack.StackLocation = XUiC_ItemStack.StackLocationTypes.Part;
		}
	}

	// Token: 0x06007D6D RID: 32109 RVA: 0x0001FF1E File Offset: 0x0001E11E
	public override ItemStack[] GetSlots()
	{
		return null;
	}

	// Token: 0x06007D6E RID: 32110 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void UpdateBackend(ItemStack[] stackList)
	{
	}

	// Token: 0x06007D6F RID: 32111 RVA: 0x002F9A38 File Offset: 0x002F7C38
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

	// Token: 0x06007D70 RID: 32112 RVA: 0x002F9AB0 File Offset: 0x002F7CB0
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

	// Token: 0x06007D71 RID: 32113 RVA: 0x002F9B40 File Offset: 0x002F7D40
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

	// Token: 0x06007D72 RID: 32114 RVA: 0x002F9BAB File Offset: 0x002F7DAB
	[PublicizedFrom(EAccessModifier.Internal)]
	public void SetMainItem(ItemStack itemStack)
	{
		this.itemClass = itemStack.itemValue.ItemClass;
	}

	// Token: 0x04005CD9 RID: 23769
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemClass itemClass;
}
