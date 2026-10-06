using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200109C RID: 4252
[Preserve]
public class XUiC_TraderItemEntry : XUiC_SelectableEntry
{
	// Token: 0x17000FE1 RID: 4065
	// (get) Token: 0x060086E0 RID: 34528 RVA: 0x00338342 File Offset: 0x00336542
	// (set) Token: 0x060086E1 RID: 34529 RVA: 0x0033834A File Offset: 0x0033654A
	public XUiC_TraderWindow TraderWindow { get; set; }

	// Token: 0x17000FE2 RID: 4066
	// (get) Token: 0x060086E2 RID: 34530 RVA: 0x00338353 File Offset: 0x00336553
	// (set) Token: 0x060086E3 RID: 34531 RVA: 0x0033835B File Offset: 0x0033655B
	public XUiC_ItemInfoWindow InfoWindow { get; set; }

	// Token: 0x17000FE3 RID: 4067
	// (get) Token: 0x060086E4 RID: 34532 RVA: 0x00338364 File Offset: 0x00336564
	// (set) Token: 0x060086E5 RID: 34533 RVA: 0x0033836C File Offset: 0x0033656C
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

	// Token: 0x060086E6 RID: 34534 RVA: 0x003383F8 File Offset: 0x003365F8
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void SelectedChanged(bool isSelected)
	{
		if (this.background != null)
		{
			this.background.Color = (isSelected ? new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue) : new Color32(64, 64, 64, byte.MaxValue));
			this.background.SpriteName = (isSelected ? "ui_game_select_row" : "menu_empty");
		}
	}

	// Token: 0x060086E7 RID: 34535 RVA: 0x00338468 File Offset: 0x00336668
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

	// Token: 0x060086E8 RID: 34536 RVA: 0x003384C8 File Offset: 0x003366C8
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

	// Token: 0x060086E9 RID: 34537 RVA: 0x00338540 File Offset: 0x00336740
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

	// Token: 0x060086EA RID: 34538 RVA: 0x00338DF4 File Offset: 0x00336FF4
	public void Refresh()
	{
		if (base.IsSelected)
		{
			this.InfoWindow.SetItemStack(this, false);
		}
		this.IsDirty = true;
		base.RefreshBindings();
	}

	// Token: 0x0400643E RID: 25662
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemStack item;

	// Token: 0x0400643F RID: 25663
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemClass itemClass;

	// Token: 0x04006440 RID: 25664
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isHovered;

	// Token: 0x04006441 RID: 25665
	[PublicizedFrom(EAccessModifier.Private)]
	public bool hasIngredients;

	// Token: 0x04006442 RID: 25666
	public int SlotIndex;

	// Token: 0x04006443 RID: 25667
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite background;

	// Token: 0x04006446 RID: 25670
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<string, int> itemNameFormatter = new CachedStringFormatter<string, int>((string _s, int _i) => string.Format("{0} ({1})", _s, _i));

	// Token: 0x04006447 RID: 25671
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterXuiRgbaColor itemicontintcolorFormatter = new CachedStringFormatterXuiRgbaColor();

	// Token: 0x04006448 RID: 25672
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterXuiRgbaColor durabilityColorFormatter = new CachedStringFormatterXuiRgbaColor();

	// Token: 0x04006449 RID: 25673
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterInt durabilityValueFormatter = new CachedStringFormatterInt();

	// Token: 0x0400644A RID: 25674
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterFloat durabilityFillFormatter = new CachedStringFormatterFloat(null);

	// Token: 0x0400644B RID: 25675
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterFloat removedDurabilityFillFormatter = new CachedStringFormatterFloat(null);

	// Token: 0x0400644C RID: 25676
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterXuiRgbaColor stateColorFormatter = new CachedStringFormatterXuiRgbaColor();

	// Token: 0x0400644D RID: 25677
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterInt itemPriceFormatter = new CachedStringFormatterInt();

	// Token: 0x0400644E RID: 25678
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterXuiRgbaColor altitemtypeiconcolorFormatter = new CachedStringFormatterXuiRgbaColor();
}
