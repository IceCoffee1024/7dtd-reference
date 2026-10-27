using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020010AC RID: 4268
[Preserve]
public class XUiC_TraderItemEntry : XUiC_SelectableEntry
{
	// Token: 0x17000FEC RID: 4076
	// (get) Token: 0x06008733 RID: 34611 RVA: 0x0033A3C6 File Offset: 0x003385C6
	// (set) Token: 0x06008734 RID: 34612 RVA: 0x0033A3CE File Offset: 0x003385CE
	public XUiC_TraderWindow TraderWindow { get; set; }

	// Token: 0x17000FED RID: 4077
	// (get) Token: 0x06008735 RID: 34613 RVA: 0x0033A3D7 File Offset: 0x003385D7
	// (set) Token: 0x06008736 RID: 34614 RVA: 0x0033A3DF File Offset: 0x003385DF
	public XUiC_ItemInfoWindow InfoWindow { get; set; }

	// Token: 0x17000FEE RID: 4078
	// (get) Token: 0x06008737 RID: 34615 RVA: 0x0033A3E8 File Offset: 0x003385E8
	// (set) Token: 0x06008738 RID: 34616 RVA: 0x0033A3F0 File Offset: 0x003385F0
	public ItemStack Item
	{
		get
		{
			return this.item;
		}
		set
		{
			this.item = value;
			this.IsDirty = true;
			this.itemClass = ((this.item == null) ? null : this.item.itemValue.ItemClass);
			base.ViewComponent.Enabled = (base.ViewComponent.IsNavigatable = this.item != null);
			base.RefreshBindings();
			if (this.item == null)
			{
				this.background.Color = new Color32(64, 64, 64, byte.MaxValue);
			}
		}
	}

