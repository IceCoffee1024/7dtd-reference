using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000F26 RID: 3878
[Preserve]
public class XUiC_CraftingInfoWindow : XUiC_InfoWindow
{
	// Token: 0x17000E47 RID: 3655
	// (get) Token: 0x060078F2 RID: 30962 RVA: 0x002DD444 File Offset: 0x002DB644
	public int SelectedCraftingTier
	{
		get
		{
			return this.selectedCraftingTier;
		}
	}

	// Token: 0x060078F3 RID: 30963 RVA: 0x002DD44C File Offset: 0x002DB64C
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

	// Token: 0x060078F4 RID: 30964 RVA: 0x002DD653 File Offset: 0x002DB853
	[PublicizedFrom(EAccessModifier.Private)]
	public void SubtractQualityButton_OnPress(XUiController _sender, int _mouseButton)
	{
		if (this.selectedCraftingTier > 1)
		{
			this.selectedCraftingTier--;
			this.IsDirty = true;
		}
	}

	// Token: 0x060078F5 RID: 30965 RVA: 0x002DD673 File Offset: 0x002DB873
	[PublicizedFrom(EAccessModifier.Private)]
	public void AddQualityButton_OnPress(XUiController _sender, int _mouseButton)
	{
		if (this.selectedCraftingTier < XUiM_Recipes.CraftingMaxTier)
		{
			this.selectedCraftingTier++;
			this.IsDirty = true;
		}
	}

	// Token: 0x060078F6 RID: 30966 RVA: 0x002DD698 File Offset: 0x002DB898
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetSelectedButtonByType(XUiC_CraftingInfoWindow.TabTypes tabType)
	{
		((XUiV_Button)this.ingredientsButton.ViewComponent).Selected = tabType == XUiC_CraftingInfoWindow.TabTypes.Ingredients;
		((XUiV_Button)this.descriptionButton.ViewComponent).Selected = tabType == XUiC_CraftingInfoWindow.TabTypes.Description;
		((XUiV_Button)this.unlockedByButton.ViewComponent).Selected = tabType == XUiC_CraftingInfoWindow.TabTypes.UnlockedBy;
	}

	// Token: 0x060078F7 RID: 30967 RVA: 0x002DD6F0 File Offset: 0x002DB8F0
	[PublicizedFrom(EAccessModifier.Private)]
	public void IngredientsButton_OnPress(XUiController _sender, int _mouseButton)
	{
		this.TabType = XUiC_CraftingInfoWindow.TabTypes.Ingredients;
		this.SetSelectedButtonByType(this.TabType);
		this.IsDirty = true;
	}

	// Token: 0x060078F8 RID: 30968 RVA: 0x002DD70C File Offset: 0x002DB90C
	[PublicizedFrom(EAccessModifier.Private)]
	public void DescriptionButton_OnPress(XUiController _sender, int _mouseButton)
	{
		this.TabType = XUiC_CraftingInfoWindow.TabTypes.Description;
		this.SetSelectedButtonByType(this.TabType);
		this.IsDirty = true;
	}

	// Token: 0x060078F9 RID: 30969 RVA: 0x002DD728 File Offset: 0x002DB928
	[PublicizedFrom(EAccessModifier.Private)]
	public void UnlockedByButton_OnPress(XUiController _sender, int _mouseButton)
	{
		this.TabType = XUiC_CraftingInfoWindow.TabTypes.UnlockedBy;
		this.SetSelectedButtonByType(this.TabType);
		this.IsDirty = true;
	}

	// Token: 0x060078FA RID: 30970 RVA: 0x002DD744 File Offset: 0x002DB944
	public override void OnOpen()
	{
		base.OnOpen();
		if (this.xui.PlayerInventory != null)
		{
			this.xui.PlayerInventory.OnBackpackItemsChanged += this.PlayerInventory_OnItemsChanged;
			this.xui.PlayerInventory.OnToolbeltItemsChanged += this.PlayerInventory_OnItemsChanged;
		}
	}

	// Token: 0x060078FB RID: 30971 RVA: 0x002DD79C File Offset: 0x002DB99C
	public override void OnClose()
	{
		base.OnClose();
		if (this.xui.PlayerInventory != null)
		{
			this.xui.PlayerInventory.OnBackpackItemsChanged -= this.PlayerInventory_OnItemsChanged;
			this.xui.PlayerInventory.OnToolbeltItemsChanged -= this.PlayerInventory_OnItemsChanged;
		}
	}

	// Token: 0x060078FC RID: 30972 RVA: 0x0008ABC9 File Offset: 0x00088DC9
	[PublicizedFrom(EAccessModifier.Private)]
	public void PlayerInventory_OnItemsChanged()
	{
		this.IsDirty = true;
	}

	// Token: 0x060078FD RID: 30973 RVA: 0x002DD7F4 File Offset: 0x002DB9F4
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleOnCountChanged(XUiController _sender, OnCountChangedEventArgs _e)
	{
		this.craftCount = _e.Count;
		this.IsDirty = true;
	}

	// Token: 0x060078FE RID: 30974 RVA: 0x002DD809 File Offset: 0x002DBA09
	public override void Deselect()
	{
		if (this.selectedEntry != null)
		{
			this.selectedEntry.IsSelected = false;
		}
	}

	// Token: 0x060078FF RID: 30975 RVA: 0x002DD820 File Offset: 0x002DBA20
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

	// Token: 0x06007900 RID: 30976 RVA: 0x002DD888 File Offset: 0x002DBA88
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

