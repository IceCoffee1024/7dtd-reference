using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02001019 RID: 4121
[Preserve]
public class XUiC_RecipeEntry : XUiC_SelectableEntry
{
	// Token: 0x17000F3D RID: 3901
	// (get) Token: 0x060081AA RID: 33194 RVA: 0x00315EF8 File Offset: 0x003140F8
	// (set) Token: 0x060081AB RID: 33195 RVA: 0x00315F00 File Offset: 0x00314100
	public bool HasIngredients
	{
		get
		{
			return this.hasIngredients;
		}
		set
		{
			this.hasIngredients = value;
			base.RefreshBindings();
		}
	}

	// Token: 0x17000F3E RID: 3902
	// (get) Token: 0x060081AC RID: 33196 RVA: 0x00315F0F File Offset: 0x0031410F
	// (set) Token: 0x060081AD RID: 33197 RVA: 0x00315F17 File Offset: 0x00314117
	public bool IsCurrentWorkstation
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000F3F RID: 3903
	// (get) Token: 0x060081AE RID: 33198 RVA: 0x00315F20 File Offset: 0x00314120
	// (set) Token: 0x060081AF RID: 33199 RVA: 0x00315F28 File Offset: 0x00314128
	public Recipe Recipe
	{
		get
		{
			return this.recipe;
		}
		set
		{
			this.recipe = value;
			this.IsCurrentWorkstation = false;
			if (this.recipe != null)
			{
				for (int i = 0; i < this.RecipeList.craftingArea.Length; i++)
				{
					if (this.RecipeList.craftingArea[i] == this.recipe.craftingArea)
					{
						this.IsCurrentWorkstation = true;
						break;
					}
				}
			}
			if (!base.IsSelected)
			{
				this.background.Color = new Color32(64, 64, 64, byte.MaxValue);
			}
			base.ViewComponent.IsNavigatable = (base.ViewComponent.IsSnappable = value != null);
		}
	}

