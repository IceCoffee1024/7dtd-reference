using System;

// Token: 0x0200114C RID: 4428
public class XUiM_AssembleItem : XUiModel
{
	// Token: 0x17001052 RID: 4178
	// (get) Token: 0x06008B72 RID: 35698 RVA: 0x00352CD1 File Offset: 0x00350ED1
	// (set) Token: 0x06008B73 RID: 35699 RVA: 0x00352CD9 File Offset: 0x00350ED9
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

	// Token: 0x17001053 RID: 4179
	// (get) Token: 0x06008B74 RID: 35700 RVA: 0x00352CEB File Offset: 0x00350EEB
	// (set) Token: 0x06008B75 RID: 35701 RVA: 0x00352CF3 File Offset: 0x00350EF3
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

	// Token: 0x17001054 RID: 4180
	// (get) Token: 0x06008B76 RID: 35702 RVA: 0x00352D24 File Offset: 0x00350F24
	// (set) Token: 0x06008B77 RID: 35703 RVA: 0x00352D2C File Offset: 0x00350F2C
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

	// Token: 0x17001055 RID: 4181
	// (get) Token: 0x06008B78 RID: 35704 RVA: 0x00352D35 File Offset: 0x00350F35
	// (set) Token: 0x06008B79 RID: 35705 RVA: 0x00352D3D File Offset: 0x00350F3D
	public int PartCount
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x06008B7A RID: 35706 RVA: 0x00352D48 File Offset: 0x00350F48
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

	// Token: 0x06008B7B RID: 35707 RVA: 0x00352DC8 File Offset: 0x00350FC8
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

	// Token: 0x06008B7C RID: 35708 RVA: 0x00352E78 File Offset: 0x00351078
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

	// Token: 0x06008B7D RID: 35709 RVA: 0x003532BB File Offset: 0x003514BB
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateAssembleWindow()
	{
		if (this.AssembleWindow != null)
		{
			this.AssembleWindow.ItemStack = this.CurrentItem;
			this.AssembleWindow.OnChanged();
		}
	}

	// Token: 0x0400677D RID: 26493
	public XUiC_AssembleWindow AssembleWindow;

	// Token: 0x0400677E RID: 26494
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemStack currentItem;

	// Token: 0x0400677F RID: 26495
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ItemStack currentItemStackController;

	// Token: 0x04006780 RID: 26496
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_EquipmentStack currentEquipmentStackController;
}
