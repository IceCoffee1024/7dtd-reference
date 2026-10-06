using System;
using UnityEngine.Scripting;

// Token: 0x02000F63 RID: 3939
[Preserve]
public class XUiC_ItemCosmeticStack : XUiC_BasePartStack
{
	// Token: 0x17000E7D RID: 3709
	// (get) Token: 0x06007B44 RID: 31556 RVA: 0x002EC884 File Offset: 0x002EAA84
	// (set) Token: 0x06007B45 RID: 31557 RVA: 0x002EC88C File Offset: 0x002EAA8C
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

	// Token: 0x17000E7E RID: 3710
	// (get) Token: 0x06007B46 RID: 31558 RVA: 0x002EC8A2 File Offset: 0x002EAAA2
	// (set) Token: 0x06007B47 RID: 31559 RVA: 0x002EC8AA File Offset: 0x002EAAAA
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

	// Token: 0x06007B48 RID: 31560 RVA: 0x002EC8B9 File Offset: 0x002EAAB9
	public override void Init()
	{
		base.Init();
		this.lblStackMissing = Localization.Get("lblPartStackMissing", false, null);
	}

	// Token: 0x06007B49 RID: 31561 RVA: 0x002EC569 File Offset: 0x002EA769
	public override string GetAtlas()
	{
		if (base.ItemStack.IsEmpty())
		{
			return "ItemIconAtlasGreyscale";
		}
		return "ItemIconAtlas";
	}

	// Token: 0x06007B4A RID: 31562 RVA: 0x002EC8D4 File Offset: 0x002EAAD4
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

	// Token: 0x06007B4B RID: 31563 RVA: 0x002EC921 File Offset: 0x002EAB21
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

	// Token: 0x06007B4C RID: 31564 RVA: 0x002EC958 File Offset: 0x002EAB58
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

	// Token: 0x06007B4D RID: 31565 RVA: 0x002ECC30 File Offset: 0x002EAE30
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool CanRemove()
	{
		return this.itemClass is ItemClassModifier && (this.itemClass as ItemClassModifier).Type == ItemClassModifier.ModifierTypes.Attachment;
	}

	// Token: 0x06007B4E RID: 31566 RVA: 0x002ECC54 File Offset: 0x002EAE54
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

	// Token: 0x04005B8A RID: 23434
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemValue itemValue;

	// Token: 0x04005B8B RID: 23435
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemClass expectedItemClass;

	// Token: 0x04005B8C RID: 23436
	[PublicizedFrom(EAccessModifier.Private)]
	public string lblStackMissing;
}
