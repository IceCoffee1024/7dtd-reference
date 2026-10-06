using System;

// Token: 0x0200113C RID: 4412
public class XUiM_AssembleItem : XUiModel
{
	// Token: 0x17001047 RID: 4167
	// (get) Token: 0x06008B1E RID: 35614 RVA: 0x00350C21 File Offset: 0x0034EE21
	// (set) Token: 0x06008B1F RID: 35615 RVA: 0x00350C29 File Offset: 0x0034EE29
	public ItemStack CurrentItem
	{
		get
		{
			return this.currentItem;
		}
		set
		{
			this.currentItem = value;
			if (value != null)
			{
				this.SetPartCount();
			}
		}
	}

	// Token: 0x17001048 RID: 4168
	// (get) Token: 0x06008B20 RID: 35616 RVA: 0x00350C3B File Offset: 0x0034EE3B
	// (set) Token: 0x06008B21 RID: 35617 RVA: 0x00350C43 File Offset: 0x0034EE43
	public XUiC_ItemStack CurrentItemStackController
	{
		get
		{
			return this.currentItemStackController;
		}
		set
		{
			if (this.currentItemStackController != null)
			{
				this.currentItemStackController.AssembleLock = false;
			}
			this.currentItemStackController = value;
			if (this.currentItemStackController != null)
			{
				this.currentItemStackController.AssembleLock = true;
			}
		}
	}

	// Token: 0x17001049 RID: 4169
	// (get) Token: 0x06008B22 RID: 35618 RVA: 0x00350C74 File Offset: 0x0034EE74
	// (set) Token: 0x06008B23 RID: 35619 RVA: 0x00350C7C File Offset: 0x0034EE7C
	public XUiC_EquipmentStack CurrentEquipmentStackController
	{
		get
		{
			return this.currentEquipmentStackController;
		}
		set
		{
			this.currentEquipmentStackController = value;
		}
	}

	// Token: 0x1700104A RID: 4170
	// (get) Token: 0x06008B24 RID: 35620 RVA: 0x00350C85 File Offset: 0x0034EE85
	// (set) Token: 0x06008B25 RID: 35621 RVA: 0x00350C8D File Offset: 0x0034EE8D
	public int PartCount
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x06008B26 RID: 35622 RVA: 0x00350C98 File Offset: 0x0034EE98
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetPartCount()
	{
		this.PartCount = 0;
		for (int i = 0; i < this.CurrentItem.itemValue.ModificationCount; i++)
		{
			if (this.CurrentItem.itemValue.GetModification(i) == null)
			{
				this.CurrentItem.itemValue.SetModification(i, ItemValue.None);
			}
			if (!this.CurrentItem.itemValue.GetModification(i).IsEmpty())
			{
				int partCount = this.PartCount;
				this.PartCount = partCount + 1;
			}
		}
	}

	// Token: 0x06008B27 RID: 35623 RVA: 0x00350D18 File Offset: 0x0034EF18
	public void RefreshAssembleItem()
	{
		this.PartCount = 0;
		bool flag = false;
		for (int i = 0; i < this.CurrentItem.itemValue.ModificationCount; i++)
		{
			if (!this.CurrentItem.itemValue.GetModification(i).IsEmpty())
			{
				int partCount = this.PartCount;
				this.PartCount = partCount + 1;
			}
			else
			{
				flag = true;
			}
		}
		if (this.CurrentItemStackController != null)
		{
			this.CurrentItemStackController.ForceSetItemStack(this.CurrentItem);
			this.CurrentItemStackController.AssembleLock = true;
		}
		if (this.currentEquipmentStackController != null)
		{
			this.currentEquipmentStackController.ItemStack = this.CurrentItem;
		}
		if (flag)
		{
			QuestEventManager.Current.AssembledItem(this.CurrentItem);
		}
	}

