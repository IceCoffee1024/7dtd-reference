using System;
using UnityEngine.Scripting;

// Token: 0x02000F62 RID: 3938
[Preserve]
public class XUiC_ItemPartStack : XUiC_BasePartStack
{
	// Token: 0x17000E7B RID: 3707
	// (get) Token: 0x06007B38 RID: 31544 RVA: 0x002EC51A File Offset: 0x002EA71A
	// (set) Token: 0x06007B39 RID: 31545 RVA: 0x002EC522 File Offset: 0x002EA722
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

	// Token: 0x17000E7C RID: 3708
	// (get) Token: 0x06007B3A RID: 31546 RVA: 0x002EC538 File Offset: 0x002EA738
	// (set) Token: 0x06007B3B RID: 31547 RVA: 0x002EC540 File Offset: 0x002EA740
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

	// Token: 0x06007B3C RID: 31548 RVA: 0x002EC54F File Offset: 0x002EA74F
	public override void Init()
	{
		base.Init();
		this.lblStackMissing = Localization.Get("lblPartStackMissing", false, null);
	}

	// Token: 0x06007B3D RID: 31549 RVA: 0x002EC569 File Offset: 0x002EA769
	public override string GetAtlas()
	{
		if (base.ItemStack.IsEmpty())
		{
			return "ItemIconAtlasGreyscale";
		}
		return "ItemIconAtlas";
	}

	// Token: 0x06007B3E RID: 31550 RVA: 0x002EC584 File Offset: 0x002EA784
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

	// Token: 0x06007B3F RID: 31551 RVA: 0x002EC5D1 File Offset: 0x002EA7D1
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

	// Token: 0x06007B40 RID: 31552 RVA: 0x002EC608 File Offset: 0x002EA808
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

	// Token: 0x06007B41 RID: 31553 RVA: 0x002EC7A0 File Offset: 0x002EA9A0
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

	// Token: 0x06007B42 RID: 31554 RVA: 0x002EC808 File Offset: 0x002EAA08
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

	// Token: 0x04005B87 RID: 23431
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemValue itemValue;

	// Token: 0x04005B88 RID: 23432
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemClass expectedItemClass;

	// Token: 0x04005B89 RID: 23433
	[PublicizedFrom(EAccessModifier.Private)]
	public string lblStackMissing;
}
