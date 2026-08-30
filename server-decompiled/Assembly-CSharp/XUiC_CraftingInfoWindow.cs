using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000EF1 RID: 3825
[Preserve]
public class XUiC_CraftingInfoWindow : XUiC_InfoWindow
{
	// Token: 0x17000E06 RID: 3590
	// (get) Token: 0x060077FC RID: 30716 RVA: 0x002D4834 File Offset: 0x002D2A34
	public int SelectedCraftingTier
	{
		get
		{
			return this.selectedCraftingTier;
		}
	}

	// Token: 0x060077FD RID: 30717 RVA: 0x002D483C File Offset: 0x002D2A3C
	public override void Init()
	{
		base.Init();
		this.itemPreview = base.GetChildById("itemPreview");
		this.windowName = base.GetChildById("windowName");
		this.windowIcon = base.GetChildById("windowIcon");
		this.description = base.GetChildById("descriptionText");
		this.craftingTime = base.GetChildById("craftingTime");
		this.addQualityButton = base.GetChildById("addQualityButton");
		this.addQualityButton.OnPress += this.AddQualityButton_OnPress;
		this.subtractQualityButton = base.GetChildById("subtractQualityButton");
		this.subtractQualityButton.OnPress += this.SubtractQualityButton_OnPress;
		this.requiredToolOverlay = base.GetChildById("requiredToolOverlay");
		this.requiredToolCheckmark = base.GetChildById("requiredToolCheckmark");
		this.requiredToolText = base.GetChildById("requiredToolText");
		this.actionItemList = (XUiC_ItemActionList)base.GetChildById("itemActions");
		this.ingredientsButton = base.GetChildById("ingredientsButton");
		this.ingredientsButton.OnPress += this.IngredientsButton_OnPress;
		this.descriptionButton = base.GetChildById("descriptionButton");
		this.descriptionButton.OnPress += this.DescriptionButton_OnPress;
		this.unlockedByButton = base.GetChildById("showunlocksButton");
		this.unlockedByButton.OnPress += this.UnlockedByButton_OnPress;
		this.recipeCraftCount = base.GetChildByType<XUiC_RecipeCraftCount>();
		if (this.recipeCraftCount != null)
		{
			this.recipeCraftCount.OnCountChanged += this.HandleOnCountChanged;
		}
		this.recipeList = this.windowGroup.Controller.GetChildByType<XUiC_RecipeList>();
		if (this.recipeList != null)
		{
			this.recipeList.RecipeChanged += this.HandleRecipeChanged;
		}
		this.categoryList = this.windowGroup.Controller.GetChildByType<XUiC_CategoryList>();
		this.ingredientList = base.GetChildByType<XUiC_IngredientList>();
		this.unlockByList = base.GetChildByType<XUiC_UnlockByList>();
	}

	// Token: 0x060077FE RID: 30718 RVA: 0x002D4A43 File Offset: 0x002D2C43
	[PublicizedFrom(EAccessModifier.Private)]
	public void SubtractQualityButton_OnPress(XUiController _sender, int _mouseButton)
	{
		if (this.selectedCraftingTier > 1)
		{
			this.selectedCraftingTier--;
			this.IsDirty = true;
		}
	}

	// Token: 0x060077FF RID: 30719 RVA: 0x002D4A63 File Offset: 0x002D2C63
	[PublicizedFrom(EAccessModifier.Private)]
	public void AddQualityButton_OnPress(XUiController _sender, int _mouseButton)
	{
		if (this.selectedCraftingTier < XUiM_Recipes.CraftingMaxTier)
		{
			this.selectedCraftingTier++;
			this.IsDirty = true;
		}
	}