	// Token: 0x060081B0 RID: 33200 RVA: 0x00315FD0 File Offset: 0x003141D0
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void SelectedChanged(bool isSelected)
	{
		if (this.background != null)
		{
			this.background.Color = (isSelected ? new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue) : new Color32(64, 64, 64, byte.MaxValue));
			this.background.SpriteName = (isSelected ? "ui_game_select_row" : "menu_empty");
		}
	}

	// Token: 0x060081B1 RID: 33201 RVA: 0x0031603D File Offset: 0x0031423D
	public void SetRecipeAndHasIngredients(Recipe recipe, bool hasIngredients)
	{
		this.Recipe = recipe;
		this.hasIngredients = hasIngredients;
		this.IsDirty = true;
		base.RefreshBindings();
		if (recipe == null)
		{
			this.background.Color = new Color32(64, 64, 64, byte.MaxValue);
		}
	}

	// Token: 0x060081B2 RID: 33202 RVA: 0x00316080 File Offset: 0x00314280
	public override void Init()
	{
		base.Init();
		for (int i = 0; i < this.children.Count; i++)
		{
			XUiView viewComponent = this.children[i].ViewComponent;
			if (viewComponent.ID.EqualsCaseInsensitive("name"))
			{
				this.lblName = viewComponent as XUiV_Label;
			}
			else if (viewComponent.ID.EqualsCaseInsensitive("icon"))
			{
				this.icoRecipe = viewComponent as XUiV_Sprite;
			}
			else if (viewComponent.ID.EqualsCaseInsensitive("favorite"))
			{
				this.icoFavorite = viewComponent as XUiV_Sprite;
			}
			else if (viewComponent.ID.EqualsCaseInsensitive("unlocked"))
			{
				this.icoBook = viewComponent as XUiV_Sprite;
			}
			else if (viewComponent.ID.EqualsCaseInsensitive("background"))
			{
				this.background = viewComponent as XUiV_Sprite;
			}
		}
		this.IsDirty = true;
	}

	// Token: 0x060081B3 RID: 33203 RVA: 0x00316168 File Offset: 0x00314368
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnHovered(bool _isOver)
	{
		this.isHovered = _isOver;
		if (this.background != null && this.recipe != null && !base.IsSelected)
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

	// Token: 0x060081B4 RID: 33204 RVA: 0x003161E0 File Offset: 0x003143E0
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string value, string bindingName)
	{
		uint num = <PrivateImplementationDetails>.ComputeStringHash(bindingName);
		if (num <= 1820482109U)
		{
			if (num <= 847165955U)
			{
				if (num != 98823489U)
				{
					if (num != 112674252U)
					{
						if (num == 847165955U)
						{
							if (bindingName == "itemtypeicon")
							{
								value = "";
								if (this.recipe != null)
								{
									ItemClass forId = ItemClass.GetForId(this.recipe.itemValueType);
									if (forId != null)
									{
										if (forId.IsBlock())
										{
											value = forId.GetBlock().ItemTypeIcon;
										}
										else
										{
											if (forId.AltItemTypeIcon != null && forId.Unlocks != "" && XUiM_ItemStack.CheckKnown(this.xui.playerUI.entityPlayer, forId, null))
											{
												value = forId.AltItemTypeIcon;
												return true;
											}
											value = forId.ItemTypeIcon;
										}
									}
								}
								return true;
							}
						}
					}
					else if (bindingName == "recipename")
					{
						value = ((this.recipe != null) ? Localization.Get(this.recipe.GetName(), false, null) : "");
						return true;
					}
				}
				else if (bindingName == "hasingredientsstatecolor")
				{
					if (this.recipe != null)
					{
						Color32 color = new Color32(148, 148, 148, byte.MaxValue);
						if (this.HasIngredients)
						{
							if (this.CustomAttributes.ContainsKey("enabled_font_color"))
							{
								color = StringParsers.ParseColor32(this.CustomAttributes["enabled_font_color"].ToString());
							}
							else
							{
								color = Color.white;
							}
						}
						else if (this.CustomAttributes.ContainsKey("disabled_font_color"))
						{
							color = StringParsers.ParseColor32(this.CustomAttributes["disabled_font_color"].ToString());
						}
						value = this.hasingredientsstatecolorFormatter.Format(color);
					}
					else
					{
						value = "255,255,255,255";
					}
					return true;
				}
			}
			else if (num != 1236476975U)
			{
				if (num != 1388578781U)
				{
					if (num == 1820482109U)
					{
						if (bindingName == "isfavorite")
						{
							value = ((this.recipe != null) ? XUiM_Recipes.GetRecipeIsFavorite(this.xui, this.recipe).ToString() : "false");
							return true;
						}
					}
				}
				else if (bindingName == "hasitemtypeicon")
				{
					value = "false";
					if (this.recipe != null)
					{
						ItemClass forId2 = ItemClass.GetForId(this.recipe.itemValueType);
						if (forId2 != null)
						{
							if (forId2.IsBlock())
							{
								value = (forId2.GetBlock().ItemTypeIcon != "").ToString();
							}
							else
							{
								value = (forId2.ItemTypeIcon != "").ToString();
							}
						}
					}
					return true;
				}
			}
			else if (bindingName == "recipeicontint")
			{
				Color32 color2 = Color.white;
				if (this.recipe != null)
				{
					ItemClass forId3 = ItemClass.GetForId(this.recipe.itemValueType);
					if (forId3 != null)
					{
						color2 = forId3.GetIconTint(null);
					}
				}
				value = this.recipeicontintcolorFormatter.Format(color2);
				return true;
			}
		}
		else if (num <= 2172950741U)
		{
			if (num != 2017922685U)
			{
				if (num != 2154375741U)
				{
					if (num == 2172950741U)
					{
						if (bindingName == "isunlockable")
						{
							value = ((this.recipe != null) ? (!this.IsCurrentWorkstation || XUiM_Recipes.GetRecipeIsUnlockable(this.xui, this.recipe) || this.recipe.isQuest || this.recipe.isChallenge || this.recipe.IsTracked).ToString() : "false");
							return true;
						}
					}
				}
				else if (bindingName == "hasrecipe")
				{
					value = (this.recipe != null).ToString();
					return true;
				}
			}
			else if (bindingName == "hasingredients")
			{
				value = ((this.recipe != null) ? this.HasIngredients.ToString() : "false");
				return true;
			}
		}
		else if (num <= 3435046057U)
		{
			if (num != 3008660032U)
			{
				if (num == 3435046057U)
				{
					if (bindingName == "unlockstatecolor")
					{
						if (this.recipe != null)
						{
							if (this.recipe.isQuest || this.recipe.isChallenge)
							{
								value = this.unlockstatecolorFormatter.Format(Color.yellow);
							}
							else
							{
								Color32 color3 = (XUiM_Recipes.GetRecipeIsUnlocked(this.xui, this.recipe) ? Color.white : Color.gray);
								value = this.unlockstatecolorFormatter.Format(color3);
							}
						}
						else
						{
							value = "255,255,255,255";
						}
						return true;
					}
				}
			}
			else if (bindingName == "unlockicon")
			{
				if (this.recipe != null)
				{
					if (this.recipe.isChallenge)
					{
						value = "ui_game_symbol_challenge";
						return true;
					}
					if (this.recipe.isQuest)
					{
						value = "ui_game_symbol_quest";
						return true;
					}
					if (this.recipe.IsTracked)
					{
						value = "ui_game_symbol_compass";
						return true;
					}
					if (XUiM_Recipes.GetRecipeIsUnlockable(this.xui, this.recipe) && !XUiM_Recipes.GetRecipeIsUnlocked(this.xui, this.recipe))
					{
						value = "ui_game_symbol_lock";
					}
					else if (!this.IsCurrentWorkstation)
					{
						WorkstationData workstationData = CraftingManager.GetWorkstationData(this.recipe.craftingArea);
						if (workstationData != null)
						{
							value = workstationData.WorkstationIcon;
						}
						else
						{
							value = "ui_game_symbol_hammer";
						}
					}
					else
					{
						value = "";
					}
				}
				else
				{
					value = "ui_game_symbol_book";
				}
				return true;
			}
		}
		else if (num != 3974800894U)
		{
			if (num == 4049247086U)
			{
				if (bindingName == "itemtypeicontint")
				{
					value = "255,255,255,255";
					if (this.recipe != null)
					{
						ItemClass forId4 = ItemClass.GetForId(this.recipe.itemValueType);
						if (forId4 != null && forId4.Unlocks != "" && XUiM_ItemStack.CheckKnown(this.xui.playerUI.entityPlayer, forId4, null))
						{
							value = this.altitemtypeiconcolorFormatter.Format(forId4.AltItemTypeIconColor);
						}
					}
					return true;
				}
			}
		}
		else if (bindingName == "recipeicon")
		{
			value = ((this.recipe != null) ? this.recipe.GetIcon() : "");
			return true;
		}
		return base.GetBindingValueInternal(ref value, bindingName);
	}

	// Token: 0x060081B5 RID: 33205 RVA: 0x002E0E5B File Offset: 0x002DF05B
	public void Refresh()
	{
		this.IsDirty = true;
		base.RefreshBindings();
	}

	// Token: 0x04006034 RID: 24628
	[PublicizedFrom(EAccessModifier.Private)]
	public Recipe recipe;

	// Token: 0x04006035 RID: 24629
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isHovered;

	// Token: 0x04006036 RID: 24630
	[PublicizedFrom(EAccessModifier.Private)]
	public bool hasIngredients;

	// Token: 0x04006037 RID: 24631
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label lblName;

	// Token: 0x04006038 RID: 24632
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite icoRecipe;

	// Token: 0x04006039 RID: 24633
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite icoFavorite;

	// Token: 0x0400603A RID: 24634
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite icoBook;

	// Token: 0x0400603B RID: 24635
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite background;

	// Token: 0x0400603C RID: 24636
	public XUiC_RecipeList RecipeList;

	// Token: 0x0400603E RID: 24638
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterXuiRgbaColor recipeicontintcolorFormatter = new CachedStringFormatterXuiRgbaColor();

	// Token: 0x0400603F RID: 24639
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterXuiRgbaColor hasingredientsstatecolorFormatter = new CachedStringFormatterXuiRgbaColor();

	// Token: 0x04006040 RID: 24640
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterXuiRgbaColor unlockstatecolorFormatter = new CachedStringFormatterXuiRgbaColor();

	// Token: 0x04006041 RID: 24641
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterXuiRgbaColor altitemtypeiconcolorFormatter = new CachedStringFormatterXuiRgbaColor();
}
