using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000FF9 RID: 4089
[Preserve]
public class XUiC_QuestTurnInEntry : XUiC_SelectableEntry
{
	// Token: 0x17000F0F RID: 3855
	// (get) Token: 0x0600810E RID: 33038 RVA: 0x0030E3E4 File Offset: 0x0030C5E4
	// (set) Token: 0x0600810F RID: 33039 RVA: 0x0030E3EC File Offset: 0x0030C5EC
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

	// Token: 0x17000F10 RID: 3856
	// (get) Token: 0x06008110 RID: 33040 RVA: 0x0030E424 File Offset: 0x0030C624
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

	// Token: 0x17000F11 RID: 3857
	// (get) Token: 0x06008111 RID: 33041 RVA: 0x0030E494 File Offset: 0x0030C694
	// (set) Token: 0x06008112 RID: 33042 RVA: 0x0030E49C File Offset: 0x0030C69C
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

	// Token: 0x06008113 RID: 33043 RVA: 0x0030E4B1 File Offset: 0x0030C6B1
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void SelectedChanged(bool isSelected)
	{
		this.RefreshBackground();
	}

	// Token: 0x06008114 RID: 33044 RVA: 0x0030E4BC File Offset: 0x0030C6BC
	[PublicizedFrom(EAccessModifier.Private)]
	public void RefreshBackground()
	{
		if (this.background != null)
		{
			this.background.Color = (base.IsSelected ? new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue) : new Color32(64, 64, 64, byte.MaxValue));
			this.background.SpriteName = (base.IsSelected ? "ui_game_select_row" : "menu_empty");
		}
	}

	// Token: 0x06008115 RID: 33045 RVA: 0x0030E534 File Offset: 0x0030C734
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

	// Token: 0x06008116 RID: 33046 RVA: 0x0030E594 File Offset: 0x0030C794
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

	// Token: 0x06008117 RID: 33047 RVA: 0x0030E5F4 File Offset: 0x0030C7F4
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

	// Token: 0x06008118 RID: 33048 RVA: 0x0030E66C File Offset: 0x0030C86C
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

	// Token: 0x06008119 RID: 33049 RVA: 0x0030EE4C File Offset: 0x0030D04C
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

	// Token: 0x0600811A RID: 33050 RVA: 0x002DDABF File Offset: 0x002DBCBF
	public void Refresh()
	{
		this.IsDirty = true;
		base.RefreshBindings();
	}

	// Token: 0x04005FA4 RID: 24484
	[PublicizedFrom(EAccessModifier.Private)]
	public BaseReward reward;

	// Token: 0x04005FA5 RID: 24485
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isHovered;

	// Token: 0x04005FA6 RID: 24486
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite background;

	// Token: 0x04005FA7 RID: 24487
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemStack item = ItemStack.Empty;

	// Token: 0x04005FA8 RID: 24488
	[PublicizedFrom(EAccessModifier.Private)]
	public bool chosen;

	// Token: 0x04005FA9 RID: 24489
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterXuiRgbaColor recipeicontintcolorFormatter = new CachedStringFormatterXuiRgbaColor();

	// Token: 0x04005FAA RID: 24490
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterXuiRgbaColor hasingredientsstatecolorFormatter = new CachedStringFormatterXuiRgbaColor();

	// Token: 0x04005FAB RID: 24491
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterXuiRgbaColor unlockstatecolorFormatter = new CachedStringFormatterXuiRgbaColor();

	// Token: 0x04005FAC RID: 24492
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterXuiRgbaColor durabilityColorFormatter = new CachedStringFormatterXuiRgbaColor();

	// Token: 0x04005FAD RID: 24493
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterInt durabilityValueFormatter = new CachedStringFormatterInt();

	// Token: 0x04005FAE RID: 24494
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterFloat durabilityFillFormatter = new CachedStringFormatterFloat(null);

	// Token: 0x04005FAF RID: 24495
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterXuiRgbaColor rewardiconcolorFormatter = new CachedStringFormatterXuiRgbaColor();

	// Token: 0x04005FB0 RID: 24496
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterXuiRgbaColor rewardchosencolorFormatter = new CachedStringFormatterXuiRgbaColor();

	// Token: 0x04005FB1 RID: 24497
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterXuiRgbaColor altitemtypeiconcolorFormatter = new CachedStringFormatterXuiRgbaColor();

	// Token: 0x04005FB2 RID: 24498
	[PublicizedFrom(EAccessModifier.Private)]
	public Color defaultColor;

	// Token: 0x04005FB3 RID: 24499
	[PublicizedFrom(EAccessModifier.Private)]
	public Color selectedColor;
}