	// Token: 0x06007800 RID: 30720 RVA: 0x002D4A88 File Offset: 0x002D2C88
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetSelectedButtonByType(XUiC_CraftingInfoWindow.TabTypes tabType)
	{
		((XUiV_Button)this.ingredientsButton.ViewComponent).Selected = tabType == XUiC_CraftingInfoWindow.TabTypes.Ingredients;
		((XUiV_Button)this.descriptionButton.ViewComponent).Selected = tabType == XUiC_CraftingInfoWindow.TabTypes.Description;
		((XUiV_Button)this.unlockedByButton.ViewComponent).Selected = tabType == XUiC_CraftingInfoWindow.TabTypes.UnlockedBy;
	}

	// Token: 0x06007801 RID: 30721 RVA: 0x002D4AE0 File Offset: 0x002D2CE0
	[PublicizedFrom(EAccessModifier.Private)]
	public void IngredientsButton_OnPress(XUiController _sender, int _mouseButton)
	{
		this.TabType = XUiC_CraftingInfoWindow.TabTypes.Ingredients;
		this.SetSelectedButtonByType(this.TabType);
		this.IsDirty = true;
	}

	// Token: 0x06007802 RID: 30722 RVA: 0x002D4AFC File Offset: 0x002D2CFC
	[PublicizedFrom(EAccessModifier.Private)]
	public void DescriptionButton_OnPress(XUiController _sender, int _mouseButton)
	{
		this.TabType = XUiC_CraftingInfoWindow.TabTypes.Description;
		this.SetSelectedButtonByType(this.TabType);
		this.IsDirty = true;
	}

	// Token: 0x06007803 RID: 30723 RVA: 0x002D4B18 File Offset: 0x002D2D18
	[PublicizedFrom(EAccessModifier.Private)]
	public void UnlockedByButton_OnPress(XUiController _sender, int _mouseButton)
	{
		this.TabType = XUiC_CraftingInfoWindow.TabTypes.UnlockedBy;
		this.SetSelectedButtonByType(this.TabType);
		this.IsDirty = true;
	}

	// Token: 0x06007804 RID: 30724 RVA: 0x002D4B34 File Offset: 0x002D2D34
	public override void OnOpen()
	{
		base.OnOpen();
		if (this.xui.PlayerInventory != null)
		{
			this.xui.PlayerInventory.OnBackpackItemsChanged += this.PlayerInventory_OnItemsChanged;
			this.xui.PlayerInventory.OnToolbeltItemsChanged += this.PlayerInventory_OnItemsChanged;
		}
	}

	// Token: 0x06007805 RID: 30725 RVA: 0x002D4B8C File Offset: 0x002D2D8C
	public override void OnClose()
	{
		base.OnClose();
		if (this.xui.PlayerInventory != null)
		{
			this.xui.PlayerInventory.OnBackpackItemsChanged -= this.PlayerInventory_OnItemsChanged;
			this.xui.PlayerInventory.OnToolbeltItemsChanged -= this.PlayerInventory_OnItemsChanged;
		}
	}

	// Token: 0x06007806 RID: 30726 RVA: 0x0008A069 File Offset: 0x00088269
	[PublicizedFrom(EAccessModifier.Private)]
	public void PlayerInventory_OnItemsChanged()
	{
		this.IsDirty = true;
	}

	// Token: 0x06007807 RID: 30727 RVA: 0x002D4BE4 File Offset: 0x002D2DE4
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleOnCountChanged(XUiController _sender, OnCountChangedEventArgs _e)
	{
		this.craftCount = _e.Count;
		this.IsDirty = true;
	}

	// Token: 0x06007808 RID: 30728 RVA: 0x002D4BF9 File Offset: 0x002D2DF9
	public override void Deselect()
	{
		if (this.selectedEntry != null)
		{
			this.selectedEntry.IsSelected = false;
		}
	}

	// Token: 0x06007809 RID: 30729 RVA: 0x002D4C10 File Offset: 0x002D2E10
	public void SetCategory(string category)
	{
		if (this.categoryList != null)
		{
			XUiC_CategoryEntry currentCategory = this.categoryList.CurrentCategory;
			if (((currentCategory != null) ? currentCategory.CategoryName : null) != category)
			{
				this.categoryList.SetCategory(category);
			}
		}
		if (this.recipeList != null && this.recipeList.GetCategory() != category)
		{
			this.recipeList.SetCategory(category);
		}
	}

