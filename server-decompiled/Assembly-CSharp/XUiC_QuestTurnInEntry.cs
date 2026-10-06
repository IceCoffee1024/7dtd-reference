using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02001008 RID: 4104
[Preserve]
public class XUiC_QuestTurnInEntry : XUiC_SelectableEntry
{
	// Token: 0x17000F36 RID: 3894
	// (get) Token: 0x0600811D RID: 33053 RVA: 0x003113CC File Offset: 0x0030F5CC
	// (set) Token: 0x0600811E RID: 33054 RVA: 0x003113D4 File Offset: 0x0030F5D4
	public BaseReward Reward
	{
		get
		{
			return this.reward;
		}
		set
		{
			this.reward = value;
			this.Refresh();
			if (!base.IsSelected)
			{
				this.background.Color = new Color32(64, 64, 64, byte.MaxValue);
			}
		}
	}

	// Token: 0x17000F37 RID: 3895
	// (get) Token: 0x0600811F RID: 33055 RVA: 0x0031140C File Offset: 0x0030F60C
	public ItemStack Item
	{
		get
		{
			if (this.reward != null && this.item.IsEmpty())
			{
				if (this.reward is RewardItem)
				{
					this.item = (this.reward as RewardItem).Item;
				}
				else if (this.reward is RewardLootItem)
				{
					this.item = (this.reward as RewardLootItem).Item;
				}
			}
			return this.item;
		}
	}

	// Token: 0x17000F38 RID: 3896
	// (get) Token: 0x06008120 RID: 33056 RVA: 0x0031147C File Offset: 0x0030F67C
	// (set) Token: 0x06008121 RID: 33057 RVA: 0x00311484 File Offset: 0x0030F684
	public bool Chosen
	{
		get
		{
			return this.chosen;
		}
		set
		{
			this.chosen = value;
			this.RefreshBackground();
			base.RefreshBindings();
		}
	}

	// Token: 0x06008122 RID: 33058 RVA: 0x00311499 File Offset: 0x0030F699
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void SelectedChanged(bool isSelected)
	{
		this.RefreshBackground();
	}

