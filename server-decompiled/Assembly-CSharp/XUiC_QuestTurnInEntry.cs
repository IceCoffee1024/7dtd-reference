using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000FE0 RID: 4064
[Preserve]
public class XUiC_QuestTurnInEntry : XUiC_SelectableEntry
{
	// Token: 0x17000ED6 RID: 3798
	// (get) Token: 0x0600801C RID: 32796 RVA: 0x00309BB0 File Offset: 0x00307DB0
	// (set) Token: 0x0600801D RID: 32797 RVA: 0x00309BB8 File Offset: 0x00307DB8
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

	// Token: 0x17000ED7 RID: 3799
	// (get) Token: 0x0600801E RID: 32798 RVA: 0x00309BF0 File Offset: 0x00307DF0
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

	// Token: 0x17000ED8 RID: 3800
	// (get) Token: 0x0600801F RID: 32799 RVA: 0x00309C60 File Offset: 0x00307E60
	// (set) Token: 0x06008020 RID: 32800 RVA: 0x00309C68 File Offset: 0x00307E68
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

	// Token: 0x06008021 RID: 32801 RVA: 0x00309C7D File Offset: 0x00307E7D
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void SelectedChanged(bool isSelected)
	{
		this.RefreshBackground();
	}

	// Token: 0x06008022 RID: 32802 RVA: 0x00309C88 File Offset: 0x00307E88
	[PublicizedFrom(EAccessModifier.Private)]
	public void RefreshBackground()
	{
		if (this.background != null)
		{
			this.background.Color = (base.IsSelected ? new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue) : new Color32(64, 64, 64, byte.MaxValue));
			this.background.SpriteName = (base.IsSelected ? "ui_game_select_row" : "menu_empty");
		}
	}

	// Token: 0x06008023 RID: 32803 RVA: 0x00309D00 File Offset: 0x00307F00
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

	// Token: 0x06008024 RID: 32804 RVA: 0x00309D60 File Offset: 0x00307F60
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

	// Token: 0x06008025 RID: 32805 RVA: 0x00309DC0 File Offset: 0x00307FC0
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

	// Token: 0x06008026 RID: 32806 RVA: 0x00309E38 File Offset: 0x00308038
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

	// Token: 0x06008027 RID: 32807 RVA: 0x0030A618 File Offset: 0x00308818
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

	// Token: 0x06008028 RID: 32808 RVA: 0x002D9B33 File Offset: 0x002D7D33
	public void Refresh()
	{
		this.IsDirty = true;
		base.RefreshBindings();
	}

	// Token: 0x04005EDD RID: 24285
	[PublicizedFrom(EAccessModifier.Private)]
	public BaseReward reward;

	// Token: 0x04005EDE RID: 24286
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isHovered;

	// Token: 0x04005EDF RID: 24287
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite background;

	// Token: 0x04005EE0 RID: 24288
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemStack item = ItemStack.Empty;

	// Token: 0x04005EE1 RID: 24289
	[PublicizedFrom(EAccessModifier.Private)]
	public bool chosen;

	// Token: 0x04005EE2 RID: 24290
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterXuiRgbaColor recipeicontintcolorFormatter = new CachedStringFormatterXuiRgbaColor();

	// Token: 0x04005EE3 RID: 24291
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterXuiRgbaColor hasingredientsstatecolorFormatter = new CachedStringFormatterXuiRgbaColor();

	// Token: 0x04005EE4 RID: 24292
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterXuiRgbaColor unlockstatecolorFormatter = new CachedStringFormatterXuiRgbaColor();

	// Token: 0x04005EE5 RID: 24293
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterXuiRgbaColor durabilityColorFormatter = new CachedStringFormatterXuiRgbaColor();

	// Token: 0x04005EE6 RID: 24294
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterInt durabilityValueFormatter = new CachedStringFormatterInt();

	// Token: 0x04005EE7 RID: 24295
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterFloat durabilityFillFormatter = new CachedStringFormatterFloat(null);

	// Token: 0x04005EE8 RID: 24296
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterXuiRgbaColor rewardiconcolorFormatter = new CachedStringFormatterXuiRgbaColor();

	// Token: 0x04005EE9 RID: 24297
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterXuiRgbaColor rewardchosencolorFormatter = new CachedStringFormatterXuiRgbaColor();

	// Token: 0x04005EEA RID: 24298
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterXuiRgbaColor altitemtypeiconcolorFormatter = new CachedStringFormatterXuiRgbaColor();

	// Token: 0x04005EEB RID: 24299
	[PublicizedFrom(EAccessModifier.Private)]
	public Color defaultColor;

	// Token: 0x04005EEC RID: 24300
	[PublicizedFrom(EAccessModifier.Private)]
	public Color selectedColor;
}
