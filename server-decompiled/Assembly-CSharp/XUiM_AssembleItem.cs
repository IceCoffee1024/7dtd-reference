using System;

// Token: 0x0200110B RID: 4363
public class XUiM_AssembleItem : XUiModel
{
	// Token: 0x17000FD9 RID: 4057
	// (get) Token: 0x060089C0 RID: 35264 RVA: 0x00347F51 File Offset: 0x00346151
	// (set) Token: 0x060089C1 RID: 35265 RVA: 0x00347F59 File Offset: 0x00346159
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

	// Token: 0x17000FDA RID: 4058
	// (get) Token: 0x060089C2 RID: 35266 RVA: 0x00347F6B File Offset: 0x0034616B
	// (set) Token: 0x060089C3 RID: 35267 RVA: 0x00347F73 File Offset: 0x00346173
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

	// Token: 0x17000FDB RID: 4059
	// (get) Token: 0x060089C4 RID: 35268 RVA: 0x00347FA4 File Offset: 0x003461A4
	// (set) Token: 0x060089C5 RID: 35269 RVA: 0x00347FAC File Offset: 0x003461AC
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

	// Token: 0x17000FDC RID: 4060
	// (get) Token: 0x060089C6 RID: 35270 RVA: 0x00347FB5 File Offset: 0x003461B5
	// (set) Token: 0x060089C7 RID: 35271 RVA: 0x00347FBD File Offset: 0x003461BD
	public int PartCount
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x060089C8 RID: 35272 RVA: 0x00347FC8 File Offset: 0x003461C8
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetPartCount()
	{
		this.PartCount = 0;
		for (int i = 0; i < this.CurrentItem.itemValue.Modifications.Length; i++)
		{
			if (this.CurrentItem.itemValue.Modifications[i] == null)
			{
				this.CurrentItem.itemValue.Modifications[i] = ItemValue.None;
			}
			if (!this.CurrentItem.itemValue.Modifications[i].IsEmpty())
			{
				int partCount = this.PartCount;
				this.PartCount = partCount + 1;
			}
		}
	}

	// Token: 0x060089C9 RID: 35273 RVA: 0x00348050 File Offset: 0x00346250
	public void RefreshAssembleItem()
	{
		this.PartCount = 0;
		bool flag = false;
		for (int i = 0; i < this.CurrentItem.itemValue.Modifications.Length; i++)
		{
			if (!this.CurrentItem.itemValue.Modifications[i].IsEmpty())
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

	// Token: 0x060089CA RID: 35274 RVA: 0x00348104 File Offset: 0x00346304
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
				for (int i = 0; i < this.CurrentItem.itemValue.CosmeticMods.Length; i++)
				{
					if (this.CurrentItem.itemValue.CosmeticMods[i] != null && this.CurrentItem.itemValue.CosmeticMods[i].ItemClass != null)
					{
						ItemClassModifier itemClassModifier2 = this.CurrentItem.itemValue.CosmeticMods[i].ItemClass as ItemClassModifier;
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
				for (int j = 0; j < this.CurrentItem.itemValue.Modifications.Length; j++)
				{
					if (this.CurrentItem.itemValue.Modifications[j] != null && this.CurrentItem.itemValue.Modifications[j].ItemClass != null && !itemClassModifier.HasAnyTags(EntityDrone.StorageModifierTags))
					{
						ItemClassModifier itemClassModifier3 = this.CurrentItem.itemValue.Modifications[j].ItemClass as ItemClassModifier;
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
				if (this.CurrentItem.itemValue.CosmeticMods != null)
				{
					for (int k = 0; k < this.CurrentItem.itemValue.CosmeticMods.Length; k++)
					{
						if (this.CurrentItem.itemValue.CosmeticMods[k] == null || this.CurrentItem.itemValue.CosmeticMods[k].IsEmpty())
						{
							float num2 = 1f - this.CurrentItem.itemValue.PercentUsesLeft;
							this.CurrentItem.itemValue.CosmeticMods[k] = _partStack.itemValue.Clone();
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
			else if (this.CurrentItem.itemValue.Modifications != null)
			{
				for (int l = 0; l < this.CurrentItem.itemValue.Modifications.Length; l++)
				{
					if (this.CurrentItem.itemValue.Modifications[l] == null || this.CurrentItem.itemValue.Modifications[l].IsEmpty())
					{
						float num3 = 1f - this.CurrentItem.itemValue.PercentUsesLeft;
						this.CurrentItem.itemValue.Modifications[l] = _partStack.itemValue.Clone();
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

	// Token: 0x060089CB RID: 35275 RVA: 0x00348559 File Offset: 0x00346759
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateAssembleWindow()
	{
		if (this.AssembleWindow != null)
		{
			this.AssembleWindow.ItemStack = this.CurrentItem;
			this.AssembleWindow.OnChanged();
		}
	}

	// Token: 0x04006621 RID: 26145
	public XUiC_AssembleWindow AssembleWindow;

	// Token: 0x04006622 RID: 26146
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemStack currentItem;

	// Token: 0x04006623 RID: 26147
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ItemStack currentItemStackController;

	// Token: 0x04006624 RID: 26148
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_EquipmentStack currentEquipmentStackController;
}
