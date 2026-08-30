using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000F08 RID: 3848
[Preserve]
public class XUiC_CraftingInfoWindow : XUiC_InfoWindow
{
	// Token: 0x17000E20 RID: 3616
	// (get) Token: 0x060078B4 RID: 30900 RVA: 0x002D87C0 File Offset: 0x002D69C0
	public int SelectedCraftingTier
	{
		get
		{
			return this.selectedCraftingTier;
		}
	}

	// Token: 0x060078B5 RID: 30901 RVA: 0x002D87C8 File Offset: 0x002D69C8
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

	// Token: 0x060078B6 RID: 30902 RVA: 0x002D89CF File Offset: 0x002D6BCF
	[PublicizedFrom(EAccessModifier.Private)]
	public void SubtractQualityButton_OnPress(XUiController _sender, int _mouseButton)
	{
		if (this.selectedCraftingTier > 1)
		{
			this.selectedCraftingTier--;
			this.IsDirty = true;
		}
	}

	// Token: 0x060078B7 RID: 30903 RVA: 0x002D89EF File Offset: 0x002D6BEF
	[PublicizedFrom(EAccessModifier.Private)]
	public void AddQualityButton_OnPress(XUiController _sender, int _mouseButton)
	{
		if (this.selectedCraftingTier < XUiM_Recipes.CraftingMaxTier)
		{
			this.selectedCraftingTier++;
			this.IsDirty = true;
		}
	}

	// Token: 0x060078B8 RID: 30904 RVA: 0x002D8A14 File Offset: 0x002D6C14
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetSelectedButtonByType(XUiC_CraftingInfoWindow.TabTypes tabType)
	{
		((XUiV_Button)this.ingredientsButton.ViewComponent).Selected = tabType == XUiC_CraftingInfoWindow.TabTypes.Ingredients;
		((XUiV_Button)this.descriptionButton.ViewComponent).Selected = tabType == XUiC_CraftingInfoWindow.TabTypes.Description;
		((XUiV_Button)this.unlockedByButton.ViewComponent).Selected = tabType == XUiC_CraftingInfoWindow.TabTypes.UnlockedBy;
	}

	// Token: 0x060078B9 RID: 30905 RVA: 0x002D8A6C File Offset: 0x002D6C6C
	[PublicizedFrom(EAccessModifier.Private)]
	public void IngredientsButton_OnPress(XUiController _sender, int _mouseButton)
	{
		this.TabType = XUiC_CraftingInfoWindow.TabTypes.Ingredients;
		this.SetSelectedButtonByType(this.TabType);
		this.IsDirty = true;
	}

	// Token: 0x060078BA RID: 30906 RVA: 0x002D8A88 File Offset: 0x002D6C88
	[PublicizedFrom(EAccessModifier.Private)]
	public void DescriptionButton_OnPress(XUiController _sender, int _mouseButton)
	{
		this.TabType = XUiC_CraftingInfoWindow.TabTypes.Description;
		this.SetSelectedButtonByType(this.TabType);
		this.IsDirty = true;
	}

	// Token: 0x060078BB RID: 30907 RVA: 0x002D8AA4 File Offset: 0x002D6CA4
	[PublicizedFrom(EAccessModifier.Private)]
	public void UnlockedByButton_OnPress(XUiController _sender, int _mouseButton)
	{
		this.TabType = XUiC_CraftingInfoWindow.TabTypes.UnlockedBy;
		this.SetSelectedButtonByType(this.TabType);
		this.IsDirty = true;
	}

	// Token: 0x060078BC RID: 30908 RVA: 0x002D8AC0 File Offset: 0x002D6CC0
	public override void OnOpen()
	{
		base.OnOpen();
		if (this.xui.PlayerInventory != null)
		{
			this.xui.PlayerInventory.OnBackpackItemsChanged += this.PlayerInventory_OnItemsChanged;
			this.xui.PlayerInventory.OnToolbeltItemsChanged += this.PlayerInventory_OnItemsChanged;
		}
	}

	// Token: 0x060078BD RID: 30909 RVA: 0x002D8B18 File Offset: 0x002D6D18
	public override void OnClose()
	{
		base.OnClose();
		if (this.xui.PlayerInventory != null)
		{
			this.xui.PlayerInventory.OnBackpackItemsChanged -= this.PlayerInventory_OnItemsChanged;
			this.xui.PlayerInventory.OnToolbeltItemsChanged -= this.PlayerInventory_OnItemsChanged;
		}
	}

	// Token: 0x060078BE RID: 30910 RVA: 0x0008A715 File Offset: 0x00088915
	[PublicizedFrom(EAccessModifier.Private)]
	public void PlayerInventory_OnItemsChanged()
	{
		this.IsDirty = true;
	}

	// Token: 0x060078BF RID: 30911 RVA: 0x002D8B70 File Offset: 0x002D6D70
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleOnCountChanged(XUiController _sender, OnCountChangedEventArgs _e)
	{
		this.craftCount = _e.Count;
		this.IsDirty = true;
	}

	// Token: 0x060078C0 RID: 30912 RVA: 0x002D8B85 File Offset: 0x002D6D85
	public override void Deselect()
	{
		if (this.selectedEntry != null)
		{
			this.selectedEntry.IsSelected = false;
		}
	}

	// Token: 0x060078C1 RID: 30913 RVA: 0x002D8B9C File Offset: 0x002D6D9C
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

	// Token: 0x060078C2 RID: 30914 RVA: 0x002D8C04 File Offset: 0x002D6E04
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