	// Token: 0x06007901 RID: 30977 RVA: 0x002DDC00 File Offset: 0x002DBE00
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

	// Token: 0x06007902 RID: 30978 RVA: 0x002DE3F0 File Offset: 0x002DC5F0
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

	// Token: 0x06007903 RID: 30979 RVA: 0x002DE48E File Offset: 0x002DC68E
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleRecipeChanged(Recipe _recipe, XUiC_RecipeEntry _recipeEntry)
	{
		if (base.WindowGroup.isShowing)
		{
			base.ViewComponent.IsVisible = true;
		}
		this.SetRecipe(_recipeEntry);
	}

	// Token: 0x06007904 RID: 30980 RVA: 0x002DE4B0 File Offset: 0x002DC6B0
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

	// Token: 0x06007905 RID: 30981 RVA: 0x002DE52E File Offset: 0x002DC72E
	public void RefreshRecipe()
	{
		if (this.recipeCraftCount != null)
		{
			this.recipeCraftCount.CalculateMaxCount();
		}
	}

	// Token: 0x040059A1 RID: 22945
	[PublicizedFrom(EAccessModifier.Private)]
	public Recipe recipe;

	// Token: 0x040059A2 RID: 22946
	[PublicizedFrom(EAccessModifier.Private)]
	public int craftCount;

	// Token: 0x040059A3 RID: 22947
	[PublicizedFrom(EAccessModifier.Private)]
	public int selectedCraftingTier = 1;

	// Token: 0x040059A4 RID: 22948
	[PublicizedFrom(EAccessModifier.Private)]
	public int selectedMaxCraftingTier = 1;

	// Token: 0x040059A5 RID: 22949
	[PublicizedFrom(EAccessModifier.Private)]
	public Recipe lastRecipeSelected;

	// Token: 0x040059A6 RID: 22950
	public XUiC_CraftingInfoWindow.TabTypes TabType;

	// Token: 0x040059A7 RID: 22951
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_RecipeEntry selectedEntry;

	// Token: 0x040059A8 RID: 22952
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController itemPreview;

	// Token: 0x040059A9 RID: 22953
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController windowName;

	// Token: 0x040059AA RID: 22954
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController windowIcon;

	// Token: 0x040059AB RID: 22955
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController description;

	// Token: 0x040059AC RID: 22956
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController craftingTime;

	// Token: 0x040059AD RID: 22957
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController requiredToolOverlay;

	// Token: 0x040059AE RID: 22958
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController requiredToolCheckmark;

	// Token: 0x040059AF RID: 22959
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController requiredToolText;

	// Token: 0x040059B0 RID: 22960
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_IngredientList ingredientList;

	// Token: 0x040059B1 RID: 22961
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_UnlockByList unlockByList;

	// Token: 0x040059B2 RID: 22962
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_CategoryList categoryList;

	// Token: 0x040059B3 RID: 22963
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ItemActionList actionItemList;

	// Token: 0x040059B4 RID: 22964
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_RecipeList recipeList;

	// Token: 0x040059B5 RID: 22965
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_InfoWindow emptyInfoWindow;

	// Token: 0x040059B6 RID: 22966
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ItemInfoWindow itemInfoWindow;

	// Token: 0x040059B7 RID: 22967
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_RecipeCraftCount recipeCraftCount;

	// Token: 0x040059B8 RID: 22968
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController ingredientsButton;

	// Token: 0x040059B9 RID: 22969
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController descriptionButton;

	// Token: 0x040059BA RID: 22970
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController unlockedByButton;

	// Token: 0x040059BB RID: 22971
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController addQualityButton;

	// Token: 0x040059BC RID: 22972
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController subtractQualityButton;

	// Token: 0x040059BD RID: 22973
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterXuiRgbaColor itemicontintcolorFormatter = new CachedStringFormatterXuiRgbaColor();

	// Token: 0x040059BE RID: 22974
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<float> craftingtimeFormatter = new CachedStringFormatter<float>((float _time) => string.Format("{0:00}:{1:00}", (int)(_time / 60f), (int)(_time % 60f)));

	// Token: 0x040059BF RID: 22975
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterXuiRgbaColor durabilitycolorFormatter = new CachedStringFormatterXuiRgbaColor();

	// Token: 0x040059C0 RID: 22976
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterFloat durabilityfillFormatter = new CachedStringFormatterFloat(null);

	// Token: 0x040059C1 RID: 22977
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterInt durabilitytextFormatter = new CachedStringFormatterInt();

	// Token: 0x040059C2 RID: 22978
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterXuiRgbaColor altitemtypeiconcolorFormatter = new CachedStringFormatterXuiRgbaColor();

	// Token: 0x040059C3 RID: 22979
	[PublicizedFrom(EAccessModifier.Private)]
	public Color validColor;

	// Token: 0x040059C4 RID: 22980
	[PublicizedFrom(EAccessModifier.Private)]
	public Color invalidColor;

	// Token: 0x040059C5 RID: 22981
	[PublicizedFrom(EAccessModifier.Private)]
	public string validSprite;

	// Token: 0x040059C6 RID: 22982
	[PublicizedFrom(EAccessModifier.Private)]
	public string invalidSprite;

	// Token: 0x02000F27 RID: 3879
	public enum TabTypes
	{
		// Token: 0x040059C8 RID: 22984
		Ingredients,
		// Token: 0x040059C9 RID: 22985
		Description,
		// Token: 0x040059CA RID: 22986
		UnlockedBy
	}
}