	// Token: 0x06008B28 RID: 35624 RVA: 0x00350DC8 File Offset: 0x0034EFC8
	public bool AddPartToItem(ItemStack _partStack, out ItemStack _resultStack)
	{
		if (this.CurrentItem == null || this.CurrentItem.IsEmpty())
		{
			_resultStack = _partStack;
			return false;
		}
		ItemClassModifier itemClassModifier = _partStack.itemValue.ItemClass as ItemClassModifier;
		if (itemClassModifier != null)
		{
			if (this.CurrentItem.itemValue.ItemClass.HasAnyTags(itemClassModifier.DisallowedTags))
			{
				_resultStack = _partStack;
				return false;
			}
			if (itemClassModifier.ModifierTags.Test_AnySet(ItemClassModifier.CosmeticModTypes))
			{
				for (int i = 0; i < this.CurrentItem.itemValue.CosmeticModCount; i++)
				{
					if (this.CurrentItem.itemValue.GetCosmeticMod(i) != null && this.CurrentItem.itemValue.GetCosmeticMod(i).ItemClass != null)
					{
						ItemClassModifier itemClassModifier2 = this.CurrentItem.itemValue.GetCosmeticMod(i).ItemClass as ItemClassModifier;
						if (itemClassModifier2 != null && itemClassModifier.ModifierTags.Test_AnySet(itemClassModifier2.ModifierTags))
						{
							_resultStack = _partStack;
							return false;
						}
					}
				}
			}
			else
			{
				int num = 0;
				for (int j = 0; j < this.CurrentItem.itemValue.ModificationCount; j++)
				{
					if (this.CurrentItem.itemValue.GetModification(j) != null && this.CurrentItem.itemValue.GetModification(j).ItemClass != null && !itemClassModifier.HasAnyTags(EntityDrone.StorageModifierTags))
					{
						ItemClassModifier itemClassModifier3 = this.CurrentItem.itemValue.GetModification(j).ItemClass as ItemClassModifier;
						if (itemClassModifier3 != null && itemClassModifier.ModifierTags.Test_AnySet(itemClassModifier3.ModifierTags))
						{
							num++;
						}
					}
				}
				if (num >= itemClassModifier.MaxModsAllowed)
				{
					_resultStack = _partStack;
					return false;
				}
			}
		}
		if (this.CurrentItem.itemValue.ItemClass.HasAnyTags(itemClassModifier.InstallableTags))
		{
			if (itemClassModifier.ModifierTags.Test_AnySet(ItemClassModifier.CosmeticModTypes))
			{
				if (this.CurrentItem.itemValue.CosmeticModCount > 0)
				{
					for (int k = 0; k < this.CurrentItem.itemValue.CosmeticModCount; k++)
					{
						if (this.CurrentItem.itemValue.GetCosmeticMod(k) == null || this.CurrentItem.itemValue.GetCosmeticMod(k).IsEmpty())
						{
							float num2 = 1f - this.CurrentItem.itemValue.PercentUsesLeft;
							this.CurrentItem.itemValue.SetCosmeticMod(k, _partStack.itemValue.Clone());
							if (this.CurrentItemStackController != null)
							{
								XUiC_AssembleWindowGroup.GetWindowGroup(this.CurrentItemStackController.xui).ItemStack = this.CurrentItem;
							}
							if (this.currentEquipmentStackController != null)
							{
								XUiC_AssembleWindowGroup.GetWindowGroup(this.CurrentEquipmentStackController.xui).ItemStack = this.CurrentItem;
							}
							this.RefreshAssembleItem();
							if (this.CurrentItem.itemValue.MaxUseTimes > 0)
							{
								this.CurrentItem.itemValue.UseTimes = (float)((int)(num2 * (float)this.CurrentItem.itemValue.MaxUseTimes));
							}
							this.UpdateAssembleWindow();
							_resultStack = ItemStack.Empty;
							return true;
						}
					}
				}
			}
			else if (this.CurrentItem.itemValue.ModificationCount > 0)
			{
				for (int l = 0; l < this.CurrentItem.itemValue.ModificationCount; l++)
				{
					if (this.CurrentItem.itemValue.GetModification(l) == null || this.CurrentItem.itemValue.GetModification(l).IsEmpty())
					{
						float num3 = 1f - this.CurrentItem.itemValue.PercentUsesLeft;
						this.CurrentItem.itemValue.SetModification(l, _partStack.itemValue.Clone());
						if (this.CurrentItemStackController != null)
						{
							XUiC_AssembleWindowGroup.GetWindowGroup(this.CurrentItemStackController.xui).ItemStack = this.CurrentItem;
						}
						if (this.currentEquipmentStackController != null)
						{
							XUiC_AssembleWindowGroup.GetWindowGroup(this.CurrentEquipmentStackController.xui).ItemStack = this.CurrentItem;
						}
						this.RefreshAssembleItem();
						if (this.CurrentItem.itemValue.MaxUseTimes > 0)
						{
							this.CurrentItem.itemValue.UseTimes = (float)((int)(num3 * (float)this.CurrentItem.itemValue.MaxUseTimes));
						}
						this.UpdateAssembleWindow();
						_resultStack = ItemStack.Empty;
						return true;
					}
				}
			}
		}
		_resultStack = _partStack;
		return false;
	}

	// Token: 0x06008B29 RID: 35625 RVA: 0x0035120B File Offset: 0x0034F40B
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateAssembleWindow()
	{
		if (this.AssembleWindow != null)
		{
			this.AssembleWindow.ItemStack = this.CurrentItem;
			this.AssembleWindow.OnChanged();
		}
	}

	// Token: 0x0400673D RID: 26429
	public XUiC_AssembleWindow AssembleWindow;

	// Token: 0x0400673E RID: 26430
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemStack currentItem;

	// Token: 0x0400673F RID: 26431
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ItemStack currentItemStackController;

	// Token: 0x04006740 RID: 26432
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_EquipmentStack currentEquipmentStackController;
}