	// Token: 0x06008739 RID: 34617 RVA: 0x0033A47C File Offset: 0x0033867C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void SelectedChanged(bool isSelected)
	{
		if (this.background != null)
		{
			this.background.Color = (isSelected ? new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue) : new Color32(64, 64, 64, byte.MaxValue));
			this.background.SpriteName = (isSelected ? "ui_game_select_row" : "menu_empty");
		}
	}

	// Token: 0x0600873A RID: 34618 RVA: 0x0033A4EC File Offset: 0x003386EC
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

	// Token: 0x0600873B RID: 34619 RVA: 0x0033A54C File Offset: 0x0033874C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnHovered(bool _isOver)
	{
		this.isHovered = _isOver;
		if (this.background != null && this.item != null && !base.IsSelected)
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

	// Token: 0x0600873C RID: 34620 RVA: 0x0033A5C4 File Offset: 0x003387C4
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string value, string bindingName)
	{
		uint num = <PrivateImplementationDetails>.ComputeStringHash(bindingName);
		if (num <= 2200816055U)
		{
			if (num <= 1062608009U)
			{
				if (num <= 847165955U)
				{
					if (num != 143608997U)
					{
						if (num == 847165955U)
						{
							if (bindingName == "itemtypeicon")
							{
								if (this.item == null)
								{
									value = "";
								}
								else if (this.item.itemValue.ItemClass.IsBlock())
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
								return true;
							}
						}
					}
					else if (bindingName == "removeddurabilityfill")
					{
						ItemStack itemStack = this.item;
						if (((itemStack != null) ? itemStack.itemValue : null) == null)
						{
							value = "1";
							return true;
						}
						value = this.removedDurabilityFillFormatter.Format(this.item.itemValue.MaxDurabilityModifier);
						return true;
					}
				}
				else if (num != 968607449U)
				{
					if (num == 1062608009U)
					{
						if (bindingName == "durabilitycolor")
						{
							if (this.item != null && !this.item.IsEmpty() && this.item.itemValue.ItemClass.ShowQualityBar)
							{
								Color32 color = QualityInfo.GetTierColor((int)this.item.itemValue.Quality);
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
				else if (bindingName == "statecolor")
				{
					if (this.item != null)
					{
						Color32 color2 = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
						value = this.stateColorFormatter.Format(color2);
					}
					else
					{
						value = "255,255,255,255";
					}
					return true;
				}
			}
			else if (num <= 1388578781U)
			{
				if (num != 1159116676U)
				{
					if (num == 1388578781U)
					{
						if (bindingName == "hasitemtypeicon")
						{
							if (this.item == null)
							{
								value = "false";
							}
							else if (this.item.itemValue.ItemClass.IsBlock())
							{
								value = (Block.list[this.item.itemValue.type].ItemTypeIcon != "").ToString();
							}
							else
							{
								value = (this.item.itemValue.ItemClass.ItemTypeIcon != "").ToString();
							}
							return true;
						}
					}
				}
				else if (bindingName == "hasitem")
				{
					value = (this.item != null).ToString();
					return true;
				}
			}
			else if (num != 1580050147U)
			{
				if (num == 2200816055U)
				{
					if (bindingName == "itemprice")
					{
						if (this.item != null)
						{
							int num2 = (this.itemClass.IsBlock() ? Block.list[this.item.itemValue.type].EconomicBundleSize : this.itemClass.EconomicBundleSize);
							value = this.itemPriceFormatter.Format(XUiM_Trader.GetBuyPrice(this.xui, this.item.itemValue, num2, null, this.SlotIndex));
						}
						else
						{
							value = "";
						}
						return true;
					}
				}
			}
			else if (bindingName == "currencyicon")
			{
				if (this.item != null)
				{
					value = TraderInfo.CurrencyItem;
				}
				else
				{
					value = "";
				}
				return true;
			}
		}
		else if (num <= 3708628627U)
		{
			if (num <= 2944858628U)
			{
				if (num != 2870857318U)
				{
					if (num == 2944858628U)
					{
						if (bindingName == "hasdurability")
						{
							value = (this.item != null && !this.item.IsEmpty() && this.item.itemValue.ItemClass.ShowQualityBar).ToString();
							return true;
						}
					}
				}
				else if (bindingName == "hasBoostedIcon")
				{
					ItemStack itemStack2 = this.item;
					value = ((itemStack2 != null) ? itemStack2.itemValue.HasAnyBoostedStats().ToString() : null) ?? "false";
					return true;
				}
			}
			else if (num != 3191456325U)
			{
				if (num == 3708628627U)
				{
					if (bindingName == "itemicon")
					{
						if (this.item != null)
						{
							value = this.item.itemValue.GetPropertyOverride("CustomIcon", this.itemClass.GetIconName());
						}
						else
						{
							value = "";
						}
						return true;
					}
				}
			}
			else if (bindingName == "itemname")
			{
				value = "";
				if (this.item != null)
				{
					if (this.item.count == 1)
					{
						value = this.itemClass.GetLocalizedItemName();
					}
					else
					{
						value = this.itemNameFormatter.Format(this.itemClass.GetLocalizedItemName(), this.item.count);
					}
				}
				return true;
			}
		}
		else if (num <= 4049247086U)
		{
			if (num != 3868891837U)
			{
				if (num == 4049247086U)
				{
					if (bindingName == "itemtypeicontint")
					{
						value = "255,255,255,255";
						if (this.item != null && this.item.itemValue.ItemClass.Unlocks != "" && XUiM_ItemStack.CheckKnown(this.xui.playerUI.entityPlayer, this.item.itemValue.ItemClass, this.item.itemValue))
						{
							value = this.altitemtypeiconcolorFormatter.Format(this.item.itemValue.ItemClass.AltItemTypeIconColor);
						}
						return true;
					}
				}
			}
			else if (bindingName == "pricecolor")
			{
				value = "255,255,255,255";
				if (this.item != null)
				{
					TraderData traderData = this.xui.Trader.TraderData;
					TraderInfo traderInfo = traderData.TraderInfo;
					if (this.xui.Trader.Trader is TileEntityVendingMachine && (traderInfo.PlayerOwned || traderInfo.Rentable))
					{
						int markup = (int)traderData.PrimaryInventory[this.SlotIndex].Markup;
						if (markup > 0)
						{
							value = "255,0,0,255";
						}
						else if (markup < 0)
						{
							value = "0,255,0,255";
						}
					}
				}
				return true;
			}
		}
		else if (num != 4053908414U)
		{
			if (num != 4107624007U)
			{
				if (num == 4172540779U)
				{
					if (bindingName == "durabilityfill")
					{
						value = ((this.item != null && !this.item.IsEmpty()) ? ((this.item.itemValue.MaxUseTimes == 0) ? "1" : this.durabilityFillFormatter.Format(((float)this.item.itemValue.MaxUseTimes - this.item.itemValue.UseTimes) / (float)this.item.itemValue.MaxUseTimesUI)) : "1");
						return true;
					}
				}
			}
			else if (bindingName == "durabilityvalue")
			{
				value = "";
				if (this.item != null)
				{
					if (this.item.itemValue.HasQuality || this.itemClass.HasSubItems)
					{
						value = ((this.item.itemValue.Quality > 0) ? this.durabilityValueFormatter.Format((int)this.item.itemValue.Quality) : "-");
					}
					else
					{
						value = "-";
					}
				}
				return true;
			}
		}
		else if (bindingName == "itemicontint")
		{
			Color32 color3 = Color.white;
			if (this.item != null)
			{
				color3 = this.item.itemValue.ItemClass.GetIconTint(this.item.itemValue);
			}
			value = this.itemicontintcolorFormatter.Format(color3);
			return true;
		}
		return base.GetBindingValueInternal(ref value, bindingName);
	}

	// Token: 0x0600873D RID: 34621 RVA: 0x0033AE78 File Offset: 0x00339078
	public void Refresh()
	{
		if (base.IsSelected)
		{
			this.InfoWindow.SetItemStack(this, false);
		}
		this.IsDirty = true;
		base.RefreshBindings();
	}

	// Token: 0x0400647D RID: 25725
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemStack item;

	// Token: 0x0400647E RID: 25726
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemClass itemClass;

	// Token: 0x0400647F RID: 25727
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isHovered;

	// Token: 0x04006480 RID: 25728
	[PublicizedFrom(EAccessModifier.Private)]
	public bool hasIngredients;

	// Token: 0x04006481 RID: 25729
	public int SlotIndex;

	// Token: 0x04006482 RID: 25730
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite background;

	// Token: 0x04006485 RID: 25733
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<string, int> itemNameFormatter = new CachedStringFormatter<string, int>((string _s, int _i) => string.Format("{0} ({1})", _s, _i));

	// Token: 0x04006486 RID: 25734
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterXuiRgbaColor itemicontintcolorFormatter = new CachedStringFormatterXuiRgbaColor();

	// Token: 0x04006487 RID: 25735
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterXuiRgbaColor durabilityColorFormatter = new CachedStringFormatterXuiRgbaColor();

	// Token: 0x04006488 RID: 25736
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterInt durabilityValueFormatter = new CachedStringFormatterInt();

	// Token: 0x04006489 RID: 25737
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterFloat durabilityFillFormatter = new CachedStringFormatterFloat(null);

	// Token: 0x0400648A RID: 25738
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterFloat removedDurabilityFillFormatter = new CachedStringFormatterFloat(null);

	// Token: 0x0400648B RID: 25739
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterXuiRgbaColor stateColorFormatter = new CachedStringFormatterXuiRgbaColor();

	// Token: 0x0400648C RID: 25740
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterInt itemPriceFormatter = new CachedStringFormatterInt();

	// Token: 0x0400648D RID: 25741
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterXuiRgbaColor altitemtypeiconcolorFormatter = new CachedStringFormatterXuiRgbaColor();
}
