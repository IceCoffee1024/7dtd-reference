using System;
using UnityEngine.Scripting;

// Token: 0x02000F54 RID: 3924
[Preserve]
public class XUiC_ItemCosmeticStack : XUiC_BasePartStack
{
	// Token: 0x17000E60 RID: 3680
	// (get) Token: 0x06007B47 RID: 31559 RVA: 0x002E9547 File Offset: 0x002E7747
	// (set) Token: 0x06007B48 RID: 31560 RVA: 0x002E954F File Offset: 0x002E774F
	public ItemValue ItemValue
	{
		get
		{
			return this.itemValue;
		}
		set
		{
			this.itemValue = value;
			base.ItemStack = new ItemStack(value, 1);
		}
	}

	// Token: 0x17000E61 RID: 3681
	// (get) Token: 0x06007B49 RID: 31561 RVA: 0x002E9565 File Offset: 0x002E7765
	// (set) Token: 0x06007B4A RID: 31562 RVA: 0x002E956D File Offset: 0x002E776D
	public ItemClass ExpectedItemClass
	{
		get
		{
			return this.expectedItemClass;
		}
		set
		{
			this.expectedItemClass = value;
			this.SetEmptySpriteName();
		}
	}

	// Token: 0x06007B4B RID: 31563 RVA: 0x002E957C File Offset: 0x002E777C
	public override void Init()
	{
		base.Init();
		this.lblStackMissing = Localization.Get("lblPartStackMissing", false, null);
	}

	// Token: 0x06007B4C RID: 31564 RVA: 0x002E9221 File Offset: 0x002E7421
	public override string GetAtlas()
	{
		if (base.ItemStack.IsEmpty())
		{
			return "ItemIconAtlasGreyscale";
		}
		return "ItemIconAtlas";
	}

	// Token: 0x06007B4D RID: 31565 RVA: 0x002E9598 File Offset: 0x002E7798
	public override string GetPartName()
	{
		if (this.itemClass == null && this.expectedItemClass == null)
		{
			return "";
		}
		if (this.itemClass == null)
		{
			return string.Format(this.lblStackMissing, this.expectedItemClass.GetLocalizedItemName());
		}
		return this.itemClass.GetLocalizedItemName();
	}

	// Token: 0x06007B4E RID: 31566 RVA: 0x002E95E5 File Offset: 0x002E77E5
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void SetEmptySpriteName()
	{
		if (this.expectedItemClass != null && this.expectedItemClass.Id != 0)
		{
			this.emptySpriteName = this.expectedItemClass.GetIconName();
			return;
		}
		this.emptySpriteName = "";
	}

	// Token: 0x06007B4F RID: 31567 RVA: 0x002E961C File Offset: 0x002E781C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool CanSwap(ItemStack stack)
	{
		ItemClassModifier itemClassModifier = stack.itemValue.ItemClass as ItemClassModifier;
		if (itemClassModifier == null)
		{
			return false;
		}
		if (this.xui.AssembleItem.CurrentItem.itemValue.ItemClass.HasAnyTags(itemClassModifier.DisallowedTags))
		{
			return false;
		}
		if (!this.xui.AssembleItem.CurrentItem.itemValue.ItemClass.HasAnyTags(itemClassModifier.InstallableTags))
		{
			return false;
		}
		if (itemClassModifier != null && !itemClassModifier.ModifierTags.Test_AnySet(ItemClassModifier.CosmeticModTypes) && this.xui.AssembleItem.CurrentItem.itemValue.Modifications.Length != 0)
		{
			bool flag = false;
			for (int i = 0; i < this.xui.AssembleItem.CurrentItem.itemValue.Modifications.Length; i++)
			{
				if (this.xui.AssembleItem.CurrentItem.itemValue.Modifications[i] == null || this.xui.AssembleItem.CurrentItem.itemValue.Modifications[i].IsEmpty())
				{
					flag = true;
				}
				else
				{
					ItemClassModifier itemClassModifier2 = this.xui.AssembleItem.CurrentItem.itemValue.Modifications[i].ItemClass as ItemClassModifier;
					if (itemClassModifier2 != null && itemClassModifier.ModifierTags.Test_AnySet(itemClassModifier2.ModifierTags))
					{
						flag = false;
						break;
					}
				}
			}
			return flag;
		}
		bool flag2 = itemClassModifier.InstallableTags.IsEmpty || this.xui.AssembleItem.CurrentItem.itemValue.ItemClass.HasAnyTags(itemClassModifier.InstallableTags);
		for (int j = 0; j < this.xui.AssembleItem.CurrentItem.itemValue.CosmeticMods.Length; j++)
		{
			if (this.xui.AssembleItem.CurrentItem.itemValue.CosmeticMods[j] != null && this.xui.AssembleItem.CurrentItem.itemValue.CosmeticMods[j].ItemClass != null && !this.xui.AssembleItem.CurrentItem.itemValue.CosmeticMods[j].ItemClass.ItemTags.IsEmpty)
			{
				ItemClassModifier itemClassModifier3 = this.xui.AssembleItem.CurrentItem.itemValue.CosmeticMods[j].ItemClass as ItemClassModifier;
				if (itemClassModifier3 != null && this.xui.AssembleItem.CurrentItem.itemValue.CosmeticMods[j] != this.itemValue && itemClassModifier.ModifierTags.Test_AnySet(itemClassModifier3.ModifierTags))
				{
					return false;
				}
			}
		}
		return flag2 && (this.itemValue == null || this.itemValue.type == 0 || (this.itemValue.ItemClass as ItemClassModifier).Type == ItemClassModifier.ModifierTypes.Attachment);
	}

	// Token: 0x06007B50 RID: 31568 RVA: 0x002E9900 File Offset: 0x002E7B00
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool CanRemove()
	{
		return this.itemClass is ItemClassModifier && (this.itemClass as ItemClassModifier).Type == ItemClassModifier.ModifierTypes.Attachment;
	}

	// Token: 0x06007B51 RID: 31569 RVA: 0x002E9924 File Offset: 0x002E7B24
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void SwapItem()
	{
		ItemClassModifier itemClassModifier = this.xui.DragAndDropWindow.CurrentStack.itemValue.ItemClass as ItemClassModifier;
		base.SwapItem();
		if (itemClassModifier != null && !itemClassModifier.ModifierTags.Test_AnySet(ItemClassModifier.CosmeticModTypes) && itemClassModifier != null && !itemClassModifier.ModifierTags.Test_AnySet(ItemClassModifier.CosmeticModTypes) && this.xui.AssembleItem.CurrentItem.itemValue.Modifications.Length != 0)
		{
			bool flag = false;
			for (int i = 0; i < this.xui.AssembleItem.CurrentItem.itemValue.Modifications.Length; i++)
			{
				ItemClassModifier itemClassModifier2 = this.xui.AssembleItem.CurrentItem.itemValue.Modifications[i].ItemClass as ItemClassModifier;
				if (itemClassModifier2 != null && itemClassModifier.ModifierTags.Test_AnySet(itemClassModifier2.ModifierTags))
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				this.xui.DragAndDropWindow.CurrentStack = ItemStack.Empty;
				this.xui.DragAndDropWindow.PickUpType = base.StackLocation;
			}
		}
	}

	// Token: 0x04005B52 RID: 23378
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemValue itemValue;

	// Token: 0x04005B53 RID: 23379
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemClass expectedItemClass;

	// Token: 0x04005B54 RID: 23380
	[PublicizedFrom(EAccessModifier.Private)]
	public string lblStackMissing;
}
