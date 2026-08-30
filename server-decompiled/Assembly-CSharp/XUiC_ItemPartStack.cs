using System;
using UnityEngine.Scripting;

// Token: 0x02000F53 RID: 3923
[Preserve]
public class XUiC_ItemPartStack : XUiC_BasePartStack
{
	// Token: 0x17000E5E RID: 3678
	// (get) Token: 0x06007B3B RID: 31547 RVA: 0x002E91D2 File Offset: 0x002E73D2
	// (set) Token: 0x06007B3C RID: 31548 RVA: 0x002E91DA File Offset: 0x002E73DA
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

	// Token: 0x17000E5F RID: 3679
	// (get) Token: 0x06007B3D RID: 31549 RVA: 0x002E91F0 File Offset: 0x002E73F0
	// (set) Token: 0x06007B3E RID: 31550 RVA: 0x002E91F8 File Offset: 0x002E73F8
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

	// Token: 0x06007B3F RID: 31551 RVA: 0x002E9207 File Offset: 0x002E7407
	public override void Init()
	{
		base.Init();
		this.lblStackMissing = Localization.Get("lblPartStackMissing", false, null);
	}

	// Token: 0x06007B40 RID: 31552 RVA: 0x002E9221 File Offset: 0x002E7421
	public override string GetAtlas()
	{
		if (base.ItemStack.IsEmpty())
		{
			return "ItemIconAtlasGreyscale";
		}
		return "ItemIconAtlas";
	}

	// Token: 0x06007B41 RID: 31553 RVA: 0x002E923C File Offset: 0x002E743C
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

	// Token: 0x06007B42 RID: 31554 RVA: 0x002E9289 File Offset: 0x002E7489
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

	// Token: 0x06007B43 RID: 31555 RVA: 0x002E92C0 File Offset: 0x002E74C0
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

	// Token: 0x06007B44 RID: 31556 RVA: 0x002E9464 File Offset: 0x002E7664
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

	// Token: 0x06007B45 RID: 31557 RVA: 0x002E94CC File Offset: 0x002E76CC
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

	// Token: 0x04005B4F RID: 23375
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemValue itemValue;

	// Token: 0x04005B50 RID: 23376
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemClass expectedItemClass;

	// Token: 0x04005B51 RID: 23377
	[PublicizedFrom(EAccessModifier.Private)]
	public string lblStackMissing;
}
