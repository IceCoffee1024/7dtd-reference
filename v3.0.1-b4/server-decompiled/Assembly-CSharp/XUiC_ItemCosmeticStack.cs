using System;
using UnityEngine.Scripting;

// Token: 0x02000F3D RID: 3901
[Preserve]
public class XUiC_ItemCosmeticStack : XUiC_BasePartStack
{
	// Token: 0x17000E45 RID: 3653
	// (get) Token: 0x06007A8E RID: 31374 RVA: 0x002E56BB File Offset: 0x002E38BB
	// (set) Token: 0x06007A8F RID: 31375 RVA: 0x002E56C3 File Offset: 0x002E38C3
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

	// Token: 0x17000E46 RID: 3654
	// (get) Token: 0x06007A90 RID: 31376 RVA: 0x002E56D9 File Offset: 0x002E38D9
	// (set) Token: 0x06007A91 RID: 31377 RVA: 0x002E56E1 File Offset: 0x002E38E1
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

	// Token: 0x06007A92 RID: 31378 RVA: 0x002E56F0 File Offset: 0x002E38F0
	public override void Init()
	{
		base.Init();
		this.lblStackMissing = Localization.Get("lblPartStackMissing", false, null);
	}

	// Token: 0x06007A93 RID: 31379 RVA: 0x002E5395 File Offset: 0x002E3595
	public override string GetAtlas()
	{
		if (base.ItemStack.IsEmpty())
		{
			return "ItemIconAtlasGreyscale";
		}
		return "ItemIconAtlas";
	}

	// Token: 0x06007A94 RID: 31380 RVA: 0x002E570C File Offset: 0x002E390C
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

	// Token: 0x06007A95 RID: 31381 RVA: 0x002E5759 File Offset: 0x002E3959
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

	// Token: 0x06007A96 RID: 31382 RVA: 0x002E5790 File Offset: 0x002E3990
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

	// Token: 0x06007A97 RID: 31383 RVA: 0x002E5A74 File Offset: 0x002E3C74
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool CanRemove()
	{
		return this.itemClass is ItemClassModifier && (this.itemClass as ItemClassModifier).Type == ItemClassModifier.ModifierTypes.Attachment;
	}

	// Token: 0x06007A98 RID: 31384 RVA: 0x002E5A98 File Offset: 0x002E3C98
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

	// Token: 0x04005A9F RID: 23199
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemValue itemValue;

	// Token: 0x04005AA0 RID: 23200
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemClass expectedItemClass;

	// Token: 0x04005AA1 RID: 23201
	[PublicizedFrom(EAccessModifier.Private)]
	public string lblStackMissing;
}
