using System;
using UnityEngine.Scripting;

// Token: 0x02000F72 RID: 3954
[Preserve]
public class XUiC_ItemCosmeticStack : XUiC_BasePartStack
{
	// Token: 0x17000E87 RID: 3719
	// (get) Token: 0x06007B87 RID: 31623 RVA: 0x002EE180 File Offset: 0x002EC380
	// (set) Token: 0x06007B88 RID: 31624 RVA: 0x002EE188 File Offset: 0x002EC388
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

	// Token: 0x17000E88 RID: 3720
	// (get) Token: 0x06007B89 RID: 31625 RVA: 0x002EE19E File Offset: 0x002EC39E
	// (set) Token: 0x06007B8A RID: 31626 RVA: 0x002EE1A6 File Offset: 0x002EC3A6
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

	// Token: 0x06007B8B RID: 31627 RVA: 0x002EE1B5 File Offset: 0x002EC3B5
	public override void Init()
	{
		base.Init();
		this.lblStackMissing = Localization.Get("lblPartStackMissing", false, null);
	}

	// Token: 0x06007B8C RID: 31628 RVA: 0x002EDE65 File Offset: 0x002EC065
	public override string GetAtlas()
	{
		if (base.ItemStack.IsEmpty())
		{
			return "ItemIconAtlasGreyscale";
		}
		return "ItemIconAtlas";
	}

	// Token: 0x06007B8D RID: 31629 RVA: 0x002EE1D0 File Offset: 0x002EC3D0
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

	// Token: 0x06007B8E RID: 31630 RVA: 0x002EE21D File Offset: 0x002EC41D
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

	// Token: 0x06007B8F RID: 31631 RVA: 0x002EE254 File Offset: 0x002EC454
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
		if (itemClassModifier != null && !itemClassModifier.ModifierTags.Test_AnySet(ItemClassModifier.CosmeticModTypes) && this.xui.AssembleItem.CurrentItem.itemValue.ModificationCount > 0)
		{
			bool flag = false;
			for (int i = 0; i < this.xui.AssembleItem.CurrentItem.itemValue.ModificationCount; i++)
			{
				if (this.xui.AssembleItem.CurrentItem.itemValue.GetModification(i) == null || this.xui.AssembleItem.CurrentItem.itemValue.GetModification(i).IsEmpty())
				{
					flag = true;
				}
				else
				{
					ItemClassModifier itemClassModifier2 = this.xui.AssembleItem.CurrentItem.itemValue.GetModification(i).ItemClass as ItemClassModifier;
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
		for (int j = 0; j < this.xui.AssembleItem.CurrentItem.itemValue.CosmeticModCount; j++)
		{
			if (this.xui.AssembleItem.CurrentItem.itemValue.GetCosmeticMod(j) != null && this.xui.AssembleItem.CurrentItem.itemValue.GetCosmeticMod(j).ItemClass != null && !this.xui.AssembleItem.CurrentItem.itemValue.GetCosmeticMod(j).ItemClass.ItemTags.IsEmpty)
			{
				ItemClassModifier itemClassModifier3 = this.xui.AssembleItem.CurrentItem.itemValue.GetCosmeticMod(j).ItemClass as ItemClassModifier;
				if (itemClassModifier3 != null && this.xui.AssembleItem.CurrentItem.itemValue.GetCosmeticMod(j) != this.itemValue && itemClassModifier.ModifierTags.Test_AnySet(itemClassModifier3.ModifierTags))
				{
					return false;
				}
			}
		}
		return flag2 && (this.itemValue == null || this.itemValue.type == 0 || (this.itemValue.ItemClass as ItemClassModifier).Type == ItemClassModifier.ModifierTypes.Attachment);
	}

	// Token: 0x06007B90 RID: 31632 RVA: 0x002EE52C File Offset: 0x002EC72C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool CanRemove()
	{
		return this.itemClass is ItemClassModifier && (this.itemClass as ItemClassModifier).Type == ItemClassModifier.ModifierTypes.Attachment;
	}

	// Token: 0x06007B91 RID: 31633 RVA: 0x002EE550 File Offset: 0x002EC750
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void SwapItem()
	{
		ItemClassModifier itemClassModifier = this.xui.DragAndDropWindow.CurrentStack.itemValue.ItemClass as ItemClassModifier;
		base.SwapItem();
		if (itemClassModifier != null && !itemClassModifier.ModifierTags.Test_AnySet(ItemClassModifier.CosmeticModTypes) && itemClassModifier != null && !itemClassModifier.ModifierTags.Test_AnySet(ItemClassModifier.CosmeticModTypes) && this.xui.AssembleItem.CurrentItem.itemValue.ModificationCount > 0)
		{
			bool flag = false;
			for (int i = 0; i < this.xui.AssembleItem.CurrentItem.itemValue.ModificationCount; i++)
			{
				ItemClassModifier itemClassModifier2 = this.xui.AssembleItem.CurrentItem.itemValue.GetModification(i).ItemClass as ItemClassModifier;
				if (itemClassModifier2 != null && itemClassModifier.ModifierTags.Test_AnySet(itemClassModifier2.ModifierTags))
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				this.xui.DragAndDropWindow.SetCurrentStack(ItemStack.Empty, true);
				this.xui.DragAndDropWindow.PickUpType = base.StackLocation;
			}
		}
	}

	// Token: 0x04005BBD RID: 23485
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemValue itemValue;

	// Token: 0x04005BBE RID: 23486
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemClass expectedItemClass;

	// Token: 0x04005BBF RID: 23487
	[PublicizedFrom(EAccessModifier.Private)]
	public string lblStackMissing;
}