	// Token: 0x060078C3 RID: 30915 RVA: 0x002D8F7C File Offset: 0x002D717C
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

	// Token: 0x060078C4 RID: 30916 RVA: 0x002D976C File Offset: 0x002D796C
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

	// Token: 0x060078C5 RID: 30917 RVA: 0x002D980A File Offset: 0x002D7A0A
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleRecipeChanged(Recipe _recipe, XUiC_RecipeEntry _recipeEntry)
	{
		if (base.WindowGroup.isShowing)
		{
			base.ViewComponent.IsVisible = true;
		}
		this.SetRecipe(_recipeEntry);
	}

	// Token: 0x060078C6 RID: 30918 RVA: 0x002D982C File Offset: 0x002D7A2C
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

	// Token: 0x060078C7 RID: 30919 RVA: 0x002D98AA File Offset: 0x002D7AAA
	public void RefreshRecipe()
	{
		if (this.recipeCraftCount != null)
		{
			this.recipeCraftCount.CalculateMaxCount();
		}
	}

	// Token: 0x04005936 RID: 22838
	[PublicizedFrom(EAccessModifier.Private)]
	public Recipe recipe;

	// Token: 0x04005937 RID: 22839
	[PublicizedFrom(EAccessModifier.Private)]
	public int craftCount;

	// Token: 0x04005938 RID: 22840
	[PublicizedFrom(EAccessModifier.Private)]
	public int selectedCraftingTier = 1;

	// Token: 0x04005939 RID: 22841
	[PublicizedFrom(EAccessModifier.Private)]
	public int selectedMaxCraftingTier = 1;

	// Token: 0x0400593A RID: 22842
	[PublicizedFrom(EAccessModifier.Private)]
	public Recipe lastRecipeSelected;

	// Token: 0x0400593B RID: 22843
	public XUiC_CraftingInfoWindow.TabTypes TabType;

	// Token: 0x0400593C RID: 22844
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_RecipeEntry selectedEntry;

	// Token: 0x0400593D RID: 22845
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController itemPreview;

	// Token: 0x0400593E RID: 22846
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController windowName;

	// Token: 0x0400593F RID: 22847
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController windowIcon;

	// Token: 0x04005940 RID: 22848
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController description;

	// Token: 0x04005941 RID: 22849
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController craftingTime;

	// Token: 0x04005942 RID: 22850
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController requiredToolOverlay;

	// Token: 0x04005943 RID: 22851
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController requiredToolCheckmark;

	// Token: 0x04005944 RID: 22852
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController requiredToolText;

	// Token: 0x04005945 RID: 22853
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_IngredientList ingredientList;

	// Token: 0x04005946 RID: 22854
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_UnlockByList unlockByList;

	// Token: 0x04005947 RID: 22855
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_CategoryList categoryList;

	// Token: 0x04005948 RID: 22856
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ItemActionList actionItemList;

	// Token: 0x04005949 RID: 22857
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_RecipeList recipeList;

	// Token: 0x0400594A RID: 22858
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_InfoWindow emptyInfoWindow;

	// Token: 0x0400594B RID: 22859
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ItemInfoWindow itemInfoWindow;

	// Token: 0x0400594C RID: 22860
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_RecipeCraftCount recipeCraftCount;

	// Token: 0x0400594D RID: 22861
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController ingredientsButton;

	// Token: 0x0400594E RID: 22862
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController descriptionButton;

	// Token: 0x0400594F RID: 22863
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController unlockedByButton;

	// Token: 0x04005950 RID: 22864
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController addQualityButton;

	// Token: 0x04005951 RID: 22865
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController subtractQualityButton;

	// Token: 0x04005952 RID: 22866
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterXuiRgbaColor itemicontintcolorFormatter = new CachedStringFormatterXuiRgbaColor();

	// Token: 0x04005953 RID: 22867
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<float> craftingtimeFormatter = new CachedStringFormatter<float>((float _time) => string.Format("{0:00}:{1:00}", (int)(_time / 60f), (int)(_time % 60f)));

	// Token: 0x04005954 RID: 22868
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterXuiRgbaColor durabilitycolorFormatter = new CachedStringFormatterXuiRgbaColor();

	// Token: 0x04005955 RID: 22869
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterFloat durabilityfillFormatter = new CachedStringFormatterFloat(null);

	// Token: 0x04005956 RID: 22870
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterInt durabilitytextFormatter = new CachedStringFormatterInt();

	// Token: 0x04005957 RID: 22871
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterXuiRgbaColor altitemtypeiconcolorFormatter = new CachedStringFormatterXuiRgbaColor();

	// Token: 0x04005958 RID: 22872
	[PublicizedFrom(EAccessModifier.Private)]
	public Color validColor;

	// Token: 0x04005959 RID: 22873
	[PublicizedFrom(EAccessModifier.Private)]
	public Color invalidColor;

	// Token: 0x0400595A RID: 22874
	[PublicizedFrom(EAccessModifier.Private)]
	public string validSprite;

	// Token: 0x0400595B RID: 22875
	[PublicizedFrom(EAccessModifier.Private)]
	public string invalidSprite;

	// Token: 0x02000F09 RID: 3849
	public enum TabTypes
	{
		// Token: 0x0400595D RID: 22877
		Ingredients,
		// Token: 0x0400595E RID: 22878
		Description,
		// Token: 0x0400595F RID: 22879
		UnlockedBy
	}
}