	// Token: 0x0600780A RID: 30730 RVA: 0x002D4C78 File Offset: 0x002D2E78
	public override void Update(float _dt)
	{
		if (!this.windowGroup.isShowing)
		{
			return;
		}
		base.Update(_dt);
		if (!this.windowGroup.isShowing)
		{
			return;
		}
		if (this.IsDirty)
		{
			if (this.emptyInfoWindow == null)
			{
				this.emptyInfoWindow = (XUiC_InfoWindow)this.xui.FindWindowGroupByName("backpack").GetChildById("emptyInfoPanel");
			}
			if (this.itemInfoWindow == null)
			{
				this.itemInfoWindow = (XUiC_ItemInfoWindow)this.xui.FindWindowGroupByName("backpack").GetChildById("itemInfoPanel");
			}
			this.lastRecipeSelected = this.recipe;
			this.recipe = ((this.selectedEntry != null) ? this.selectedEntry.Recipe : null);
			bool flag = this.recipe != null;
			if (flag)
			{
				int num = this.recipe.GetCraftingTier(this.xui.playerUI.entityPlayer);
				if (num > XUiM_Recipes.CraftingMaxTier)
				{
					num = XUiM_Recipes.CraftingMaxTier;
				}
				if (this.recipe != this.lastRecipeSelected || (this.recipe == this.lastRecipeSelected && num != this.selectedMaxCraftingTier))
				{
					this.selectedCraftingTier = num;
				}
				this.selectedMaxCraftingTier = Mathf.Min(num, XUiM_Recipes.CraftingMaxTier);
			}
			if (this.emptyInfoWindow != null && !flag && !this.itemInfoWindow.ViewComponent.IsVisible)
			{
				this.emptyInfoWindow.ViewComponent.IsVisible = true;
			}
			if (this.itemPreview == null)
			{
				return;
			}
			XUiView viewComponent = this.itemPreview.ViewComponent;
			XUiView viewComponent2 = this.windowName.ViewComponent;
			XUiView viewComponent3 = this.windowIcon.ViewComponent;
			XUiView viewComponent4 = this.description.ViewComponent;
			XUiView viewComponent5 = this.craftingTime.ViewComponent;
			if (this.ingredientList != null)
			{
				this.ingredientList.CraftingTier = this.selectedCraftingTier;
				this.ingredientList.Recipe = this.recipe;
			}
			if (this.unlockByList != null)
			{
				this.unlockByList.Recipe = this.recipe;
			}
			this.actionItemList.SetCraftingActionList(flag ? XUiC_ItemActionList.ItemActionListTypes.Crafting : XUiC_ItemActionList.ItemActionListTypes.None, this.selectedEntry);
			XUiC_WorkstationToolGrid childByType = this.windowGroup.Controller.GetChildByType<XUiC_WorkstationToolGrid>();
			if (childByType != null && this.selectedEntry != null && this.selectedEntry.Recipe != null && this.selectedEntry.Recipe.craftingToolType != 0)
			{
				this.requiredToolOverlay.ViewComponent.IsVisible = true;
				ItemClass forId = ItemClass.GetForId(this.selectedEntry.Recipe.craftingToolType);
				if (forId != null)
				{
					string text;
					if (forId.IsBlock())
					{
						text = Block.list[forId.Id].GetLocalizedBlockName();
					}
					else
					{
						text = forId.GetLocalizedItemName();
					}
					string text2 = Localization.Get("xuiToolRequired", false, null);
					((XUiV_Label)this.requiredToolText.ViewComponent).Text = string.Format(text2, text);
					if (childByType.HasRequirement(this.selectedEntry.Recipe))
					{
						((XUiV_Sprite)this.requiredToolCheckmark.ViewComponent).Color = this.validColor;
						((XUiV_Sprite)this.requiredToolCheckmark.ViewComponent).SpriteName = this.validSprite;
					}
					else
					{
						((XUiV_Sprite)this.requiredToolCheckmark.ViewComponent).Color = this.invalidColor;
						((XUiV_Sprite)this.requiredToolCheckmark.ViewComponent).SpriteName = this.invalidSprite;
					}
				}
			}
			else
			{
				this.requiredToolOverlay.ViewComponent.IsVisible = false;
			}
			this.recipeCraftCount.RefreshCounts();
			base.RefreshBindings();
			this.IsDirty = false;
		}
	}

