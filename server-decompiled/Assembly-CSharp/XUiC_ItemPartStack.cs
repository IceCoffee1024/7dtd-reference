using System;
using UnityEngine.Scripting;

// Token: 0x02000F71 RID: 3953
[Preserve]
public class XUiC_ItemPartStack : XUiC_BasePartStack
{
	// Token: 0x17000E85 RID: 3717
	// (get) Token: 0x06007B7B RID: 31611 RVA: 0x002EDE16 File Offset: 0x002EC016
	// (set) Token: 0x06007B7C RID: 31612 RVA: 0x002EDE1E File Offset: 0x002EC01E
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

	// Token: 0x17000E86 RID: 3718
	// (get) Token: 0x06007B7D RID: 31613 RVA: 0x002EDE34 File Offset: 0x002EC034
	// (set) Token: 0x06007B7E RID: 31614 RVA: 0x002EDE3C File Offset: 0x002EC03C
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

	// Token: 0x06007B7F RID: 31615 RVA: 0x002EDE4B File Offset: 0x002EC04B
	public override void Init()
	{
		base.Init();
		this.lblStackMissing = Localization.Get("lblPartStackMissing", false, null);
	}

	// Token: 0x06007B80 RID: 31616 RVA: 0x002EDE65 File Offset: 0x002EC065
	public override string GetAtlas()
	{
		if (base.ItemStack.IsEmpty())
		{
			return "ItemIconAtlasGreyscale";
		}
		return "ItemIconAtlas";
	}

	// Token: 0x06007B81 RID: 31617 RVA: 0x002EDE80 File Offset: 0x002EC080
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

	// Token: 0x06007B82 RID: 31618 RVA: 0x002EDECD File Offset: 0x002EC0CD
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

	// Token: 0x06007B83 RID: 31619 RVA: 0x002EDF04 File Offset: 0x002EC104
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool CanSwap(ItemStack stack)
	{
		ItemClassModifier itemClassModifier = stack.itemValue.ItemClass as ItemClassModifier;
		if (itemClassModifier == null)
		{
			return false;
		}
		ItemValue itemValue = this.xui.AssembleItem.CurrentItem.itemValue;
		ItemClass itemClass = itemValue.ItemClass;
		if (itemClass.HasAnyTags(itemClassModifier.DisallowedTags))
		{
			return false;
		}
		if (!itemClass.HasAnyTags(itemClassModifier.InstallableTags))
		{
			return false;
		}
		if (itemClassModifier.HasAnyTags(EntityDrone.StorageModifierTags))
		{
			return true;
		}
		if (itemClassModifier.ModifierTags.Test_AnySet(ItemClassModifier.CosmeticModTypes) && itemValue.CosmeticModCount > 0)
		{
			for (int i = 0; i < itemValue.CosmeticModCount; i++)
			{
				if (itemValue.GetCosmeticMod(i) == null || itemValue.GetCosmeticMod(i).IsEmpty())
				{
					return true;
				}
			}
			return false;
		}
		bool flag = itemClassModifier.InstallableTags.IsEmpty || itemClass.HasAnyTags(itemClassModifier.InstallableTags);
		int num = 0;
		ItemClass itemClass2 = base.ItemClass;
		for (int j = 0; j < itemValue.ModificationCount; j++)
		{
			if (itemValue.GetModification(j).ItemClass != null)
			{
				ItemClassModifier itemClassModifier2 = itemValue.GetModification(j).ItemClass as ItemClassModifier;
				if (itemClassModifier2 != null && !itemValue.GetModification(j).Equals(this.itemValue) && itemClassModifier.ModifierTags.Test_AnySet(itemClassModifier2.ModifierTags))
				{
					num++;
				}
			}
		}
		return num < stack.itemValue.ItemClass.MaxModsAllowed && flag && (this.itemValue == null || this.itemValue.type == 0 || (this.itemValue.ItemClass as ItemClassModifier).Type == ItemClassModifier.ModifierTypes.Attachment);
	}

	// Token: 0x06007B84 RID: 31620 RVA: 0x002EE09C File Offset: 0x002EC29C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool CanRemove()
	{
		bool flag = true;
		ItemClassModifier itemClassModifier = this.itemClass as ItemClassModifier;
		if (itemClassModifier != null)
		{
			flag = flag && itemClassModifier.Type == ItemClassModifier.ModifierTypes.Attachment;
			if (itemClassModifier.ModifierTags.Test_AnySet(EntityVehicle.StorageModifierTags))
			{
				XUiC_VehicleWindowGroup xuiC_VehicleWindowGroup = base.WindowGroup.Controller as XUiC_VehicleWindowGroup;
				if (xuiC_VehicleWindowGroup != null)
				{
					flag = flag && xuiC_VehicleWindowGroup.CurrentVehicleEntity.CanRemoveInventoryMod();
				}
			}
		}
		return flag;
	}

	// Token: 0x06007B85 RID: 31621 RVA: 0x002EE104 File Offset: 0x002EC304
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void SwapItem()
	{
		ItemClassModifier itemClassModifier = this.xui.DragAndDropWindow.CurrentStack.itemValue.ItemClass as ItemClassModifier;
		base.SwapItem();
		if (itemClassModifier != null && itemClassModifier.ModifierTags.Test_AnySet(ItemClassModifier.CosmeticModTypes))
		{
			this.xui.DragAndDropWindow.SetCurrentStack(ItemStack.Empty, true);
			this.xui.DragAndDropWindow.PickUpType = base.StackLocation;
		}
	}

	// Token: 0x04005BBA RID: 23482
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemValue itemValue;

	// Token: 0x04005BBB RID: 23483
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemClass expectedItemClass;

	// Token: 0x04005BBC RID: 23484
	[PublicizedFrom(EAccessModifier.Private)]
	public string lblStackMissing;
}