	// Token: 0x06008123 RID: 33059 RVA: 0x003114A4 File Offset: 0x0030F6A4
	[PublicizedFrom(EAccessModifier.Private)]
	public void RefreshBackground()
	{
		if (this.background != null)
		{
			this.background.Color = (base.IsSelected ? new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue) : new Color32(64, 64, 64, byte.MaxValue));
			this.background.SpriteName = (base.IsSelected ? "ui_game_select_row" : "menu_empty");
		}
	}

	// Token: 0x06008124 RID: 33060 RVA: 0x0031151C File Offset: 0x0030F71C
	public void SetBaseReward(BaseReward reward)
	{
		this.Reward = reward;
		this.item = ItemStack.Empty;
		this.IsDirty = true;
		base.RefreshBindings();
		base.ViewComponent.Enabled = reward != null;
		if (reward == null)
		{
			this.background.Color = new Color32(64, 64, 64, byte.MaxValue);
		}
	}

	// Token: 0x06008125 RID: 33061 RVA: 0x0031157C File Offset: 0x0030F77C
	public override void Init()
	{
		base.Init();
		for (int i = 0; i < this.children.Count; i++)
		{
			XUiView viewComponent = this.children[i].ViewComponent;
			if (viewComponent.ID.EqualsCaseInsensitive("background"))
			{
				this.background = viewComponent as XUiV_Sprite;
			}
		}
		this.IsDirty = true;
	}

	// Token: 0x06008126 RID: 33062 RVA: 0x003115DC File Offset: 0x0030F7DC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnHovered(bool _isOver)
	{
		this.isHovered = _isOver;
		if (this.background != null && this.reward != null && !base.IsSelected)
		{
			if (_isOver)
			{
				this.background.Color = new Color32(96, 96, 96, byte.MaxValue);
			}
			else
			{
				this.background.Color = new Color32(64, 64, 64, byte.MaxValue);
			}
		}
		base.OnHovered(_isOver);
	}

	// Token: 0x06008127 RID: 33063 RVA: 0x00311654 File Offset: 0x0030F854
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string value, string bindingName)
	{
		uint num = <PrivateImplementationDetails>.ComputeStringHash(bindingName);
		if (num <= 1388578781U)
		{
			if (num <= 847165955U)
			{
				if (num != 392967384U)
				{
					if (num != 602070729U)
					{
						if (num == 847165955U)
						{
							if (bindingName == "itemtypeicon")
							{
								if (this.reward != null)
								{
									if (this.item != null && !this.item.IsEmpty())
									{
										if (this.item.itemValue.ItemClass.IsBlock())
										{
											value = Block.list[this.item.itemValue.type].ItemTypeIcon;
										}
										else
										{
											if (this.item.itemValue.ItemClass.AltItemTypeIcon != null && this.item.itemValue.ItemClass.Unlocks != "" && XUiM_ItemStack.CheckKnown(this.xui.playerUI.entityPlayer, this.item.itemValue.ItemClass, this.item.itemValue))
											{
												value = this.item.itemValue.ItemClass.AltItemTypeIcon;
												return true;
											}
											value = this.item.itemValue.ItemClass.ItemTypeIcon;
										}
									}
									else
									{
										value = "";
									}
								}
								else
								{
									value = "";
								}
								return true;
							}
						}
					}
					else if (bindingName == "namecolor")
					{
						value = ((this.reward != null && this.chosen) ? this.rewardchosencolorFormatter.Format(this.selectedColor) : this.rewardchosencolorFormatter.Format(this.defaultColor));
						return true;
					}
				}
				else if (bindingName == "hasotherreward")
				{
					if (this.reward != null)
					{
						if (this.item != null && !this.item.IsEmpty())
						{
							value = "false";
						}
						else
						{
							value = "true";
						}
					}
					else
					{
						value = "true";
					}
					return true;
				}
			}
			else if (num <= 1062608009U)
			{
				if (num != 899280930U)
				{
					if (num == 1062608009U)
					{
						if (bindingName == "durabilitycolor")
						{
							if (this.Item != null && !this.Item.IsEmpty() && this.Item.itemValue.ItemClass.ShowQualityBar)
							{
								Color32 color = QualityInfo.GetTierColor((int)this.Item.itemValue.Quality);
								value = this.durabilityColorFormatter.Format(color);
							}
							else
							{
								value = "0,0,0,0";
							}
							return true;
						}
					}
				}
				else if (bindingName == "chosenicon")
				{
					value = ((this.reward != null && this.chosen) ? "ui_game_symbol_check" : "");
					return true;
				}
			}
			else if (num != 1290001761U)
			{
				if (num == 1388578781U)
				{
					if (bindingName == "hasitemtypeicon")
					{
						if (this.reward != null)
						{
							if (this.item != null && !this.item.IsEmpty())
							{
								if (this.item.itemValue.ItemClass.IsBlock())
								{
									value = (Block.list[this.item.itemValue.type].ItemTypeIcon != "").ToString();
								}
								else
								{
									value = (this.item.itemValue.ItemClass.ItemTypeIcon != "").ToString();
								}
							}
							else
							{
								value = "false";
							}
						}
						else
						{
							value = "false";
						}
						return true;
					}
				}
			}
			else if (bindingName == "rewardname")
			{
				value = ((this.reward != null) ? this.reward.GetRewardText() : "");
				return true;
			}
		}
		else if (num <= 2485383123U)
		{
			if (num <= 2048631988U)
			{
				if (num != 1771022199U)
				{
					if (num == 2048631988U)
					{
						if (bindingName == "hasreward")
						{
							value = (this.reward != null).ToString();
							return true;
						}
					}
				}
				else if (bindingName == "rewardicon")
				{
					if (this.reward != null)
					{
						if (this.item != null && !this.item.IsEmpty())
						{
							value = this.item.itemValue.ItemClass.GetIconName();
						}
						else
						{
							value = this.reward.Icon;
						}
					}
					else
					{
						value = "";
					}
					return true;
				}
			}
			else if (num != 2400639434U)
			{
				if (num == 2485383123U)
				{
					if (bindingName == "hasitemreward")
					{
						if (this.reward != null)
						{
							if (this.item != null && !this.item.IsEmpty())
							{
								value = "true";
							}
							else
							{
								value = "false";
							}
						}
						else
						{
							value = "false";
						}
						return true;
					}
				}
			}
			else if (bindingName == "rewardicontint")
			{
				if (this.reward != null)
				{
					if (this.item != null && !this.item.IsEmpty())
					{
						value = this.rewardiconcolorFormatter.Format(this.item.itemValue.ItemClass.GetIconTint(this.item.itemValue));
					}
					else
					{
						value = "255,255,255,255";
					}
				}
				else
				{
					value = "255,255,255,255";
				}
				return true;
			}
		}
		else if (num <= 4049247086U)
		{
			if (num != 2944858628U)
			{
				if (num == 4049247086U)
				{
					if (bindingName == "itemtypeicontint")
					{
						value = "255,255,255,255";
						if (this.item != null && !this.item.IsEmpty() && this.item.itemValue.ItemClass.Unlocks != "" && XUiM_ItemStack.CheckKnown(this.xui.playerUI.entityPlayer, this.item.itemValue.ItemClass, this.item.itemValue))
						{
							value = this.altitemtypeiconcolorFormatter.Format(this.item.itemValue.ItemClass.AltItemTypeIconColor);
						}
						return true;
					}
				}
			}
			else if (bindingName == "hasdurability")
			{
				value = (this.Item != null && !this.Item.IsEmpty() && this.Item.itemValue.ItemClass.ShowQualityBar).ToString();
				return true;
			}
		}
		else if (num != 4107624007U)
		{
			if (num == 4172540779U)
			{
				if (bindingName == "durabilityfill")
				{
					value = ((this.Item != null && !this.Item.IsEmpty()) ? ((this.Item.itemValue.MaxUseTimes == 0) ? "1" : this.durabilityFillFormatter.Format(this.Item.itemValue.PercentUsesLeft)) : "0");
					return true;
				}
			}
		}
		else if (bindingName == "durabilityvalue")
		{
			value = "";
			if (this.Item != null && !this.Item.IsEmpty())
			{
				if (this.Item.itemValue.HasQuality || (!this.Item.IsEmpty() && this.Item.itemValue.ItemClass.HasSubItems))
				{
					value = ((this.Item.itemValue.Quality > 0) ? this.durabilityValueFormatter.Format((int)this.Item.itemValue.Quality) : "-");
				}
				else
				{
					value = "";
				}
			}
			else
			{
				value = "";
			}
			return true;
		}
		return base.GetBindingValueInternal(ref value, bindingName);
	}

	// Token: 0x06008128 RID: 33064 RVA: 0x00311E34 File Offset: 0x00310034
	public override bool ParseAttribute(string name, string value)
	{
		bool flag = base.ParseAttribute(name, value);
		if (!flag)
		{
			if (!(name == "default_color"))
			{
				if (!(name == "selected_color"))
				{
					return false;
				}
				this.selectedColor = StringParsers.ParseColor32(value);
			}
			else
			{
				this.defaultColor = StringParsers.ParseColor32(value);
			}
			return true;
		}
		return flag;
	}

	// Token: 0x06008129 RID: 33065 RVA: 0x002E0E5B File Offset: 0x002DF05B
	public void Refresh()
	{
		this.IsDirty = true;
		base.RefreshBindings();
	}

	// Token: 0x04005FCD RID: 24525
	[PublicizedFrom(EAccessModifier.Private)]
	public BaseReward reward;

	// Token: 0x04005FCE RID: 24526
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isHovered;

	// Token: 0x04005FCF RID: 24527
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite background;

	// Token: 0x04005FD0 RID: 24528
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemStack item = ItemStack.Empty;

	// Token: 0x04005FD1 RID: 24529
	[PublicizedFrom(EAccessModifier.Private)]
	public bool chosen;

	// Token: 0x04005FD2 RID: 24530
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterXuiRgbaColor recipeicontintcolorFormatter = new CachedStringFormatterXuiRgbaColor();

	// Token: 0x04005FD3 RID: 24531
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterXuiRgbaColor hasingredientsstatecolorFormatter = new CachedStringFormatterXuiRgbaColor();

	// Token: 0x04005FD4 RID: 24532
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterXuiRgbaColor unlockstatecolorFormatter = new CachedStringFormatterXuiRgbaColor();

	// Token: 0x04005FD5 RID: 24533
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterXuiRgbaColor durabilityColorFormatter = new CachedStringFormatterXuiRgbaColor();

	// Token: 0x04005FD6 RID: 24534
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterInt durabilityValueFormatter = new CachedStringFormatterInt();

	// Token: 0x04005FD7 RID: 24535
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterFloat durabilityFillFormatter = new CachedStringFormatterFloat(null);

	// Token: 0x04005FD8 RID: 24536
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterXuiRgbaColor rewardiconcolorFormatter = new CachedStringFormatterXuiRgbaColor();

	// Token: 0x04005FD9 RID: 24537
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterXuiRgbaColor rewardchosencolorFormatter = new CachedStringFormatterXuiRgbaColor();

	// Token: 0x04005FDA RID: 24538
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterXuiRgbaColor altitemtypeiconcolorFormatter = new CachedStringFormatterXuiRgbaColor();

	// Token: 0x04005FDB RID: 24539
	[PublicizedFrom(EAccessModifier.Private)]
	public Color defaultColor;

	// Token: 0x04005FDC RID: 24540
	[PublicizedFrom(EAccessModifier.Private)]
	public Color selectedColor;
}