	// Token: 0x0600780B RID: 30731 RVA: 0x002D4FF0 File Offset: 0x002D31F0
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string value, string bindingName)
	{
		uint num = <PrivateImplementationDetails>.ComputeStringHash(bindingName);
		if (num <= 2943089870U)
		{
			if (num <= 1022877350U)
			{
				if (num <= 789508807U)
				{
					if (num != 386135447U)
					{
						if (num == 789508807U)
						{
							if (bindingName == "enablesubtractquality")
							{
								if (this.recipe != null && this.recipe.GetOutputItemClass().ShowQualityBar && this.selectedCraftingTier > 1)
								{
									value = "true";
								}
								else
								{
									value = "false";
								}
								return true;
							}
						}
					}
					else if (bindingName == "showunlockedbytab")
					{
						value = "false";
						if (this.recipe != null && !XUiM_Recipes.GetRecipeIsUnlocked(this.xui, this.recipe))
						{
							ItemClass forId = ItemClass.GetForId(this.recipe.itemValueType);
							if (forId != null)
							{
								if (forId.IsBlock())
								{
									value = (forId.GetBlock().UnlockedBy.Length != 0).ToString();
								}
								else
								{
									value = (forId.UnlockedBy.Length != 0).ToString();
								}
							}
						}
						return true;
					}
				}
				else if (num != 847165955U)
				{
					if (num != 936888752U)
					{
						if (num == 1022877350U)
						{
							if (bindingName == "durabilityjustify")
							{
								value = "center";
								if (this.recipe != null && !this.recipe.GetOutputItemClass().ShowQualityBar)
								{
									value = "right";
								}
								return true;
							}
						}
					}
					else if (bindingName == "showingredients")
					{
						value = (this.TabType == XUiC_CraftingInfoWindow.TabTypes.Ingredients).ToString();
						return true;
					}
				}
				else if (bindingName == "itemtypeicon")
				{
					value = "";
					if (this.recipe != null)
					{
						ItemClass forId2 = ItemClass.GetForId(this.recipe.itemValueType);
						if (forId2 != null)
						{
							if (forId2.IsBlock())
							{
								value = forId2.GetBlock().ItemTypeIcon;
							}
							else
							{
								if (forId2.AltItemTypeIcon != null && forId2.Unlocks != "" && XUiM_ItemStack.CheckKnown(this.xui.playerUI.entityPlayer, forId2, null))
								{
									value = forId2.AltItemTypeIcon;
									return true;
								}
								value = forId2.ItemTypeIcon;
							}
						}
					}
					return true;
				}
			}
			else if (num <= 1388578781U)
			{
				if (num != 1062608009U)
				{
					if (num == 1388578781U)
					{
						if (bindingName == "hasitemtypeicon")
						{
							value = "false";
							if (this.recipe != null)
							{
								ItemClass forId3 = ItemClass.GetForId(this.recipe.itemValueType);
								if (forId3 != null)
								{
									if (forId3.IsBlock())
									{
										value = (forId3.GetBlock().ItemTypeIcon != "").ToString();
									}
									else
									{
										value = (forId3.ItemTypeIcon != "").ToString();
									}
								}
							}
							return true;
						}
					}
				}
				else if (bindingName == "durabilitycolor")
				{
					Color32 color = Color.white;
					if (this.recipe != null)
					{
						color = QualityInfo.GetTierColor(this.selectedCraftingTier);
					}
					value = this.durabilitycolorFormatter.Format(color);
					return true;
				}
			}
			else if (num != 1585275412U)
			{
				if (num != 1953932597U)
				{
					if (num == 2943089870U)
					{
						if (bindingName == "enableaddquality")
						{
							if (this.recipe != null && this.recipe.GetOutputItemClass().ShowQualityBar && this.selectedCraftingTier < this.selectedMaxCraftingTier)
							{
								value = "true";
							}
							else
							{
								value = "false";
							}
							return true;
						}
					}
				}
				else if (bindingName == "durabilitytext")
				{
					value = "";
					if (this.recipe != null && this.recipe.GetOutputItemClass().ShowQualityBar)
					{
						value = this.durabilitytextFormatter.Format(this.selectedCraftingTier);
					}
					return true;
				}
			}
			else if (bindingName == "itemgroupicon")
			{
				string text;
				if (this.recipe == null)
				{
					text = "";
				}
				else
				{
					XUiC_CategoryEntry currentCategory = this.categoryList.CurrentCategory;
					text = ((currentCategory != null) ? currentCategory.SpriteName : null);
				}
				value = text;
				return true;
			}
		}
		else if (num <= 3708628627U)
		{
			if (num <= 3154262838U)
			{
				if (num != 2944858628U)
				{
					if (num == 3154262838U)
					{
						if (bindingName == "showdescription")
						{
							value = (this.TabType == XUiC_CraftingInfoWindow.TabTypes.Description).ToString();
							return true;
						}
					}
				}
				else if (bindingName == "hasdurability")
				{
					value = (this.recipe != null && this.recipe.GetOutputItemClass().ShowQualityBar).ToString();
					return true;
				}
			}
			else if (num != 3191456325U)
			{
				if (num != 3262997624U)
				{
					if (num == 3708628627U)
					{
						if (bindingName == "itemicon")
						{
							if (this.recipe != null)
							{
								ItemValue itemValue = new ItemValue(this.recipe.itemValueType, false);
								value = itemValue.GetPropertyOverride("CustomIcon", itemValue.ItemClass.GetIconName());
							}
							return true;
						}
					}
				}
				else if (bindingName == "itemdescription")
				{
					string text2 = "";
					if (this.recipe != null)
					{
						ItemClass forId4 = ItemClass.GetForId(this.recipe.itemValueType);
						if (forId4 != null)
						{
							if (forId4.IsBlock())
							{
								string descriptionKey = Block.list[this.recipe.itemValueType].DescriptionKey;
								if (Localization.Exists(descriptionKey, false))
								{
									text2 = Localization.Get(descriptionKey, false, null);
								}
							}
							else
							{
								string itemDescriptionKey = forId4.GetItemDescriptionKey();
								if (Localization.Exists(itemDescriptionKey, false))
								{
									text2 = Localization.Get(itemDescriptionKey, false, null);
								}
							}
						}
					}
					value = text2;
					return true;
				}
			}
			else if (bindingName == "itemname")
			{
				value = ((this.recipe != null) ? Localization.Get(this.recipe.GetName(), false, null) : "");
				return true;
			}
		}
		else if (num <= 4049247086U)
		{
			if (num != 4044995374U)
			{
				if (num == 4049247086U)
				{
					if (bindingName == "itemtypeicontint")
					{
						value = "255,255,255,255";
						if (this.recipe != null)
						{
							ItemClass forId5 = ItemClass.GetForId(this.recipe.itemValueType);
							if (forId5 != null && forId5.Unlocks != "" && XUiM_ItemStack.CheckKnown(this.xui.playerUI.entityPlayer, forId5, null))
							{
								value = this.altitemtypeiconcolorFormatter.Format(forId5.AltItemTypeIconColor);
							}
						}
						return true;
					}
				}
			}
			else if (bindingName == "craftingtime")
			{
				value = "";
				if (this.recipe != null)
				{
					float recipeCraftTime = XUiM_Recipes.GetRecipeCraftTime(this.xui, this.recipe);
					float num2 = recipeCraftTime * (float)(this.craftCount - 1) + recipeCraftTime;
					value = this.craftingtimeFormatter.Format(num2 + 0.5f);
				}
				return true;
			}
		}
		else if (num != 4053908414U)
		{
			if (num != 4172540779U)
			{
				if (num == 4270832456U)
				{
					if (bindingName == "showunlockedby")
					{
						value = (this.TabType == XUiC_CraftingInfoWindow.TabTypes.UnlockedBy).ToString();
						return true;
					}
				}
			}
			else if (bindingName == "durabilityfill")
			{
				value = ((this.recipe == null) ? "0" : "1");
				return true;
			}
		}
		else if (bindingName == "itemicontint")
		{
			Color32 color2 = Color.white;
			if (this.recipe != null)
			{
				ItemValue itemValue2 = new ItemValue(this.recipe.itemValueType, false);
				color2 = itemValue2.ItemClass.GetIconTint(itemValue2);
			}
			value = this.itemicontintcolorFormatter.Format(color2);
			return true;
		}
		return base.GetBindingValueInternal(ref value, bindingName);
	}

	// Token: 0x0600780C RID: 30732 RVA: 0x002D57E0 File Offset: 0x002D39E0
	public void SetRecipe(XUiC_RecipeEntry _recipeEntry)
	{
		this.selectedEntry = _recipeEntry;
		if (this.recipeCraftCount != null)
		{
			this.recipeCraftCount.IsDirty = true;
			this.craftCount = this.recipeCraftCount.Count;
		}
		else
		{
			this.craftCount = 1;
		}
		if (this.selectedEntry != null && this.selectedEntry.Recipe != null)
		{
			if (XUiM_Recipes.GetRecipeIsUnlocked(this.xui, this.selectedEntry.Recipe))
			{
				this.TabType = XUiC_CraftingInfoWindow.TabTypes.Ingredients;
				this.SetSelectedButtonByType(this.TabType);
			}
			else
			{
				this.TabType = XUiC_CraftingInfoWindow.TabTypes.UnlockedBy;
				this.SetSelectedButtonByType(this.TabType);
			}
		}
		this.IsDirty = true;
	}

	// Token: 0x0600780D RID: 30733 RVA: 0x002D587E File Offset: 0x002D3A7E
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleRecipeChanged(Recipe _recipe, XUiC_RecipeEntry _recipeEntry)
	{
		if (base.WindowGroup.isShowing)
		{
			base.ViewComponent.IsVisible = true;
		}
		this.SetRecipe(_recipeEntry);
	}

	// Token: 0x0600780E RID: 30734 RVA: 0x002D58A0 File Offset: 0x002D3AA0
	public override bool ParseAttribute(string attribute, string value)
	{
		if (base.ParseAttribute(attribute, value))
		{
			return true;
		}
		if (attribute == "valid_color")
		{
			this.validColor = StringParsers.ParseColor32(value);
			return true;
		}
		if (attribute == "invalid_color")
		{
			this.invalidColor = StringParsers.ParseColor32(value);
			return true;
		}
		if (attribute == "valid_sprite")
		{
			this.validSprite = value;
			return true;
		}
		if (!(attribute == "invalid_sprite"))
		{
			return false;
		}
		this.invalidSprite = value;
		return true;
	}

	// Token: 0x0600780F RID: 30735 RVA: 0x002D591E File Offset: 0x002D3B1E
	public void RefreshRecipe()
	{
		if (this.recipeCraftCount != null)
		{
			this.recipeCraftCount.CalculateMaxCount();
		}
	}

	// Token: 0x04005884 RID: 22660
	[PublicizedFrom(EAccessModifier.Private)]
	public Recipe recipe;

	// Token: 0x04005885 RID: 22661
	[PublicizedFrom(EAccessModifier.Private)]
	public int craftCount;

	// Token: 0x04005886 RID: 22662
	[PublicizedFrom(EAccessModifier.Private)]
	public int selectedCraftingTier = 1;

	// Token: 0x04005887 RID: 22663
	[PublicizedFrom(EAccessModifier.Private)]
	public int selectedMaxCraftingTier = 1;

	// Token: 0x04005888 RID: 22664
	[PublicizedFrom(EAccessModifier.Private)]
	public Recipe lastRecipeSelected;

	// Token: 0x04005889 RID: 22665
	public XUiC_CraftingInfoWindow.TabTypes TabType;

	// Token: 0x0400588A RID: 22666
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_RecipeEntry selectedEntry;

	// Token: 0x0400588B RID: 22667
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController itemPreview;

	// Token: 0x0400588C RID: 22668
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController windowName;

	// Token: 0x0400588D RID: 22669
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController windowIcon;

	// Token: 0x0400588E RID: 22670
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController description;

	// Token: 0x0400588F RID: 22671
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController craftingTime;

	// Token: 0x04005890 RID: 22672
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController requiredToolOverlay;

	// Token: 0x04005891 RID: 22673
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController requiredToolCheckmark;

	// Token: 0x04005892 RID: 22674
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController requiredToolText;

	// Token: 0x04005893 RID: 22675
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_IngredientList ingredientList;

	// Token: 0x04005894 RID: 22676
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_UnlockByList unlockByList;

	// Token: 0x04005895 RID: 22677
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_CategoryList categoryList;

	// Token: 0x04005896 RID: 22678
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ItemActionList actionItemList;

	// Token: 0x04005897 RID: 22679
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_RecipeList recipeList;

	// Token: 0x04005898 RID: 22680
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_InfoWindow emptyInfoWindow;

	// Token: 0x04005899 RID: 22681
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ItemInfoWindow itemInfoWindow;

	// Token: 0x0400589A RID: 22682
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_RecipeCraftCount recipeCraftCount;

	// Token: 0x0400589B RID: 22683
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController ingredientsButton;

	// Token: 0x0400589C RID: 22684
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController descriptionButton;

	// Token: 0x0400589D RID: 22685
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController unlockedByButton;

	// Token: 0x0400589E RID: 22686
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController addQualityButton;

	// Token: 0x0400589F RID: 22687
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController subtractQualityButton;

	// Token: 0x040058A0 RID: 22688
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterXuiRgbaColor itemicontintcolorFormatter = new CachedStringFormatterXuiRgbaColor();

	// Token: 0x040058A1 RID: 22689
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<float> craftingtimeFormatter = new CachedStringFormatter<float>((float _time) => string.Format("{0:00}:{1:00}", (int)(_time / 60f), (int)(_time % 60f)));

	// Token: 0x040058A2 RID: 22690
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterXuiRgbaColor durabilitycolorFormatter = new CachedStringFormatterXuiRgbaColor();

	// Token: 0x040058A3 RID: 22691
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterFloat durabilityfillFormatter = new CachedStringFormatterFloat(null);

	// Token: 0x040058A4 RID: 22692
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterInt durabilitytextFormatter = new CachedStringFormatterInt();

	// Token: 0x040058A5 RID: 22693
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterXuiRgbaColor altitemtypeiconcolorFormatter = new CachedStringFormatterXuiRgbaColor();

	// Token: 0x040058A6 RID: 22694
	[PublicizedFrom(EAccessModifier.Private)]
	public Color validColor;

	// Token: 0x040058A7 RID: 22695
	[PublicizedFrom(EAccessModifier.Private)]
	public Color invalidColor;

	// Token: 0x040058A8 RID: 22696
	[PublicizedFrom(EAccessModifier.Private)]
	public string validSprite;

	// Token: 0x040058A9 RID: 22697
	[PublicizedFrom(EAccessModifier.Private)]
	public string invalidSprite;

	// Token: 0x02000EF2 RID: 3826
	public enum TabTypes
	{
		// Token: 0x040058AB RID: 22699
		Ingredients,
		// Token: 0x040058AC RID: 22700
		Description,
		// Token: 0x040058AD RID: 22701
		UnlockedBy
	}
}
