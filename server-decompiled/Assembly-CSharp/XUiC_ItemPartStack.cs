using System;
using UnityEngine.Scripting;

// Token: 0x02000F3C RID: 3900
[Preserve]
public class XUiC_ItemPartStack : XUiC_BasePartStack
{
	// Token: 0x17000E43 RID: 3651
	// (get) Token: 0x06007A82 RID: 31362 RVA: 0x002E5346 File Offset: 0x002E3546
	// (set) Token: 0x06007A83 RID: 31363 RVA: 0x002E534E File Offset: 0x002E354E
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

	// Token: 0x17000E44 RID: 3652
	// (get) Token: 0x06007A84 RID: 31364 RVA: 0x002E5364 File Offset: 0x002E3564
	// (set) Token: 0x06007A85 RID: 31365 RVA: 0x002E536C File Offset: 0x002E356C
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

	// Token: 0x06007A86 RID: 31366 RVA: 0x002E537B File Offset: 0x002E357B
	public override void Init()
	{
		base.Init();
		this.lblStackMissing = Localization.Get("lblPartStackMissing", false, null);
	}

	// Token: 0x06007A87 RID: 31367 RVA: 0x002E5395 File Offset: 0x002E3595
	public override string GetAtlas()
	{
		if (base.ItemStack.IsEmpty())
		{
			return "ItemIconAtlasGreyscale";
		}
		return "ItemIconAtlas";
	}

	// Token: 0x06007A88 RID: 31368 RVA: 0x002E53B0 File Offset: 0x002E35B0
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

	// Token: 0x06007A89 RID: 31369 RVA: 0x002E53FD File Offset: 0x002E35FD
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

	// Token: 0x06007A8A RID: 31370 RVA: 0x002E5434 File Offset: 0x002E3634
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
		if (itemClassModifier.ModifierTags.Test_AnySet(ItemClassModifier.CosmeticModTypes) && itemValue.CosmeticMods.Length != 0)
		{
			for (int i = 0; i < itemValue.CosmeticMods.Length; i++)
			{
				if (itemValue.CosmeticMods[i] == null || itemValue.CosmeticMods[i].IsEmpty())
				{
					return true;
				}
			}
			return false;
		}
		bool flag = itemClassModifier.InstallableTags.IsEmpty || itemClass.HasAnyTags(itemClassModifier.InstallableTags);
		int num = 0;
		ItemClass itemClass2 = base.ItemClass;
		for (int j = 0; j < itemValue.Modifications.Length; j++)
		{
			if (itemValue.Modifications[j].ItemClass != null)
			{
				ItemClassModifier itemClassModifier2 = itemValue.Modifications[j].ItemClass as ItemClassModifier;
				if (itemClassModifier2 != null && !itemValue.Modifications[j].Equals(this.itemValue) && itemClassModifier.ModifierTags.Test_AnySet(itemClassModifier2.ModifierTags))
				{
					num++;
				}
			}
		}
		return num < stack.itemValue.ItemClass.MaxModsAllowed && flag && (this.itemValue == null || this.itemValue.type == 0 || (this.itemValue.ItemClass as ItemClassModifier).Type == ItemClassModifier.ModifierTypes.Attachment);
	}

	// Token: 0x06007A8B RID: 31371 RVA: 0x002E55D8 File Offset: 0x002E37D8
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

	// Token: 0x06007A8C RID: 31372 RVA: 0x002E5640 File Offset: 0x002E3840
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void SwapItem()
	{
		ItemClassModifier itemClassModifier = this.xui.DragAndDropWindow.CurrentStack.itemValue.ItemClass as ItemClassModifier;
		base.SwapItem();
		if (itemClassModifier != null && itemClassModifier.ModifierTags.Test_AnySet(ItemClassModifier.CosmeticModTypes))
		{
			this.xui.DragAndDropWindow.CurrentStack = ItemStack.Empty;
			this.xui.DragAndDropWindow.PickUpType = base.StackLocation;
		}
	}

	// Token: 0x04005A9C RID: 23196
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemValue itemValue;

	// Token: 0x04005A9D RID: 23197
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemClass expectedItemClass;

	// Token: 0x04005A9E RID: 23198
	[PublicizedFrom(EAccessModifier.Private)]
	public string lblStackMissing;
}
